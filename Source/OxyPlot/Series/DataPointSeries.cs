using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace OxyPlot
{
    /// <summary>
    ///   DataPointProvider interface.
    /// </summary>
    public interface IDataPointProvider
    {
        /// <summary>
        ///   Gets the data point.
        /// </summary>
        /// <returns></returns>
        DataPoint GetDataPoint();
    }

    /// <summary>
    /// Base class for series that contain a collection of IDataPoints.
    /// </summary>
    public abstract class DataPointSeries : PlotSeriesBase
    {
        protected DataPointSeries()
        {
            DataFieldX = "X";
            DataFieldY = "Y";
        }

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
        ///   Gets or sets the mapping delegate.
        ///   Example: series1.Mapping = item => new DataPoint(((MyType)item).Time,((MyType)item).Value);
        /// </summary>
        /// <value>The mapping.</value>
        public Func<object, IDataPoint> Mapping { get; set; }

        protected IList<IDataPoint> points = new List<IDataPoint>();

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>The points.</value>
        public IList<IDataPoint> Points
        {
            get
            {
                return this.points;
            }
        }

        public bool CanTrackerInterpolatePoints { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "DataPointSeries" /> is smooth.
        /// </summary>
        /// <value><c>true</c> if smooth; otherwise, <c>false</c>.</value>
        public bool Smooth { get; set; }

        #region ISeries Members

        public override void UpdateData()
        {
            if (ItemsSource == null)
            {
                return;
            }
            this.AddDataPoints(points);
        }

        protected void AddDataPoints( IList<IDataPoint> points)
        {
            points.Clear();

            // Use the mapping to generate the points
            if (Mapping != null)
            {
                foreach (var item in ItemsSource)
                {
                    points.Add(Mapping(item));
                }
                return ;
            }

            // Get DataPoints from the items in ItemsSource 
            // if they implement IDataPointProvider
            // If DataFields are set, this is not used
            if (DataFieldX == null || DataFieldY == null)
            {
                foreach (var item in ItemsSource)
                {
                    var dp = item as IDataPoint;
                    if (dp != null)
                    {
                        points.Add(dp);
                        continue;
                    }

                    var idpp = item as IDataPointProvider;
                    if (idpp == null)
                    {
                        continue;
                    }

                    points.Add(idpp.GetDataPoint());
                }
            }

            // TODO: is there a better way to do this?
            // http://msdn.microsoft.com/en-us/library/bb613546.aspx

            // Using reflection on DataFieldX and DataFieldY
            AddDataPoints(points, ItemsSource, DataFieldX, DataFieldY);
        }

        /// <summary>
        ///   Updates the max/min from the datapoints.
        /// </summary>
        public override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            InternalUpdateMaxMin(Points);
        }

        /// <summary>
        ///   Gets the point in the dataset that is nearest the specified point.
        /// </summary>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (interpolate && !CanTrackerInterpolatePoints)
                return null;
            int index;
            TrackerHitResult result = null;
            DataPoint dpn;
            ScreenPoint spn;
            if (interpolate)
            {
                if (GetNearestInterpolatedPointInternal(Points, point, out dpn, out spn, out index))
                {
                    var item = GetItem(ItemsSource, index);
                    return new TrackerHitResult(this, dpn, spn, item);
                }
            }
            else
            {
                if (GetNearestPointInternal(Points, point, out dpn, out spn, out index))
                {
                    var item = GetItem(ItemsSource, index);
                    return new TrackerHitResult(this, dpn, spn, item);
                }
            }
            return result;
        }

       



        #endregion

        protected void AddDataPoints(IList<IDataPoint> dest, IEnumerable itemsSource, string dataFieldX, string dataFieldY)
        {
            PropertyInfo pix = null;
            PropertyInfo piy = null;
            Type t = null;

            foreach (var o in itemsSource)
            {
                if (pix == null || o.GetType() != t)
                {
                    t = o.GetType();
                    pix = t.GetProperty(dataFieldX);
                    piy = t.GetProperty(dataFieldY);
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

                var x = ToDouble(pix.GetValue(o, null));
                var y = ToDouble(piy.GetValue(o, null));


                var pp = new DataPoint(x, y);
                dest.Add(pp);
            }
        }


        /// <summary>
        /// Updates the Max/Min limits from the specified point list.
        /// </summary>
        /// <param name="pts">The points.</param>
        protected void InternalUpdateMaxMin(IList<IDataPoint> pts)
        {
            if (pts == null || pts.Count == 0)
            {
                return;
            }

            double minx = MinX;
            double miny = MinY;
            double maxx = MaxX;
            double maxy = MaxY;

            foreach (var pt in pts)
            {
                if (!IsValidPoint(pt, XAxis, YAxis))
                    continue;
                double x = pt.X;
                double y = pt.Y;
                if (x < minx || double.IsNaN(minx)) minx = x;
                if (x > maxx || double.IsNaN(maxx)) maxx = x;
                if (y < miny || double.IsNaN(miny)) miny = y;
                if (y > maxy || double.IsNaN(maxy)) maxy = y;
            }

            MinX = minx;
            MinY = miny;
            MaxX = maxx;
            MaxY = maxy;

            XAxis.Include(MinX);
            XAxis.Include(MaxX);
            YAxis.Include(MinY);
            YAxis.Include(MaxY);
        }

        /// <summary>
        ///   Gets the value from the specified X.
        /// </summary>
        /// <param name = "x">The x coordinate.</param>
        /// <returns></returns>
        public double? GetValueFromX(double x)
        {
            int n = Points.Count;

            for (int i = 0; i + 1 < n; i++)
            {
                if (IsBetween(x, Points[i].X, Points[i + 1].Y))
                {
                    return Points[i].Y +
                           (Points[i + 1].Y - Points[i].Y) /
                           (Points[i + 1].X - Points[i].X) * (x - Points[i].X);
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

        /// <summary>
        /// Determines whether the specified point is valid.
        /// </summary>
        /// <param name="pt">The pointt.</param>
        /// <param name="xAxis">The x axis.</param>
        /// <param name="yAxis">The y axis.</param>
        /// <returns>
        ///   <c>true</c> if the point is valid; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsValidPoint(IDataPoint pt, IAxis xAxis, IAxis yAxis)
        {
            return !double.IsNaN(pt.X) && !double.IsInfinity(pt.X)
                   && !double.IsNaN(pt.Y) && !double.IsInfinity(pt.Y)
                   && (xAxis != null && xAxis.IsValidValue(pt.X))
                   && (yAxis != null && yAxis.IsValidValue(pt.Y));
        }
    }
}