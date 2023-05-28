using Microsoft.ML.OnnxRuntime;

namespace OpenMocap.ML.Services
{
    public class OnnxSessionBuilder
    {
        private readonly IEnvService _envService;
        private readonly OnnxRuntimeOptionProvider _options;

        public OnnxSessionBuilder(
            IEnvService envService,
            OnnxRuntimeOptionProvider options)
        {
            _envService = envService;
            _options = options;
        }

        public InferenceSession Build(ModelType modelType)
        {
            var filename = MapFileName(modelType);
            var path = _envService.GetCombinedPath("Models/" + filename);

            InferenceSession result;
            if (_options.SessionOptions != null)
            {
                //_options.SessionOptions.LogSeverityLevel = OrtLoggingLevel.ORT_LOGGING_LEVEL_VERBOSE;
                //_options.SessionOptions.GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL;
                result = new InferenceSession(path, _options.SessionOptions);
            }
            else
            {
                result = new InferenceSession(path);

            }

            return result;
        }

        private static string MapFileName(ModelType modelType)
            => modelType switch
            {
                ModelType.Yolo5m => "yolov5m.onnx",
                ModelType.HRNet => "pose_hrnet_w32_256x192.onnx",

                _ => throw new InvalidOperationException("Unknown model type")
            };
    }
}
