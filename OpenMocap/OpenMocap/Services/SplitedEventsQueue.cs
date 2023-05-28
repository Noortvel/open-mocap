using System.Collections.Concurrent;

namespace OpenMocap.Services
{
    public record SplitedEvent(Guid OperationId);

    public class SplitedEventsQueue
    {
        private readonly ConcurrentQueue<SplitedEvent> _storage = new();

        public void Enqueue(SplitedEvent splitedEvent)
        {
            _storage.Enqueue(splitedEvent);
        }

        public SplitedEvent? DequeueOrDefault()
        {
            if(_storage.Count == 0)
            {
                return null;
            }

            if (_storage.TryDequeue(out var splitedEvent))
            {
                return splitedEvent;
            }

            return null;
        }

    }
}
