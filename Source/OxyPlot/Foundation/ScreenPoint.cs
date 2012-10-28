// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenPoint.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Describes a point defined in the screen coordinate system.
// </summary>
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
        /// <summary>
        /// The undefined.
        /// </summary>
        public static readonly ScreenPoint Undefined = new ScreenPoint(double.NaN, double.NaN);

        /// <summary>
        /// The x.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        internal double x;

        /// <summary>
        /// The y.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter",
            Justification = "Reviewed. Suppression is OK here.")]
        internal double y;

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

        /// <summary>
        /// Gets or sets the X.
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
        /// Gets or sets the Y.
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

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="p1"> The p1. </param>
        /// <param name="p2"> The p2. </param>
        /// <returns> The result of the operator. </returns>
        public static ScreenVector operator +(ScreenPoint p1, ScreenPoint p2)
        {
            return new ScreenVector(p1.x + p2.x, p1.y + p2.y);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="p1"> The p1. </param>
        /// <param name="p2"> The p2. </param>
        /// <returns> The result of the operator. </returns>
        public static ScreenPoint operator +(ScreenPoint p1, ScreenVector p2)
        {
            return new ScreenPoint(p1.x + p2.x, p1.y + p2.y);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="p1"> The p1. </param>
        /// <param name="p2"> The p2. </param>
        /// <returns> The result of the operator. </returns>
        public static ScreenVector operator -(ScreenPoint p1, ScreenPoint p2)
        {
            return new ScreenVector(p1.x - p2.x, p1.y - p2.y);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="p1"> The p1. </param>
        /// <param name="p2"> The p2. </param>
        /// <returns> The result of the operator. </returns>
        public static ScreenPoint operator -(ScreenPoint p1, ScreenVector p2)
        {
            return new ScreenPoint(p1.x - p2.x, p1.y - p2.y);
        }

    }
}