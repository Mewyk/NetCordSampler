using System.Text;

namespace NetCordSampler.Utilities;

public class Housekeeping
{
    public static string ParseXmlComment(string input)
    {
        var processedInput = ProcessLines(input);
        processedInput = CleanXmlTags(processedInput);
        processedInput = CleanSummaryXml(processedInput);

        return processedInput.Trim();
    }

    public static string ProcessLines(string input)
    {
        var result = new StringBuilder();
        string[] lines = input.Split(
            ["\r\n", "\r", "\n"],
            StringSplitOptions.None);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            if (line.Contains("\r\n")) continue;
            if (line.Contains('\r')) continue;
            if (line.Contains('\n')) continue;

            result.AppendLine(line);
        }

        return result.ToString();
    }

    public static string CleanXmlTags(string input) =>
        input
            .Replace("<see langword=\"", "`")
            .Replace("\"/>", "`")
            .Replace("<see cref=\"", "`")
            .Replace("\"/>", "`")
            .Replace("<c>", "`")
            .Replace("</c>>", "`")
            .Replace("&#38;", "&");

    public static string CleanSummaryXml(string input) =>
        input.Replace("///", string.Empty);
}
