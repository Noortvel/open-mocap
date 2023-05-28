using SixLabors.ImageSharp;
using System.Numerics;

namespace OpenMocap.ML.Services.Transformers
{
    internal class AfterKeys
    {
        private readonly Size _inputSize;
        private readonly Size _outputSize;
        private readonly Vector2 _scale;

        public AfterKeys(Size inputSize, Size outputSize)
        {
            _inputSize = inputSize;
            _outputSize = outputSize;
            _scale = new Vector2(
                (float)_inputSize.Width / _outputSize.Width,
                (float)_inputSize.Height / _outputSize.Height);
        }

        public Vector2i Inverse(Point point)
        {
            return new Vector2i(
                (int)MathF.Round(point.X * _scale.X),
                (int)MathF.Round(point.Y * _scale.Y)
                );
        }
    }
}
