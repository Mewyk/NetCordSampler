namespace NetCordSampler.Interfaces;

public interface ICodeSample<T> where T : class
{
    T CreateDefault(SamplerSettings settings);
    bool IsEmpty(T instance);
}
