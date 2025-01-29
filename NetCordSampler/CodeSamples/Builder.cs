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

    public string CustomBuild<T>(Action<T> configure) where T : class, new()
    {
        var netcordObject = new T();
        configure(netcordObject);

        var codeSample = CodeSampleLocator.GetCodeSample<T>()
            ?? throw new InvalidOperationException(
                $"No ICodeSample<{typeof(T).Name}> found for type {typeof(T).Name}");

        if (codeSample.IsEmpty(netcordObject))
            throw new ArgumentException("Requires at least one property to be set");

        return BuildCodeSample(netcordObject);
    }

    public static T CreateCustom<T>(Action<T> configure, Func<T, bool> isEmpty) where T : new()
    {
        var instance = new T();
        configure(instance);

        if (isEmpty(instance))
            throw new ArgumentException("Requires at least one property to be set");

        return instance;
    }

    public string BuildCodeSample<T>(T netcordObject, int indent = 0)
    {
        ArgumentNullException.ThrowIfNull(netcordObject);

        var type = netcordObject.GetType();
        var stringBuilder = new StringBuilder();
        var indentString = new string(' ', indent);

        stringBuilder.AppendLine($"new {type.Name}()");
        stringBuilder.AppendLine($"{indentString}{{");

        var properties = GetProperties(type);
        var nonDefaultProps = properties.Where(property =>
        {
            var value = property.GetValue(netcordObject);
            if (value is null)
                return false;

            if (property.PropertyType.IsValueType)
                return !value.Equals(Activator.CreateInstance(property.PropertyType));

            return true;
        }).ToList();

        for (int iterator = 0; iterator < nonDefaultProps.Count; iterator++)
        {
            var property = nonDefaultProps[iterator];
            var value = property.GetValue(netcordObject);
            string formattedValue = FormatValue(value!, indent + 4);
            var comma = iterator == nonDefaultProps.Count - 1 ? "" : ",";
            stringBuilder.AppendLine($"{indentString}    {property.Name} = {formattedValue}{comma}");
        }

        stringBuilder.Append($"{indentString}}}");
        return stringBuilder.ToString();
    }

    private static PropertyInfo[] GetProperties(Type targetType) =>
        PropertyCache.GetOrAdd(targetType, type =>
            type.GetProperties(BindingFlags.Public | BindingFlags.Instance));

    private string FormatValue(object value, int indent)
    {
        return value switch
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
    }

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
