using OpenMocap.Domain.Dtos;
using System.Collections.Concurrent;

namespace OpenMocap.Front.Services
{
    public class MocapResultsStorage
    {
        private readonly ConcurrentDictionary<Guid, MocapResultDto> _storage = new();

        public void Add(MocapResultDto result)
        {
            _storage.TryAdd(result.OperationId, result);
        }

        public void Remove(Guid operationId)
        {
            _storage.TryRemove(operationId, out var result);
        }

        public void RemoveRange(IEnumerable<Guid> operationIds)
        {
            foreach (var operationId in operationIds)
            {
                _storage.TryRemove(operationId, out var result);
            }
        }

        public MocapResultDto? GetOrDefault(Guid operationId)
        {
            if(_storage.TryGetValue(operationId, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
