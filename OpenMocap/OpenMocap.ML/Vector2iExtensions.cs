using SixLabors.ImageSharp;

namespace OpenMocap.ML
{
    public static class Vector2iExtensions
    {
        public static Size ToSize(this Vector2i v)
        {
            return new Size(v.X, v.Y);
        }
    }
}
