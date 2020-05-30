// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an axis presenting <see cref="System.DateTime" /> values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Represents an axis presenting <see cref="System.DateTime" /> values.
    /// </summary>
    /// <remarks>The actual numeric values on the axis are days since 1900/01/01.
    /// Use the static ToDouble and ToDateTime to convert numeric values to and from DateTimes.
    /// The StringFormat value can be used to force formatting of the axis values
    /// <code>"yyyy-MM-dd"</code> shows date
    /// <code>"w"</code> or <code>"ww"</code> shows week number
    /// <code>"h:mm"</code> shows hours and minutes</remarks>
    public class DateTimeAxis : LinearAxis
    {
        /// <summary>
        /// The time origin.
        /// </summary>
        /// <remarks>This gives the same numeric date values as Excel</remarks>
        private static readonly DateTime TimeOrigin = new DateTime(1899, 12, 31, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// The maximum day value
        /// </summary>
        private static readonly double MaxDayValue = (DateTime.MaxValue - TimeOrigin).TotalDays;

        /// <summary>
        /// The minimum day value
        /// </summary>
        private static readonly double MinDayValue = (DateTime.MinValue - TimeOrigin).TotalDays;

        /// <summary>
        /// The actual interval type.
        /// </summary>
        private DateTimeIntervalType actualIntervalType;

        /// <summary>
        /// The actual minor interval type.
        /// </summary>
        private DateTimeIntervalType actualMinorIntervalType;

        /// <summary>
        /// Initializes a new instance of the <see cref = "DateTimeAxis" /> class.
        /// </summary>
        public DateTimeAxis()
        {
            this.Position = AxisPosition.Bottom;
            this.IntervalType = DateTimeIntervalType.Auto;
            this.FirstDayOfWeek = DayOfWeek.Monday;
            this.CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek;
        }

        /// <summary>
        /// Gets or sets CalendarWeekRule.
        /// </summary>
        public CalendarWeekRule CalendarWeekRule { get; set; }

        /// <summary>
        /// Gets or sets FirstDayOfWeek.
        /// </summary>
        public DayOfWeek FirstDayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets IntervalType.
        /// </summary>
        public DateTimeIntervalType IntervalType { get; set; }

        /// <summary>
        /// Gets or sets MinorIntervalType.
        /// </summary>
        public DateTimeIntervalType MinorIntervalType { get; set; }

        /// <summary>
        /// Gets or sets the time zone (used when formatting date/time values).
        /// </summary>
        /// <value>The time zone info.</value>
        /// <remarks>No date/time conversion will be performed if this property is <c>null</c>.</remarks>
        public TimeZoneInfo TimeZone { get; set; }

        /// <summary>
        /// Creates a data point.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>A data point.</returns>
        public static DataPoint CreateDataPoint(DateTime x, double y)
        {
            return new DataPoint(ToDouble(x), y);
        }

        /// <summary>
        /// Creates a data point.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>A data point.</returns>
        public static DataPoint CreateDataPoint(DateTime x, DateTime y)
        {
            return new DataPoint(ToDouble(x), ToDouble(y));
        }

        /// <summary>
        /// Creates a data point.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>A data point.</returns>
        public static DataPoint CreateDataPoint(double x, DateTime y)
        {
            return new DataPoint(x, ToDouble(y));
        }

        /// <summary>
        /// Converts a numeric representation of the date (number of days after the time origin) to a DateTime structure.
        /// </summary>
        /// <param name="value">The number of days after the time origin.</param>
        /// <returns>A <see cref="DateTime" /> structure. Ticks = 0 if the value is invalid.</returns>
        public static DateTime ToDateTime(double value)
        {
            if (double.IsNaN(value) || value < MinDayValue || value > MaxDayValue)
            {
                return new DateTime();
            }

            return TimeOrigin.AddDays(value - 1);
        }

        /// <summary>
        /// Converts a DateTime to days after the time origin.
        /// </summary>
        /// <param name="value">The date/time structure.</param>
        /// <returns>The number of days after the time origin.</returns>
        public static double ToDouble(DateTime value)
        {
            var span = value - TimeOrigin;
            return span.TotalDays + 1;
        }

        /// <summary>
        /// Gets the tick values.
        /// </summary>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <param name="majorTickValues">The major tick values.</param>
        /// <param name="minorTickValues">The minor tick values.</param>
        public override void GetTickValues(
            out IList<double> majorLabelValues, out IList<double> majorTickValues, out IList<double> minorTickValues)
        {
            minorTickValues = this.CreateDateTimeTickValues(
                this.ActualMinimum, this.ActualMaximum, this.ActualMinorStep, this.actualMinorIntervalType);
            majorTickValues = this.CreateDateTimeTickValues(
                this.ActualMinimum, this.ActualMaximum, this.ActualMajorStep, this.actualIntervalType);
            majorLabelValues = majorTickValues;

            minorTickValues = AxisUtilities.FilterRedundantMinorTicks(majorTickValues, minorTickValues);
        }

        /// <summary>
        /// Gets the value from an axis coordinate, converts from double to the correct data type if necessary.
        /// e.g. DateTimeAxis returns the DateTime and CategoryAxis returns category strings.
        /// </summary>
        /// <param name="x">The coordinate.</param>
        /// <returns>The value.</returns>
        public override object GetValue(double x)
        {
            var time = ToDateTime(x);

            if (this.TimeZone != null)
            {
                time = TimeZoneInfo.ConvertTime(time, this.TimeZone);
            }

            return time;
        }

        /// <summary>
        /// Updates the intervals.
        /// </summary>
        /// <param name="plotArea">The plot area.</param>
        internal override void UpdateIntervals(OxyRect plotArea)
        {
            base.UpdateIntervals(plotArea);
            switch (this.actualIntervalType)
            {
                case DateTimeIntervalType.Years:
                    this.ActualMinorStep = 31;
                    this.actualMinorIntervalType = DateTimeIntervalType.Years;
                    if (this.StringFormat == null)
                    {
                        this.ActualStringFormat = "yyyy";
                    }

                    break;
                case DateTimeIntervalType.Months:
                    this.actualMinorIntervalType = DateTimeIntervalType.Months;
                    if (this.StringFormat == null)
                    {
                        this.ActualStringFormat = "yyyy-MM-dd";
                    }

                    break;
                case DateTimeIntervalType.Weeks:
                    this.actualMinorIntervalType = DateTimeIntervalType.Days;
                    this.ActualMajorStep = 7;
                    this.ActualMinorStep = 1;
                    if (this.StringFormat == null)
                    {
                        this.ActualStringFormat = "yyyy/ww";
                    }

                    break;
                case DateTimeIntervalType.Days:
                    this.ActualMinorStep = this.ActualMajorStep;
                    if (this.StringFormat == null)
                    {
                        this.ActualStringFormat = "yyyy-MM-dd";
                    }

                    break;
                case DateTimeIntervalType.Hours:
                    this.ActualMinorStep = this.ActualMajorStep;
                    if (this.StringFormat == null)
                    {
                        this.ActualStringFormat = "HH:mm";
                    }

                    break;
                case DateTimeIntervalType.Minutes:
                    this.ActualMinorStep = this.ActualMajorStep;
                    if (this.StringFormat == null)
                    {
                        this.ActualStringFormat = "HH:mm";
                    }

                    break;
                case DateTimeIntervalType.Seconds:
                    this.ActualMinorStep = this.ActualMajorStep;
                    if (this.StringFormat == null)
                    {
                        this.ActualStringFormat = "HH:mm:ss";
                    }

                    break;
                    
                    
                    
                case DateTimeIntervalType.Milliseconds:
                    this.ActualMinorStep = this.ActualMajorStep;
                    if (this.ActualStringFormat == null)
                    {
                        this.ActualStringFormat = "HH:mm:ss.fff";
                    }

                    break;
                    
                case DateTimeIntervalType.Manual:
                    break;
                case DateTimeIntervalType.Auto:
                    break;
            }
        }

        /// <summary>
        /// Gets the default string format.
        /// </summary>
        /// <returns>
        /// The format string.
        /// </returns>
        protected override string GetDefaultStringFormat()
        {
            return null;
        }

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        /// <param name="x">The value to format.</param>
        /// <returns>The formatted value.</returns>
        protected override string FormatValueOverride(double x)
        {
            // convert the double value to a DateTime
            var time = ToDateTime(x);

            // If a time zone is specified, convert the time
            if (this.TimeZone != null)
            {
                time = TimeZoneInfo.ConvertTime(time, this.TimeZone);
            }

            string fmt = this.ActualStringFormat;
            if (fmt == null)
            {
                return time.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            }

            int week = this.GetWeek(time);
            fmt = fmt.Replace("ww", week.ToString("00"));
            fmt = fmt.Replace("w", week.ToString(CultureInfo.InvariantCulture));
            fmt = string.Concat("{0:", fmt, "}");
            return string.Format(this.ActualCulture, fmt, time);
        }

        /// <summary>
        /// Calculates the actual interval.
        /// </summary>
        /// <param name="availableSize">Size of the available area.</param>
        /// <param name="maxIntervalSize">Maximum length of the intervals.</param>
        /// <returns>The calculate actual interval.</returns>
        protected override double CalculateActualInterval(double availableSize, double maxIntervalSize)
        {
            const double Year = 365.25;
            const double Month = 30.5;
            const double Week = 7;
            const double Day = 1.0;
            const double Hour = Day / 24;
            const double Minute = Hour / 60;
            const double Second = Minute / 60;
            const double MilliSecond = Second / 1000;

            double range = Math.Abs(this.ActualMinimum - this.ActualMaximum);

            var goodIntervals = new[]
                                    {   MilliSecond, 2 * MilliSecond, 10 * MilliSecond, 100 * MilliSecond,
                                        Second, 2 * Second, 5 * Second, 10 * Second, 30 * Second, Minute, 2 * Minute,
                                        5 * Minute, 10 * Minute, 30 * Minute, Hour, 4 * Hour, 8 * Hour, 12 * Hour, Day,
                                        2 * Day, 5 * Day, Week, 2 * Week, Month, 2 * Month, 3 * Month, 4 * Month,
                                        6 * Month, Year
                                    };

            double interval = goodIntervals[0];

            int maxNumberOfIntervals = Math.Max((int)(availableSize / maxIntervalSize), 2);

            while (true)
            {
                if (range / interval < maxNumberOfIntervals)
                {
                    break;
                }

                double nextInterval = goodIntervals.FirstOrDefault(i => i > interval);
                if (Math.Abs(nextInterval) <= double.Epsilon)
                {
                    nextInterval = interval * 2;
                }

                interval = nextInterval;
            }

            this.actualIntervalType = this.IntervalType;
            this.actualMinorIntervalType = this.MinorIntervalType;

            if (this.IntervalType == DateTimeIntervalType.Auto)
            {
                this.actualIntervalType = DateTimeIntervalType.Milliseconds;

                if (interval >= 1.0 / 24 / 60 / 60)
                {
                    this.actualIntervalType = DateTimeIntervalType.Seconds;
                }
                    
                if (interval >= 1.0 / 24 / 60)
                {
                    this.actualIntervalType = DateTimeIntervalType.Minutes;
                }

                if (interval >= 1.0 / 24)
                {
                    this.actualIntervalType = DateTimeIntervalType.Hours;
                }

                if (interval >= 1)
                {
                    this.actualIntervalType = DateTimeIntervalType.Days;
                }

                if (interval >= 30)
                {
                    this.actualIntervalType = DateTimeIntervalType.Months;
                }

                if (range >= 365.25)
                {
                    this.actualIntervalType = DateTimeIntervalType.Years;
                }
            }

            if (this.actualIntervalType == DateTimeIntervalType.Months)
            {
                double monthsRange = range / 30.5;
                interval = this.CalculateActualInterval(availableSize, maxIntervalSize, monthsRange);
            }

            if (this.actualIntervalType == DateTimeIntervalType.Years)
            {
                double yearsRange = range / 365.25;
                interval = this.CalculateActualInterval(availableSize, maxIntervalSize, yearsRange);
            }

            if (this.actualMinorIntervalType == DateTimeIntervalType.Auto)
            {
                switch (this.actualIntervalType)
                {
                    case DateTimeIntervalType.Years:
                        this.actualMinorIntervalType = DateTimeIntervalType.Months;
                        break;
                    case DateTimeIntervalType.Months:
                        this.actualMinorIntervalType = DateTimeIntervalType.Days;
                        break;
                    case DateTimeIntervalType.Weeks:
                        this.actualMinorIntervalType = DateTimeIntervalType.Days;
                        break;
                    case DateTimeIntervalType.Days:
                        this.actualMinorIntervalType = DateTimeIntervalType.Hours;
                        break;
                    case DateTimeIntervalType.Hours:
                        this.actualMinorIntervalType = DateTimeIntervalType.Minutes;
                        break;
                    default:
                        this.actualMinorIntervalType = DateTimeIntervalType.Days;
                        break;
                }
            }

            return interval;
        }

        /// <summary>
        /// Creates the date tick values.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <param name="step">The step.</param>
        /// <param name="intervalType">Type of the interval.</param>
        /// <returns>Date tick values.</returns>
        private IList<double> CreateDateTickValues(
            double min, double max, double step, DateTimeIntervalType intervalType)
        {
            var values = new Collection<double>();
            var start = ToDateTime(min);
            if (start.Ticks == 0)
            {
                // Invalid start time
                return values;
            }

            switch (intervalType)
            {
                case DateTimeIntervalType.Weeks:

                    // make sure the first tick is at the 1st day of a week
                    start = start.AddDays(-(int)start.DayOfWeek + (int)this.FirstDayOfWeek);
                    break;
                case DateTimeIntervalType.Months:

                    // make sure the first tick is at the 1st of a month
                    start = new DateTime(start.Year, start.Month, 1);
                    break;
                case DateTimeIntervalType.Years:

                    // make sure the first tick is at Jan 1st
                    start = new DateTime(start.Year, 1, 1);
                    break;
            }

            // Adds a tick to the end time to make sure the end DateTime is included.
            var end = ToDateTime(max).AddTicks(1);
            if (end.Ticks == 0)
            {
                // Invalid end time
                return values;
            }

            var current = start;
            double eps = step * 1e-3;
            var minDateTime = ToDateTime(min - eps);
            var maxDateTime = ToDateTime(max + eps);

            if (minDateTime.Ticks == 0 || maxDateTime.Ticks == 0)
            {
                // Invalid min/max time
                return values;
            }

            while (current < end)
            {
                if (current > minDateTime && current < maxDateTime)
                {
                    values.Add(ToDouble(current));
                }

                try
                {
                    switch (intervalType)
                    {
                        case DateTimeIntervalType.Months:
                            current = current.AddMonths((int)Math.Ceiling(step));
                            break;
                        case DateTimeIntervalType.Years:
                            current = current.AddYears((int)Math.Ceiling(step));
                            break;
                        default:
                            current = current.AddDays(step);
                            break;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    // AddMonths/AddYears/AddDays can throw an exception
                    // We could test this by comparing to MaxDayValue/MinDayValue, but it is easier to catch the exception...
                    break;
                }
            }

            return values;
        }

        /// <summary>
        /// Creates <see cref="DateTime" /> tick values.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="intervalType">The interval type.</param>
        /// <returns>A list of <see cref="DateTime" /> tick values.</returns>
        private IList<double> CreateDateTimeTickValues(
            double min, double max, double interval, DateTimeIntervalType intervalType)
        {
            // If the step size is more than 7 days (e.g. months or years) we use a specialized tick generation method that adds tick values with uneven spacing...
            if (intervalType > DateTimeIntervalType.Days)
            {
                return this.CreateDateTickValues(min, max, interval, intervalType);
            }

            // For shorter step sizes we use the method from Axis
            return this.CreateTickValues(min, max, interval);
        }

        /// <summary>
        /// Gets the week number for the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The week number for the current culture.</returns>
        private int GetWeek(DateTime date)
        {
            return this.ActualCulture.Calendar.GetWeekOfYear(date, this.CalendarWeekRule, this.FirstDayOfWeek);
        }
    }
}
