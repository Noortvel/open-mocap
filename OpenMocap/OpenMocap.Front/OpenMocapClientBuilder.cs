namespace OpenMocap.Front
{
    public class OpenMocapClientBuilder
    {
        private readonly OpenMocapApiProvider _openMocapApiProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public OpenMocapClientBuilder(OpenMocapApiProvider openMocapApiProvider, IHttpClientFactory httpClientFactory)
        {
            _openMocapApiProvider = openMocapApiProvider;
            _httpClientFactory = httpClientFactory;
        }

        public IReadOnlyList<OpenMocapClient> GetAll()
        {
           var urls = _openMocapApiProvider
                .GetAllUrls();

            var list = new List<OpenMocapClient>(urls.Length);
            foreach (var url in urls)
            {
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(url);
                var client = new OpenMocapClient(httpClient, _openMocapApiProvider);
                list.Add(client);
            }

            return list;
        }
    }
}
