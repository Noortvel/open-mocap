using OpenMocap.CoreServices.Services;
using OpenMocap.Domain.Dtos;
using OpenMocap.Services;

namespace OpenMocap.BackgroundServices
{
    public class SplitedEventsHandler : BackgroundService
    {
        private readonly TimeSpan sleepTime = TimeSpan.FromSeconds(1);

        private readonly IServiceProvider _serviceProvider;

        public SplitedEventsHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var queue = _serviceProvider
                    .GetRequiredService<SplitedEventsQueue>();
                var message = queue.DequeueOrDefault();
                if(message == null)
                {
                    await Task.Delay(sleepTime, stoppingToken);
                    continue;
                }

                var mocaper = _serviceProvider
                    .GetRequiredService<IVideoMocaperService>();
                var vidoInfoStorage = _serviceProvider
                    .GetRequiredService<IVideoInfoStorage>();
                var callbacks = _serviceProvider
                    .GetRequiredService<CallbacksRepository>();
                var httpClient = _serviceProvider
                    .GetRequiredService<HttpClient>();

                var keys = await mocaper
                    .GetHumanKeypoints(message.OperationId, stoppingToken);

                var info = vidoInfoStorage.GetOrDefault(message.OperationId);

                foreach (var callback in callbacks.GetAll())
                {
                    await httpClient.PostAsJsonAsync(
                        callback,
                        new MocapResultDto(
                            message.OperationId,
                            info,
                            keys),
                        cancellationToken: stoppingToken
                        );
                }

            }
        }
    }
}
