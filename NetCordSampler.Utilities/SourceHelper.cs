using System.Collections.Immutable;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace NetCordSampler.Utilities;

public static class SourceHelper
{
    private static readonly HttpClient httpClient = new();

    public static async Task<ImmutableArray<SampleObject>> GetSampleObjectsAsync(IEnumerable<string> fileNames)
    {
        var tasks = fileNames.Select(async fileName =>
        {
            var sourceCode = await DownloadFileAsync(fileName);
            if (string.IsNullOrEmpty(sourceCode))
                return null;

            var sampleObject = ExtractSampleObject(sourceCode);
            return sampleObject ?? null;
        });

        var sampleObjects = await Task.WhenAll(tasks);
        return sampleObjects
            .Where(sampleObject => sampleObject != null)
            .ToImmutableArray()!;
    }

    private static async Task<string?> DownloadFileAsync(string fileName)
    {
        var gitUrl = "https://raw.githubusercontent.com/NetCordDev/NetCord/alpha";
        var folder = "NetCord/Rest";

        try
        {
            return await httpClient.GetStringAsync($"{gitUrl}/{folder}/{fileName}");
        }
        catch (HttpRequestException httpException)
        when (httpException.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine($"File not found: {gitUrl}/{folder}/{fileName}");
            return null;
        }
    }

    private static SampleObject? ExtractSampleObject(string sourceCode)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        var rootNode = syntaxTree.GetRoot();
        var typeSyntax = rootNode
            .DescendantNodes()
            .OfType<TypeDeclarationSyntax>()
            .FirstOrDefault();

        if (typeSyntax != null)
            return new SampleObject
            {
                Namespace = GetNamespace(typeSyntax),
                Name = typeSyntax.Identifier.Text,
                Description = GetTypeDescription(typeSyntax), // Temporary
                SourceSummary = GetTypeDescription(typeSyntax), // GetTypeSummary(typeSyntax),
                LastUpdated = DateTimeOffset.UtcNow,
                Limitations = null,
                Properties = GetProperties(typeSyntax)
            };

        Console.WriteLine("Type declaration not found in the source code.");
        return null;
    }

    public static string GetFullTypeName(TypeDeclarationSyntax typeSyntax)
    {
        var namespaces = new Stack<string>();
        var types = new Stack<string>();
        var parent = typeSyntax.Parent;

        types.Push(typeSyntax.Identifier.Text);
        while (parent != null)
        {
            if (parent is NamespaceDeclarationSyntax namespaceSyntax)
                namespaces.Push(namespaceSyntax.Name.ToString());
            else if (parent is FileScopedNamespaceDeclarationSyntax scopedNamespace)
                namespaces.Push(scopedNamespace.Name.ToString());
            else if (parent is TypeDeclarationSyntax containingType)
                types.Push(containingType.Identifier.Text);

            parent = parent.Parent;
        }

        return string.Join(".", namespaces.Concat(types));
    }

    private static string GetNamespace(TypeDeclarationSyntax typeSyntax) =>
        typeSyntax.Parent is BaseNamespaceDeclarationSyntax namespaceSyntax
            ? namespaceSyntax.Name.ToString()
            : string.Empty;

    private static string GetTypeDescription(TypeDeclarationSyntax typeSyntax) =>
        Housekeeping.ParseXmlComment(GetXmlSummary(typeSyntax));

    /*private static string GetTypeSummary(TypeDeclarationSyntax typeSyntax) =>
        GetXmlSummary(typeSyntax);*/

    private static string GetXmlSummary(SyntaxNode member) =>
        member
            .GetLeadingTrivia()
            .Select(trivia => trivia.GetStructure())
            .OfType<DocumentationCommentTriviaSyntax>()
            .SelectMany(documentation => documentation.Content)
            .OfType<XmlElementSyntax>()
            .Where(element => element.StartTag.Name.ToString() == "summary")
            .Select(element => element.Content.ToString())
            .FirstOrDefault() ?? string.Empty;

    private static ImmutableList<SampleObject.Property>? GetProperties(TypeDeclarationSyntax typeSyntax)
    {
        return typeSyntax.Members
            .OfType<PropertyDeclarationSyntax>()
            .Select(propertySyntax =>
            {
                var summaryText = GetXmlSummary(propertySyntax);
                var parsedSummary = Housekeeping.ParseXmlComment(summaryText);

                return new SampleObject.Property
                {
                    Name = propertySyntax.Identifier.Text,
                    Type = propertySyntax.Type.ToString(),
                    Description = parsedSummary, // Temporary
                    SourceSummary = parsedSummary,
                    LastUpdated = DateTimeOffset.UtcNow,
                    Limitations = null
                };
            }).ToImmutableList();
    }
}
