using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;

namespace OxyPlotControls
{
    /// <summary>
    /// A control that provides a combobox for selecting and managing series in an OxyPlot chart.
    /// Allows users to select, reorder, and delete series from the plot.
    /// </summary>
    public partial class SeriesSelectorControl : UserControl
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="PlotModel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlotModelProperty = DependencyProperty.Register(
            nameof(PlotModel),
            typeof(PlotModel),
            typeof(SeriesSelectorControl),
            new PropertyMetadata(null, InitializePlotModel));

        /// <summary>
        /// Gets or sets the PlotModel that this selector is bound to.
        /// </summary>
        public PlotModel PlotModel
        {
            get => (PlotModel)GetValue(PlotModelProperty);
            set => SetValue(PlotModelProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="SelectedSeries"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedSeriesProperty = DependencyProperty.Register(
            nameof(SelectedSeries),
            typeof(OxyPlot.Series.Series),
            typeof(SeriesSelectorControl),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the currently selected series.
        /// </summary>
        public OxyPlot.Series.Series SelectedSeries
        {
            get => (OxyPlot.Series.Series)GetValue(SelectedSeriesProperty);
            set => SetValue(SelectedSeriesProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ComboBoxStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ComboBoxStyleProperty = DependencyProperty.Register(
            nameof(ComboBoxStyle),
            typeof(Style),
            typeof(SeriesSelectorControl),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the style to apply to the series selection combobox.
        /// </summary>
        public Style ComboBoxStyle
        {
            get => (Style)GetValue(ComboBoxStyleProperty);
            set => SetValue(ComboBoxStyleProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ExpanderStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpanderStyleProperty = DependencyProperty.Register(
            nameof(ExpanderStyle),
            typeof(Style),
            typeof(SeriesSelectorControl));

        /// <summary>
        /// Gets or sets the style to apply to expanders in the series properties control.
        /// </summary>
        public Style ExpanderStyle
        {
            get => (Style)GetValue(ExpanderStyleProperty);
            set => SetValue(ExpanderStyleProperty, value);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesSelectorControl"/> class.
        /// </summary>
        public SeriesSelectorControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the default combobox style from resources.
        /// </summary>
        private void SetDefaultComboboxStyle()
        {
            ComboBoxStyle = (Style)FindResource("CleanComboBoxStyle");
        }

        /// <summary>
        /// Handles changes to the PlotModel property.
        /// </summary>
        private static void InitializePlotModel(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SeriesSelectorControl thisControl) return;

            thisControl.SeriesPropertyControlComboBox.ItemsSource = null;
            if (e.NewValue is not PlotModel newPlotModel) return;

            thisControl.AddHandlers();
            if (thisControl.ComboBoxStyle == null) thisControl.SetDefaultComboboxStyle();

            thisControl.SeriesPropertyControlComboBox.ItemsSource = newPlotModel.Series;

            if (newPlotModel.Series.Count > 0) thisControl.SeriesPropertyControlComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Adds required event handlers for series collection changes.
        /// </summary>
        private void AddHandlers()
        {
            if (PlotModel?.Series is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged += Series_CollectionChanged;
            }
        }

        /// <summary>
        /// Handles changes to the series collection.
        /// </summary>
        private void Series_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // New item was added
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    SeriesPropertyControlComboBox.SelectedItem = newItem;
                    SeriesPropertiesControl.Series = newItem as OxyPlot.Series.Series;
                    break;
                }
            }

            // Item was removed
            if (e.OldItems != null)
            {
                if (PlotModel != null && PlotModel.Series.Count == 0)
                {
                    // SeriesPropertiesControl.HideExpanders();
                }
                else
                {
                    SeriesPropertyControlComboBox.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Handles selection changes in the series combobox.
        /// </summary>
        private void SeriesPropertyControlComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Early exit if nothing is selected
            if (SeriesPropertyControlComboBox.SelectedItem == null) return;

            var seriesToSelect = SeriesPropertyControlComboBox.SelectedItem as OxyPlot.Series.Series;
            if (seriesToSelect == null) return;
            SeriesPropertiesControl.Series = seriesToSelect;
        }

        /// <summary>
        /// Handles the click event for moving a series up in the order.
        /// </summary>
        private void MoveSeriesUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlotModel == null) return;
            if (sender is not Button btn) return;
            if (btn.DataContext is not OxyPlot.Series.Series seriesToMoveUp) return;

            int index = PlotModel.Series.IndexOf(seriesToMoveUp);
            if (index == 0 || index == -1) return;

            // Swap series positions
            var oldSeries = new System.Collections.Generic.List<OxyPlot.Series.Series>(PlotModel.Series);
            PlotModel.Series.Clear();
            oldSeries[index] = oldSeries[index - 1];
            oldSeries[index - 1] = seriesToMoveUp;
            for (int i = 0; i < oldSeries.Count; i++)
            {
                PlotModel.Series.Add(oldSeries[i]);
            }

            SeriesPropertyControlComboBox.SelectedIndex = index + 1;
            PlotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// Handles the click event for moving a series down in the order.
        /// </summary>
        private void MoveSeriesDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlotModel == null) return;
            if (sender is not Button btn) return;
            if (btn.DataContext is not OxyPlot.Series.Series seriesToMoveDown) return;

            int index = PlotModel.Series.IndexOf(seriesToMoveDown);
            if (index == PlotModel.Series.Count - 1 || index == -1) return;

            var oldSeries = new System.Collections.Generic.List<OxyPlot.Series.Series>(PlotModel.Series);
            PlotModel.Series.Clear();
            oldSeries[index] = oldSeries[index + 1];
            oldSeries[index + 1] = seriesToMoveDown;
            for (int i = 0; i < oldSeries.Count; i++)
            {
                PlotModel.Series.Add(oldSeries[i]);
            }

            SeriesPropertyControlComboBox.SelectedIndex = index + 1;
            PlotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// Handles the click event for deleting a series.
        /// </summary>
        private void DeleteSeriesButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlotModel == null) return;
            if (sender is not Button btn) return;
            if (btn.DataContext is not OxyPlot.Series.Series seriesToDelete) return;

            int index = PlotModel.Series.IndexOf(seriesToDelete);
            if (index == SeriesPropertyControlComboBox.SelectedIndex)
            {
                if (index > 0) index -= 1;
                if (PlotModel.Series.Count == 1) index = -1;
            }
            PlotModel.Series.Remove(seriesToDelete);
            SeriesPropertyControlComboBox.SelectedIndex = index;
            PlotModel.InvalidatePlot(false);

            SeriesPropertiesControl.CloseExpanders();
        }
    }
}
