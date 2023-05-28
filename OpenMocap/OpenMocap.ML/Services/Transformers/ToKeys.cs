using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Numerics;

namespace OpenMocap.ML.Services.Transformers
{
    internal class ToKeys
    {
        private Size _detectorSize;
        private Vector2i tOffset;
        private Vector2 tScale;
        private bool IsForwarded;
        
        public ToKeys(Size detectorSize)
        {
            _detectorSize = detectorSize;
        }

        public IImageProcessingContext Forward(
            IImageProcessingContext context,
            Size imageSize)
        {
            // 1. Находим ближаший прямоугольник подгодящий по соотношению
            // сторон с детектором и добавляем размер(пустое пространство)
            var f_size = imageSize.ToVector2i();
            var next_size = Utils.CalcNextRatioSize(f_size, _detectorSize.ToVector2i());
            var result = context
                .Resize(
                    new ResizeOptions()
                    {
                        Size = next_size.ToSize(),
                        Mode = ResizeMode.BoxPad,
                    })
                .Resize(
                    new ResizeOptions() // 2. Изменение размера до детектора.
                    {
                        Size = _detectorSize,
                        Mode = ResizeMode.Stretch,
                    });

            var locatorSize = _detectorSize.ToVector2i();
            var next_dt = new Vector2i(
              next_size.X - f_size.X,
              next_size.Y - f_size.Y);
            var hbor = Utils.Cacl2Sections(next_dt.X);
            var vbor = Utils.Cacl2Sections(next_dt.Y);

            tOffset = new Vector2i(hbor.X, vbor.Y);
            tScale = new Vector2(
                (float)next_size.X / (locatorSize.X),
                (float)next_size.Y / (locatorSize.Y)
                );
            IsForwarded = true;

            return result;
        }

        public Vector2i Inverse(Vector2i point)
        {
            if(!IsForwarded)
                ExceptionBuilder.ThrowNotForwarded();

            var nx = (int)((point.X * tScale.X) - tOffset.X);
            var ny = (int)((point.Y * tScale.Y) - tOffset.Y);
            return new(nx, ny);
        }
    }
}
