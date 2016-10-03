// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolyLineAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an annotation that shows a polyline.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an annotation that shows a polyline.
    /// </summary>
    public class PolylineAnnotation : PathAnnotation
    {
        /// <summary>
        /// The points.
        /// </summary>
        private readonly List<DataPoint> points = new List<DataPoint>();

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>The points.</value>
        public List<DataPoint> Points
        {
            get
            {
                return this.points;
            }
        }

        /// <summary>
        /// Gets or sets the minimum length of the segment.
        /// Increasing this number will increase performance,
        /// but make the curve less accurate.
        /// </summary>
        /// <value>The minimum length of the segment.</value>
        public double MinimumSegmentLength { get; set; }

        /// <summary>
        /// Gets or sets the interpolation algorithm.
        /// </summary>
        /// <value>An interpolation algorithm.</value>
        public IInterpolationAlgorithm InterpolationAlgorithm { get; set; }

        /// <summary>
        /// Gets the screen points.
        /// </summary>
        /// <returns>The list of points to display on screen for this path.</returns>
        protected override IList<ScreenPoint> GetScreenPoints()
        {
            var screenPoints = this.Points.Select(this.Transform).ToList();

            if (this.InterpolationAlgorithm != null)
            {
                var resampledPoints = ScreenPointHelper.ResamplePoints(screenPoints, this.MinimumSegmentLength);
                return this.InterpolationAlgorithm.CreateSpline(resampledPoints, false, 0.25);
            }

            return this.Points.Select(this.Transform).ToList();
        }
    }
}