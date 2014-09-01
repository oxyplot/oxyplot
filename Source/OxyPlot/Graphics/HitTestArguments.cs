// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HitTestArguments.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents arguments for the hit test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents arguments for the hit test.
    /// </summary>
    public class HitTestArguments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HitTestArguments"/> class.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="tolerance">The tolerance.</param>
        public HitTestArguments(ScreenPoint point, double tolerance)
        {
            this.Point = point;
            this.Tolerance = tolerance;
        }

        /// <summary>
        /// Gets the point to hit test.
        /// </summary>
        public ScreenPoint Point { get; private set; }

        /// <summary>
        /// Gets the hit test tolerance.
        /// </summary>
        public double Tolerance { get; private set; }
    }
}