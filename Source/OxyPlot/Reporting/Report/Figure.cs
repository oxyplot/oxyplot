// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Figure.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a base class for figures (drawings, images and plots).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Provides a base class for figures (drawings, images and plots).
    /// </summary>
    public abstract class Figure : ReportItem
    {
        /// <summary>
        /// Gets or sets the figure number.
        /// </summary>
        public int FigureNumber { get; set; }

        /// <summary>
        /// Gets or sets the figure text.
        /// </summary>
        /// <remarks>No figure text will be shown if set to <c>null</c>.
        /// A figure number will be counted if the figure text is not <c>null</c>.</remarks>
        public string FigureText { get; set; }

        /// <summary>
        /// Gets the full caption for the figure.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The caption string.</returns>
        public string GetFullCaption(ReportStyle style)
        {
            return string.Format(style.FigureTextFormatString, this.FigureNumber, this.FigureText);
        }
    }
}