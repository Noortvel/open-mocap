using OpenMocap.Core.ImageBytes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace OpenMocap.CoreServices.Services.Impl
{
    public class PngImagesPipeParser
    {
        public Image<Rgb24>[] GetImages(byte[] bytes)
        {
            byte[] sof = PngBytes.StartOfFileBytes;
            if (bytes.Length == 0 || bytes.Length < sof.Length)
            {
                throw new InvalidOperationException(
                    $"bytes is 0 or so small");
            }

            var bordersIndexes = new List<int>();

            for (int i = 0; i < bytes.Length - sof.Length;)
            {
                bool isOk = true;
                for (int j = 0; j < sof.Length; j++)
                {
                    var bVal = bytes[i + j];
                    var tVal = sof[j];
                    if (bVal != tVal)
                    {
                        i += j + 1;
                        isOk = false;
                        break;
                    }
                }

                if (!isOk)
                {
                    continue;
                }

                bordersIndexes.Add(i);
                i += sof.Length;
            }

            bordersIndexes.Add(bytes.Length);

            var outImages = new Image<Rgb24>[bordersIndexes.Count - 1];
            for (int i = 0; i < bordersIndexes.Count - 1; i++)
            {
                var start = bordersIndexes[i];
                var end = bordersIndexes[i + 1];
                var slice = bytes[start..end];
                outImages[i] = Image.Load<Rgb24>(slice);
            }

            return outImages;
        }
    }
}
