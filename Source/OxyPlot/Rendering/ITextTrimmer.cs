// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextTrimmer.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Trims text.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Rendering
{
    /// <summary>
    /// Trims text.
    /// </summary>
    public interface ITextTrimmer
    {
        /// <summary>
        /// Trims the given line of text so that it fits in the given width.
        /// </summary>
        /// <param name="textMeasurer">The <see cref="ITextMeasurer"/> to use.</param>
        /// <param name="line">The line of text to trim.</param>
        /// <param name="width">The width in which the text must fix.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size in 1/96ths of an inch.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The trimmed line of text.</returns>
        string Trim(ITextMeasurer textMeasurer, string line, double width, string fontFamily, double fontSize, double fontWeight);
    }
}
