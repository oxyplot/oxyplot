// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a render context that does nothing except measure text.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a render context that does nothing except measure text.
    /// </summary>
    public class NullRenderContext : RenderContextBase
    {
        /// <summary>
        /// The text measurer
        /// </summary>
        private readonly IRenderContext textMeasurer = new PdfRenderContext(1, 1, OxyColors.White);

        /// <inheritdoc/>
        public override void DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode, double[] dashArray, LineJoin lineJoin)
        {
            // do nothing
        }

        /// <inheritdoc/>
        public override void DrawPolygon(IList<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode, double[] dashArray, LineJoin lineJoin)
        {
            // do nothing
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
        public override void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily, double fontSize, double fontWeight, double rotate, HorizontalAlignment halign, VerticalAlignment valign, OxySize? maxSize)
        {
            // do nothing
        }

        /// <summary>
        /// Measures the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>
        /// The text size.
        /// </returns>
        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            // Use the Pdf text measurer
            return this.textMeasurer.MeasureText(text, fontFamily, fontSize, fontWeight);
        }
    }
}
