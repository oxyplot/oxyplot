// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPoint.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a point in the data space.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a point in the data space.
    /// </summary>
    /// <remarks><see cref="DataPoint" />s are transformed to <see cref="ScreenPoint" />s.</remarks>
    public struct DataPoint : ICodeGenerating, IEquatable<DataPoint>
    {
        /// <summary>
        /// The undefined.
        /// </summary>
        public static readonly DataPoint Undefined = new DataPoint(double.NaN, double.NaN);

        /// <summary>
        /// The x-coordinate.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        internal readonly double x;

        /// <summary>
        /// The y-coordinate.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        internal readonly double y;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPoint" /> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public DataPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets the X-coordinate of the point.
        /// </summary>
        /// <value>The X-coordinate.</value>
        public double X
        {
            get
            {
                return this.x;
            }
        }

        /// <summary>
        /// Gets the Y-coordinate of the point.
        /// </summary>
        /// <value>The Y-coordinate.</value>
        public double Y
        {
            get
            {
                return this.y;
            }
        }

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>The to code.</returns>
        public string ToCode()
        {
            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.x, this.y);
        }

        /// <summary>
        /// Determines whether this and another specified <see cref="T:DataPoint" /> have the same value.
        /// </summary>
        /// <param name="other">The point to compare to this instance.</param>
        /// <returns><c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(DataPoint other)
        {
            return this.x.Equals(other.x) && this.y.Equals(other.y);
        }

        /// <summary>
        /// Translates a <see cref="DataPoint" /> by a <see cref="DataVector" />.
        /// </summary>
        /// <param name="p1">The point.</param>
        /// <param name="p2">The vector.</param>
        /// <returns>The translated point.</returns>
        public static DataPoint operator +(DataPoint p1, DataVector p2)
        {
            return new DataPoint(p1.x + p2.x, p1.y + p2.y);
        }

        /// <summary>
        /// Subtracts a <see cref="DataPoint" /> from a <see cref="DataPoint" />
        /// and returns the result as a <see cref="DataVector" />.
        /// </summary>
        /// <param name="p1">The point on which to perform the subtraction.</param>
        /// <param name="p2">The point to subtract from p1.</param>
        /// <returns>A <see cref="DataVector" /> structure that represents the difference between p1 and p2.</returns>
        public static DataVector operator -(DataPoint p1, DataPoint p2)
        {
            return new DataVector(p1.x - p2.x, p1.y - p2.y);
        }

        /// <summary>
        /// Subtracts a <see cref="DataVector" /> from a <see cref="DataPoint" />
        /// and returns the result as a <see cref="DataPoint" />.
        /// </summary>
        /// <param name="point">The point on which to perform the subtraction.</param>
        /// <param name="vector">The vector to subtract from p1.</param>
        /// <returns>A <see cref="DataPoint" /> that represents point translated by the negative vector.</returns>
        public static DataPoint operator -(DataPoint point, DataVector vector)
        {
            return new DataPoint(point.x - vector.x, point.y - vector.y);
        }

        /// <summary>
        /// Determines whether the given values is a real fine value.
        /// </summary>
        /// <param name="f">The number to evaluate.</param>
        /// <returns><c>true</c> if <paramref name="f"/> is non-NaN and non-Infinite, otherwise <c>false</c>.</returns>
        private static bool IsFinite(double f)
        {
            return !(double.IsNaN(f) || double.IsInfinity(f));
        }

        /// <summary>
        /// Intersects two rays in data space, determining distance along them where they meet.
        /// </summary>
        /// <param name="ray0start">The origin of the first ray.</param>
        /// <param name="ray0direction">The direction of the first ray.</param>
        /// <param name="ray1start">The origin of the second ray.</param>
        /// <param name="ray1direction">The direction of the second ray.</param>
        /// <param name="lambda0">The distance along the first ray in multiples of its direction.</param>
        /// <param name="lambda1">The distance along the second ray in multiples of its direction.</param>
        /// <returns><c>True</c> if the rays intersect, otherwise <c>false</c></returns>
        public static bool IntersectRayWithRay(DataPoint ray0start, DataVector ray0direction, DataPoint ray1start, DataVector ray1direction, out double lambda0, out double lambda1)
        {
            var numerator0 = ray1direction.X * (ray0start.Y - ray1start.Y) - ray1direction.Y * (ray0start.X - ray1start.X);
            var numerator1 = ray0direction.X * (ray1start.Y - ray0start.Y) - ray0direction.Y * (ray1start.X - ray0start.X);
            var denomonator = ray1direction.X * ray0direction.Y - ray0direction.X * ray1direction.Y;

            lambda0 = numerator0 / denomonator;
            lambda1 = numerator1 / denomonator;

            return IsFinite(lambda0) && IsFinite(lambda1);
        }

        /// <summary>
        /// Intersects two lines in data space.
        /// </summary>
        /// <param name="line0Start">The start point of the first line.</param>
        /// <param name="line0End">The end point of the first line.</param>
        /// <param name="line1Start">The start point of the second line.</param>
        /// <param name="line1End">The end point of the second line.</param>
        /// <param name="intersection">The point where the two lines intersect.</param>
        /// <returns><c>True</c> if the lines intersect, otherwise false.</returns>
        public static bool IntersectLineWithLine(DataPoint line0Start, DataPoint line0End, DataPoint line1Start, DataPoint line1End, out DataPoint intersection)
        {
            var line0Dir = line0End - line0Start;
            var line1Dir = line1End - line1Start;
            bool notParallel = IntersectRayWithRay(line0Start, line0Dir, line0Start, line0Dir, out double lambda0, out double lambda1);

            if (!notParallel)
            {
                intersection = default;
                return false;
            }

            intersection = line1Start + line1Dir * lambda1;
            return lambda0 >= 0 && lambda0 <= 1 && lambda1 >= 0 && lambda1 <= 1;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.x + " " + this.y;
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
            return this.x == this.x && this.y == this.y;
            // ReSharper restore CompareOfFloatsByEqualityOperator
            // ReSharper restore EqualExpressionComparison
#pragma warning restore 1718
        }
    }
}
