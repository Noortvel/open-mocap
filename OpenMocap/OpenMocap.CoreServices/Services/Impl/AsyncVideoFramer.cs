using OpenMocap.Core.ImageBytes;

namespace OpenMocap.CoreServices.Services.Impl
{
    /// <summary>
    /// Split video to frames using ffmpeg and his http inreface. Thats why it async.
    /// </summary>
    internal class AsyncVideoFramer : IAsyncVideoFramer
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IFFmpegProcess _ffmpegService;
        private readonly IImagesRepository _imagesRepository;

        public AsyncVideoFramer(
            IVideoRepository videoRepository,
            IFFmpegProcess ffmpegService,
            IImagesRepository imagesRepository)
        {
            _videoRepository = videoRepository;
            _ffmpegService = ffmpegService;
            _imagesRepository = imagesRepository;
        }

        /// <summary>
        /// Runs async video split to frames.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="token"></param>
        /// <returns>ProcessingId</returns>
        public async Task<Guid> RunSplit(
            Stream stream,
            CancellationToken token)
        {
            var id = Guid.NewGuid();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();
            return id;
        }

        /// <summary>
        /// Runs async video split to frames.
        /// </summary>
        /// <param name="operationId">Set operation id for identify returned async callback.</param>
        /// <param name="stream"></param>
        /// <param name="token"></param>
        public async Task RunSplit(
            Guid operationId,
            byte[] bytes,
            CancellationToken token)
        {
            var id = operationId;
            await _videoRepository.Save(id, bytes, token);
            await _ffmpegService.SendSplitToFrames(id, token);
        }

        /// <summary>
        /// Save raw images stream. Not use directly, its for callback.
        /// </summary>
        /// <param name="processingId"></param>
        /// <param name="stream"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task SaveRawImages(Guid processingId, Stream stream, CancellationToken token)
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();
            await _imagesRepository.Save(processingId, bytes, token);
        }

        ///// <summary>
        ///// Split raw stream to seperate images bytes and returns its.
        ///// </summary>
        ///// <param name="processingId"></param>
        ///// <param name="token"></param>
        ///// <returns></returns>
        ///// <exception cref="InvalidOperationException"></exception>
        //public async Task<byte[][]> GetSplitedImagesStreamBytes(
        //    Guid processingId,
        //    CancellationToken token)
        //{
        //    var bytes = await _imagesRepository.Get(processingId, token);
        //    byte[] sof = PngBytes.StartOfFileBytes;
        //    if (bytes.Length < sof.Length)
        //    {
        //        throw new InvalidOperationException(
        //            $"Bytes(VideId={processingId}) in repository not valid");
        //    }

        //    var bordersIndexes = new List<int>();

        //    for (int i = 0; i < bytes.Length - sof.Length;)
        //    {
        //        bool isOk = true;
        //        for (int j = 0; j < sof.Length; j++)
        //        {
        //            var bVal = bytes[i + j];
        //            var tVal = sof[j];
        //            if (bVal != tVal)
        //            {
        //                i += j + 1;
        //                isOk = false;
        //                break;
        //            }
        //        }

        //        if (!isOk)
        //        {
        //            continue;
        //        }

        //        bordersIndexes.Add(i);
        //        i += sof.Length;
        //    }

        //    bordersIndexes.Add(bytes.Length);

        //    var outImages = new byte[bordersIndexes.Count - 1][];
        //    for (int i = 0; i < bordersIndexes.Count - 1; i++)
        //    {
        //        var start = bordersIndexes[i];
        //        var end = bordersIndexes[i + 1];
        //        outImages[i] = bytes[start..end];
        //    }

        //    return outImages;
        //}

        public Task<byte[]> GetRawPipeImagesStream(Guid processingId, CancellationToken token)
        {
            return _imagesRepository.Get(processingId, token);
        }
    }
}
