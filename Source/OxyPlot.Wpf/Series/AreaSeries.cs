using System.Windows;
using System.Windows.Media;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// This is a WPF wrapper of OxyPlot.AreaSeries
    /// Note that property changes are not currently making the plot refresh itself.
    /// </summary>
    public class AreaSeries : LineSeries
    {
        public static readonly DependencyProperty DataFieldX2Property =
            DependencyProperty.Register("DataFieldX2", typeof (string), typeof (AreaSeries),
                                        new UIPropertyMetadata(null, DataChanged));

        public static readonly DependencyProperty DataFieldY2Property =
            DependencyProperty.Register("DataFieldY2", typeof (string), typeof (AreaSeries),
                                        new UIPropertyMetadata(null, DataChanged));

        public static readonly DependencyProperty ConstantY2Property =
            DependencyProperty.Register("ConstantY2", typeof (double), typeof (AreaSeries), new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof (Color), typeof (AreaSeries),
                                        new UIPropertyMetadata(Colors.LightGray));

        public string DataFieldX2
        {
            get { return (string) GetValue(DataFieldX2Property); }
            set { SetValue(DataFieldX2Property, value); }
        }

        public string DataFieldY2
        {
            get { return (string) GetValue(DataFieldY2Property); }
            set { SetValue(DataFieldY2Property, value); }
        }

        public double ConstantY2
        {
            get { return (double) GetValue(ConstantY2Property); }
            set { SetValue(ConstantY2Property, value); }
        }

        public Color Fill
        {
            get { return (Color) GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public override OxyPlot.DataSeries CreateModel()
        {
            var s = new OxyPlot.AreaSeries();
            ConvertProperties(s);
            s.DataFieldX2 = DataFieldX2;
            s.DataFieldY2 = DataFieldY2;
            s.ConstantY2 = ConstantY2;
            s.Fill = Fill.ToOxyColor();
            return s;
        }
    }
}