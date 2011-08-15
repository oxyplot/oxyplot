using System;
using System.Diagnostics;

namespace OxyPlot
{
    using System.Globalization;

    /// <summary>
    /// Rectangle class.
    /// </summary>
    public struct OxyRect
    {
        private double height;
        private double left;
        private double top;
        private double width;

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyRect"/> struct.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public OxyRect(double left, double top, double width, double height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            Debug.Assert(width >= 0 && height >= 0);
        }

        /// <summary>
        /// Creates a rectangle from the specified corner coordinates.
        /// </summary>
        /// <param name="x0">The x0.</param>
        /// <param name="y0">The y0.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <returns></returns>
        public static OxyRect Create(double x0, double y0, double x1, double y1)
        {
            return new OxyRect(Math.Min(x0, x1), Math.Min(y0, y1), Math.Abs(x1 - x0), Math.Abs(y1 - y0));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3})", left, top, width, height);
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public double Top
        {
            get { return top; }
            set { top = value; }
        }

        /// <summary>
        /// Gets or sets the bottom.
        /// </summary>
        /// <value>
        /// The bottom.
        /// </value>
        public double Bottom
        {
            get { return top + height; }
            set { height = value - top; }
        }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public double Left
        {
            get { return left; }
            set { left = value; }
        }

        /// <summary>
        /// Gets or sets the right.
        /// </summary>
        /// <value>
        /// The right.
        /// </value>
        public double Right
        {
            get { return left + width; }
            set { width = value - left; }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public double Height
        {
            get { return height; }
            set { height = value; }
        }
    }
}