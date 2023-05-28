namespace OpenMocap.BackgroundServices
{
    public class MocapJob
    {
        public MocapJob(Guid operationId, byte[] input)
        {
            OperationId = operationId;
            Input = input;
        }
        public Guid OperationId { get; init; }

        public byte[] Input { get; init; }
    }
}
