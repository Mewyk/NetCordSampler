using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using NetCord;

namespace NetCordSampler.CodeSamples;

public static class Builder
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = new();

    public static T CreateCustom<T>(Action<T> configure, Func<T, bool> isEmpty) where T : new()
    {
        var instance = new T();
        configure(instance);

        if (isEmpty(instance))
            throw new ArgumentException("Requires at least one property to be set.");

        return instance;
    }

    public static string BuildCodeSample<T>(T netcordObject, int indent = 0)
    {
        ArgumentNullException.ThrowIfNull(netcordObject);

        var type = netcordObject.GetType();
        var stringBuilder = new StringBuilder();
        var indentString = new string(' ', indent);

        stringBuilder.AppendLine($"{indentString}new {type.Name}()");
        stringBuilder.AppendLine($"{indentString}{{");

        var properties = GetProperties(type);
        var nonNullProps = properties.Where(property => property.GetValue(netcordObject) is not null).ToList();
        for (int iterator = 0; iterator < nonNullProps.Count; iterator++)
        {
            var property = nonNullProps[iterator];
            var value = property.GetValue(netcordObject);
            string formattedValue = FormatValue(value!, indent + 4);
            var comma = iterator == nonNullProps.Count - 1 ? "" : ",";
            stringBuilder.AppendLine($"{indentString}    {property.Name} = {formattedValue}{comma}");
        }

        stringBuilder.Append($"{indentString}}}");
        return stringBuilder.ToString();
    }

    private static PropertyInfo[] GetProperties(Type targetType) =>
        PropertyCache.GetOrAdd(targetType, type =>
            type.GetProperties(BindingFlags.Public | BindingFlags.Instance));

    private static string FormatValue(object value, int indent)
    {
        return value switch
        {
            string str => $"\"{str}\"",
            DateTimeOffset offset => $"DateTimeOffset.Parse(\"{offset:O}\")",
            Color color => $"new({color.RawValue})",
            IEnumerable<object> collection => FormatCollection(collection, indent),
            var netcordObject when netcordObject.GetType().IsClass && netcordObject.GetType().Namespace?.StartsWith("NetCord") == true
                => BuildCodeSample(netcordObject, indent),
            _ => value.ToString() ?? string.Empty
        };
    }

    private static string FormatCollection(IEnumerable<object> collection, int indent)
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
