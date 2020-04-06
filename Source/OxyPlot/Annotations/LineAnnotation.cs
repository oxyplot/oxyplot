// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an annotation that shows a straight line.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot.Axes;

    /// <summary>
    /// Represents an annotation that shows a straight line.
    /// </summary>
    public class LineAnnotation : PathAnnotation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "LineAnnotation" /> class.
        /// </summary>
        public LineAnnotation()
        {
            this.Type = LineAnnotationType.LinearEquation;
        }

        /// <summary>
        /// Gets or sets the y-intercept when Type is LinearEquation.
        /// </summary>
        /// <value>The intercept value.</value>
        /// <remarks>Linear equation y-intercept (the b in y=mx+b).
        /// http://en.wikipedia.org/wiki/Linear_equation</remarks>
        public double Intercept { get; set; }

        /// <summary>
        /// Gets or sets the slope when Type is LinearEquation.
        /// </summary>
        /// <value>The slope value.</value>
        /// <remarks>Linear equation slope (the m in y=mx+b)
        /// http://en.wikipedia.org/wiki/Linear_equation</remarks>
        public double Slope { get; set; }

        /// <summary>
        /// Gets or sets the type of line equation.
        /// </summary>
        public LineAnnotationType Type { get; set; }

        /// <summary>
        /// Gets or sets the X position for vertical lines (only for Type==Vertical).
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y position for horizontal lines (only for Type==Horizontal)
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets the screen points.
        /// </summary>
        /// <returns>The list of points to display on screen for this path.</returns>
        protected override IList<ScreenPoint> GetScreenPoints()
        {
            // y=f(x)
            Func<double, double> fx = null;

            // x=f(y)
            Func<double, double> fy = null;

            switch (this.Type)
            {
                case LineAnnotationType.Horizontal:
                    fx = x => this.Y;
                    break;
                case LineAnnotationType.Vertical:
                    fy = y => this.X;
                    break;
                default:
                    fx = x => (this.Slope * x) + this.Intercept;
                    break;
            }

            var points = new List<DataPoint>();

            bool isCurvedLine = !(this.XAxis is LinearAxis && this.YAxis is LinearAxis);

            if (!isCurvedLine)
            {
                // we only need to calculate two points if it is a straight line
                if (fx != null)
                {
                    points.Add(new DataPoint(this.ActualMinimumX, fx(this.ActualMinimumX)));
                    points.Add(new DataPoint(this.ActualMaximumX, fx(this.ActualMaximumX)));
                }
                else
                {
                    points.Add(new DataPoint(fy(this.ActualMinimumY), this.ActualMinimumY));
                    points.Add(new DataPoint(fy(this.ActualMaximumY), this.ActualMaximumY));
                }
            }
            else
            {
                if (fx != null)
                {
                    // todo: the step size should be adaptive
                    var n = 100;
                    var dx = (this.ActualMaximumX - this.ActualMinimumX) / 100;
                    for (int i = 0; i <= n; i++)
                    {
                        var x = this.ActualMinimumX + i * dx;
                        points.Add(new DataPoint(x, fx(x)));
                    }
                }
                else
                {
                    // todo: the step size should be adaptive
                    var n = 100;
                    var dy = (this.ActualMaximumY - this.ActualMinimumY) / n;
                    for (int i = 0; i <= n; i++)
                    {
                        var y = this.ActualMinimumY + i * dy;
                        points.Add(new DataPoint(fy(y), y));
                    }
                }
            }

            // transform to screen coordinates
            return points.Select(this.Transform).ToArray();
        }
    }
}
