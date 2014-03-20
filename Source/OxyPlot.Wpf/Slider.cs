// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Slider.cs" company="OxyPlot">
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
//   Shows a slider with horizontal and vertical lines, and a templated content control.
//   The ContentTemplate must be set to define how the slider label looks.
//   The Plots contains a default template in the Themes/Generic.xaml file.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// Shows a slider with horizontal and vertical lines, and a templated content control.
    /// The ContentTemplate must be set to define how the slider label looks.
    /// The Plots contains a default template in the Themes/Generic.xaml file.
    /// </summary>
    public class Slider : Canvas
    {
        private readonly Line sliderLine1 = new Line();
        private readonly Line sliderLine2 = new Line();
        private readonly ContentControl content = new ContentControl();

        /// <summary>
        /// Gets or sets the slider label format.
        /// </summary>
        /// <value>The slider label format.</value>
        public string SliderLabelFormat { get; set; }

        /// <summary>
        /// Gets or sets the content template.
        /// </summary>
        /// <value>The content template.</value>
        public DataTemplate ContentTemplate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Slider"/> class.
        /// </summary>
        /// <param name="pc">The parent Plot.</param>
        public Slider()
        {
            SliderLabelFormat = null; // "{0:0.###} {1:0.###}";

            Children.Add(sliderLine1);
            Children.Add(sliderLine2);
            Children.Add(content);

            sliderLine1.StrokeThickness = 1;
            sliderLine2.StrokeThickness = 1;
            sliderLine1.Stroke = new SolidColorBrush(Color.FromArgb(80, 0, 0, 0));
            sliderLine2.Stroke = sliderLine1.Stroke;

            sliderLine1.SnapsToDevicePixels = true;
            sliderLine2.SnapsToDevicePixels = true;
            sliderLine1.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            sliderLine2.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            Hide();
        }

        /// <summary>
        /// Sets the position of the slider.
        /// </summary>
        /// <param name="dp">The data point.</param>
        /// <param name="s">The series.</param>
        public void SetPosition(DataPoint dp, OxyPlot.DataSeries s)
        {
            var dp1 = new DataPoint(dp.X, s.YAxis.ActualMaximum);
            var dp2 = new DataPoint(dp.X, s.YAxis.ActualMinimum);
            var dp3 = new DataPoint(s.XAxis.ActualMinimum, dp.Y);
            var dp4 = new DataPoint(s.XAxis.ActualMaximum, dp.Y);

            var pt0 = AxisBase.Transform(dp, s.XAxis, s.YAxis);
            var pt1 = AxisBase.Transform(dp1, s.XAxis, s.YAxis);
            var pt2 = AxisBase.Transform(dp2, s.XAxis, s.YAxis);
            var pt3 = AxisBase.Transform(dp3, s.XAxis, s.YAxis);
            var pt4 = AxisBase.Transform(dp4, s.XAxis, s.YAxis);

            if (ContentTemplate != null)
            {
                content.Content = new SliderViewModel(s, dp, SliderLabelFormat);
                content.ContentTemplate = ContentTemplate;
                SetLeft(content, pt0.X);
                SetTop(content, pt0.Y);
            }

            sliderLine1.X1 = pt1.X;
            sliderLine1.Y1 = pt1.Y;
            sliderLine1.X2 = pt2.X;
            sliderLine1.Y2 = pt2.Y;

            sliderLine2.X1 = pt3.X;
            sliderLine2.Y1 = pt3.Y;
            sliderLine2.X2 = pt4.X;
            sliderLine2.Y2 = pt4.Y;

            Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hides the slider.
        /// </summary>
        public void Hide()
        {
            Visibility = Visibility.Hidden;
        }
    }

    public class SliderViewModel
    {
        /// <summary>
        /// Gets or sets the position of the slider (data coordinates).
        /// </summary>
        /// <value>The point.</value>
        public DataPoint Point { get; set; }

        /// <summary>
        /// Gets or sets the data series.
        /// </summary>
        /// <value>The series.</value>
        public OxyPlot.DataSeries Series { get; set; }

        /// <summary>
        /// Gets or sets the format string.
        /// {0}: {1:0.#####}, {2}: {3:0.#####}
        /// xAxisTitle: xValue, yAxisTitle: yValue
        /// </summary>
        /// <value>The format.</value>
        public string Format { get; set; }

        private const string DefaultFormat = "{0}: {1:0.#####}, {2}: {3:0.#####}";

        /// <summary>
        /// Initializes a new instance of the <see cref="SliderViewModel"/> class.
        /// </summary>
        /// <param name="series">The series.</param>
        /// <param name="point">The point.</param>
        /// <param name="fmt">The format string.</param>
        public SliderViewModel(OxyPlot.DataSeries series, DataPoint point, string fmt = null)
        {
            if (String.IsNullOrEmpty(fmt))
                fmt = DefaultFormat;

            Format = fmt;
            Series = series;
            Point = point;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var xTitle = Series.XAxis.Title;
            var yTitle = Series.YAxis.Title;
            if (String.IsNullOrEmpty(xTitle)) xTitle = "X";
            if (String.IsNullOrEmpty(yTitle)) yTitle = "Y";
            return String.Format(CultureInfo.InvariantCulture, Format, xTitle, Point.X, yTitle, Point.Y);
        }
    }
}