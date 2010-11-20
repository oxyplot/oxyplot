using System.Windows;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// This is a WPF wrapper of OxyPlot.LineSeries
    /// Only a few properties are yet included.
    /// </summary>
    public class LineSeries : DataSeries
    {
        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof (double), typeof (LineSeries), new UIPropertyMetadata(1.0));

        public static readonly DependencyProperty SmoothProperty =
            DependencyProperty.Register("Smooth", typeof (bool), typeof (LineSeries), new UIPropertyMetadata(false));

        public static readonly DependencyProperty LineStyleProperty =
            DependencyProperty.Register("LineStyle", typeof (LineStyle), typeof (LineSeries),
                                        new UIPropertyMetadata(LineStyle.Solid));

        public double Thickness
        {
            get { return (double) GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        public bool Smooth
        {
            get { return (bool) GetValue(SmoothProperty); }
            set { SetValue(SmoothProperty, value); }
        }

        public LineStyle LineStyle
        {
            get { return (LineStyle) GetValue(LineStyleProperty); }
            set { SetValue(LineStyleProperty, value); }
        }

        public override OxyPlot.DataSeries CreateModel()
        {
            var s = new OxyPlot.LineSeries();
            ConvertProperties(s);
            return s;
        }

        public override void ConvertProperties(OxyPlot.DataSeries s)
        {
            base.ConvertProperties(s);
            var ls = s as OxyPlot.LineSeries;
            ls.Color = Color.ToOxyColor();
            ls.StrokeThickness = Thickness;
            ls.LineStyle = LineStyle;
            s.Smooth = Smooth;
        }
    }
}