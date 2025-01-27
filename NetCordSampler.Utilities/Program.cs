using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NetCordSampler.Utilities;

internal static class Program
{
    private static readonly HttpClient httpClient = new();

    public static async Task Main()
    {
        var fileNamesPath = Path.Combine(AppContext.BaseDirectory, "NetcordRestSourceFiles.txt");
        var enumsOutputPath = Path.Combine(AppContext.BaseDirectory, "NetCordRestEnumeration.cs");
        var collectionsOutputPath = Path.Combine(AppContext.BaseDirectory, "NetCordRestImmutable.cs");

        var fileNames = await File.ReadAllLinesAsync(fileNamesPath);
        var cleanedData = await GetCleanSummariesAsync(fileNames);

        // Enum
        var enumSourceCode = SourceGenerator.GenerateEnum(cleanedData);
        File.WriteAllText(enumsOutputPath, enumSourceCode);

        // Immutable
        var immutableSourceCode = SourceGenerator.GenerateImmutableSourceCode(cleanedData);
        File.WriteAllText(collectionsOutputPath, immutableSourceCode);
    }

    private static async Task<ImmutableDictionary<string, ImmutableDictionary<string, PropertyInformation>>> GetCleanSummariesAsync(IEnumerable<string> fileNames)
    {
        var netcordSourceFiles = fileNames.Select(async fileName =>
        {
            var sourceCode = await DownloadFileAsync(fileName);
            if (sourceCode != null)
                return KeyValuePair.Create(
                    Housekeeping.CleanFileName(fileName),
                    ExtractSummaries(sourceCode));
            else return default;
        });

        var results = await Task.WhenAll(netcordSourceFiles);

        var summariesDictionary = results
            .Where(result => !string.IsNullOrEmpty(result.Key) && result.Value != null && result.Value.Count > 0)
            .ToImmutableDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => keyValuePair.Value,
                StringComparer.OrdinalIgnoreCase);

        return summariesDictionary;
    }

    private static async Task<string?> DownloadFileAsync(string fileName)
    {
        var url = $"https://raw.githubusercontent.com/NetCordDev/NetCord/alpha/NetCord/Rest/{fileName}";
        try
        {
            return await httpClient.GetStringAsync(url);
        }
        catch (HttpRequestException httpRequestException)
        when (httpRequestException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine($"File not found: {url}");
            return null;
        }
    }

    private static ImmutableDictionary<string, PropertyInformation> ExtractSummaries(string sourceCode)
    {
        var builder = ImmutableDictionary.CreateBuilder<string, PropertyInformation>();

        // Source parsed to a syntax tree
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        // Root node of the tree
        var rootNode = syntaxTree.GetRoot();

        // Find and process all of the properties in the tree
        var propertyDeclarations = rootNode.DescendantNodes().OfType<PropertyDeclarationSyntax>();
        foreach (var propertyDeclaration in propertyDeclarations)
        {
            // Extract the summary comment
            var summaryText = propertyDeclaration

            // Get the leading trivia
            .GetLeadingTrivia()

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

            if (!string.IsNullOrEmpty(summaryText))
            {
                var propertyInformation = new PropertyInformation
                {
                    Summary = summaryText,
                    Type = propertyDeclaration.Type.ToString(),
                    AccessModifier = propertyDeclaration.Modifiers
                        .FirstOrDefault(modifier => modifier.IsKind(SyntaxKind.PublicKeyword) ||
                            modifier.IsKind(SyntaxKind.PrivateKeyword) ||
                            modifier.IsKind(SyntaxKind.ProtectedKeyword) ||
                            modifier.IsKind(SyntaxKind.InternalKeyword))
                        .ToString(),
                    IsStatic = propertyDeclaration.Modifiers.Any(SyntaxKind.StaticKeyword),
                    IsNullable = propertyDeclaration.Type is NullableTypeSyntax,
                    TypeIdentifierName = propertyDeclaration.Type is IdentifierNameSyntax identifierName
                        ? identifierName.Identifier.Text
                        : propertyDeclaration.Type.ToString(),
                    GenericArguments = propertyDeclaration.Type is GenericNameSyntax genericName
                        ? genericName.TypeArgumentList.Arguments.Select(arg => arg.ToString()).ToList()
                        : [],
                    IsVirtual = propertyDeclaration.Modifiers.Any(SyntaxKind.VirtualKeyword),
                    IsAbstract = propertyDeclaration.Modifiers.Any(SyntaxKind.AbstractKeyword),
                    IsOverride = propertyDeclaration.Modifiers.Any(SyntaxKind.OverrideKeyword),
                    DeclaringType = propertyDeclaration.Parent is ClassDeclarationSyntax classDecl
                        ? classDecl.Identifier.Text
                        : string.Empty,
                    IsRequired = propertyDeclaration.Modifiers.Any(SyntaxKind.RequiredKeyword)
                };

                // Extract attributes
                foreach (var attributeList in propertyDeclaration.AttributeLists)
                    foreach (var attribute in attributeList.Attributes)
                        propertyInformation.Attributes.Add(attribute.Name.ToString());

                if (!builder.ContainsKey(propertyDeclaration.Identifier.Text))
                    builder[propertyDeclaration.Identifier.Text] = propertyInformation;
            }
        }

        return builder.ToImmutable();
    }
}
