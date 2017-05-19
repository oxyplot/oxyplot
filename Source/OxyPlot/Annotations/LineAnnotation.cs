﻿// --------------------------------------------------------------------------------------------------------------------
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
            this.Aliased = false;

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
                    fy = y => (y - this.Intercept) / this.Slope;
                    break;
            }

            var points = new List<DataPoint>();

            bool isCurvedLine = !(this.XAxis is LinearAxis && this.YAxis is LinearAxis);

            if (!isCurvedLine)
            {
                // we only need to calculate two points if it is a straight line
                if (fx != null)
                {
                    // Calculate minimum and maximum x any y values clipped to min/max range
                    double ymin = Clamp(fx(this.ActualMinimumX), this.ActualMinimumY, this.ActualMaximumY);
                    double ymax = Clamp(fx(this.ActualMaximumX),this.ActualMinimumY,this.ActualMaximumY);
                    points.Add(new DataPoint(fy(ymin), ymin));
                    points.Add(new DataPoint(fy(ymax), ymax));
                }
                else
                {
                    points.Add(new DataPoint(fy(this.ActualMinimumX), this.ActualMinimumY));
                    points.Add(new DataPoint(fy(this.ActualMaximumX), this.ActualMaximumY));
                }

                if (this.Type == LineAnnotationType.Horizontal || this.Type == LineAnnotationType.Vertical)
                {
                    // use aliased line drawing for horizontal and vertical lines
                    this.Aliased = true;
                }
            }
            else
            {
                if (fx != null)
                {
                    double x = this.ActualMinimumX;

                    // todo: the step size should be adaptive
                    double dx = (this.ActualMaximumX - this.ActualMinimumX) / 100;
                    while (true)
                    {
                        points.Add(new DataPoint(x, fx(x)));
                        if (x > this.ActualMaximumX)
                        {
                            break;
                        }

                        x += dx;
                    }
                }
                else
                {
                    double y = this.ActualMinimumY;

                    // todo: the step size should be adaptive
                    double dy = (this.ActualMaximumY - this.ActualMinimumY) / 100;
                    while (true)
                    {
                        points.Add(new DataPoint(fy(y), y));
                        if (y > this.ActualMaximumY)
                        {
                            break;
                        }

                        y += dy;
                    }
                }
            }

            // transform to screen coordinates
            return points.Select(this.Transform).ToList();
        }

        /// <summary>
        /// Limits value to within Min and Maximum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        static double Clamp(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}