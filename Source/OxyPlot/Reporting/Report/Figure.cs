// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Figure.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents a figure (abstract base class for DrawingFigure, Image and PlotFigure).
    /// </summary>
    public abstract class Figure : ReportItem
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets FigureNumber.
        /// </summary>
        public int FigureNumber { get; set; }

        /// <summary>
        ///   Gets or sets FigureText.
        /// </summary>
        public string FigureText { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The get full caption.
        /// </summary>
        /// <param name="style">
        /// The style.
        /// </param>
        /// <returns>
        /// The get full caption.
        /// </returns>
        public string GetFullCaption(ReportStyle style)
        {
            return string.Format(style.FigureTextFormatString, this.FigureNumber, this.FigureText);
        }

        #endregion
    }
}