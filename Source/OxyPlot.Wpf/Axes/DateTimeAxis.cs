using System;
using System.Windows;

namespace OxyPlot.Wpf
{
    public class DateTimeAxis : Axis
    {
        public DateTime FirstDateTime
        {
            get { return (DateTime)GetValue(FirstDateTimeProperty); }
            set { SetValue(FirstDateTimeProperty, value); }
        }

        public static readonly DependencyProperty FirstDateTimeProperty =
            DependencyProperty.Register("FirstDateTime", typeof(DateTime), typeof(DateTimeAxis), new UIPropertyMetadata(DateTime.MinValue));


        public DateTime LastDateTime
        {
            get { return (DateTime)GetValue(LastDateTimeProperty); }
            set { SetValue(LastDateTimeProperty, value); }
        }

        public static readonly DependencyProperty LastDateTimeProperty =
            DependencyProperty.Register("LastDateTime", typeof(DateTime), typeof(DateTimeAxis), new UIPropertyMetadata(DateTime.MaxValue));

        public DateTimeIntervalType IntervalType
        {
            get { return (DateTimeIntervalType)GetValue(IntervalTypeProperty); }
            set { SetValue(IntervalTypeProperty, value); }
        }

        public static readonly DependencyProperty IntervalTypeProperty =
            DependencyProperty.Register("IntervalType", typeof(DateTimeIntervalType), typeof(DateTimeAxis), new UIPropertyMetadata(DateTimeIntervalType.Auto));

        

        public DateTimeAxis()
        {
            ModelAxis = new OxyPlot.DateTimeAxis();
        }

        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = ModelAxis as OxyPlot.DateTimeAxis;

            a.IntervalType = IntervalType;
            
            if (FirstDateTime > DateTime.MinValue)
                a.Minimum = OxyPlot.DateTimeAxis.ToDouble(FirstDateTime);
            if (LastDateTime < DateTime.MaxValue)
                a.Maximum = OxyPlot.DateTimeAxis.ToDouble(LastDateTime);
        }
    }
}