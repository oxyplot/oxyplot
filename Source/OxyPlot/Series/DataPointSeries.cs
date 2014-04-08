// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPointSeries.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Provides an abstract base class for series that contain a collection of <see cref="DataPoint"/>s.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

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
        /// Initializes a new instance of the <see cref = "DataPointSeries" /> class.
        /// </summary>
        protected DataPointSeries()
        {
            this.DataFieldX = null;
            this.DataFieldY = null;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tracker can interpolate points.
        /// </summary>
        public bool CanTrackerInterpolatePoints { get; set; }

        /// <summary>
        /// Gets or sets the data field X.
        /// </summary>
        /// <value>The data field X.</value>
        public string DataFieldX { get; set; }

        /// <summary>
        /// Gets or sets the data field Y.
        /// </summary>
        /// <value>The data field Y.</value>
        public string DataFieldY { get; set; }

        /// <summary>
        /// Gets or sets the mapping delegate.
        /// Example: series1.Mapping = item => new DataPoint(((MyType)item).Time,((MyType)item).Value);
        /// </summary>
        /// <value>The mapping.</value>
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
        /// Gets or sets a value indicating whether this <see cref = "DataPointSeries" /> is smooth.
        /// </summary>
        /// <value><c>true</c> if smooth; otherwise, <c>false</c>.</value>
        public bool Smooth { get; set; }

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

            if (interpolate)
            {
                return this.GetNearestInterpolatedPointInternal(this.ActualPoints, point);
            }

            return this.GetNearestPointInternal(this.ActualPoints, point);
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
        /// Updates the max/min from the data points.
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
        /// Adds data points from the specified source to the specified destination.
        /// </summary>
        /// <param name="target">The destination list.</param>
        /// <param name="itemsSource">The source.</param>
        /// <param name="dataFieldX">The x-coordinate data field.</param>
        /// <param name="dataFieldY">The y-coordinate data field.</param>
        protected void AddDataPoints(List<DataPoint> target, IEnumerable itemsSource, string dataFieldX, string dataFieldY)
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
                target.Add(pp);
            }

            ////var filler = new ListFiller<DataPoint>();
            ////filler.Add(dataFieldX, (item, value) => item.X = this.ToDouble(value));
            ////filler.Add(dataFieldY, (item, value) => item.Y = this.ToDouble(value));
            ////filler.Fill(target, itemsSource);
        }

        /// <summary>
        /// Updates the Max/Min limits from the specified point list.
        /// </summary>
        /// <param name="pts">The points.</param>
        protected void InternalUpdateMaxMin(List<DataPoint> pts)
        {
            if (pts == null || pts.Count == 0)
            {
                return;
            }

            double minx = double.MaxValue;
            double miny = double.MaxValue;
            double maxx = double.MinValue;
            double maxy = double.MinValue;

            if (double.IsNaN(minx))
            {
                minx = double.MaxValue;
            }

            if (double.IsNaN(miny))
            {
                miny = double.MaxValue;
            }

            if (double.IsNaN(maxx))
            {
                maxx = double.MinValue;
            }

            if (double.IsNaN(maxy))
            {
                maxy = double.MinValue;
            }

            foreach (var pt in pts)
            {
                double x = pt.X;
                double y = pt.Y;

                // Check if the point is defined (the code below is faster than double.IsNaN)
                // ReSharper disable EqualExpressionComparison
                // ReSharper disable CompareOfFloatsByEqualityOperator
#pragma warning disable 1718
                if (x != x || y != y)
                // ReSharper restore CompareOfFloatsByEqualityOperator
                // ReSharper restore EqualExpressionComparison
#pragma warning restore 1718
                {
                    continue;
                }

                if (x < minx)
                {
                    minx = x;
                }

                if (x > maxx)
                {
                    maxx = x;
                }

                if (y < miny)
                {
                    miny = y;
                }

                if (y > maxy)
                {
                    maxy = y;
                }
            }

            if (minx < double.MaxValue)
            {
                this.MinX = minx;
            }

            if (miny < double.MaxValue)
            {
                this.MinY = miny;
            }

            if (maxx > double.MinValue)
            {
                this.MaxX = maxx;
            }

            if (maxy > double.MinValue)
            {
                this.MaxY = maxy;
            }
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
            // Use the mapping to generate the points
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
                // TODO: is there a better way to do this?
                // http://msdn.microsoft.com/en-us/library/bb613546.aspx

                // Using reflection on DataFieldX and DataFieldY
                this.AddDataPoints(this.itemsSourcePoints, this.ItemsSource, this.DataFieldX, this.DataFieldY);
            }
        }
    }
}