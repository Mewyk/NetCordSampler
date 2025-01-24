using NetCord.Rest;
using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.Rest;

public class EmbedCodeSample : ICodeSample
{
    private static readonly Func<EmbedProperties> DefaultEmbed = () => new EmbedProperties
    {
        Title = "Embed Title",
        Description = "Embed Description",
        Color = new(0x7289DA),
        Timestamp = DateTimeOffset.UtcNow,
        Image = "https://example.com/image.png",
        Thumbnail = "https://example.com/thumbnail.png",
        Url = "https://example.com",
        Author = EmbedAuthorCodeSample.GetDefault(),
        Footer = EmbedFooterCodeSample.GetDefault(),
        Fields =
        [
            EmbedFieldCodeSample.GetDefault(),
            EmbedFieldCodeSample.GetDefault()
        ]
    };

    public string BuildCodeSample(object netcordObject)
        => Builder.BuildCodeSample(netcordObject);

    public static EmbedProperties GetDefault() 
        => DefaultEmbed();

    public string QuickBuild(Type type)
    {
        if (type != typeof(EmbedProperties))
            throw new ArgumentException($"Unsupported type: {type.Name}");

        return Builder.BuildCodeSample(DefaultEmbed());
    }
}