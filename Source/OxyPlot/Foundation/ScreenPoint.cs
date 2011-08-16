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

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenPoint"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public ScreenPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets or sets the X.
        /// </summary>
        /// <value>
        /// The X.
        /// </value>
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>
        /// The Y.
        /// </value>
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return x + " " + y;
        }

        /// <summary>
        /// Gets the distances to the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The distance.</returns>
        public double DistanceTo(ScreenPoint point)
        {
            double dx = point.x - x;
            double dy = point.y - y;
            return Math.Sqrt(dx*dx + dy*dy);
        }

        /// <summary>
        /// Gets the squared distance to the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The squared distance.</returns>
        public double DistanceToSquared(ScreenPoint point)
        {
            double dx = point.x - x;
            double dy = point.y - y;
            return dx * dx + dy * dy;
        }
    }
}