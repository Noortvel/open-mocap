using OpenMocap.Domain;
using System.Collections.Concurrent;

namespace OpenMocap.CoreServices.Services.Impl
{
    public class VideoInfoStorage : IVideoInfoStorage
    {
        private readonly ConcurrentDictionary<Guid, VideoInfo> _storage = new();

        public void Set(Guid operationId, VideoInfo videoInfo)
        {
            _storage.TryAdd(operationId, videoInfo);
        }

        public VideoInfo? GetOrDefault(Guid operationId)
        {
            return _storage.GetValueOrDefault(operationId);
        }
    }
}
