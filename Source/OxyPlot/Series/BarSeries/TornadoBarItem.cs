// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TornadoBarItem.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents an item for the TornadoBarSeries.
    /// </summary>
    public class TornadoBarItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="TornadoBarItem" /> class.
        /// </summary>
        public TornadoBarItem()
        {
            this.Minimum = double.NaN;
            this.Maximum = double.NaN;
            this.BaseValue = double.NaN;
            this.MinimumColor = null;
            this.MaximumColor = null;
            this.Label = null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the base value.
        /// </summary>
        public double BaseValue { get; set; }

        /// <summary>
        /// Gets or sets the Label of TornadoBarItem corresponding to the Labels in CategoryAxis.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///   Gets or sets the maximum.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        ///   Gets or sets the color for the maximum bar.
        /// </summary>
        public OxyColor MaximumColor { get; set; }

        /// <summary>
        ///   Gets or sets the minimum value.
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        ///   Gets or sets the color for the minimum bar.
        /// </summary>
        public OxyColor MinimumColor { get; set; }

        #endregion
    }
}