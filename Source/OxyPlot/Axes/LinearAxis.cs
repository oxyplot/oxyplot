namespace OxyPlot
{
    /// <summary>
    /// Linear axis class.
    /// </summary>
    public class LinearAxis : Axis
    {


        public LinearAxis()
        {

        }

        public LinearAxis(AxisPosition pos, double minimum = double.NaN, double maximum = double.NaN, string title = null)
            : this(pos, minimum, maximum, double.NaN, double.NaN, title)
        {
        }

        public LinearAxis(AxisPosition pos, double minimum, double maximum, double majorStep, double minorStep, string title = null)
            : this()
        {
            Title = title;
            Position = pos;
            Minimum = minimum;
            Maximum = maximum;
            MajorStep = majorStep;
            MinorStep = minorStep;
        }

    }
}