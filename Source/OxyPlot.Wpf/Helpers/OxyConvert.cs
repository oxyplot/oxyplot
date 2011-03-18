using System;
using System.Windows;
using System.Windows.Media;

namespace OxyPlot.Wpf
{
    public static class OxyConvert
    {
        public static OxyColor ToOxyColor(Color color)
        {
            return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Color ToColor(OxyColor c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static Point ToPoint(ScreenPoint pt)
        {
            return new Point(pt.X, pt.Y);
        }

        public static ScreenPoint ToScreenPoint(Point pt)
        {
            return new ScreenPoint(pt.X, pt.Y);
        }

        public static Brush ToBrush(OxyColor c)
        {
            return new SolidColorBrush(c.ToColor());
        }

        public static Rect ToRect(OxyRect r)
        {
            return new Rect(r.Left,r.Top,r.Width,r.Height);
        }
    }
}