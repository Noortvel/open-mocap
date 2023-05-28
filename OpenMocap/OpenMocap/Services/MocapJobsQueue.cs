using OpenMocap.BackgroundServices;
using System.Collections.Concurrent;

namespace OpenMocap.Services
{
    public class MocapJobsQueue
    {
        private readonly ConcurrentQueue<MocapJob> _jobs = new();

        public void Enqueue(MocapJob job)
            => _jobs.Enqueue(job);

        public MocapJob? DequeueOrDefault()
        {
            if (_jobs.Count == 0)
            {
                return null;
            }

            if (_jobs.TryDequeue(out var job))
            {
                return job;
            }

            return null;
        }
    }
}
