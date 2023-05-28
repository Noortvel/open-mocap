using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace OpenMocap.ML.Services.Transformers
{
    internal class AfterBox
    {
        private const int expandValue = 30;
        private bool isForwarded = false;
        private Vector2i tOffset;

        public IImageProcessingContext Forward(
            IImageProcessingContext context,
            Size imageSize,
            Rectangle rectangle)
        {
            var nbox = Utils.ExpandRectVal(
                rectangle,
                expandValue,
                imageSize.Width,
                imageSize.Height);
            
            var result = context.Crop(nbox);

            tOffset = new Vector2i(nbox.X, nbox.Y);
            isForwarded = true;

            return result;
        }

        public Vector2i Inverse(Vector2i point)
        {
            if (!isForwarded)
                ExceptionBuilder.ThrowNotForwarded();

            return tOffset + point;
        }
    }
}
