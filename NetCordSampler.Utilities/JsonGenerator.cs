using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace NetCordSampler.Utilities;

public static class JsonGenerator
{
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static string GenerateJson(List<SampleObject> sampleObjects) => 
        JsonSerializer.Serialize(sampleObjects, jsonOptions);
}

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