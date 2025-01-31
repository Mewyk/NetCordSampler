using System.Text;

namespace NetCordSampler.Utilities;

public static class SourceBuilder
{
    private static readonly string[] CollectionTypes = ["IEnumerable", "IList", "ICollection", "List", "ImmutableArray"];

    public static string GenerateEnum(IEnumerable<SamplerData> sampleClasses)
    {
        var sourceBuilder = new StringBuilder()
            .AppendLine("namespace NetCordSampler.Enums;\n")
            .AppendLine("public static class RestEnums\n{");

        foreach (var sampleClass in sampleClasses)
            sourceBuilder.AppendLine(
                BuildMainEnum(sampleClass));

        if (sampleClasses.Any())
            sourceBuilder.AppendLine(
                BuildMasterEnum(sampleClasses.Select(obj => obj.Name)));

        return sourceBuilder
            .AppendLine("}")
            .ToString();
    }

    public static string GenerateImmutableSourceCode(IEnumerable<SamplerData> sampleClasses)
    {
        if (!sampleClasses.Any())
            return string.Empty;

        var immutableBuilder = new StringBuilder()
            .AppendLine("using System.Collections.Immutable;\n")
            .AppendLine("namespace NetCordSampler.ImmutableCollections;\n")
            .AppendLine("public static class RestImmutableCollections\n{")
            .AppendLine("\tpublic static readonly ImmutableDictionary<string, ImmutableArray<string>> Collections =")
            .AppendLine("\t\tImmutableDictionary.Create<string, ImmutableArray<string>>()");

        var lastClass = sampleClasses.Last();

        foreach (var sampleClass in sampleClasses)
        {
            var propertyNames = sampleClass.Properties?
                .Select(prop => $"\"{prop.Name}\"") ?? [];

            var addCodeLine = $"\t\t\t.Add(\"{sampleClass.Name}\", [{string.Join(", ", propertyNames)}])";

            if (sampleClass == lastClass)
                addCodeLine += ";";

            immutableBuilder.AppendLine(addCodeLine);
        }

        return immutableBuilder
            .AppendLine("}")
            .ToString();
    }

    private static string BuildMainEnum(SamplerData sampleClass)
    {
        var enumBuilder = new StringBuilder()
            .AppendLine($"\tpublic enum {sampleClass.Name}\n\t{{");

        var properties = sampleClass.Properties;
        if (properties != null)
        {
            int index = 0;
            foreach (var property in properties)
            {
                index++;
                var attributeBlock = new List<string>
                    {
                        $"typeof({property.Type})",
                        $"\"{property.Description}\""
                    };

                enumBuilder
                    .AppendLine($"\t\t[SamplerData({string.Join(", ", attributeBlock)})]")
                    .Append($"\t\t{property.Name}");

                if (index < properties.Count)
                    enumBuilder.AppendLine(",");
                else
                    enumBuilder.AppendLine();
            }
        }

        return enumBuilder
            .AppendLine("\t}")
            .ToString();
    }

    private static string BuildMasterEnum(IEnumerable<string> enumNames) => new StringBuilder()
        .AppendLine("\tpublic enum RestObjects\n\t{")
        .AppendLine(string.Join(",\n", enumNames.Select(name => $"\t\t{name}")))
        .AppendLine("\t}")
        .ToString();

