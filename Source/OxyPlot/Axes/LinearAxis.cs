namespace OxyPlot
{
    /// <summary>
    ///   Linear axis class.
    /// </summary>
    public class LinearAxis : AxisBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether to format numbers as fractions.
        /// </summary>
        public bool FormatAsFractions { get; set; }

        /// <summary>
        /// Gets or sets the fraction unit.
        /// Remember to set FormatAsFractions to true.
        /// </summary>
        /// <value>The fraction unit.</value>
        public double FractionUnit { get; set; }

        /// <summary>
        /// Gets or sets the fraction unit symbol.
        /// Use FractionUnit = Math.PI and FractionUnitSymbol = "π" if you want the axis to show "π/2,π,3π/2,2π" etc.
        /// Use FractionUnit = 1 and FractionUnitSymbol = "L" if you want the axis to show "0,L/2,L" etc.
        /// Remember to set FormatAsFractions to true.
        /// </summary>
        /// <value>The fraction unit symbol.</value>
        public string FractionUnitSymbol { get; set; }


        /// <summary>
        ///   Initializes a new instance of the <see cref = "LinearAxis" /> class.
        /// </summary>
        public LinearAxis()
        {
            FractionUnit = 1.0;
            FractionUnitSymbol = null;
            FormatAsFractions = false;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LinearAxis" /> class.
        /// </summary>
        /// <param name = "pos">The pos.</param>
        /// <param name = "title">The title.</param>
        public LinearAxis(AxisPosition pos, string title)
            : this()
        {
            Position = pos;
            Title = title;
        }

        public LinearAxis(AxisPosition pos, double minimum = double.NaN, double maximum = double.NaN,
                          string title = null)
            : this(pos, minimum, maximum, double.NaN, double.NaN, title)
        {
        }

        public LinearAxis(AxisPosition pos, double minimum, double maximum, double majorStep, double minorStep,
                          string title = null)
            : this(pos, title)
        {
            Minimum = minimum;
            Maximum = maximum;
            MajorStep = majorStep;
            MinorStep = minorStep;
        }

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        /// <param name="x">The value.</param>
        /// <returns>The formatted value.</returns>
        public override string FormatValue(double x)
        {
            if (FormatAsFractions) return FractionHelper.ConvertToFractionString(x, FractionUnit, FractionUnitSymbol, 1e-6);
            return base.FormatValue(x);
        }
    }
}