using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OxyPlot
{
    /// <summary>
    ///   LineSeries are rendered to polylines.
    /// </summary>
    public class LineSeries : DataSeries
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "LineSeries" /> class.
        /// </summary>
        public LineSeries()
        {
            MinimumSegmentLength = 2;
            StrokeThickness = 2;
            MarkerSize = 3;
            MarkerStrokeThickness = 1;
            CanTrackerInterpolatePoints = true;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LineSeries" /> class.
        /// </summary>
        /// <param name = "title">The title.</param>
        public LineSeries(string title)
            : this()
        {
            Title = title;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LineSeries" /> class.
        /// </summary>
        /// <param name = "color">The color of the line stroke.</param>
        /// <param name = "strokeThickness">The stroke thickness (optional).</param>
        /// <param name = "title">The title (optional).</param>
        public LineSeries(OxyColor color, double strokeThickness = 1, string title = null)
            : this()
        {
            Color = color;
            StrokeThickness = strokeThickness;
            Title = title;
        }

        /// <summary>
        ///   Gets or sets the color of the curve.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color { get; set; }

        /// <summary>
        ///   Gets or sets the thickness of the curve.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        ///   Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        ///   Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public OxyPenLineJoin LineJoin { get; set; }

        /// <summary>
        ///   Gets or sets the dashes array. 
        ///   If this is not null it overrides the LineStyle property.
        /// </summary>
        /// <value>The dashes.</value>
        public double[] Dashes { get; set; }

        /// <summary>
        ///   Gets or sets the type of the marker.
        /// </summary>
        /// <value>The type of the marker.</value>
        public MarkerType MarkerType { get; set; }

        /// <summary>
        /// Gets or sets the marker outline polygon.
        /// If this property is set, the MarkerType will not be used.
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
        ///   Gets or sets the marker fill color.
        /// </summary>
        /// <value>The marker fill.</value>
        public OxyColor MarkerFill { get; set; }

        /// <summary>
        ///   Gets or sets the minimum length of the segment.
        ///   Increasing this number will increase performance, 
        ///   but make the curve less accurate.
        /// </summary>
        /// <value>The minimum length of the segment.</value>
        public double MinimumSegmentLength { get; set; }

        /// <summary>
        ///   Renders the LineSeries on the specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "model">The owner plot model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);

            if (points.Count == 0)
            {
                return;
            }

            Debug.Assert(XAxis != null && YAxis != null, "Axis has not been defined.");

            double minDistSquared = MinimumSegmentLength * MinimumSegmentLength;

            var clippingRect = GetClippingRect();

            int n = points.Count;

            // Transform all points to screen coordinates
            var allPoints = new ScreenPoint[n];
            for (int i = 0; i < n; i++)
            {
                allPoints[i] = XAxis.Transform(points[i], YAxis);
            }

            // spline smoothing (should only be used on small datasets)
            // todo: could do spline smoothing only on the visible part of the curve...
            var screenPoints = Smooth
                                   ? CanonicalSplineHelper.CreateSpline(allPoints, 0.5, null, false, 0.25).ToArray()
                                   : allPoints;

            // clip the line segments with the clipping rectangle
            if (StrokeThickness>0 && LineStyle!=LineStyle.None)
                rc.DrawClippedLine(screenPoints, clippingRect, minDistSquared, Color, StrokeThickness, LineStyle, LineJoin,false);
            if (MarkerType!=MarkerType.None)
                rc.DrawMarkers(allPoints, clippingRect, MarkerType, MarkerOutline, new[] { MarkerSize }, MarkerFill, MarkerStroke,
                               MarkerStrokeThickness);
        }

        /// <summary>
        ///   Renders the legend symbol for the line series on the 
        ///   specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "legendBox">The bounding rectangle of the legend box.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            double xmid = (legendBox.Left + legendBox.Right) / 2;
            double ymid = (legendBox.Top + legendBox.Bottom) / 2;
            var pts = new[]
                          {
                              new ScreenPoint(legendBox.Left, ymid), 
                              new ScreenPoint(legendBox.Right, ymid)
                          };
            rc.DrawLine(pts, Color, StrokeThickness, LineStyleHelper.GetDashArray(LineStyle));
            var midpt = new ScreenPoint(xmid, ymid);
            rc.DrawMarker(midpt, legendBox, MarkerType, MarkerOutline, MarkerSize, MarkerFill, MarkerStroke, MarkerStrokeThickness);
        }

        public override void SetDefaultValues(PlotModel model)
        {
            // todo: should use ActualLineStyle and ActualColor?
            if (Color == null)
            {
                LineStyle = model.GetDefaultLineStyle();
                Color = model.GetDefaultColor();
                if (MarkerFill == null)
                    MarkerFill = Color;
            }
        }
    }
}