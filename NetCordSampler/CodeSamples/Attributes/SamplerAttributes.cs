namespace NetCordSampler.CodeSamples.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SummaryAttribute(string summary) : Attribute
{
    public string Summary { get; } = summary;
}

[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Method | 
    AttributeTargets.Property | AttributeTargets.Field)]
public class SamplerDataAttribute : Attribute
{
    private const string RequiredNamespace = "NetCord";
    public string Description { get; }
    public object NetCordObject { get; }

    public SamplerDataAttribute(object netcordObject, string description)
    {
        NetCordObject = netcordObject;
        Description = description;

        if (netcordObject is Type typeValue)
            if (typeValue.Namespace != RequiredNamespace)
                throw new ArgumentException(
                    $"Invalid namespace'{RequiredNamespace}'", 
                    nameof(netcordObject));
    }

    // TODO: Add actual logic
    public void IdentifyObject()
    {
        Console.WriteLine($"Name: {Description}");
        switch (NetCordObject)
        {
            case int intValue:
                Console.WriteLine($"Integer value: {intValue}");
                break;
            case string stringValue:
                Console.WriteLine($"String value: {stringValue}");
                break;
            case Type typeValue:
                Console.WriteLine($"Type value: {typeValue}");
                break;
            default:
                Console.WriteLine("Unknown type");
                break;
        }
    }

    /* Incomplete
    [AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Method |
    AttributeTargets.Property | AttributeTargets.Field)]
    public class DiscordRulesAttribute(
    string? name,
    string? description,
    string? value,
    string? min,
    string? max) : Attribute
    {
        public string? Name { get; } = name;
        public string? Description { get; } = description;
        public string? Value { get; } = value;
        public string? Min { get; } = min;
        public string? Max { get; } = max;
    } */
}