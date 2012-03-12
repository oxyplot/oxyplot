// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleBarItem.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents a rectangle item in a RectangleBarSeries.
    /// </summary>
    public class RectangleBarItem
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the color.
        /// </summary>
        public OxyColor Color { get; set; }

        /// <summary>
        ///   Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///   Gets or sets the x0 coordinate.
        /// </summary>
        public double X0 { get; set; }

        /// <summary>
        ///   Gets or sets the x1 coordinate.
        /// </summary>
        public double X1 { get; set; }

        /// <summary>
        ///   Gets or sets the y0 coordinate.
        /// </summary>
        public double Y0 { get; set; }

        /// <summary>
        ///   Gets or sets the y1 coordinate.
        /// </summary>
        public double Y1 { get; set; }

        #endregion
    }
}