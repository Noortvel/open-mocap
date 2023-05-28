using OpenMocap.Front.Services;

namespace OpenMocap.Front.BackgroundServices
{
    public class MetricsRefresher : RefresherBase
    {
        public MetricsRefresher(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        private const int SendChunk = 100;

        protected override async Task<bool> Handle(
            IServiceProvider provider,
            CancellationToken cancellationToken)
        {
            var videoHubClients = provider
                    .GetRequiredService<VideoHubClients>();
            var openMocapClient = provider
                .GetRequiredService<OpenMocapClient>();

            var operationsChunks =
                videoHubClients.Operations.Chunk(SendChunk);

            foreach (var chunk in operationsChunks)
            {
                var getTasks = chunk
                    .Select(async x =>
                    {
                        var result = await openMocapClient
                            .GetVideoProcessingProgress(x, cancellationToken);
                        if (result != null)
                        {
                            await videoHubClients.PushVideoProgress(
                                result.OperationId,
                                result,
                                cancellationToken);
                        }
                    });

                await Task.WhenAll(getTasks);
            }

            return false;
        }
    }
}
