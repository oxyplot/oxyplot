using System;

namespace OxyPlot
{
    /// <summary>
    /// The FunctionSeries generates its dataset from a Func.
    /// Define f(x) and make a plot on the range [x0,x1]
    /// or define fx(t) and fy(t) and make a plot on the range [t0,t1]
    /// </summary>
    public class FunctionSeries : LineSeries
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionSeries"/> class.
        /// </summary>
        /// <param name="f">The function f(x).</param>
        /// <param name="x0">The start x value.</param>
        /// <param name="x1">The end x value.</param>
        /// <param name="dx">The increment in x.</param>
        /// <param name="title">The title (optional).</param>
        public FunctionSeries(Func<double, double> f, double x0, double x1, double dx, string title = null)
        {
            Title = title;
            for (double x = x0; x <= x1+dx/2; x += dx)
                Points.Add(new DataPoint(x, f(x)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionSeries"/> class.
        /// </summary>
        /// <param name="fx">The function fx(t).</param>
        /// <param name="fy">The function fy(t).</param>
        /// <param name="t0">The t0.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="dt">The increment dt.</param>
        /// <param name="title">The title.</param>
        public FunctionSeries(Func<double, double> fx, Func<double, double> fy, double t0, double t1, double dt, string title = null)
        {
            Title = title;
            for (double t = t0; t <= t1+dt/2; t += dt)
                Points.Add(new DataPoint(fx(t), fy(t)));
        }
    }
}