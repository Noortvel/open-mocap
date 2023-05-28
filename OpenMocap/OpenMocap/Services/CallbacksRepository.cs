using System.Collections.Concurrent;

namespace OpenMocap.Services
{
    public class CallbacksRepository
    {
        // Not exists ConcurrentSet in .Net
        private readonly ConcurrentDictionary<string, object?> _storage = new();

        public void Add(string callback)
        {
            _storage.TryAdd(callback, null);
        }

        public IReadOnlyCollection<string> GetAll()
        {
            return _storage.Keys.ToArray();
        }

        public int Count => _storage.Keys.Count;
    }
}
