// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StairStepSeries.cs" company="OxyPlot">
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
//   This is a WPF wrapper of OxyPlot.StairStepSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.StairStepSeries
    /// </summary>
    public class StairStepSeries : LineSeries
    {
        /// <summary>
        /// Identifies the <see cref="VerticalStrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalStrokeThicknessProperty =
            DependencyProperty.Register("VerticalStrokeThickness", typeof(double), typeof(StairStepSeries), new UIPropertyMetadata(double.NaN, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="VerticalStrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalLineStyleProperty =
            DependencyProperty.Register("VerticalLineStyle", typeof(LineStyle), typeof(StairStepSeries), new UIPropertyMetadata(LineStyle.Undefined, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="StairStepSeries"/> class.
        /// </summary>
        public StairStepSeries()
        {
            this.InternalSeries = new OxyPlot.Series.StairStepSeries();
        }

        /// <summary>
        /// Gets or sets the stroke thickness of the vertical line segments.
        /// </summary>
        /// <value>The vertical stroke thickness.</value>
        /// <remarks>Set the value to NaN to use the StrokeThickness property for both horizontal and vertical segments.
        /// Using the VerticalStrokeThickness property will have a small performance hit.</remarks>
        public double VerticalStrokeThickness
        {
            get { return (double)this.GetValue(VerticalStrokeThicknessProperty); }
            set { this.SetValue(VerticalStrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the line style of the vertical line segments.
        /// </summary>
        /// <value>The vertical line style.</value>
        public LineStyle VerticalLineStyle
        {
            get { return (LineStyle)this.GetValue(VerticalLineStyleProperty); }
            set { this.SetValue(VerticalLineStyleProperty, value); }
        }

        /// <summary>
        /// Creates the internal series.
        /// </summary>
        /// <returns>
        /// The internal series.
        /// </returns>
        public override OxyPlot.Series.Series CreateModel()
        {
            this.SynchronizeProperties(this.InternalSeries);
            return this.InternalSeries;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.StairStepSeries)series;
            s.VerticalLineStyle = this.VerticalLineStyle;
            s.VerticalStrokeThickness = this.VerticalStrokeThickness;
        }
    }
}