using Microsoft.Extensions.Configuration;
using NetCord.Rest;
using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedFooterCodeSample : ICodeSample<EmbedFooterProperties>
{
    public static EmbedFooterProperties CreateDefault() => new()
    {
        Text = "Default Footer Text",
        IconUrl = "https://example.com/default-footer-icon.png"
    };

    public string QuickBuild() => BuildCodeSample(CreateDefault());

    public static EmbedFooterProperties CreateCustom(Action<EmbedFooterProperties> configuration) =>
        Builder.CreateCustom(configuration, IsEmpty);

    private static bool IsEmpty(EmbedFooterProperties footer) =>
        string.IsNullOrEmpty(footer.Text) &&
        string.IsNullOrEmpty(footer.IconUrl);

    public string BuildCodeSample(EmbedFooterProperties netcordObject) =>
        Builder.BuildCodeSample(netcordObject);
}
