using System.Text;

namespace NetCordSampler.Utilities;

public static class SourceGenerator
{
    public static string GenerateEnum(IEnumerable<SampleObject> sampleObjects)
    {
        var sourceBuilder = new StringBuilder()
            .AppendLine("namespace NetCordSampler.Enums;\n")
            .AppendLine("public static class RestEnums\n{");

        foreach (var sampleObject in sampleObjects)
            sourceBuilder.AppendLine(
                BuildMainEnum(sampleObject));

        if (sampleObjects.Any())
            sourceBuilder.AppendLine(
                BuildMasterEnum(sampleObjects.Select(obj => obj.Name)));

        return sourceBuilder
            .AppendLine("}")
            .ToString();
    }

    public static string GenerateImmutableSourceCode(IEnumerable<SampleObject> sampleObjects)
    {
        if (!sampleObjects.Any())
            return string.Empty;

        var immutableBuilder = new StringBuilder()
            .AppendLine("using System.Collections.Immutable;\n")
            .AppendLine("namespace NetCordSampler.ImmutableCollections;\n")
            .AppendLine("public static class RestImmutableCollections\n{")
            .AppendLine("\tpublic static readonly ImmutableDictionary<string, ImmutableArray<string>> Collections =")
            .AppendLine("\t\tImmutableDictionary.Create<string, ImmutableArray<string>>()");

        var sampleObjectList = sampleObjects.ToList();
        var lastObject = sampleObjectList.Last();

        foreach (var sampleObject in sampleObjectList)
        {
            var propertyNames = sampleObject.Properties?
                .Select(prop => $"\"{prop.Name}\"") ?? [];

            var addCodeLine = $"\t\t\t.Add(\"{sampleObject.Name}\", [{string.Join(", ", propertyNames)}])";

            if (sampleObject == lastObject)
                addCodeLine += ";";

            immutableBuilder.AppendLine(addCodeLine);
        }

        return immutableBuilder
            .AppendLine("}")
            .ToString();
    }

    private static string BuildMainEnum(SampleObject sampleObject)
    {
        var enumBuilder = new StringBuilder()
            .AppendLine($"\tpublic enum {sampleObject.Name}\n\t{{");

        var properties = sampleObject.Properties;
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
}
