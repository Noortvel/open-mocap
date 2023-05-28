namespace OpenMocap.CoreServices.Services.Impl
{
    internal class ImagesRepository : IImagesRepository
    {
        private readonly Dictionary<Guid, byte[]> _storage = new();

        public Task<byte[]> Get(Guid videoId, CancellationToken token)
        {
            return Task.FromResult(_storage[videoId]);
        }

        public Task Save(Guid videoId, byte[] data, CancellationToken token)
        {
            _storage[videoId] = data;
            return Task.CompletedTask;
        }
    }
}
