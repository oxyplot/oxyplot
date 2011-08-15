using System;
using System.Windows;
using System.Windows.Media;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// Extension method used to convert to/from Windows/Windows.Media classes.
    /// </summary>
    public static class ConverterExtensions
    {
        /// <summary>
        /// Converts a Point array to a ScreenPoint array.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>A ScreenPoint array.</returns>
        public static ScreenPoint[] ToScreenPointArray(this Point[] points)
        {
            if (points == null) return null;
            var pts = new ScreenPoint[points.Length];
            for (int i = 0; i < points.Length; i++) pts[i] = points[i].ToScreenPoint();
            return pts;
        }

        public static OxyColor ToOxyColor(this Color color)
        {
            return OxyConvert.ToOxyColor(color);
        }

        public static OxyColor ToOxyColor(this Color? color)
        {
            return color.HasValue ? color.Value.ToOxyColor() : null;
        }

        public static OxyColor ToOxyColor(this Brush brush)
        {
            var scb = brush as SolidColorBrush;
            return scb != null ? scb.Color.ToOxyColor() : null;
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

        public static OxyThickness ToOxyThickness(this Thickness t)
        {
            return new OxyThickness(t.Left, t.Top, t.Right, t.Bottom);
        }
    
        public static double DistanceTo(this Point p1, Point p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static HorizontalTextAlign ToHorizontalTextAlign(this HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Center:
                    return HorizontalTextAlign.Center;
                case HorizontalAlignment.Right:
                    return HorizontalTextAlign.Right;
                default:
                    return HorizontalTextAlign.Left;
            }
        }
    }
}