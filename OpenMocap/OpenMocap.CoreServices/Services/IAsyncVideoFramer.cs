namespace OpenMocap.CoreServices.Services
{
    public interface IAsyncVideoFramer
    {
        /// <summary>
        /// Run split video to frames.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Guid> RunSplit(
            Stream stream,
            CancellationToken token);

        /// <summary>
        /// Runs async video split to frames.
        /// </summary>
        /// <param name="operationId">Set operation id for identify returned async callback.</param>
        /// <param name="stream"></param>
        /// <param name="token"></param>
        public Task RunSplit(
            Guid operationId,
            byte[] bytes,
            CancellationToken token);

        Task SaveRawImages(Guid processingId, Stream stream, CancellationToken token);
        Task<byte[]> GetRawPipeImagesStream(Guid processingId, CancellationToken token);
    }
}
