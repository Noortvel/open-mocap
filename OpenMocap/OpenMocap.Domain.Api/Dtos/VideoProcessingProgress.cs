namespace OpenMocap.Domain.Dtos
{
    public record VideoProcessingProgress(
        Guid OperationId,
        int Current,
        int Max,
        DateTime Updated);
}
