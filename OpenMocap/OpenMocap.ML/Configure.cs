using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML.OnnxRuntime;
using OpenMocap.ML.Services;
using OpenMocap.ML.Services.Locators;

namespace OpenMocap.ML
{
    public static class Configure
    {
        public static IServiceCollection AddMLServices(
           this IServiceCollection services,
           Action<MlOptions>? configure = null)
        {
            var provider = new OnnxRuntimeOptionProvider();
            var mlOptions = new MlOptions();
            if(configure == null)
            {
                mlOptions.OnnxRuntimeType = OnnxRuntimeType.Cpu;
                provider.SessionOptions = new SessionOptions();
            }
            else
            {
                configure.Invoke(mlOptions);
                if(mlOptions.OnnxRuntimeType == OnnxRuntimeType.Gpu)
                {
                    var gpuDeviceId = mlOptions.GpuDevice ?? 0;
                    provider.SessionOptions = SessionOptions
                        .MakeSessionOptionWithCudaProvider(gpuDeviceId);
                }
                else
                {
                    provider.SessionOptions = new SessionOptions();
                }
            }

            provider.SessionOptions.GraphOptimizationLevel =
                GraphOptimizationLevel.ORT_ENABLE_ALL;

            services
                .AddSingleton<MocapService>()
                .AddSingleton<HumanBoxLocator>()
                .AddSingleton<HumanKeysLocator>()
                .AddSingleton<IEnvService, EnvService>()
                .AddSingleton<OnnxRuntimeOptionProvider>(provider)
                .AddSingleton<OnnxSessionBuilder>()
                ;

            return services;
        }
    }
}
