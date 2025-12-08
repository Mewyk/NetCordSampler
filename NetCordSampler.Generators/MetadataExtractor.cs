using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NetCordSampler.Generators;

/// <summary>
/// Extracts metadata from NetCord types and generates SamplerTypes.json.
/// </summary>
[Generator]
public class MetadataExtractor : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // This generator creates the initial SamplerTypes.json file
        // It will scan NetCord assemblies and extract type information
        
        context.RegisterPostInitializationOutput(ctx =>
        {
            // We'll generate the JSON file in a later step
            // For now, just set up the infrastructure
        });
    }

    private static string ExtractXmlDocumentation(ISymbol symbol)
    {
        var xml = symbol.GetDocumentationCommentXml();
        if (string.IsNullOrEmpty(xml))
            return string.Empty;

        // Simple extraction of <summary> content
        var summaryStart = xml!.IndexOf("<summary>", StringComparison.Ordinal);
        var summaryEnd = xml.IndexOf("</summary>", StringComparison.Ordinal);
        
        if (summaryStart >= 0 && summaryEnd > summaryStart)
        {
            var summary = xml.Substring(summaryStart + 9, summaryEnd - summaryStart - 9);
            return summary.Trim().Replace("\n", " ").Replace("  ", " ");
        }

        return string.Empty;
    }
}
