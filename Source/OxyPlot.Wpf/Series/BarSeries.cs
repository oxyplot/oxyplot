namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.BarSeries
    /// </summary>
    public class BarSeries : PlotSeriesBase
    {
        #region Constants and Fields

        public static readonly DependencyProperty BarWidthProperty = DependencyProperty.Register(
            "BarWidth", typeof(double), typeof(BarSeries), new UIPropertyMetadata(0.5));

        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register(
            "FillColor", typeof(Color?), typeof(BarSeries), new UIPropertyMetadata(null));

        public static readonly DependencyProperty IsStackedProperty = DependencyProperty.Register(
            "IsStacked", typeof(bool), typeof(BarSeries), new UIPropertyMetadata(false));

        public static readonly DependencyProperty StrokeColorProperty = DependencyProperty.Register(
            "StrokeColor", typeof(Color), typeof(BarSeries), new UIPropertyMetadata(Colors.Black));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", typeof(double), typeof(BarSeries), new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty ValueFieldProperty = DependencyProperty.Register(
            "ValueField", typeof(string), typeof(BarSeries), new UIPropertyMetadata(null));

        #endregion

        #region Constructors and Destructors

        static BarSeries()
        {
            TrackerFormatStringProperty.OverrideMetadata(
                typeof(CategoryAxis), new FrameworkPropertyMetadata("{0} {1}: {2}", DataChanged));
        }

        #endregion

        #region Public Properties

        public double BarWidth
        {
            get
            {
                return (double)this.GetValue(BarWidthProperty);
            }
            set
            {
                this.SetValue(BarWidthProperty, value);
            }
        }

        public Color? FillColor
        {
            get
            {
                return (Color?)this.GetValue(FillColorProperty);
            }
            set
            {
                this.SetValue(FillColorProperty, value);
            }
        }

        public bool IsStacked
        {
            get
            {
                return (bool)this.GetValue(IsStackedProperty);
            }
            set
            {
                this.SetValue(IsStackedProperty, value);
            }
        }

        public Color StrokeColor
        {
            get
            {
                return (Color)this.GetValue(StrokeColorProperty);
            }
            set
            {
                this.SetValue(StrokeColorProperty, value);
            }
        }

        public double StrokeThickness
        {
            get
            {
                return (double)this.GetValue(StrokeThicknessProperty);
            }
            set
            {
                this.SetValue(StrokeThicknessProperty, value);
            }
        }

        public string ValueField
        {
            get
            {
                return (string)this.GetValue(ValueFieldProperty);
            }
            set
            {
                this.SetValue(ValueFieldProperty, value);
            }
        }

        #endregion

        #region Public Methods

        public override OxyPlot.ISeries CreateModel()
        {
            var s = new OxyPlot.BarSeries();
            this.SynchronizeProperties(s);
            return s;
        }

        public override void SynchronizeProperties(OxyPlot.ISeries series)
        {
            base.SynchronizeProperties(series);
            var s = series as OxyPlot.BarSeries;
            if (s != null)
            {
                s.FillColor = this.FillColor.ToOxyColor();
                s.StrokeColor = this.StrokeColor.ToOxyColor();
                s.StrokeThickness = this.StrokeThickness;
                s.BarWidth = this.BarWidth;
                s.IsStacked = this.IsStacked;
                s.ValueField = this.ValueField;
                s.ItemsSource = this.ItemsSource;
            }
        }

        #endregion
    }
}