// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyRect.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Describes the width, height, and point origin of a rectangle.
    /// </summary>
    public struct OxyRect
    {
        #region Constants and Fields

        /// <summary>
        ///   The height.
        /// </summary>
        private double height;

        /// <summary>
        ///   The left.
        /// </summary>
        private double left;

        /// <summary>
        ///   The top.
        /// </summary>
        private double top;

        /// <summary>
        ///   The width.
        /// </summary>
        private double width;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyRect"/> struct.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="top">
        /// The top.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public OxyRect(double left, double top, double width, double height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            Debug.Assert(width >= 0 && height >= 0, "Width and height should be larger than 0.");
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the bottom.
        /// </summary>
        /// <value>
        ///   The bottom.
        /// </value>
        public double Bottom
        {
            get
            {
                return this.top + this.height;
            }

            set
            {
                this.height = value - this.top;
            }
        }

        /// <summary>
        ///   Gets or sets the height.
        /// </summary>
        /// <value>
        ///   The height.
        /// </value>
        public double Height
        {
            get
            {
                return this.height;
            }

            set
            {
                this.height = value;
            }
        }

        /// <summary>
        ///   Gets or sets the left.
        /// </summary>
        /// <value>
        ///   The left.
        /// </value>
        public double Left
        {
            get
            {
                return this.left;
            }

            set
            {
                this.left = value;
            }
        }

        /// <summary>
        ///   Gets or sets the right.
        /// </summary>
        /// <value>
        ///   The right.
        /// </value>
        public double Right
        {
            get
            {
                return this.left + this.width;
            }

            set
            {
                this.width = value - this.left;
            }
        }

        /// <summary>
        ///   Gets or sets the top.
        /// </summary>
        /// <value>
        ///   The top.
        /// </value>
        public double Top
        {
            get
            {
                return this.top;
            }

            set
            {
                this.top = value;
            }
        }

        /// <summary>
        ///   Gets or sets the width.
        /// </summary>
        /// <value>
        ///   The width.
        /// </value>
        public double Width
        {
            get
            {
                return this.width;
            }

            set
            {
                this.width = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a rectangle from the specified corner coordinates.
        /// </summary>
        /// <param name="x0">
        /// The x0.
        /// </param>
        /// <param name="y0">
        /// The y0.
        /// </param>
        /// <param name="x1">
        /// The x1.
        /// </param>
        /// <param name="y1">
        /// The y1.
        /// </param>
        /// <returns>
        /// A rectangle.
        /// </returns>
        public static OxyRect Create(double x0, double y0, double x1, double y1)
        {
            return new OxyRect(Math.Min(x0, x1), Math.Min(y0, y1), Math.Abs(x1 - x0), Math.Abs(y1 - y0));
        }

        /// <summary>
        /// Determines whether the specified point is inside the rectangle.
        /// </summary>
        /// <param name="x">
        /// The x coordinate.
        /// </param>
        /// <param name="y">
        /// The y coordinate.
        /// </param>
        /// <returns>
        /// <c>true</c> if the rectangle contains the specified point; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(double x, double y)
        {
            return x >= this.Left && x <= this.Right && y >= this.Top && y <= this.Bottom;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3})", this.left, this.top, this.width, this.height);
        }

        #endregion
    }
}