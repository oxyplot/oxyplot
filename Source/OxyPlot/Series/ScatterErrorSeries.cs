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
//   Represents a series for scatter plots with the possibility to display error bars.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Represents a series for scatter plots with the possibility to display error bars.
    /// </summary>
    public class ScatterErrorSeries : ScatterSeries<ScatterErrorPoint>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScatterErrorSeries" /> class.
        /// </summary>
        public ScatterErrorSeries()
        {
            this.ErrorBarColor = OxyColors.Black;
            this.ErrorBarStrokeThickness = 1;
            this.ErrorBarStopWidth = 4.0;
            this.MinimumErrorSize = 1.5;
        }

        /// <summary>
        ///     Gets or sets the data field for the X error property.
        /// </summary>
        /// <value>
        ///     The data field.
        /// </value>
        public string DataFieldErrorX { get; set; }

        /// <summary>
        ///     Gets or sets the data field for the Y error property.
        /// </summary>
        /// <value>
        ///     The data field.
        /// </value>
        public string DataFieldErrorY { get; set; }

        /// <summary>
        ///     Gets or sets the color of the error bar.
        /// </summary>
        /// <value>
        ///     The color of the error bar.
        /// </value>
        public OxyColor ErrorBarColor { get; set; }

        /// <summary>
        ///     Gets or sets the width of the error bar stop.
        /// </summary>
        /// <value>
        ///     The width of the error bar stop.
        /// </value>
        public double ErrorBarStopWidth { get; set; }

        /// <summary>
        ///     Gets or sets the error bar stroke thickness.
        /// </summary>
        /// <value>
        ///     The error bar stroke thickness.
        /// </value>
        public double ErrorBarStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the minimum size (relative to <see cref="ScatterSeries{T}.MarkerSize" />) of the error bars to be shown. 
        /// </summary>
        public double MinimumErrorSize { get; set; }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        /// <param name="model">
        /// The owner plot model.
        /// </param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);

            var clippingRectangle = this.GetClippingRect();

            var segments = new List<ScreenPoint>();
            foreach (var point in this.ActualPoints)
            {
                if (point == null)
                {
                    continue;
                }

                if (point.ErrorX > 0.0)
                {
                    var leftErrorPoint = this.XAxis.Transform(point.X - (point.ErrorX * 0.5), point.Y, this.YAxis);
                    var rightErrorPoint = this.XAxis.Transform(point.X + (point.ErrorX * 0.5), point.Y, this.YAxis);

                    if (Math.Abs(rightErrorPoint.X - leftErrorPoint.X) > this.MarkerSize * this.MinimumErrorSize)
                    {
                        segments.Add(leftErrorPoint);
                        segments.Add(rightErrorPoint);
                        segments.Add(new ScreenPoint(leftErrorPoint.X, leftErrorPoint.Y - this.ErrorBarStopWidth));
                        segments.Add(new ScreenPoint(leftErrorPoint.X, leftErrorPoint.Y + this.ErrorBarStopWidth));
                        segments.Add(new ScreenPoint(rightErrorPoint.X, rightErrorPoint.Y - this.ErrorBarStopWidth));
                        segments.Add(new ScreenPoint(rightErrorPoint.X, rightErrorPoint.Y + this.ErrorBarStopWidth));
                    }
                }

                if (point.ErrorY > 0.0)
                {
                    var topErrorPoint = this.XAxis.Transform(point.X, point.Y - (point.ErrorY * 0.5), this.YAxis);
                    var bottomErrorPoint = this.XAxis.Transform(point.X, point.Y + (point.ErrorY * 0.5), this.YAxis);

                    if (Math.Abs(topErrorPoint.Y - bottomErrorPoint.Y) > this.MarkerSize * this.MinimumErrorSize)
                    {
                        segments.Add(topErrorPoint);
                        segments.Add(bottomErrorPoint);
                        segments.Add(new ScreenPoint(topErrorPoint.X - this.ErrorBarStopWidth, topErrorPoint.Y));
                        segments.Add(new ScreenPoint(topErrorPoint.X + this.ErrorBarStopWidth, topErrorPoint.Y));
                        segments.Add(new ScreenPoint(bottomErrorPoint.X - this.ErrorBarStopWidth, bottomErrorPoint.Y));
                        segments.Add(new ScreenPoint(bottomErrorPoint.X + this.ErrorBarStopWidth, bottomErrorPoint.Y));
                    }
                }
            }

            rc.DrawClippedLineSegments(clippingRectangle, segments, this.GetSelectableColor(this.ErrorBarColor), this.ErrorBarStrokeThickness, null, OxyPenLineJoin.Bevel, true);
        }

        /// <summary>
        /// Selects all points for which the passed function returns true.
        /// </summary>
        /// <param name="func">
        /// The function.
        /// </param>
        public void SelectAll(Func<ScatterErrorPoint, bool> func)
        {
            foreach (var dataPoint in this.Points.Where(func))
            {
                this.SelectItem(this.Points.IndexOf(dataPoint));
            }
        }

        /// <summary>
        /// Defines the data fields used by the code that reflects on the <see cref="ItemsSeries.ItemsSource" />.
        /// </summary>
        /// <param name="filler">The list filler.</param>
        protected override void DefineDataFields(ListFiller<ScatterErrorPoint> filler)
        {
            base.DefineDataFields(filler);
            filler.Add(this.DataFieldErrorX, (item, value) => item.ErrorX = Convert.ToDouble(value));
            filler.Add(this.DataFieldErrorY, (item, value) => item.ErrorY = Convert.ToDouble(value));
        }
    }
}
