// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScatterErrorSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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
    /// Represents a series for scatter plots with the possibility to display error bars.
    /// </summary>
    public class ScatterErrorSeries : ScatterSeries<ScatterErrorPoint>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterErrorSeries" /> class.
        /// </summary>
        public ScatterErrorSeries()
        {
            this.ErrorBarColor = OxyColors.Black;
            this.ErrorBarStrokeThickness = 1;
            this.ErrorBarStopWidth = 4.0;
            this.MinimumErrorSize = 0;
        }

        /// <summary>
        /// Gets or sets the data field for the X error property.
        /// </summary>
        /// <value>
        /// The data field.
        /// </value>
        public string DataFieldErrorX { get; set; }

        /// <summary>
        /// Gets or sets the data field for the Y error property.
        /// </summary>
        /// <value>
        /// The data field.
        /// </value>
        public string DataFieldErrorY { get; set; }

        /// <summary>
        /// Gets or sets the color of the error bar.
        /// </summary>
        /// <value>
        /// The color of the error bar.
        /// </value>
        public OxyColor ErrorBarColor { get; set; }

        /// <summary>
        /// Gets or sets the width of the error bar stop.
        /// </summary>
        /// <value>
        /// The width of the error bar stop.
        /// </value>
        public double ErrorBarStopWidth { get; set; }

        /// <summary>
        /// Gets or sets the error bar stroke thickness.
        /// </summary>
        /// <value>
        /// The error bar stroke thickness.
        /// </value>
        public double ErrorBarStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the minimum size (relative to <see cref="ScatterSeries{T}.MarkerSize" />) of the error bars to be shown.
        /// </summary>
        /// <value>
        /// The minimum size of the error.
        /// </value>
        public double MinimumErrorSize { get; set; }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">
        /// The rendering context.
        /// </param>
        public override void Render(IRenderContext rc)
        {
            base.Render(rc);

            var actualPoints = this.ActualPointsList;
            if (actualPoints == null || actualPoints.Count == 0)
            {
                return;
            }

            var clippingRectangle = this.GetClippingRect();

            var segments = new List<ScreenPoint>();
            foreach (var point in actualPoints)
            {
                if (point == null)
                {
                    continue;
                }

                var errorBarVectorX = this.Orientate(new ScreenVector(0, this.ErrorBarStopWidth));
                var errorBarVectorY = this.Orientate(new ScreenVector(this.ErrorBarStopWidth, 0));

                if (point.ErrorX > 0.0)
                {
                    var leftErrorPoint = this.Transform(point.X - (point.ErrorX * 0.5), point.Y);
                    var rightErrorPoint = this.Transform(point.X + (point.ErrorX * 0.5), point.Y);

                    if (rightErrorPoint.DistanceTo(leftErrorPoint) > this.MarkerSize * this.MinimumErrorSize)
                    {
                        segments.Add(leftErrorPoint);
                        segments.Add(rightErrorPoint);
                        segments.Add(leftErrorPoint - errorBarVectorX);
                        segments.Add(leftErrorPoint + errorBarVectorX);
                        segments.Add(rightErrorPoint - errorBarVectorX);
                        segments.Add(rightErrorPoint + errorBarVectorX);
                    }
                }

                if (point.ErrorY > 0.0)
                {
                    var topErrorPoint = this.Transform(point.X, point.Y - (point.ErrorY * 0.5));
                    var bottomErrorPoint = this.Transform(point.X, point.Y + (point.ErrorY * 0.5));

                    if (topErrorPoint.DistanceTo(bottomErrorPoint) > this.MarkerSize * this.MinimumErrorSize)
                    {
                        segments.Add(topErrorPoint);
                        segments.Add(bottomErrorPoint);
                        segments.Add(topErrorPoint - errorBarVectorY);
                        segments.Add(topErrorPoint + errorBarVectorY);
                        segments.Add(bottomErrorPoint - errorBarVectorY);
                        segments.Add(bottomErrorPoint + errorBarVectorY);
                    }
                }
            }

            rc.DrawClippedLineSegments(
                clippingRectangle, 
                segments, 
                this.GetSelectableColor(this.ErrorBarColor), 
                this.ErrorBarStrokeThickness, 
                this.EdgeRenderingMode,
                null, 
                LineJoin.Bevel);
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
        /// Updates from data fields.
        /// </summary>
        protected override void UpdateFromDataFields()
        {
            var filler = new ListBuilder<ScatterErrorPoint>();
            filler.Add(this.DataFieldX, double.NaN);
            filler.Add(this.DataFieldY, double.NaN);
            filler.Add(this.DataFieldErrorX, double.NaN);
            filler.Add(this.DataFieldErrorY, double.NaN);
            filler.Add(this.DataFieldSize, double.NaN);
            filler.Add(this.DataFieldValue, double.NaN);
            filler.Add(this.DataFieldTag, (object)null);
            filler.FillT(this.ItemsSourcePoints, this.ItemsSource, args => new ScatterErrorPoint(Convert.ToDouble(args[0]), Convert.ToDouble(args[1]), Convert.ToDouble(args[2]), Convert.ToDouble(args[3]), Convert.ToDouble(args[4]), Convert.ToDouble(args[5]), args[6]));
        }
    }
}
