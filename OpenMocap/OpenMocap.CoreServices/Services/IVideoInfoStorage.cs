using OpenMocap.Domain;

namespace OpenMocap.CoreServices.Services
{
    public interface IVideoInfoStorage
    {
        public void Set(Guid operationId, VideoInfo videoInfo);

        public VideoInfo? GetOrDefault(Guid operationId);
    }
}
