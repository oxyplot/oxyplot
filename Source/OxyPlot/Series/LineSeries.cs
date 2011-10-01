// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Represents a line series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Represents a line series.
    /// </summary>
    public class LineSeries : DataPointSeries
    {
        #region Constants and Fields

        /// <summary>
        /// The smoothed points.
        /// </summary>
        private Lazy<IList<IDataPoint>> smoothedPoints;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LineSeries" /> class.
        /// </summary>
        public LineSeries()
        {
            this.MinimumSegmentLength = 2;
            this.StrokeThickness = 2;
            this.MarkerSize = 3;
            this.MarkerStrokeThickness = 1;
            this.CanTrackerInterpolatePoints = true;

            this.ResetSmoothedPoints();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        public LineSeries(string title)
            : this()
        {
            this.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries"/> class.
        /// </summary>
        /// <param name="color">
        /// The color of the line stroke.
        /// </param>
        /// <param name="strokeThickness">
        /// The stroke thickness (optional).
        /// </param>
        /// <param name="title">
        /// The title (optional).
        /// </param>
        public LineSeries(OxyColor color, double strokeThickness = 1, string title = null)
            : this()
        {
            this.Color = color;
            this.StrokeThickness = strokeThickness;
            this.Title = title;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the color of the curve.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color { get; set; }

        /// <summary>
        ///   Gets or sets the dashes array. 
        ///   If this is not null it overrides the LineStyle property.
        /// </summary>
        /// <value>The dashes.</value>
        public double[] Dashes { get; set; }

        /// <summary>
        ///   Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public OxyPenLineJoin LineJoin { get; set; }

        /// <summary>
        ///   Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        ///   Gets or sets the marker fill color.
        /// </summary>
        /// <value>The marker fill.</value>
        public OxyColor MarkerFill { get; set; }

        /// <summary>
        ///   Gets or sets the marker outline polygon.
        ///   If this property is set, the MarkerType will not be used.
        /// </summary>
        /// <value>The marker outline.</value>
        public ScreenPoint[] MarkerOutline { get; set; }

        /// <summary>
        ///   Gets or sets the size of the marker.
        /// </summary>
        /// <value>The size of the marker.</value>
        public double MarkerSize { get; set; }

        /// <summary>
        ///   Gets or sets the marker stroke.
        /// </summary>
        /// <value>The marker stroke.</value>
        public OxyColor MarkerStroke { get; set; }

        /// <summary>
        ///   Gets or sets the marker stroke thickness.
        /// </summary>
        /// <value>The marker stroke thickness.</value>
        public double MarkerStrokeThickness { get; set; }

        /// <summary>
        ///   Gets or sets the type of the marker.
        /// </summary>
        /// <value>The type of the marker.</value>
        public MarkerType MarkerType { get; set; }

        /// <summary>
        ///   Gets or sets the minimum length of the segment.
        ///   Increasing this number will increase performance, 
        ///   but make the curve less accurate.
        /// </summary>
        /// <value>The minimum length of the segment.</value>
        public double MinimumSegmentLength { get; set; }

        /// <summary>
        ///   Gets or sets the thickness of the curve.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the smoothed points.
        /// </summary>
        /// <value>The smoothed points.</value>
        protected IList<IDataPoint> SmoothedPoints
        {
            get
            {
                return this.smoothedPoints.Value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The get nearest point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="interpolate">
        /// The interpolate.
        /// </param>
        /// <returns>
        /// A hit result for the nearest point.
        /// </returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (interpolate)
            {
                // Cannot interpolate if there is no line
                if (this.Color == null || this.StrokeThickness == 0)
                {
                    return null;
                }

                if (!this.CanTrackerInterpolatePoints)
                {
                    return null;
                }
            }

            if (interpolate && this.Smooth)
            {
                // interpolate smoothed line
                int index;
                DataPoint dpn;
                ScreenPoint spn;
                if (this.GetNearestInterpolatedPointInternal(this.SmoothedPoints, point, out dpn, out spn, out index))
                {
                    object item = this.SmoothedPoints[index];
                    return new TrackerHitResult(this, dpn, spn, item);
                }
            }

            return base.GetNearestPoint(point, interpolate);
        }

        /// <summary>
        /// Renders the LineSeries on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="model">
        /// The owner plot model.
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);

            if (this.points.Count == 0)
            {
                return;
            }

            Debug.Assert(this.XAxis != null && this.YAxis != null, "Axis has not been defined.");

            double minDistSquared = this.MinimumSegmentLength * this.MinimumSegmentLength;
            OxyRect clippingRect = this.GetClippingRect();

            var transformedPoints = new List<ScreenPoint>();

            Action<IList<ScreenPoint>> renderPoints = allPoints =>
                {
                    IList<ScreenPoint> screenPoints = allPoints;
                    if (this.Smooth)
                    {
                        // spline smoothing (should only be used on small datasets)
                        IList<ScreenPoint> resampledPoints = ScreenPointHelper.ResamplePoints(
                            allPoints, this.MinimumSegmentLength);
                        screenPoints = CanonicalSplineHelper.CreateSpline(resampledPoints, 0.5, null, false, 0.25);
                    }

                    // clip the line segments with the clipping rectangle
                    if (this.StrokeThickness > 0 && this.LineStyle != LineStyle.None)
                    {
                        rc.DrawClippedLine(
                            screenPoints, 
                            clippingRect, 
                            minDistSquared, 
                            this.Color, 
                            this.StrokeThickness, 
                            this.LineStyle, 
                            this.LineJoin, 
                            false);
                    }

                    if (this.MarkerType != MarkerType.None)
                    {
                        rc.DrawMarkers(
                            allPoints, 
                            clippingRect, 
                            this.MarkerType, 
                            this.MarkerOutline, 
                            new[] { this.MarkerSize }, 
                            this.MarkerFill, 
                            this.MarkerStroke, 
                            this.MarkerStrokeThickness);
                    }
                };

            // Transform all points to screen coordinates
            // Render the line when invalid points occur
            foreach (var point in this.points)
            {
                if (!this.IsValidPoint(point, this.XAxis, this.YAxis))
                {
                    renderPoints(transformedPoints);
                    transformedPoints.Clear();
                    continue;
                }

                transformedPoints.Add(this.XAxis.Transform(point.X, point.Y, this.YAxis));
            }

            renderPoints(transformedPoints);
        }

        /// <summary>
        /// Renders the legend symbol for the line series on the 
        ///   specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="legendBox">
        /// The bounding rectangle of the legend box.
        /// </param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double ymid = (legendBox.Top + legendBox.Bottom) / 2;
            var pts = new[] { new ScreenPoint(legendBox.Left, ymid), new ScreenPoint(legendBox.Right, ymid) };
            rc.DrawLine(pts, this.Color, this.StrokeThickness, LineStyleHelper.GetDashArray(this.LineStyle));
            var midpt = new ScreenPoint(xmid, ymid);
            rc.DrawMarker(
                midpt, 
                legendBox, 
                this.MarkerType, 
                this.MarkerOutline, 
                this.MarkerSize, 
                this.MarkerFill, 
                this.MarkerStroke, 
                this.MarkerStrokeThickness);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The set default values.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        protected internal override void SetDefaultValues(PlotModel model)
        {
            // todo: should use ActualLineStyle and ActualColor?
            if (this.Color == null)
            {
                this.LineStyle = model.GetDefaultLineStyle();
                this.Color = model.GetDefaultColor();
                if (this.MarkerFill == null)
                {
                    this.MarkerFill = this.Color;
                }
            }
        }

        /// <summary>
        /// Updates the axes to include the max and min of this series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            if (this.Smooth)
            {
                // Make sure the smooth points are re-evaluated.
                this.ResetSmoothedPoints();

                var xl = this.SmoothedPoints.Select(s => s.X);

                this.MinX = xl.Min();
                this.MaxX = xl.Max();

                var yl = this.SmoothedPoints.Select(s => s.Y);

                this.MinY = yl.Min();
                this.MaxY = yl.Max();
            }
            else
            {
                base.UpdateMaxMin();
            }
        }

        /// <summary>
        /// Force the smoothed points to be re-evaluated.
        /// </summary>
        protected void ResetSmoothedPoints()
        {
            this.smoothedPoints = new Lazy<IList<IDataPoint>>(this.GetSmoothedPoints);
        }

        /// <summary>
        /// Gets the smoothed points.
        /// </summary>
        /// <returns>
        /// A list of smooth data points.
        /// </returns>
        private IList<IDataPoint> GetSmoothedPoints()
        {
            return CanonicalSplineHelper.CreateSpline(this.points, 0.5, null, false, 0.25);
        }

        #endregion
    }
}