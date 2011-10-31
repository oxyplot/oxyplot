// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeAxis.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.DateTimeAxis.
    /// </summary>
    public class DateTimeAxis : Axis
    {
        #region Constants and Fields

        /// <summary>
        ///   The calendar week rule property.
        /// </summary>
        public static readonly DependencyProperty CalendarWeekRuleProperty =
            DependencyProperty.Register(
                "CalendarWeekRule", 
                typeof(CalendarWeekRule), 
                typeof(DateTimeAxis), 
                new PropertyMetadata(CalendarWeekRule.FirstFourDayWeek, DataChanged));

        /// <summary>
        ///   The first date time property.
        /// </summary>
        public static readonly DependencyProperty FirstDateTimeProperty = DependencyProperty.Register(
            "FirstDateTime", typeof(DateTime), typeof(DateTimeAxis), new PropertyMetadata(DateTime.MinValue));

        /// <summary>
        ///   The first day of week property.
        /// </summary>
        public static readonly DependencyProperty FirstDayOfWeekProperty = DependencyProperty.Register(
            "FirstDayOfWeek", 
            typeof(DayOfWeek), 
            typeof(DateTimeAxis), 
            new PropertyMetadata(DayOfWeek.Monday, DataChanged));

        /// <summary>
        ///   The interval type property.
        /// </summary>
        public static readonly DependencyProperty IntervalTypeProperty = DependencyProperty.Register(
            "IntervalType", 
            typeof(DateTimeIntervalType), 
            typeof(DateTimeAxis), 
            new PropertyMetadata(DateTimeIntervalType.Auto));

        /// <summary>
        ///   The last date time property.
        /// </summary>
        public static readonly DependencyProperty LastDateTimeProperty = DependencyProperty.Register(
            "LastDateTime", typeof(DateTime), typeof(DateTimeAxis), new PropertyMetadata(DateTime.MaxValue));

        /// <summary>
        ///   The minor interval type property.
        /// </summary>
        public static readonly DependencyProperty MinorIntervalTypeProperty =
            DependencyProperty.Register(
                "MinorIntervalType", 
                typeof(DateTimeIntervalType), 
                typeof(DateTimeAxis), 
                new PropertyMetadata(DateTimeIntervalType.Auto, DataChanged));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DateTimeAxis" /> class.
        /// </summary>
        public DateTimeAxis()
        {
            this.internalAxis = new OxyPlot.DateTimeAxis();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets CalendarWeekRule.
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
        ///   Gets or sets FirstDateTime.
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
        ///   Gets or sets FirstDayOfWeek.
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
        ///   Gets or sets IntervalType.
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
        ///   Gets or sets LastDateTime.
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
        ///   Gets or sets MinorIntervalType.
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

        #endregion

        #region Public Methods

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// </returns>
        public override OxyPlot.Axis CreateModel()
        {
            this.SynchronizeProperties();
            return this.internalAxis;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = this.internalAxis as OxyPlot.DateTimeAxis;

            a.IntervalType = this.IntervalType;
            a.MinorIntervalType = this.MinorIntervalType;
            a.FirstDayOfWeek = this.FirstDayOfWeek;
            a.CalendarWeekRule = this.CalendarWeekRule;

            if (this.FirstDateTime > DateTime.MinValue)
            {
                a.Minimum = OxyPlot.DateTimeAxis.ToDouble(this.FirstDateTime);
            }

            if (this.LastDateTime < DateTime.MaxValue)
            {
                a.Maximum = OxyPlot.DateTimeAxis.ToDouble(this.LastDateTime);
            }
        }

        #endregion
    }
}