namespace OxyPlot
{
    /// <summary>
    /// Linear axis class.
    /// </summary>
    public class LinearAxis : RangeAxis
    {


        public LinearAxis()
        {

        }

        public LinearAxis(AxisPosition pos, double minimum, double maximum)
            : this()
        {
            Position = pos;
            Minimum = minimum;
            Maximum = maximum;
        }

    }
}