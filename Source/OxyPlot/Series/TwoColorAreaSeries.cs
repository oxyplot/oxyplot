// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoColorAreaSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a two-color area series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a two-color area series.
    /// </summary>
    public class TwoColorAreaSeries : AreaSeries
    {
        /// <summary>
        /// The default second color.
        /// </summary>
        private OxyColor defaultColor2;

        /// <summary>
        /// The collection of points above the limit.
        /// </summary>
        private List<DataPoint> abovePoints;

        /// <summary>
        /// The collection of points below the limit.
        /// </summary>
        private List<DataPoint> belowPoints;

        /// <summary>
        /// Start index of a visible rendering window for markers.
        /// </summary>
        private int markerStartIndex;
        
        /// <summary>
        /// Initializes a new instance of the <see cref = "TwoColorAreaSeries" /> class.
        /// </summary>
        public TwoColorAreaSeries()
        {
            this.Color2 = OxyColors.Blue;

            this.Fill = OxyColors.Automatic;
            this.Fill2 = OxyColors.Automatic;

            this.MarkerFill2 = OxyColors.Automatic;
            this.MarkerStroke2 = OxyColors.Automatic;
            this.LineStyle2 = LineStyle.Solid;
        }

        /// <summary>
        /// Gets or sets the area fill color below the limit line.
        /// </summary>
        /// <value>The fill below the limit line.</value>
        public OxyColor Fill2 { get; set; }

        /// <summary>
        /// Gets the actual fill color below the limit line.
        /// </summary>
        /// <value>The actual fill below the limit line.</value>
        public OxyColor ActualFill2
        {
            get
            {
                return this.Fill2.GetActualColor(OxyColor.FromAColor(100, this.ActualColor2));
            }
        }

        /// <summary>
        /// Gets the actual second color.
        /// </summary>
        /// <value>The actual color.</value>
        public override OxyColor ActualColor2
        {
            get { return this.Color2.GetActualColor(this.defaultColor2); }
        }

        /// <summary>
        /// Gets or sets the dash array for the rendered line that is below the limit (overrides <see cref="LineStyle" />).
        /// </summary>
        /// <value>The dash array.</value>
        /// <remarks>If this is not <c>null</c> it overrides the <see cref="LineStyle" /> property.</remarks>
        public double[] Dashes2 { get; set; }

        /// <summary>
        /// Gets or sets the line style for the part of the line that is below the limit.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle2 { get; set; }

        /// <summary>
        /// Gets the actual line style for the part of the line that is below the limit.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle ActualLineStyle2
        {
            get
            {
                return this.LineStyle2 != LineStyle.Automatic ? this.LineStyle2 : LineStyle.Solid;
            }
        }

        /// <summary>
        /// Gets the actual dash array for the line that is below the limit.
        /// </summary>
        public double[] ActualDashArray2
        {
            get
            {
                return this.Dashes2 ?? this.ActualLineStyle2.GetDashArray();
            }
        }

        /// <summary>
        /// Gets or sets the marker fill color which is below the limit line. The default is <see cref="OxyColors.Automatic" />.
        /// </summary>
        /// <value>The marker fill.</value>
        public OxyColor MarkerFill2 { get; set; }

        /// <summary>
        /// Gets or sets the marker stroke which is below the limit line. The default is <c>OxyColors.Automatic</c>.
        /// </summary>
        /// <value>The marker stroke.</value>
        public OxyColor MarkerStroke2 { get; set; }

        /// <summary>
        /// Gets or sets a baseline for the series.
        /// </summary>
        public double Limit { get; set; }

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate if set to <c>true</c> .</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            TrackerHitResult result;

            if (interpolate && this.CanTrackerInterpolatePoints)
            {
                result = this.GetNearestInterpolatedPointInternal(this.ActualPoints, point);
            }
            else
            {
                result = this.GetNearestPointInternal(this.ActualPoints, point);
            }

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

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            // determine render range
            var xmin = this.XAxis.ActualMinimum;
            var xmax = this.XAxis.ActualMaximum;
            this.WindowStartIndex = this.UpdateWindowStartIndex(this.abovePoints, this.GetPointX, xmin, this.WindowStartIndex);
            this.WindowStartIndex2 = this.UpdateWindowStartIndex(this.belowPoints, this.GetPointX, xmin, this.WindowStartIndex2);

            double minDistSquared = this.MinimumSegmentLength * this.MinimumSegmentLength;

            var clippingRect = this.GetClippingRect();
            rc.SetClip(clippingRect);

            var areaContext = new TwoColorAreaRenderContext
            {
                Points = this.abovePoints,
                WindowStartIndex = this.WindowStartIndex,
                XMax = xmax,
                RenderContext = rc,
                ClippingRect = clippingRect,
                MinDistSquared = minDistSquared,
                Reverse = false,
                Color = this.ActualColor,
                Fill = this.ActualFill,
                MarkerFill = this.ActualMarkerFill,
                MarkerStroke = this.MarkerStroke,
                DashArray = this.ActualDashArray,
                Baseline = this.Limit
            };

            this.RenderChunkedPoints(areaContext);

            areaContext.Points = this.belowPoints;
            areaContext.Reverse = this.Reverse2;
            areaContext.Color = this.ActualColor2;
            areaContext.Fill = this.ActualFill2;
            areaContext.MarkerFill = this.MarkerFill2;
            areaContext.MarkerStroke = this.MarkerStroke2;
            areaContext.DashArray = this.ActualDashArray2;
            if (this.IsPoints2Defined)
            {
                areaContext.Baseline = this.ConstantY2;
            }

            this.RenderChunkedPoints(areaContext);

            if (!this.IsPoints2Defined)
            {
                var markerSizes = new[] { this.MarkerSize };
                double limit = this.Limit;
                var points = this.ActualPoints;
                var aboveMarkers = new List<ScreenPoint>();
                var belowMarkers = new List<ScreenPoint>();
                this.markerStartIndex = this.UpdateWindowStartIndex(points, this.GetPointX, xmin, this.markerStartIndex);

                int markerClipCount = 0;
                for (int i = this.markerStartIndex; i < points.Count; i++)
                {
                    var point = points[i];
                    (point.y >= limit ? aboveMarkers : belowMarkers).Add(this.Transform(point.x, point.y));

                    markerClipCount += point.x > xmax ? 1 : 0;
                    if (markerClipCount > 1)
                    {
                        break;
                    }
                }

                rc.DrawMarkers(
                    clippingRect,
                    aboveMarkers,
                    this.MarkerType,
                    null,
                    markerSizes, 
                    this.ActualMarkerFill, 
                    this.MarkerStroke,
                    this.MarkerStrokeThickness,
                    this.EdgeRenderingMode,
                    1);
                rc.DrawMarkers(
                    clippingRect,
                    belowMarkers,
                    this.MarkerType,
                    null,
                    markerSizes,
                    this.MarkerFill2,
                    this.MarkerStroke2,
                    this.MarkerStrokeThickness,
                    this.EdgeRenderingMode,
                    1);
            }

            rc.ResetClip();
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected internal override void SetDefaultValues()
        {
            base.SetDefaultValues();

            if (this.Color2.IsAutomatic())
            {
                this.defaultColor2 = this.PlotModel.GetDefaultColor();
            }

            if (this.LineStyle2 == LineStyle.Automatic)
            {
                this.LineStyle2 = this.PlotModel.GetDefaultLineStyle();
            }
        }

        /// <summary>
        /// The update data.
        /// </summary>
        protected internal override void UpdateData()
        {
            base.UpdateData();

            if (this.IsPoints2Defined)
            {
                this.abovePoints = this.ActualPoints;
                this.belowPoints = this.ActualPoints2;
            }
            else
            {
                this.SplitPoints(this.ActualPoints);
            }
        }

        /// <summary>
        /// Renders a chunk of points on the screen.
        /// </summary>
        /// <param name="context">Render context.</param>
        /// <param name="points">Screen points.</param>
        /// <returns>The list of resampled points.</returns>
        protected override List<ScreenPoint> RenderScreenPoints(AreaRenderContext context, List<ScreenPoint> points)
        {
            var result = base.RenderScreenPoints(context, points);
            var twoColorContext = (TwoColorAreaRenderContext)context;

            var baseline = this.GetConstantScreenPoints2(result, twoColorContext.Baseline);
            var poligon = new List<ScreenPoint>(baseline);
            poligon.AddRange(result);

            context.RenderContext.DrawClippedPolygon(
                context.ClippingRect,
                poligon,
                context.MinDistSquared,
                this.GetSelectableFillColor(twoColorContext.Fill),
                OxyColors.Undefined,
                0,
                this.EdgeRenderingMode);

            if (this.IsPoints2Defined)
            {
                var markerSizes = new[] { this.MarkerSize };

                // draw the markers on top
                context.RenderContext.DrawMarkers(
                    context.ClippingRect,
                    result,
                    this.MarkerType,
                    null,
                    markerSizes,
                    twoColorContext.MarkerFill,
                    twoColorContext.MarkerStroke,
                    this.MarkerStrokeThickness,
                    this.EdgeRenderingMode,
                    1);
            }

            return result;
        }

        /// <summary>
        /// Splits a collection of points into two collections based on their Y value.
        /// </summary>
        /// <param name="source">A collection of points to split.</param>
        private void SplitPoints(List<DataPoint> source)
        {
            var nan = new DataPoint(double.NaN, double.NaN);
            double limit = this.Limit;
            this.abovePoints = new List<DataPoint>(source.Count);
            this.belowPoints = new List<DataPoint>(source.Count);

            bool lastAbove = false;
            DataPoint? lastPoint = null;
            foreach (var point in source)
            {
                bool isAbove = point.y >= limit;

                if (lastPoint != null && isAbove != lastAbove)
                {
                    var shared = new DataPoint(this.GetInterpolatedX(lastPoint.Value, point, limit), limit);
                    this.abovePoints.Add(isAbove ? nan : shared);
                    this.abovePoints.Add(isAbove ? shared : nan);

                    this.belowPoints.Add(isAbove ? shared : nan);
                    this.belowPoints.Add(isAbove ? nan : shared);
                }

                (isAbove ? this.abovePoints : this.belowPoints).Add(point);

                lastPoint = point;
                lastAbove = isAbove;
            }
        }

        /// <summary>
        /// Gets the screen points when baseline is used.
        /// </summary>
        /// <param name="source">The list of polygon screen points.</param>
        /// <param name="baseline">Baseline Y value for the polygon.</param>
        /// <returns>A sequence of <see cref="T:DataPoint"/>.</returns>
        private List<ScreenPoint> GetConstantScreenPoints2(List<ScreenPoint> source, double baseline)
        {
            var result = new List<ScreenPoint>();

            if (double.IsNaN(baseline) || source.Count <= 0)
            {
                return result;
            }

            var p1 = this.InverseTransform(source[0]);
            p1 = new DataPoint(p1.X, baseline);
            result.Add(this.Transform(p1));

            var p2 = this.InverseTransform(source[source.Count - 1]);
            p2 = new DataPoint(p2.X, baseline);
            result.Add(this.Transform(p2));

            if (this.Reverse2)
            {
                result.Reverse();
            }

            return result;
        }

        /// <summary>
        /// Gets interpolated X coordinate for given Y on a straight line
        /// between two points.
        /// </summary>
        /// <param name="a">First point.</param>
        /// <param name="b">Second point.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>Corresponding X coordinate.</returns>
        private double GetInterpolatedX(DataPoint a, DataPoint b, double y)
        {
            return (((y - a.y) / (b.y - a.y)) * (b.x - a.x)) + a.x;
        }

        /// <summary>
        /// Render context for two color area plot.
        /// </summary>
        protected class TwoColorAreaRenderContext : AreaRenderContext
        {
            /// <summary>
            /// Gets or sets area baseline value.
            /// </summary>
            public double Baseline { get; set; }

            /// <summary>
            /// Gets or sets polygon fill color.
            /// </summary>
            public OxyColor Fill { get; set; }

            /// <summary>
            /// Gets or sets marker fill color.
            /// </summary>
            public OxyColor MarkerFill { get; set; }

            /// <summary>
            /// Gets or sets marker stroke color.
            /// </summary>
            public OxyColor MarkerStroke { get; set; }
        }
    }
}
