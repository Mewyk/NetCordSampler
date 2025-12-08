using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;

namespace NetCordSampler.MetadataExtractor;

/// <summary>
/// Scans NetCord assemblies and generates SamplerTypes.json with metadata for all Properties types.
/// </summary>
class Program
{
    private static XDocument? xmlDoc;
    private static SampleDefaultsConfig? defaultsConfig;

    static void Main(string[] args)
    {
        Console.WriteLine("NetCord Metadata Extractor");
        Console.WriteLine("==========================\n");

        var solutionDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        var outputPath = args.Length > 0 
            ? args[0] 
            : Path.Combine(solutionDir, "NetCordSampler", "SamplerTypes.json");

        Console.WriteLine($"Output: {outputPath}\n");

        LoadConfiguration(solutionDir);
        LoadXmlDocumentation();

        Console.WriteLine("Regenerating metadata from scratch...\n");

        var netcordAssembly = typeof(NetCord.Rest.EmbedProperties).Assembly;
        Console.WriteLine($"Scanning assembly: {netcordAssembly.GetName().Name}\n");

        var allTypes = ExtractTypes(netcordAssembly);
        
        SaveTypes(outputPath, allTypes);

        Console.WriteLine($"\n✓ Complete! Generated {allTypes.Count} types");
    }

    static void LoadConfiguration(string solutionDir)
    {
        try
        {
            var appSettingsPath = Path.Combine(solutionDir, "NetCordSampler", "appsettings.json");
            
            if (!File.Exists(appSettingsPath))
            {
                Console.WriteLine($"Warning: appsettings.json not found at {appSettingsPath}. Using built-in defaults.\n");
                return;
            }

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(appSettingsPath, optional: true)
                .Build();

            defaultsConfig = new SampleDefaultsConfig();
            configuration.GetSection("SampleDefaults").Bind(defaultsConfig);

            Console.WriteLine($"Loaded configuration with {defaultsConfig.PatternDefaults.Count} pattern defaults\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Failed to load configuration: {ex.Message}\n");
        }
    }

    static void LoadXmlDocumentation()
    {
        var solutionDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        var xmlPath = Path.Combine(solutionDir, "External", "NetCord", "NetCord", "bin", "Release", "net10.0", "NetCord.xml");

        if (File.Exists(xmlPath))
        {
            try
            {
                xmlDoc = XDocument.Load(xmlPath);
                Console.WriteLine($"Loaded XML documentation: {xmlPath}\n");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to load XML from {xmlPath}: {ex.Message}");
            }
        }

        Console.WriteLine("Warning: NetCord XML documentation not found. Descriptions will be generic.\n");
    }

    static string GetXmlTypeMemberName(Type type)
        => $"T:{type.FullName?.Replace('+', '.')}";

    static string GetXmlPropertyMemberName(PropertyInfo property)
        => $"P:{property.DeclaringType?.FullName?.Replace('+', '.')}.{property.Name}";

    static string GetXmlConstructorMemberName(ConstructorInfo constructor)
    {
        var typeName = constructor.DeclaringType?.FullName?.Replace('+', '.');
        var parameters = constructor.GetParameters();
        
        if (parameters.Length == 0)
            return $"M:{typeName}.#ctor";

        var paramTypes = string.Join(",", parameters.Select(p => 
            p.ParameterType.FullName?.Replace('+', '.') ?? p.ParameterType.Name));
        return $"M:{typeName}.#ctor({paramTypes})";
    }

    static string? GetXmlSummary(string memberName)
    {
        if (xmlDoc == null)
            return null;

        var member = xmlDoc.Descendants("member")
            .FirstOrDefault(m => m.Attribute("name")?.Value == memberName);

        if (member == null)
            return null;

        var summary = member.Element("summary");
        if (summary == null)
            return null;

        var text = summary.Value.Trim();
        return System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ");
    }

    static string? GetXmlParamDescription(string memberName, string paramName)
    {
        if (xmlDoc == null)
            return null;

        var member = xmlDoc.Descendants("member")
            .FirstOrDefault(m => m.Attribute("name")?.Value == memberName);

        if (member == null)
            return null;

        var param = member.Elements("param")
            .FirstOrDefault(p => p.Attribute("name")?.Value == paramName);

        if (param == null)
            return null;

        var text = param.Value.Trim();
        return System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ");
    }

