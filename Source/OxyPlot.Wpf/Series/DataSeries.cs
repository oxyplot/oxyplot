using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// Base class for data series
    /// todo: this should listen to collection changes
    /// </summary>
    public abstract class DataSeries : ItemsControl
    {
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            OnDataChanged();
        }

        protected static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataSeries)d).OnDataChanged();
        }

        protected void OnDataChanged()
        {
            // post event to  parent
        }

        protected static void VisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataSeries)d).OnVisualChanged();
        }

        protected void OnVisualChanged()
        {
            // post event to  parent
        }

        public string DataFieldX
        {
            get { return (string)GetValue(DataFieldXProperty); }
            set { SetValue(DataFieldXProperty, value); }
        }

        public static readonly DependencyProperty DataFieldXProperty =
            DependencyProperty.Register("DataFieldX", typeof(string), typeof(DataSeries), new UIPropertyMetadata("X", DataChanged));

        public string DataFieldY
        {
            get { return (string)GetValue(DataFieldYProperty); }
            set { SetValue(DataFieldYProperty, value); }
        }

        public static readonly DependencyProperty DataFieldYProperty =
            DependencyProperty.Register("DataFieldY", typeof(string), typeof(DataSeries), new UIPropertyMetadata("Y", DataChanged));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DataSeries), new UIPropertyMetadata(null, VisualChanged));

        public System.Windows.Media.Color Color
        {
            get { return (System.Windows.Media.Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(System.Windows.Media.Color), typeof(DataSeries), new UIPropertyMetadata(System.Windows.Media.Colors.Red, VisualChanged));

        public abstract OxyPlot.DataPointSeries CreateModel();

        public virtual void SynchronizeProperties(OxyPlot.DataPointSeries s)
        {
            s.ItemsSource = ItemsSource;
            s.DataFieldX = DataFieldX;
            s.DataFieldY = DataFieldY;
            s.Title = Title;
        }
    }
}