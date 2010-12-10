namespace OxyPlot
{
    /// <summary>
    /// DataPoint interface.
    /// </summary>
    public interface IDataPoint
    {
        /// <summary>
        /// Gets the X.
        /// </summary>
        /// <value>The X.</value>
        double X { get; }

        /// <summary>
        /// Gets the Y.
        /// </summary>
        /// <value>The Y.</value>
        double Y { get; }
    }

    /// <summary>
    /// DataPoint value type.
    /// </summary>
    public struct DataPoint : IDataPoint
    {
        public static readonly DataPoint Undefined = new DataPoint(double.NaN, double.NaN);
        internal double x;
        internal double y;

        public DataPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public override string ToString()
        {
            return x + " " + y;
        }
    }
}