    static string? GetXmlRemarks(string memberName)
    {
        if (xmlDoc == null)
            return null;

        var member = xmlDoc.Descendants("member")
            .FirstOrDefault(m => m.Attribute("name")?.Value == memberName);

        if (member == null)
            return null;

        var remarks = member.Element("remarks");
        if (remarks == null)
            return null;

        var text = remarks.Value.Trim();
        return System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ");
    }

    static List<TypeMetadata> ExtractTypes(Assembly assembly)
    {
        var types = new List<TypeMetadata>();

        var typesList = assembly.GetTypes()
            .Where(t => t.Namespace?.StartsWith("NetCord.Rest") == true
                     && t.Name.EndsWith("Properties")
                     && (t.IsClass || t.IsValueType)
                     && !t.IsAbstract
                     && t.IsPublic
                     && (t.IsValueType || t.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length > 0))
            .OrderBy(t => t.FullName);

        foreach (var type in typesList)
        {
            var fullTypeName = type.FullName ?? type.Name;
            
            Console.WriteLine($"  Extract: {fullTypeName}");

            var metadata = new TypeMetadata
            {
                FullType = fullTypeName,
                Description = $"Represents properties for {type.Name.Replace("Properties", "")}.",
                SourceSummary = ExtractXmlSummary(type),
                Category = GetCategory(type.Namespace ?? ""),
                ConstructorParams = ExtractConstructorParams(type),
                Properties = ExtractProperties(type),
                Limitations = []
            };

            types.Add(metadata);
        }

        return types;
    }

    static string GetCategory(string namespaceName)
    {
        var parts = namespaceName.Split('.');
        return parts.Length > 1 ? parts[^1] : "Other";
    }

    static string ExtractXmlSummary(Type type)
    {
        var memberName = GetXmlTypeMemberName(type);
        var summary = GetXmlSummary(memberName);
        return summary ?? $"Represents properties for {type.Name.Replace("Properties", "")}.";
    }

    static List<PropertyMetadata> ExtractProperties(Type type)
    {
        var properties = new List<PropertyMetadata>();

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (prop.DeclaringType != type)
                continue;

            var memberName = GetXmlPropertyMemberName(prop);
            var xmlSummary = GetXmlSummary(memberName);
            var xmlRemarks = GetXmlRemarks(memberName);
            var description = xmlSummary ?? $"Gets or sets the {prop.Name}.";
            var maxLength = ExtractMaxLengthFromRemarks(xmlRemarks) ?? GuessMaxLength(prop.Name);

            var propMeta = new PropertyMetadata
            {
                Name = prop.Name,
                Type = GetFriendlyTypeName(prop.PropertyType),
                Description = description,
                SourceSummary = xmlSummary ?? description,
                ConfigKey = GuessConfigKey(prop),
                DefaultValue = GuessDefaultValue(prop.PropertyType, prop.Name),
                NestedSample = IsNestedSample(prop.PropertyType),
                CollectionOf = GetCollectionElementType(prop.PropertyType),
                MaxLength = maxLength
            };

            properties.Add(propMeta);
        }

