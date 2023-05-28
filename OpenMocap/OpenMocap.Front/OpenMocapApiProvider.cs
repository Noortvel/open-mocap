using Microsoft.Extensions.Options;

namespace OpenMocap.Front
{
    public class OpenMocapApiProvider
    {
        private readonly IOptions<AddressesOptions> _options;

        public OpenMocapApiProvider(IOptions<AddressesOptions> options)
        {
            _options = options;
        }

        private object _locker = new object();
        private int Counter = 0;

        public string GetNextUrl()
        {
            var clients = GetAllUrls();
            string client;
            lock (_locker) {
                client = clients[Counter];
                Counter++;
                if(Counter == clients.Length)
                {
                    Counter = 0;
                }
            }

            return client;
        }

        public string[] GetAllUrls()
        {
            var host = _options.Value?.WorkerHost
                ?? throw new InvalidOperationException("Define WorkerHost in appsettings");
            var ports = _options.Value?.WorkerPorts
                ?? throw new InvalidOperationException("Define WorkerPorts in appsettings");

            if (ports.Length == 0)
            {
                throw new InvalidOperationException("Define WorkerPorts in appsettings");
            }

            var result = new string[ports.Length];
            for(int i = 0; i < ports.Length; i++)
            {
                result[i] = $"http://{host}:{ports[i]}/";
            }

            return result;
        }
    }
}
