//-----------------------------------------------------------------------
// <copyright file="ScatterSeries.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// ScatterSeries are used to create scatter plots.
    /// </summary>
    /// <remarks>
    /// http://en.wikipedia.org/wiki/Scatter_plot
    /// </remarks>
    public class ScatterSeries : DataPointSeries
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterSeries"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="markerFill">
        /// The marker fill color.
        /// </param>
        /// <param name="markerSize">
        /// Size of the markers (If ScatterPoint.Size is set, this value will be overriden).
        /// </param>
        public ScatterSeries(string title, OxyColor markerFill = null, double markerSize = 5)
            : this()
        {
            this.MarkerFill = markerFill;
            this.MarkerSize = markerSize;
            this.Title = title;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ScatterSeries" /> class.
        /// </summary>
        public ScatterSeries()
        {
            this.DataFieldSize = null;
            this.DataFieldValue = null;

            this.MarkerFill = null;
            this.MarkerSize = 5;
            this.MarkerType = MarkerType.Square;
            this.MarkerStroke = null;
            this.MarkerStrokeThickness = 1.0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the screen resolution.
        ///   If this number is greater than 1, bins of that size is created for both x and y directions. Only one point will be drawn in each bin.
        /// </summary>
        public int BinSize { get; set; }

        /// <summary>
        ///   Gets or sets the data field for the size.
        /// </summary>
        /// <value>The size data field.</value>
        public string DataFieldSize { get; set; }

        /// <summary>
        ///   Gets or sets the tag data field.
        /// </summary>
        /// <value>The tag data field.</value>
        public string DataFieldTag { get; set; }

        /// <summary>
        ///   Gets or sets the value data field.
        /// </summary>
        /// <value>The value data field.</value>
        public string DataFieldValue { get; set; }

        /// <summary>
        ///   Gets or sets the marker fill color.
        ///   If null, this color will be automatically set.
        /// </summary>
        /// <value>The marker fill color.</value>
        public OxyColor MarkerFill { get; set; }

        /// <summary>
        ///   Gets or sets the marker outline polygon.
        ///   Set MarkerType to Custom to use this.
        /// </summary>
        /// <value>The marker outline.</value>
        public ScreenPoint[] MarkerOutline { get; set; }

        /// <summary>
        ///   Gets or sets the size of the marker (same size for all items).
        /// </summary>
        /// <value>The size of the markers.</value>
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="interpolate">
        /// interpolate if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// A TrackerHitResult for the current hit.
        /// </returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (interpolate)
            {
                return null;
            }

            TrackerHitResult result = null;
            double minimumDistance = double.MaxValue;
            int i = 0;
            foreach (var p in this.points)
            {
                if (p.X < this.XAxis.ActualMinimum || p.X > this.XAxis.ActualMaximum || p.Y < this.YAxis.ActualMinimum
                    || p.Y > this.YAxis.ActualMaximum)
                {
                    continue;
                }

                var dp = new DataPoint(p.X, p.Y);
                ScreenPoint sp = AxisBase.Transform(dp, this.XAxis, this.YAxis);
                double dx = sp.x - point.x;
                double dy = sp.y - point.y;
                double d2 = dx * dx + dy * dy;

                if (d2 < minimumDistance)
                {
                    result = new TrackerHitResult(this, dp, sp, this.GetItem(this.ItemsSource, i));
                    minimumDistance = d2;
                }

                i++;
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified point is valid.
        /// </summary>
        /// <param name="pt">
        /// The pointt.
        /// </param>
        /// <param name="xAxis">
        /// The x axis.
        /// </param>
        /// <param name="yAxis">
        /// The y axis.
        /// </param>
        /// <returns>
        /// <c>true</c> if the point is valid; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsValidPoint(ScatterPoint pt, IAxis xAxis, IAxis yAxis)
        {
            return !double.IsNaN(pt.X) && !double.IsInfinity(pt.X) && !double.IsNaN(pt.Y) && !double.IsInfinity(pt.Y)
                   && (xAxis != null && xAxis.IsValidValue(pt.X)) && (yAxis != null && yAxis.IsValidValue(pt.Y));
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

            OxyRect clippingRect = this.GetClippingRect();

            int n = this.points.Count;

            // Transform all points to screen coordinates
            var allPoints = new ScreenPoint[n];
            var markerSizes = new double[n];
            for (int i = 0; i < n; i++)
            {
                var dp = new DataPoint(this.points[i].X, this.points[i].Y);
                double size = double.NaN;
                double value = double.NaN;
                if (this.points[i] is ScatterPoint)
                {
                    size = ((ScatterPoint)this.points[i]).Size;
                    value = ((ScatterPoint)this.points[i]).Value;
                }

                if (double.IsNaN(size))
                {
                    size = this.MarkerSize;
                }

                allPoints[i] = this.XAxis.Transform(dp.X, dp.Y, this.YAxis);
                markerSizes[i] = size;
            }

            var binOffset = this.XAxis.Transform(this.MinX, this.MaxY, this.YAxis);

            // Draw the markers
            rc.DrawMarkers(
                allPoints, 
                clippingRect, 
                this.MarkerType, 
                this.MarkerOutline, 
                markerSizes, 
                this.MarkerFill, 
                this.MarkerStroke, 
                this.MarkerStrokeThickness, 
                this.BinSize, 
                binOffset);
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
        /// Sets the default values.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        protected internal override void SetDefaultValues(PlotModel model)
        {
            if (this.MarkerFill == null)
            {
                this.MarkerFill = model.GetDefaultColor();
            }
        }

        /// <summary>
        /// The update data.
        /// </summary>
        protected internal override void UpdateData()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            this.points.Clear();

            // Use the mapping to generate the points
            if (this.Mapping != null)
            {
                foreach (var item in this.ItemsSource)
                {
                    this.points.Add(this.Mapping(item));
                }

                return;
            }

            // Get DataPoints from the items in ItemsSource 
            // if they implement IDataPointProvider
            // If DataFields are set, this is not used
            /*if (DataFieldX == null || DataFieldY == null)
            {
                foreach (var item in ItemsSource)
                {
                    var idpp = item as IScatterPointProvider;
                    if (idpp == null)
                    {
                        continue;
                    }

                    points.Add(idpp.GetScatterPoint());
                }

                return;
            }*/

            // Using reflection to add points
            this.AddScatterPoints(
                this.Points, 
                this.ItemsSource, 
                this.DataFieldX, 
                this.DataFieldY, 
                this.DataFieldSize, 
                this.DataFieldValue, 
                this.DataFieldTag);
        }

        /// <summary>
        /// Updates the max/min from the datapoints.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            this.InternalUpdateMaxMin(this.points);
        }

        /// <summary>
        /// The add scatter points.
        /// </summary>
        /// <param name="dest">
        /// The dest.
        /// </param>
        /// <param name="itemsSource">
        /// The items source.
        /// </param>
        /// <param name="dataFieldX">
        /// The data field x.
        /// </param>
        /// <param name="dataFieldY">
        /// The data field y.
        /// </param>
        /// <param name="dataFieldSize">
        /// The data field size.
        /// </param>
        /// <param name="dataFieldValue">
        /// The data field value.
        /// </param>
        /// <param name="dataFieldTag">
        /// The data field tag.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        protected void AddScatterPoints(
            IList<IDataPoint> dest, 
            IEnumerable itemsSource, 
            string dataFieldX, 
            string dataFieldY, 
            string dataFieldSize, 
            string dataFieldValue, 
            string dataFieldTag)
        {
            PropertyInfo pix = null;
            PropertyInfo piy = null;
            PropertyInfo pis = null;
            PropertyInfo piv = null;
            PropertyInfo pit = null;
            Type t = null;

            foreach (var o in itemsSource)
            {
                if (pix == null || o.GetType() != t)
                {
                    t = o.GetType();
                    pix = t.GetProperty(dataFieldX);
                    piy = t.GetProperty(dataFieldY);
                    pis = dataFieldSize != null ? t.GetProperty(dataFieldSize) : null;
                    piv = dataFieldValue != null ? t.GetProperty(dataFieldValue) : null;
                    pit = dataFieldTag != null ? t.GetProperty(dataFieldTag) : null;
                    if (pix == null)
                    {
                        throw new InvalidOperationException(
                            string.Format("Could not find data field {0} on type {1}", this.DataFieldX, t));
                    }

                    if (piy == null)
                    {
                        throw new InvalidOperationException(
                            string.Format("Could not find data field {0} on type {1}", this.DataFieldY, t));
                    }
                }

                double x = this.ToDouble(pix.GetValue(o, null));
                double y = this.ToDouble(piy.GetValue(o, null));
                double s = pis != null ? this.ToDouble(pis.GetValue(o, null)) : double.NaN;
                double v = piv != null ? this.ToDouble(piv.GetValue(o, null)) : double.NaN;
                object tag = pit != null ? pit.GetValue(o, null) : null;

                var p = new ScatterPoint(x, y, s, v, tag);
                dest.Add(p);
            }
        }

        /// <summary>
        /// Updates the Max/Min limits from the specified point list.
        /// </summary>
        /// <param name="pts">
        /// The points.
        /// </param>
        protected void InternalUpdateMaxMin(IList<ScatterPoint> pts)
        {
            if (pts == null || pts.Count == 0)
            {
                return;
            }

            double minx = this.MinX;
            double miny = this.MinY;
            double maxx = this.MaxX;
            double maxy = this.MaxY;

            foreach (var pt in pts)
            {
                if (!this.IsValidPoint(pt, this.XAxis, this.YAxis))
                {
                    continue;
                }

                if (pt.x < minx || double.IsNaN(minx))
                {
                    minx = pt.x;
                }

                if (pt.x > maxx || double.IsNaN(maxx))
                {
                    maxx = pt.x;
                }

                if (pt.y < miny || double.IsNaN(miny))
                {
                    miny = pt.y;
                }

                if (pt.y > maxy || double.IsNaN(maxy))
                {
                    maxy = pt.y;
                }
            }

            this.MinX = minx;
            this.MinY = miny;
            this.MaxX = maxx;
            this.MaxY = maxy;

            this.XAxis.Include(this.MinX);
            this.XAxis.Include(this.MaxX);
            this.YAxis.Include(this.MinY);
            this.YAxis.Include(this.MaxY);
        }

        #endregion
    }
}
