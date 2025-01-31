using System.Collections.Immutable;

namespace NetCordSampler.Utilities;

public class SampleClass
{
    public string FullName =>
        string.IsNullOrEmpty(Namespace)
            ? Name
            : $"{Namespace}.{Name}";

    public string Namespace { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SourceSummary { get; set; } = string.Empty;
    public string Accessibility { get; set; } = string.Empty;
    public DateTimeOffset? LastUpdated { get; set; }
    public ImmutableList<Limitation>? Limitations { get; set; } = [];
    public ImmutableList<Property>? Properties { get; set; } = [];

    public class Property
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsStatic { get; set; } // New property to indicate static properties
        public string Description { get; set; } = string.Empty;
        public string SourceSummary { get; set; } = string.Empty;
        public string Accessibility { get; set; } = string.Empty;
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
