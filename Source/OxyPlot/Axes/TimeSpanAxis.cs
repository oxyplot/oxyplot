// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an axis presenting <see cref="System.TimeSpan" /> values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represents an axis presenting <see cref="System.TimeSpan" /> values.
    /// </summary>
    /// <remarks>The values should be in seconds.
    /// The StringFormat value can be used to force formatting of the axis values
    /// <code>"h:mm"</code> shows hours and minutes
    /// <code>"m:ss"</code> shows minutes and seconds</remarks>
    public class TimeSpanAxis : LinearAxis
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "TimeSpanAxis" /> class.
        /// </summary>
        public TimeSpanAxis()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanAxis" /> class.
        /// </summary>
        /// <param name="position">The position of the axis.</param>
        /// <param name="title">The axis title.</param>
        /// <param name="format">The string format for the axis values.</param>
        [Obsolete]
        public TimeSpanAxis(AxisPosition position, string title = null, string format = "m:ss")
            : base(position, title)
        {
            this.StringFormat = format;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanAxis" /> class.
        /// </summary>
        /// <param name="position">The position of the axis.</param>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="title">The axis title.</param>
        /// <param name="format">The string format for the axis values.</param>
        [Obsolete]
        public TimeSpanAxis(
            AxisPosition position,
            double minimum,
            double maximum = double.NaN,
            string title = null,
            string format = "m:ss")
            : base(position, minimum, maximum, title)
        {
            this.StringFormat = format;
        }

        /// <summary>
        /// Converts a time span to a double.
        /// </summary>
        /// <param name="s">The time span.</param>
        /// <returns>A double value.</returns>
        public static double ToDouble(TimeSpan s)
        {
            return s.TotalSeconds;
        }

        /// <summary>
        /// Converts a double to a time span.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A time span.</returns>
        public static TimeSpan ToTimeSpan(double value)
        {
            return TimeSpan.FromSeconds(value);
        }

        /// <summary>
        /// Gets the value from an axis coordinate, converts from double to the correct data type if necessary. e.g. DateTimeAxis returns the DateTime and CategoryAxis returns category strings.
        /// </summary>
        /// <param name="x">The coordinate.</param>
        /// <returns>The value.</returns>
        public override object GetValue(double x)
        {
            return TimeSpan.FromSeconds(x);
        }

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        /// <param name="x">The value to format.</param>
        /// <returns>The formatted value.</returns>
        protected override string FormatValueOverride(double x)
        {
            var span = TimeSpan.FromSeconds(x);
            var s = this.ActualStringFormat ?? "h:mm:ss";

            s = s.Replace("mm", span.Minutes.ToString("00"));
            s = s.Replace("ss", span.Seconds.ToString("00"));
            s = s.Replace("hh", span.Hours.ToString("00"));
            s = s.Replace("msec", span.Milliseconds.ToString("000"));
            s = s.Replace("m", ((int)span.TotalMinutes).ToString("0"));
            s = s.Replace("s", ((int)span.TotalSeconds).ToString("0"));
            s = s.Replace("h", ((int)span.TotalHours).ToString("0"));
            return s;
        }

        /// <summary>
        /// Calculates the actual interval.
        /// </summary>
        /// <param name="availableSize">Size of the available area.</param>
        /// <param name="maxIntervalSize">Maximum length of the intervals.</param>
        /// <returns>The calculate actual interval.</returns>
        protected override double CalculateActualInterval(double availableSize, double maxIntervalSize)
        {
            double range = Math.Abs(this.ActualMinimum - this.ActualMaximum);
            double interval = 1;
            var goodIntervals = new[] { 1.0, 5, 10, 30, 60, 120, 300, 600, 900, 1200, 1800, 3600 };

            int maxNumberOfIntervals = Math.Max((int)(availableSize / maxIntervalSize), 2);

            while (true)
            {
                if (range / interval < maxNumberOfIntervals)
                {
                    return interval;
                }

                double nextInterval = goodIntervals.FirstOrDefault(i => i > interval);
                if (Math.Abs(nextInterval) < double.Epsilon)
                {
                    nextInterval = interval * 2;
                }

                interval = nextInterval;
            }
        }
    }
}