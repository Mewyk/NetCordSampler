using System.Text;
using System.Collections.Immutable;

namespace NetCordSampler.Utilities;

public static class SourceGenerator
{
    public static string GenerateEnum(ImmutableDictionary<string, ImmutableDictionary<string, PropertyInformation>> data)
    {
        var sourceBuilder = new StringBuilder()
            .AppendLine("namespace NetCordSampler.Enums;\n")
            .AppendLine("public static class RestEnums\n{");

        foreach (var (objectName, properties) in data)
            sourceBuilder.AppendLine(BuildMainEnum(objectName, properties));

        if (!data.IsEmpty)
            sourceBuilder.AppendLine(BuildMasterEnum(data.Keys));

        return sourceBuilder
            .AppendLine("}")
            .ToString();
    }

    public static string GenerateImmutableSourceCode(ImmutableDictionary<string, ImmutableDictionary<string, PropertyInformation>> data)
    {
        if (data.IsEmpty)
            return string.Empty;

        var immutableBuilder = new StringBuilder()
            .AppendLine("using System.Collections.Immutable;\n")
            .AppendLine("namespace NetCordSampler.ImmutableCollections;\n")
            .AppendLine("public static class RestImmutableCollections\n{")
            .AppendLine("\tpublic static readonly ImmutableDictionary<string, ImmutableArray<string>> Collections =")
            .AppendLine("\t\tImmutableDictionary.Create<string, ImmutableArray<string>>()");

        var lastKey = data.Keys.Last();
        foreach (var (objectName, properties) in data)
        {
            var propertyNames = properties.Keys.Select(name => $"\"{name}\"");
            var addCodeLine = $"\t\t\t.Add(\"{objectName}\", [{string.Join(", ", propertyNames)}])";

            if (objectName == lastKey)
                addCodeLine += ";";

            immutableBuilder.AppendLine(addCodeLine);
        }

        return immutableBuilder
            .AppendLine("}")
            .ToString();
    }

    private static string BuildMainEnum(string objectName, ImmutableDictionary<string, PropertyInformation> properties)
    {
        var enumBuilder = new StringBuilder()
            .AppendLine($"\tpublic enum {objectName}\n\t{{");

        int index = 0;
        var propertiesList = properties.ToList();
        foreach (var property in propertiesList)
        {
            index++;
            var attributeBlock = new List<string>
            {
                $"typeof({property.Value.Type})",
                $"\"{property.Value.Summary}\""
            };

            enumBuilder
                //.AppendLine($"\t\t[DiscordRules(null, null, null, null, null)]")
                //.AppendLine($"\t\t[Summary(\"{property.Value.Summary}\")]")
                .AppendLine($"\t\t[SamplerData({string.Join(", ", attributeBlock)})]")
                .Append($"\t\t{property.Key}");

            if (index < propertiesList.Count)
                enumBuilder.AppendLine(",");
            else 
                enumBuilder.AppendLine();
        }

        return enumBuilder
            .AppendLine("\t}")
            .ToString();
    }

    private static string BuildMasterEnum(IEnumerable<string> enumNames)
    {
        return new StringBuilder()
            .AppendLine("\tpublic enum RestObjects\n\t{")
            .AppendLine(string.Join(",\n", enumNames.Select(name => $"\t\t{name}")))
            .AppendLine("\t}")
            .ToString();
    }
}

public class PropertyInformation
{
    private string type = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Type 
    { 
        get => type.Replace("?", string.Empty); 
        set => type = value; 
    }
}
