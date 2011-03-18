using System;
using System.Windows;
using System.Windows.Media;

namespace OxyPlot.Wpf
{
    public static class ConverterExtensions
    {
        public static OxyColor ToOxyColor(this Color color)
        {
            return OxyConvert.ToOxyColor(color);
        }

        public static Color ToColor(this OxyColor c)
        {
            return OxyConvert.ToColor(c);
        }

        public static Point ToPoint(this ScreenPoint pt)
        {
            return OxyConvert.ToPoint(pt);
        }

        public static ScreenPoint ToScreenPoint(this Point pt)
        {
            return OxyConvert.ToScreenPoint(pt);
        }

        public static Brush ToBrush(this OxyColor c)
        {
            return OxyConvert.ToBrush(c);
        }

        public static Rect ToRect(this OxyRect r)
        {
            return OxyConvert.ToRect(r);
        }

        public static double DistanceTo(this Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}