using Microsoft.Extensions.Options;

namespace OpenMocap.Front
{
    public class UrlProvider
    {
        private readonly IOptions<AddressesOptions> _options;

        public UrlProvider(IOptions<AddressesOptions> options)
        {
            _options = options;
        }

        public string GetCurrentAddress()
        {
            var host = _options.Value?.SelfHost
                ?? throw new InvalidOperationException("Define SelfHost appsetting");
            var port = _options.Value?.SelfPort
                ?? throw new InvalidOperationException("Define SelfPort appsetting");

            return $"http://{host}:{port}";
        }
    }
}
