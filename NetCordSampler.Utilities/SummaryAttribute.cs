namespace NetCordSampler.Utilities;

[AttributeUsage(AttributeTargets.Field)]
public class SummaryAttribute(string description) : Attribute
{
    public string Description { get; } = description;
}