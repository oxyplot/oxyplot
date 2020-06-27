// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenPoint.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a point defined in screen space.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a point defined in screen space.
    /// </summary>
    /// <remarks>The rendering methods transforms <see cref="DataPoint" />s to <see cref="ScreenPoint" />s.</remarks>
    public struct ScreenPoint : IEquatable<ScreenPoint>
    {
        /// <summary>
        /// The undefined point.
        /// </summary>
        public static readonly ScreenPoint Undefined = new ScreenPoint(double.NaN, double.NaN);

        /// <summary>
        /// The x-coordinate.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        // ReSharper disable once InconsistentNaming
        internal double x;

        /// <summary>
        /// The y-coordinate.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        // ReSharper disable once InconsistentNaming
        internal double y;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenPoint" /> struct.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        public ScreenPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets the x-coordinate.
        /// </summary>
        /// <value>The x-coordinate.</value>
        public double X
        {
            get
            {
                return this.x;
            }
        }

        /// <summary>
        /// Gets the y-coordinate.
        /// </summary>
        /// <value>The y-coordinate.</value>
        public double Y
        {
            get
            {
                return this.y;
            }
        }

        /// <summary>
        /// Determines whether the specified point is undefined.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns><c>true</c> if the specified point is undefined; otherwise, <c>false</c> .</returns>
        public static bool IsUndefined(ScreenPoint point)
        {
            return double.IsNaN(point.x) && double.IsNaN(point.y);
        }

        /// <summary>
        /// Translates a <see cref="ScreenPoint" /> by a <see cref="ScreenVector" />.
        /// </summary>
        /// <param name="p1">The point.</param>
        /// <param name="p2">The vector.</param>
        /// <returns>The translated point.</returns>
        public static ScreenPoint operator +(ScreenPoint p1, ScreenVector p2)
        {
            return new ScreenPoint(p1.x + p2.x, p1.y + p2.y);
        }

        /// <summary>
        /// Subtracts a <see cref="ScreenPoint" /> from a <see cref="ScreenPoint" />
        /// and returns the result as a <see cref="ScreenVector" />.
        /// </summary>
        /// <param name="p1">The point on which to perform the subtraction.</param>
        /// <param name="p2">The point to subtract from p1.</param>
        /// <returns>A <see cref="ScreenVector" /> structure that represents the difference between p1 and p2.</returns>
        public static ScreenVector operator -(ScreenPoint p1, ScreenPoint p2)
        {
            return new ScreenVector(p1.x - p2.x, p1.y - p2.y);
        }

        /// <summary>
        /// Subtracts a <see cref="ScreenVector" /> from a <see cref="ScreenPoint" />
        /// and returns the result as a <see cref="ScreenPoint" />.
        /// </summary>
        /// <param name="point">The point on which to perform the subtraction.</param>
        /// <param name="vector">The vector to subtract from p1.</param>
        /// <returns>A <see cref="ScreenPoint" /> that represents point translated by the negative vector.</returns>
        public static ScreenPoint operator -(ScreenPoint point, ScreenVector vector)
        {
            return new ScreenPoint(point.x - vector.x, point.y - vector.y);
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
        /// Intersects two rays in screen space, determining distance along them where they meet.
        /// </summary>
        /// <param name="ray0start">The origin of the first ray.</param>
        /// <param name="ray0direction">The direction of the first ray.</param>
        /// <param name="ray1start">The origin of the second ray.</param>
        /// <param name="ray1direction">The direction of the second ray.</param>
        /// <param name="lambda0">The distance along the first ray in multiples of its direction.</param>
        /// <param name="lambda1">The distance along the second ray in multiples of its direction.</param>
        /// <returns><c>True</c> if the rays intersect, otherwise <c>false</c></returns>
        public static bool IntersectRayWithRay(ScreenPoint ray0start, ScreenVector ray0direction, ScreenPoint ray1start, ScreenVector ray1direction, out double lambda0, out double lambda1)
        {
            var numerator0 = ray1direction.X * (ray0start.Y - ray1start.Y) - ray1direction.Y * (ray0start.X - ray1start.X);
            var numerator1 = ray0direction.X * (ray1start.Y - ray0start.Y) - ray0direction.Y * (ray1start.X - ray0start.X);
            var denomonator = ray1direction.X * ray0direction.Y - ray0direction.X * ray1direction.Y;

            lambda0 = numerator0 / denomonator;
            lambda1 = numerator1 / denomonator;

            return IsFinite(lambda0) && IsFinite(lambda1);
        }

        /// <summary>
        /// Intersects two lines in screen space.
        /// </summary>
        /// <param name="line0Start">The start point of the first line.</param>
        /// <param name="line0End">The end point of the first line.</param>
        /// <param name="line1Start">The start point of the second line.</param>
        /// <param name="line1End">The end point of the second line.</param>
        /// <param name="intersection">The point where the two lines intersect.</param>
        /// <returns><c>True</c> if the lines intersect, otherwise false.</returns>
        public static bool IntersectLineWithLine(ScreenPoint line0Start, ScreenPoint line0End, ScreenPoint line1Start, ScreenPoint line1End, out ScreenPoint intersection)
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
        /// Gets the distance to the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The distance.</returns>
        public double DistanceTo(ScreenPoint point)
        {
            double dx = point.x - this.x;
            double dy = point.y - this.y;
            return Math.Sqrt((dx * dx) + (dy * dy));
        }

        /// <summary>
        /// Gets the squared distance to the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The squared distance.</returns>
        public double DistanceToSquared(ScreenPoint point)
        {
            double dx = point.x - this.x;
            double dy = point.y - this.y;
            return (dx * dx) + (dy * dy);
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
        /// Determines whether this instance and another specified <see cref="T:ScreenPoint" /> object have the same value.
        /// </summary>
        /// <param name="other">The point to compare to this instance.</param>
        /// <returns><c>true</c> if the value of the <paramref name="other" /> parameter is the same as the value of this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(ScreenPoint other)
        {
            return this.x.Equals(other.x) && this.y.Equals(other.y);
        }
    }
}
