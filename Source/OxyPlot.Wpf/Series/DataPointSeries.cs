// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPointSeries.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
//   Base class for data series
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System;
    using System.Collections;
    using System.Windows;

    /// <summary>
    /// Base class for data series
    /// </summary>
    public abstract class DataPointSeries : XYAxisSeries
    {
        /// <summary>
        /// The can tracker interpolate points property.
        /// </summary>
        public static readonly DependencyProperty CanTrackerInterpolatePointsProperty =
            DependencyProperty.Register(
                "CanTrackerInterpolatePoints",
                typeof(bool),
                typeof(DataPointSeries),
                new PropertyMetadata(false, AppearanceChanged));

        /// <summary>
        /// The data field x property.
        /// </summary>
        public static readonly DependencyProperty DataFieldXProperty = DependencyProperty.Register(
            "DataFieldX", typeof(string), typeof(DataPointSeries), new PropertyMetadata("X", DataChanged));

        /// <summary>
        /// The data field y property.
        /// </summary>
        public static readonly DependencyProperty DataFieldYProperty = DependencyProperty.Register(
            "DataFieldY", typeof(string), typeof(DataPointSeries), new PropertyMetadata("Y", DataChanged));

        /// <summary>
        /// The mapping property.
        /// </summary>
        public static readonly DependencyProperty MappingProperty = DependencyProperty.Register(
            "Mapping", typeof(Func<object, IDataPoint>), typeof(DataPointSeries), new UIPropertyMetadata(null));

        /// <summary>
        /// The smooth property.
        /// </summary>
        public static readonly DependencyProperty SmoothProperty = DependencyProperty.Register(
            "Smooth", typeof(bool), typeof(DataPointSeries), new UIPropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether the tracker can interpolate points.
        /// </summary>
        public bool CanTrackerInterpolatePoints
        {
            get
            {
                return (bool)this.GetValue(CanTrackerInterpolatePointsProperty);
            }

            set
            {
                this.SetValue(CanTrackerInterpolatePointsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldX.
        /// </summary>
        public string DataFieldX
        {
            get
            {
                return (string)this.GetValue(DataFieldXProperty);
            }

            set
            {
                this.SetValue(DataFieldXProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldY.
        /// </summary>
        public string DataFieldY
        {
            get
            {
                return (string)this.GetValue(DataFieldYProperty);
            }

            set
            {
                this.SetValue(DataFieldYProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the mapping.
        /// </summary>
        /// <value>
        /// The mapping.
        /// </value>
        public Func<object, IDataPoint> Mapping
        {
            get
            {
                return (Func<object, IDataPoint>)this.GetValue(MappingProperty);
            }

            set
            {
                this.SetValue(MappingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DataPointSeries"/> is smooth.
        /// </summary>
        /// <value>
        /// <c>true</c> if smooth; otherwise, <c>false</c> .
        /// </value>
        public bool Smooth
        {
            get
            {
                return (bool)this.GetValue(SmoothProperty);
            }

            set
            {
                this.SetValue(SmoothProperty, value);
            }
        }

        /// <summary>
        /// The on items source changed.
        /// </summary>
        /// <param name="oldValue">
        /// The old value.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            this.OnDataChanged();
        }

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        /// <param name="series">
        /// The series.
        /// </param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.DataPointSeries)series;
            s.ItemsSource = this.ItemsSource;
            s.DataFieldX = this.DataFieldX;
            s.DataFieldY = this.DataFieldY;
            s.CanTrackerInterpolatePoints = this.CanTrackerInterpolatePoints;
            s.Smooth = this.Smooth;
            s.Mapping = this.Mapping;
        }

    }
}