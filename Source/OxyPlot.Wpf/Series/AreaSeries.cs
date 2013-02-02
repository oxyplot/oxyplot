// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AreaSeries.cs" company="OxyPlot">
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
//   This is a WPF wrapper of OxyPlot.AreaSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.AreaSeries
    /// </summary>
    public class AreaSeries : LineSeries
    {
        /// <summary>
        /// The constant y 2 property.
        /// </summary>
        public static readonly DependencyProperty ConstantY2Property = DependencyProperty.Register(
            "ConstantY2", typeof(double), typeof(AreaSeries), new PropertyMetadata(0.0, DataChanged));

        /// <summary>
        /// The data field x 2 property.
        /// </summary>
        public static readonly DependencyProperty DataFieldX2Property = DependencyProperty.Register(
            "DataFieldX2", typeof(string), typeof(AreaSeries), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// The data field y 2 property.
        /// </summary>
        public static readonly DependencyProperty DataFieldY2Property = DependencyProperty.Register(
            "DataFieldY2", typeof(string), typeof(AreaSeries), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// The fill property.
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof(Color?), typeof(AreaSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The reverse 2 property.
        /// </summary>
        public static readonly DependencyProperty Reverse2Property = DependencyProperty.Register(
            "Reverse2", typeof(bool), typeof(AreaSeries), new PropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "AreaSeries" /> class.
        /// </summary>
        public AreaSeries()
        {
            this.InternalSeries = new OxyPlot.Series.AreaSeries();
        }

        /// <summary>
        /// Gets or sets ConstantY2.
        /// </summary>
        public double ConstantY2
        {
            get
            {
                return (double)this.GetValue(ConstantY2Property);
            }

            set
            {
                this.SetValue(ConstantY2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldX2.
        /// </summary>
        public string DataFieldX2
        {
            get
            {
                return (string)this.GetValue(DataFieldX2Property);
            }

            set
            {
                this.SetValue(DataFieldX2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets DataFieldY2.
        /// </summary>
        public string DataFieldY2
        {
            get
            {
                return (string)this.GetValue(DataFieldY2Property);
            }

            set
            {
                this.SetValue(DataFieldY2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets Fill.
        /// </summary>
        public Color? Fill
        {
            get
            {
                return (Color?)this.GetValue(FillProperty);
            }

            set
            {
                this.SetValue(FillProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Reverse2.
        /// </summary>
        public bool Reverse2
        {
            get
            {
                return (bool)this.GetValue(Reverse2Property);
            }

            set
            {
                this.SetValue(Reverse2Property, value);
            }
        }

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// </returns>
        public override OxyPlot.Series.Series CreateModel()
        {
            this.SynchronizeProperties(this.InternalSeries);
            return this.InternalSeries;
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
            var s = series as OxyPlot.Series.AreaSeries;
            s.DataFieldX2 = this.DataFieldX2;
            s.DataFieldY2 = this.DataFieldY2;
            s.ConstantY2 = this.ConstantY2;
            s.Fill = this.Fill.ToOxyColor();
            s.Reverse2 = this.Reverse2;
        }

    }
}