// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextVerticalAlignment.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Contains metrics for a given font.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Rendering
{
    using System;

    /// <summary>
    /// Contains metrics for a given font.
    /// </summary>
    public class FontMetrics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FontMetrics" /> class.
        /// </summary>
        /// <param name="ascender">The ascender.</param>
        /// <param name="descender">The descender.</param>
        /// <param name="leading">The leading.</param>
        public FontMetrics(double ascender, double descender, double leading)
        {
            if (ascender < 0)
            {
                throw new ArgumentException("Ascender must be non-negative.", nameof(ascender));
            }

            if (descender < 0)
            {
                throw new ArgumentException("Descender must be non-negative.", nameof(descender));
            }

            this.Ascender = ascender;
            this.Descender = descender;
            this.Leading = leading;
        }

        /// <summary>
        /// Gets the distance from the baseline to the top of the font.
        /// </summary>
        public double Ascender { get; }

        /// <summary>
        /// Gets the distance from the baseline to the bottom of the font.
        /// </summary>
        public double Descender { get; }

        /// <summary>
        /// Gets the distance between the bottom of a line of text and the top of the next line of text.
        /// </summary>
        public double Leading { get; }

        /// <summary>
        /// Gets the cell height of the font, equal to the sum of the <see cref="Ascender"/> and <see cref="Descender"/>.
        /// </summary>
        public double CellHeight => this.Ascender + this.Descender;

        /// <summary>
        /// Gets the line height of the font, equal to the sum of the <see cref="CellHeight"/> and <see cref="Leading"/>.
        /// </summary>
        public double LineHeight => this.CellHeight + this.Leading;
    }
}
