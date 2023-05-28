using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMocap.CoreServices.Services.Impl
{
    internal class VideoRepository : IVideoRepository
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
