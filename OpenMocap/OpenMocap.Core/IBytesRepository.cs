namespace OpenMocap.Core
{
    public interface IBytesRepository
    {
        Task Save(Guid videoId, byte[] data, CancellationToken token);

        Task<byte[]> Get(Guid videoId, CancellationToken token);
    }
}
