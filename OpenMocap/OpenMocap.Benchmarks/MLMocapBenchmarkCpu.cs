using BenchmarkDotNet.Attributes;
using Microsoft.ML.OnnxRuntime;
using OpenMocap.CoreServices.Services.Impl;
using OpenMocap.ML.Services;
using OpenMocap.ML.Services.Locators;

namespace OpenMocap.Benchmarks
{
    public class MLMocapBenchmarkCpu
    {
        private static MocapService _mocapService;
        private static PngImagesPipeParser _pngImagesPipeParser;
        private static byte[] _pngBytes;

        [GlobalSetup]
        public void Setup()
        {
            var envService = new EnvService();
            var options = new OnnxRuntimeOptionProvider();
            if (IsGpu)
            {
                options.SessionOptions = SessionOptions
                        .MakeSessionOptionWithCudaProvider(GpuDevice);
            }

            var sessionBuilder = new OnnxSessionBuilder(envService, options);
            var humanBoxLocator = new HumanBoxLocator(sessionBuilder);
            var humanKeysLocator = new HumanKeysLocator(sessionBuilder);
            _mocapService = new(humanBoxLocator, humanKeysLocator);
            _pngImagesPipeParser = new();
            _pngBytes = File.ReadAllBytes("raw_images_pipe.bin");
        }

        protected virtual bool IsGpu { get; } = true;
        protected virtual int GpuDevice { get; } = 0;


        [Benchmark]
        public Task MocapVideo()
        {
            var images = _pngImagesPipeParser.GetImages(_pngBytes);
            foreach (var image in images)
            {
                var data = _mocapService.Run(image);
            }

            return Task.CompletedTask;
        }
    }
}
