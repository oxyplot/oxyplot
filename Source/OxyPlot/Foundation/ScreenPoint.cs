using System;

namespace OxyPlot
{
    /// <summary>
    /// A point defined in the screen coordinate system.
    /// The rendering methods transforms DataPoints to ScreenPoints.
    /// </summary>
    public struct ScreenPoint
    {
        public static readonly ScreenPoint Undefined = new ScreenPoint(double.NaN, double.NaN);

        internal double x;
        internal double y;

        public ScreenPoint(double x, double y)
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

        /// <summary>
        /// Gets the distances to the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public double DistanceTo(ScreenPoint point)
        {
            double dx = point.x - x;
            double dy = point.y - y;
            return Math.Sqrt(dx*dx + dy*dy);
        }
    }
}