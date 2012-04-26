// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenVector.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a vector defined in the screen coordinate system.
    /// </summary>
    public struct ScreenVector
    {
        #region Constants and Fields

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
        /// Initializes a new instance of the <see cref="ScreenVector"/> struct. 
        /// </summary>
        /// <param name="x">
        /// The x. 
        /// </param>
        /// <param name="y">
        /// The y. 
        /// </param>
        public ScreenVector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the length.
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt((this.x * this.x) + (this.y * this.y));
            }
        }

        /// <summary>
        ///   Gets the length squared.
        /// </summary>
        public double LengthSquared
        {
            get
            {
                return (this.x * this.x) + (this.y * this.y);
            }
        }

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
        /// Normalizes this vector.
        /// </summary>
        public void Normalize()
        {
            double l = Math.Sqrt((this.x * this.x) + (this.y * this.y));
            if (l > 0)
            {
                this.x /= l;
                this.y /= l;
            }
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
        ///   Implements the operator *.
        /// </summary>
        /// <param name="v"> The v. </param>
        /// <param name="d"> The d. </param>
        /// <returns> The result of the operator. </returns>
        public static ScreenVector operator *(ScreenVector v, double d)
        {
            return new ScreenVector(v.x * d, v.y * d);
        }

        #endregion
    }
}
