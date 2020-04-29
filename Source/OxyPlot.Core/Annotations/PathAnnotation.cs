// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PathAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for all annotations that contain paths (lines, functions or polylines).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    using System;
    using System.Collections.Generic;
    using OxyPlot;

    /// <summary>
    /// Provides an abstract base class for all annotations that contain paths (lines, functions or polylines).
    /// </summary>
    public abstract class PathAnnotation : TextualAnnotation
    {
        /// <summary>
        /// The points of the line, transformed to screen coordinates.
        /// </summary>
        private IList<ScreenPoint> screenPoints;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathAnnotation" /> class.
        /// </summary>
        protected PathAnnotation()
        {
            this.MinimumX = double.MinValue;
            this.MaximumX = double.MaxValue;
            this.MinimumY = double.MinValue;
            this.MaximumY = double.MaxValue;
            this.Color = OxyColors.Blue;
            this.StrokeThickness = 1;
            this.LineStyle = LineStyle.Dash;
            this.LineJoin = LineJoin.Miter;

            this.TextLinePosition = 1;
            this.TextOrientation = AnnotationTextOrientation.AlongLine;
            this.TextMargin = 12;
            this.ClipText = true;
            this.TextHorizontalAlignment = HorizontalAlignment.Right;
            this.TextVerticalAlignment = VerticalAlignment.Top;
        }

        /// <summary>
        /// Gets or sets the color of the line.
        /// </summary>
        public OxyColor Color { get; set; }

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
        /// Gets or sets the maximum X coordinate for the line.
        /// </summary>
        public double MaximumX { get; set; }

        /// <summary>
        /// Gets or sets the maximum Y coordinate for the line.
        /// </summary>
        public double MaximumY { get; set; }

        /// <summary>
        /// Gets or sets the minimum X coordinate for the line.
        /// </summary>
        public double MinimumX { get; set; }

        /// <summary>
        /// Gets or sets the minimum Y coordinate for the line.
        /// </summary>
        public double MinimumY { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the text margin (along the line).
        /// </summary>
        /// <value>The text margin.</value>
        public double TextMargin { get; set; }

        /// <summary>
        /// Gets or sets the text padding (in the direction of the text).
        /// </summary>
        /// <value>The text padding.</value>
        public double TextPadding { get; set; }

        /// <summary>
        /// Gets or sets the text orientation.
        /// </summary>
        /// <value>The text orientation.</value>
        public AnnotationTextOrientation TextOrientation { get; set; }

        /// <summary>
        /// Gets or sets the text position relative to the line.
        /// </summary>
        /// <value>The text position in the interval [0,1].</value>
        /// <remarks>Positions smaller than 0.25 are left aligned at the start of the line
        /// Positions larger than 0.75 are right aligned at the end of the line
        /// Other positions are center aligned at the specified position</remarks>
        public double TextLinePosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to clip the text within the plot area.
        /// </summary>
        /// <value><c>true</c> if text should be clipped within the plot area; otherwise, <c>false</c>.</value>
        public bool ClipText { get; set; }

        /// <summary>
        /// Gets or sets the actual minimum value on the x axis.
        /// </summary>
        /// <value>The actual minimum value on the x axis.</value>
        protected double ActualMinimumX { get; set; }

        /// <summary>
        /// Gets or sets the actual minimum value on the y axis.
        /// </summary>
        /// <value>The actual minimum value on the y axis.</value>
        protected double ActualMinimumY { get; set; }

        /// <summary>
        /// Gets or sets the actual maximum value on the x axis.
        /// </summary>
        /// <value>The actual maximum value on the x axis.</value>
        protected double ActualMaximumX { get; set; }

        /// <summary>
        /// Gets or sets the actual maximum value on the y axis.
        /// </summary>
        /// <value>The actual maximum value on the y axis.</value>
        protected double ActualMaximumY { get; set; }

        /// <summary>
        /// Renders the annotation on the specified context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        public override void Render(IRenderContext rc)
        {
            base.Render(rc);

            this.CalculateActualMinimumsMaximums();

            this.screenPoints = this.GetScreenPoints();

            var clippingRectangle = this.GetClippingRect();

            const double MinimumSegmentLength = 4;

            var clippedPoints = new List<ScreenPoint>();
            var dashArray = this.LineStyle.GetDashArray();

            if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
            {
                rc.DrawClippedLine(
                   clippingRectangle,
                   this.screenPoints,
                   MinimumSegmentLength * MinimumSegmentLength,
                   this.GetSelectableColor(this.Color),
                   this.StrokeThickness,
                   this.EdgeRenderingMode,
                   dashArray,
                   this.LineJoin,
                   null,
                   clippedPoints.AddRange);
            }

            var margin = this.TextMargin;

            this.GetActualTextAlignment(out var ha, out var va);

            if (ha == HorizontalAlignment.Center)
            {
                margin = 0;
            }
            else
            {
                margin *= this.TextLinePosition < 0.5 ? 1 : -1;
            }

            if (GetPointAtRelativeDistance(clippedPoints, this.TextLinePosition, margin, out var position, out var angle))
            {
                if (angle < -90)
                {
                    angle += 180;
                }

                if (angle > 90)
                {
                    angle -= 180;
                }

                switch (this.TextOrientation)
                {
                    case AnnotationTextOrientation.Horizontal:
                        angle = 0;
                        break;
                    case AnnotationTextOrientation.Vertical:
                        angle = -90;
                        break;
                }

                // Apply 'padding' to the position
                var angleInRadians = angle / 180 * Math.PI;
                var f = 1;

                if (ha == HorizontalAlignment.Right)
                {
                    f = -1;
                }

                if (ha == HorizontalAlignment.Center)
                {
                    f = 0;
                }

                position += new ScreenVector(f * this.TextPadding * Math.Cos(angleInRadians), f * this.TextPadding * Math.Sin(angleInRadians));

                if (!string.IsNullOrEmpty(this.Text))
                {
                    var textPosition = this.GetActualTextPosition(() => position);

                    if (this.TextPosition.IsDefined())
                    {
                        angle = this.TextRotation;
                    }

                    if (this.ClipText)
                    {
                        var cs = new CohenSutherlandClipping(clippingRectangle);
                        if (cs.IsInside(position))
                        {
                            rc.DrawClippedText(
                                clippingRectangle,
                                textPosition,
                                this.Text,
                                this.ActualTextColor,
                                this.ActualFont,
                                this.ActualFontSize,
                                this.ActualFontWeight,
                                angle,
                                this.TextHorizontalAlignment,
                                this.TextVerticalAlignment);
                        }
                    }
                    else
                    {
                        rc.DrawText(
                           textPosition,
                           this.Text,
                           this.ActualTextColor,
                           this.ActualFont,
                           this.ActualFontSize,
                           this.ActualFontWeight,
                           angle,
                           ha,
                           va);
                    }
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// The result of the hit test.
        /// </returns>
        protected override HitTestResult HitTestOverride(HitTestArguments args)
        {
            if (this.screenPoints == null)
            {
                return null;
            }

            var nearestPoint = ScreenPointHelper.FindNearestPointOnPolyline(args.Point, this.screenPoints);
            double dist = (args.Point - nearestPoint).Length;
            if (dist < args.Tolerance)
            {
                return new HitTestResult(this, nearestPoint);
            }

            return null;
        }

        /// <summary>
        /// Gets the screen points.
        /// </summary>
        /// <returns>The list of points to display on screen for this path.</returns>
        protected abstract IList<ScreenPoint> GetScreenPoints();

        /// <summary>
        /// Calculates the actual minimums and maximums.
        /// </summary>
        protected virtual void CalculateActualMinimumsMaximums()
        {
            this.ActualMinimumX = Math.Max(this.MinimumX, this.XAxis.ActualMinimum);
            this.ActualMaximumX = Math.Min(this.MaximumX, this.XAxis.ActualMaximum);
            this.ActualMinimumY = Math.Max(this.MinimumY, this.YAxis.ActualMinimum);
            this.ActualMaximumY = Math.Min(this.MaximumY, this.YAxis.ActualMaximum);

            var topLeft = this.InverseTransform(this.PlotModel.PlotArea.TopLeft);
            var bottomRight = this.InverseTransform(this.PlotModel.PlotArea.BottomRight);

            if (!this.ClipByXAxis)
            {
                this.ActualMaximumX = Math.Max(topLeft.X, bottomRight.X);
                this.ActualMinimumX = Math.Min(topLeft.X, bottomRight.X);
            }

            if (!this.ClipByYAxis)
            {
                this.ActualMaximumY = Math.Max(topLeft.Y, bottomRight.Y);
                this.ActualMinimumY = Math.Min(topLeft.Y, bottomRight.Y);
            }
        }

        /// <summary>
        /// Gets the point on a curve at the specified relative distance along the curve.
        /// </summary>
        /// <param name="pts">The curve points.</param>
        /// <param name="p">The relative distance along the curve.</param>
        /// <param name="margin">The margins.</param>
        /// <param name="position">The position.</param>
        /// <param name="angle">The angle.</param>
        /// <returns>True if a position was found.</returns>
        private static bool GetPointAtRelativeDistance(
            IList<ScreenPoint> pts, double p, double margin, out ScreenPoint position, out double angle)
        {
            if (pts == null || pts.Count == 0)
            {
                position = new ScreenPoint();
                angle = 0;
                return false;
            }

            double length = 0;
            for (int i = 1; i < pts.Count; i++)
            {
                length += (pts[i] - pts[i - 1]).Length;
            }

            double l = (length * p) + margin;
            double eps = 1e-8;
            length = 0;
            for (int i = 1; i < pts.Count; i++)
            {
                double dl = (pts[i] - pts[i - 1]).Length;
                if (l >= length - eps && l <= length + dl + eps)
                {
                    double f = (l - length) / dl;
                    double x = (pts[i].X * f) + (pts[i - 1].X * (1 - f));
                    double y = (pts[i].Y * f) + (pts[i - 1].Y * (1 - f));
                    position = new ScreenPoint(x, y);
                    double dx = pts[i].X - pts[i - 1].X;
                    double dy = pts[i].Y - pts[i - 1].Y;
                    angle = Math.Atan2(dy, dx) / Math.PI * 180;
                    return true;
                }

                length += dl;
            }

            position = pts[0];
            angle = 0;
            return false;
        }
    }
}
