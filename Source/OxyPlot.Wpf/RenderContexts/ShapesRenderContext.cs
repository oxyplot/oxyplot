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

        private void Add(FrameworkElement e)
        {
            canvas.Children.Add(e);
        }

        public ShapesRenderContext(Canvas canvas)
        {
            this.canvas = canvas;
            Width = canvas.ActualWidth;
            Height = canvas.ActualHeight;
        }

        #region IRenderContext Members

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public double Width { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to paint the background.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the background should be painted; otherwise, <c>false</c>.
        /// </value>
        public bool PaintBackground
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public double Height { get; private set; }

        /// <summary>
        /// Should not combine too many geometries into the same group...
        /// </summary>
        public int MaxGeometriesPerPath = 256;
        public int MaxFiguresPerGeometry = 16;

        readonly Dictionary<OxyColor, Brush> brushCache = new Dictionary<OxyColor, Brush>();

        public void DrawLineSegments(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            Path path = null;
            StreamGeometry geometry = null;
            StreamGeometryContext sgc = null;
            int count = 0;

            for (int i = 0; i + 1 < points.Count; i += 2)
            {
                if (path == null)
                {
                    path = new Path();
                    SetStroke(path, stroke, thickness, lineJoin, dashArray, aliased);
                    geometry = new StreamGeometry();
                    sgc = geometry.Open();
                }
                sgc.BeginFigure(points[i].ToPoint(aliased), false, false);
                sgc.LineTo(points[i + 1].ToPoint(aliased), true, false);
                count++;

                // Must limit the number of figures, otherwise drawing errors...
                if (count > MaxFiguresPerGeometry)
                {
                    sgc.Close();
                    path.Data = geometry;
                    Add(path);
                    path = null;
                    count = 0;

                }
            }

            if (path != null)
            {
                sgc.Close();
                path.Data = geometry;
                Add(path);
            }
        }

        public void DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray,
                             OxyPenLineJoin lineJoin, bool aliased)
        {
            var e = new Polyline();
            SetStroke(e, stroke, thickness, lineJoin, dashArray, aliased);

            var pc = new PointCollection(points.Count);
            foreach (var p in points)
            {
                pc.Add(p.ToPoint(aliased));
            }
            e.Points = pc;

            Add(e);
        }

        private void SetStroke(Shape shape, OxyColor stroke, double thickness, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter, double[] dashArray = null, bool aliased = false)
        {
            if (stroke != null && thickness > 0)
            {
                shape.Stroke = GetCachedBrush(stroke);

                switch (lineJoin)
                {
                    case OxyPenLineJoin.Round:
                        shape.StrokeLineJoin = PenLineJoin.Round;
                        break;
                    case OxyPenLineJoin.Bevel:
                        shape.StrokeLineJoin = PenLineJoin.Bevel;
                        break;
                    //  The default StrokeLineJoin is Miter
                }

                if (thickness != 1) // default values is 1
                    shape.StrokeThickness = thickness;
                if (dashArray != null)
                    shape.StrokeDashArray = new DoubleCollection(dashArray);
            }
            if (aliased)
            {
                shape.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                shape.SnapsToDevicePixels = true;
            }

        }

        /// <summary>
        /// Gets the cached brush.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The brush.</returns>
        private Brush GetCachedBrush(OxyColor color)
        {
            if (color.A == 0)
            {
                return null;
            }

            Brush brush;
            if (!this.brushCache.TryGetValue(color, out brush))
            {
                brush = new SolidColorBrush(color.ToColor());
                brush.Freeze();
                this.brushCache.Add(color, brush);
            }
            return brush;
        }

        public void DrawPolygon(IList<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness,
                                double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            var e = new Polygon();
            SetStroke(e, stroke, thickness, lineJoin, dashArray, aliased);

            if (fill != null)
                e.Fill = GetCachedBrush(fill);

            var pc = new PointCollection(points.Count);
            foreach (var p in points)
                pc.Add(p.ToPoint(aliased));
            e.Points = pc;

            Add(e);
        }

        public void DrawPolygons(IList<IList<ScreenPoint>> polygons, OxyColor fill, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            Path path = null;
            StreamGeometry geometry = null;
            StreamGeometryContext sgc = null;
            int count = 0;

            foreach (var polygon in polygons)
            {
                if (path == null)
                {
                    path = new Path();
                    SetStroke(path, stroke, thickness, lineJoin, dashArray, aliased);
                    if (fill != null)
                    {
                        path.Fill = this.GetCachedBrush(fill);
                    }
                    geometry = new StreamGeometry { FillRule = FillRule.Nonzero };
                    sgc = geometry.Open();
                }


                bool first = true;
                foreach (var p in polygon)
                {
                    if (first)
                    {
                        sgc.BeginFigure(p.ToPoint(aliased), fill != null, true);
                        first = false;
                    }
                    else
                    {
                        sgc.LineTo(p.ToPoint(aliased), stroke != null, true);
                    }
                }

                count++;

                // Must limit the number of figures, otherwise drawing errors...
                if (count > MaxFiguresPerGeometry)
                {
                    sgc.Close();
                    path.Data = geometry;
                    Add(path);
                    path = null;
                    count = 0;

                }
            }

            if (path != null)
            {
                sgc.Close();
                path.Data = geometry;
                Add(path);
            }
        }

        /// <summary>
        /// Draws the rectangle.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="fill">The fill.</param>
        /// <param name="stroke">The stroke.</param>
        /// <param name="thickness">The thickness.</param>
        public void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            var e = new Rectangle();
            SetStroke(e, stroke, thickness, OxyPenLineJoin.Miter, null, true);

            if (fill != null)
            {
                e.Fill = this.GetCachedBrush(fill);
            }

            e.Width = rect.Width;
            e.Height = rect.Height;
            Canvas.SetLeft(e, rect.Left);
            Canvas.SetTop(e, rect.Top);
            Add(e);
        }

        public void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            var path = new Path();
            SetStroke(path, stroke, thickness);
            if (fill != null)
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
            foreach (var rect in rectangles)
            {
                gg.Children.Add(new RectangleGeometry { Rect = rect.ToRect(true) });
            }
            path.Data = gg;
            Add(path);
            /*
            Path path = null;
            GeometryGroup gg = null;
            int count = 0;
            foreach (var rect in rectangles)
            {
                if (path == null)
                {
                    path = new Path();
                    SetStroke(path, stroke, thickness, OxyPenLineJoin.Miter, null, true);
                    if (fill != null)
                        path.Fill = GetCachedBrush(fill);

                    gg = new GeometryGroup();
                    gg.FillRule = FillRule.Nonzero;
                }
                gg.Children.Add(new RectangleGeometry(rect.ToRect()));
                count++;
                if (count == MaxGeometriesPerPath)
                {
                    path.Data = gg;
                    Add(path);
                    path = null;
                    count = 0;
                }
            }
            if (path != null)
            {
                path.Data = gg;
                Add(path);
            }*/
        }

        public void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            var e = new Ellipse();
            SetStroke(e, stroke, thickness, OxyPenLineJoin.Miter, null, false);
            if (fill != null)
            {
                e.Fill = GetCachedBrush(fill);
            }

            e.Width = rect.Width;
            e.Height = rect.Height;
            Canvas.SetLeft(e, rect.Left);
            Canvas.SetTop(e, rect.Top);
            Add(e);
        }

        public void DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            var path = new Path();
            SetStroke(path, stroke, thickness);
            if (fill != null)
            {
                path.Fill = GetCachedBrush(fill);
            }

            var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
            foreach (var rect in rectangles)
            {
                gg.Children.Add(new EllipseGeometry(rect.ToRect(true)));
            }

            path.Data = gg;
            this.Add(path);
        }

        public void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily, double fontSize,
                             double fontWeight, double rotate, HorizontalTextAlign halign, VerticalTextAlign valign)
        {
            var tb = new TextBlock
                         {
                             Text = text,
                             Foreground = GetCachedBrush(fill)
                         };
            if (fontFamily != null)
                tb.FontFamily = new FontFamily(fontFamily);
            if (fontSize > 0)
                tb.FontSize = fontSize;
            if (fontWeight > 0)
                tb.FontWeight = GetFontWeight(fontWeight);

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

            Add(tb);
        }
        private static FontWeight GetFontWeight(double fontWeight)
        {
            return fontWeight > FontWeights.Normal ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Normal;
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
                tb.FontWeight = GetFontWeight(fontWeight);

            tb.Measure(new Size(1000, 1000));

            return new OxySize(tb.DesiredSize.Width, tb.DesiredSize.Height);
        }

        #endregion
    }
}