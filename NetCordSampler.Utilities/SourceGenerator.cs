using System.Text;
using System.Collections.Immutable;

namespace NetCordSampler.Utilities;

public static class SourceGenerator
{
    public static string GenerateEnum(ImmutableDictionary<string, ImmutableDictionary<string, string>> data)
    {
        var sourceBuilder = new StringBuilder()
            .AppendLine("namespace NetCordSampler.Enums;")
            .AppendLine()
            .AppendLine("public static class RestEnums")
            .AppendLine("{");

        var enumDefinitions = data
            .Where(keyValuePair => keyValuePair.Value.Count > 0)
            .Select(keyValuePair => BuildMainEnum(keyValuePair.Key, keyValuePair.Value))
            .ToList();

        foreach (var enumDefinition in enumDefinitions)
            sourceBuilder.AppendLine(enumDefinition);


        if (enumDefinitions.Count > 0)
        {
            var masterEnum = BuildMasterEnum(data.Keys);
            sourceBuilder.AppendLine(masterEnum);
        }

        sourceBuilder.AppendLine("}");

        return sourceBuilder.ToString();
    }

    public static string GenerateImmutableSourceCode(ImmutableDictionary<string, ImmutableDictionary<string, string>> data)
    {
        // TODO: Handle this properly
        if (data.IsEmpty)
            return string.Empty;

        var ImmutableBuilder = new StringBuilder()
            .AppendLine("using System.Collections.Immutable;")
            .AppendLine()
            .AppendLine("namespace NetCordSampler.ImmutableCollections;")
            .AppendLine()
            .AppendLine("public static class RestImmutableCollections")
            .AppendLine("{")
            .AppendLine("\tpublic static readonly ImmutableDictionary<string, ImmutableArray<string>> Collections =")
            .AppendLine("\t\tImmutableDictionary.Create<string, ImmutableArray<string>>()");

        var lastKey = data.Keys.Last();
        foreach (var (key, properties) in data)
        {
            var values = properties.Keys.Select(value => $"\"{value}\"");
            var addCodeLine = $"\t\t\t.Add(\"{key}\", ImmutableArray.Create({string.Join(", ", values)}))";

            if (key == lastKey) addCodeLine += ";";
            ImmutableBuilder.AppendLine(addCodeLine);
        }

        return ImmutableBuilder
            .AppendLine("}")
            .ToString();
    }

    private static string BuildMainEnum(string enumName, ImmutableDictionary<string, string> properties)
    {
        var enumBuilder = new StringBuilder()
            .AppendLine($"\tpublic enum {enumName}")
            .AppendLine("\t{");

        var propertyLines = properties.Select(property =>
        {
            return $"\t\t[Summary(\"{property.Value}\")]\n\t\t{property.Key}";
        });

        return enumBuilder
            .AppendLine(string.Join(",\n", propertyLines))
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
