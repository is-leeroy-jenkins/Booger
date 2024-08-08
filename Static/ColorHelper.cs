namespace Booger
{
    public static class ColorHelper
    {
        public static double GetBrightness(int r, int g, int b)
        {
            double y = 0.299 * r + 0.587 * g + 0.114 * b;

            return y / 255;
        }
    }
}
