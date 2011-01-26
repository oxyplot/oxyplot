using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace OxyPlot
{
    /// <summary>
    /// A date time interval.
    /// </summary>
    public enum DateTimeIntervalType
    {
        /// <summary>
        /// Automatically determine interval.
        /// </summary>
        Auto = 0,

        /// <summary>
        /// Interval type is milliseconds.
        /// </summary>
        Milliseconds = 1,

        /// <summary>
        /// Interval type is seconds.
        /// </summary>
        Seconds = 2,

        /// <summary>
        /// Interval type is minutes.
        /// </summary>
        Minutes = 3,

        /// <summary>
        /// Interval type is hours.
        /// </summary>
        Hours = 4,

        /// <summary>
        /// Interval type is days.
        /// </summary>
        Days = 5,

        /// <summary>
        /// Interval type is weeks.
        /// </summary>
        Weeks = 6,

        /// <summary>
        /// Interval type is months.
        /// </summary>
        Months = 7,

        /// <summary>
        /// Interval type is years.
        /// </summary>
        Years = 8,
    }

    /// <summary>
    ///   DateTime Axis
    ///   The actual numeric values on the axis are days since 1900/01/01.
    ///   Use the static ToDouble and ToDateTime to convert numeric values to DateTimes.
    ///   The StringFormat value can be used to force formatting of the axis values
    ///   "yyyy-MM-dd" shows date
    ///   "w" or "ww" shows week number
    ///   "h:mm" shows hours and minutes
    /// </summary>
    public class DateTimeAxis : LinearAxis
    {
        // TODO: this class needs some clean-up

        private const string DefaultFormat = "yyyy-MM-dd";

        internal static DateTime time0 = new DateTime(1900, 1, 1);

        private DateTimeIntervalType ActualIntervalType;
        private DateTimeIntervalType ActualMinorIntervalType;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DateTimeAxis" /> class.
        /// </summary>
        /// <param name = "pos">The position.</param>
        /// <param name = "title">The axis title.</param>
        /// <param name = "format">The string format for the axis values.</param>
        /// <param name = "intervalType">The interval type.</param>
        public DateTimeAxis(AxisPosition pos = AxisPosition.Bottom, string title = null, string format = DefaultFormat,
                            DateTimeIntervalType intervalType = DateTimeIntervalType.Auto)
            : base(pos, title)
        {
            StringFormat = format;
            IntervalType = intervalType;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DateTimeAxis" /> class.
        /// </summary>
        /// <param name = "firstDateTime">The first date/time on the axis.</param>
        /// <param name = "lastDateTime">The last date/time on the axis.</param>
        /// <param name = "pos">The position of the axis.</param>
        /// <param name = "title">The axis title.</param>
        /// <param name = "format">The string format for the axis values.</param>
        /// <param name="intervalType">The interval type.</param>
        public DateTimeAxis(DateTime firstDateTime, DateTime lastDateTime,
                            AxisPosition pos = AxisPosition.Bottom, string title = null, string format = DefaultFormat,
                            DateTimeIntervalType intervalType = DateTimeIntervalType.Auto)
            : this(pos, title, format, intervalType)
        {
            Minimum = ToDouble(firstDateTime);
            Maximum = ToDouble(lastDateTime);
        }

        public DateTime MinimumDateTime { get; set; }
        public DateTimeIntervalType IntervalType { get; set; }

        public static double ToDouble(DateTime value)
        {
            TimeSpan span = value - time0;
            return span.TotalDays;
        }

        public static DateTime ToDateTime(double value)
        {
            return time0.AddDays(value);
        }

        /// <summary>
        ///   Formats the specified value by the axis' ActualStringFormat.
        /// </summary>
        /// <param name = "x">The x.</param>
        /// <returns></returns>
        public override string FormatValue(double x)
        {
            // convert the double value to a DateTime
            DateTime time = ToDateTime(x);
            string fmt = ActualStringFormat ?? DefaultFormat;
            int week = GetWeek(time);
            fmt = fmt.Replace("ww", week.ToString("00"));
            fmt = fmt.Replace("w", week.ToString());
            return time.ToString(fmt, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the week number for the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The week number fr the current culture.</returns>
        private static int GetWeek(DateTime date)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek,
                                                                     DayOfWeek.Monday);
        }


        public override void GetTickValues(out ICollection<double> majorValues, out ICollection<double> minorValues)
        {
            minorValues = CreateDateTimeTickValues(ActualMinimum, ActualMaximum, ActualMinorStep,
                                                   ActualMinorIntervalType);
            majorValues = CreateDateTimeTickValues(ActualMinimum, ActualMaximum, ActualMajorStep, ActualIntervalType);
        }

        private ICollection<double> CreateDateTimeTickValues(double min, double max, double interval,
                                                             DateTimeIntervalType intervalType)
        {
            // If the step size is more than 7 days (e.g. months or years) we use a specialized tick generation method that adds tick values with uneven spacing...
            if (interval > 7)
                return CreateDateTickValues(min, max, interval, intervalType);

            // For shorter step sizes we use the method from AxisBase
            return CreateTickValues(min, max, interval);
        }

        protected override double GetLabelSize()
        {
            // todo: should measure sample DateTimes (min/max?) using the ActualStringFormat to get the size of the label

            switch (position)
            {
                case AxisPosition.Top:
                case AxisPosition.Bottom:
                    return 120;
            }
            return base.GetLabelSize();
        }

        public override void UpdateIntervals(double dx, double dy)
        {
            base.UpdateIntervals(dx, dy);
            switch (ActualIntervalType)
            {
                case DateTimeIntervalType.Years:
                    ActualMinorStep = 31;
                    ActualMinorIntervalType = DateTimeIntervalType.Months;
                    if (ActualStringFormat == null)
                        ActualStringFormat = "yyyy";
                    break;
                case DateTimeIntervalType.Months:
                    ActualMajorStep = 31;
                    ActualMinorStep = 31;
                    ActualMinorIntervalType = DateTimeIntervalType.Months;
                    if (ActualStringFormat==null)
                        ActualStringFormat = "yyyy-MM-dd";
                    break;
                case DateTimeIntervalType.Weeks:
                    ActualMajorStep = 7;
                    ActualMinorStep = 1;
                    if (ActualStringFormat == null)
                        ActualStringFormat = "yyyy/ww";
                    break;
                case DateTimeIntervalType.Days:
                    // ActualMajorStep = 1;
                    ActualMinorStep = ActualMajorStep;
                    if (ActualStringFormat == null)
                        ActualStringFormat = "yyyy-MM-dd";
                    break;
                case DateTimeIntervalType.Hours:
                    // ActualMajorStep = 1.0 / 24;
                    ActualMinorStep = ActualMajorStep;
                    if (ActualStringFormat == null)
                        ActualStringFormat = "HH:mm";
                    break;
                case DateTimeIntervalType.Minutes:
                    ActualMinorStep = ActualMajorStep;
                    if (ActualStringFormat == null)
                        ActualStringFormat = "HH:mm";
                    break;
                case DateTimeIntervalType.Seconds:
                    ActualMinorStep = ActualMajorStep;
                    if (ActualStringFormat == null)
                        ActualStringFormat = "HH:mm:ss";
                    break;
                default:
                    ActualMinorStep = ActualMajorStep;
                    break;
            }
        }

        private static ICollection<double> CreateDateTickValues(double min, double max, double step, DateTimeIntervalType intervalType)
        {
            DateTime start = ToDateTime(min);
            switch (intervalType)
            {
                case DateTimeIntervalType.Weeks:
                    // make sure the first tick is at the 1st day of a week
                    // todo: day 0 is sunday? globalize?
                    start = start.AddDays(-(int)start.DayOfWeek);
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

            DateTime current = start;
            var values = new Collection<double>();
            while (current < end)
            {
                if (current >= start)
                    values.Add(ToDouble(current));

                switch (intervalType)
                {
                    case DateTimeIntervalType.Months:
                        current = current.AddMonths(1);
                        break;
                    case DateTimeIntervalType.Years:
                        current = current.AddYears(1);
                        break;
                    default:
                        current = current.AddDays(step);
                        break;
                }
            }
            return values;
        }

        protected override double CalculateActualInterval(double availableSize, double maxIntervalSize)
        {
            double range = Math.Abs(ActualMinimum - ActualMaximum);

            ActualIntervalType = IntervalType;

            if (ActualIntervalType == DateTimeIntervalType.Auto)
            {
                ActualIntervalType = DateTimeIntervalType.Seconds;
                if (range > 1.0 / 24 / 30)
                    ActualIntervalType = DateTimeIntervalType.Minutes;
                if (range > 1.0 / 12)
                    ActualIntervalType = DateTimeIntervalType.Hours;
                if (range > 2)
                    ActualIntervalType = DateTimeIntervalType.Days;
                //if (range > 7 * 2)
                //    ActualIntervalType = DateTimeIntervalType.Weeks;
                if (range > 30 * 2)
                    ActualIntervalType = DateTimeIntervalType.Months;
                if (range > 365 * 2)
                    ActualIntervalType = DateTimeIntervalType.Years;
            }

            double interval = 1.0 / 24 / 60;
            var goodIntervals = new[]
                                    {
                                        1.0/24/60, 1.0/24/30, 1.0/24/12, 1.0/24/6, 1.0/24/2, 1.0/24, 1.0/6, 1.0/4, 1.0/2
                                        , 1
                                        , 7, 14, 30, 365
                                    };

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