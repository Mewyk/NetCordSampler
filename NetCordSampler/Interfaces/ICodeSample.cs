namespace NetCordSampler.Interfaces;

public interface ICodeSample<T> where T : class
{
    string BuildCodeSample(T netcordObject);
    string QuickBuild();
}
