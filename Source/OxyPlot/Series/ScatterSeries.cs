using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace OxyPlot
{
    public struct ScatterPoint : IDataPoint
    {
        internal double x;
        internal double y;
        internal double size;
        internal double value;
        internal object tag;

        public ScatterPoint(double x, double y, double size=double.NaN, double value = double.NaN, object tag=null)
        {
            this.x = x;
            this.y = y;
            this.size = size;
            this.value = value;
            this.tag = tag;
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Size
        {
            get { return size; }
            set { size = value; }
        }
        
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public override string ToString()
        {
            return x + " " + y;
        }
        
    }
    /// <summary>
    ///   LineSeries are rendered to polylines.
    /// </summary>
    public class ScatterSeries : PlotSeriesBase
    {
        protected IList<ScatterPoint> points;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries"/> class.
        /// </summary>
        /// <param name="title">The title (optional).</param>
        /// <param name="color">The color of the line stroke.</param>
        /// <param name="markerSize">Size of the markers.</param>
        public ScatterSeries(string title = null, OxyColor color = null, double markerSize = 5)
        {
            points=new List<ScatterPoint>();
            DataFieldX = "X";
            DataFieldY = "Y";
            DataFieldSize = null;
            DataFieldValue = null;

            MarkerType = MarkerType.Square;
            MarkerSize = markerSize;
            Title = title;
        }

        /// <summary>
        ///   Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        ///   Gets or sets the data field X.
        /// </summary>
        /// <value>The data field X.</value>
        public string DataFieldX { get; set; }

        /// <summary>
        ///   Gets or sets the data field Y.
        /// </summary>
        /// <value>The data field Y.</value>
        public string DataFieldY { get; set; }

        public string DataFieldSize { get; set; }
        public string DataFieldValue { get; set; }

        //public string DataFieldMarkerType { get; set; }

        /// <summary>
        /// Gets or sets the size of the marker (same size for all items).
        /// </summary>
        /// <value>The size of the markers.</value>
        public double MarkerSize { get; set; }

        /// <summary>
        /// Gets or sets the marker sizes (independent size for each item).
        /// If this property is set, it overrides MarkerSize.
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
        /// Gets or sets the marker stroke.
        /// </summary>
        /// <value>The marker stroke.</value>
        public OxyColor MarkerStroke { get; set; }

        /// <summary>
        /// Gets or sets the marker stroke thickness.
        /// </summary>
        /// <value>The marker stroke thickness.</value>
        public double MarkerStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the marker fill color.
        /// </summary>
        /// <value>The marker fill color.</value>
        public OxyColor MarkerFill { get; set; }

        /// <summary>
        /// Gets or sets the screen resolution.
        /// If this number is greater than 1, bins of that size is created for both x and y directions. Only one point will be drawn in each bin.
        /// </summary>
        public int BinSize { get; set; }

        public IList<ScatterPoint> Points
        {
            get { return points; }
            set { points = value; }
        }

        /// <summary>
        /// Renders the LineSeries on the specified rendering context.
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
                var dp = new DataPoint(points[i].x, points[i].y);
                allPoints[i] = XAxis.Transform(dp, YAxis);
                markerSizes[i] = double.IsNaN(points[i].Size) ? MarkerSize : points[i].Size * MarkerSize;
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

        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            return null;
        }

        public override void SetDefaultValues(PlotModel model)
        {
            if (MarkerFill == null)
                MarkerFill = model.GetDefaultColor();
        }
    }
}