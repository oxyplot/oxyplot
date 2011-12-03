// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShapesRenderContext.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using FontWeights = OxyPlot.FontWeights;

    /// <summary>
    /// Rendering WPF shapes to a Canvas
    /// </summary>
    public class ShapesRenderContext : IRenderContext
    {
        #region Constants and Fields

        /// <summary>
        ///   The max figures per geometry.
        /// </summary>
        public int MaxFiguresPerGeometry = 16;

        /// <summary>
        ///   Should not combine too many geometries into the same group...
        /// </summary>
        public int MaxGeometriesPerPath = 256;

        /// <summary>
        ///   The brush cache.
        /// </summary>
        private readonly Dictionary<OxyColor, Brush> brushCache = new Dictionary<OxyColor, Brush>();

        /// <summary>
        ///   The canvas.
        /// </summary>
        private readonly Canvas canvas;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapesRenderContext"/> class.
        /// </summary>
        /// <param name="canvas">
        /// The canvas.
        /// </param>
        public ShapesRenderContext(Canvas canvas)
        {
            this.canvas = canvas;
            this.Width = canvas.ActualWidth;
            this.Height = canvas.ActualHeight;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public double Height { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether to paint the background.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the background should be painted; otherwise, <c>false</c>.
        /// </value>
        public bool PaintBackground
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        ///   Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public double Width { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the current tooltip.
        /// </summary>
        /// <value>
        ///   The current tooltip.
        /// </value>
        private string CurrentToolTip { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The draw ellipse.
        /// </summary>
        /// <param name="rect">
        /// The rect.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        public void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            var e = new Ellipse();
            this.SetStroke(e, stroke, thickness, OxyPenLineJoin.Miter, null, false);
            if (fill != null)
            {
                e.Fill = this.GetCachedBrush(fill);
            }

            e.Width = rect.Width;
            e.Height = rect.Height;
            Canvas.SetLeft(e, rect.Left);
            Canvas.SetTop(e, rect.Top);
            this.Add(e);
        }

        /// <summary>
        /// The draw ellipses.
        /// </summary>
        /// <param name="rectangles">
        /// The rectangles.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        public void DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            var path = new Path();
            this.SetStroke(path, stroke, thickness);
            if (fill != null)
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
            foreach (var rect in rectangles)
            {
                gg.Children.Add(new EllipseGeometry(rect.ToRect(true)));
            }

            path.Data = gg;
            this.Add(path);
        }

        /// <summary>
        /// The draw line.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        /// <param name="dashArray">
        /// The dash array.
        /// </param>
        /// <param name="lineJoin">
        /// The line join.
        /// </param>
        /// <param name="aliased">
        /// The aliased.
        /// </param>
        public void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPenLineJoin lineJoin,
            bool aliased)
        {
            var e = new Polyline();
            this.SetStroke(e, stroke, thickness, lineJoin, dashArray, aliased);

            var pc = new PointCollection(points.Count);
            foreach (var p in points)
            {
                pc.Add(p.ToPoint(aliased));
            }

            e.Points = pc;

            this.Add(e);
        }

        /// <summary>
        /// The draw line segments.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        /// <param name="dashArray">
        /// The dash array.
        /// </param>
        /// <param name="lineJoin">
        /// The line join.
        /// </param>
        /// <param name="aliased">
        /// The aliased.
        /// </param>
        public void DrawLineSegments(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPenLineJoin lineJoin,
            bool aliased)
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
                    this.SetStroke(path, stroke, thickness, lineJoin, dashArray, aliased);
                    geometry = new StreamGeometry();
                    sgc = geometry.Open();
                }

                sgc.BeginFigure(points[i].ToPoint(aliased), false, false);
                sgc.LineTo(points[i + 1].ToPoint(aliased), true, false);
                count++;

                // Must limit the number of figures, otherwise drawing errors...
                if (count > this.MaxFiguresPerGeometry)
                {
                    sgc.Close();
                    path.Data = geometry;
                    this.Add(path);
                    path = null;
                    count = 0;
                }
            }

            if (path != null)
            {
                sgc.Close();
                path.Data = geometry;
                this.Add(path);
            }
        }

        /// <summary>
        /// The draw polygon.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        /// <param name="dashArray">
        /// The dash array.
        /// </param>
        /// <param name="lineJoin">
        /// The line join.
        /// </param>
        /// <param name="aliased">
        /// The aliased.
        /// </param>
        public void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPenLineJoin lineJoin,
            bool aliased)
        {
            var e = new Polygon();
            this.SetStroke(e, stroke, thickness, lineJoin, dashArray, aliased);

            if (fill != null)
            {
                e.Fill = this.GetCachedBrush(fill);
            }

            var pc = new PointCollection(points.Count);
            foreach (var p in points)
            {
                pc.Add(p.ToPoint(aliased));
            }

            e.Points = pc;

            this.Add(e);
        }

        /// <summary>
        /// The draw polygons.
        /// </summary>
        /// <param name="polygons">
        /// The polygons.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        /// <param name="dashArray">
        /// The dash array.
        /// </param>
        /// <param name="lineJoin">
        /// The line join.
        /// </param>
        /// <param name="aliased">
        /// The aliased.
        /// </param>
        public void DrawPolygons(
            IList<IList<ScreenPoint>> polygons,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPenLineJoin lineJoin,
            bool aliased)
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
                    this.SetStroke(path, stroke, thickness, lineJoin, dashArray, aliased);
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
                if (count > this.MaxFiguresPerGeometry)
                {
                    sgc.Close();
                    path.Data = geometry;
                    this.Add(path);
                    path = null;
                    count = 0;
                }
            }

            if (path != null)
            {
                sgc.Close();
                path.Data = geometry;
                this.Add(path);
            }
        }

        /// <summary>
        /// Draws the rectangle.
        /// </summary>
        /// <param name="rect">
        /// The rect.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        public void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            var e = new Rectangle();
            this.SetStroke(e, stroke, thickness, OxyPenLineJoin.Miter, null, true);

            if (fill != null)
            {
                e.Fill = this.GetCachedBrush(fill);
            }

            e.Width = rect.Width;
            e.Height = rect.Height;
            Canvas.SetLeft(e, rect.Left);
            Canvas.SetTop(e, rect.Top);
            this.Add(e);
        }

        /// <summary>
        /// The draw rectangles.
        /// </summary>
        /// <param name="rectangles">
        /// The rectangles.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        public void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            var path = new Path();
            this.SetStroke(path, stroke, thickness);
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
            this.Add(path);

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

        /// <summary>
        /// The draw text.
        /// </summary>
        /// <param name="p">
        /// The p.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="fontFamily">
        /// The font family.
        /// </param>
        /// <param name="fontSize">
        /// The font size.
        /// </param>
        /// <param name="fontWeight">
        /// The font weight.
        /// </param>
        /// <param name="rotate">
        /// The rotate.
        /// </param>
        /// <param name="halign">
        /// The halign.
        /// </param>
        /// <param name="valign">
        /// The valign.
        /// </param>
        /// <param name="maxSize">
        /// The maximum size of the text.
        /// </param>
        public void DrawText(
            ScreenPoint p,
            string text,
            OxyColor fill,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double rotate,
            HorizontalTextAlign halign,
            VerticalTextAlign valign,
            OxySize? maxSize)
        {
            var tb = new TextBlock { Text = text, Foreground = this.GetCachedBrush(fill) };
            if (fontFamily != null)
            {
                tb.FontFamily = new FontFamily(fontFamily);
            }

            if (fontSize > 0)
            {
                tb.FontSize = fontSize;
            }

            if (fontWeight > 0)
            {
                tb.FontWeight = GetFontWeight(fontWeight);
            }

            tb.Measure(new Size(1000, 1000));
            var size = tb.DesiredSize;
            if (maxSize != null)
            {
                if (size.Width > maxSize.Value.Width)
                {
                    size.Width = maxSize.Value.Width;
                }

                if (size.Height > maxSize.Value.Height)
                {
                    size.Height = maxSize.Value.Height;
                }

                tb.Width = size.Width;
                tb.Height = size.Height;
            }

            double dx = 0;
            if (halign == HorizontalTextAlign.Center)
            {
                dx = -size.Width / 2;
            }

            if (halign == HorizontalTextAlign.Right)
            {
                dx = -size.Width;
            }

            double dy = 0;
            if (valign == VerticalTextAlign.Middle)
            {
                dy = -size.Height / 2;
            }

            if (valign == VerticalTextAlign.Bottom)
            {
                dy = -size.Height;
            }

            var transform = new TransformGroup();
            transform.Children.Add(new TranslateTransform(dx, dy));
            if (rotate != 0)
            {
                transform.Children.Add(new RotateTransform(rotate));
            }

            transform.Children.Add(new TranslateTransform(p.X, p.Y));
            tb.RenderTransform = transform;

            tb.SetValue(RenderOptions.ClearTypeHintProperty, ClearTypeHint.Enabled);
            this.ApplyTooltip(tb);
            this.Add(tb);
        }

        /// <summary>
        /// The measure text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The text size.</returns>
        public OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (string.IsNullOrEmpty(text))
            {
                return OxySize.Empty;
            }

            Typeface typeface = new Typeface(
                new FontFamily(fontFamily), FontStyles.Normal, GetFontWeight(fontWeight), FontStretches.Normal);

            GlyphTypeface glyphTypeface;
            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
            {
                throw new InvalidOperationException("No glyph typeface found");
            }

            return MeasureTextSize(glyphTypeface, fontSize, text);
        }

        /// <summary>
        /// Fast text size calculation
        /// </summary>
        /// <param name="glyphTypeface">The glyph typeface.</param>
        /// <param name="sizeInEm">The em size.</param>
        /// <param name="s">The text.</param>
        /// <returns>
        /// The text size.
        /// </returns>
        private static OxySize MeasureTextSize(GlyphTypeface glyphTypeface, double sizeInEm, string s)
        {
            double width = 0;
            double lineWidth = 0;
            int lines = 0;
            foreach (char ch in s)
            {
                switch (ch)
                {
                    case '\n':
                        lines++;
                        if (lineWidth > width)
                        {
                            width = lineWidth;
                        }

                        lineWidth = 0;
                        continue;
                    case '\t':
                        continue;
                }

                ushort glyph = glyphTypeface.CharacterToGlyphMap[ch];
                double advanceWidth = glyphTypeface.AdvanceWidths[glyph];
                lineWidth += advanceWidth;
            }

            lines++;
            if (lineWidth > width)
            {
                width = lineWidth;
            }

            return new OxySize(Math.Round(width * sizeInEm, 2), Math.Round(lines * glyphTypeface.Height * sizeInEm, 2));
        }

        /// <summary>
        /// Sets the tool tip for the following items.
        /// </summary>
        /// <param name="text">
        /// The text in the tooltip.
        /// </param>
        /// <params>
        ///   This is only used in the plot controls.
        /// </params>
        public void SetToolTip(string text)
        {
            this.CurrentToolTip = text;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get font weight.
        /// </summary>
        /// <param name="fontWeight">
        /// The font weight.
        /// </param>
        /// <returns>
        /// </returns>
        private static FontWeight GetFontWeight(double fontWeight)
        {
            return fontWeight > FontWeights.Normal ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Normal;
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Add(FrameworkElement e)
        {
            this.canvas.Children.Add(e);
        }

        /// <summary>
        /// The apply tooltip.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        private void ApplyTooltip(FrameworkElement element)
        {
            if (!string.IsNullOrEmpty(this.CurrentToolTip))
            {
                element.ToolTip = this.CurrentToolTip;
            }
        }

        /// <summary>
        /// Gets the cached brush.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <returns>
        /// The brush.
        /// </returns>
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

        /// <summary>
        /// The set stroke.
        /// </summary>
        /// <param name="shape">
        /// The shape.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        /// <param name="lineJoin">
        /// The line join.
        /// </param>
        /// <param name="dashArray">
        /// The dash array.
        /// </param>
        /// <param name="aliased">
        /// The aliased.
        /// </param>
        private void SetStroke(
            Shape shape,
            OxyColor stroke,
            double thickness,
            OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter,
            double[] dashArray = null,
            bool aliased = false)
        {
            if (stroke != null && thickness > 0)
            {
                shape.Stroke = this.GetCachedBrush(stroke);

                switch (lineJoin)
                {
                    case OxyPenLineJoin.Round:
                        shape.StrokeLineJoin = PenLineJoin.Round;
                        break;
                    case OxyPenLineJoin.Bevel:
                        shape.StrokeLineJoin = PenLineJoin.Bevel;
                        break;

                    // The default StrokeLineJoin is Miter
                }

                if (thickness != 1)
                {
                    // default values is 1
                    shape.StrokeThickness = thickness;
                }

                if (dashArray != null)
                {
                    shape.StrokeDashArray = new DoubleCollection(dashArray);
                }
            }

            if (aliased)
            {
                shape.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                shape.SnapsToDevicePixels = true;
            }
        }

        #endregion
    }
}