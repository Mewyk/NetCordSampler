using System.Text;

namespace NetCordSampler.Utilities;

public static class Housekeeping
{
    public static string ParseXmlComment(string input)
    {
        var processedInput = ProcessLines(input);
        processedInput = CleanXmlTags(processedInput);
        processedInput = CleanSummaryXml(processedInput);

        // Because line breaks in comments are not caught by Split
        return processedInput
            .Replace("\n", string.Empty)
            .Replace("\r", string.Empty)
            .Trim();
    }

    public static string ProcessLines(string input)
    {
        var result = new StringBuilder();
        string[] lines = input.Split(
            ["\r\n", "\r", "\n"],
            StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
            result.AppendLine(line);

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

    public static string CleanFileName(string fileName) =>
        fileName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
            ? fileName[..^3]
            : fileName;
}
