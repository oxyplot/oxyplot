// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterErrorSeries.cs" company="OxyPlot">
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
//   This is a WPF wrapper of OxyPlot.ScatterErrorSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.ScatterErrorSeries
    /// </summary>
    public class ScatterErrorSeries : ScatterSeries
    {
        /// <summary>
        /// Identifies the <see cref="DataFieldError"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataFieldErrorProperty =
            DependencyProperty.Register("DataFieldError", typeof(string), typeof(ScatterErrorSeries), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="ErrorBarColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ErrorBarColorProperty =
            DependencyProperty.Register("ErrorBarColor", typeof(Color), typeof(ScatterErrorSeries), new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ErrorBarStopWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ErrorBarStopWidthProperty =
            DependencyProperty.Register("ErrorBarStopWidth", typeof(double), typeof(ScatterErrorSeries), new PropertyMetadata(4.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ErrorBarStrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ErrorBarStrokeThicknessProperty =
            DependencyProperty.Register("ErrorBarStrokeThickness", typeof(double), typeof(ScatterErrorSeries), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="AlwaysShowErrorBars"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlwaysShowErrorBarsProperty =
            DependencyProperty.Register("AlwaysShowErrorBars", typeof(bool), typeof(ScatterErrorSeries), new PropertyMetadata(false, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterErrorSeries"/> class.
        /// </summary>
        public ScatterErrorSeries()
        {
            this.InternalSeries = new OxyPlot.Series.ScatterErrorSeries();
        }

        /// <summary>
        /// Gets or sets the data field error.
        /// </summary>
        /// <value>
        /// The data field error.
        /// </value>
        public string DataFieldError
        {
            get { return (string)this.GetValue(DataFieldErrorProperty); }
            set { this.SetValue(DataFieldErrorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the error bar.
        /// </summary>
        /// <value>
        /// The color of the error bar.
        /// </value>
        public Color ErrorBarColor
        {
            get { return (Color)this.GetValue(ErrorBarColorProperty); }
            set { this.SetValue(ErrorBarColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the error bar stop.
        /// </summary>
        /// <value>
        /// The width of the error bar stop.
        /// </value>
        public double ErrorBarStopWidth
        {
            get { return (double)this.GetValue(ErrorBarStopWidthProperty); }
            set { this.SetValue(ErrorBarStopWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the error bar stroke thickness.
        /// </summary>
        /// <value>
        /// The error bar stroke thickness.
        /// </value>
        public double ErrorBarStrokeThickness
        {
            get { return (double)this.GetValue(ErrorBarStrokeThicknessProperty); }
            set { this.SetValue(ErrorBarStrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the error bars should always be displayed, regardless of their size. <br />
        /// By default, the error bars are only displayed when they are bigger than 1.5x the cursor size.
        /// </summary>
        /// <value>
        /// <c>true</c> if the error bars should always be displayed; otherwise, <c>false</c>.
        /// </value>
        public bool AlwaysShowErrorBars
        {
            get { return (bool)this.GetValue(AlwaysShowErrorBarsProperty); }
            set { this.SetValue(AlwaysShowErrorBarsProperty, value); }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.ScatterErrorSeries)series;
            s.DataFieldError = this.DataFieldError;
            s.ErrorBarColor = this.ErrorBarColor.ToOxyColor();
            s.ErrorBarStopWidth = this.ErrorBarStopWidth;
            s.ErrorBarStrokeThickness = this.ErrorBarStrokeThickness;
            s.AlwaysShowErrorBars = this.AlwaysShowErrorBars;
        }
    }
}
