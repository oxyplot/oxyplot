// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TornadoBarItem.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Represents an item for the TornadoBarSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents an item for the TornadoBarSeries.
    /// </summary>
    public class TornadoBarItem : CategorizedItem, ICodeGenerating
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TornadoBarItem"/> class.
        /// </summary>
        public TornadoBarItem()
        {
            this.Minimum = double.NaN;
            this.Maximum = double.NaN;
            this.BaseValue = double.NaN;
            this.MinimumColor = null;
            this.MaximumColor = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TornadoBarItem"/> class.
        /// </summary>
        /// <param name="minimum">
        /// The minimum. 
        /// </param>
        /// <param name="maximum">
        /// The maximum. 
        /// </param>
        /// <param name="baseValue">
        /// The base value. 
        /// </param>
        /// <param name="minimumColor">
        /// The minimum color. 
        /// </param>
        /// <param name="maximumColor">
        /// The maximum color. 
        /// </param>
        public TornadoBarItem(
            double minimum, 
            double maximum, 
            double baseValue = double.NaN, 
            OxyColor minimumColor = null, 
            OxyColor maximumColor = null)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.BaseValue = baseValue;
            this.MinimumColor = minimumColor;
            this.MaximumColor = maximumColor;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the base value.
        /// </summary>
        public double BaseValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the color for the maximum bar.
        /// </summary>
        public OxyColor MaximumColor { get; set; }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        /// Gets or sets the color for the minimum bar.
        /// </summary>
        public OxyColor MinimumColor { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns c# code that generates this instance.
        /// </summary>
        /// <returns>
        /// C# code. 
        /// </returns>
        public string ToCode()
        {
            if (this.MaximumColor != null)
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(), 
                    "{0},{1},{2},{3},{4}", 
                    this.Minimum, 
                    this.Maximum, 
                    this.BaseValue, 
                    this.MinimumColor.ToCode(), 
                    this.MaximumColor.ToCode());
            }

            if (this.MinimumColor != null)
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(), 
                    "{0},{1},{2},{3}", 
                    this.Minimum, 
                    this.Maximum, 
                    this.BaseValue, 
                    this.MinimumColor.ToCode());
            }

            if (!double.IsNaN(this.BaseValue))
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(), "{0},{1},{2}", this.Minimum, this.Maximum, this.BaseValue);
            }

            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.Minimum, this.Maximum);
        }

        #endregion
    }
}