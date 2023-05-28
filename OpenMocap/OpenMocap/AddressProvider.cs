using Microsoft.Extensions.Options;
using OpenMocap.CoreServices.Services;

namespace OpenMocap
{
    public class AddressProvider : IAddressProvider
    {
        private readonly IOptions<AddressesOptions> _options;

        public AddressProvider(IOptions<AddressesOptions> options)
        {
            _options = options;
        }

        public string GetInternalAddress()
        {
            var host = _options.Value?.InternalHost
                ?? throw new InvalidOperationException("Define InternalHost in appsettings");
            var port = _options.Value?.InternalPort
                ?? throw new InvalidOperationException("Define InternalPort in appsettings");

            return $"http://{host}:{port}";
        }
    }
}
