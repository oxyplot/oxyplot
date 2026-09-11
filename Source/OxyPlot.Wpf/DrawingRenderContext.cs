// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawingRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Implements <see cref="IRenderContext" /> for <see cref="RenderSurface" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using HorizontalAlignment = OxyPlot.HorizontalAlignment;
    using VerticalAlignment = OxyPlot.VerticalAlignment;

    /// <summary>
    /// Implements <see cref="IRenderContext" /> for <see cref="RenderSurface" />.
    /// </summary>
    public class DrawingRenderContext : WpfRenderContext
    {
        /// <summary>
        /// The canvas.
        /// </summary>
        private readonly RenderSurface _renderSurface;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingRenderContext" /> class.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public DrawingRenderContext(RenderSurface canvas)
        {
            this._renderSurface = canvas;
            this.TextFormattingMode = TextFormattingMode.Display;
            this.TextMeasurementMethod = TextMeasurementMethod.TextBlock;
            this.RendersToScreen = true;
        }

        /// <inheritdoc />
        protected override void SetClip(OxyRect clippingRectangle)
        {
            this._renderSurface.SetClip(ToRect(clippingRectangle));
        }

        /// <inheritdoc />
        protected override void ResetClip()
        {
            this._renderSurface.ResetClip();
        }

        ///<inheritdoc/>
        public override void DrawEllipses(
            IList<OxyRect> rectangles,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode)
        {
            if (rectangles.Count == 0)
            {
                return;
            }

            double actualStrokeThickness = this.GetActualStrokeThickness(thickness, edgeRenderingMode);

            Pen pen = this.GetPen_(stroke, actualStrokeThickness);
            Brush fillBrush = null;
            if (!fill.IsUndefined())
            {
                fillBrush = this.GetCachedBrush(fill);
            }
            foreach (var rect in rectangles)
            {
                var geometry = new EllipseGeometry(new Point(rect.Center.X, rect.Center.Y), rect.Width / 2, rect.Height / 2);
                this._renderSurface.DrawEllipse(fillBrush, pen, geometry.Center, geometry.RadiusX, geometry.RadiusY, GetEdgeMode(edgeRenderingMode));
                if (!string.IsNullOrEmpty(this._currentToolTip))
                {
                    this._renderSurface.RegisterTooltipRegion(geometry, this._currentToolTip);
                }
            }
        }

        ///<inheritdoc/>
        public override void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            if (points.Count < 2)
            {
                return;
            }

            var actualStrokeThickness = this.GetActualStrokeThickness(thickness, edgeRenderingMode);
            var actualPoints = this.GetActualPoints(points, actualStrokeThickness, edgeRenderingMode);
            var streamGeometry = new StreamGeometry();
            using (var streamGeometryContext = streamGeometry.Open())
            {
                streamGeometryContext.BeginFigure(actualPoints.First(), false, false);
                streamGeometryContext.PolyLineTo(actualPoints.Skip(1).ToArray(), !stroke.IsUndefined(), false);
            }
            streamGeometry.Freeze();
            var pen = this.GetPen_(stroke, actualStrokeThickness, lineJoin, dashArray);

            this._renderSurface.DrawGeometry(null, pen, streamGeometry, GetEdgeMode(edgeRenderingMode));
            if (!string.IsNullOrEmpty(this._currentToolTip))
            {
                this._renderSurface.RegisterTooltipRegion(streamGeometry, this._currentToolTip);
            }
        }

        ///<inheritdoc/>
        public override void DrawLineSegments(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            if (points.Count < 2)
            {
                return;
            }

            var actualStrokeThickness = this.GetActualStrokeThickness(thickness, edgeRenderingMode);
            var actualPoints = this.GetActualPoints(points, actualStrokeThickness, edgeRenderingMode).ToArray();
            var streamGeometry = new StreamGeometry();
            using (var streamGeometryContext = streamGeometry.Open())
            {
                streamGeometryContext.BeginFigure(actualPoints[0], false, false);
                for (var i = 1; i < actualPoints.Length; i++)
                {
                    streamGeometryContext.LineTo(actualPoints[i], (i & 0x1) == 1, false);
                }
            }
            streamGeometry.Freeze();

            var pen = this.GetPen_(stroke, actualStrokeThickness, lineJoin, dashArray);

            this._renderSurface.DrawGeometry(null, pen, streamGeometry, GetEdgeMode(edgeRenderingMode));
            if (!string.IsNullOrEmpty(this._currentToolTip))
            {
                this._renderSurface.RegisterTooltipRegion(streamGeometry, this._currentToolTip);
            }
        }

        ///<inheritdoc/>
        public override void DrawPolygons(
            IList<IList<ScreenPoint>> polygons,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            if (polygons.Count == 0)
            {
                return;
            }

            double actualStrokeThickness = this.GetActualStrokeThickness(thickness, edgeRenderingMode);

            Pen pen = this.GetPen_(stroke, actualStrokeThickness, lineJoin, dashArray);
            Brush fillBrush = null;
            if (!fill.IsUndefined())
            {
                fillBrush = this.GetCachedBrush(fill);
            }

            StreamGeometry streamGeometry = new StreamGeometry
            {
                FillRule = FillRule.Nonzero
            };
            using (StreamGeometryContext streamGeometryContext = streamGeometry.Open())
            {
                foreach (IList<ScreenPoint> polygon in polygons)
                {
                    var actualPoints = this.GetActualPoints(polygon, actualStrokeThickness, edgeRenderingMode);
                    streamGeometryContext.BeginFigure(actualPoints.First(), !fill.IsUndefined(), true);
                    streamGeometryContext.PolyLineTo(actualPoints.Skip(1).ToArray(), !stroke.IsUndefined(), false);
                }
            }
            streamGeometry.Freeze();
            this._renderSurface.DrawGeometry(fillBrush, pen, streamGeometry, GetEdgeMode(edgeRenderingMode));
            if (!string.IsNullOrEmpty(this._currentToolTip))
            {
                this._renderSurface.RegisterTooltipRegion(streamGeometry, this._currentToolTip);
            }
        }

        ///<inheritdoc/>
        public override void DrawRectangles(
            IList<OxyRect> rectangles,
            OxyColor fill, OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode)
        {
            if (rectangles.Count == 0)
            {
                return;
            }

            double actualStrokeThickness = this.GetActualStrokeThickness(thickness, edgeRenderingMode);
            Brush fillBrush = null;
            Pen pen = this.GetPen_(stroke, actualStrokeThickness);
            if (!fill.IsUndefined())
            {
                fillBrush = this.GetCachedBrush(fill);
            }

            foreach (var rect in rectangles)
            {
                var r = this.GetActualRect(rect, thickness, edgeRenderingMode);
                this._renderSurface.DrawRectangle(fillBrush, pen, r, GetEdgeMode(edgeRenderingMode));
                if (!string.IsNullOrEmpty(this._currentToolTip))
                {
                    this._renderSurface.RegisterTooltipRegion(new RectangleGeometry(r), this._currentToolTip);
                }
            }
        }

        ///<inheritdoc/>
        public override void DrawText(
            ScreenPoint p,
            string text,
            OxyColor fill,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double rotate,
            HorizontalAlignment halign,
            VerticalAlignment valign,
            OxySize? maxSize)
        {
            double dx = 0;
            double dy = 0;
            OxySize measuredSize = default;

            if (maxSize != null || halign != HorizontalAlignment.Left || valign != VerticalAlignment.Top || !string.IsNullOrEmpty(this._currentToolTip))
            {
                measuredSize = this.MeasureText(text, fontFamily, fontSize, fontWeight);
                Size size = new Size(measuredSize.Width, measuredSize.Height);
                if (maxSize.HasValue)
                {
                    if (size.Width > maxSize.Value.Width + 0.001)
                    {
                        size.Width = Math.Max(maxSize.Value.Width, 0.0);
                    }

                    if (size.Height > maxSize.Value.Height + 0.001)
                    {
                        size.Height = Math.Max(maxSize.Value.Height, 0.0);
                    }
                }

                if (halign == HorizontalAlignment.Center)
                    dx = -size.Width / 2.0;
                else if (halign == HorizontalAlignment.Right)
                    dx = -size.Width;
                if (valign == VerticalAlignment.Middle)
                    dy = -size.Height / 2.0;
                else if (valign == VerticalAlignment.Bottom)
                    dy = -size.Height;
            }

            var transform = new TransformGroup();
            transform.Children.Add(new TranslateTransform(dx, dy));
            if (Math.Abs(rotate) > double.Epsilon)
            {
                transform.Children.Add(new RotateTransform(rotate, p.X, p.Y));
            }

            if (maxSize != null)
            {
                var clipRectangle = new Rect(p.X, p.Y, maxSize.Value.Width, maxSize.Value.Height);
                clipRectangle = transform.TransformBounds(clipRectangle);

                this.PushClip(new OxyRect(clipRectangle.Left, clipRectangle.Top, clipRectangle.Width, clipRectangle.Height));
            }

            this._renderSurface.DrawText(new Point(p.X, p.Y), text, this.GetCachedBrush(fill), this.GetCachedFontFamily(fontFamily),
                fontSize,
                GetFontWeight(fontWeight), this.TextFormattingMode, transform);

            if (!string.IsNullOrEmpty(this._currentToolTip))
            {
                var textGeom = new RectangleGeometry(new Rect(p.X, p.Y, measuredSize.Width, measuredSize.Height))
                {
                    Transform = transform
                };
                this._renderSurface.RegisterTooltipRegion(textGeom, this._currentToolTip);
            }

            if (maxSize != null)
            {
                this.PopClip();
            }
        }

        ///<inheritdoc/>
        public override void DrawImage(
            OxyImage source,
            double srcX,
            double srcY,
            double srcWidth,
            double srcHeight,
            double destX,
            double destY,
            double destWidth,
            double destHeight,
            double opacity,
            bool interpolate)
        {
            if (destWidth <= 0 || destHeight <= 0 || srcWidth <= 0 || srcHeight <= 0)
            {
                return;
            }

            var bitmapChain = this.GetImageSource(source);

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (srcX == 0 && srcY == 0 && srcWidth == bitmapChain.PixelWidth && srcHeight == bitmapChain.PixelHeight)
            // ReSharper restore CompareOfFloatsByEqualityOperator
            {
                // do not crop
            }
            else
            {
                bitmapChain = new CroppedBitmap(bitmapChain, new Int32Rect((int)srcX, (int)srcY, (int)srcWidth, (int)srcHeight));
            }

            this._renderSurface.DrawImage(bitmapChain, destX, destY, destWidth, destHeight, opacity, interpolate ? BitmapScalingMode.HighQuality : BitmapScalingMode.NearestNeighbor);
            if (!string.IsNullOrEmpty(this._currentToolTip))
            {
                this._renderSurface.RegisterTooltipRegion(new RectangleGeometry(new Rect(destX, destY, destWidth, destHeight)), this._currentToolTip);
            }
        }

        private static EdgeMode GetEdgeMode(EdgeRenderingMode edgeRenderingMode)
        {
            return edgeRenderingMode == EdgeRenderingMode.PreferSpeed ? EdgeMode.Aliased : EdgeMode.Unspecified;
        }

        private Pen GetPen_(OxyColor stroke, double thickness, LineJoin lineJoin = LineJoin.Miter, double[] dashArray = null, double dashOffset = 0)
        {
            var pen = new Pen
            {
                EndLineCap = PenLineCap.Flat,
                StartLineCap = PenLineCap.Flat,
                DashCap = PenLineCap.Flat
            };

            if (!stroke.IsUndefined() && thickness > 0.0)
            {
                pen.Brush = this.GetCachedBrush(stroke);

                pen.LineJoin = lineJoin switch
                {
                    LineJoin.Round => PenLineJoin.Round,
                    LineJoin.Bevel => PenLineJoin.Bevel,
                    LineJoin.Miter => PenLineJoin.Miter,
                    _ => pen.LineJoin
                };

                pen.Thickness = thickness;
                if (dashArray != null)
                {
                    pen.DashStyle = new DashStyle(dashArray, dashOffset);
                }
            }
            else
            {
                pen.Thickness = 0.0;
            }

            pen.Freeze();
            return pen;
        }
    }
}
