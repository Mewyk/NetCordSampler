namespace NetCordSampler.Interfaces;

public interface ICodeSample
{
    string BuildCodeSample(object netcordObject);
    string QuickBuild(Type type);
}