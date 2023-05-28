using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMocap.Core.ImageBytes
{
    public static class PngBytes
    {
        public static byte[] StartOfFileBytes = new byte[]
        {
            137, 80, 78, 71, 13, 10, 26, 10
        };
    }
}
