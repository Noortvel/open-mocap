using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMocap.Benchmarks
{
    public class MLMocapBenchmarkCudaGpu0 : MLMocapBenchmarkCpu
    {
        protected override bool IsGpu => true;
        protected override int GpuDevice => 0;
    }
}
