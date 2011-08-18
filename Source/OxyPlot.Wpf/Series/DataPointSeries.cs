namespace OxyPlot.Wpf
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Base class for data series
    /// </summary>
    public abstract class DataPointSeries : PlotSeriesBase
    {
        #region Constants and Fields

        public static readonly DependencyProperty DataFieldXProperty = DependencyProperty.Register(
            "DataFieldX", typeof(string), typeof(DataPointSeries), new UIPropertyMetadata("X", DataChanged));

        public static readonly DependencyProperty DataFieldYProperty = DependencyProperty.Register(
            "DataFieldY", typeof(string), typeof(DataPointSeries), new UIPropertyMetadata("Y", DataChanged));

        #endregion

        #region Public Properties

        public string DataFieldX
        {
            get
            {
                return (string)this.GetValue(DataFieldXProperty);
            }
            set
            {
                this.SetValue(DataFieldXProperty, value);
            }
        }

        public string DataFieldY
        {
            get
            {
                return (string)this.GetValue(DataFieldYProperty);
            }
            set
            {
                this.SetValue(DataFieldYProperty, value);
            }
        }

        #endregion

        #region Public Methods

        protected override void SynchronizeProperties(OxyPlot.ISeries series)
        {
            base.SynchronizeProperties(series);
            var s = series as OxyPlot.DataPointSeries;
            s.ItemsSource = this.ItemsSource;
            s.DataFieldX = this.DataFieldX;
            s.DataFieldY = this.DataFieldY;
        }

        #endregion

        #region Methods

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            this.OnDataChanged();
        }

        #endregion
    }
}