using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OxyPlot
{
    /// <summary>
    ///   LineSeries are rendered to polylines.
    /// </summary>
    public class ScatterSeries : DataSeries
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "LineSeries" /> class.
        /// </summary>
        /// <param name = "color">The color of the line stroke.</param>
        /// <param name="radius"></param>
        /// <param name = "title">The title (optional).</param>
        public ScatterSeries(string title = null, OxyColor color = null, double markerSize = 5)
        {
            MarkerType = MarkerType.Square;
            MarkerSize = markerSize;
            Title = title;
        }

        //public string DataFieldSize { get; set; }
        //public string DataFieldSizeValue { get; set; }
        //public string DataFieldColor { get; set; }
        //public string DataFieldColorValue { get; set; }
        //public string DataFieldMarkerType { get; set; }

        //public IList<double> SizeValues { get; set; }

        // public IList<OxyColor> Colors { get; set; }
        //public IList<DataPoint> ColorValues { get; set; }

        /// <summary>
        /// Gets or sets the size of the marker.
        /// </summary>
        /// <value>The size of the marker.</value>
        public double MarkerSize { get; set; }

        /// <summary>
        /// Gets or sets the marker sizes.
        /// This overrides MarkerSize.
        /// </summary>
        /// <value>The marker sizes.</value>
        public IList<double> MarkerSizes { get; set; }

        /// <summary>
        ///   Gets or sets the type of the marker.
        /// </summary>
        /// <value>The type of the marker.</value>
        public MarkerType MarkerType { get; set; }

        /// <summary>
        /// Gets or sets the marker outline polygon.
        /// Set MarkerType to Custom to use this.
        /// </summary>
        /// <value>The marker outline.</value>
        public ScreenPoint[] MarkerOutline { get; set; }

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
        /// Gets or sets the screen resolution.
        /// If this number is greater than 1, bins of that size is created for both x and y directions. Only one point will be drawn in each bin.
        /// </summary>
        public int BinSize { get; set; }

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

            Debug.Assert(MarkerSizes==null || MarkerSizes.Count == Points.Count,"Number of Points and MarkerSizes should be equal.");
            var clippingRect = GetClippingRect();

            int n = points.Count;

            // Transform all points to screen coordinates
            var allPoints = new ScreenPoint[n];
            var markerSizes = new double[n];
            for (int i = 0; i < n; i++)
            {
                allPoints[i] = XAxis.Transform(points[i], YAxis);
                markerSizes[i] = MarkerSizes!=null ? MarkerSizes[i] : MarkerSize;
            }
            rc.DrawMarkers(allPoints, clippingRect, MarkerType, MarkerOutline, markerSizes, MarkerFill, MarkerStroke,
                           MarkerStrokeThickness, BinSize);
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

            var midpt = new ScreenPoint(xmid, ymid);
            rc.DrawMarker(midpt, legendBox, MarkerType, MarkerOutline, MarkerSize, MarkerFill, MarkerStroke, MarkerStrokeThickness);
        }

        public override void SetDefaultValues(PlotModel model)
        {
            if (MarkerFill == null)
                MarkerFill = model.GetDefaultColor();
        }
    }
}