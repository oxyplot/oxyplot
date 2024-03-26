// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextArranger.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Arranges text.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Rendering
{
    /// <summary>
    /// Arranges text.
    /// </summary>
    public interface ITextArranger
    {
        /// <summary>
        /// Gets or sets the <see cref="ITextMeasurer"/> used by this instance.
        /// </summary>
        ITextMeasurer TextMeasurer { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ITextTrimmer"/> used by this instance.
        /// </summary>
        ITextTrimmer TextTrimmer { get; set; }

        /// <summary>
        /// Splits the text into multiple lines, and indicates where they should be rendered given the given target alignment.
        /// </summary>
        /// <param name="p">The position.</param>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font (in device independent units, 1/96 inch).</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotation">The rotation angle.</param>
        /// <param name="horizontalAlignment">The horizontal alignment.</param>
        /// <param name="verticalAlignment">The vertical alignment.</param>
        /// <param name="maxSize">The maximum size of the text (in device independent units, 1/96 inch). If set to <c>null</c>, the text will not be clipped.</param>
        /// <param name="targetHorizontalAlignment">The horizontal alignment used to render the text.</param>
        /// <param name="targetVerticalAlignment">The vertical alignment used to render the text.</param>
        /// <param name="lines">The separate lines of text.</param>
        /// <param name="linePositions">The point at which to render each line.</param>
        void ArrangeText(ScreenPoint p, string text, string fontFamily, double fontSize, double fontWeight, double rotation, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, OxySize? maxSize, HorizontalAlignment targetHorizontalAlignment, TextVerticalAlignment targetVerticalAlignment, out string[] lines, out ScreenPoint[] linePositions);

        /// <summary>
        /// Measures the size of the text as it would be when arranged by the ArrangeText method."/>.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font (in device independent units, 1/96 inch).</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The size of the text.</returns>
        /// <remarks>Returns an <see cref="OxySize"/> with no width and height if the text is <c>null</c> or empty.</remarks>
        OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight);
    }
}
