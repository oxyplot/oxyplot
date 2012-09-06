namespace OxyPlot.Wpf
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Abstract base class for series that use X and Y axes.
    /// </summary>
    public abstract class PlotSeriesBase : ItemsControl, ISeries
    {
        #region Constants and Fields

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color?), typeof(PlotSeriesBase), new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(PlotSeriesBase), new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty TrackerFormatStringProperty =
            DependencyProperty.Register(
                "TrackerFormatString",
                typeof(string),
                typeof(PlotSeriesBase),
                new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty TrackerKeyProperty = DependencyProperty.Register(
            "TrackerKey", typeof(string), typeof(PlotSeriesBase), new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty XAxisKeyProperty = DependencyProperty.Register(
            "XAxisKey", typeof(string), typeof(PlotSeriesBase), new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty YAxisKeyProperty = DependencyProperty.Register(
            "YAxisKey", typeof(string), typeof(PlotSeriesBase), new UIPropertyMetadata(null, VisualChanged));

        #endregion

        #region Public Properties

        public Color? Color
        {
            get
            {
                return (Color?)this.GetValue(ColorProperty);
            }
            set
            {
                this.SetValue(ColorProperty, value);
            }
        }

        public string Title
        {
            get
            {
                return (string)this.GetValue(TitleProperty);
            }
            set
            {
                this.SetValue(TitleProperty, value);
            }
        }

        public string TrackerFormatString
        {
            get
            {
                return (string)this.GetValue(TrackerFormatStringProperty);
            }
            set
            {
                this.SetValue(TrackerFormatStringProperty, value);
            }
        }

        public string TrackerKey
        {
            get
            {
                return (string)this.GetValue(TrackerKeyProperty);
            }
            set
            {
                this.SetValue(TrackerKeyProperty, value);
            }
        }

        public string XAxisKey
        {
            get
            {
                return (string)this.GetValue(XAxisKeyProperty);
            }
            set
            {
                this.SetValue(XAxisKeyProperty, value);
            }
        }

        public string YAxisKey
        {
            get
            {
                return (string)this.GetValue(YAxisKeyProperty);
            }
            set
            {
                this.SetValue(YAxisKeyProperty, value);
            }
        }

        #endregion

        #region Public Methods

        public abstract OxyPlot.ISeries CreateModel();

        protected virtual void SynchronizeProperties(OxyPlot.ISeries series)
        {
            var s = series as OxyPlot.PlotSeriesBase;
            s.ItemsSource = this.ItemsSource;
            s.Background = this.Background.ToOxyColor();
            s.Title = this.Title;
            s.TrackerFormatString = this.TrackerFormatString;
            s.TrackerKey = this.TrackerKey;
            s.TrackerFormatString = this.TrackerFormatString;
            s.XAxisKey = this.XAxisKey;
            s.YAxisKey = this.YAxisKey;
        }

        #endregion

        #region Methods

        protected static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotSeriesBase)d).OnDataChanged();
        }

        protected static void VisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotSeriesBase)d).OnVisualChanged();
        }

        protected void OnDataChanged()
        {
            var pc = this.Parent as IPlotControl;
            if (pc != null)
            {
                // pc.UpdateModel();
            }
            this.OnVisualChanged();
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            this.OnDataChanged();
        }

        protected void OnVisualChanged()
        {
            var pc = this.Parent as IPlotControl;
            if (pc != null)
            {
                pc.InvalidatePlot();
            }
        }

        #endregion
    }
}