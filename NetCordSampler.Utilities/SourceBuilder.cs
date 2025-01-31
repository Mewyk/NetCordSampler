using System.Text;

namespace NetCordSampler.Utilities;

public static class SourceBuilder
{
    private static readonly string[] CollectionTypes = ["IEnumerable", "IList", "ICollection", "List", "ImmutableArray"];

    public static string GenerateEnum(IEnumerable<SampleClass> sampleClasses)
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

    public static string GenerateImmutableSourceCode(IEnumerable<SampleClass> sampleClasses)
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

    private static string BuildMainEnum(SampleClass sampleClass)
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

    public static void GenerateSampleSourceCode(IEnumerable<SampleClass> sampleClasses, string outputDirectory)
    {
        var indent = "\t";
        var classNames = new HashSet<string>(sampleClasses.Select(so => so.Name));
        var unknownTypes = new HashSet<string>();

        foreach (var sampleClass in sampleClasses)
        {
            var className = sampleClass.Name;
            var camelCaseName = char.ToLowerInvariant(className[0]) + className[1..];
            var sourceBuilder = new StringBuilder();

            // Usings
            sourceBuilder.AppendLine("using NetCord.Rest;");
            sourceBuilder.AppendLine();
            sourceBuilder.AppendLine("using NetCordSampler.Interfaces;");
            sourceBuilder.AppendLine();

            // Namespace
            sourceBuilder.AppendLine("namespace NetCordSampler.CodeSamples.RestObjects;");
            sourceBuilder.AppendLine();

            // Class
            sourceBuilder.AppendLine($"public class {className}Sample : ICodeSample<{className}>");
            sourceBuilder.AppendLine("{");

            // CreateDefault
            sourceBuilder.AppendLine($"{indent}public {className} CreateDefault(SamplerSettings settings) => new()");
            sourceBuilder.AppendLine($"{indent}{{");

            if (sampleClass.Properties != null && !sampleClass.Properties.IsEmpty)
            {
                foreach (var property in sampleClass.Properties)
                {
                    var propertyName = property.Name;
                    var propertyType = property.Type.TrimEnd('?'); // Remove nullable

                    string? propertyAssignment = null;

                    // First handle by property names
                    if (propertyName == "Title")
                        propertyAssignment = $"{propertyName} = \"{propertyName}\"";
                    else if (propertyName == "Description")
                    {
                        // Properties.Description
                        if (!string.IsNullOrEmpty(property.Description))
                            propertyAssignment = $"{propertyName} = \"{property.Description}\"";
                        else
                            propertyAssignment = $"{propertyName} = settings.DefaultValues.MissingDescription";
                    }
                    else if (propertyName == "Timestamp")
                        propertyAssignment = $"{propertyName} = DateTimeOffset.UtcNow";
                    else if (propertyName == "Image")
                        propertyAssignment = $"{propertyName} = settings.DefaultValues.Urls.Image";
                    else if (propertyName == "Thumbnail")
                        propertyAssignment = $"{propertyName} = settings.DefaultValues.Urls.Thumbnail";
                    else if (propertyName == "Url")
                        propertyAssignment = $"{propertyName} = settings.DefaultValues.Urls.Website";
                    else if (propertyName == "Icon")
                        propertyAssignment = $"{propertyName} = settings.DefaultValues.Urls.Icon";
                    else
                    {
                        var (CollectionType, InnerType) = GetCollectionType(typeName: propertyType);

                        if (CollectionType != null)
                        {
                            if (!classNames.Contains(InnerType))
                            {
                                propertyAssignment = $"{propertyName} = null";
                                unknownTypes.Add(InnerType);
                            }
                            else
                                propertyAssignment = $"{propertyName} = [\n\t\t\tnew {InnerType}Sample().CreateDefault(settings),\n\t\t\tnew {InnerType}Sample().CreateDefault(settings)\n\t\t]";
                        }
                        else if (classNames.Contains(propertyType))
                            propertyAssignment = $"{propertyName} = new {propertyType}Sample().CreateDefault(settings)";
                        else if (propertyType == "string")
                            propertyAssignment = $"{propertyName} = string.Empty";
                        else if (propertyType is "int" or "long" or "double" or "float" or "decimal")
                            propertyAssignment = $"{propertyName} = 0";
                        else if (propertyType == "bool")
                            propertyAssignment = $"{propertyName} = false";
                        else // Unknown type
                        {
                            propertyAssignment = $"{propertyName} = null";
                            unknownTypes.Add(propertyType);
                        }
                    }

                    if (propertyAssignment != null)
                        sourceBuilder.AppendLine($"{indent}\t{propertyAssignment},");
                }
            }

            // Remove trailing comma
            if (sourceBuilder.ToString().EndsWith(",\n"))
                sourceBuilder.Remove(sourceBuilder.Length - 2, 1);

            // Close CreateDefault method
            sourceBuilder
                .AppendLine($"{indent}}};")
                .AppendLine();

            // IsEmpty method
            sourceBuilder.AppendLine($"{indent}public bool IsEmpty({className} {camelCaseName}) =>");
            if (sampleClass.Properties != null && !sampleClass.Properties.IsEmpty)
            {
                var checks = new List<string>();
                foreach (var property in sampleClass.Properties)
                {
                    var propertyName = property.Name;
                    var propertyType = property.Type.TrimEnd('?'); // Remove nullable
                    string? check = null;

                    var (CollectionType, InnerType) = GetCollectionType(typeName: propertyType);

                    if (CollectionType != null)
                        check = classNames.Contains(InnerType) 
                            ? $"!{camelCaseName}.{propertyName}.Any()" 
                            : $"{camelCaseName}.{propertyName} == null";
                    else if (propertyType == "string")
                        check = $"string.IsNullOrEmpty({camelCaseName}.{propertyName})";
                    else if (propertyType is "int" or "long" or "double" or "float" or "decimal")
                        check = $"{camelCaseName}.{propertyName} == 0";
                    else if (propertyType == "bool")
                        check = $"!{camelCaseName}.{propertyName}";
                    else if (classNames.Contains(propertyType))
                        check = $"{camelCaseName}.{propertyName} == null";
                    else
                        check = $"{camelCaseName}.{propertyName} == null";

                    checks.Add(check);
                }

                sourceBuilder.AppendLine($"{indent}\t" + string.Join(" &&\n\t", checks) + ";");
            }
            else sourceBuilder.AppendLine($"{indent}\ttrue;");

            sourceBuilder.AppendLine("}");

            // Write
            var fileName = Path.Combine(outputDirectory, $"{className}Sample.cs");
            File.WriteAllText(fileName, sourceBuilder.ToString());
        }
    }

    private static (string? CollectionType, string InnerType) GetCollectionType(string typeName) => 
        typeName.IndexOf('<') > 0 && 
        typeName.LastIndexOf('>') > typeName.IndexOf('<') && 
        CollectionTypes.Contains(typeName[..typeName.IndexOf('<')])
            ? ((string? CollectionType, string InnerType))(typeName[..typeName.IndexOf('<')], 
                typeName[(typeName.IndexOf('<') + 1)..typeName.LastIndexOf('>')])
            : ((string? CollectionType, string InnerType))(null, string.Empty);
}
