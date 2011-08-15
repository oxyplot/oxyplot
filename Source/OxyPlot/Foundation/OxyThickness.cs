namespace OxyPlot
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Thickness class.
    /// </summary>
    public struct OxyThickness
    {
        private double bottom;
        private double left;
        private double right;
        private double top;

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyThickness"/> struct.
        /// </summary>
        /// <param name="thickness">The thickness.</param>
        public OxyThickness(double thickness)
            : this(thickness, thickness, thickness, thickness)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyThickness"/> struct.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        public OxyThickness(double left, double top, double right, double bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3})", left, top, right, bottom);
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
            get { return bottom; }
            set { bottom = value; }
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
            get { return right; }
            set { right = value; }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public double Width
        {
            get { return Right - Left; }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public double Height
        {
            get { return Bottom - Top; }
        }
    }
}