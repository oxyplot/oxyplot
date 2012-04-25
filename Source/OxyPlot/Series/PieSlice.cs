// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieSlice.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represent a slice of a PieSeries.
    /// </summary>
    public class PieSlice
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PieSlice" /> class.
        /// </summary>
        public PieSlice()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PieSlice"/> class.
        /// </summary>
        /// <param name="label">
        /// The label.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        public PieSlice(string label, double value, OxyColor fill = null)
        {
            this.Label = label;
            this.Value = value;
            this.Fill = fill;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets Fill.
        /// </summary>
        public OxyColor Fill { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether IsExploded.
        /// </summary>
        public bool IsExploded { get; set; }

        /// <summary>
        ///   Gets or sets Label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///   Gets or sets Value.
        /// </summary>
        public double Value { get; set; }

        #endregion
    }
}