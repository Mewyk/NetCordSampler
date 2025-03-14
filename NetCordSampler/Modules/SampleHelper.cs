﻿using System.Collections.Immutable;
using System.Text.Json;
using NetCord.Rest;

using NetCordSampler.CodeSamples;

namespace NetCordSampler.Modules;

public static class SampleHelper
{
    public static readonly ImmutableList<SampleObject> Samples;
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public class SampleObject
    {
        public string FullName =>
            string.IsNullOrEmpty(Namespace)
            ? Name
            : $"{Namespace}.{Name}";

        public string Namespace { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SourceSummary { get; set; } = string.Empty;
        public DateTimeOffset? LastUpdated { get; set; }
        public ImmutableList<Limitation>? Limitations { get; set; } = [];
        public ImmutableList<Property>? Properties { get; set; } = [];

        public class Property
        {
            public string Name { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string SourceSummary { get; set; } = string.Empty;
            public DateTimeOffset? LastUpdated { get; set; }
            public ImmutableList<Limitation>? Limitations { get; set; } = [];
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
        string sampleSelection, Builder builder, SamplerSettings settings)
    {
        string codeSample = builder.QuickBuild(sampleSelection);

        return new InteractionMessageProperties()
            .WithContent($"```CSharp\n{codeSample}\n```");
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

    public static IEnumerable<string> FindSamples(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return Samples
            .Select(s => s.Name)
            .Where(n => n.Contains(value, StringComparison.OrdinalIgnoreCase))
            .OrderBy(n => n.IndexOf(value, StringComparison.OrdinalIgnoreCase))
            .ThenBy(n => n);
    }
}
