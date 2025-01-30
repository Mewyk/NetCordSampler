using NetCord.Rest;

using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedPropertiesSample : ICodeSample<EmbedProperties>
{
    public EmbedProperties CreateDefault(SamplerSettings settings) => new()
    {
        Title = settings.DefaultValues.MissingTitle,
        Description = settings.DefaultValues.MissingDescription,
        Color = new(0xFF00FF), 
        Timestamp = DateTimeOffset.UtcNow,
        Image = settings.DefaultValues.Urls.Image,
        Thumbnail = settings.DefaultValues.Urls.Thumbnail,
        Url = settings.DefaultValues.Urls.Website,
        Author = new EmbedAuthorPropertiesSample().CreateDefault(settings),
        Footer = new EmbedFooterPropertiesSample().CreateDefault(settings),
        Fields =
        [
            new EmbedFieldPropertiesSample().CreateDefault(settings),
            new EmbedFieldPropertiesSample().CreateDefault(settings)
        ]
    };

    public bool IsEmpty(EmbedProperties embed) =>
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
}
