namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.AreaSeries
    /// Note that property changes are not currently making the plot refresh itself.
    /// </summary>
    public class AreaSeries : LineSeries
    {
        #region Constants and Fields

        public static readonly DependencyProperty ConstantY2Property = DependencyProperty.Register(
            "ConstantY2", typeof(double), typeof(AreaSeries), new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty DataFieldX2Property = DependencyProperty.Register(
            "DataFieldX2", typeof(string), typeof(AreaSeries), new UIPropertyMetadata(null, DataChanged));

        public static readonly DependencyProperty DataFieldY2Property = DependencyProperty.Register(
            "DataFieldY2", typeof(string), typeof(AreaSeries), new UIPropertyMetadata(null, DataChanged));

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof(Color), typeof(AreaSeries), new UIPropertyMetadata(Colors.LightGray));

        #endregion

        #region Public Properties

        public double ConstantY2
        {
            get
            {
                return (double)this.GetValue(ConstantY2Property);
            }
            set
            {
                this.SetValue(ConstantY2Property, value);
            }
        }

        public string DataFieldX2
        {
            get
            {
                return (string)this.GetValue(DataFieldX2Property);
            }
            set
            {
                this.SetValue(DataFieldX2Property, value);
            }
        }

        public string DataFieldY2
        {
            get
            {
                return (string)this.GetValue(DataFieldY2Property);
            }
            set
            {
                this.SetValue(DataFieldY2Property, value);
            }
        }

        public Color Fill
        {
            get
            {
                return (Color)this.GetValue(FillProperty);
            }
            set
            {
                this.SetValue(FillProperty, value);
            }
        }

        #endregion

        #region Public Methods

        public override OxyPlot.ISeries CreateModel()
        {
            var s = new OxyPlot.AreaSeries();
            this.SynchronizeProperties(s);
            return s;
        }

        public override void SynchronizeProperties(OxyPlot.ISeries series)
        {
            base.SynchronizeProperties(series);
            var s = series as OxyPlot.AreaSeries;
            s.DataFieldX2 = this.DataFieldX2;
            s.DataFieldY2 = this.DataFieldY2;
            s.ConstantY2 = this.ConstantY2;
            s.Fill = this.Fill.ToOxyColor();
        }

        #endregion
    }
}