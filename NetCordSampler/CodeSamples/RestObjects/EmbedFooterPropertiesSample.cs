using NetCord.Rest;

using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedFooterPropertiesSample : ICodeSample<EmbedFooterProperties>
{
    public EmbedFooterProperties CreateDefault(SamplerSettings settings) => new()
    {
        Text = settings.DefaultValues.MissingDescription,
        IconUrl = settings.DefaultValues.Urls.Thumbnail
    };

    public bool IsEmpty(EmbedFooterProperties footer) => 
        string.IsNullOrEmpty(footer.Text) &&
        string.IsNullOrEmpty(footer.IconUrl);
}
