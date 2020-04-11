// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderContextBase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for rendering contexts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides an abstract base class for rendering contexts.
    /// </summary>
    public abstract class RenderContextBase : IRenderContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderContextBase" /> class.
        /// </summary>
        protected RenderContextBase()
        {
            this.RendersToScreen = true;
        }

        /// <summary>
        /// Gets a value indicating whether the specified points form a straight line (i.e. parallel to the pixel raster).
        /// </summary>
        /// <remarks>
        /// To determine whether a line is straight, the coordinates of <paramref name="p1"/> and <paramref name="p2"/> are compared. If either the X or the Y
        /// coordinates (or both) of both points are very close together, the line is considered straight. The threshold of what is considered 'very close' 
        /// is fixed at 1e-5.
        /// </remarks>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <returns>true if the points form a straight line; false otherwise.</returns>
        public static bool IsStraightLine(ScreenPoint p1, ScreenPoint p2)
        {
            const double epsilon = 1e-5;
            return Math.Abs(p1.X - p2.X) < epsilon || Math.Abs(p1.Y - p2.Y) < epsilon;
        }

        /// <summary>
        /// Gets a value indicating whether the specified points form a straight line (i.e. parallel to the pixel raster).
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>true if the points form a straight line; false otherwise.</returns>
        public static bool IsStraightLine(IList<ScreenPoint> points)
        {
            for (int i = 1; i < points.Count; i++)
            {
                if (!IsStraightLine(points[i - 1], points[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the context renders to screen.
        /// </summary>
        /// <value><c>true</c> if the context renders to screen; otherwise, <c>false</c>.</value>
        public bool RendersToScreen { get; set; }

        /// <inheritdoc/>
        public virtual void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            var polygon = CreateEllipse(rect);
            this.DrawPolygon(polygon, fill, stroke, thickness, edgeRenderingMode, null, LineJoin.Miter);
        }

        /// <inheritdoc/>
        public virtual void DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            foreach (var r in rectangles)
            {
                this.DrawEllipse(r, fill, stroke, thickness, edgeRenderingMode);
            }
        }

        /// <inheritdoc/>
        public abstract void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin);

        /// <inheritdoc/>
        public virtual void DrawLineSegments(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness, 
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            for (int i = 0; i + 1 < points.Count; i += 2)
            {
                this.DrawLine(new[] { points[i], points[i + 1] }, stroke, thickness, edgeRenderingMode, dashArray, lineJoin);
            }
        }

        /// <inheritdoc/>
        public abstract void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin);

        /// <inheritdoc/>
        public virtual void DrawPolygons(
            IList<IList<ScreenPoint>> polygons,
            OxyColor fill,
            OxyColor stroke,
            double thickness, 
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            foreach (var polygon in polygons)
            {
                this.DrawPolygon(polygon, fill, stroke, thickness, edgeRenderingMode, dashArray, lineJoin);
            }
        }

        /// <inheritdoc/>
        public virtual void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            var polygon = CreateRectangle(rect);
            this.DrawPolygon(polygon, fill, stroke, thickness, edgeRenderingMode, null, LineJoin.Miter);
        }

        /// <inheritdoc/>
        public virtual void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            foreach (var r in rectangles)
            {
                this.DrawRectangle(r, fill, stroke, thickness, edgeRenderingMode);
            }
        }

        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="p">The position of the text.</param>
        /// <param name="text">The text.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotate">The rotation angle.</param>
        /// <param name="halign">The horizontal alignment.</param>
        /// <param name="valign">The vertical alignment.</param>
        /// <param name="maxSize">The maximum size of the text.</param>
        public abstract void DrawText(
            ScreenPoint p,
            string text,
            OxyColor fill,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double rotate,
            HorizontalAlignment halign,
            VerticalAlignment valign,
            OxySize? maxSize);

        /// <summary>
        /// Measures the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The text size.</returns>
        public abstract OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight);

        /// <summary>
        /// Sets the tool tip for the following items.
        /// </summary>
        /// <param name="text">The text in the tooltip.</param>
        public virtual void SetToolTip(string text)
        {
        }

        /// <summary>
        /// Cleans up resources not in use.
        /// </summary>
        /// <remarks>This method is called at the end of each rendering.</remarks>
        public virtual void CleanUp()
        {
        }

        /// <summary>
        /// Draws the specified portion of the specified <see cref="OxyImage" /> at the specified location and with the specified size.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw.</param>
        /// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw.</param>
        /// <param name="srcWidth">Width of the portion of the source image to draw.</param>
        /// <param name="srcHeight">Height of the portion of the source image to draw.</param>
        /// <param name="destX">The x-coordinate of the upper-left corner of drawn image.</param>
        /// <param name="destY">The y-coordinate of the upper-left corner of drawn image.</param>
        /// <param name="destWidth">The width of the drawn image.</param>
        /// <param name="destHeight">The height of the drawn image.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="interpolate">Interpolate if set to <c>true</c>.</param>
        public virtual void DrawImage(
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
        }

        /// <summary>
        /// Sets the clip rectangle.
        /// </summary>
        /// <param name="rect">The clip rectangle.</param>
        /// <returns>True if the clip rectangle was set.</returns>
        public virtual bool SetClip(OxyRect rect)
        {
            return false;
        }

        /// <summary>
        /// Resets the clip rectangle.
        /// </summary>
        public virtual void ResetClip()
        {
        }

        /// <summary>
        /// Creates an ellipse polygon.
        /// </summary>
        /// <param name="rect">The bounding rectangle.</param>
        /// <param name="n">The number of points.</param>
        /// <returns>The points defining the ellipse.</returns>
        /// <remarks>Note that this is very slow, not optimized in any way.</remarks>
        protected static ScreenPoint[] CreateEllipse(OxyRect rect, int n = 40)
        {
            double cx = rect.Center.X;
            double cy = rect.Center.Y;
            double dx = rect.Width / 2;
            double dy = rect.Height / 2;
            var points = new ScreenPoint[n];
            for (int i = 0; i < n; i++)
            {
                double t = Math.PI * 2 * i / n;
                points[i] = new ScreenPoint(cx + (Math.Cos(t) * dx), cy + (Math.Sin(t) * dy));
            }

            return points;
        }

        /// <summary>
        /// Creates a rectangle polygon.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>The points defining the rectangle.</returns>
        protected static ScreenPoint[] CreateRectangle(OxyRect rect)
        {
            return new[]
                       {
                           new ScreenPoint(rect.Left, rect.Top), new ScreenPoint(rect.Left, rect.Bottom),
                           new ScreenPoint(rect.Right, rect.Bottom), new ScreenPoint(rect.Right, rect.Top)
                       };
        }

        /// <summary>
        /// Returns a value indicating whether anti-aliasing should be used for the given edge rendering mode.
        /// </summary>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>true if anti-aliasing should be used; false otherwise.</returns>
        protected virtual bool ShouldUseAntiAliasingForRect(EdgeRenderingMode edgeRenderingMode)
        {
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.PreferGeometricAccuracy:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns a value indicating whether anti-aliasing should be used for the given edge rendering mode.
        /// </summary>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>true if anti-aliasing should be used; false otherwise.</returns>
        protected virtual bool ShouldUseAntiAliasingForEllipse(EdgeRenderingMode edgeRenderingMode)
        {
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.PreferSpeed:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Returns a value indicating whether anti-aliasing should be used for the given edge rendering mode.
        /// </summary>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <param name="points">The points.</param>
        /// <returns>true if anti-aliasing should be used; false otherwise.</returns>
        protected virtual bool ShouldUseAntiAliasingForLine(EdgeRenderingMode edgeRenderingMode, IList<ScreenPoint> points)
        {
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.PreferSpeed:
                case EdgeRenderingMode.PreferSharpness:
                case EdgeRenderingMode.Automatic when IsStraightLine(points):
                case EdgeRenderingMode.Adaptive when IsStraightLine(points):
                    return false;
                default:
                    return true;
            }
        }
    }
}
