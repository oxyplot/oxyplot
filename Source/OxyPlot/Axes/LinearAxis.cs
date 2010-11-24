namespace OxyPlot
{
    /// <summary>
    /// Linear axis class.
    /// </summary>
    public class LinearAxis : Axis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearAxis"/> class.
        /// </summary>
        public LinearAxis()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearAxis"/> class.
        /// </summary>
        /// <param name="pos">The pos.</param>
        /// <param name="title">The title.</param>
        public LinearAxis(AxisPosition pos, string title)
            :this()
        {
            this.Position = pos;
            this.Title = title;
        }

        public LinearAxis(AxisPosition pos, double minimum = double.NaN, double maximum = double.NaN, string title = null)
            : this(pos, minimum, maximum, double.NaN, double.NaN, title)
        {
        }

        public LinearAxis(AxisPosition pos, double minimum, double maximum, double majorStep, double minorStep, string title = null)
            : this(pos,title)
        {
            Minimum = minimum;
            Maximum = maximum;
            MajorStep = majorStep;
            MinorStep = minorStep;
        }

    }
}