namespace OxyPlot.Wpf
{
    using System;
    using System.Globalization;
    using System.Windows;

    public class DateTimeAxis : AxisBase
    {
        #region Constants and Fields

        public static readonly DependencyProperty CalendarWeekRuleProperty =
            DependencyProperty.Register(
                "CalendarWeekRule",
                typeof(CalendarWeekRule),
                typeof(DateTimeAxis),
                new UIPropertyMetadata(CalendarWeekRule.FirstFourDayWeek, DataChanged));

        public static readonly DependencyProperty FirstDateTimeProperty = DependencyProperty.Register(
            "FirstDateTime", typeof(DateTime), typeof(DateTimeAxis), new UIPropertyMetadata(DateTime.MinValue));

        public static readonly DependencyProperty FirstDayOfWeekProperty = DependencyProperty.Register(
            "FirstDayOfWeek",
            typeof(DayOfWeek),
            typeof(DateTimeAxis),
            new UIPropertyMetadata(DayOfWeek.Monday, DataChanged));

        public static readonly DependencyProperty IntervalTypeProperty = DependencyProperty.Register(
            "IntervalType",
            typeof(DateTimeIntervalType),
            typeof(DateTimeAxis),
            new UIPropertyMetadata(DateTimeIntervalType.Auto));

        public static readonly DependencyProperty LastDateTimeProperty = DependencyProperty.Register(
            "LastDateTime", typeof(DateTime), typeof(DateTimeAxis), new UIPropertyMetadata(DateTime.MaxValue));

        public static readonly DependencyProperty MinorIntervalTypeProperty =
            DependencyProperty.Register(
                "MinorIntervalType",
                typeof(DateTimeIntervalType),
                typeof(DateTimeAxis),
                new UIPropertyMetadata(DateTimeIntervalType.Auto, DataChanged));

        #endregion

        #region Constructors and Destructors

        public DateTimeAxis()
        {
            this.Axis = new OxyPlot.DateTimeAxis();
        }

        #endregion

        #region Public Properties

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

        public override OxyPlot.IAxis CreateModel()
        {
            this.SynchronizeProperties();
            return this.Axis;
        }

        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = this.Axis as OxyPlot.DateTimeAxis;

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