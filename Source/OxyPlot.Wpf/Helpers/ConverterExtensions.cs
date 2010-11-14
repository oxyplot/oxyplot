namespace OxyPlot.Wpf
{
    internal static class ConverterExtensions
    {
        public static Color ToModelColor(this System.Windows.Media.Color color)
        {
            return Color.FromARGB(color.A, color.R, color.G, color.B);
        }

        public static System.Windows.Media.Color ToColor(this Color c)
        {
            return System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static double DistanceTo(this System.Windows.Point p1, System.Windows.Point p2)
        {
            return System.Windows.Point.Subtract(p1, p2).Length;
        }
    }
}