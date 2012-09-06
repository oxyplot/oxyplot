// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeAxis.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;

namespace OxyPlot
{
    /// <summary>
    ///   Time Axis
    ///   The values should be in seconds.
    ///   The StringFormat value can be used to force formatting of the axis values
    ///   "h:mm" shows hours and minutes
    ///   "m:ss" shows minutes and seconds
    /// </summary>
    public class TimeAxis : LinearAxis
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "TimeAxis" /> class.
        /// </summary>
        /// <param name = "pos">The position.</param>
        /// <param name = "title">The axis title.</param>
        /// <param name = "format">The string format for the axis values.</param>
        public TimeAxis(AxisPosition pos, string title = null, string format = "m:ss")
            : base(pos, title)
        {
            StringFormat = format;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TimeAxis" /> class.
        /// </summary>
        /// <param name = "pos">The position.</param>
        /// <param name = "min">The min.</param>
        /// <param name = "max">The max.</param>
        /// <param name = "title">The axis title.</param>
        /// <param name = "format">The string format for the axis values.</param>
        public TimeAxis(AxisPosition pos = AxisPosition.Bottom, double min = double.NaN, double max = double.NaN,
                        string title = null, string format = "m:ss")
            : base(pos, min, max, title)
        {
            StringFormat = format;
        }

        /// <summary>
        ///   Formats the value.
        /// </summary>
        /// <param name = "x">The x.</param>
        /// <returns></returns>
        public override string FormatValue(double x)
        {
            var span = TimeSpan.FromSeconds(x);
            string s = ActualStringFormat ?? "h:mm:ss";

            s = s.Replace("mm", span.Minutes.ToString("00"));
            s = s.Replace("ss", span.Seconds.ToString("00"));
            s = s.Replace("hh", span.Hours.ToString("00"));
            s = s.Replace("msec", span.Milliseconds.ToString("000"));
            s = s.Replace("m", ((int)span.TotalMinutes).ToString("0"));
            s = s.Replace("s", ((int)span.TotalSeconds).ToString("0"));
            s = s.Replace("h", ((int)span.TotalHours).ToString("0"));
            return s;
        }

        protected override double CalculateActualInterval(double availableSize, double maxIntervalSize)
        {
            double range = Math.Abs(ActualMinimum - ActualMaximum);
            double interval = 1;
            var goodIntervals = new[] { 1.0, 5, 10, 30, 60, 120, 300, 600, 900, 1200, 1800, 3600 };

            const int maxSteps = 20;

            while (true)
            {
                if (range / interval < maxSteps)
                {
                    return interval;
                }

                double nextInterval = goodIntervals.FirstOrDefault(i => i > interval);
                if (nextInterval == 0)
                {
                    nextInterval = interval * 2;
                }

                interval = nextInterval;
            }
        }
    }
}