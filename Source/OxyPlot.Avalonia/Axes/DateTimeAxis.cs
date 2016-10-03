// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.DateTimeAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using OxyPlot.Axes;
    using System;
    using System.Globalization;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.DateTimeAxis.
    /// </summary>
    public class DateTimeAxis : Axis
    {
        /// <summary>
        /// Identifies the <see cref="CalendarWeekRule"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<CalendarWeekRule> CalendarWeekRuleProperty = AvaloniaProperty.Register<DateTimeAxis, CalendarWeekRule>(nameof(CalendarWeekRule), CalendarWeekRule.FirstFourDayWeek);

        /// <summary>
        /// Identifies the <see cref="FirstDateTime"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<DateTime> FirstDateTimeProperty = AvaloniaProperty.Register<DateTimeAxis, DateTime>(nameof(FirstDateTime), DateTime.MinValue);

        /// <summary>
        /// Identifies the <see cref="FirstDayOfWeek"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty = AvaloniaProperty.Register<DateTimeAxis, DayOfWeek>(nameof(FirstDayOfWeek), DayOfWeek.Monday);

        /// <summary>
        /// Identifies the <see cref="IntervalType"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<DateTimeIntervalType> IntervalTypeProperty = AvaloniaProperty.Register<DateTimeAxis, DateTimeIntervalType>(nameof(IntervalType), DateTimeIntervalType.Auto);

        /// <summary>
        /// Identifies the <see cref="LastDateTime"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<DateTime> LastDateTimeProperty = AvaloniaProperty.Register<DateTimeAxis, DateTime>(nameof(LastDateTime), DateTime.MaxValue);

        /// <summary>
        /// Identifies the <see cref="MinorIntervalType"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<DateTimeIntervalType> MinorIntervalTypeProperty = AvaloniaProperty.Register<DateTimeAxis, DateTimeIntervalType>(nameof(MinorIntervalType), DateTimeIntervalType.Auto);

        /// <summary>
        /// Initializes static members of the <see cref="DateTimeAxis" /> class.
        /// </summary>
        static DateTimeAxis()
        {
            PositionProperty.OverrideDefaultValue<DateTimeAxis>(AxisPosition.Bottom);
            PositionProperty.Changed.AddClassHandler<DateTimeAxis>(AppearanceChanged);
            CalendarWeekRuleProperty.Changed.AddClassHandler<DateTimeAxis>(DataChanged);
            FirstDayOfWeekProperty.Changed.AddClassHandler<DateTimeAxis>(DataChanged);
            MinorIntervalTypeProperty.Changed.AddClassHandler<DateTimeAxis>(DataChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "DateTimeAxis" /> class.
        /// </summary>
        public DateTimeAxis()
        {
            InternalAxis = new Axes.DateTimeAxis();
        }

        /// <summary>
        /// Gets or sets CalendarWeekRule.
        /// </summary>
        public CalendarWeekRule CalendarWeekRule
        {
            get
            {
                return GetValue(CalendarWeekRuleProperty);
            }

            set
            {
                SetValue(CalendarWeekRuleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FirstDateTime.
        /// </summary>
        public DateTime FirstDateTime
        {
            get
            {
                return GetValue(FirstDateTimeProperty);
            }

            set
            {
                SetValue(FirstDateTimeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FirstDayOfWeek.
        /// </summary>
        public DayOfWeek FirstDayOfWeek
        {
            get
            {
                return GetValue(FirstDayOfWeekProperty);
            }

            set
            {
                SetValue(FirstDayOfWeekProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets IntervalType.
        /// </summary>
        public DateTimeIntervalType IntervalType
        {
            get
            {
                return GetValue(IntervalTypeProperty);
            }

            set
            {
                SetValue(IntervalTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LastDateTime.
        /// </summary>
        public DateTime LastDateTime
        {
            get
            {
                return GetValue(LastDateTimeProperty);
            }

            set
            {
                SetValue(LastDateTimeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinorIntervalType.
        /// </summary>
        public DateTimeIntervalType MinorIntervalType
        {
            get
            {
                return GetValue(MinorIntervalTypeProperty);
            }

            set
            {
                SetValue(MinorIntervalTypeProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal model.
        /// </summary>
        /// <returns>The internal axis.</returns>
        public override Axes.Axis CreateModel()
        {
            SynchronizeProperties();
            return InternalAxis;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Axes.DateTimeAxis)InternalAxis;

            a.IntervalType = IntervalType;
            a.MinorIntervalType = MinorIntervalType;
            a.FirstDayOfWeek = FirstDayOfWeek;
            a.CalendarWeekRule = CalendarWeekRule;

            if (FirstDateTime > DateTime.MinValue)
            {
                a.Minimum = Axes.DateTimeAxis.ToDouble(FirstDateTime);
            }

            if (LastDateTime < DateTime.MaxValue)
            {
                a.Maximum = Axes.DateTimeAxis.ToDouble(LastDateTime);
            }
        }
    }
}