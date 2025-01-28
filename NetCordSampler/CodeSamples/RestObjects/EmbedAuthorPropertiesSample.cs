using NetCord.Rest;

using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedAuthorPropertiesSample : ICodeSample<EmbedAuthorProperties>
{
    public EmbedAuthorProperties CreateDefault(SamplerSettings settings) => new()
    {
        Name = settings.DefaultValues.MissingTitle,
        IconUrl = settings.DefaultValues.Urls.Thumbnail,
        Url = settings.DefaultValues.Urls.Website
    };

    public bool IsEmpty(EmbedAuthorProperties author) => 
        string.IsNullOrEmpty(author.Name) &&
        string.IsNullOrEmpty(author.IconUrl) &&
        string.IsNullOrEmpty(author.Url);
}
