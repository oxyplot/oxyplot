// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRect.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a rectangle in the data space.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a point in the data space.
    /// </summary>
    /// <remarks><see cref="DataRect" />s are transformed to <see cref="ScreenRect" />s.</remarks>
    public struct DataRect : ICodeGenerating, IEquatable<DataRect>
    {
        /// <summary>
        /// The undefined.
        /// </summary>
        public static readonly DataRect Undefined = new DataRect(DataPoint.Undefined, DataPoint.Undefined, 0);

        /// <summary>
        /// The x-coordinate.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        internal readonly DataPoint a;

        /// <summary>
        /// The y-coordinate.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        internal readonly DataPoint b;

        /// <summary>
        /// The value.
        /// </summary>
        internal readonly double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRect" /> struct.
        /// </summary>
        /// <param name="x1">The x coordinate of the first corner.</param>
        /// <param name="x2">The x coordinate of the diagonally-opposite corner.</param>
        /// <param name="y1">The y coordinate of the first corner.</param>
        /// <param name="y2">The y coordinate of the diagonally-opposite corner.</param>
        public DataRect(double x1, double x2, double y1, double y2, double value)
        {
            this.a = new DataPoint(x1, y1);
            this.b = new DataPoint(x2, y2);
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRect" /> struct.
        /// </summary>
        /// <param name="a">The first corner.</param>
        /// <param name="b">The diagonally-opposite corner.</param>
        public DataRect(DataPoint a, DataPoint b, double value)
        {
            this.a = a;
            this.b = b;
            this.value = value;
        }

        /// <summary>
        /// Gets the first data point.
        /// </summary>
        /// <value>The first data point.</value>
        public DataPoint A
        {
            get
            {
                return this.a;
            }
        }

        /// <summary>
        /// Gets the diagonally-opposite data point.
        /// </summary>
        /// <value>The diagonally-opposite data point.</value>
        public DataPoint B
        {
            get
            {
                return this.b;
            }
        }

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>The to code.</returns>
        public string ToCode()
        {
            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.a, this.b);
        }

        /// <summary>
        /// Determines whether this instance and another specified <see cref="T:DataRect" /> object have the same value.
        /// </summary>
        /// <param name="other">The point to compare to this instance.</param>
        /// <returns><c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(DataRect other)
        {
            return this.a.Equals(other.a) && this.b.Equals(other.b);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.a + " " + this.b;
        }

        /// <summary>
        /// Determines whether this point is defined.
        /// </summary>
        /// <returns><c>true</c> if this point is defined; otherwise, <c>false</c>.</returns>
        public bool IsDefined()
        {
            // check that x and y is not NaN (the code below is faster than double.IsNaN)
#pragma warning disable 1718
            // ReSharper disable EqualExpressionComparison
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return this.a.IsDefined() && this.b.IsDefined();
            // ReSharper restore CompareOfFloatsByEqualityOperator
            // ReSharper restore EqualExpressionComparison
#pragma warning restore 1718
        }
    }
}