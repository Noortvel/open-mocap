using Microsoft.Extensions.DependencyInjection;
using OpenMocap.CoreServices.Services;
using OpenMocap.CoreServices.Services.Impl;
using OpenMocap.ML;

namespace OpenMocap.CoreServices
{
    public static class Configure
    {
        public static IServiceCollection AddCoreServices(
            this IServiceCollection services)
        {
            services
                .AddSingleton<IVideoRepository, VideoRepository>()
                .AddSingleton<IAsyncVideoFramer, AsyncVideoFramer>()
                .AddSingleton<IImagesRepository, ImagesRepository>()
                .AddSingleton<IFFmpegProcess, FFmpegProcess>()
                .AddSingleton<IVideoMocaperService, VideoMocaperService>()
                .AddSingleton<PngImagesPipeParser>()
                .AddSingleton<IVideoInfoStorage, VideoInfoStorage>()
                .AddSingleton<IVideoProgressMetric, VideoProgressMetric>()
                ;

            return services;
        }
    }
}
