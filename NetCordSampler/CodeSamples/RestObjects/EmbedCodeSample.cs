using Microsoft.Extensions.Options;

using NetCord.Rest;

using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedCodeSample(
    IOptions<SamplerSettings> settings) : ICodeSample<EmbedProperties>
{
    private readonly SamplerSettings _settings = settings.Value;

    public static EmbedProperties CreateDefault(SamplerSettings samplerSettings) => new()
    {
        Title = "Default Title", // samplerSettings.DefaultValues.MissingTitle
        Description = "Default Description", // samplerSettings.DefaultValues.MissingDescription
        Color = new(0xFF00FF),
        Timestamp = DateTimeOffset.UtcNow,
        Image = samplerSettings.DefaultValues.Urls.Image,
        Thumbnail = samplerSettings.DefaultValues.Urls.Thumbnail,
        Url = samplerSettings.DefaultValues.Urls.Website,
        Author = EmbedAuthorCodeSample.CreateDefault(samplerSettings),
        Footer = EmbedFooterCodeSample.CreateDefault(samplerSettings),
        Fields =
        [
            EmbedFieldCodeSample.CreateDefault(samplerSettings),
            EmbedFieldCodeSample.CreateDefault(samplerSettings)
        ]
    };

    public string QuickBuild() => BuildCodeSample(CreateDefault(_settings));

    public static EmbedProperties CreateCustom(Action<EmbedProperties> configuration) =>
        Builder.CreateCustom(configuration, IsEmpty);

    private static bool IsEmpty(EmbedProperties embed) =>
        string.IsNullOrEmpty(embed.Title) &&
        string.IsNullOrEmpty(embed.Description) &&
        string.IsNullOrEmpty(embed.Url) &&
        embed.Timestamp == default &&
        embed.Color.RawValue == 0 &&
        embed.Author == null &&
        embed.Footer == null &&
        embed.Image == null &&
        embed.Thumbnail == null &&
        (embed.Fields == null || !embed.Fields.Any());

    public string BuildCodeSample(EmbedProperties netcordObject) =>
        Builder.BuildCodeSample(netcordObject);
}
