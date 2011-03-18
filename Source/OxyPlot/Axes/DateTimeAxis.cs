using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace OxyPlot
{
    /// <summary>
    ///   A date time interval.
    /// </summary>
    public enum DateTimeIntervalType
    {
        /// <summary>
        ///   Automatically determine interval.
        /// </summary>
        Auto = 0,

        /// <summary>
        ///   Interval type is milliseconds.
        /// </summary>
        Milliseconds = 1,

        /// <summary>
        ///   Interval type is seconds.
        /// </summary>
        Seconds = 2,

        /// <summary>
        ///   Interval type is minutes.
        /// </summary>
        Minutes = 3,

        /// <summary>
        ///   Interval type is hours.
        /// </summary>
        Hours = 4,

        /// <summary>
        ///   Interval type is days.
        /// </summary>
        Days = 5,

        /// <summary>
        ///   Interval type is weeks.
        /// </summary>
        Weeks = 6,

        /// <summary>
        ///   Interval type is months.
        /// </summary>
        Months = 7,

        /// <summary>
        ///   Interval type is years.
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
        // ========================================
        // TODO: this class needs some clean-up
        // ========================================

        internal static DateTime timeOrigin = new DateTime(1900, 1, 1); // Same date values as Excel

        private DateTimeIntervalType ActualIntervalType;

        private DateTimeIntervalType ActualMinorIntervalType;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DateTimeAxis" /> class.
        /// </summary>
        /// <param name = "pos">The position.</param>
        /// <param name = "title">The axis title.</param>
        /// <param name = "format">The string format for the axis values.</param>
        /// <param name = "intervalType">The interval type.</param>
        public DateTimeAxis(AxisPosition pos = AxisPosition.Bottom, string title = null, string format = null,
                            DateTimeIntervalType intervalType = DateTimeIntervalType.Auto)
            : base(pos, title)
        {
            Culture = CultureInfo.CurrentCulture;
            FirstDayOfWeek = DayOfWeek.Monday;
            CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek;

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
        /// <param name = "intervalType">The interval type.</param>
        public DateTimeAxis(DateTime firstDateTime, DateTime lastDateTime,
                            AxisPosition pos = AxisPosition.Bottom, string title = null, string format = null,
                            DateTimeIntervalType intervalType = DateTimeIntervalType.Auto)
            : this(pos, title, format, intervalType)
        {
            Minimum = ToDouble(firstDateTime);
            Maximum = ToDouble(lastDateTime);
        }

        public DateTimeIntervalType IntervalType { get; set; }

        public DateTimeIntervalType MinorIntervalType { get; set; }

        public DayOfWeek FirstDayOfWeek { get; set; }

        public CalendarWeekRule CalendarWeekRule { get; set; }

        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Converts a DateTime to a double.
        /// </summary>
        /// <param name="value">The date/time.</param>
        /// <returns></returns>
        public static double ToDouble(DateTime value)
        {
            var span = value - timeOrigin;
            return span.TotalDays + 1;
        }

        /// <summary>
        /// Converts a double precision value to a DateTime.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(double value)
        {
            if (double.IsNaN(value))
                return new DateTime();
            return timeOrigin.AddDays(value - 1);
        }

        public static DataPoint CreateDataPoint(DateTime x, double y)
        {
            return new DataPoint(DateTimeAxis.ToDouble(x),y);
        }
        public static DataPoint CreateDataPoint(DateTime x, DateTime y)
        {
            return new DataPoint(DateTimeAxis.ToDouble(x),DateTimeAxis.ToDouble(y));
        }

        public static DataPoint CreateDataPoint(double x, DateTime y)
        {
            return new DataPoint(x,DateTimeAxis.ToDouble(y));
        }

        /// <summary>
        ///   Formats the specified value by the axis' ActualStringFormat.
        /// </summary>
        /// <param name = "x">The x.</param>
        /// <returns>The formatted DateTime value</returns>
        public override string FormatValue(double x)
        {
            // convert the double value to a DateTime
            var time = ToDateTime(x);

            string fmt = ActualStringFormat;
            if (fmt == null)
            {
                return time.ToShortDateString();
            }

            int week = GetWeek(time);
            fmt = fmt.Replace("ww", week.ToString("00"));
            fmt = fmt.Replace("w", week.ToString());
            return time.ToString(fmt, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///   Gets the week number for the specified date.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <returns>The week number fr the current culture.</returns>
        private int GetWeek(DateTime date)
        {
            return Culture.Calendar.GetWeekOfYear(date, CalendarWeekRule, FirstDayOfWeek);
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
            if (intervalType > DateTimeIntervalType.Days)
            {
                return CreateDateTickValues(min, max, interval, intervalType);
            }

            // For shorter step sizes we use the method from AxisBase
            return CreateTickValues(min, max, interval);
        }

        protected override double GetLabelSize()
        {
            // todo: should measure sample DateTimes (min/max?) using the ActualStringFormat to get the size of the label
            string max = FormatValue(ActualMaximum);
            string min = FormatValue(ActualMinimum);
            int length = Math.Max(max.Length, min.Length);

            switch (Position)
            {
                case AxisPosition.Top:
                case AxisPosition.Bottom:
                    return length * 10;
            }

            return base.GetLabelSize();
        }

        public override void UpdateIntervals(OxyRect plotArea)
        {
            base.UpdateIntervals(plotArea);
            switch (ActualIntervalType)
            {
                case DateTimeIntervalType.Years:
                    ActualMinorStep = 31;
                    ActualMinorIntervalType = DateTimeIntervalType.Years;
                    if (ActualStringFormat == null)
                    {
                        ActualStringFormat = "yyyy";
                    }

                    break;
                case DateTimeIntervalType.Months:
                    ActualMinorIntervalType = DateTimeIntervalType.Months;
                    if (ActualStringFormat == null)
                    {
                        ActualStringFormat = "yyyy-MM-dd";
                    }

                    break;
                case DateTimeIntervalType.Weeks:
                    ActualMinorIntervalType = DateTimeIntervalType.Days;
                    ActualMajorStep = 7;
                    ActualMinorStep = 1;
                    if (ActualStringFormat == null)
                    {
                        ActualStringFormat = "yyyy/ww";
                    }

                    break;
                case DateTimeIntervalType.Days:
                    ActualMinorStep = ActualMajorStep;
                    if (ActualStringFormat == null)
                    {
                        ActualStringFormat = "yyyy-MM-dd";
                    }

                    break;
                case DateTimeIntervalType.Hours:
                    ActualMinorStep = ActualMajorStep;
                    if (ActualStringFormat == null)
                    {
                        ActualStringFormat = "HH:mm";
                    }

                    break;
                case DateTimeIntervalType.Minutes:
                    ActualMinorStep = ActualMajorStep;
                    if (ActualStringFormat == null)
                    {
                        ActualStringFormat = "HH:mm";
                    }

                    break;
                case DateTimeIntervalType.Seconds:
                    ActualMinorStep = ActualMajorStep;
                    if (ActualStringFormat == null)
                    {
                        ActualStringFormat = "HH:mm:ss";
                    }

                    break;
                default:
                    ActualMinorStep = ActualMajorStep;
                    break;
            }
        }

        private ICollection<double> CreateDateTickValues(double min, double max, double step,
                                                         DateTimeIntervalType intervalType)
        {
            var start = ToDateTime(min);
            switch (intervalType)
            {
                case DateTimeIntervalType.Weeks:

                    // make sure the first tick is at the 1st day of a week
                    start = start.AddDays(-(int)start.DayOfWeek + (int)FirstDayOfWeek);
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

            var current = start;
            var values = new Collection<double>();
            while (current < end)
            {
                if (current >= start)
                {
                    values.Add(ToDouble(current));
                }

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

            return values;
        }

        protected override double CalculateActualInterval(double availableSize, double maxIntervalSize)
        {
            double range = Math.Abs(ActualMinimum - ActualMaximum);


            var goodIntervals = new[]
                                    {
                                        1.0/24/60, 1.0/24/30, 1.0/24/12, 1.0/24/6, 1.0/24/2, 1.0/24, 1.0/6, 1.0/4, 1.0/2
                                        , 1, 2, 7, 14, 30.5, 30.5*2, 30.5*3, 30.5*4, 30.5*6, 365.25
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
                if (nextInterval == 0)
                {
                    nextInterval = interval * 2;
                }

                interval = nextInterval;
            }

            ActualIntervalType = IntervalType;
            ActualMinorIntervalType = MinorIntervalType;

            if (ActualIntervalType == DateTimeIntervalType.Auto)
            {
                ActualIntervalType = DateTimeIntervalType.Seconds;
                if (interval >= 1.0 / 24 / 60)
                {
                    ActualIntervalType = DateTimeIntervalType.Minutes;
                }

                if (interval >= 1.0 / 24)
                {
                    ActualIntervalType = DateTimeIntervalType.Hours;
                }

                if (interval >= 1)
                {
                    ActualIntervalType = DateTimeIntervalType.Days;
                }

                if (interval >= 30)
                {
                    ActualIntervalType = DateTimeIntervalType.Months;
                }

                if (range >= 365.25)
                {
                    ActualIntervalType = DateTimeIntervalType.Years;
                }
            }

            if (ActualIntervalType == DateTimeIntervalType.Months)
            {
                double monthsRange = range / 30.5;
                interval = CalculateActualInterval2(availableSize, maxIntervalSize, monthsRange);
            }

            if (ActualIntervalType == DateTimeIntervalType.Years)
            {
                double yearsRange = range / 365.25;
                interval = CalculateActualInterval2(availableSize, maxIntervalSize, yearsRange);
            }

            if (ActualMinorIntervalType == DateTimeIntervalType.Auto)
            {
                switch (ActualIntervalType)
                {
                    case DateTimeIntervalType.Years:
                        ActualMinorIntervalType = DateTimeIntervalType.Months;
                        break;
                    case DateTimeIntervalType.Months:
                        ActualMinorIntervalType = DateTimeIntervalType.Days;
                        break;
                    case DateTimeIntervalType.Weeks:
                        ActualMinorIntervalType = DateTimeIntervalType.Days;
                        break;
                    case DateTimeIntervalType.Days:
                        ActualMinorIntervalType = DateTimeIntervalType.Hours;
                        break;
                    case DateTimeIntervalType.Hours:
                        ActualMinorIntervalType = DateTimeIntervalType.Minutes;
                        break;
                    default:
                        ActualMinorIntervalType = DateTimeIntervalType.Days;
                        break;
                }
            }

            return interval;
        }
    }
}