// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPointSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for series that contain a collection of <see cref="DataPoint" />s.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides an abstract base class for series that contain a collection of <see cref="DataPoint" />s.
    /// </summary>
    public abstract class DataPointSeries : XYAxisSeries
    {
        /// <summary>
        /// The list of data points.
        /// </summary>
        private readonly List<DataPoint> points = new List<DataPoint>();

        /// <summary>
        /// The data points from the items source.
        /// </summary>
        private List<DataPoint> itemsSourcePoints;

        /// <summary>
        /// Specifies if the itemsSourcePoints list can be modified.
        /// </summary>
        private bool ownsItemsSourcePoints;

        /// <summary>
        /// Gets or sets a value indicating whether the tracker can interpolate points.
        /// </summary>
        public bool CanTrackerInterpolatePoints { get; set; }

        /// <summary>
        /// Gets or sets the data field X. The default is <c>null</c>.
        /// </summary>
        /// <value>The data field X.</value>
        public string DataFieldX { get; set; }

        /// <summary>
        /// Gets or sets the data field Y. The default is <c>null</c>.
        /// </summary>
        /// <value>The data field Y.</value>
        public string DataFieldY { get; set; }

        /// <summary>
        /// Gets or sets the delegate used to map from <see cref="ItemsSeries.ItemsSource" /> to the <see cref="ActualPoints" />. The default is <c>null</c>.
        /// </summary>
        /// <value>The mapping.</value>
        /// <remarks>Example: series1.Mapping = item => new DataPoint(((MyType)item).Time,((MyType)item).Value);</remarks>
        public Func<object, DataPoint> Mapping { get; set; }

        /// <summary>
        /// Gets the list of points.
        /// </summary>
        /// <value>A list of <see cref="DataPoint" />.</value>
        public List<DataPoint> Points
        {
            get
            {
                return this.points;
            }
        }

        /// <summary>
        /// Gets the list of points that should be rendered.
        /// </summary>
        /// <value>A list of <see cref="DataPoint" />.</value>
        protected List<DataPoint> ActualPoints
        {
            get
            {
                return this.ItemsSource != null ? this.itemsSourcePoints : this.points;
            }
        }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            if (interpolate && !this.CanTrackerInterpolatePoints)
            {
                return null;
            }

            TrackerHitResult result = null;
            if (interpolate)
            {
                result = this.GetNearestInterpolatedPointInternal(this.ActualPoints, point);
            }

            if (result == null)
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
        /// Updates the data.
        /// </summary>
        protected internal override void UpdateData()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            this.UpdateItemsSourcePoints();
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            base.UpdateMaxMin();
            this.InternalUpdateMaxMin(this.ActualPoints);
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="i">The index of the item.</param>
        /// <returns>The item of the index.</returns>
        protected override object GetItem(int i)
        {
            var actualPoints = this.ActualPoints;
            if (this.ItemsSource == null && actualPoints != null && i < actualPoints.Count)
            {
                return actualPoints[i];
            }

            return base.GetItem(i);
        }

        /// <summary>
        /// Clears or creates the <see cref="itemsSourcePoints"/> list.
        /// </summary>
        private void ClearItemsSourcePoints()
        {
            if (!this.ownsItemsSourcePoints || this.itemsSourcePoints == null)
            {
                this.itemsSourcePoints = new List<DataPoint>();
            }
            else
            {
                this.itemsSourcePoints.Clear();
            }

            this.ownsItemsSourcePoints = true;
        }

        /// <summary>
        /// Updates the points from the <see cref="ItemsSeries.ItemsSource" />.
        /// </summary>
        private void UpdateItemsSourcePoints()
        {
            // Use the Mapping property to generate the points
            if (this.Mapping != null)
            {
                this.ClearItemsSourcePoints();
                foreach (var item in this.ItemsSource)
                {
                    this.itemsSourcePoints.Add(this.Mapping(item));
                }

                return;
            }

            var sourceAsListOfDataPoints = this.ItemsSource as List<DataPoint>;
            if (sourceAsListOfDataPoints != null)
            {
                this.itemsSourcePoints = sourceAsListOfDataPoints;
                this.ownsItemsSourcePoints = false;
                return;
            }

            this.ClearItemsSourcePoints();

            var sourceAsEnumerableDataPoints = this.ItemsSource as IEnumerable<DataPoint>;
            if (sourceAsEnumerableDataPoints != null)
            {
                this.itemsSourcePoints.AddRange(sourceAsEnumerableDataPoints);
                return;
            }

            // Get DataPoints from the items in ItemsSource
            // if they implement IDataPointProvider
            // If DataFields are set, this is not used
            if (this.DataFieldX == null || this.DataFieldY == null)
            {
                foreach (var item in this.ItemsSource)
                {
                    if (item is DataPoint)
                    {
                        this.itemsSourcePoints.Add((DataPoint)item);
                        continue;
                    }

                    var idpp = item as IDataPointProvider;
                    if (idpp == null)
                    {
                        continue;
                    }

                    this.itemsSourcePoints.Add(idpp.GetDataPoint());
                }
            }
            else
            {
                var filler = new ListBuilder<DataPoint>();
                filler.Add(this.DataFieldX, double.NaN);
                filler.Add(this.DataFieldY, double.NaN);
                filler.Fill(this.itemsSourcePoints, this.ItemsSource, args => new DataPoint(Axes.Axis.ToDouble(args[0]), Axes.Axis.ToDouble(args[1])));
            }
        }
    }
}