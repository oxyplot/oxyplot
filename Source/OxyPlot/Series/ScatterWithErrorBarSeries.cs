// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterWithErrorBarSeries.cs" company="OxyPlot">
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
    public class ScatterWithErrorBarSeries : ScatterSeries
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScatterWithErrorBarSeries" /> class.
        /// </summary>
        public ScatterWithErrorBarSeries()
            : base()
        {
            this.ErrorBarColor = OxyColors.Black;
            this.ErrorBarStrokeThickness = 1;
            this.ErrorBarStopWidth = 4.0;
            this.AlwaysShowErrorBars = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the data field for the error property.
        /// </summary>
        /// <value>
        ///     The data field.
        /// </value>
        public string DataFieldError { get; set; }

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
        /// Gets or sets a value indicating whether the error bars should always be displayed, regardless of their size. <br />
        /// By default, the error bars are only displayed when they are bigger than 1.5x the cursor size.
        /// </summary>
        /// <value>
        /// <c>true</c> if the error bars should always be displayed; otherwise, <c>false</c>.
        /// </value>
        public bool AlwaysShowErrorBars { get; set; }

        #endregion

        #region Public Methods and Operators

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

            OxyRect clippingRectangle = this.GetClippingRect();

            var segments = new List<ScreenPoint>();
            foreach (IDataPoint point in this.Points)
            {
                ScreenPoint screenPoint = this.XAxis.Transform(point.X, point.Y, this.YAxis);

                var errorItem = point as ScatterErrorPoint;
                double error = errorItem != null ? errorItem.Error : 0.0;

                if (error > 0.0)
                {
                    ScreenPoint topErrorPoint = this.XAxis.Transform(point.X, point.Y - (error * 0.5), this.YAxis);
                    ScreenPoint bottomErrorPoint = this.XAxis.Transform(point.X, point.Y + (error * 0.5), this.YAxis);

                    if (topErrorPoint.Y - bottomErrorPoint.Y > this.MarkerSize * 1.5 || this.AlwaysShowErrorBars)
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

            // clip the line segments with the clipping rectangle
            for (int i = 0; i + 1 < segments.Count; i += 2)
            {
                rc.DrawClippedLine(
                    new[] { segments[i], segments[i + 1] },
                    clippingRectangle,
                    2,
                    this.GetSelectableColor(this.ErrorBarColor),
                    this.ErrorBarStrokeThickness,
                    null,
                    OxyPenLineJoin.Bevel,
                    true);
            }
        }

        /// <summary>
        /// Selects all points for which the passed function returns true.
        /// </summary>
        /// <param name="func">
        /// The function.
        /// </param>
        public void SelectAll(Func<IDataPoint, bool> func)
        {
            foreach (var dataPoint in this.Points.Where(func))
            {
                this.SelectItem(this.Points.IndexOf(dataPoint));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Updates the data.
        /// </summary>
        protected internal override void UpdateData()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            this.Points.Clear();

            // Use the mapping to generate the points
            if (this.Mapping != null)
            {
                foreach (object item in this.ItemsSource)
                {
                    this.Points.Add(this.Mapping(item));
                }

                return;
            }

            var dest = new List<IDataPoint>();

            var filler = new ListFiller<ScatterErrorPoint>();
            filler.Add(this.DataFieldX, (item, value) => item.X = Convert.ToDouble(value));
            filler.Add(this.DataFieldY, (item, value) => item.Y = Convert.ToDouble(value));
            filler.Add(this.DataFieldSize, (item, value) => item.Size = Convert.ToDouble(value));
            filler.Add(this.DataFieldValue, (item, value) => item.Value = Convert.ToDouble(value));
            filler.Add(this.DataFieldTag, (item, value) => item.Tag = value);
            filler.Add(this.DataFieldError, (item, value) => item.Error = Convert.ToDouble(value));
            filler.Fill(dest, this.ItemsSource);

            this.Points = dest;
        }

        #endregion
    }
}
