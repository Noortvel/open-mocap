using OpenMocap.ML;

namespace OpenMocap.Domain.Dtos
{
    public record MocapResultDto(
        Guid OperationId,
        VideoInfo? Info,
        IReadOnlyList<IReadOnlyList<Vector2i>> Points
    );
}
