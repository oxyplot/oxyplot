// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.DateTimeAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows;

    using OxyPlot.Axes;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.DateTimeAxis.
    /// </summary>
    public class DateTimeAxis : Axis
    {
        /// <summary>
        /// Identifies the <see cref="CalendarWeekRule"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CalendarWeekRuleProperty =
            DependencyProperty.Register(
                "CalendarWeekRule",
                typeof(CalendarWeekRule),
                typeof(DateTimeAxis),
                new PropertyMetadata(CalendarWeekRule.FirstFourDayWeek, DataChanged));

        /// <summary>
        /// Identifies the <see cref="FirstDateTime"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FirstDateTimeProperty = DependencyProperty.Register(
            "FirstDateTime", typeof(DateTime), typeof(DateTimeAxis), new PropertyMetadata(DateTime.MinValue));

        /// <summary>
        /// Identifies the <see cref="FirstDayOfWeek"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FirstDayOfWeekProperty = DependencyProperty.Register(
            "FirstDayOfWeek",
            typeof(DayOfWeek),
            typeof(DateTimeAxis),
            new PropertyMetadata(DayOfWeek.Monday, DataChanged));

        /// <summary>
        /// Identifies the <see cref="IntervalType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IntervalTypeProperty = DependencyProperty.Register(
            "IntervalType",
            typeof(DateTimeIntervalType),
            typeof(DateTimeAxis),
            new PropertyMetadata(DateTimeIntervalType.Auto));

        /// <summary>
        /// Identifies the <see cref="LastDateTime"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastDateTimeProperty = DependencyProperty.Register(
            "LastDateTime", typeof(DateTime), typeof(DateTimeAxis), new PropertyMetadata(DateTime.MaxValue));

        /// <summary>
        /// Identifies the <see cref="MinorIntervalType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinorIntervalTypeProperty =
            DependencyProperty.Register(
                "MinorIntervalType",
                typeof(DateTimeIntervalType),
                typeof(DateTimeAxis),
                new PropertyMetadata(DateTimeIntervalType.Auto, DataChanged));

        /// <summary>
        /// Initializes static members of the <see cref="DateTimeAxis" /> class.
        /// </summary>
        static DateTimeAxis()
        {
            PositionProperty.OverrideMetadata(typeof(DateTimeAxis), new PropertyMetadata(AxisPosition.Bottom, AppearanceChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "DateTimeAxis" /> class.
        /// </summary>
        public DateTimeAxis()
        {
            this.InternalAxis = new Axes.DateTimeAxis();
        }

        /// <summary>
        /// Gets or sets CalendarWeekRule.
        /// </summary>
        public CalendarWeekRule CalendarWeekRule
        {
            get
            {
                return (CalendarWeekRule)this.GetValue(CalendarWeekRuleProperty);
            }

            set
            {
                this.SetValue(CalendarWeekRuleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FirstDateTime.
        /// </summary>
        public DateTime FirstDateTime
        {
            get
            {
                return (DateTime)this.GetValue(FirstDateTimeProperty);
            }

            set
            {
                this.SetValue(FirstDateTimeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FirstDayOfWeek.
        /// </summary>
        public DayOfWeek FirstDayOfWeek
        {
            get
            {
                return (DayOfWeek)this.GetValue(FirstDayOfWeekProperty);
            }

            set
            {
                this.SetValue(FirstDayOfWeekProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets IntervalType.
        /// </summary>
        public DateTimeIntervalType IntervalType
        {
            get
            {
                return (DateTimeIntervalType)this.GetValue(IntervalTypeProperty);
            }

            set
            {
                this.SetValue(IntervalTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LastDateTime.
        /// </summary>
        public DateTime LastDateTime
        {
            get
            {
                return (DateTime)this.GetValue(LastDateTimeProperty);
            }

            set
            {
                this.SetValue(LastDateTimeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinorIntervalType.
        /// </summary>
        public DateTimeIntervalType MinorIntervalType
        {
            get
            {
                return (DateTimeIntervalType)this.GetValue(MinorIntervalTypeProperty);
            }

            set
            {
                this.SetValue(MinorIntervalTypeProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal model.
        /// </summary>
        /// <returns>The internal axis.</returns>
        public override Axes.Axis CreateModel()
        {
            this.SynchronizeProperties();
            return this.InternalAxis;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Axes.DateTimeAxis)this.InternalAxis;

            a.IntervalType = this.IntervalType;
            a.MinorIntervalType = this.MinorIntervalType;
            a.FirstDayOfWeek = this.FirstDayOfWeek;
            a.CalendarWeekRule = this.CalendarWeekRule;

            if (this.FirstDateTime > DateTime.MinValue)
            {
                a.Minimum = Axes.DateTimeAxis.ToDouble(this.FirstDateTime);
            }

            if (this.LastDateTime < DateTime.MaxValue)
            {
                a.Maximum = Axes.DateTimeAxis.ToDouble(this.LastDateTime);
            }
        }
    }
}