using Microsoft.Extensions.Options;

using NetCord.Rest;

using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedFooterCodeSample(
    IOptions<SamplerSettings> settings) : ICodeSample<EmbedFooterProperties>
{
    private readonly SamplerSettings _settings = settings.Value;

    public static EmbedFooterProperties CreateDefault(SamplerSettings samplerSettings) => new()
    {
        Text = samplerSettings.DefaultValues.MissingDescription,
        IconUrl = samplerSettings.DefaultValues.Urls.Thumbnail
    };

    public string QuickBuild() => BuildCodeSample(CreateDefault(_settings));

    public static EmbedFooterProperties CreateCustom(Action<EmbedFooterProperties> configuration) =>
        Builder.CreateCustom(configuration, IsEmpty);

    private static bool IsEmpty(EmbedFooterProperties footer) =>
        string.IsNullOrEmpty(footer.Text) &&
        string.IsNullOrEmpty(footer.IconUrl);

    public string BuildCodeSample(EmbedFooterProperties netcordObject) =>
        Builder.BuildCodeSample(netcordObject);
}
