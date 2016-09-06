// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRectSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for series that contain a collection of <see cref="DataRect" />s.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides an abstract base class for series that contain a collection of <see cref="DataPoint" />s.
    /// </summary>
    public abstract class DataRectSeries : XYAxisSeries
    {
        /// <summary>
        /// The list of data points.
        /// </summary>
        private readonly List<DataRect> rects = new List<DataRect>();

        /// <summary>
        /// The data points from the items source.
        /// </summary>
        private List<DataRect> itemsSourceRects;

        /// <summary>
        /// Specifies if the itemsSourcePoints list can be modified.
        /// </summary>
        private bool ownsItemsSourcePoints;

        /// <summary>
        /// Gets or sets a value indicating whether the tracker can interpolate points.
        /// </summary>
        public bool CanTrackerInterpolatePoints { get; set; }

        /// <summary>
        /// Gets or sets the delegate used to map from <see cref="ItemsSeries.ItemsSource" /> to the <see cref="ActualRects" />. The default is <c>null</c>.
        /// </summary>
        /// <value>The mapping.</value>
        /// <remarks>Example: series1.Mapping = item => new DataRect(new DataPoint((MyType)item).Time1,((MyType)item).Value1), new DataPoint((MyType)item).Time2,((MyType)item).Value2));</remarks>
        public Func<object, DataRect> Mapping { get; set; }

        /// <summary>
        /// Gets the list of points.
        /// </summary>
        /// <value>A list of <see cref="DataPoint" />.</value>
        public List<DataRect> Rects
        {
            get
            {
                return this.rects;
            }
        }

        /// <summary>
        /// Gets the list of points that should be rendered.
        /// </summary>
        /// <value>A list of <see cref="DataPoint" />.</value>
        protected List<DataRect> ActualRects
        {
            get
            {
                return this.ItemsSource != null ? this.itemsSourceRects : this.rects;
            }
        }

        ///// <summary>
        ///// Gets the point on the series that is nearest the specified point.
        ///// </summary>
        ///// <param name="point">The point.</param>
        ///// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        ///// <returns>A TrackerHitResult for the current hit.</returns>
        //public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        //{
        //    if (interpolate && !this.CanTrackerInterpolatePoints)
        //    {
        //        return null;
        //    }

        //    TrackerHitResult result = null;
        //    if (interpolate)
        //    {
        //        result = this.GetNearestInterpolatedPointInternal(this.ActualRects, point);
        //    }

        //    if (result == null)
        //    {
        //        result = this.GetNearestPointInternal(this.ActualRects, point);
        //    }

        //    if (result != null)
        //    {
        //        result.Text = StringHelper.Format(
        //            this.ActualCulture,
        //            this.TrackerFormatString,
        //            result.Item,
        //            this.Title,
        //            this.XAxis.Title ?? XYAxisSeries.DefaultXAxisTitle,
        //            this.XAxis.GetValue(result.DataPoint.X),
        //            this.YAxis.Title ?? XYAxisSeries.DefaultYAxisTitle,
        //            this.YAxis.GetValue(result.DataPoint.Y));
        //    }

        //    return result;
        //}

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
            List<DataPoint> allDataPoints = new List<DataPoint>();
            allDataPoints.AddRange(this.ActualRects.Select(rect => rect.A));
            allDataPoints.AddRange(this.ActualRects.Select(rect => rect.B));
            this.InternalUpdateMaxMin(allDataPoints);
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="i">The index of the item.</param>
        /// <returns>The item of the index.</returns>
        protected override object GetItem(int i)
        {
            var actualPoints = this.ActualRects;
            if (this.ItemsSource == null && actualPoints != null && i < actualPoints.Count)
            {
                return actualPoints[i];
            }

            return base.GetItem(i);
        }

        /// <summary>
        /// Clears or creates the <see cref="itemsSourceRects"/> list.
        /// </summary>
        private void ClearItemsSourcePoints()
        {
            if (!this.ownsItemsSourcePoints || this.itemsSourceRects == null)
            {
                this.itemsSourceRects = new List<DataRect>();
            }
            else
            {
                this.itemsSourceRects.Clear();
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
                    this.itemsSourceRects.Add(this.Mapping(item));
                }

                return;
            }

            var sourceAsListOfDataRects = this.ItemsSource as List<DataRect>;
            if (sourceAsListOfDataRects != null)
            {
                this.itemsSourceRects = sourceAsListOfDataRects;
                this.ownsItemsSourcePoints = false;
                return;
            }

            this.ClearItemsSourcePoints();

            var sourceAsEnumerableDataRects = this.ItemsSource as IEnumerable<DataRect>;
            if (sourceAsEnumerableDataRects != null)
            {
                this.itemsSourceRects.AddRange(sourceAsEnumerableDataRects);
                return;
            }
        }
    }
}