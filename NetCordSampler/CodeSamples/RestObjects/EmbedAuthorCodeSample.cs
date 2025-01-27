using NetCord.Rest;
using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedAuthorCodeSample : ICodeSample
{
    private static readonly Func<EmbedAuthorProperties> DefaultAuthor = () => new EmbedAuthorProperties
    {
        Name = "Example Author",
        IconUrl = "https://example.com/author.png"
    };

    public string BuildCodeSample(object netcordObject)
        => Builder.BuildCodeSample(netcordObject);

    public static EmbedAuthorProperties GetDefault()
        => DefaultAuthor();

    public string QuickBuild(Type type)
    {
        if (type != typeof(EmbedAuthorProperties))
            throw new ArgumentException($"Unsupported type: {type.Name}");

        return Builder.BuildCodeSample(DefaultAuthor());
    }
}