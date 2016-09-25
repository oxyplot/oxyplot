﻿// --------------------------------------------------------------------------------------------------------------------
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
        /// Initializes a new instance of the <see cref = "PolylineAnnotation" /> class.
        /// </summary>
        public PolylineAnnotation()
        {
            this.InterpolationAlgorithm = InterpolationAlgorithm.Canonical;
        }

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
        /// Gets or sets a value indicating whether this <see cref = "PolylineAnnotation" /> is smooth.
        /// </summary>
        /// <value><c>true</c> if smooth; otherwise, <c>false</c>.</value>
        public bool Smooth { get; set; }

        /// <summary>
        /// Gets or sets a type of interpolation algorithm used for smoothing this <see cref = "PolylineAnnotation" />.
        /// </summary>
        /// <value>Type of interpolation algorithm.</value>
        public InterpolationAlgorithm InterpolationAlgorithm { get; set; }

        /// <summary>
        /// Gets the screen points.
        /// </summary>
        /// <returns>The list of points to display on screen for this path.</returns>
        protected override IList<ScreenPoint> GetScreenPoints()
        {
            var screenPoints = this.Points.Select(this.Transform).ToList();

            if (this.Smooth)
            {
                var resampledPoints = ScreenPointHelper.ResamplePoints(screenPoints, this.MinimumSegmentLength);
                return InterpolationAlgorithm.CreateSpline(resampledPoints, false, 0.25);
            }

            return this.Points.Select(this.Transform).ToList();
        }
    }
}