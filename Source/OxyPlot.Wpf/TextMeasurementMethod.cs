// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextMeasurementMethod.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The text measurement methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    /// <summary>
    /// The text measurement methods.
    /// </summary>
    public enum TextMeasurementMethod
    {
        /// <summary>
        /// Measurement by TextBlock. This gives a more accurate result than <see cref="GlyphTypeface"/> as it takes into account text shaping.
        /// </summary>
        TextBlock,

        /// <summary>
        /// Measurement by glyph typeface. This is faster than <see cref="TextBlock"/>, but does not take into account text shaping.
        /// </summary>
        GlyphTypeface
    }
}
