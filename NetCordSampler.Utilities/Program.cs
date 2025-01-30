namespace NetCordSampler.Utilities;

internal static class Program
{
    public static async Task Main()
    {
        var fileNamesPath = Path.Combine(AppContext.BaseDirectory, "NetcordRestSourceFiles.txt");
        var enumsOutputPath = Path.Combine(AppContext.BaseDirectory, "NetCordRestEnumeration.cs");
        var collectionsOutputPath = Path.Combine(AppContext.BaseDirectory, "NetCordRestImmutable.cs");
        var jsonOutputPath = Path.Combine(AppContext.BaseDirectory, "SamplerObjects.json");

        var fileNames = await File.ReadAllLinesAsync(fileNamesPath);
        var sampleObjects = await SourceHelper.GetSampleObjectsAsync(fileNames);

        // Json
        var jsonContent = JsonGenerator.GenerateJson([.. sampleObjects]);
        await File.WriteAllTextAsync(jsonOutputPath, jsonContent);

        // Enums
        var enumSourceCode = SourceBuilder.GenerateEnum(sampleObjects);
        File.WriteAllText(enumsOutputPath, enumSourceCode);

        // Immutable
        var immutableSourceCode = SourceBuilder.GenerateImmutableSourceCode(sampleObjects);
        File.WriteAllText(collectionsOutputPath, immutableSourceCode);
    }
}
