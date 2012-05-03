// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntervalBarItem.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents an interval in an IntervalBarSeries.
    /// </summary>
    public class IntervalBarItem
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the color.
        /// </summary>
        public OxyColor Color { get; set; }

        /// <summary>
        ///   Gets or sets the end value.
        /// </summary>
        public double End { get; set; }

        /// <summary>
        /// Gets or sets the Label of IntervalBarItem corresponding to the Labels in CategoryAxis.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///   Gets or sets the start value.
        /// </summary>
        public double Start { get; set; }

        /// <summary>
        ///   Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        #endregion
    }
}