    public static void GenerateSampleSourceCode(IEnumerable<SamplerData> sampleClasses, string outputDirectory)
    {
        var indent = "\t";
        var classNames = new HashSet<string>(sampleClasses.Select(so => so.Name));
        var unknownTypes = new HashSet<string>();

        foreach (var sampleClass in sampleClasses)
        {
            if (!string.Equals(sampleClass.Accessibility, "public", StringComparison.OrdinalIgnoreCase))
                continue;

            var className = sampleClass.Name;
            if (string.IsNullOrEmpty(className))
                continue;

            var camelCaseName = char.ToLowerInvariant(className[0]) + className[1..];
            var sourceBuilder = new StringBuilder();

            // Usings
            sourceBuilder.AppendLine("using NetCord.Rest;");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("using NetCordSampler.Interfaces;");
            sourceBuilder.AppendLine();

            // Namespace
            sourceBuilder.AppendLine("namespace NetCordSampler.CodeSamples.Rest;");
            sourceBuilder.AppendLine();

            // Class
            sourceBuilder.AppendLine($"public class {className}Sample : ICodeSample<{className}>");
            sourceBuilder.AppendLine("{");

            // CreateDefault
            sourceBuilder.AppendLine($"{indent}public {className} CreateDefault(SamplerSettings settings) => new()");
            sourceBuilder.AppendLine($"{indent}{{");

            if (sampleClass.Properties != null && !sampleClass.Properties.IsEmpty)
            {
                var assignments = new List<string>();

                foreach (var property in sampleClass.Properties)
                {
                    if (!string.Equals(property.Accessibility, "public", StringComparison.OrdinalIgnoreCase) || property.IsStatic)
                        continue;

                    var propertyName = property.Name;
                    var propertyType = property.Type.TrimEnd('?'); // Remove nullable

                    string? propertyAssignment = null;

                    // Handle by property names
                    propertyAssignment = propertyType switch
                    {
                        "string" when propertyName.Equals("Title", StringComparison.OrdinalIgnoreCase) =>
                            $"{propertyName} = \"{propertyName}\",",
                        "string" when propertyName.Equals("Description", StringComparison.OrdinalIgnoreCase) =>
                            string.IsNullOrEmpty(property.Description)
                                ? $"{propertyName} = settings.DefaultValues.MissingDescription,"
                                : $"{propertyName} = \"{property.Description}\",",
                        "string" when propertyName.Equals("Text", StringComparison.OrdinalIgnoreCase) =>
                            string.IsNullOrEmpty(property.Description)
                                ? $"{propertyName} = settings.DefaultValues.MissingDescription,"
                                : $"{propertyName} = \"{property.Description}\",",
                        "DateTimeOffset" when propertyName.Equals("Timestamp", StringComparison.OrdinalIgnoreCase) =>
                            $"{propertyName} = DateTimeOffset.UtcNow,",
                        "string" when propertyName.Equals("Image", StringComparison.OrdinalIgnoreCase) =>
                            $"{propertyName} = settings.DefaultValues.Urls.Image,",
                        "string" when propertyName.Equals("ImageUrl", StringComparison.OrdinalIgnoreCase) =>
                            $"{propertyName} = settings.DefaultValues.Urls.Image,",
                        "string" when propertyName.Equals("Thumbnail", StringComparison.OrdinalIgnoreCase) =>
                            $"{propertyName} = settings.DefaultValues.Urls.Thumbnail,",
                        "string" when propertyName.Equals("ThumbnailUrl", StringComparison.OrdinalIgnoreCase) =>
                            $"{propertyName} = settings.DefaultValues.Urls.Thumbnail,",
                        "string" when propertyName.Equals("Url", StringComparison.OrdinalIgnoreCase) =>
                            $"{propertyName} = settings.DefaultValues.Urls.Website,",
                        "string" when propertyName.Equals("Icon", StringComparison.OrdinalIgnoreCase) =>
                            $"{propertyName} = settings.DefaultValues.Urls.Icon,",
                        "string" when propertyName.Equals("IconUrl", StringComparison.OrdinalIgnoreCase) =>
                            $"{propertyName} = settings.DefaultValues.Urls.Icon,",
                        _ => HandleGenericAssignment(property, propertyType, classNames, ref unknownTypes)
                    };

                    if (propertyAssignment != null)
                        assignments.Add($"{indent}\t{propertyAssignment}");
                }

                // Remove trailing comma
                if (assignments.Count > 0)
                {
                    int lastIndex = assignments.Count - 1;
                    assignments[lastIndex] = assignments[lastIndex].TrimEnd(',');
                }

                var joinedAssignments = string.Join("\n", assignments);
                sourceBuilder.AppendLine(joinedAssignments);
            }

            // Close CreateDefault method
            sourceBuilder.AppendLine($"{indent}}};");
            sourceBuilder.AppendLine();

            // IsEmpty method
            sourceBuilder.AppendLine($"{indent}public bool IsEmpty({className} {camelCaseName}) =>");
            if (sampleClass.Properties != null && !sampleClass.Properties.IsEmpty)
            {
                var checks = new List<string>();
                foreach (var property in sampleClass.Properties)
                {
                    if (!string.Equals(property.Accessibility, "public", StringComparison.OrdinalIgnoreCase) || property.IsStatic)
                        continue;

                    var propertyName = property.Name;
                    var propertyType = property.Type.TrimEnd('?'); // Remove nullable

                    string? check = propertyType switch
                    {
                        "string" when propertyName.Equals("Title", StringComparison.OrdinalIgnoreCase) =>
                            $"string.IsNullOrEmpty({camelCaseName}.{propertyName})",
                        "string" when propertyName.Equals("Description", StringComparison.OrdinalIgnoreCase) =>
                            $"string.IsNullOrEmpty({camelCaseName}.{propertyName})",
                        "string" when propertyName.Equals("Text", StringComparison.OrdinalIgnoreCase) =>
                            $"string.IsNullOrEmpty({camelCaseName}.{propertyName})",
                        "DateTimeOffset" when propertyName.Equals("Timestamp", StringComparison.OrdinalIgnoreCase) =>
                            $"{camelCaseName}.{propertyName} == default",
                        "string" when propertyName.Equals("Image", StringComparison.OrdinalIgnoreCase) =>
                            $"string.IsNullOrEmpty({camelCaseName}.{propertyName})",
                        "string" when propertyName.Equals("Thumbnail", StringComparison.OrdinalIgnoreCase) =>
                            $"string.IsNullOrEmpty({camelCaseName}.{propertyName})",
                        "string" when propertyName.Equals("Url", StringComparison.OrdinalIgnoreCase) =>
                            $"string.IsNullOrEmpty({camelCaseName}.{propertyName})",
                        "string" when propertyName.Equals("Icon", StringComparison.OrdinalIgnoreCase) =>
                            $"string.IsNullOrEmpty({camelCaseName}.{propertyName})",
                        "string" when propertyName.Equals("IconUrl", StringComparison.OrdinalIgnoreCase) =>
                            $"string.IsNullOrEmpty({camelCaseName}.{propertyName})",
                        _ => HandleGenericCheck(property, propertyType, camelCaseName, classNames)
                    };

                    if (check != null)
                        checks.Add(check);
                }

                if (checks.Count != 0)
                {
                    var indentForChecks = $"{indent}\t";
                    var joinedChecks = string.Join($" &&\n{indentForChecks}", checks);
                    sourceBuilder.AppendLine($"{indent}\t{joinedChecks};");
                }
                else sourceBuilder.AppendLine($"{indent}\ttrue;");
            }
            else sourceBuilder.AppendLine($"{indent}\ttrue;");

            sourceBuilder.AppendLine("}");

            // Write
            var fileName = Path.Combine(outputDirectory, $"{className}Sample.cs");
            File.WriteAllText(fileName, sourceBuilder.ToString());
        }
    }

