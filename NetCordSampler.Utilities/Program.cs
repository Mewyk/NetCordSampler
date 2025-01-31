namespace NetCordSampler.Utilities;

internal static class Program
{
    public static async Task Main()
    {
        var fileNamesPath = Path.Combine(AppContext.BaseDirectory, "NetcordRestSourceFiles.txt");
        var enumsOutputPath = Path.Combine(AppContext.BaseDirectory, "NetCordRestEnumeration.cs");
        var collectionsOutputPath = Path.Combine(AppContext.BaseDirectory, "NetCordRestImmutable.cs");
        var jsonOutputPath = Path.Combine(AppContext.BaseDirectory, "SamplerObjects.json");
        var samplesOutput = Path.Combine(AppContext.BaseDirectory, "CodeSamples/Rest");

        if (!Directory.Exists(samplesOutput))
            Directory.CreateDirectory(samplesOutput);

        var fileNames = await File.ReadAllLinesAsync(fileNamesPath);
        var sampleClasses = await SourceHelper.GetSampleObjectsAsync(fileNames);

        // Generate Json
        var jsonContent = JsonGenerator.GenerateJson([.. sampleClasses]);
        await File.WriteAllTextAsync(jsonOutputPath, jsonContent);

        // Generate Enums
        var enumSourceCode = SourceBuilder.GenerateEnum(sampleClasses);
        File.WriteAllText(enumsOutputPath, enumSourceCode);

        // Generate Immutable
        var immutableSourceCode = SourceBuilder.GenerateImmutableSourceCode(sampleClasses);
        File.WriteAllText(collectionsOutputPath, immutableSourceCode);

        // Generate CodeSamples
        SourceBuilder.GenerateSampleSourceCode(sampleClasses, samplesOutput);
    }
}
