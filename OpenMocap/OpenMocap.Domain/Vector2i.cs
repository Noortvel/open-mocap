namespace OpenMocap.ML
{
    public struct Vector2i
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2i(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector2i operator +(Vector2i a, Vector2i b)
            => new(a.X + b.X, a.Y + b.Y);
    }
}
