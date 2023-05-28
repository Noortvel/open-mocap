using Microsoft.ML.OnnxRuntime;

namespace OpenMocap.ML.Services
{
    public class OnnxRuntimeOptionProvider : IDisposable
    {
        public SessionOptions? SessionOptions { get; set; }

        public void Dispose()
        {
            SessionOptions?.Dispose();
        }
    }
}
