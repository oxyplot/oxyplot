// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtrapolationLineSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a line series with extrapolated or interpolated intervals.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// Represents a series where the line can be rendered using a different style
    /// or color in defined intervals of X. The style specified in the LineStyle
    /// property determines how the line is rendered in these intervals. Outside
    /// the intervals the style is always solid.
    /// </summary>
    public class ExtrapolationLineSeries : LineSeries
    {
        /// <summary>
        /// Default color for the extrapolated parts of the curve. Currently hard-coded.
        /// </summary>
        private readonly OxyColor defaultExtrapolationColor = OxyColors.Black;

        /// <summary>
        /// Default line style for the extrapolated parts of the curve. Currently hard-coded.
        /// </summary>
        private readonly LineStyle defaultExtrapolationLineStyle = LineStyle.Dash;

        private List<DataRange> orderedIntervals;

        /// <summary>
        /// Initializes a new instance of the <see cref = "ExtrapolationLineSeries" /> class.
        /// </summary>
        public ExtrapolationLineSeries()
        {
            this.ExtrapolationColor = OxyColors.Black;
            this.LineStyle = LineStyle.Dot;
        }

        /// <summary>
        /// Gets or sets the color for the part of the line that is inside an interval.
        /// </summary>
        public OxyColor ExtrapolationColor { get; set; }

        /// <summary>
        /// Gets the actual extrapolation color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualExtrapolationColor => this.ExtrapolationColor.GetActualColor(this.defaultExtrapolationColor);

        /// <summary>
        /// Gets or sets the dash array for the extrapolated intervals of the rendered line
        /// (overrides <see cref="ExtrapolationLineStyle" />). The default is <c>null</c>.
        /// </summary>
        /// <value>The dash array for extrapolated intervals.</value>
        /// <remarks>If this is not <c>null</c> it overrides the <see cref="ExtrapolationLineStyle" /> property.</remarks>
        public double[] ExtrapolationDashes { get; set; }

        /// <summary>
        /// Gets or sets the style for the extrapolated parts of the line.
        /// </summary>
        public LineStyle ExtrapolationLineStyle { get; set; }

        /// <summary>
        /// Gets the actual extrapolation line style.
        /// </summary>
        public LineStyle ActualExtrapolationLineStyle
        {
            get
            {
                return this.ExtrapolationLineStyle != LineStyle.Automatic ?
                    this.ExtrapolationLineStyle :
                    this.defaultExtrapolationLineStyle;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the extrapolated regions of the series will
        /// be taken into account when calculating the minima and maxima of the dataset.
        /// These regions will hence also be ignored when auto-scaling the axes.
        /// </summary>
        public bool IgnoreExtraplotationForScaling { get; set; }

        /// <summary>
        /// Gets the list of X intervals within which the line is rendered using the second color and style.
        /// </summary>
        public IList<DataRange> Intervals { get; } = new List<DataRange>();

        /// <summary>
        /// Gets the actual dash array for the extrapolated parts of the line.
        /// </summary>
        protected double[] ActualExtrapolationDashArray
        {
            get
            {
                return this.ExtrapolationDashes ?? this.ActualExtrapolationLineStyle.GetDashArray();
            }
        }

        /// <summary>
        /// Renders the legend symbol for the extrapolation line series on the
        /// specified rendering context. Both lines (normal and extrapolated)
        /// are displayed.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The bounding rectangle of the legend box.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            var xmid = (legendBox.Left + legendBox.Right) / 2;
            var ymid = (legendBox.Top + legendBox.Bottom) / 2;

            var pts = new[] { new ScreenPoint(legendBox.Left, ymid), new ScreenPoint(xmid, ymid) };

            rc.DrawLine(
                pts,
                this.GetSelectableColor(this.ActualColor),
                this.StrokeThickness,
                this.EdgeRenderingMode,
                this.ActualDashArray);

            pts = new[] { new ScreenPoint(xmid, ymid), new ScreenPoint(legendBox.Right, ymid) };

            rc.DrawLine(
                pts,
                this.GetSelectableColor(this.ActualExtrapolationColor),
                this.StrokeThickness,
                this.EdgeRenderingMode,
                this.ActualExtrapolationDashArray);

            var midpt = new ScreenPoint(xmid, ymid);

            rc.DrawMarker(
                midpt,
                this.MarkerType,
                this.MarkerOutline,
                this.MarkerSize,
                this.ActualMarkerFill,
                this.MarkerStroke,
                this.MarkerStrokeThickness,
                this.EdgeRenderingMode);
        }

        /// <summary>
        /// Updates the data and sorts the intervals.
        /// </summary>
        protected internal override void UpdateData()
        {
            base.UpdateData();

            this.orderedIntervals = this.MergeOverlaps(this.Intervals);
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            if (this.IgnoreExtraplotationForScaling && this.orderedIntervals.Any())
            {
                this.MinX = this.Points
                    .Where(p => !this.InAnyInterval(p.X))
                    .Select(p => p.X)
                    .Where(x => !double.IsNaN(x))
                    .MinOrDefault(double.NaN);

                this.MinY = this.Points
                    .Where(p => !this.InAnyInterval(p.X))
                    .Select(p => p.Y)
                    .Where(y => !double.IsNaN(y))
                    .MinOrDefault(double.NaN);

                this.MaxX = this.Points
                    .Where(p => !this.InAnyInterval(p.X))
                    .Select(p => p.X)
                    .Where(x => !double.IsNaN(x))
                    .MaxOrDefault(double.NaN);

                this.MaxY = this.Points
                    .Where(p => !this.InAnyInterval(p.X))
                    .Select(p => p.Y)
                    .Where(y => !double.IsNaN(y))
                    .MaxOrDefault(double.NaN);
            }
            else
            {
                base.UpdateMaxMin();
            }
        }

        /// <summary>
        /// Renders a continuous line.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="pointsToRender">The points to render.</param>
        protected override void RenderLine(IRenderContext rc, IList<ScreenPoint> pointsToRender)
        {
            if (this.StrokeThickness <= 0 || this.ActualLineStyle == LineStyle.None)
            {
                return;
            }

            var clippingRect = this.GetClippingRect();

            var p1 = this.InverseTransform(clippingRect.BottomLeft);
            var p2 = this.InverseTransform(clippingRect.TopRight);

            var minX = Math.Min(p1.X, p2.X);
            var maxX = Math.Max(p1.X, p2.X);

            var minY = Math.Min(p1.Y, p2.Y);
            var maxY = Math.Max(p1.Y, p2.Y);

            var clippingRectangles = this.CreateClippingRectangles(clippingRect, minX, maxX, minY, maxY);

            foreach (var rect in clippingRectangles)
            {
                var centerX = this.InverseTransform(rect.Center).X;

                bool isInterval = this.orderedIntervals != null
                    && this.orderedIntervals.Any(i => i.Contains(centerX));

                using (rc.AutoResetClip(rect))
                {
                    this.RenderLinePart(rc, pointsToRender, isInterval);
                }
            }
        }

        /// <summary>
        /// Creates clipping rectangles for the parts of the line which are either
        /// rendered in normal style or in extrapolation style.
        /// </summary>
        private IEnumerable<OxyRect> CreateClippingRectangles(
            OxyRect clippingRect, double minX, double maxX, double minY, double maxY)
        {
            var previous = minX;

            if (this.orderedIntervals != null && this.orderedIntervals.Any())
            {
                IEnumerable<double> flatLimits
                    = this.Flatten(this.orderedIntervals).Where(l => l >= minX && l <= maxX);

                foreach (var limiter in flatLimits)
                {
                    yield return new OxyRect(
                        this.Transform(previous, minY),
                        this.Transform(limiter, maxY))
                        .Clip(clippingRect);

                    previous = limiter;
                }
            }

            yield return new OxyRect(
                this.Transform(previous, minY),
                this.Transform(maxX, maxY))
                .Clip(clippingRect);
        }

        /// <summary>
        /// Returns a flat sequence of doubles containing alternating minima
        /// and maxima of the original data range intervals.
        /// </summary>
        private IEnumerable<double> Flatten(IEnumerable<DataRange> intervals)
        {
            foreach (var interval in intervals)
            {
                yield return interval.Minimum;
                yield return interval.Maximum;
            }
        }

        /// <summary>
        /// Renders the part of the line which is given by the provided list of screen points.
        /// </summary>
        private void RenderLinePart(IRenderContext rc, IList<ScreenPoint> pointsToRender, bool isInterval)
        {
            OxyColor color = isInterval ? this.ExtrapolationColor : this.Color;

            var dashes = isInterval ?
                this.ActualExtrapolationDashArray :
                this.ActualDashArray;

            rc.DrawReducedLine(
                pointsToRender,
                this.MinimumSegmentLength * this.MinimumSegmentLength,
                this.GetSelectableColor(color),
                this.StrokeThickness,
                this.EdgeRenderingMode,
                dashes,
                this.LineJoin);
        }

        /// <summary>
        /// Sorts the intervals by their minimum and merges those intervals which overlap, i.e.
        /// replaces them by their union.
        /// </summary>
        private List<DataRange> MergeOverlaps(IEnumerable<DataRange> intervals)
        {
            var orderedList = new List<DataRange>();

            if (intervals != null)
            {
                IOrderedEnumerable<DataRange> ordered = intervals.OrderBy(i => i.Minimum);

                foreach (var current in ordered)
                {
                    DataRange previous = orderedList.LastOrDefault();

                    if (current.IntersectsWith(previous))
                    {
                        orderedList[orderedList.Count - 1]
                            = new DataRange(previous.Minimum, Math.Max(previous.Maximum, current.Maximum));
                    }
                    else
                    {
                        orderedList.Add(current);
                    }
                }
            }

            return orderedList;
        }

        /// <summary>
        /// Checks whether the given x-value is within any of the
        /// ordered intervals using binary search.
        /// </summary>
        /// <param name="x">The value to be checked for.</param>
        /// <returns><c>true</c> if x is inside any interval.</returns>
        private bool InAnyInterval(double x)
        {
            var min = 0;
            var max = this.orderedIntervals.Count - 1;

            while (min <= max)
            {
                var mid = (min + max) / 2;
                var comparison = this.Compare(this.orderedIntervals[mid], x);

                if (comparison == 0)
                {
                    return true;
                }
                else if (comparison < 0)
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether the given x-value is within the provided interval.
        /// </summary>
        /// <param name="interval">The interval to check against.</param>
        /// <param name="x">The value to be checked.</param>
        /// <returns>0 if x is within inclusive interval, -1 if x smaller interval's min, 1 if x larger interval's max.</returns>
        private int Compare(DataRange interval, double x)
        {
            if (x < interval.Minimum)
            {
                return -1;
            }

            if (x > interval.Maximum)
            {
                return 1;
            }

            return 0;
        }
    }
}
