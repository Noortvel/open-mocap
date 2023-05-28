using Microsoft.Extensions.DependencyInjection;
using OpenMocap.CoreServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMocap.CoreServices.Benchmarks
{
    public class VideoMocaperServiceBenchmark
    {
        public VideoMocaperServiceBenchmark()
        {
            var serviceCollection = new ServiceCollection();
            Configure.AddCoreServices(serviceCollection);
            var provider = serviceCollection.BuildServiceProvider();
            var mocapService = provider.GetRequiredService<IVideoMocaperService>();

        }
    }
}
