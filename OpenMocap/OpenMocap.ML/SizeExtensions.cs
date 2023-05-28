using SixLabors.ImageSharp;

namespace OpenMocap.ML
{
    public static class SizeExtensions
    {
        public static Vector2i ToVector2i(this Size size)
        {
            return new Vector2i(size.Width, size.Height);
        }
    }
}
