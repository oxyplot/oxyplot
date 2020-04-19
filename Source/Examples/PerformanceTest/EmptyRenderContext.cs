// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an empty <see cref="IRenderContext" /> that does nothing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PerformanceTest
{
    using System.Collections.Generic;

    using OxyPlot;

    /// <summary>
    /// Provides an empty <see cref="IRenderContext" /> that does nothing.
    /// </summary>
    public class EmptyRenderContext : IRenderContext
    {
        /// <inheritdoc/>
        public bool RendersToScreen { get; set; } = true;

        /// <inheritdoc/>
        public void CleanUp()
        {
        }

        /// <inheritdoc/>
        public void DrawEllipse(OxyRect extents, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
        }

        /// <inheritdoc/>
        public void DrawEllipses(IList<OxyRect> extents, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
        }

        /// <inheritdoc/>
        public void DrawImage(OxyImage source, double srcX, double srcY, double srcWidth, double srcHeight, double destX, double destY, double destWidth, double destHeight, double opacity, bool interpolate)
        {
        }

        /// <inheritdoc/>
        public void DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode, double[] dashArray = null, LineJoin lineJoin = LineJoin.Miter)
        {
        }

        /// <inheritdoc/>
        public void DrawLineSegments(IList<ScreenPoint> points, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode, double[] dashArray = null, LineJoin lineJoin = LineJoin.Miter)
        {
        }

        /// <inheritdoc/>
        public void DrawPolygon(IList<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode, double[] dashArray = null, LineJoin lineJoin = LineJoin.Miter)
        {
        }

        /// <inheritdoc/>
        public void DrawPolygons(IList<IList<ScreenPoint>> polygons, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode, double[] dashArray = null, LineJoin lineJoin = LineJoin.Miter)
        {
        }

        /// <inheritdoc/>
        public void DrawRectangle(OxyRect rectangle, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
        }

        /// <inheritdoc/>
        public void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
        }

        /// <inheritdoc/>
        public void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily = null, double fontSize = 10, double fontWeight = 400, double rotation = 0, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, VerticalAlignment verticalAlignment = VerticalAlignment.Top, OxySize? maxSize = null)
        {
        }

        /// <inheritdoc/>
        public OxySize MeasureText(string text, string fontFamily = null, double fontSize = 10, double fontWeight = 500)
        {
            return OxySize.Empty;
        }

        /// <inheritdoc/>
        public void ResetClip()
        {
        }

        /// <inheritdoc/>
        public bool SetClip(OxyRect clippingRectangle)
        {
            return true;
        }

        /// <inheritdoc/>
        public void SetToolTip(string text)
        {
        }
    }
}