        return properties;
    }

    static int? ExtractMaxLengthFromRemarks(string? remarks)
    {
        if (string.IsNullOrWhiteSpace(remarks))
            return null;

        var patterns = new[]
        {
            @"max(?:imum)?\s+(\d+)\s+character",
            @"(\d+)\s+character(?:s)?\s+max",
            @"max(?:imum)?\s+length\s+(?:of\s+)?(\d+)",
            @"length\s+(?:must\s+be\s+)?(?:at\s+most\s+)?(\d+)"
        };

        foreach (var pattern in patterns)
        {
            var match = System.Text.RegularExpressions.Regex.Match(remarks, pattern, 
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            
            if (match.Success && int.TryParse(match.Groups[1].Value, out var length))
                return length;
        }

        return null;
    }

    static List<ConstructorParam> ExtractConstructorParams(Type type)
    {
        var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        if (ctors.Length == 0)
            return [];

        var mainCtor = ctors
            .Where(c => !c.GetParameters().Any(p => 
                !p.HasDefaultValue && 
                p.ParameterType.IsClass &&
                p.ParameterType.Namespace == "NetCord" &&
                p.ParameterType.Name.EndsWith("Properties")))
            .OrderByDescending(c => c.GetParameters().Count(p => !p.HasDefaultValue))
            .ThenByDescending(c => c.GetParameters().Length)
            .FirstOrDefault() ?? ctors.First();
        
        var parameters = mainCtor.GetParameters();

        if (parameters.Length == 0)
            return [];

        var ctorMemberName = GetXmlConstructorMemberName(mainCtor);
        var constructorParams = new List<ConstructorParam>();
        foreach (var param in parameters)
        {
            var paramType = param.ParameterType;
            var implementations = paramType.IsInterface 
                ? FindConcreteImplementations(paramType, type.Assembly)
                : null;

            var paramDescription = GetXmlParamDescription(ctorMemberName, param.Name ?? "");
            var constructorParam = new ConstructorParam
            {
                Name = param.Name ?? "",
                Type = GetFriendlyTypeName(paramType),
                HasDefaultValue = param.HasDefaultValue,
                InterfaceImplementations = implementations,
                DefaultValue = param.HasDefaultValue ? param.DefaultValue?.ToString() : null,
                Description = paramDescription
            };
            constructorParams.Add(constructorParam);
        }

        return constructorParams;
    }

    static string GetFriendlyTypeName(Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type);
        if (underlyingType != null)
        {
            return GetFriendlyTypeName(underlyingType) + "?";
        }

        if (type.IsGenericType)
        {
            var genericTypeDef = type.GetGenericTypeDefinition();
            var genericArgs = type.GetGenericArguments();
            var typeName = type.Name.Substring(0, type.Name.IndexOf('`'));
            var namespaceName = type.Namespace ?? "";
            
            var args = string.Join(", ", genericArgs.Select(GetFriendlyTypeName));
            return $"{namespaceName}.{typeName}<{args}>";
        }

        if (type.IsArray)
        {
            return GetFriendlyTypeName(type.GetElementType()!) + "[]";
        }

        if (!string.IsNullOrEmpty(type.Namespace))
        {
            return $"{type.Namespace}.{type.Name}";
        }

        return type.Name;
    }

    static List<string>? FindConcreteImplementations(Type interfaceType, Assembly assembly)
    {
        if (!interfaceType.IsInterface)
            return null;

        var implementations = assembly.GetTypes()
            .Where(t => !t.IsAbstract 
                     && !t.IsInterface
                     && t.IsPublic
                     && interfaceType.IsAssignableFrom(t)
                     && t.Namespace?.StartsWith("NetCord.Rest") == true
                     && t.Name.EndsWith("Properties"))
            .Select(t => t.FullName ?? t.Name)
            .OrderBy(n => n)
            .ToList();

        return implementations.Count > 0 ? implementations : null;
    }

    static string? GuessConfigKey(PropertyInfo prop)
    {
        if (prop.PropertyType == typeof(string) && defaultsConfig != null)
        {
            if (defaultsConfig.PatternDefaults.ContainsKey(prop.Name))
                return prop.Name;

            if (prop.Name.EndsWith("Title") && defaultsConfig.PatternDefaults.ContainsKey("Title"))
                return "Title";
            if (prop.Name.EndsWith("Description") && defaultsConfig.PatternDefaults.ContainsKey("Description"))
                return "Description";
            if (prop.Name.EndsWith("Url") && defaultsConfig.PatternDefaults.ContainsKey("Url"))
                return "Url";
        }
        
        return null;
    }

    static string? GuessDefaultValue(Type type, string propertyName = "")
    {
        if (type == typeof(string) && defaultsConfig != null && !string.IsNullOrEmpty(propertyName))
        {
            var safePatterns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Name", "Title", "Description", "Label", "Text", "Content", "FileName",
                "Url", "ImageUrl", "ThumbnailUrl", "IconUrl", "AvatarUrl",
                "Nickname", "Username", "ThreadName", "Topic", "Location",
                "Placeholder", "EmojiName", "UnicodeIcon", "RtcRegion",
                "PlatformUsername", "CommandName"
            };

            if (safePatterns.Contains(propertyName) && 
                defaultsConfig.PatternDefaults.TryGetValue(propertyName, out var patternDefault))
                return $"\"{patternDefault}\"";

            foreach (var pattern in safePatterns)
            {
                if (propertyName.EndsWith(pattern, StringComparison.OrdinalIgnoreCase) &&
                    defaultsConfig.PatternDefaults.ContainsKey(pattern))
                {
                    return $"\"{defaultsConfig.PatternDefaults[pattern]}\"";
                }
            }
        }

        if (defaultsConfig != null)
        {
            var typeKey = type.FullName ?? type.Name;
            if (defaultsConfig.TypeDefaults.TryGetValue(typeKey, out var typeDefault))
            {
                if (type == typeof(string))
                    return $"\"{typeDefault}\"";
                return typeDefault;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType != null && defaultsConfig.TypeDefaults.TryGetValue(underlyingType.FullName ?? underlyingType.Name, out var nullableDefault))
                    return nullableDefault;
            }
        }

        if (type.IsEnum)
        {
            var firstValue = Enum.GetValues(type).Cast<object>().FirstOrDefault();
            if (firstValue != null)
            {
                var typeName = type.Name;
                return $"{typeName}.{firstValue}";
            }
        }
        
        return null;
    }

    static bool IsNestedSample(Type type)
    {
        return type.Namespace?.StartsWith("NetCord.Rest") == true 
            && type.Name.EndsWith("Properties")
            && (type.IsClass || type.IsValueType);
    }

    static string? GetCollectionElementType(Type type)
    {
        // Don't treat Nullable as a collection
        if (Nullable.GetUnderlyingType(type) != null)
            return null;

        if (type.IsGenericType)
        {
            var genericArgs = type.GetGenericArguments();
            if (genericArgs.Length == 1 && IsNestedSample(genericArgs[0]))
            {
                return GetFriendlyTypeName(genericArgs[0]);
            }
        }
        return null;
    }

    static int? GuessMaxLength(string propertyName)
    {
        return propertyName switch
        {
            "Title" => 256,
            "Description" => 4096,
            "Name" when propertyName.Contains("Field") => 256,
            "Value" when propertyName.Contains("Field") => 1024,
            "Text" when propertyName.Contains("Footer") => 2048,
            "Name" when propertyName.Contains("Author") => 256,
            "Content" => 2000,
            "Label" => 80,
            "Placeholder" => 150,
            "CustomId" => 100,
            "Name" => 100,
            "FileName" => 256,
            _ => null
        };
    }

    class SampleDefaultsConfig
    {
        public Dictionary<string, string> PatternDefaults { get; set; } = [];
        public Dictionary<string, string> TypeDefaults { get; set; } = [];
        public Dictionary<string, Dictionary<string, string>> SpecificTypeDefaults { get; set; } = [];
    }

    static void SaveTypes(string path, List<TypeMetadata> types)
    {
        var container = new TypeContainer { Types = [.. types] };
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = null,  // Use PascalCase (default) to match generator expectations
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,  // Don't escape < and >
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull  // Omit null values
        };

        var json = JsonSerializer.Serialize(container, options);
        
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);
        File.WriteAllText(path, json);
        
        Console.WriteLine($"\nSaved to: {path}");
    }
}

record TypeContainer
{
    public TypeMetadata[] Types { get; set; } = [];
}

record TypeMetadata
{
    public string FullType { get; set; } = "";
    public string Description { get; set; } = "";
    public string SourceSummary { get; set; } = "";
    public string Category { get; set; } = "";
    public List<ConstructorParam> ConstructorParams { get; set; } = [];
    public List<PropertyMetadata> Properties { get; set; } = [];
    public List<object> Limitations { get; set; } = [];
}

record ConstructorParam
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public bool HasDefaultValue { get; set; }
    public string? DefaultValue { get; set; }
    public string? Description { get; set; }
    public List<string>? InterfaceImplementations { get; set; }
}

record PropertyMetadata
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public string? Description { get; set; }
    public string? SourceSummary { get; set; }
    public string? ConfigKey { get; set; }
    public string? DefaultValue { get; set; }
    public bool NestedSample { get; set; }
    public string? CollectionOf { get; set; }
    public int? MaxLength { get; set; }
}
