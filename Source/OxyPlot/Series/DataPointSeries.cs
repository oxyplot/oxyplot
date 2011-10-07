//-----------------------------------------------------------------------
// <copyright file="DataPointSeries.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// DataPointProvider interface.
    /// </summary>
    public interface IDataPointProvider
    {
        #region Public Methods

        /// <summary>
        /// Gets the data point.
        /// </summary>
        /// <returns>
        /// </returns>
        DataPoint GetDataPoint();

        #endregion
    }

    /// <summary>
    /// Base class for series that contain a collection of IDataPoints.
    /// </summary>
    public abstract class DataPointSeries : ItemsSeries
    {
        #region Constants and Fields

        /// <summary>
        ///   The points list.
        /// </summary>
        protected IList<IDataPoint> points = new List<IDataPoint>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DataPointSeries" /> class.
        /// </summary>
        protected DataPointSeries()
        {
            this.DataFieldX = "X";
            this.DataFieldY = "Y";
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether CanTrackerInterpolatePoints.
        /// </summary>
        public bool CanTrackerInterpolatePoints { get; set; }

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

        /// <summary>
        ///   Gets or sets the points list.
        /// </summary>
        /// <value>The points list.</value>
        public IList<IDataPoint> Points
        {
            get
            {
                return this.points;
            }

            set
            {
                this.points = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "DataPointSeries" /> is smooth.
        /// </summary>
        /// <value><c>true</c> if smooth; otherwise, <c>false</c>.</value>
        public bool Smooth { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the point in the dataset that is nearest the specified point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="interpolate">
        /// The interpolate.
        /// </param>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (interpolate && !this.CanTrackerInterpolatePoints)
            {
                return null;
            }

            int index;
            TrackerHitResult result = null;
            DataPoint dpn;
            ScreenPoint spn;
            if (interpolate)
            {
                if (this.GetNearestInterpolatedPointInternal(this.Points, point, out dpn, out spn, out index))
                {
                    object item = this.GetItem(this.ItemsSource, index);
                    return new TrackerHitResult(this, dpn, spn, item);
                }
            }
            else
            {
                if (this.GetNearestPointInternal(this.Points, point, out dpn, out spn, out index))
                {
                    object item = this.GetItem(this.ItemsSource, index);
                    return new TrackerHitResult(this, dpn, spn, item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the value from the specified X.
        /// </summary>
        /// <param name="x">
        /// The x coordinate.
        /// </param>
        /// <returns>
        /// </returns>
        public double? GetValueFromX(double x)
        {
            int n = this.Points.Count;

            for (int i = 0; i + 1 < n; i++)
            {
                if (IsBetween(x, this.Points[i].X, this.Points[i + 1].Y))
                {
                    return this.Points[i].Y
                           +
                           (this.Points[i + 1].Y - this.Points[i].Y) / (this.Points[i + 1].X - this.Points[i].X)
                           * (x - this.Points[i].X);
                }
            }

            return null;
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
        public virtual bool IsValidPoint(IDataPoint pt, IAxis xAxis, IAxis yAxis)
        {
            return !double.IsNaN(pt.X) && !double.IsInfinity(pt.X) && !double.IsNaN(pt.Y) && !double.IsInfinity(pt.Y)
                   && (xAxis != null && xAxis.IsValidValue(pt.X)) && (yAxis != null && yAxis.IsValidValue(pt.Y));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the axes to include the max and min of this series.
        /// </summary>
        protected internal override void UpdateAxisMaxMin()
        {
            this.XAxis.Include(this.MinX);
            this.XAxis.Include(this.MaxX);
            this.YAxis.Include(this.MinY);
            this.YAxis.Include(this.MaxY);
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

            this.AddDataPoints(this.points);
        }

        /// <summary>
        /// Updates the max/min from the datapoints.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            this.InternalUpdateMaxMin(this.Points);
        }

        /// <summary>
        /// The add data points.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        protected void AddDataPoints(IList<IDataPoint> points)
        {
            points.Clear();

            // Use the mapping to generate the points
            if (this.Mapping != null)
            {
                foreach (object item in this.ItemsSource)
                {
                    points.Add(this.Mapping(item));
                }

                return;
            }

            // Get DataPoints from the items in ItemsSource 
            // if they implement IDataPointProvider
            // If DataFields are set, this is not used
            if (this.DataFieldX == null || this.DataFieldY == null)
            {
                foreach (object item in this.ItemsSource)
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
            this.AddDataPoints(points, this.ItemsSource, this.DataFieldX, this.DataFieldY);
        }

        /// <summary>
        /// The add data points.
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
        /// <exception cref="InvalidOperationException">
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        protected void AddDataPoints(
            IList<IDataPoint> dest, IEnumerable itemsSource, string dataFieldX, string dataFieldY)
        {
            PropertyInfo pix = null;
            PropertyInfo piy = null;
            Type t = null;

            foreach (object o in itemsSource)
            {
                if (pix == null || o.GetType() != t)
                {
                    t = o.GetType();
                    pix = t.GetProperty(dataFieldX);
                    piy = t.GetProperty(dataFieldY);
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

                var pp = new DataPoint(x, y);
                dest.Add(pp);
            }
        }

        /// <summary>
        /// Updates the Max/Min limits from the specified point list.
        /// </summary>
        /// <param name="pts">
        /// The points.
        /// </param>
        protected void InternalUpdateMaxMin(IList<IDataPoint> pts)
        {
            if (pts == null || pts.Count == 0)
            {
                return;
            }

            double minx = this.MinX;
            double miny = this.MinY;
            double maxx = this.MaxX;
            double maxy = this.MaxY;

            foreach (IDataPoint pt in pts)
            {
                if (!this.IsValidPoint(pt, this.XAxis, this.YAxis))
                {
                    continue;
                }

                double x = pt.X;
                double y = pt.Y;
                if (x < minx || double.IsNaN(minx))
                {
                    minx = x;
                }

                if (x > maxx || double.IsNaN(maxx))
                {
                    maxx = x;
                }

                if (y < miny || double.IsNaN(miny))
                {
                    miny = y;
                }

                if (y > maxy || double.IsNaN(maxy))
                {
                    maxy = y;
                }
            }

            this.MinX = minx;
            this.MinY = miny;
            this.MaxX = maxx;
            this.MaxY = maxy;
        }

        /// <summary>
        /// The is between.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="x0">
        /// The x 0.
        /// </param>
        /// <param name="x1">
        /// The x 1.
        /// </param>
        /// <returns>
        /// The is between.
        /// </returns>
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

        #endregion
    }
}
