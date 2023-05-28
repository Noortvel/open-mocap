using SixLabors.ImageSharp;

namespace OpenMocap.ML.Services.Transformers
{
    internal static class Utils
    {
        public static float CalcExpand(float value)
        {
            var _eps = float.Epsilon;
            var half = value / 2;
            var r_half = (int)half;
            var offset = (half - r_half) > _eps ? 1 : 0;
            var result_expand = r_half + offset;
            return result_expand;
        }

        public static Vector2i Cacl2Sections(float value)
        {
            var _eps = float.Epsilon;
            var half = value / 2;
            var r_half = (int)half;
            var offset = (half - r_half) > _eps ? 1 : 0;
            var result_expand = r_half + offset;

            return new Vector2i(r_half, result_expand);
        }

        public static Vector2i CalcNextRatioSize(
            Vector2i original,
            Vector2i destination)
        {
            var target_ratio = destination.Y / (float)(destination.X);
            var original_ratio = original.Y / (float)(original.X);
            var scale = target_ratio * (float)(original.X) / (float)(original.Y);
            var new_width = original.X;
            var new_height = original.Y;

            if (original_ratio < target_ratio)
            {
                new_height = (int)(original.Y * scale);
            }
            else
            {
                new_width = (int)(original.X / scale);
            }

            return new Vector2i(new_width, new_height);
        }

        public static Rectangle ExpandRectVal(
            Rectangle src,
            float value,
            float max_width,
            float max_height)
        {
            return Utils.ExpandRect(src, value, value, max_width, max_height);
        }

        public static Rectangle ExpandRect(
            Rectangle src,
            float width_value,
            float height_value,
            float max_width,
            float max_height)
        {
            var width_expand = (int)Utils.CalcExpand(width_value);
            var height_expand = (int)Utils.CalcExpand(height_value);

            var r_x = src.X - width_expand;
            var r_y = src.Y - height_expand;
            var r_width = src.Width + (int)width_value;
            var r_height = src.Height + (int)height_value;

            if (r_x < 0)
                r_x = 0;
            if (r_y < 0)
                r_y = 0;
            if (r_width > max_width)
                r_width = (int)max_width;
            if (r_height > max_height)
                r_height = (int)max_height;

            return new Rectangle(r_x, r_y, r_width, r_height);
        }
    }
}
