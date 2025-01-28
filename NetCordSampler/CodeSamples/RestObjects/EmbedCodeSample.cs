using NetCord;
using NetCord.Rest;
using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedCodeSample : ICodeSample<EmbedProperties>
{
    public static EmbedProperties CreateDefault() => new()
    {
        Title = "Default Title",
        Description = "Default Description",
        Color = new(0xFF00FF),
        Timestamp = DateTimeOffset.UtcNow,
        Image = "https://example.com/default-image.png",
        Thumbnail = "https://example.com/default-thumbnail.png",
        Url = "https://example.com",
        Author = EmbedAuthorCodeSample.CreateDefault(),
        Footer = EmbedFooterCodeSample.CreateDefault(),
        Fields =
        [
            EmbedFieldCodeSample.CreateDefault(),
            EmbedFieldCodeSample.CreateDefault()
        ]
    };

    public string QuickBuild() => BuildCodeSample(CreateDefault());

    public static EmbedProperties CreateCustom(Action<EmbedProperties> configuration) =>
        Builder.CreateCustom(configuration, IsEmpty);

    private static bool IsEmpty(EmbedProperties embed)
    {
        //Console.WriteLine(embed.Color);
        Console.WriteLine(embed.Color.RawValue);
        return string.IsNullOrEmpty(embed.Title) &&
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

    public string BuildCodeSample(EmbedProperties netcordObject) =>
        Builder.BuildCodeSample(netcordObject);
}
