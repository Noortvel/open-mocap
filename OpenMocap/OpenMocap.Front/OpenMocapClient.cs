using OpenMocap.BackgroundServices;
using OpenMocap.Domain.Dtos;

namespace OpenMocap.Front
{
    public class OpenMocapClient
    {
        private readonly HttpClient _httpClient;
        private readonly OpenMocapApiProvider _provider;

        public OpenMocapClient(
            HttpClient httpClient,
            OpenMocapApiProvider provider)
        {
            _httpClient = httpClient;
            _provider = provider;
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(_provider.GetNextUrl());
            }
        }

        public async Task<VideoProcessingProgress?> GetVideoProcessingProgress(
            Guid operationId,
            CancellationToken token)
        {
            var response = await _httpClient
                .GetAsync($"/video_processing_progress/{operationId}", token);
            response.EnsureSuccessStatusCode();
            if(response.Content == null
                || response.Content.Headers.ContentLength == 0)
            {
                return null;
            }

            return await response.Content
                .ReadFromJsonAsync<VideoProcessingProgress>(cancellationToken: token);
        }

        public async Task Run(
            MocapJob dto,
            CancellationToken token)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/mocap/run_async",
                dto,
                token);

            response.EnsureSuccessStatusCode();
        }

        public async Task RegisterResultReciver(
            RegisterResultReciverDto dto,
            CancellationToken token)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/mocap/register_result_reciver",
                dto,
                token);

            response.EnsureSuccessStatusCode();
        }
    }
}
