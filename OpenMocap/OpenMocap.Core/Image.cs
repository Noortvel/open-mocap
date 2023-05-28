namespace OpenMocap.Core
{
    public class Image
    {
        public byte[] Data { get; init; }

        public Image(byte[] data)
        {
            Data = data;
        }
    }
}
