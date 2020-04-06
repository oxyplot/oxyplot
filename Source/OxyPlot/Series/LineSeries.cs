// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a line series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a line series.
    /// </summary>
    public class LineSeries : DataPointSeries
    {
        /// <summary>
        /// The divisor value used to calculate tolerance for line smoothing.
        /// </summary>
        private const double ToleranceDivisor = 200;

        /// <summary>
        /// The output buffer.
        /// </summary>
        private List<ScreenPoint> outputBuffer;

        /// <summary>
        /// The buffer for contiguous screen points.
        /// </summary>
        private List<ScreenPoint> contiguousScreenPointsBuffer;

        /// <summary>
        /// The buffer for decimated points.
        /// </summary>
        private List<ScreenPoint> decimatorBuffer;

        /// <summary>
        /// The default color.
        /// </summary>
        private OxyColor defaultColor;

        /// <summary>
        /// The default marker fill color.
        /// </summary>
        private OxyColor defaultMarkerFill;

        /// <summary>
        /// The default line style.
        /// </summary>
        private LineStyle defaultLineStyle;

        /// <summary>
        /// The smoothed points.
        /// </summary>
        private List<DataPoint> smoothedPoints;

        /// <summary>
        /// Initializes a new instance of the <see cref = "LineSeries" /> class.
        /// </summary>
        public LineSeries()
        {
            this.StrokeThickness = 2;
            this.LineJoin = LineJoin.Bevel;
            this.LineStyle = LineStyle.Automatic;

            this.Color = OxyColors.Automatic;
            this.BrokenLineColor = OxyColors.Undefined;

            this.MarkerFill = OxyColors.Automatic;
            this.MarkerStroke = OxyColors.Automatic;
            this.MarkerResolution = 0;
            this.MarkerSize = 3;
            this.MarkerStrokeThickness = 1;
            this.MarkerType = MarkerType.None;

            this.MinimumSegmentLength = 2;

            this.CanTrackerInterpolatePoints = true;
            this.LabelMargin = 6;
            this.smoothedPoints = new List<DataPoint>();
        }

        /// <summary>
        /// Gets or sets the color of the curve.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets or sets the color of the broken line segments. The default is <see cref="OxyColors.Undefined"/>. Set it to <see cref="OxyColors.Automatic"/> if it should follow the <see cref="Color" />.
        /// </summary>
        /// <remarks>Add <c>DataPoint.Undefined</c> in the Points collection to create breaks in the line.</remarks>
        public OxyColor BrokenLineColor { get; set; }

        /// <summary>
        /// Gets or sets the broken line style. The default is <see cref="OxyPlot.LineStyle.Solid" />.
        /// </summary>
        public LineStyle BrokenLineStyle { get; set; }

        /// <summary>
        /// Gets or sets the broken line thickness. The default is <c>0</c> (no line).
        /// </summary>
        public double BrokenLineThickness { get; set; }

        /// <summary>
        /// Gets or sets the dash array for the rendered line (overrides <see cref="LineStyle" />). The default is <c>null</c>.
        /// </summary>
        /// <value>The dash array.</value>
        /// <remarks>If this is not <c>null</c> it overrides the <see cref="LineStyle" /> property.</remarks>
        public double[] Dashes { get; set; }

        /// <summary>
        /// Gets or sets the decimator.
        /// </summary>
        /// <value>
        /// The decimator action.
        /// </value>
        /// <remarks>The decimator can be used to improve the performance of the rendering. See the example.</remarks>
        public Action<List<ScreenPoint>, List<ScreenPoint>> Decimator { get; set; }

        /// <summary>
        /// Gets or sets the label format string. The default is <c>null</c> (no labels).
        /// </summary>
        /// <value>The label format string.</value>
        public string LabelFormatString { get; set; }

        /// <summary>
        /// Gets or sets the label margins. The default is <c>6</c>.
        /// </summary>
        public double LabelMargin { get; set; }

        /// <summary>
        /// Gets or sets the line join. The default is <see cref="OxyPlot.LineJoin.Bevel" />.
        /// </summary>
        /// <value>The line join.</value>
        public LineJoin LineJoin { get; set; }

        /// <summary>
        /// Gets or sets the line style. The default is <see cref="OxyPlot.LineStyle.Automatic" />.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the position of a legend rendered on the line. The default is <c>LineLegendPosition.None</c>.
        /// </summary>
        /// <value>A value specifying the position of the legend.</value>
        public LineLegendPosition LineLegendPosition { get; set; }

        /// <summary>
        /// Gets or sets the marker fill color. The default is <see cref="OxyColors.Automatic" />.
        /// </summary>
        /// <value>The marker fill.</value>
        public OxyColor MarkerFill { get; set; }

        /// <summary>
        /// Gets or sets the a custom polygon outline for the markers. Set <see cref="MarkerType" /> to <see cref="OxyPlot.MarkerType.Custom" /> to use this property. The default is <c>null</c>.
        /// </summary>
        /// <value>A polyline.</value>
        public ScreenPoint[] MarkerOutline { get; set; }

        /// <summary>
        /// Gets or sets the marker resolution. The default is <c>0</c>.
        /// </summary>
        /// <value>The marker resolution.</value>
        public int MarkerResolution { get; set; }

        /// <summary>
        /// Gets or sets the size of the marker. The default is <c>3</c>.
        /// </summary>
        /// <value>The size of the marker.</value>
        public double MarkerSize { get; set; }

        /// <summary>
        /// Gets or sets the marker stroke. The default is <c>OxyColors.Automatic</c>.
        /// </summary>
        /// <value>The marker stroke.</value>
        public OxyColor MarkerStroke { get; set; }

        /// <summary>
        /// Gets or sets the marker stroke thickness. The default is <c>2</c>.
        /// </summary>
        /// <value>The marker stroke thickness.</value>
        public double MarkerStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the type of the marker. The default is <c>MarkerType.None</c>.
        /// </summary>
        /// <value>The type of the marker.</value>
        /// <remarks>If MarkerType.Custom is used, the MarkerOutline property must be specified.</remarks>
        public MarkerType MarkerType { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of the segment.
        /// Increasing this number will increase performance,
        /// but make the curve less accurate. The default is <c>2</c>.
        /// </summary>
        /// <value>The minimum length of the segment.</value>
        public double MinimumSegmentLength { get; set; }

        /// <summary>
        /// Gets or sets a type of interpolation algorithm used for smoothing this <see cref = "DataPointSeries" />.
        /// </summary>
        /// <value>Type of interpolation algorithm.</value>
        public IInterpolationAlgorithm InterpolationAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the curve.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets the actual color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualColor
        {
            get
            {
                return this.Color.GetActualColor(this.defaultColor);
            }
        }

        /// <summary>
        /// Gets the actual marker fill color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualMarkerFill
        {
            get
            {
                return this.MarkerFill.GetActualColor(this.defaultMarkerFill);
            }
        }

        /// <summary>
        /// Gets the actual line style.
        /// </summary>
        /// <value>The actual line style.</value>
        protected LineStyle ActualLineStyle
        {
            get
            {
                return this.LineStyle != LineStyle.Automatic ? this.LineStyle : this.defaultLineStyle;
            }
        }

        /// <summary>
        /// Gets the actual dash array for the line.
        /// </summary>
        protected double[] ActualDashArray
        {
            get
            {
                return this.Dashes ?? this.ActualLineStyle.GetDashArray();
            }
        }

        /// <summary>
        /// Gets the smoothed points.
        /// </summary>
        /// <value>The smoothed points.</value>
        protected List<DataPoint> SmoothedPoints
        {
            get
            {
                return this.smoothedPoints;
            }
        }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (interpolate)
            {
                // Cannot interpolate if there is no line
                if (this.ActualColor.IsInvisible() || this.StrokeThickness.Equals(0))
                {
                    return null;
                }

                if (!this.CanTrackerInterpolatePoints)
                {
                    return null;
                }
            }

            if (interpolate && this.InterpolationAlgorithm != null)
            {
                var result = this.GetNearestInterpolatedPointInternal(this.SmoothedPoints, point);
                if (result != null)
                {
                    result.Text = StringHelper.Format(
                        this.ActualCulture,
                        this.TrackerFormatString,
                        result.Item,
                        this.Title,
                        this.XAxis.Title ?? XYAxisSeries.DefaultXAxisTitle,
                        this.XAxis.GetValue(result.DataPoint.X),
                        this.YAxis.Title ?? XYAxisSeries.DefaultYAxisTitle,
                        this.YAxis.GetValue(result.DataPoint.Y));
                }

                return result;
            }

            return base.GetNearestPoint(point, interpolate);
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            var actualPoints = this.ActualPoints;
            if (actualPoints == null || actualPoints.Count == 0)
            {
                return;
            }

            this.VerifyAxes();

            var clippingRect = this.GetClippingRect();
            rc.SetClip(clippingRect);

            this.RenderPoints(rc, clippingRect, actualPoints);

            if (this.LabelFormatString != null)
            {
                // render point labels (not optimized for performance)
                this.RenderPointLabels(rc, clippingRect);
            }

            rc.ResetClip();

            if (this.LineLegendPosition != LineLegendPosition.None && !string.IsNullOrEmpty(this.Title))
            {
                // renders a legend on the line
                this.RenderLegendOnLine(rc, clippingRect);
            }
        }

        /// <summary>
        /// Renders the legend symbol for the line series on the
        /// specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The bounding rectangle of the legend box.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double ymid = (legendBox.Top + legendBox.Bottom) / 2;
            var pts = new[] { new ScreenPoint(legendBox.Left, ymid), new ScreenPoint(legendBox.Right, ymid) };
            rc.DrawLine(
                pts,
                this.GetSelectableColor(this.ActualColor),
                this.StrokeThickness,
                this.EdgeRenderingMode,
                this.ActualDashArray);
            var midpt = new ScreenPoint(xmid, ymid);
            rc.DrawMarker(
                legendBox,
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
        /// Sets default values from the plot model.
        /// </summary>
        protected internal override void SetDefaultValues()
        {
            if (this.LineStyle == LineStyle.Automatic)
            {
                this.defaultLineStyle = this.PlotModel.GetDefaultLineStyle();
            }

            if (this.Color.IsAutomatic())
            {
                this.defaultColor = this.PlotModel.GetDefaultColor();
            }

            if (this.MarkerFill.IsAutomatic())
            {
                // No color was explicitly provided. Use the line color if it was set, else use default.
                this.defaultMarkerFill = this.Color.IsAutomatic() ? this.defaultColor : this.Color;
            }
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            if (this.InterpolationAlgorithm != null)
            {
                // Update the max/min from the control points
                base.UpdateMaxMin();

                // Make sure the smooth points are re-evaluated.
                this.ResetSmoothedPoints();

                if (this.SmoothedPoints.Count == 0)
                {
                    return;
                }

                // Update the max/min from the smoothed points
                this.MinX = this.SmoothedPoints.Where(x => !double.IsNaN(x.X)).Min(x => x.X);
                this.MinY = this.SmoothedPoints.Where(x => !double.IsNaN(x.Y)).Min(x => x.Y);
                this.MaxX = this.SmoothedPoints.Where(x => !double.IsNaN(x.X)).Max(x => x.X);
                this.MaxY = this.SmoothedPoints.Where(x => !double.IsNaN(x.Y)).Max(x => x.Y);
            }
            else
            {
                base.UpdateMaxMin();
            }
        }

        /// <summary>
        /// Renders the points as line, broken line and markers.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <param name="points">The points to render.</param>
        protected void RenderPoints(IRenderContext rc, OxyRect clippingRect, IList<DataPoint> points)
        {
            var lastValidPoint = new ScreenPoint?();
            var areBrokenLinesRendered = this.BrokenLineThickness > 0 && this.BrokenLineStyle != LineStyle.None;
            var dashArray = areBrokenLinesRendered ? this.BrokenLineStyle.GetDashArray() : null;
            var broken = areBrokenLinesRendered ? new List<ScreenPoint>(2) : null;

            if (this.contiguousScreenPointsBuffer == null)
            {
                this.contiguousScreenPointsBuffer = new List<ScreenPoint>(points.Count);
            }

			int startIdx = 0;
			double xmax = double.MaxValue;

			if (this.IsXMonotonic)
			{
				// determine render range
				var xmin = this.XAxis.ActualMinimum;
				xmax = this.XAxis.ActualMaximum;
				this.WindowStartIndex = this.UpdateWindowStartIndex(points, point => point.X, xmin, this.WindowStartIndex);
				
				startIdx = this.WindowStartIndex;
			}

			for (int i = startIdx; i < points.Count; i++)
	        {
				if (!this.ExtractNextContiguousLineSegment(points, ref i, ref lastValidPoint, xmax, broken, this.contiguousScreenPointsBuffer))
		        {
			        break;
		        }

				if (areBrokenLinesRendered)
				{
					if (broken.Count > 0)
					{
						var actualBrokenLineColor = this.BrokenLineColor.IsAutomatic()
														? this.ActualColor
														: this.BrokenLineColor;

						rc.DrawClippedLineSegments(
							clippingRect,
							broken,
							actualBrokenLineColor,
							this.BrokenLineThickness,
                            this.EdgeRenderingMode,
							dashArray,
							this.LineJoin);
						broken.Clear();
					}
				}
				else
				{
					lastValidPoint = null;
				}

				if (this.Decimator != null)
				{
					if (this.decimatorBuffer == null)
					{
						this.decimatorBuffer = new List<ScreenPoint>(this.contiguousScreenPointsBuffer.Count);
					}
					else
					{
						this.decimatorBuffer.Clear();
					}

					this.Decimator(this.contiguousScreenPointsBuffer, this.decimatorBuffer);
					this.RenderLineAndMarkers(rc, clippingRect, this.decimatorBuffer);
				}
				else
				{
					this.RenderLineAndMarkers(rc, clippingRect, this.contiguousScreenPointsBuffer);
				}

				this.contiguousScreenPointsBuffer.Clear();
			}
        }

	    /// <summary>
	    /// Extracts a single contiguous line segment beginning with the element at the position of the enumerator when the method
	    /// is called. Initial invalid data points are ignored.
	    /// </summary>
	    /// <param name="pointIdx">Current point index</param>
	    /// <param name="previousContiguousLineSegmentEndPoint">Initially set to null, but I will update I won't give a broken line if this is null</param>
	    /// <param name="xmax">Maximum visible X value</param>
	    /// <param name="broken">place to put broken segment</param>
	    /// <param name="contiguous">place to put contiguous segment</param>
	    /// <param name="points">Points collection</param>
	    /// <returns>
	    ///   <c>true</c> if line segments are extracted, <c>false</c> if reached end.
	    /// </returns>
	    protected bool ExtractNextContiguousLineSegment(
			IList<DataPoint> points,
			ref int pointIdx,
			ref ScreenPoint? previousContiguousLineSegmentEndPoint,
			double xmax,
            // ReSharper disable SuggestBaseTypeForParameter
            List<ScreenPoint> broken,
            List<ScreenPoint> contiguous)
        // ReSharper restore SuggestBaseTypeForParameter
        {
            DataPoint currentPoint = default(DataPoint);
		    bool hasValidPoint = false;
		    
            // Skip all undefined points
		    for (; pointIdx < points.Count; pointIdx++)
		    {
				currentPoint = points[pointIdx];
			    if (currentPoint.X > xmax)
			    {
				    return false;
			    }
			    
				// ReSharper disable once AssignmentInConditionalExpression
			    if (hasValidPoint = this.IsValidPoint(currentPoint))
			    {
				    break;
			    }
		    }

		    if (!hasValidPoint)
		    {
			    return false;
		    }

            // First valid point
            var screenPoint = this.Transform(currentPoint);

            // Handle broken line segment if exists
            if (previousContiguousLineSegmentEndPoint.HasValue)
            {
                broken.Add(previousContiguousLineSegmentEndPoint.Value);
                broken.Add(screenPoint);
            }

            // Add first point
            contiguous.Add(screenPoint);

			// Add all points up until the next invalid one
			int clipCount = 0;
			for (pointIdx++; pointIdx < points.Count; pointIdx++)
		    {
				currentPoint = points[pointIdx];
				clipCount += currentPoint.X > xmax ? 1 : 0;
				if (clipCount > 1)
				{
					break;
				}
				if (!this.IsValidPoint(currentPoint))
			    {
				    break;
			    }

				screenPoint = this.Transform(currentPoint);
				contiguous.Add(screenPoint);
			}

			previousContiguousLineSegmentEndPoint = screenPoint;

            return true;
        }

        /// <summary>
        /// Renders the point labels.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        protected void RenderPointLabels(IRenderContext rc, OxyRect clippingRect)
        {
            int index = -1;
            foreach (var point in this.ActualPoints)
            {
                index++;

                if (!this.IsValidPoint(point))
                {
                    continue;
                }

                var pt = this.Transform(point) + new ScreenVector(0, -this.LabelMargin);

                if (!clippingRect.Contains(pt))
                {
                    continue;
                }

                var item = this.GetItem(index);
                var s = StringHelper.Format(this.ActualCulture, this.LabelFormatString, item, point.X, point.Y);

#if SUPPORTLABELPLACEMENT
                    switch (this.LabelPlacement)
                    {
                        case LabelPlacement.Inside:
                            pt = new ScreenPoint(rect.Right - this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                            ha = HorizontalAlignment.Right;
                            break;
                        case LabelPlacement.Middle:
                            pt = new ScreenPoint((rect.Left + rect.Right) / 2, (rect.Top + rect.Bottom) / 2);
                            ha = HorizontalAlignment.Center;
                            break;
                        case LabelPlacement.Base:
                            pt = new ScreenPoint(rect.Left + this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                            ha = HorizontalAlignment.Left;
                            break;
                        default: // Outside
                            pt = new ScreenPoint(rect.Right + this.LabelMargin, (rect.Top + rect.Bottom) / 2);
                            ha = HorizontalAlignment.Left;
                            break;
                    }
#endif

                rc.DrawClippedText(
                    clippingRect,
                    pt,
                    s,
                    this.ActualTextColor,
                    this.ActualFont,
                    this.ActualFontSize,
                    this.ActualFontWeight,
                    0,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Bottom);
            }
        }

        /// <summary>
        /// Renders a legend on the line.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        protected void RenderLegendOnLine(IRenderContext rc, OxyRect clippingRect)
        {
            // Find the position
            DataPoint point;
            HorizontalAlignment ha;
            var va = VerticalAlignment.Middle;
            double dx = 4;

            switch (this.LineLegendPosition)
            {
                case LineLegendPosition.Start:
                    point = this.ActualPoints[0];
                    ha = HorizontalAlignment.Right;
                    dx = -dx;
                    break;
                case LineLegendPosition.End:
                    point = this.ActualPoints[this.ActualPoints.Count - 1];
                    ha = HorizontalAlignment.Left;
                    break;
                case LineLegendPosition.None:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.Orientate(ref ha, ref va);
            var pt = this.Transform(point) + this.Orientate(new ScreenVector(dx, 0));

            // Render the legend
            rc.DrawClippedText(
                clippingRect,
                pt,
                this.Title,
                this.ActualTextColor,
                this.ActualFont,
                this.ActualFontSize,
                this.ActualFontWeight,
                0,
                ha,
                va);
        }

        /// <summary>
        /// Renders the transformed points as a line (smoothed if <see cref="InterpolationAlgorithm"/> isn’t <c>null</c>) and markers (if <see cref="MarkerType"/> is not <c>None</c>).
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <param name="pointsToRender">The points to render.</param>
        protected virtual void RenderLineAndMarkers(IRenderContext rc, OxyRect clippingRect, IList<ScreenPoint> pointsToRender)
        {
            var screenPoints = pointsToRender;
            if (this.InterpolationAlgorithm != null)
            {
                // spline smoothing (should only be used on small datasets)
                var resampledPoints = ScreenPointHelper.ResamplePoints(pointsToRender, this.MinimumSegmentLength);
                screenPoints = this.InterpolationAlgorithm.CreateSpline(resampledPoints, false, 0.25);
            }

            // clip the line segments with the clipping rectangle
            if (this.StrokeThickness > 0 && this.ActualLineStyle != LineStyle.None)
            {
                this.RenderLine(rc, clippingRect, screenPoints);
            }

            if (this.MarkerType != MarkerType.None)
            {
                var markerBinOffset = this.MarkerResolution > 0 ? this.Transform(this.MinX, this.MinY) : default(ScreenPoint);

                rc.DrawMarkers(
                    clippingRect, 
                    pointsToRender, 
                    this.MarkerType, 
                    this.MarkerOutline, 
                    new[] { this.MarkerSize }, 
                    this.ActualMarkerFill, 
                    this.MarkerStroke, 
                    this.MarkerStrokeThickness, 
                    this.EdgeRenderingMode,
                    this.MarkerResolution, 
                    markerBinOffset);
            }
        }

        /// <summary>
        /// Renders a continuous line.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <param name="pointsToRender">The points to render.</param>
        protected virtual void RenderLine(IRenderContext rc, OxyRect clippingRect, IList<ScreenPoint> pointsToRender)
        {
            var dashArray = this.ActualDashArray;

            if (this.outputBuffer == null)
            {
                this.outputBuffer = new List<ScreenPoint>(pointsToRender.Count);
            }

            rc.DrawClippedLine(
                clippingRect, 
                pointsToRender, 
                this.MinimumSegmentLength * this.MinimumSegmentLength, 
                this.GetSelectableColor(this.ActualColor), 
                this.StrokeThickness, 
                this.EdgeRenderingMode,
                dashArray, 
                this.LineJoin, 
                this.outputBuffer);
        }

        /// <summary>
        /// Force the smoothed points to be re-evaluated.
        /// </summary>
        protected virtual void ResetSmoothedPoints()
        {
            double tolerance = Math.Abs(Math.Max(this.MaxX - this.MinX, this.MaxY - this.MinY) / ToleranceDivisor);
            this.smoothedPoints = this.InterpolationAlgorithm.CreateSpline(this.ActualPoints, false, tolerance);
        }

        /// <summary>
        /// Represents a line segment.
        /// </summary>
        protected class Segment
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Segment" /> class.
            /// </summary>
            /// <param name="point1">The first point of the segment.</param>
            /// <param name="point2">The second point of the segment.</param>
            public Segment(DataPoint point1, DataPoint point2)
            {
                this.Point1 = point1;
                this.Point2 = point2;
            }

            /// <summary>
            /// Gets the first point1 of the segment.
            /// </summary>
            public DataPoint Point1 { get; private set; }

            /// <summary>
            /// Gets the second point of the segment.
            /// </summary>
            public DataPoint Point2 { get; private set; }
        }
    }
}
