using OpenMocap.CoreServices.Services;
using OpenMocap.Domain.Dtos;
using OpenMocap.Services;

namespace OpenMocap.BackgroundServices
{
    public class MocapAsyncRunnner : BackgroundService
    {
        private readonly TimeSpan sleepTime = TimeSpan.FromSeconds(1);

        private readonly IServiceProvider _serviceProvider;

        public MocapAsyncRunnner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var logger = _serviceProvider
                    .GetRequiredService<ILogger<MocapAsyncRunnner>>();
                var queue = _serviceProvider.GetRequiredService<MocapJobsQueue>();

                var job = queue.DequeueOrDefault();
                if(job != null)
                {
                    logger.LogInformation(
                        "Start process: Job(OperationId={OperationId})",
                        job.OperationId);

                    var framer = _serviceProvider
                        .GetRequiredService<IAsyncVideoFramer>();

                    await framer
                        .RunSplit(job.OperationId, job.Input, stoppingToken);
                }

                await Task.Delay(sleepTime, stoppingToken);
            }
        }
    }
}
