using System.Text;
using System.Text.Json;

namespace NetCordSampler.Utilities;

public class SourceGenerator
{
    public static string GenerateEnum(Dictionary<string, Dictionary<string, string>> jsonContent)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("namespace NetCordSampler.Enums;");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("public class RestEnums");
        stringBuilder.AppendLine("{");

        foreach (var (fileName, properties) in jsonContent)
        {
            if (properties == null || properties.Count == 0)
                continue;

            var enumName = fileName.Replace(".cs", "");
            stringBuilder.AppendLine($"\tpublic enum {enumName}");
            stringBuilder.AppendLine("\t{");

            var propertyCount = properties.Count;
            var currentIndex = 0;

            foreach (var (propertyName, description) in properties)
            {
                var cleanDescription = Housekeeping.ParseXmlComment(description);
                stringBuilder.AppendLine($"\t\t[Summary(\"{cleanDescription}\")]");
                stringBuilder.Append($"\t\t{propertyName}");

                if (++currentIndex < propertyCount)
                    stringBuilder.AppendLine(",");
                else
                    stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine("\t}");
            stringBuilder.AppendLine();
        }

        stringBuilder.AppendLine("}");
        return stringBuilder.ToString();
    }

    public static Dictionary<string, Dictionary<string, string>> ParseJsonFile(string filePath)
    {
        var jsonContent = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<
            Dictionary<string, Dictionary<string, string>>>(jsonContent) ?? [];
    }
}
