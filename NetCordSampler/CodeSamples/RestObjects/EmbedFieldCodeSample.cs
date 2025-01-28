using Microsoft.Extensions.Options;

using NetCord.Rest;

using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedFieldCodeSample(
    IOptions<SamplerSettings> settings) : ICodeSample<EmbedFieldProperties>
{
    private readonly SamplerSettings _settings = settings.Value;

    public static EmbedFieldProperties CreateDefault(SamplerSettings samplerSettings) => new()
    {
        Name = samplerSettings.DefaultValues.MissingTitle,
        Value = samplerSettings.DefaultValues.MissingDescription,
        Inline = false
    };

    public string QuickBuild() => BuildCodeSample(CreateDefault(_settings));

    public static EmbedFieldProperties CreateCustom(Action<EmbedFieldProperties> configuration) =>
        Builder.CreateCustom(configuration, IsEmpty);

    private static bool IsEmpty(EmbedFieldProperties field) =>
        string.IsNullOrEmpty(field.Name) &&
        string.IsNullOrEmpty(field.Value) &&
        !field.Inline;

    public string BuildCodeSample(EmbedFieldProperties netcordObject) =>
        Builder.BuildCodeSample(netcordObject);
}
