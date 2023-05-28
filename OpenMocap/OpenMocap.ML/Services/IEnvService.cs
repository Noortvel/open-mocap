using System.Reflection;

namespace OpenMocap.ML.Services
{
    public interface IEnvService
    {
        string GetPath()
            => Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ??
            throw new InvalidOperationException();

        string GetCombinedPath(string combine)
            => Path.Combine(GetPath(), combine);
    }
}
