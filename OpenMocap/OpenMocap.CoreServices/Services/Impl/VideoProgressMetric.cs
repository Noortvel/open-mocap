using OpenMocap.Domain.Dtos;

namespace OpenMocap.CoreServices.Services.Impl
{
    public class VideoProgressMetric : IVideoProgressMetric
    {
        private readonly Dictionary<Guid, VideoProcessingProgressInternal> _storage = new();

        public void Create(Guid operationId, int maxValue)
        {
            _storage[operationId] = new(1, maxValue, DateTime.UtcNow);
        }

        public VideoProcessingProgress? GetOrDefault(Guid operationId)
        {
            _storage.TryGetValue(
                operationId,
                out VideoProcessingProgressInternal? data);
            return data == null ?
                null
                : new(operationId, data.current, data.max, data.updated);
        }

        public void Increment(Guid operationId)
        {
            var info = _storage[operationId];
            info.updated = DateTime.UtcNow;
            info.current++;
        }

        private class VideoProcessingProgressInternal
        {
            public VideoProcessingProgressInternal(
                int current,
                int max,
                DateTime updated)
            {
                this.current = current;
                this.max = max;
                this.updated = updated;
            }

            public int current;
            public int max;
            public DateTime updated;
        }
    }
}
