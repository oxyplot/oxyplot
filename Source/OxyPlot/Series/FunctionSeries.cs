// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a line series that generates its dataset from a function.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;

    /// <summary>
    /// Represents a line series that generates its dataset from a function.
    /// </summary>
    /// <remarks>Define <c>f(x)</c> and make a plot on the range <c>[x0,x1]</c> or define <c>x(t)</c> and <c>y(t)</c> and make a plot on the range <c>[t0,t1]</c>.</remarks>
    public class FunctionSeries : LineSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "FunctionSeries" /> class.
        /// </summary>
        public FunctionSeries()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionSeries" /> class using a function <c>f(x)</c>.
        /// </summary>
        /// <param name="f">The function <c>f(x)</c>.</param>
        /// <param name="x0">The start x value.</param>
        /// <param name="x1">The end x value.</param>
        /// <param name="dx">The increment in x.</param>
        /// <param name="title">The title (optional).</param>
        public FunctionSeries(Func<double, double> f, double x0, double x1, double dx, string title = null)
        {
            this.Title = title;
            for (double x = x0; x <= x1 + (dx * 0.5); x += dx)
            {
                this.Points.Add(new DataPoint(x, f(x)));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionSeries" /> class using a function <c>f(x)</c>.
        /// </summary>
        /// <param name="f">The function <c>f(x)</c>.</param>
        /// <param name="x0">The start x value.</param>
        /// <param name="x1">The end x value.</param>
        /// <param name="n">The number of points.</param>
        /// <param name="title">The title (optional).</param>
        public FunctionSeries(Func<double, double> f, double x0, double x1, int n, string title = null)
            : this(f, x0, x1, (x1 - x0) / (n - 1), title)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionSeries" /> class using functions <c>x(t)</c> and <c>y(t)</c>.
        /// </summary>
        /// <param name="fx">The function <c>x(t)</c>.</param>
        /// <param name="fy">The function <c>y(t)</c>.</param>
        /// <param name="t0">The start t parameter.</param>
        /// <param name="t1">The end t parameter.</param>
        /// <param name="dt">The increment in t.</param>
        /// <param name="title">The title.</param>
        public FunctionSeries(Func<double, double> fx, Func<double, double> fy, double t0, double t1, double dt, string title = null)
        {
            this.Title = title;
            for (double t = t0; t <= t1 + (dt * 0.5); t += dt)
            {
                this.Points.Add(new DataPoint(fx(t), fy(t)));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionSeries" /> class using functions <c>x(t)</c> and <c>y(t)</c>.
        /// </summary>
        /// <param name="fx">The function <c>x(t)</c>.</param>
        /// <param name="fy">The function <c>y(t)</c>.</param>
        /// <param name="t0">The start t parameter.</param>
        /// <param name="t1">The end t parameter.</param>
        /// <param name="n">The number of points.</param>
        /// <param name="title">The title.</param>
        public FunctionSeries(
            Func<double, double> fx, Func<double, double> fy, double t0, double t1, int n, string title = null)
            : this(fx, fy, t0, t1, (t1 - t0) / (n - 1), title)
        {
        }
    }
}
