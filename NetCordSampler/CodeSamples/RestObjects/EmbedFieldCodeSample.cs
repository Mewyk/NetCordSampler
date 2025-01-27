using NetCord.Rest;
using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples.RestObjects;

public class EmbedFieldCodeSample : ICodeSample
{
    private static readonly Func<EmbedFieldProperties> DefaultField = () => new EmbedFieldProperties
    {
        Name = "Field Name",
        Value = "Field Value",
        Inline = true
    };

    public string BuildCodeSample(object netcordObject)
        => Builder.BuildCodeSample(netcordObject);

    public static EmbedFieldProperties GetDefault()
        => DefaultField();

    public string QuickBuild(Type type)
    {
        if (type != typeof(EmbedFieldProperties))
            throw new ArgumentException($"Unsupported type: {type.Name}");

        return Builder.BuildCodeSample(DefaultField());
    }
}