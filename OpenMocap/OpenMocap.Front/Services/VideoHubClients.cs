using Microsoft.AspNetCore.SignalR;
using OpenMocap.Domain.Dtos;
using OpenMocap.Front.SignalR;

namespace OpenMocap.Front.Services
{
    public class VideoHubClients
    {
        private readonly IHubContext<VideoHub> _hubContext;
        private readonly OperationConnectionStorage _operationConnectionStorage;

        public VideoHubClients(
            IHubContext<VideoHub> hubContext,
            OperationConnectionStorage operationConnectionStorage)
        {
            _hubContext = hubContext;
            _operationConnectionStorage = operationConnectionStorage;
        }

        public async Task PushMocapResult(
            Guid operationId,
            MocapResultDto message,
            CancellationToken token)
        {
            var connections = _operationConnectionStorage.GetConnections(operationId);

            foreach(var connection in connections)
            {
                var client = _hubContext.Clients.Client(connection);
                await client.SendAsync(
                    "MocapResult",
                    message,
                    token);
            }
        }

        public async Task PushVideoProgress(
            Guid operationId,
            VideoProcessingProgress message,
            CancellationToken token)
        {
            var connections = _operationConnectionStorage.GetConnections(operationId);

            foreach(var connection in connections)
            {
                var client = _hubContext.Clients.Client(connection);
                await client.SendAsync(
                    "VideoProgress",
                    message,
                    token);
            }
        }

        public Guid[] Operations => _operationConnectionStorage.Operations;
    }
}
