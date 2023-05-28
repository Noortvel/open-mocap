using OpenMocap.Front.HttpHandlers;

namespace OpenMocap.Front.BackgroundServices
{
    public class Warmup : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public Warmup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000);

            var urlProvider = _serviceProvider.GetRequiredService<UrlProvider>();
            var openMocapClientBuilder = _serviceProvider.GetRequiredService<OpenMocapClientBuilder>();

            var url = urlProvider.GetCurrentAddress();
            url = url + ResultReciver.UrlPattern;
            var clients = openMocapClientBuilder.GetAll();
            foreach (var client in clients)
            {
                await client.RegisterResultReciver(new(url), stoppingToken);
            }
        }
    }
}
