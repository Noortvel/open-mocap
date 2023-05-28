using Microsoft.AspNetCore.SignalR;
using OpenMocap.Front.Services;

namespace OpenMocap.Front.SignalR
{
    public class VideoHub : Hub
    {
        public const string Url = "/video_hub";

        private readonly IServiceProvider _serviceProvider;

        public VideoHub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task RegisterClientReciveVideoIdProgress(
            Guid operationId)
        {
            var logger = _serviceProvider
                .GetRequiredService<ILogger<VideoHub>>();
            try
            {
                var storage = _serviceProvider
                    .GetRequiredService<OperationConnectionStorage>();
                var client = _serviceProvider
                    .GetRequiredService<OpenMocapClient>();
                var videoHubClients = _serviceProvider
                    .GetRequiredService<VideoHubClients>();


                storage.Add(operationId, Context.ConnectionId);
                var reuslt = await client.GetVideoProcessingProgress(
                    operationId,
                    Context.ConnectionAborted);
                if (reuslt != null)
                {
                    await videoHubClients.PushVideoProgress(
                        operationId,
                        reuslt,
                        Context.ConnectionAborted);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to register callbacks, {Message}", ex.Message);
            }
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var storage = _serviceProvider
                .GetRequiredService<OperationConnectionStorage>();
            storage.RemoveIfExists(Context.ConnectionId);

            return Task.CompletedTask;
        }
    }
}
