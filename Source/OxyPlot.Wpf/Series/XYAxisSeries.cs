// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XYAxisSeries.cs" company="OxyPlot">
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
//   Abstract base class for series that use X and Y axes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Collections;
    using System.Windows;

    /// <summary>
    /// Abstract base class for series that use X and Y axes.
    /// </summary>
    public abstract class XYAxisSeries : ItemsSeries
    {
        /// <summary>
        /// The x axis key property.
        /// </summary>
        public static readonly DependencyProperty XAxisKeyProperty = DependencyProperty.Register(
            "XAxisKey", typeof(string), typeof(XYAxisSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The y axis key property.
        /// </summary>
        public static readonly DependencyProperty YAxisKeyProperty = DependencyProperty.Register(
            "YAxisKey", typeof(string), typeof(XYAxisSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Gets or sets the x-axis key.
        /// </summary>
        public string XAxisKey
        {
            get
            {
                return (string)this.GetValue(XAxisKeyProperty);
            }

            set
            {
                this.SetValue(XAxisKeyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the y axis key.
        /// </summary>
        public string YAxisKey
        {
            get
            {
                return (string)this.GetValue(YAxisKeyProperty);
            }

            set
            {
                this.SetValue(YAxisKeyProperty, value);
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
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.XYAxisSeries)series;
            s.XAxisKey = this.XAxisKey;
            s.YAxisKey = this.YAxisKey;
        }
    }
}