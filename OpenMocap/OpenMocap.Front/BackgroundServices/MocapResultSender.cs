using OpenMocap.Front.Services;

namespace OpenMocap.Front.BackgroundServices
{
    public class MocapResultSender : RefresherBase
    {
        public MocapResultSender(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        private const int SendChunkSize = 500;
        protected override async Task<bool> Handle(
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var hub = serviceProvider
                .GetRequiredService<VideoHubClients>();
            var resultStorage = serviceProvider
                .GetRequiredService<MocapResultsStorage>();

            var resultChunks = hub.Operations
                .Select(resultStorage.GetOrDefault)
                .Where(x => x != null)
                .Chunk(SendChunkSize)
                ;

            bool isProcessed = false;
            foreach (var chunk in resultChunks)
            {
                await Task.WhenAll(
                    chunk.Select(xx => hub.PushMocapResult(
                        xx!.OperationId,
                        xx!,
                        cancellationToken)));
                resultStorage.RemoveRange(chunk.Select(x => x!.OperationId));
                isProcessed = true;
            }

            return isProcessed;
        }
    }
}
