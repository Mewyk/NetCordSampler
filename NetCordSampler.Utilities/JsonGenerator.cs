using System.Text.Json.Serialization;
using System.Text.Json;

namespace NetCordSampler.Utilities;

public static class JsonGenerator
{
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static string GenerateJson(List<SampleClass> sampleObjects) => 
        JsonSerializer.Serialize(sampleObjects, jsonOptions);
}