    private static string? HandleGenericAssignment(SamplerData.Property property, string propertyType, HashSet<string> classNames, ref HashSet<string> unknownTypes)
    {
        var (CollectionType, InnerType) = GetCollectionType(propertyType);

        if (CollectionType != null)
        {
            if (!classNames.Contains(InnerType))
            {
                unknownTypes.Add(InnerType);
                return $"{property.Name} = null,";
            }
            else
            {
                return $"{property.Name} = new List<{InnerType}>()\n\t\t\t{{\n\t\t\t\tnew {InnerType}Sample().CreateDefault(settings),\n\t\t\t\tnew {InnerType}Sample().CreateDefault(settings)\n\t\t\t}},";
            }
        }
        else if (classNames.Contains(propertyType))
        {
            return $"{property.Name} = new {propertyType}Sample().CreateDefault(settings),";
        }
        else
        {
            return propertyType switch
            {
                "string" => $"{property.Name} = string.Empty,",
                "int" or "long" or "double" or "float" or "decimal" => $"{property.Name} = 0,",
                "bool" => $"{property.Name} = false,",
                _ => $"{property.Name} = null,"
            };
        }
    }

    private static string? HandleGenericCheck(
        SamplerData.Property property, 
        string propertyType, 
        string camelCaseName, 
        HashSet<string> classNames)
    {
        var (CollectionType, InnerType) = GetCollectionType(propertyType);

        if (CollectionType != null)
            return classNames.Contains(InnerType)
                ? $"!{camelCaseName}.{property.Name}.Any()"
                : $"{camelCaseName}.{property.Name} == null";
        else if (propertyType == "string")
            return $"string.IsNullOrEmpty({camelCaseName}.{property.Name})";
        else if (propertyType is "int" or "long" or "double" or "float" or "decimal")
            return $"{camelCaseName}.{property.Name} == 0";
        else if (propertyType == "bool")
            return $"!{camelCaseName}.{property.Name}";
        else if (classNames.Contains(propertyType))
            return $"{camelCaseName}.{property.Name} == null";
        else
            return $"{camelCaseName}.{property.Name} == null";
    }

    private static (string? CollectionType, string InnerType) GetCollectionType(string typeName) =>
        typeName.Contains('<') && typeName.Contains('>') &&
        CollectionTypes.Contains(typeName[..typeName.IndexOf('<')])
            ? (typeName[..typeName.IndexOf('<')],
                typeName.Substring(typeName.IndexOf('<') + 1, 
                typeName.LastIndexOf('>') - typeName.IndexOf('<') - 1))
            : (null, string.Empty);
}
