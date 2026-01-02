using System;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Series;
using OxyPlotControls.Controls.Base;
using OxyPlotControls.Factories;
using OxyPlotControls.Managers;

namespace OxyPlotControls.Controls.Selectors;

/// <summary>
/// Control for selecting and managing series in a PlotModel.
/// Provides series selection, add/delete/reorder functionality, and property editing.
/// </summary>
public partial class SeriesSelectorControl : UserControl
{
    private SeriesManager? _seriesManager;

    public SeriesSelectorControl()
    {
        InitializeComponent();
    }

    #region Dependency Properties

    public static readonly DependencyProperty ModelProperty =
        DependencyProperty.Register(
            nameof(Model),
            typeof(PlotModel),
            typeof(SeriesSelectorControl),
            new PropertyMetadata(null, OnModelChanged));

    public PlotModel? Model
    {
        get => (PlotModel?)GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SeriesSelectorControl control)
        {
            control.OnModelChanged(e.NewValue as PlotModel);
        }
    }

    #endregion

    private void OnModelChanged(PlotModel? model)
    {
        if (model == null)
        {
            SeriesComboBox.ItemsSource = null;
            SeriesPropertyEditor.Content = null;
            _seriesManager = null;
            return;
        }

        _seriesManager = new SeriesManager(model);
        SeriesComboBox.ItemsSource = model.Series;

        if (model.Series.Count > 0)
        {
            SeriesComboBox.SelectedIndex = 0;
        }
    }

    private void SeriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SeriesComboBox.SelectedItem is Series series)
        {
            var editor = SeriesControlFactory.CreateControl(series);

            if (editor is SeriesControlBase seriesControl)
            {
                seriesControl.Series = series;
                seriesControl.Model = Model;
            }

            SeriesPropertyEditor.Content = editor;
        }
        else
        {
            SeriesPropertyEditor.Content = null;
        }
    }

    #region Series Management

    private void MoveUp_Click(object sender, RoutedEventArgs e)
    {
        if (SeriesComboBox.SelectedItem is Series series && _seriesManager != null)
        {
            if (_seriesManager.MoveSeriesUp(series))
            {
                RefreshSelection(series);
            }
        }
    }

    private void MoveDown_Click(object sender, RoutedEventArgs e)
    {
        if (SeriesComboBox.SelectedItem is Series series && _seriesManager != null)
        {
            if (_seriesManager.MoveSeriesDown(series))
            {
                RefreshSelection(series);
            }
        }
    }

    private void DeleteSeries_Click(object sender, RoutedEventArgs e)
    {
        if (SeriesComboBox.SelectedItem is Series series && Model != null)
        {
            var result = MessageBox.Show(
                $"Delete series '{series.Title}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Model.Series.Remove(series);
                Model.InvalidatePlot(false);
            }
        }
    }

    private void AddSeries_Click(object sender, RoutedEventArgs e)
    {
        // Show context menu
        if (sender is Button button && button.ContextMenu != null)
        {
            button.ContextMenu.PlacementTarget = button;
            button.ContextMenu.IsOpen = true;
        }
    }

    private void AddLineSeries_Click(object sender, RoutedEventArgs e) => AddNewSeries(new LineSeries { Title = "Line Series" });
    private void AddBarSeries_Click(object sender, RoutedEventArgs e) => AddNewSeries(new BarSeries { Title = "Bar Series" });
    private void AddColumnSeries_Click(object sender, RoutedEventArgs e) => AddNewSeries(new ColumnSeries { Title = "Column Series" });
    private void AddScatterSeries_Click(object sender, RoutedEventArgs e) => AddNewSeries(new ScatterSeries { Title = "Scatter Series" });
    private void AddAreaSeries_Click(object sender, RoutedEventArgs e) => AddNewSeries(new AreaSeries { Title = "Area Series" });
    private void AddPieSeries_Click(object sender, RoutedEventArgs e) => AddNewSeries(new PieSeries { Title = "Pie Series" });

    private void AddNewSeries(Series series)
    {
        if (Model != null)
        {
            Model.Series.Add(series);
            Model.InvalidatePlot(false);
            SeriesComboBox.SelectedItem = series;
        }
    }

    private void RefreshSelection(Series series)
    {
        // Refresh ComboBox to update display
        var temp = SeriesComboBox.ItemsSource;
        SeriesComboBox.ItemsSource = null;
        SeriesComboBox.ItemsSource = temp;
        SeriesComboBox.SelectedItem = series;
    }

    #endregion
}
