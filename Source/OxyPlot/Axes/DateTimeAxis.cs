// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeAxis.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using System.Linq;

	/// <summary>
	/// Represents a DateTime axis.
	/// </summary>
	/// <remarks>
	/// The actual numeric values on the axis are days since 1900/01/01.
	///   Use the static ToDouble and ToDateTime to convert numeric values to DateTimes.
	///   The StringFormat value can be used to force formatting of the axis values
	///   "yyyy-MM-dd" shows date
	///   "w" or "ww" shows week number
	///   "h:mm" shows hours and minutes
	/// </remarks>
	public class DateTimeAxis : LinearAxis
	{
		#region Constants and Fields

		/// <summary>
		///   The time origin.
		/// </summary>
		private static DateTime timeOrigin = new DateTime(1900, 1, 1); // Same date values as Excel

		/// <summary>
		///   The actual interval type.
		/// </summary>
		private DateTimeIntervalType actualIntervalType;

		/// <summary>
		///   The actual minor interval type.
		/// </summary>
		private DateTimeIntervalType actualMinorIntervalType;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///   Initializes a new instance of the <see cref = "DateTimeAxis" /> class.
		/// </summary>
		public DateTimeAxis()
		{
			this.Position = AxisPosition.Bottom;
			this.IntervalType = DateTimeIntervalType.Auto;
			this.FirstDayOfWeek = DayOfWeek.Monday;
			this.CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DateTimeAxis"/> class.
		/// </summary>
		/// <param name="pos">
		/// The position.
		/// </param>
		/// <param name="title">
		/// The axis title.
		/// </param>
		/// <param name="format">
		/// The string format for the axis values.
		/// </param>
		/// <param name="intervalType">
		/// The interval type.
		/// </param>
		public DateTimeAxis(
			 AxisPosition pos = AxisPosition.Bottom,
			 string title = null,
			 string format = null,
			 DateTimeIntervalType intervalType = DateTimeIntervalType.Auto)
			: base(pos, title)
		{
			this.FirstDayOfWeek = DayOfWeek.Monday;
			this.CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek;

			this.StringFormat = format;
			this.IntervalType = intervalType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DateTimeAxis"/> class.
		/// </summary>
		/// <param name="firstDateTime">
		/// The first date/time on the axis.
		/// </param>
		/// <param name="lastDateTime">
		/// The last date/time on the axis.
		/// </param>
		/// <param name="pos">
		/// The position of the axis.
		/// </param>
		/// <param name="title">
		/// The axis title.
		/// </param>
		/// <param name="format">
		/// The string format for the axis values.
		/// </param>
		/// <param name="intervalType">
		/// The interval type.
		/// </param>
		public DateTimeAxis(
			 DateTime firstDateTime,
			 DateTime lastDateTime,
			 AxisPosition pos = AxisPosition.Bottom,
			 string title = null,
			 string format = null,
			 DateTimeIntervalType intervalType = DateTimeIntervalType.Auto)
			: this(pos, title, format, intervalType)
		{
			this.Minimum = ToDouble(firstDateTime);
			this.Maximum = ToDouble(lastDateTime);
		}

		#endregion

		#region Public Properties

		/// <summary>
		///   Gets or sets CalendarWeekRule.
		/// </summary>
		public CalendarWeekRule CalendarWeekRule { get; set; }

		/// <summary>
		///   Gets or sets FirstDayOfWeek.
		/// </summary>
		public DayOfWeek FirstDayOfWeek { get; set; }

		/// <summary>
		///   Gets or sets IntervalType.
		/// </summary>
		public DateTimeIntervalType IntervalType { get; set; }

		/// <summary>
		///   Gets or sets MinorIntervalType.
		/// </summary>
		public DateTimeIntervalType MinorIntervalType { get; set; }

		#endregion

		#region Public Methods

		/// <summary>
		/// The create data point.
		/// </summary>
		/// <param name="x">
		/// The x.
		/// </param>
		/// <param name="y">
		/// The y.
		/// </param>
		/// <returns>
		/// A data point.
		/// </returns>
		public static DataPoint CreateDataPoint(DateTime x, double y)
		{
			return new DataPoint(ToDouble(x), y);
		}

		/// <summary>
		/// The create data point.
		/// </summary>
		/// <param name="x">
		/// The x.
		/// </param>
		/// <param name="y">
		/// The y.
		/// </param>
		/// <returns>
		/// A data point.
		/// </returns>
		public static DataPoint CreateDataPoint(DateTime x, DateTime y)
		{
			return new DataPoint(ToDouble(x), ToDouble(y));
		}

		/// <summary>
		/// The create data point.
		/// </summary>
		/// <param name="x">
		/// The x.
		/// </param>
		/// <param name="y">
		/// The y.
		/// </param>
		/// <returns>
		/// A data point.
		/// </returns>
		public static DataPoint CreateDataPoint(double x, DateTime y)
		{
			return new DataPoint(x, ToDouble(y));
		}

		/// <summary>
		/// Converts a double precision value to a DateTime.
		/// </summary>
		/// <param name="value">
		/// The value.
		/// </param>
		/// <returns>
		/// A date/time structure.
		/// </returns>
		public static DateTime ToDateTime(double value)
		{
			if (double.IsNaN(value))
			{
				return new DateTime();
			}

			return timeOrigin.AddDays(value - 1);
		}

		/// <summary>
		/// Converts a DateTime to a double.
		/// </summary>
		/// <param name="value">
		/// The date/time.
		/// </param>
		/// <returns>
		/// The to double.
		/// </returns>
		public static double ToDouble(DateTime value)
		{
			TimeSpan span = value - timeOrigin;
			return span.TotalDays + 1;
		}

		/// <summary>
		/// Formats the specified value by the axis' ActualStringFormat.
		/// </summary>
		/// <param name="x">
		/// The x.
		/// </param>
		/// <returns>
		/// The formatted DateTime value
		/// </returns>
		public override string FormatValue(double x)
		{
			// convert the double value to a DateTime
			DateTime time = ToDateTime(x);

			string fmt = this.ActualStringFormat;
			if (fmt == null)
			{
				return time.ToShortDateString();
			}

			int week = this.GetWeek(time);
			fmt = fmt.Replace("ww", week.ToString("00"));
			fmt = fmt.Replace("w", week.ToString(CultureInfo.InvariantCulture));
			return time.ToString(fmt, this.ActualCulture);
		}

		/// <summary>
		/// The get tick values.
		/// </summary>
		/// <param name="majorLabelValues">
		/// The major label values.
		/// </param>
		/// <param name="majorTickValues">
		/// The major tick values.
		/// </param>
		/// <param name="minorTickValues">
		/// The minor tick values.
		/// </param>
		public override void GetTickValues(
			 out IList<double> majorLabelValues, out IList<double> majorTickValues, out IList<double> minorTickValues)
		{
			minorTickValues = this.CreateDateTimeTickValues(
				 this.ActualMinimum, this.ActualMaximum, this.ActualMinorStep, this.actualMinorIntervalType);
			majorTickValues = this.CreateDateTimeTickValues(
				 this.ActualMinimum, this.ActualMaximum, this.ActualMajorStep, this.actualIntervalType);
			majorLabelValues = majorTickValues;
		}

		/// <summary>
		/// Gets the value from an axis coordinate, converts from double to the correct data type if neccessary.
		///   e.g. DateTimeAxis returns the DateTime and CategoryAxis returns category strings.
		/// </summary>
		/// <param name="x">
		/// The coordinate.
		/// </param>
		/// <returns>
		/// The value.
		/// </returns>
		public override object GetValue(double x)
		{
			return ToDateTime(x);
		}

		#endregion

		#region Methods

		/// <summary>
		/// The update intervals.
		/// </summary>
		/// <param name="plotArea">
		/// The plot area.
		/// </param>
		internal override void UpdateIntervals(OxyRect plotArea)
		{
			base.UpdateIntervals(plotArea);
			switch (this.actualIntervalType)
			{
				case DateTimeIntervalType.Years:
					this.ActualMinorStep = 31;
					this.actualMinorIntervalType = DateTimeIntervalType.Years;
					if (this.ActualStringFormat == null)
					{
						this.ActualStringFormat = "yyyy";
					}

					break;
				case DateTimeIntervalType.Months:
					this.actualMinorIntervalType = DateTimeIntervalType.Months;
					if (this.ActualStringFormat == null)
					{
						this.ActualStringFormat = "yyyy-MM-dd";
					}

					break;
				case DateTimeIntervalType.Weeks:
					this.actualMinorIntervalType = DateTimeIntervalType.Days;
					this.ActualMajorStep = 7;
					this.ActualMinorStep = 1;
					if (this.ActualStringFormat == null)
					{
						this.ActualStringFormat = "yyyy/ww";
					}

					break;
				case DateTimeIntervalType.Days:
					this.ActualMinorStep = this.ActualMajorStep;
					if (this.ActualStringFormat == null)
					{
						this.ActualStringFormat = "yyyy-MM-dd";
					}

					break;
				case DateTimeIntervalType.Hours:
					this.ActualMinorStep = this.ActualMajorStep;
					if (this.ActualStringFormat == null)
					{
						this.ActualStringFormat = "HH:mm";
					}

					break;
				case DateTimeIntervalType.Minutes:
					this.ActualMinorStep = this.ActualMajorStep;
					if (this.ActualStringFormat == null)
					{
						this.ActualStringFormat = "HH:mm";
					}

					break;
				case DateTimeIntervalType.Seconds:
					this.ActualMinorStep = this.ActualMajorStep;
					if (this.ActualStringFormat == null)
					{
						this.ActualStringFormat = "HH:mm:ss";
					}

					break;
				case DateTimeIntervalType.Manual:
					break;
				case DateTimeIntervalType.Auto:
					break;
			}
		}

		/// <summary>
		/// Calculates the actual interval.
		/// </summary>
		/// <param name="availableSize">
		/// Size of the available area.
		/// </param>
		/// <param name="maxIntervalSize">
		/// Maximum length of the intervals.
		/// </param>
		/// <returns>
		/// The calculate actual interval.
		/// </returns>
		protected override double CalculateActualInterval(double availableSize, double maxIntervalSize)
		{
			const double YEAR = 365.25;
			const double MONTH = 30.5;
			const double WEEK = 7;
			const double DAY = 1.0;
			const double HOUR = DAY / 24;
			const double MINUTE = HOUR / 60;
			const double SECOND = MINUTE / 60;

			double range = Math.Abs(this.ActualMinimum - this.ActualMaximum);

			var goodIntervals = new[]
				{
					SECOND, 2 * SECOND, 5 * SECOND, 10 * SECOND, 30 * SECOND,
					MINUTE, 2 * MINUTE, 5 * MINUTE, 10 * MINUTE, 30 * MINUTE, 
					HOUR, 4 * HOUR, 8 * HOUR, 12 * HOUR, 
					DAY, 2 * DAY, 5 * DAY,
					WEEK, 2 * WEEK, 
					MONTH, 2 * MONTH, 3 * MONTH, 4 * MONTH, 6 * MONTH, 
					YEAR
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
				if (Math.Abs(nextInterval) < double.Epsilon)
				{
					nextInterval = interval * 2;
				}

				interval = nextInterval;
			}

			this.actualIntervalType = this.IntervalType;
			this.actualMinorIntervalType = this.MinorIntervalType;

			if (this.IntervalType == DateTimeIntervalType.Auto)
			{
				this.actualIntervalType = DateTimeIntervalType.Seconds;
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
		/// <param name="min">
		/// The min.
		/// </param>
		/// <param name="max">
		/// The max.
		/// </param>
		/// <param name="step">
		/// The step.
		/// </param>
		/// <param name="intervalType">
		/// Type of the interval.
		/// </param>
		/// <returns>
		/// Date tick values.
		/// </returns>
		private IList<double> CreateDateTickValues(
			 double min, double max, double step, DateTimeIntervalType intervalType)
		{
			DateTime start = ToDateTime(min);
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
			DateTime end = ToDateTime(max).AddTicks(1);

			DateTime current = start;
			var values = new Collection<double>();
			double eps = step * 1e-3;
			DateTime minDateTime = ToDateTime(min - eps);
			DateTime maxDateTime = ToDateTime(max + eps);
			while (current < end)
			{
				if (current > minDateTime && current < maxDateTime)
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

		/// <summary>
		/// The create date time tick values.
		/// </summary>
		/// <param name="min">
		/// The min.
		/// </param>
		/// <param name="max">
		/// The max.
		/// </param>
		/// <param name="interval">
		/// The interval.
		/// </param>
		/// <param name="intervalType">
		/// The interval type.
		/// </param>
		/// DateTime tick values.
		/// <returns>
		/// DateTime tick values.
		/// </returns>
		private IList<double> CreateDateTimeTickValues(
			 double min, double max, double interval, DateTimeIntervalType intervalType)
		{
			// If the step size is more than 7 days (e.g. months or years) we use a specialized tick generation method that adds tick values with uneven spacing...
			if (intervalType > DateTimeIntervalType.Days)
			{
				return this.CreateDateTickValues(min, max, interval, intervalType);
			}

			// For shorter step sizes we use the method from Axis
			return CreateTickValues(min, max, interval);
		}

		/// <summary>
		/// Gets the week number for the specified date.
		/// </summary>
		/// <param name="date">
		/// The date.
		/// </param>
		/// <returns>
		/// The week number fr the current culture.
		/// </returns>
		private int GetWeek(DateTime date)
		{
			return this.ActualCulture.Calendar.GetWeekOfYear(date, this.CalendarWeekRule, this.FirstDayOfWeek);
		}

		#endregion
	}
}