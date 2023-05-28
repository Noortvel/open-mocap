using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMocap.Core.ImageBytes
{
    public static class BmpBytes
    {
        // Header
        // 2 bytes for identify 
        public static readonly byte[] StartOfFile = new byte[] { 0x42, 0x4D };

        //After start of file, next 4 bytes is file size in bytes
        /// <summary>
        /// Returns size of bmp images from bytes representation.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int GetFileSize(byte[] source, int offset, int len)
        {
            for (int i = offset; i < len - 1; i++)
            {
                if (source[i] == StartOfFile[0]
                    && source[i + 1] == StartOfFile[1]
                    && i + 1 + 4 < len)
                {
                    return BitConverter.ToInt32(source, i + 2);
                }
            }

            throw new InvalidOperationException("Cannot find bmp header");
        }

        /// <summary>
        /// Getting file bytes from byte array.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static ReadOnlySpan<byte> GetFile(byte[] source, int offset, out int fileLen)
        {
            fileLen = GetFileSize(source, offset, source.Length);
            return new ReadOnlySpan<byte>(source, offset, fileLen);
        }
    }
}
