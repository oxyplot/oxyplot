using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// Rendering WPF shapes to a Canvas
    /// </summary>
    public class ShapesRenderContext : IRenderContext
    {
        private readonly Canvas canvas;

        public ShapesRenderContext(Canvas canvas)
        {
            this.canvas = canvas;
            Width = canvas.ActualWidth;
            Height = canvas.ActualHeight;
        }

        #region IRenderContext Members

        public double Width { get; private set; }

        public double Height { get; private set; }

        Dictionary<OxyColor, Brush> brushCache = new Dictionary<OxyColor, Brush>();

        public void DrawLine(IEnumerable<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray,
                             bool aliased)
        {
            var pl = new Polyline();
            if (stroke != null && thickness > 0)
            {
                pl.Stroke = GetCachedBrush(stroke);
                //   Default StrokeLineJoin is Miter
                //   pl.StrokeLineJoin = PenLineJoin.Round;
                if (thickness != 1) // default values is 1
                    pl.StrokeThickness = thickness;
                if (dashArray != null)
                    pl.StrokeDashArray = new DoubleCollection(dashArray);
            }
            // pl.Fill = null;
            if (aliased)
                pl.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            var pc = new PointCollection();
            foreach (var p in points)
                pc.Add(ToPoint(p));
            pl.Points = pc;

            canvas.Children.Add(pl);
        }

        private Brush GetCachedBrush(OxyColor stroke)
        {
            Brush brush;
            if (!brushCache.TryGetValue(stroke, out brush))
            {
                brush = new SolidColorBrush(stroke.ToColor());
                brushCache.Add(stroke, brush);
            }
            return brush;
        }

        public void DrawPolygon(IEnumerable<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness,
                                double[] dashArray, bool aliased)
        {
            var po = new Polygon();
            if (stroke != null && thickness > 0)
            {
                po.Stroke = GetCachedBrush(stroke);
                if (thickness != 1)
                    po.StrokeThickness = thickness;
                if (dashArray != null)
                    po.StrokeDashArray = new DoubleCollection(dashArray);
            }
            if (aliased)
                po.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            if (fill != null)
                po.Fill = GetCachedBrush(fill);
            
            var pc = new PointCollection();
            foreach (var p in points)
                pc.Add(ToPoint(p));
            po.Points = pc;

            canvas.Children.Add(po);
        }

        ///<summary>
        /// Draws a rectangle.
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="width"></param>
        ///<param name="height"></param>
        ///<param name="fill"></param>
        ///<param name="stroke"></param>
        ///<param name="thickness"></param>
        public void DrawRectangle(double x,double y, double width,double height, OxyColor fill, OxyColor stroke,
                                double thickness)
        {
            var el = new Rectangle();
            if (stroke != null)
            {
                el.Stroke = new SolidColorBrush(stroke.ToColor());
                el.StrokeThickness = thickness;
            }
            if (fill != null)
            {
                el.Fill = new SolidColorBrush(fill.ToColor());
            }

            el.Width = width;
            el.Height = height;
            Canvas.SetLeft(el, x);
            Canvas.SetTop(el, y);
            canvas.Children.Add(el);
        }
        
        public void DrawEllipse(double x, double y, double width, double height, OxyColor fill, OxyColor stroke,
                                double thickness)
        {
            var el = new Ellipse();
            if (stroke != null)
            {
                el.Stroke = new SolidColorBrush(stroke.ToColor());
                el.StrokeThickness = thickness;
            }
            if (fill != null)
            {
                el.Fill = new SolidColorBrush(fill.ToColor());
            }

            el.Width = width;
            el.Height = height;
            Canvas.SetLeft(el, x);
            Canvas.SetTop(el, y);
            canvas.Children.Add(el);
        }

        public void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily, double fontSize,
                             double fontWeight, double rotate, HorizontalTextAlign halign, VerticalTextAlign valign)
        {
            var tb = new TextBlock
                         {
                             Text = text,
                             Foreground = new SolidColorBrush(fill.ToColor())
                         };
            if (fontFamily != null)
                tb.FontFamily = new FontFamily(fontFamily);
            if (fontSize > 0)
                tb.FontSize = fontSize;
            if (fontWeight > 0)
                tb.FontWeight = FontWeight.FromOpenTypeWeight((int)fontWeight);

            tb.Measure(new Size(1000, 1000));

            double dx = 0;
            if (halign == HorizontalTextAlign.Center)
                dx = -tb.DesiredSize.Width / 2;
            if (halign == HorizontalTextAlign.Right)
                dx = -tb.DesiredSize.Width;

            double dy = 0;
            if (valign == VerticalTextAlign.Middle)
                dy = -tb.DesiredSize.Height / 2;
            if (valign == VerticalTextAlign.Bottom)
                dy = -tb.DesiredSize.Height;


            var transform = new TransformGroup();
            transform.Children.Add(new TranslateTransform(dx, dy));
            if (rotate != 0)
                transform.Children.Add(new RotateTransform(rotate));
            transform.Children.Add(new TranslateTransform(p.X, p.Y));
            tb.RenderTransform = transform;

            tb.SetValue(RenderOptions.ClearTypeHintProperty, ClearTypeHint.Enabled);

            canvas.Children.Add(tb);
        }

        public OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (String.IsNullOrEmpty(text))
                return OxySize.Empty;

            var tb = new TextBlock
                         {
                             Text = text
                         };

            if (fontFamily != null)
                tb.FontFamily = new FontFamily(fontFamily);
            if (fontSize > 0)
                tb.FontSize = fontSize;
            if (fontWeight > 0)
                tb.FontWeight = FontWeight.FromOpenTypeWeight((int)fontWeight);

            tb.Measure(new Size(1000, 1000));

            return new OxySize(tb.DesiredSize.Width, tb.DesiredSize.Height);
        }

        #endregion

        private static Point ToPoint(ScreenPoint point)
        {
            return new Point(point.X, point.Y);
        }
    }
}