using System.Text.Json;
using System.Collections.Concurrent;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NetCordSampler.Utilities;

internal static class Program
{
    private static readonly HttpClient httpClient = new();
    private static readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true };

    public static async Task Main()
    {
        var fileNamesPath = Path.Combine(AppContext.BaseDirectory, "NetcordRestSourceFIles.txt");
        var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "NetcordRestSourceFiles.json");
        var enumsOutputPath = Path.Combine(AppContext.BaseDirectory, "GeneratedEnums.cs");

        var fileNames = await File.ReadAllLinesAsync(fileNamesPath);
        var allSummaries = await CollectAllSummariesAsync(fileNames);

        foreach (var (fileName, summaries) in allSummaries)
        {
            Console.WriteLine($"File: {fileName}");
            foreach (var (propertyName, summary) in summaries)
                Console.WriteLine($"\t{propertyName}: {summary}");
        }

        // Serialize to a json file
        var json = JsonSerializer.Serialize(allSummaries, jsonOptions);
        await File.WriteAllTextAsync(jsonFilePath, json);

        // Generate enums
        var jsonContent = SourceGenerator.ParseJsonFile(jsonFilePath);
        var generatedCode = SourceGenerator.GenerateEnum(jsonContent);

        // Save to a class file
        Directory.CreateDirectory(Path.GetDirectoryName(enumsOutputPath)!);
        File.WriteAllText(enumsOutputPath, generatedCode);
    }

    // Temporary
    private static async Task<ConcurrentDictionary<string, ConcurrentDictionary<string, string>>>
        CollectAllSummariesAsync(IEnumerable<string> fileNames)
    {
        var summariesDictionary = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
        var downloadTasks = fileNames.Select(async fileName =>
        {
            var sourceCode = await DownloadFileAsync(fileName);
            if (sourceCode != null)
            {
                var summaries = ExtractSummaries(sourceCode);
                summariesDictionary[fileName] = summaries;
            }
        });
        await Task.WhenAll(downloadTasks);
        return summariesDictionary;
    }

    private static async Task<string?> DownloadFileAsync(string fileName)
    {
        var url = $"https://raw.githubusercontent.com/NetCordDev/NetCord/alpha/NetCord/Rest/{fileName}";
        Console.WriteLine(url);
        try
        {
            var content = await httpClient.GetStringAsync(url);
            return content;
        }
        catch (HttpRequestException httpRequestException) when (httpRequestException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine($"File not found: {url}");
            return null;
        }
    }

    private static ConcurrentDictionary<string, string> ExtractSummaries(string sourceCode)
    {
        var result = new ConcurrentDictionary<string, string>();

        // Source parsed to a syntax tree
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        // Root node of the tree
        var rootNode = syntaxTree.GetRoot();

        // Find and process all of the properties in the tree
        var propertyDeclarations = rootNode.DescendantNodes().OfType<PropertyDeclarationSyntax>();
        foreach (var propertyDeclaration in propertyDeclarations)
        {
            // Get the leading trivia associated with the property
            var sourceCodeSummary = propertyDeclaration.GetLeadingTrivia()

                // Select the structure
                .Select(trivia => trivia.GetStructure())

                // Filter by comment trivia
                .OfType<DocumentationCommentTriviaSyntax>()

                // Flatten comments to single sequence
                .SelectMany(documentation => documentation.Content.OfType<XmlElementSyntax>())

                // Filter Xml by "summary" element
                .Where(element => element.StartTag.Name.ToString() == "summary")

                // Clean excess comment syntax and format/adjust its contents
                .Select(element => Housekeeping.ParseXmlComment(element.Content.ToString()))
                .FirstOrDefault();

            // Add the processed summary string to the master dictionary
            if (!string.IsNullOrEmpty(sourceCodeSummary))
            {
                result[propertyDeclaration.Identifier.Text] = sourceCodeSummary;
                Console.WriteLine(sourceCodeSummary); // Temporary
            }
        }

        return result;
    }
}
