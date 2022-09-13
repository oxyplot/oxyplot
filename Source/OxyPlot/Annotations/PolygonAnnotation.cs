﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolygonAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an annotation that shows a polygon.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#nullable enable

namespace OxyPlot.Annotations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an annotation that shows a polygon.
    /// </summary>
    public class PolygonAnnotation : ShapeAnnotation
    {
        /// <summary>
        /// The polygon points transformed to screen coordinates.
        /// </summary>
        private IList<ScreenPoint>? screenPoints;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonAnnotation" /> class.
        /// </summary>
        public PolygonAnnotation()
        {
            this.LineStyle = LineStyle.Solid;
            this.LineJoin = LineJoin.Miter;
            this.MinimumSegmentLength = 2;

            this.Points = new List<DataPoint>();
        }

        /// <summary>
        /// Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public LineJoin LineJoin { get; set; }

        /// <summary>
        /// Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of the segment.
        /// Increasing this number will increase performance,
        /// but make the polygon less accurate. The default is <c>2</c>.
        /// </summary>
        /// <value>The minimum length of the segment.</value>
        public double MinimumSegmentLength { get; set; }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>The points.</value>
        public List<DataPoint> Points { get; private set; }

        /// <inheritdoc/>
        public override void Render(IRenderContext rc)
        {
            base.Render(rc);
            if (this.Points == null)
            {
                return;
            }

            // transform to screen coordinates
            this.screenPoints = this.Points.Select(this.Transform).ToList();
            if (this.screenPoints.Count == 0)
            {
                return;
            }

            rc.DrawReducedPolygon(
                this.screenPoints,
                this.MinimumSegmentLength * this.MinimumSegmentLength,
                this.GetSelectableFillColor(this.Fill),
                this.GetSelectableColor(this.Stroke),
                this.StrokeThickness,
                this.EdgeRenderingMode,
                this.LineStyle,
                this.LineJoin);

            if (!string.IsNullOrEmpty(this.Text))
            {
                this.GetActualTextAlignment(out var ha, out var va);
                var textPosition = this.GetActualTextPosition(() => ScreenPointHelper.GetCentroid(this.screenPoints));

                rc.DrawText(
                    textPosition,
                    this.Text,
                    this.ActualTextColor,
                    this.ActualFont,
                    this.ActualFontSize,
                    this.ActualFontWeight,
                    this.TextRotation,
                    ha,
                    va);
            }
        }

        /// <summary>
        /// When overridden in a derived class, tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// The result of the hit test.
        /// </returns>
        protected override HitTestResult? HitTestOverride(HitTestArguments args)
        {
            if (this.screenPoints == null)
            {
                // Points not specified.
                return null;
            }

            return ScreenPointHelper.IsPointInPolygon(args.Point, this.screenPoints) ? new HitTestResult(this, args.Point) : null;
        }
    }
}
