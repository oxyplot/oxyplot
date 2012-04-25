// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenPoint.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Describes a point defined in the screen coordinate system.
    /// </summary>
    /// <remarks>
    /// The rendering methods transforms DataPoints to ScreenPoints.
    /// </remarks>
    public struct ScreenPoint
    {
        #region Constants and Fields

        /// <summary>
        ///   The undefined.
        /// </summary>
        public static readonly ScreenPoint Undefined = new ScreenPoint(double.NaN, double.NaN);

        /// <summary>
        ///   The x.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        internal double x;

        /// <summary>
        ///   The y.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        internal double y;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenPoint"/> struct. 
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <param name="y">
        /// The y. 
        /// </param>
        public ScreenPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the X.
        /// </summary>
        /// <value> The X. </value>
        public double X
        {
            get
            {
                return this.x;
            }

            set
            {
                this.x = value;
            }
        }

        /// <summary>
        ///   Gets or sets the Y.
        /// </summary>
        /// <value> The Y. </value>
        public double Y
        {
            get
            {
                return this.y;
            }

            set
            {
                this.y = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether the specified point is undefined.
        /// </summary>
        /// <param name="point">
        /// The point. 
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified point is undefined; otherwise, <c>false</c> . 
        /// </returns>
        public static bool IsUndefined(ScreenPoint point)
        {
            return point.X == Undefined.X && point.Y == Undefined.Y;
        }

        /// <summary>
        /// Gets the distances to the specified point.
        /// </summary>
        /// <param name="point">
        /// The point. 
        /// </param>
        /// <returns>
        /// The distance. 
        /// </returns>
        public double DistanceTo(ScreenPoint point)
        {
            double dx = point.x - this.x;
            double dy = point.y - this.y;
            return Math.Sqrt((dx * dx) + (dy * dy));
        }

        /// <summary>
        /// Gets the squared distance to the specified point.
        /// </summary>
        /// <param name="point">
        /// The point. 
        /// </param>
        /// <returns>
        /// The squared distance. 
        /// </returns>
        public double DistanceToSquared(ScreenPoint point)
        {
            double dx = point.x - this.x;
            double dy = point.y - this.y;
            return (dx * dx) + (dy * dy);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance. 
        /// </returns>
        public override string ToString()
        {
            return this.x + " " + this.y;
        }

        #endregion

        #region Operators

        /// <summary>
        ///   Implements the operator +.
        /// </summary>
        /// <param name="p1"> The p1. </param>
        /// <param name="p2"> The p2. </param>
        /// <returns> The result of the operator. </returns>
        public static ScreenVector operator +(ScreenPoint p1, ScreenPoint p2)
        {
            return new ScreenVector(p1.x + p2.x, p1.y + p2.y);
        }

        /// <summary>
        ///   Implements the operator +.
        /// </summary>
        /// <param name="p1"> The p1. </param>
        /// <param name="p2"> The p2. </param>
        /// <returns> The result of the operator. </returns>
        public static ScreenPoint operator +(ScreenPoint p1, ScreenVector p2)
        {
            return new ScreenPoint(p1.x + p2.x, p1.y + p2.y);
        }

        /// <summary>
        ///   Implements the operator -.
        /// </summary>
        /// <param name="p1"> The p1. </param>
        /// <param name="p2"> The p2. </param>
        /// <returns> The result of the operator. </returns>
        public static ScreenVector operator -(ScreenPoint p1, ScreenPoint p2)
        {
            return new ScreenVector(p1.x - p2.x, p1.y - p2.y);
        }

        /// <summary>
        ///   Implements the operator -.
        /// </summary>
        /// <param name="p1"> The p1. </param>
        /// <param name="p2"> The p2. </param>
        /// <returns> The result of the operator. </returns>
        public static ScreenPoint operator -(ScreenPoint p1, ScreenVector p2)
        {
            return new ScreenPoint(p1.x - p2.x, p1.y - p2.y);
        }

        #endregion
    }
}