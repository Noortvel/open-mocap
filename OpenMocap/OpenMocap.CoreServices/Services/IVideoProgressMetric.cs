using OpenMocap.Domain.Dtos;

namespace OpenMocap.CoreServices.Services
{
    public interface IVideoProgressMetric
    {
        void Create(Guid operationId, int maxValue);
        void Increment(Guid operationId);
        VideoProcessingProgress? GetOrDefault(Guid operationId);
    }
}
