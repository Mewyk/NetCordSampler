using Microsoft.Extensions.Options;

using NetCord.Rest;

using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedAuthorCodeSample(
    IOptions<SamplerSettings> settings) : ICodeSample<EmbedAuthorProperties>
{
    private readonly SamplerSettings _settings = settings.Value;

    public static EmbedAuthorProperties CreateDefault(SamplerSettings samplerSettings) => new()
    {
        Name = samplerSettings.DefaultValues.MissingTitle,
        IconUrl = samplerSettings.DefaultValues.Urls.Thumbnail,
        Url = samplerSettings.DefaultValues.Urls.Website
    };

    public string QuickBuild() => BuildCodeSample(CreateDefault(_settings));

    public static EmbedAuthorProperties CreateCustom(Action<EmbedAuthorProperties> configuration) =>
        Builder.CreateCustom(configuration, IsEmpty);

    private static bool IsEmpty(EmbedAuthorProperties author) =>
        string.IsNullOrEmpty(author.Name) &&
        string.IsNullOrEmpty(author.IconUrl) &&
        string.IsNullOrEmpty(author.Url);

    public string BuildCodeSample(EmbedAuthorProperties netcordObject) =>
        Builder.BuildCodeSample(netcordObject);
}
