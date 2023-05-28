using OpenMocap.ML;
using OpenMocap.ML.Services;

namespace OpenMocap.CoreServices.Services.Impl
{
    internal class VideoMocaperService : IVideoMocaperService
    {
        private readonly MocapService _mocapService;
        private readonly IAsyncVideoFramer _asyncVideoFramer;
        private readonly PngImagesPipeParser _pngImagesPipeParser;
        private readonly IVideoProgressMetric _videoProgressMetric;
        private readonly IVideoInfoStorage _videoInfoStorage;

        public VideoMocaperService(
            MocapService mocapService,
            IAsyncVideoFramer asyncVideoFramer,
            PngImagesPipeParser pngImagesPipeParser,
            IVideoProgressMetric videoProgressMetric,
            IVideoInfoStorage videoInfoStorage)
        {
            _mocapService = mocapService;
            _asyncVideoFramer = asyncVideoFramer;
            _pngImagesPipeParser = pngImagesPipeParser;
            _videoProgressMetric = videoProgressMetric;
            _videoInfoStorage = videoInfoStorage;
        }

        public async Task<IReadOnlyList<IReadOnlyList<Vector2i>>> GetHumanKeypoints(
            Guid processingId,
            CancellationToken token)
        {
            var rawPipe = await _asyncVideoFramer.GetRawPipeImagesStream(processingId, token);
            var images = _pngImagesPipeParser.GetImages(rawPipe);
            var videoFramesPoints = new List<IReadOnlyList<Vector2i>>(images.Length);

            _videoProgressMetric.Create(processingId, images.Length);

            int width = 0;
            int height = 0;

            foreach (var image in images)
            {
                var points = _mocapService.Run(image);
                _videoProgressMetric.Increment(processingId);
                videoFramesPoints.Add(points);
                width = image.Width;
                height = image.Height;
            }

            _videoInfoStorage.Set(processingId, new(width, height));

            return videoFramesPoints;
        }
    }
}
