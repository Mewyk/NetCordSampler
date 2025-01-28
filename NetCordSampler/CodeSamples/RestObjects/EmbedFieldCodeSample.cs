using NetCord.Rest;
using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedFieldCodeSample : ICodeSample<EmbedFieldProperties>
{
    public static EmbedFieldProperties CreateDefault() => new()
    {
        Name = "Default Field Name",
        Value = "Default Field Value",
        Inline = false
    };

    public string QuickBuild() => BuildCodeSample(CreateDefault());

    public static EmbedFieldProperties CreateCustom(Action<EmbedFieldProperties> configuration) =>
        Builder.CreateCustom(configuration, IsEmpty);

    private static bool IsEmpty(EmbedFieldProperties field) =>
        string.IsNullOrEmpty(field.Name) &&
        string.IsNullOrEmpty(field.Value) &&
        !field.Inline;

    public string BuildCodeSample(EmbedFieldProperties netcordObject) =>
        Builder.BuildCodeSample(netcordObject);
}
