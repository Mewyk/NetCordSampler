using NetCord.Rest;
using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedAuthorCodeSample : ICodeSample<EmbedAuthorProperties>
{
    public static EmbedAuthorProperties CreateDefault() => new()
    {
        Name = "Default Author Name",
        IconUrl = "https://example.com/default-author-icon.png",
        Url = "https://example.com/author"
    };

    public string QuickBuild() => BuildCodeSample(CreateDefault());

    public static EmbedAuthorProperties CreateCustom(Action<EmbedAuthorProperties> configuration) =>
        Builder.CreateCustom(configuration, IsEmpty);

    private static bool IsEmpty(EmbedAuthorProperties author) =>
        string.IsNullOrEmpty(author.Name) &&
        string.IsNullOrEmpty(author.IconUrl) &&
        string.IsNullOrEmpty(author.Url);

    public string BuildCodeSample(EmbedAuthorProperties netcordObject) =>
        Builder.BuildCodeSample(netcordObject);
}
