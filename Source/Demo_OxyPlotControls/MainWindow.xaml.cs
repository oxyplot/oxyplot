using System;
using System.IO;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlotControls.Serialization;

namespace Demo_OxyPlotControls;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Random _random = new();

    public static readonly DependencyProperty PlotModelProperty =
        DependencyProperty.Register(
            nameof(PlotModel),
            typeof(PlotModel),
            typeof(MainWindow),
            new PropertyMetadata(null));

    public PlotModel PlotModel
    {
        get => (PlotModel)GetValue(PlotModelProperty);
        set => SetValue(PlotModelProperty, value);
    }

    public MainWindow()
    {
        InitializeComponent();
        InitializePlot();
    }

    private void InitializePlot()
    {
        PlotModel = new PlotModel
        {
            Title = "OxyPlot Controls Demo",
            Subtitle = "Modern C# Implementation",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        // Add default axes
        PlotModel.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "X Axis",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        PlotModel.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Y Axis",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        // Add sample data
        AddLineSeries_Click(this, new RoutedEventArgs());

        UpdateStatus("Plot initialized successfully");
    }

    private void AddLineSeries_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var series = new LineSeries
            {
                Title = $"Line Series {PlotModel.Series.Count + 1}",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                Color = OxyColor.FromRgb(
                    (byte)_random.Next(256),
                    (byte)_random.Next(256),
                    (byte)_random.Next(256))
            };

            // Generate random data
            for (int i = 0; i < 50; i++)
            {
                series.Points.Add(new DataPoint(i, _random.NextDouble() * 100));
            }

            PlotModel.Series.Add(series);
            PlotModel.InvalidatePlot(true);

            UpdateStatus($"Added {series.Title}");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error: {ex.Message}", isError: true);
        }
    }

    private void AddBarSeries_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var series = new BarSeries
            {
                Title = $"Bar Series {PlotModel.Series.Count + 1}",
                FillColor = OxyColor.FromRgb(
                    (byte)_random.Next(256),
                    (byte)_random.Next(256),
                    (byte)_random.Next(256))
            };

            // Generate random data
            for (int i = 0; i < 5; i++)
            {
                series.Items.Add(new BarItem { Value = _random.NextDouble() * 100 });
            }

            PlotModel.Series.Add(series);
            PlotModel.InvalidatePlot(true);

            UpdateStatus($"Added {series.Title}");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error: {ex.Message}", isError: true);
        }
    }

    private void ClearSeries_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            PlotModel.Series.Clear();
            PlotModel.InvalidatePlot(true);
            UpdateStatus("All series cleared");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error: {ex.Message}", isError: true);
        }
    }

    private void SaveSettings_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var dialog = new SaveFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                DefaultExt = ".xml",
                FileName = "PlotSettings.xml"
            };

            if (dialog.ShowDialog() == true)
            {
                var xml = PlotModelSerializer.Serialize(PlotModel);
                xml.Save(dialog.FileName);
                UpdateStatus($"Settings saved to {Path.GetFileName(dialog.FileName)}");
            }
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error saving: {ex.Message}", isError: true);
        }
    }

    private void LoadSettings_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var dialog = new OpenFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                DefaultExt = ".xml"
            };

            if (dialog.ShowDialog() == true)
            {
                var xml = XElement.Load(dialog.FileName);
                var model = PlotModelSerializer.Deserialize(xml);

                if (model != null)
                {
                    // Preserve axes and series from current model
                    model.Axes.Clear();
                    model.Series.Clear();

                    foreach (var axis in PlotModel.Axes)
                        model.Axes.Add(axis);

                    foreach (var series in PlotModel.Series)
                        model.Series.Add(series);

                    PlotModel = model;
                    UpdateStatus($"Settings loaded from {Path.GetFileName(dialog.FileName)}");
                }
                else
                {
                    UpdateStatus("Failed to load settings", isError: true);
                }
            }
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading: {ex.Message}", isError: true);
        }
    }

    private void UpdateStatus(string message, bool isError = false)
    {
        StatusText.Text = $"{DateTime.Now:HH:mm:ss} - {message}";
        StatusText.Foreground = isError ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.Green;
    }
}
