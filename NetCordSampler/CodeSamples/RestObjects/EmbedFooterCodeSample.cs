using NetCord.Rest;
using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedFooterCodeSample : ICodeSample
{
    private static readonly Func<EmbedFooterProperties> DefaultFooter = () => new EmbedFooterProperties
    {
        Text = "Example Footer",
        IconUrl = "https://example.com/footer.png"
    };

    public string BuildCodeSample(object netcordObject)
        => Builder.BuildCodeSample(netcordObject);

    public static EmbedFooterProperties GetDefault()
        => DefaultFooter();

    public string QuickBuild(Type type)
    {
        if (type != typeof(EmbedFooterProperties))
            throw new ArgumentException($"Unsupported type: {type.Name}");

        return Builder.BuildCodeSample(DefaultFooter());
    }
}
