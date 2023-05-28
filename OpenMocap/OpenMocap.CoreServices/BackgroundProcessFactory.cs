using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenMocap.Core
{
    internal static class BackgroundProcessFactory
    {
        public static Process BuildProcess(string fileNameWithoutExtension)
        {
            var process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName =
                GetOsSpecifiedFileName(fileNameWithoutExtension);
            return process;
        }

        private static string GetOsSpecifiedFileName(string fileName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return $"./Binary/{fileName}.exe";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return $"./Binary/{fileName}";
            }

            throw new NotSupportedException("This os not supported");
        }
    }
}
