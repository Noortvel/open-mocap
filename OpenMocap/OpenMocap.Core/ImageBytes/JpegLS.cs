using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMocap.Core.ImageBytes
{
    public static class JpegLS
    {
        public static readonly byte[] StartOfImageBytes = new byte[] { 0xFF, 0xD8 };
        public static readonly byte[] EndOfImageBytes = new byte[] { 0xFF, 0xD9 };

        //public static readonly char StartOfImageChar = Convert.ToChar(0xFFD8);
        //public static readonly char EndOfImageChar = Convert.ToChar(0xFFD9);

        //public const int StartOfImageBytes = 0xFFD8;
        //public const int EndOfImageBytes = 0xFFD9;
    }
}
