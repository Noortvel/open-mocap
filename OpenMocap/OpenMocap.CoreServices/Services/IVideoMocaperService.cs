using OpenMocap.ML;

namespace OpenMocap.CoreServices.Services
{
    public interface IVideoMocaperService
    {
        Task<IReadOnlyList<IReadOnlyList<Vector2i>>> GetHumanKeypoints(
            Guid operationId,
            CancellationToken token);
    }
}
