using NetCord.Rest;

using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedFieldPropertiesSample : ICodeSample<EmbedFieldProperties>
{
    public EmbedFieldProperties CreateDefault(SamplerSettings settings) => new()
    {
        Name = settings.DefaultValues.MissingTitle,
        Value = settings.DefaultValues.MissingDescription,
        Inline = false
    };

    public bool IsEmpty(EmbedFieldProperties field) => 
        string.IsNullOrEmpty(field.Name) &&
        string.IsNullOrEmpty(field.Value) &&
        !field.Inline;
}
