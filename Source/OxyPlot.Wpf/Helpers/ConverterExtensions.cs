using System;
using System.Windows;
using System.Windows.Media;

namespace OxyPlot.Wpf
{
    internal static class ConverterExtensions
    {
        public static OxyColor ToOxyColor(this System.Windows.Media.Color color)
        {
            return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Windows.Media.Color ToColor(this OxyColor c)
        {
            return System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static Point ToPoint(this ScreenPoint pt)
        {
            return new Point(pt.X,pt.Y);
        }

        public static Brush ToBrush(this OxyColor c)
        {
            return new SolidColorBrush(c.ToColor());
        }

        public static double DistanceTo(this Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt(dx*dx + dy*dy);
        }
    }
}