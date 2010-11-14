using System.Windows;

namespace OxyPlot.Wpf
{
    public class LinearAxis : Axis
    {
        public AxisPosition Position
        {
            get { return (AxisPosition)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(AxisPosition), typeof(LinearAxis), new UIPropertyMetadata(AxisPosition.Left));


        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(Axis), new UIPropertyMetadata(double.NaN));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(Axis), new UIPropertyMetadata(double.NaN));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Axis), new UIPropertyMetadata(null));

        public LinearAxis()
        {
            ModelAxis = new OxyPlot.LinearAxis();
        }

        public override void UpdateModelProperties()
        {
            var a = ModelAxis as OxyPlot.LinearAxis;
            a.Minimum = Minimum;
            a.Maximum = Maximum;
            a.Title = Title;
            a.Position = Position;
        }
    }
}