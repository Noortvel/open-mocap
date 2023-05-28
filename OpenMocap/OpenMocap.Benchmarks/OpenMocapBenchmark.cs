using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;

namespace OpenMocap.Benchmarks
{
    //NOT USE, ITS NOT READY
    public class OpenMocapBenchmark
    {
        private const string VideoName = "stream_ready.mp4";
        
        private static HttpClient _client = null!;
        private static byte[] _videoBytes = null!;

        [GlobalSetup]
        public void Setup()
        {
            var factory = new WebApplicationFactory<OpenMocap.ITarget>()
                .WithWebHostBuilder(configuration => { });

            _client = factory.CreateClient();
            _videoBytes = File.ReadAllBytes($"Videos/{VideoName}");
        }

        [Benchmark]
        public async Task Mocap()
        {
            //Upload
            var videoContent = new ByteArrayContent(_videoBytes);
            videoContent.Headers.ContentType = MediaTypeHeaderValue.Parse("video/mp4");
            using var multipartFormContent = new MultipartFormDataContent();
            multipartFormContent.Add(videoContent, "video", VideoName);
            var response = await _client.PostAsync("/async_run_split", multipartFormContent);
            response.EnsureSuccessStatusCode();
            var id = await response.Content.ReadAsStringAsync();
            id = id.Replace("\"", "");
            var processId = Guid.Parse(id);

            //GetKeypoins
            var keypoinsResponse = await _client.GetAsync($"/async_get_keypoints/{processId}");
            keypoinsResponse.EnsureSuccessStatusCode();
        }
    }
}
