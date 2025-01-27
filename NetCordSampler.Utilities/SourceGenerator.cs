using System.Text;
using System.Collections.Immutable;

namespace NetCordSampler.Utilities;

public static class SourceGenerator
{
    public static string GenerateEnum(ImmutableDictionary<string, ImmutableDictionary<string, PropertyInformation>> data)
    {
        var sourceBuilder = new StringBuilder()
            .AppendLine("namespace NetCordSampler.Enums;")
            .AppendLine()
            .AppendLine("public static class RestEnums")
            .AppendLine("{");

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
            .AppendLine("using System.Collections.Immutable;")
            .AppendLine()
            .AppendLine("namespace NetCordSampler.ImmutableCollections;")
            .AppendLine()
            .AppendLine("public static class RestImmutableCollections")
            .AppendLine("{")
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
            .Append($"\tpublic enum {objectName}\n")
            .Append("\t{\n");

        var propertiesList = properties.ToList();
        int totalProperties = propertiesList.Count;
        int currentIndex = 0;

        foreach (var property in propertiesList)
        {
            currentIndex++;
            var propertyInfo = property.Value;
            var attributeBlock = new List<string>
            {
                $"typeof({propertyInfo.Type})",
                $"\"{propertyInfo.Summary}\"",
                $"\"{propertyInfo.IsRequired}\""
            };

            enumBuilder
                .AppendLine($"\t\t[Summary(\"{propertyInfo.Summary}\")]")
                //.AppendLine($"\t\t[DiscordRules(\"\")]\n")
                .AppendLine($"\t\t[SamplerData({string.Join(", ", attributeBlock)})]")
                .Append($"\t\t{property.Key}");

            if (currentIndex < totalProperties)
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
            .AppendLine("\tpublic enum RestObjects")
            .AppendLine("\t{")
            .AppendLine(string.Join(",\n", enumNames.Select(name => $"\t\t{name}")))
            .AppendLine("\t}")
            .ToString();
    }
}

public class PropertyInformation
{
    public string Summary { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string AccessModifier { get; set; } = string.Empty;
    public bool IsStatic { get; set; }
    public bool IsNullable { get; set; }
    public string TypeIdentifierName { get; set; } = string.Empty;
    public List<string> Attributes { get; set; } = [];
    public List<string> GenericArguments { get; set; } = [];
    public bool IsVirtual { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsOverride { get; set; }
    public bool HasBackingField { get; set; }
    public string DeclaringType { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
}
