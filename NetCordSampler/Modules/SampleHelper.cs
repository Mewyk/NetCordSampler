using System.Collections.Immutable;
using System.Text.Json;

using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

using NetCordSampler.CodeSamples;

namespace NetCordSampler.Modules;

public static class SampleHelper
{
    private static readonly ImmutableList<SampleObject> Samples;
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public class SampleObject
    {
        public string Namespace { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; // Short description derived from xml summary
        public string SourceSummary { get; set; } = string.Empty; // Summary from source code xml
        public DateTimeOffset? LastUpdated { get; set; } // Object data last updated time
        public Limitation? Limitations { get; set; }
        public ImmutableList<Property>? Properties { get; set; } = [];

        public class Property
        {
            public string Name { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty; // Short description derived from xml summary
            public DateTimeOffset? LastUpdated { get; set; } // Property data last updated time
            public Limitation? Limitations { get; set; }
        }

        public class Limitation
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
            public int Min { get; set; }
            public int Max { get; set; }
        }
    }

    public static InteractionMessageProperties CreateQuickBuildMessage(
        string sampleSelection, SamplerSettings settings, SlashCommandContext context)
    {
        var sample = Samples.FirstOrDefault(sample =>
            sample.Name.Equals(sampleSelection, StringComparison.OrdinalIgnoreCase)) 
            ?? throw new ArgumentException($"{sampleSelection} not found");

        var fullTypeName = $"{sample.Namespace}.{sample.Name}";

        // Untested
        var type = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .FirstOrDefault(type => type.FullName == fullTypeName) 
            ?? throw new InvalidOperationException(
                $"{sample.Name} not in namespace {sample.Namespace}");

        /*var type = Type.GetType($"{sample.Namespace}.{sample.Name}") 
            ?? throw new InvalidOperationException(
                $"{sample.Name} not in namespace {sample.Namespace}");*/

        var netcordObject = Activator.CreateInstance(type) 
            ?? throw new InvalidOperationException(
                $"Failed to create an instance of {type.FullName}");

        string codeSample = Builder.BuildCodeSample(netcordObject, indent: 4);

        return new InteractionMessageProperties()
            .WithContent($"```CSharp\n{codeSample}\n```")               
            .AddEmbeds(new EmbedProperties()
            {

            });
    }

    static SampleHelper()
    {
        var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "SamplerObjects.json");

        if (!File.Exists(jsonFilePath))
            throw new FileNotFoundException($"{jsonFilePath} was not found");

        var jsonContent = File.ReadAllText(jsonFilePath);
        var sampleList = JsonSerializer.Deserialize<ImmutableList<SampleObject>>(jsonContent, jsonOptions) ?? [];

        Samples = sampleList;
    }

    public static ImmutableList<SampleObject> FindSamples(
        string value, int skip, int limit, out int total)
    {
        ArgumentNullException.ThrowIfNull(value);

        var filtered = Samples
            .Where(sample => sample.Name.Contains(
                value, StringComparison.OrdinalIgnoreCase))
            .OrderBy(sample => sample.Name)
            .ToImmutableList();

        total = filtered.Count;

        return filtered.Skip(skip).Take(limit).ToImmutableList();
    }
}
