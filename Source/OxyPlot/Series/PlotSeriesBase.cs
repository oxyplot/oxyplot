using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace OxyPlot
{
    /// <summary>
    /// Abstract base class for Series that contains an X-axis and Y-axis
    /// </summary>
    public abstract class PlotSeriesBase : ITrackableSeries
    {
        /// <summary>
        ///   Gets or sets the X axis.
        /// </summary>
        /// <value>The X axis.</value>
        public IAxis XAxis { get; set; }

        /// <summary>
        ///   Gets or sets the Y axis.
        /// </summary>
        /// <value>The Y axis.</value>
        public IAxis YAxis { get; set; }

        /// <summary>
        ///   Gets or sets the X axis key.
        /// </summary>
        /// <value>The X axis key.</value>
        public string XAxisKey { get; set; }

        /// <summary>
        ///   Gets or sets the Y axis key.
        /// </summary>
        /// <value>The Y axis key.</value>
        public string YAxisKey { get; set; }

        /// <summary>
        /// Gets or sets the key for the tracker to use on this series.
        /// </summary>
        public string TrackerKey { get; set; }

        /// <summary>
        ///   Gets or sets the min X of the dataset.
        /// </summary>
        /// <value>The min X.</value>
        public double MinX { get; protected set; }

        /// <summary>
        ///   Gets or sets the max X of the dataset.
        /// </summary>
        /// <value>The max X.</value>
        public double MaxX { get; protected set; }

        /// <summary>
        ///   Gets or sets the min Y of the dataset.
        /// </summary>
        /// <value>The min Y.</value>
        public double MinY { get; protected set; }

        /// <summary>
        ///   Gets or sets the max Y of the dataset.
        /// </summary>
        /// <value>The max Y.</value>
        public double MaxY { get; protected set; }

        /// <summary>
        ///   Gets or sets the background of the series.
        ///   The background area is defined by the x and y axes.
        /// </summary>
        /// <value>The background.</value>
        public OxyColor Background { get; set; }


        #region ISeries Members

        /// <summary>
        ///   Gets the title of the Series.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

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

        public virtual void UpdateData()
        {
        }

        public virtual bool AreAxesRequired()
        {
            return true;
        }

        public virtual bool IsUsing(IAxis axis)
        {
            return XAxis == axis || YAxis == axis;
        }

        public void EnsureAxes(Collection<IAxis> axes, IAxis defaultXAxis, IAxis defaultYAxis)
        {
            if (XAxisKey != null)
            {
                XAxis = axes.FirstOrDefault(a => a.Key == XAxisKey);
            }

            if (YAxisKey != null)
            {
                YAxis = axes.FirstOrDefault(a => a.Key == YAxisKey);
            }

            // If axes are not found, use the default axes
            if (XAxis == null)
            {
                XAxis = defaultXAxis;
            }

            if (YAxis == null)
            {
                YAxis = defaultYAxis;
            }
        }

        /// <summary>
        /// Gets the rectangle the series uses on the screen (screen coordinates).
        /// </summary>
        /// <returns></returns>
        public OxyRect GetScreenRectangle()
        {
            return GetClippingRect();
        }

        protected OxyRect GetClippingRect()
        {
            var minX = Math.Min(XAxis.ScreenMin.X, XAxis.ScreenMax.X);
            var minY = Math.Min(YAxis.ScreenMin.Y, YAxis.ScreenMax.Y);
            var maxX = Math.Max(XAxis.ScreenMin.X, XAxis.ScreenMax.X);
            var maxY = Math.Max(YAxis.ScreenMin.Y, YAxis.ScreenMax.Y);

            return new OxyRect(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        ///   Updates the max/min values.
        /// </summary>
        public virtual void UpdateMaxMin()
        {
            MinX = MinY = MaxX = MaxY = double.NaN;
        }


        public abstract TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate);

        public virtual void SetDefaultValues(PlotModel model)
        {
        }

        /// <summary>
        /// Gets or sets a format string used for the tracker.
        /// </summary>
        public string TrackerFormatString { get; set; }

        #endregion

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

        protected bool GetNearestPointInternal(IEnumerable<DataPoint> points, ScreenPoint point, out DataPoint dpn, out ScreenPoint spn, out int index)
        {
            spn = default(ScreenPoint);
            dpn = default(DataPoint);
            index = -1;

            double minimumDistance = double.MaxValue;
            int i = 0;
            foreach (var p in points)
            {
                var sp = AxisBase.Transform(p, XAxis, YAxis);
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
        ///   Gets the point on the curve that is nearest the specified point.
        /// </summary>
        /// <param name = "point">The point.</param>
        /// <param name = "dpn">The nearest point (data coordinates).</param>
        /// <param name = "spn">The nearest point (screen coordinates).</param>
        /// <returns></returns>
        protected bool GetNearestInterpolatedPointInternal(IList<DataPoint> points, ScreenPoint point, out DataPoint dpn, out ScreenPoint spn, out int index)
        {
            spn = default(ScreenPoint);
            dpn = default(DataPoint);
            index = -1;

            // http://local.wasp.uwa.edu.au/~pbourke/geometry/pointline/
            double minimumDistance = double.MaxValue;

            for (int i = 0; i + 1 < points.Count; i++)
            {
                var p1 = points[i];
                var p2 = points[i + 1];
                var sp1 = AxisBase.Transform(p1, XAxis, YAxis);
                var sp2 = AxisBase.Transform(p2, XAxis, YAxis);

                double sp21X = sp2.x - sp1.x;
                double sp21Y = sp2.y - sp1.y;
                double u1 = (point.x - sp1.x) * sp21X + (point.y - sp1.y) * sp21Y;
                double u2 = sp21X * sp21X + sp21Y * sp21Y;
                double ds = sp21X * sp21X + sp21Y * sp21Y;

                if (ds < 4)
                {
                    // if the points are very close, we can get numerical problems, just use the first point...
                    u1 = 0; u2 = 1;
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
    }
}