namespace Procedural
{
    public class BlockOutRoom
    {
        public readonly int Width;
        public readonly int Height;

        public BlockOutRoom(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static BlockOutRoom Default()
        {
            return new BlockOutRoom(10, 10);
        }
    }
}
