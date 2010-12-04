using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace OxyPlot
{
    public interface IDataPointProvider
    {
        DataPoint GetDataPoint();
    }

    public abstract class DataSeries : ISeries
    {
        internal Collection<DataPoint> InternalPoints;

        protected DataSeries()
        {
            InternalPoints = new Collection<DataPoint>();
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

        /// <summary>
        ///   Gets or sets the points.
        /// </summary>
        /// <value>The points.</value>
        [Browsable(false)]
        public Collection<DataPoint> Points
        {
            get { return InternalPoints; }
            set { InternalPoints = value; }
        }

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
        ///   Gets or sets a value indicating whether this <see cref = "DataSeries" /> is smooth.
        /// </summary>
        /// <value><c>true</c> if smooth; otherwise, <c>false</c>.</value>
        public bool Smooth { get; set; }

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

        #endregion

        public virtual void UpdateData()
        {
            if (ItemsSource == null)
            {
                return;
            }

            InternalPoints.Clear();

            // Get DataPoints from the items in ItemsSource 
            // if they implement IDataPointProvider
            // If DataFields are set, this is not used
            if (DataFieldX == null || DataFieldY == null)
            {
                foreach (var item in ItemsSource)
                {
                    var idpp = item as IDataPointProvider;
                    if (idpp == null)
                    {
                        continue;
                    }

                    InternalPoints.Add(idpp.GetDataPoint());
                }

                return;
            }

            // TODO: is there a better way to do this?
            // http://msdn.microsoft.com/en-us/library/bb613546.aspx

            // Using reflection on DataFieldX and DataFieldY
            PropertyInfo pix = null;
            PropertyInfo piy = null;
            Type t = null;

            foreach (var o in ItemsSource)
            {
                if (pix == null || o.GetType() != t)
                {
                    t = o.GetType();
                    pix = t.GetProperty(DataFieldX);
                    piy = t.GetProperty(DataFieldY);
                    if (pix == null)
                    {
                        throw new InvalidOperationException(string.Format("Could not find data field {0} on type {1}",
                                                                          DataFieldX, t));
                    }

                    if (piy == null)
                    {
                        throw new InvalidOperationException(string.Format("Could not find data field {0} on type {1}",
                                                                          DataFieldY, t));
                    }
                }

                var x = (double)pix.GetValue(o, null);
                var y = (double)piy.GetValue(o, null);


                var pp = new DataPoint(x, y);
                InternalPoints.Add(pp);
            }
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
        ///   Updates the max/min from the datapoints.
        /// </summary>
        public virtual void UpdateMaxMin()
        {
            MinX = MinY = MaxX = MaxY = double.NaN;

            if (InternalPoints == null || InternalPoints.Count == 0)
            {
                return;
            }

            MinX = MaxX = InternalPoints[0].x;
            MinY = MaxY = InternalPoints[0].y;
            foreach (var pt in InternalPoints)
            {
                MinX = Math.Min(MinX, pt.x);
                MaxX = Math.Max(MaxX, pt.x);

                MinY = Math.Min(MinY, pt.y);
                MaxY = Math.Max(MaxY, pt.y);
            }

            XAxis.Include(MinX);
            XAxis.Include(MaxX);
            YAxis.Include(MinY);
            YAxis.Include(MaxY);
        }

        /// <summary>
        ///   Gets the point in the dataset that is nearest the specified point.
        /// </summary>
        /// <param name = "point">The point.</param>
        /// <param name="dpn">The nearest point (data coordinates).</param>
        /// <param name="spn">The nearest point (screen coordinates).</param>
        /// <returns></returns>
        public bool GetNearestPoint(ScreenPoint point, out DataPoint dpn, out ScreenPoint spn)
        {
            var dp = AxisBase.InverseTransform(point, XAxis, YAxis);

            double mindist = double.MaxValue;
            DataPoint? nearest = null;
            foreach (var p in InternalPoints)
            {
                double dx = dp.x - p.x;
                double dy = dp.y - p.y;
                double d2 = dx * dx + dy * dy;

                if (d2 < mindist)
                {
                    nearest = p;
                    mindist = d2;
                }
            }
            if (nearest != null)
            {
                dpn = nearest.Value;
                spn = XAxis.Transform(dpn, YAxis);
                return true;
            }
            spn = default(ScreenPoint);
            dpn = default(DataPoint);
            return false;
        }

        /// <summary>
        ///   Gets the point on the curve that is nearest the specified point.
        /// </summary>
        /// <param name = "point">The point.</param>
        /// <param name="dpn">The nearest point (data coordinates).</param>
        /// <param name="spn">The nearest point (screen coordinates).</param>
        /// <returns></returns>
        public bool GetNearestInterpolatedPoint(ScreenPoint point, out DataPoint dpn, out ScreenPoint spn)
        {
            var p3 = AxisBase.InverseTransform(point, XAxis, YAxis);

            // http://local.wasp.uwa.edu.au/~pbourke/geometry/pointline/
            double mindist = double.MaxValue;
            DataPoint? pt = null;
            for (int i = 0; i + 1 < InternalPoints.Count; i++)
            {
                var p1 = InternalPoints[i];
                var p2 = InternalPoints[i + 1];

                double p21X = p2.x - p1.x;
                double p21Y = p2.y - p1.y;
                double u1 = (p3.x - p1.x) * p21X + (p3.y - p1.y) * p21Y;
                double u2 = p21X * p21X + p21Y * p21Y;
                if (u2 == 0)
                {
                    continue; // P1 && P2 coincident
                }

                double u = u1 / u2;
                if (u < 0 || u > 1)
                {
                    continue; // outside line
                }

                double x = p1.x + u * p21X;
                double y = p1.y + u * p21Y;

                double dx = p3.x - x;
                double dy = p3.y - y;
                double d2 = dx * dx + dy * dy;

                if (d2 < mindist)
                {
                    pt = new DataPoint(x, y);
                    mindist = d2;
                }
            }
            if (pt != null)
            {
                dpn = pt.Value;
                spn = AxisBase.Transform(pt.Value, XAxis, YAxis);
                return true;
            }
            spn = default(ScreenPoint);
            dpn = default(DataPoint);
            return false;
        }

        /// <summary>
        ///   Gets the value from the specified X.
        /// </summary>
        /// <param name = "x">The x.</param>
        /// <returns></returns>
        public double? GetValueFromX(double x)
        {
            for (int i = 0; i + 1 < InternalPoints.Count; i++)
            {
                if (IsBetween(x, InternalPoints[i].x, InternalPoints[i + 1].x))
                {
                    return InternalPoints[i].y +
                           (InternalPoints[i + 1].y - InternalPoints[i].y) / (InternalPoints[i + 1].x - InternalPoints[i].x) * (x - InternalPoints[i].x);
                }
            }

            return null;
        }

        private static bool IsBetween(double x, double x0, double x1)
        {
            if (x >= x0 && x <= x1)
            {
                return true;
            }

            if (x >= x1 && x <= x0)
            {
                return true;
            }

            return false;
        }
    }
}