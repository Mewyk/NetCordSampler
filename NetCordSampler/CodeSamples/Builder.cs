using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

using Microsoft.Extensions.Options;

using NetCord;

namespace NetCordSampler.CodeSamples;

public class Builder(IOptions<Configuration> settings)
{
    private readonly SamplerSettings _settings = settings.Value.Sampler;
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = new();

    public string QuickBuild<T>() where T : class, new()
    {
        var codeSample = CodeSampleLocator.GetCodeSample<T>()
            ?? throw new InvalidOperationException(
                $"No ICodeSample<{typeof(T).Name}> found for type {typeof(T).Name}");

        return BuildCodeSample(
            codeSample.CreateDefault(_settings));
    }

    public string QuickBuild(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
            throw new ArgumentNullException(nameof(typeName));

        var targetType = FindTypeByName(typeName) 
            ?? throw new ArgumentException($"{typeName} not found", nameof(typeName));
        
        if (!targetType.IsClass)
            throw new ArgumentException($"{typeName} is not a class", nameof(typeName));

        if (targetType.GetConstructor(Type.EmptyTypes) == null)
            throw new ArgumentException($"{typeName} does not have a parameterless constructor", nameof(typeName));

        var methodInfo = typeof(Builder).GetMethod(nameof(QuickBuild), Type.EmptyTypes) 
            ?? throw new InvalidOperationException("Method QuickBuild<> not found");
        
        var genericMethod = methodInfo.MakeGenericMethod(targetType);

        return (string?)genericMethod.Invoke(this, null) 
            ?? throw new InvalidOperationException("Returned null");
    }

    private static Type? FindTypeByName(string typeName)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var type = assembly.GetTypes().FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.Ordinal));
            if (type != null)
                return type;
        }
        return null;
    }

    public string CustomBuild<T>(Action<T> configure) where T : class, new()
    {
        var sampleType = new T();
        configure(sampleType);

        var codeSample = CodeSampleLocator.GetCodeSample<T>()
            ?? throw new InvalidOperationException(
                $"No ICodeSample<{typeof(T).Name}> found for type {typeof(T).Name}");

        return codeSample.IsEmpty(sampleType)
            ? throw new ArgumentException("Requires at least one property to be set")
            : BuildCodeSample(sampleType);
    }

    private string BuildCodeSample<T>(T sampleType, int indent = 0)
    {
        ArgumentNullException.ThrowIfNull(sampleType);

        var type = sampleType.GetType();
        var stringBuilder = new StringBuilder();
        var indentString = new string(' ', indent);

        stringBuilder.AppendLine($"new {type.Name}()");
        stringBuilder.AppendLine($"{indentString}{{");

        var properties = GetProperties(type);
        var nonDefaultProps = properties.Where(property =>
        {
            var value = property.GetValue(sampleType);
            if (value is null)
                return false;

            if (property.PropertyType.IsValueType)
                return !value.Equals(Activator.CreateInstance(property.PropertyType));

            return true;
        }).ToList();

        for (int iterator = 0; iterator < nonDefaultProps.Count; iterator++)
        {
            var property = nonDefaultProps[iterator];
            var value = property.GetValue(sampleType);

            // TODO: Handle special formatting elsewhere
            string formattedValue;
            if (property.Name == "Image" || property.Name == "Thumbnail")
            {
                if (value is not null)
                {
                    var urlProperty = value.GetType().GetProperty("Url");
                    var urlValue = urlProperty?.GetValue(value) as string ?? string.Empty;
                    formattedValue = $"\"{urlValue}\"";
                }
                else formattedValue = "null";
            }
            else
            {
                formattedValue = FormatValue(value!, indent + 4);
            }

            var comma = iterator == nonDefaultProps.Count - 1 ? "" : ",";
            stringBuilder.AppendLine($"{indentString}    {property.Name} = {formattedValue}{comma}");
        }

        stringBuilder.Append($"{indentString}}}");
        return stringBuilder.ToString();
    }

    private static PropertyInfo[] GetProperties(Type targetType) =>
        PropertyCache.GetOrAdd(targetType, type =>
            type.GetProperties(BindingFlags.Public | BindingFlags.Instance));

    private string FormatValue(object value, int indent) => value switch
    {
        string stringValue => $"\"{stringValue}\"",
        DateTimeOffset offset => $"DateTimeOffset.Parse(\"{offset:O}\")",
        Color color => $"new({color.RawValue})",
        IEnumerable<object> collection => FormatCollection(collection, indent),
        var netcordObject when netcordObject.GetType().IsClass
            && netcordObject.GetType().Namespace?.StartsWith("NetCord") == true
            => BuildCodeSample(netcordObject, indent),
        _ => value.ToString() ?? string.Empty
    };

    private string FormatCollection(IEnumerable<object> collection, int indent)
    {
        var stringBuilder = new StringBuilder("[\n");
        var items = collection.ToList();

        for (int iterator = 0; iterator < items.Count; iterator++)
        {
            var comma = iterator == items.Count - 1 ? "" : ",";
            stringBuilder.AppendLine(
                $"{new string(' ', indent + 4)}{FormatValue(items[iterator], indent + 4)}{comma}");
        }

        stringBuilder.Append($"{new string(' ', indent)}]");
        return stringBuilder.ToString();
    }
}
