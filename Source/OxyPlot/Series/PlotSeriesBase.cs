namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Abstract base class for Series that contains an X-axis and Y-axis
    /// </summary>
    public abstract class PlotSeriesBase : ITrackableSeries
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the background of the series.
        ///   The background area is defined by the x and y axes.
        /// </summary>
        /// <value>The background color.</value>
        public OxyColor Background { get; set; }

        /// <summary>
        ///   Gets or sets the maximum x-coordinate of the dataset.
        /// </summary>
        /// <value>The maximum x-coordinate.</value>
        public double MaxX { get; protected set; }

        /// <summary>
        ///   Gets or sets the maximum y-coordinate of the dataset.
        /// </summary>
        /// <value>The maximum y-coordinate.</value>
        public double MaxY { get; protected set; }

        /// <summary>
        ///   Gets or sets the minimum x-coordinate of the dataset.
        /// </summary>
        /// <value>The minimum x-coordinate.</value>
        public double MinX { get; protected set; }

        /// <summary>
        ///   Gets or sets the minimum y-coordinate of the dataset.
        /// </summary>
        /// <value>The minimum y-coordinate.</value>
        public double MinY { get; protected set; }

        /// <summary>
        ///   Gets the title of the Series.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a format string used for the tracker.
        /// </summary>
        public string TrackerFormatString { get; set; }

        /// <summary>
        /// Gets or sets the key for the tracker to use on this series.
        /// </summary>
        public string TrackerKey { get; set; }

        /// <summary>
        ///   Gets or sets the x-axis.
        /// </summary>
        /// <value>The x-axis.</value>
        public IAxis XAxis { get; set; }

        /// <summary>
        ///   Gets or sets the x-axis key.
        /// </summary>
        /// <value>The x-axis key.</value>
        public string XAxisKey { get; set; }

        /// <summary>
        ///   Gets or sets the y-axis.
        /// </summary>
        /// <value>The y-axis.</value>
        public IAxis YAxis { get; set; }

        /// <summary>
        ///   Gets or sets the y-axis key.
        /// </summary>
        /// <value>The y-axis key.</value>
        public string YAxisKey { get; set; }

        #endregion

        #region Public Methods

        public virtual bool AreAxesRequired()
        {
            return true;
        }

        public void EnsureAxes(Collection<IAxis> axes, IAxis defaultXAxis, IAxis defaultYAxis)
        {
            if (this.XAxisKey != null)
            {
                this.XAxis = axes.FirstOrDefault(a => a.Key == this.XAxisKey);
            }

            if (this.YAxisKey != null)
            {
                this.YAxis = axes.FirstOrDefault(a => a.Key == this.YAxisKey);
            }

            // If axes are not found, use the default axes
            if (this.XAxis == null)
            {
                this.XAxis = defaultXAxis;
            }

            if (this.YAxis == null)
            {
                this.YAxis = defaultYAxis;
            }
        }

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">interpolate if set to <c>true</c>.</param>
        /// <returns>
        /// A TrackerHitResult for the current hit.
        /// </returns>
        public abstract TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate);

        /// <summary>
        /// Gets the rectangle the series uses on the screen (screen coordinates).
        /// </summary>
        /// <returns></returns>
        public OxyRect GetScreenRectangle()
        {
            return this.GetClippingRect();
        }

        public virtual bool IsUsing(IAxis axis)
        {
            return this.XAxis == axis || this.YAxis == axis;
        }

        /// <summary>
        ///   Renders the Series on the specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "model">The model.</param>
        public virtual void Render(IRenderContext rc, PlotModel model)
        {
        }

        /// <summary>
        ///   Renders the legend symbol on the specified rendering context.
        /// </summary>
        /// <param name = "rc">The rendering context.</param>
        /// <param name = "legendBox">The rect.</param>
        public virtual void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
        }

        public virtual void SetDefaultValues(PlotModel model)
        {
        }

        public virtual void UpdateData()
        {
        }

        /// <summary>
        ///   Updates the max/minimum values.
        /// </summary>
        public virtual void UpdateMaxMin()
        {
            this.MinX = this.MinY = this.MaxX = this.MaxY = double.NaN;
        }

        #endregion

        #region Methods

        protected OxyRect GetClippingRect()
        {
            double minX = Math.Min(this.XAxis.ScreenMin.X, this.XAxis.ScreenMax.X);
            double minY = Math.Min(this.YAxis.ScreenMin.Y, this.YAxis.ScreenMax.Y);
            double maxX = Math.Max(this.XAxis.ScreenMin.X, this.XAxis.ScreenMax.X);
            double maxY = Math.Max(this.YAxis.ScreenMin.Y, this.YAxis.ScreenMax.Y);

            return new OxyRect(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Gets the item of the specified index.
        /// Returns null if ItemsSource is not set, or the index is outside the boundaries.
        /// </summary>
        protected object GetItem(IEnumerable itemsSource, int index)
        {
            if (itemsSource == null || index < 0)
            {
                return null;
            }

            var list = itemsSource as IList;
            if (list != null)
            {
                if (index < list.Count && index >= 0)
                {
                    return list[index];
                }
                return null;
            }

            // todo: can this be improved?
            int i = 0;
            foreach (object item in itemsSource)
            {
                if (i++ == index)
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        ///   Gets the point on the curve that is nearest the specified point.
        /// </summary>
        /// <param name = "point">The point.</param>
        /// <param name = "dpn">The nearest point (data coordinates).</param>
        /// <param name = "spn">The nearest point (screen coordinates).</param>
        /// <returns></returns>
        protected bool GetNearestInterpolatedPointInternal(
            IList<DataPoint> points, ScreenPoint point, out DataPoint dpn, out ScreenPoint spn, out int index)
        {
            spn = default(ScreenPoint);
            dpn = default(DataPoint);
            index = -1;

            // http://local.wasp.uwa.edu.au/~pbourke/geometry/pointline/
            double minimumDistance = double.MaxValue;

            for (int i = 0; i + 1 < points.Count; i++)
            {
                DataPoint p1 = points[i];
                DataPoint p2 = points[i + 1];
                ScreenPoint sp1 = AxisBase.Transform(p1, this.XAxis, this.YAxis);
                ScreenPoint sp2 = AxisBase.Transform(p2, this.XAxis, this.YAxis);

                double sp21X = sp2.x - sp1.x;
                double sp21Y = sp2.y - sp1.y;
                double u1 = (point.x - sp1.x) * sp21X + (point.y - sp1.y) * sp21Y;
                double u2 = sp21X * sp21X + sp21Y * sp21Y;
                double ds = sp21X * sp21X + sp21Y * sp21Y;

                if (ds < 4)
                {
                    // if the points are very close, we can get numerical problems, just use the first point...
                    u1 = 0;
                    u2 = 1;
                }

                if (u2 < double.Epsilon && u2 > -double.Epsilon)
                {
                    continue; // P1 && P2 coincident
                }

                double u = u1 / u2;
                if (u < 0 || u > 1)
                {
                    continue; // outside line
                }

                double sx = sp1.x + u * sp21X;
                double sy = sp1.y + u * sp21Y;

                double dx = point.x - sx;
                double dy = point.y - sy;
                double distance = dx * dx + dy * dy;

                if (distance < minimumDistance)
                {
                    double px = p1.x + u * (p2.x - p1.x);
                    double py = p1.y + u * (p2.y - p1.y);
                    dpn = new DataPoint(px, py);
                    spn = new ScreenPoint(sx, sy);
                    minimumDistance = distance;
                    index = i;
                }
            }

            return minimumDistance < double.MaxValue;
        }

        protected bool GetNearestPointInternal(
            IEnumerable<DataPoint> points, ScreenPoint point, out DataPoint dpn, out ScreenPoint spn, out int index)
        {
            spn = default(ScreenPoint);
            dpn = default(DataPoint);
            index = -1;

            double minimumDistance = double.MaxValue;
            int i = 0;
            foreach (DataPoint p in points)
            {
                ScreenPoint sp = AxisBase.Transform(p, this.XAxis, this.YAxis);
                double dx = sp.x - point.x;
                double dy = sp.y - point.y;
                double d2 = dx * dx + dy * dy;

                if (d2 < minimumDistance)
                {
                    dpn = p;
                    spn = sp;
                    minimumDistance = d2;
                    index = i;
                }
                i++;
            }

            return minimumDistance < double.MaxValue;
        }

        /// <summary>
        /// Converts the value of the specified object to a double precision floating point number.
        /// DateTime objects are converted using DateTimeAxis.ToDouble
        /// TimeSpan objects are converted using TimeSpanAxis.ToDouble
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected virtual double ToDouble(object value)
        {
            if (value is DateTime)
            {
                return DateTimeAxis.ToDouble((DateTime)value);
            }

            if (value is TimeSpan)
            {
                return ((TimeSpan)value).TotalSeconds;
            }

            return Convert.ToDouble(value);
        }

        #endregion
    }
}