using System.Windows;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// This is a WPF wrapper of OxyPlot.AreaSeries
    /// Only a few properties are yet included.
    /// </summary>
    public class AreaSeries : LineSeries
    {
        public string DataFieldBase
        {
            get { return (string)GetValue(DataFieldBaseProperty); }
            set { SetValue(DataFieldBaseProperty, value); }
        }

        public static readonly DependencyProperty DataFieldBaseProperty =
            DependencyProperty.Register("DataFieldBase", typeof(string), typeof(DataSeries), new UIPropertyMetadata(null, DataChanged));

        public double Baseline
        {
            get { return (double)GetValue(BaselineProperty); }
            set { SetValue(BaselineProperty, value); }
        }

        public static readonly DependencyProperty BaselineProperty =
            DependencyProperty.Register("Baseline", typeof(double), typeof(AreaSeries), new UIPropertyMetadata(0.0));

        public System.Windows.Media.Color Fill
        {
            get { return (System.Windows.Media.Color)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(System.Windows.Media.Color), typeof(AreaSeries), new UIPropertyMetadata(System.Windows.Media.Colors.LightGray));


        public override OxyPlot.DataSeries CreateModel()
        {
            var s = new OxyPlot.AreaSeries();
            ConvertProperties(s);
            s.DataFieldY2 = DataFieldBase;
            s.ConstantY2 = Baseline;
            s.Fill = Fill.ToOxyColor();
            return s;
        }
    }
}