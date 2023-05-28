using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Numerics;

namespace OpenMocap.ML.Services.Transformers
{
    internal class ToBox
    {
        private Vector2i toOffset;
        private Vector2 toScale;
        private bool isForwarded;

        public IImageProcessingContext Forward(
            Size imageSize,
            Size locatorSize,
            IImageProcessingContext context)
        {
            int targetWidth;
            int targetHeight;
            int yOffset;
            int xOffset;

            if (imageSize.Width >= imageSize.Height)
            {
                xOffset = 0;
                yOffset = -(imageSize.Width - imageSize.Height) / 2;
                targetWidth = imageSize.Width;
                targetHeight = imageSize.Width;
            }
            else
            {
                xOffset = -(imageSize.Height - imageSize.Width) / 2;
                yOffset = 0;
                targetWidth = imageSize.Height;
                targetHeight = imageSize.Height;
            }

            var cropped = context
                .Resize(
                    new ResizeOptions
                    {
                        Size = new(targetWidth, targetHeight),
                        Mode = ResizeMode.BoxPad,
                    })
                .Resize(locatorSize);

            toOffset = new Vector2i(xOffset, yOffset);
            toScale = new Vector2(
                locatorSize.Width / (float)targetWidth,
                locatorSize.Height / (float)targetHeight);
            isForwarded = true;

            return cropped;
        }

        public Vector2i Inverse(Vector2i point)
        {
            if (!isForwarded)
                ExceptionBuilder.ThrowNotForwarded();

            return new Vector2i(
                (int)MathF.Round(point.X / toScale.X) + toOffset.X,
                (int)MathF.Round(point.Y / toScale.Y) + toOffset.Y
                );
        }

    }
}
