// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoColorLineSeries.cs" company="OxyPlot">
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
//   The WPF wrapper for OxyPlot.TwoColorLineSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// The WPF wrapper for OxyPlot.TwoColorLineSeries.
    /// </summary>
    public class TwoColorLineSeries : LineSeries
    {
        /// <summary>
        /// The color 2 property.
        /// </summary>
        public static readonly DependencyProperty Color2Property = DependencyProperty.Register(
            "Color2", typeof(Color), typeof(TwoColorLineSeries), new UIPropertyMetadata(Colors.Blue, AppearanceChanged));

        /// <summary>
        /// The limit property.
        /// </summary>
        public static readonly DependencyProperty LimitProperty = DependencyProperty.Register(
            "Limit", typeof(double), typeof(TwoColorLineSeries), new UIPropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// The line style 2 property.
        /// </summary>
        public static readonly DependencyProperty LineStyle2Property = DependencyProperty.Register(
            "LineStyle2",
            typeof(LineStyle),
            typeof(TwoColorLineSeries),
            new UIPropertyMetadata(LineStyle.Solid, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "TwoColorLineSeries" /> class.
        /// </summary>
        public TwoColorLineSeries()
        {
            this.InternalSeries = new OxyPlot.Series.TwoColorLineSeries();
        }

        /// <summary>
        /// Gets or sets Color2.
        /// </summary>
        public Color Color2
        {
            get
            {
                return (Color)this.GetValue(Color2Property);
            }

            set
            {
                this.SetValue(Color2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Limit.
        /// </summary>
        public double Limit
        {
            get
            {
                return (double)this.GetValue(LimitProperty);
            }

            set
            {
                this.SetValue(LimitProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineStyle2.
        /// </summary>
        public LineStyle LineStyle2
        {
            get
            {
                return (LineStyle)this.GetValue(LineStyle2Property);
            }

            set
            {
                this.SetValue(LineStyle2Property, value);
            }
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
            var s = series as OxyPlot.Series.TwoColorLineSeries;
            s.Limit = this.Limit;
            s.Color2 = this.Color2.ToOxyColor();
        }

    }
}