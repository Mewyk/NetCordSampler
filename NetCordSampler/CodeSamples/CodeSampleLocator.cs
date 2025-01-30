using System.Reflection;

using NetCordSampler.Interfaces;

namespace NetCordSampler.CodeSamples;

public static class CodeSampleLocator
{
    private static readonly Dictionary<Type, object> _codeSamples = [];

    static CodeSampleLocator()
    {
        var sampleTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(
                type => !type.IsAbstract
                && type.GetInterfaces().Any(
                    interfaceType => interfaceType.IsGenericType
                    && interfaceType.GetGenericTypeDefinition() == typeof(ICodeSample<>)))
            .ToList();

        foreach (var type in sampleTypes)
        {
            var interfaces = type.GetInterfaces().Where(
                interfaceType => interfaceType.IsGenericType
                && interfaceType.GetGenericTypeDefinition() == typeof(ICodeSample<>));

            foreach (var interfaceItem in interfaces)
            {
                var targetType = interfaceItem.GetGenericArguments()[0];
                var instance = Activator.CreateInstance(type);
                if (instance != null)
                    _codeSamples[targetType] = instance;
            }
        }
    }

    public static ICodeSample<T>? GetCodeSample<T>() where T : class => 
        _codeSamples.TryGetValue(typeof(T), out var codeSample)
            ? codeSample as ICodeSample<T>
            : null;
}
