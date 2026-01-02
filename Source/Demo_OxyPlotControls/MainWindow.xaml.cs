using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlotControls.Serialization;

namespace Demo_OxyPlotControls;

/// <summary>
/// Comprehensive demonstration of OxyPlot Controls with all series types and axis types.
/// </summary>
/// <remarks>
/// This demo showcases:
/// - 10 different series types (Line, Scatter, Area, Histogram, Column, Bar, BoxPlot, HeatMap, ScatterError, DateTime)
/// - Multiple axis types (Linear, Category, DateTime, LinearColorAxis)
/// - Interactive property editing via OxyplotPropertiesControl
/// - Full toolbar functionality (Pan, Zoom, Annotations, Export)
/// - Save/Load plot settings
/// </remarks>
public partial class MainWindow : Window
{
    private readonly Random _random = new(314); // Fixed seed for reproducible demos

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
        // Default demo will be set by ComboBox SelectedIndex
    }

    #region Data Generation Helpers

    /// <summary>
    /// Creates a normal distribution curve.
    /// </summary>
    /// <param name="x0">Start X value</param>
    /// <param name="x1">End X value</param>
    /// <param name="mean">Mean (center) of distribution</param>
    /// <param name="variance">Variance (spread) of distribution</param>
    /// <param name="n">Number of points to generate</param>
    /// <returns>List of data points forming a normal distribution</returns>
    private List<DataPoint> CreateNormalDistribution(double x0, double x1, double mean, double variance, int n = 1000)
    {
        var result = new List<DataPoint>();
        for (int i = 0; i < n; i++)
        {
            double x = x0 + ((x1 - x0) * i / (n - 1));
            double f = 1.0 / Math.Sqrt(2 * Math.PI * variance) *
                       Math.Exp(-(x - mean) * (x - mean) / 2 / variance);
            result.Add(new DataPoint(x, f));
        }
        return result;
    }

    /// <summary>
    /// Generates sample time series data for demonstration.
    /// </summary>
    private List<DataPoint> CreateTimeSeriesData()
    {
        var result = new List<DataPoint>();
        var startDate = new DateTime(2024, 1, 1);

        // Generate 365 days of data with seasonal pattern
        for (int i = 0; i < 365; i++)
        {
            var date = startDate.AddDays(i);
            var x = DateTimeAxis.ToDouble(date);

            // Seasonal pattern + random noise
            var seasonal = 50 + 30 * Math.Sin(2 * Math.PI * i / 365.0);
            var noise = (_random.NextDouble() - 0.5) * 10;
            var y = seasonal + noise;

            result.Add(new DataPoint(x, y));
        }

        return result;
    }

    #endregion

    #region Series Demonstration Methods

    /// <summary>
    /// Demonstrates Line Series with two normal distribution curves.
    /// </summary>
    private void LineSeries_Create()
    {
        var model = new PlotModel
        {
            Title = "Line Series",
            Subtitle = "Two normal distributions with different parameters",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        // Add axes
        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "X Axis",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Y Axis",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        // Line Series 1: Wide distribution
        var series1 = new LineSeries
        {
            Title = "Distribution 1 (σ²=1.0)",
            Color = OxyColors.Blue,
            StrokeThickness = 2,
            MarkerType = MarkerType.None
        };
        series1.Points.AddRange(CreateNormalDistribution(-5, 5, 0, 1.0, 200));
        model.Series.Add(series1);

        // Line Series 2: Narrow distribution
        var series2 = new LineSeries
        {
            Title = "Distribution 2 (σ²=0.25)",
            Color = OxyColors.Red,
            StrokeThickness = 2,
            MarkerType = MarkerType.None
        };
        series2.Points.AddRange(CreateNormalDistribution(-2, 2, 0, 0.25, 200));
        model.Series.Add(series2);

        PlotModel = model;
        UpdateStatus("Line Series demonstration loaded");
    }

    /// <summary>
    /// Demonstrates Scatter Series with color-coded points and LinearColorAxis.
    /// </summary>
    private void ScatterSeries_Create()
    {
        var model = new PlotModel
        {
            Title = "Scatter Series",
            Subtitle = "Random points with size and color values",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        // Add axes
        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "X Axis",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Y Axis",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        // Add LinearColorAxis for color mapping
        var colorAxis = new LinearColorAxis
        {
            Position = AxisPosition.Right,
            Title = "Value",
            Palette = OxyPalettes.Jet(200),
            Minimum = 50,
            Maximum = 500
        };
        model.Axes.Add(colorAxis);

        // Scatter Series with random data
        var scatterSeries = new ScatterSeries
        {
            Title = "Random Scatter Points",
            MarkerType = MarkerType.Circle
        };

        for (int i = 0; i < 100; i++)
        {
            var x = _random.NextDouble();
            var y = _random.NextDouble();
            var size = _random.Next(5, 15);
            var colorValue = _random.Next(50, 500);

            scatterSeries.Points.Add(new ScatterPoint(x, y, size, colorValue));
        }

        model.Series.Add(scatterSeries);
        PlotModel = model;
        UpdateStatus("Scatter Series demonstration loaded");
    }

    /// <summary>
    /// Demonstrates Area Series.
    /// </summary>
    private void AreaSeries_Create()
    {
        var model = new PlotModel
        {
            Title = "Area Series",
            Subtitle = "Filled area under a normal distribution curve",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "X Axis",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Y Axis",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        var areaSeries = new AreaSeries
        {
            Title = "Area Under Curve",
            Color = OxyColors.Green,
            Fill = OxyColor.FromAColor(100, OxyColors.Green),
            StrokeThickness = 2
        };

        var points = CreateNormalDistribution(-5, 5, 0, 1.0, 200);
        foreach (var point in points)
        {
            areaSeries.Points.Add(point);
        }

        model.Series.Add(areaSeries);
        PlotModel = model;
        UpdateStatus("Area Series demonstration loaded");
    }

    /// <summary>
    /// Demonstrates Histogram Series.
    /// </summary>
    private void HistogramSeries_Create()
    {
        var model = new PlotModel
        {
            Title = "Histogram Series",
            Subtitle = "Distribution of values in bins",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Value Range",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Frequency",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        var histogramSeries = new HistogramSeries
        {
            Title = "Value Distribution",
            FillColor = OxyColors.SkyBlue,
            StrokeColor = OxyColors.Navy,
            StrokeThickness = 2
        };

        histogramSeries.Items.Add(new HistogramItem(0, 200, 400));
        histogramSeries.Items.Add(new HistogramItem(200, 400, 100));
        histogramSeries.Items.Add(new HistogramItem(400, 600, 800));
        histogramSeries.Items.Add(new HistogramItem(600, 800, 2000));

        model.Series.Add(histogramSeries);
        PlotModel = model;
        UpdateStatus("Histogram Series demonstration loaded");
    }

    /// <summary>
    /// Demonstrates Column Series with CategoryAxis.
    /// </summary>
    private void ColumnSeries_Create()
    {
        var model = new PlotModel
        {
            Title = "Column Series",
            Subtitle = "Test scores by subject",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        var categoryAxis = new CategoryAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Subject",
            ItemsSource = new[] { "Math", "Science", "English", "History" }
        };
        model.Axes.Add(categoryAxis);

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Score",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        var columnSeries = new ColumnSeries
        {
            Title = "Test Scores",
            FillColor = OxyColors.CornflowerBlue
        };

        columnSeries.Items.Add(new ColumnItem(85, 0));
        columnSeries.Items.Add(new ColumnItem(92, 1));
        columnSeries.Items.Add(new ColumnItem(78, 2));
        columnSeries.Items.Add(new ColumnItem(88, 3));

        model.Series.Add(columnSeries);
        PlotModel = model;
        UpdateStatus("Column Series demonstration loaded");
    }

    /// <summary>
    /// Demonstrates Bar Series with CategoryAxis.
    /// </summary>
    private void BarSeries_Create()
    {
        var model = new PlotModel
        {
            Title = "Bar Series",
            Subtitle = "Cake popularity percentage",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        var categoryAxis = new CategoryAxis
        {
            Position = AxisPosition.Left,
            Title = "Cake Type",
            ItemsSource = new[] { "Apple Cake", "Baumkuchen", "Bundt Cake", "Chocolate Cake", "Carrot Cake" }
        };
        model.Axes.Add(categoryAxis);

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Popularity %",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        var barSeries = new BarSeries
        {
            Title = "Cake Popularity",
            FillColor = OxyColors.SandyBrown,
            LabelPlacement = LabelPlacement.Inside,
            LabelFormatString = "{0:.0}%"
        };

        // Generate random popularity percentages
        var values = new double[5];
        for (int i = 0; i < 5; i++)
            values[i] = _random.NextDouble();

        var sum = values.Sum();

        for (int i = 0; i < 5; i++)
            barSeries.Items.Add(new BarItem(values[i] / sum * 100));

        model.Series.Add(barSeries);
        PlotModel = model;
        UpdateStatus("Bar Series demonstration loaded");
    }

    /// <summary>
    /// Demonstrates BoxPlot Series with CategoryAxis and outliers.
    /// </summary>
    private void BoxPlotSeries_Create()
    {
        var model = new PlotModel
        {
            Title = "Box Plot Series",
            Subtitle = "Statistical distribution with outliers",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        var categoryAxis = new CategoryAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Category",
            ItemsSource = new[] { "Math", "Science", "English", "History" }
        };
        model.Axes.Add(categoryAxis);

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Score",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        var boxPlotSeries = new BoxPlotSeries
        {
            Title = "Score Distribution",
            Fill = OxyColors.LightBlue,
            StrokeThickness = 2,
            BoxWidth = 0.4
        };

        // Add box plot items with outliers
        boxPlotSeries.Items.Add(new BoxPlotItem(0, 3, 5, 7, 20, 30)
        {
            Outliers = new List<double> { 2, 50 }
        });

        boxPlotSeries.Items.Add(new BoxPlotItem(1, 4, 12, 14, 30, 40)
        {
            Outliers = new List<double> { 60 }
        });

        boxPlotSeries.Items.Add(new BoxPlotItem(2, 7, 10, 12, 25, 35));
        boxPlotSeries.Items.Add(new BoxPlotItem(3, 12, 15, 17, 30, 40));

        model.Series.Add(boxPlotSeries);
        PlotModel = model;
        UpdateStatus("BoxPlot Series demonstration loaded");
    }

    /// <summary>
    /// Demonstrates HeatMap Series with LinearColorAxis and 2D data.
    /// </summary>
    private void HeatMapSeries_Create()
    {
        var model = new PlotModel
        {
            Title = "Heat Map Series",
            Subtitle = "2D Gaussian distribution",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "X",
            MajorGridlineStyle = LineStyle.Solid
        });

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Y",
            MajorGridlineStyle = LineStyle.Solid
        });

        // Add color axis
        var colorAxis = new LinearColorAxis
        {
            Position = AxisPosition.Right,
            Title = "Intensity",
            Palette = OxyPalettes.Jet(256)
        };
        model.Axes.Add(colorAxis);

        // Generate 1D normal distribution
        var singleData = new double[100];
        for (int x = 0; x < 100; x++)
        {
            singleData[x] = Math.Exp(-0.5 * Math.Pow((x - 50.0) / 20.0, 2));
        }

        // Generate 2D normal distribution by multiplying 1D distributions
        var data = new double[100, 100];
        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                data[y, x] = singleData[x] * singleData[(y + 30) % 100] * 100;
            }
        }

        var heatMapSeries = new HeatMapSeries
        {
            X0 = 0,
            X1 = 99,
            Y0 = 0,
            Y1 = 99,
            Interpolate = true,
            Data = data,
            RenderMethod = HeatMapRenderMethod.Bitmap
        };

        model.Series.Add(heatMapSeries);
        PlotModel = model;
        UpdateStatus("HeatMap Series demonstration loaded");
    }

    /// <summary>
    /// Demonstrates ScatterError Series with error bars and LinearColorAxis.
    /// </summary>
    private void ScatterErrorSeries_Create()
    {
        var model = new PlotModel
        {
            Title = "Scatter Error Series",
            Subtitle = "Points with X and Y error bars",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "X Axis",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Y Axis",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        // Add color axis
        var colorAxis = new LinearColorAxis
        {
            Position = AxisPosition.Right,
            Title = "Value",
            Palette = OxyPalettes.Jet(200)
        };
        model.Axes.Add(colorAxis);

        var scatterErrorSeries = new ScatterErrorSeries
        {
            Title = "Measurements with Uncertainty",
            MarkerType = MarkerType.Circle,
            ErrorBarColor = OxyColors.Black,
            ErrorBarStrokeThickness = 1
        };

        for (int i = 0; i < 30; i++)
        {
            var x = _random.NextDouble();
            var y = _random.NextDouble();
            var size = _random.Next(5, 15);
            var colorValue = _random.Next(100, 1000);

            var errorX = x * 0.1;
            var errorY = y * 0.1;

            scatterErrorSeries.Points.Add(new ScatterErrorPoint(
                x, y,
                x - errorX, x + errorX,
                y - errorY, y + errorY,
                size, colorValue));
        }

        model.Series.Add(scatterErrorSeries);
        PlotModel = model;
        UpdateStatus("ScatterError Series demonstration loaded");
    }

    /// <summary>
    /// Demonstrates DateTime Series with DateTimeAxis.
    /// </summary>
    private void DateTimeSeries_Create()
    {
        var model = new PlotModel
        {
            Title = "DateTime Series",
            Subtitle = "Time series data over one year",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        var dateTimeAxis = new DateTimeAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Date",
            StringFormat = "MMM yyyy",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot,
            IntervalType = DateTimeIntervalType.Months
        };
        model.Axes.Add(dateTimeAxis);

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Temperature (°F)",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        var lineSeries = new LineSeries
        {
            Title = "Seasonal Temperature",
            Color = OxyColors.OrangeRed,
            StrokeThickness = 2,
            MarkerType = MarkerType.None
        };

        lineSeries.Points.AddRange(CreateTimeSeriesData());
        model.Series.Add(lineSeries);

        PlotModel = model;
        UpdateStatus("DateTime Series demonstration loaded");
    }

    #endregion

    #region UI Event Handlers

    private void DemoTypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (DemoTypeComboBox.SelectedItem is not System.Windows.Controls.ComboBoxItem item)
            return;

        try
        {
            switch (item.Content.ToString())
            {
                case "Line Series":
                    LineSeries_Create();
                    break;
                case "Scatter Series":
                    ScatterSeries_Create();
                    break;
                case "Area Series":
                    AreaSeries_Create();
                    break;
                case "Histogram Series":
                    HistogramSeries_Create();
                    break;
                case "Column Series":
                    ColumnSeries_Create();
                    break;
                case "Bar Series":
                    BarSeries_Create();
                    break;
                case "Box Plot Series":
                    BoxPlotSeries_Create();
                    break;
                case "Heat Map Series":
                    HeatMapSeries_Create();
                    break;
                case "Scatter Error Series":
                    ScatterErrorSeries_Create();
                    break;
                case "DateTime Series":
                    DateTimeSeries_Create();
                    break;
                default:
                    UpdateStatus($"Unknown demo type: {item.Content}", isError: true);
                    break;
            }
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading demo: {ex.Message}", isError: true);
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
        StatusText.Foreground = isError
            ? System.Windows.Media.Brushes.Red
            : System.Windows.Media.Brushes.Green;
    }

    #endregion
}
