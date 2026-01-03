using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlotControls.Serialization;

namespace Demo_OxyPlotControls;

/// <summary>
/// Comprehensive demonstration of OxyPlotControls with all series types, axis types, and interactive features.
/// </summary>
/// <remarks>
/// <para>
/// This demo showcases the full capabilities of the OxyPlotControls library including:
/// </para>
/// <list type="bullet">
///   <item><description>19 different demo configurations covering all major series types</description></item>
///   <item><description>Both direct data and data-bound series patterns</description></item>
///   <item><description>Multiple axis types: Linear, Logarithmic, Category, DateTime, LinearColorAxis</description></item>
///   <item><description>Interactive property editing via OxyplotPropertiesControl</description></item>
///   <item><description>Full toolbar functionality: Pan, Zoom, Annotations, Export</description></item>
///   <item><description>Save/Load plot settings with XML serialization</description></item>
///   <item><description>Annotations demonstration with various annotation types</description></item>
/// </list>
/// </remarks>
public partial class MainWindow : Window
{
    #region Fields

    /// <summary>
    /// Random number generator with fixed seed for reproducible demos.
    /// </summary>
    private readonly Random _random = new(314);

    /// <summary>
    /// Sample data source for data-bound series demonstrations.
    /// </summary>
    private ObservableCollection<DataItem> _dataItems = new();

    /// <summary>
    /// Sample category data source for bar/column charts.
    /// </summary>
    private ObservableCollection<CategoryDataItem> _categoryItems = new();

    /// <summary>
    /// Indicates whether the window has been fully loaded.
    /// </summary>
    private bool _isLoaded;

    #endregion

    #region Dependency Properties

    /// <summary>
    /// Identifies the <see cref="PlotModel"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PlotModelProperty =
        DependencyProperty.Register(
            nameof(PlotModel),
            typeof(PlotModel),
            typeof(MainWindow),
            new PropertyMetadata(null, OnPlotModelChanged));

    /// <summary>
    /// Gets or sets the current PlotModel being displayed.
    /// </summary>
    /// <value>The OxyPlot PlotModel instance.</value>
    public PlotModel PlotModel
    {
        get => (PlotModel)GetValue(PlotModelProperty);
        set => SetValue(PlotModelProperty, value);
    }

    /// <summary>
    /// Called when the PlotModel property changes.
    /// </summary>
    /// <param name="d">The dependency object.</param>
    /// <param name="e">The event arguments.</param>
    private static void OnPlotModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is MainWindow window && e.NewValue is PlotModel model)
        {
            window.UpdatePlotStats(model);
        }
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        InitializeSampleData();
        Loaded += MainWindow_Loaded;
    }

    /// <summary>
    /// Handles the Loaded event of the MainWindow.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments.</param>
    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _isLoaded = true;
    }

    #endregion

    #region Sample Data Classes

    /// <summary>
    /// Represents a single data point for data-bound series.
    /// </summary>
    public class DataItem : INotifyPropertyChanged
    {
        private double _x;
        private double _y;
        private double _size;
        private double _value;

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        public double X
        {
            get => _x;
            set { _x = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        public double Y
        {
            get => _y;
            set { _y = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the point size (for scatter series).
        /// </summary>
        public double Size
        {
            get => _size;
            set { _size = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the color value (for color-mapped series).
        /// </summary>
        public double Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Represents a category data item for bar/column charts.
    /// </summary>
    public class CategoryDataItem : INotifyPropertyChanged
    {
        private string _category = string.Empty;
        private double _value;
        private double _value2;

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the primary value.
        /// </summary>
        public double Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the secondary value (for grouped charts).
        /// </summary>
        public double Value2
        {
            get => _value2;
            set { _value2 = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    #endregion

    #region Data Initialization

    /// <summary>
    /// Initializes sample data collections for data-bound demos.
    /// </summary>
    private void InitializeSampleData()
    {
        // Initialize scatter/line data items
        _dataItems = new ObservableCollection<DataItem>();
        for (int i = 0; i < 100; i++)
        {
            _dataItems.Add(new DataItem
            {
                X = _random.NextDouble() * 10,
                Y = _random.NextDouble() * 10,
                Size = _random.Next(5, 20),
                Value = _random.Next(50, 500)
            });
        }

        // Initialize category data items
        _categoryItems = new ObservableCollection<CategoryDataItem>
        {
            new() { Category = "Mathematics", Value = 85, Value2 = 78 },
            new() { Category = "Science", Value = 92, Value2 = 88 },
            new() { Category = "English", Value = 78, Value2 = 82 },
            new() { Category = "History", Value = 88, Value2 = 85 },
            new() { Category = "Art", Value = 95, Value2 = 90 }
        };
    }

    #endregion

    #region Data Generation Helpers

    /// <summary>
    /// Creates a normal (Gaussian) distribution curve.
    /// </summary>
    /// <param name="x0">Start X value.</param>
    /// <param name="x1">End X value.</param>
    /// <param name="mean">Mean (center) of distribution.</param>
    /// <param name="variance">Variance (spread) of distribution.</param>
    /// <param name="n">Number of points to generate (must be >= 2).</param>
    /// <returns>List of data points forming a normal distribution.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when n is less than 2.</exception>
    private static List<DataPoint> CreateNormalDistribution(double x0, double x1, double mean, double variance, int n = 1000)
    {
        if (n < 2)
            throw new ArgumentOutOfRangeException(nameof(n), "Number of points must be at least 2");

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
    /// Generates sample time series data with seasonal pattern.
    /// </summary>
    /// <returns>List of data points representing temperature over one year.</returns>
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

    /// <summary>
    /// Generates exponential growth data for logarithmic axis demonstration.
    /// </summary>
    /// <returns>List of data points showing exponential growth.</returns>
    private static List<DataPoint> CreateExponentialData()
    {
        var result = new List<DataPoint>();
        for (int i = 1; i <= 100; i++)
        {
            double x = i;
            double y = Math.Pow(1.1, i);
            result.Add(new DataPoint(x, y));
        }
        return result;
    }

    #endregion

    #region Series Demonstration Methods - Direct Data

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
            Title = "Distribution 1 (var=1.0)",
            Color = OxyColors.Blue,
            StrokeThickness = 2,
            MarkerType = MarkerType.None
        };
        series1.Points.AddRange(CreateNormalDistribution(-5, 5, 0, 1.0, 200));
        model.Series.Add(series1);

        // Line Series 2: Narrow distribution
        var series2 = new LineSeries
        {
            Title = "Distribution 2 (var=0.25)",
            Color = OxyColors.Red,
            StrokeThickness = 2,
            MarkerType = MarkerType.None
        };
        series2.Points.AddRange(CreateNormalDistribution(-2, 2, 0, 0.25, 200));
        model.Series.Add(series2);

        PlotModel = model;
        UpdateStatus("Line Series demonstration loaded - Two Gaussian distributions with different variances");
    }

    /// <summary>
    /// Demonstrates Line Series with data binding using ItemsSource.
    /// </summary>
    private void LineSeriesBound_Create()
    {
        var model = new PlotModel
        {
            Title = "Line Series (Data Bound)",
            Subtitle = "Series bound to ObservableCollection via ItemsSource",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "X Axis",
            MajorGridlineStyle = LineStyle.Solid
        });

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Y Axis",
            MajorGridlineStyle = LineStyle.Solid
        });

        // Create sorted data for line series
        var sortedData = _dataItems.OrderBy(d => d.X).ToList();

        var lineSeries = new LineSeries
        {
            Title = "Bound Line Series",
            Color = OxyColors.DarkGreen,
            StrokeThickness = 2,
            MarkerType = MarkerType.Circle,
            MarkerSize = 4,
            MarkerFill = OxyColors.Green,
            ItemsSource = sortedData,
            DataFieldX = nameof(DataItem.X),
            DataFieldY = nameof(DataItem.Y)
        };

        model.Series.Add(lineSeries);
        PlotModel = model;
        UpdateStatus("Line Series (Data Bound) - Series uses ItemsSource with DataFieldX/Y mappings");
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

        var colorAxis = new LinearColorAxis
        {
            Position = AxisPosition.Right,
            Title = "Value",
            Palette = OxyPalettes.Jet(200),
            Minimum = 50,
            Maximum = 500
        };
        model.Axes.Add(colorAxis);

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
        UpdateStatus("Scatter Series - 100 random points with variable size and color-mapped values");
    }

    /// <summary>
    /// Demonstrates Scatter Series with data binding.
    /// </summary>
    private void ScatterSeriesBound_Create()
    {
        var model = new PlotModel
        {
            Title = "Scatter Series (Data Bound)",
            Subtitle = "Series bound to ObservableCollection with size and color mapping",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "X Axis",
            MajorGridlineStyle = LineStyle.Solid
        });

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Y Axis",
            MajorGridlineStyle = LineStyle.Solid
        });

        var colorAxis = new LinearColorAxis
        {
            Position = AxisPosition.Right,
            Title = "Color Value",
            Palette = OxyPalettes.Rainbow(200),
            Minimum = 50,
            Maximum = 500
        };
        model.Axes.Add(colorAxis);

        var scatterSeries = new ScatterSeries
        {
            Title = "Bound Scatter Points",
            MarkerType = MarkerType.Circle,
            ItemsSource = _dataItems,
            DataFieldX = nameof(DataItem.X),
            DataFieldY = nameof(DataItem.Y),
            DataFieldSize = nameof(DataItem.Size),
            DataFieldValue = nameof(DataItem.Value)
        };

        model.Series.Add(scatterSeries);
        PlotModel = model;
        UpdateStatus("Scatter Series (Data Bound) - Uses ItemsSource with DataFieldX/Y/Size/Value mappings");
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
        UpdateStatus("Area Series - Gaussian distribution with semi-transparent fill");
    }

    /// <summary>
    /// Demonstrates Area Series with data binding.
    /// </summary>
    private void AreaSeriesBound_Create()
    {
        var model = new PlotModel
        {
            Title = "Area Series (Data Bound)",
            Subtitle = "Area series bound to data collection",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "X Axis",
            MajorGridlineStyle = LineStyle.Solid
        });

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Y Axis",
            MajorGridlineStyle = LineStyle.Solid
        });

        var sortedData = _dataItems.OrderBy(d => d.X).ToList();

        var areaSeries = new AreaSeries
        {
            Title = "Bound Area",
            Color = OxyColors.Purple,
            Fill = OxyColor.FromAColor(80, OxyColors.Purple),
            StrokeThickness = 2,
            ItemsSource = sortedData,
            DataFieldX = nameof(DataItem.X),
            DataFieldY = nameof(DataItem.Y)
        };

        model.Series.Add(areaSeries);
        PlotModel = model;
        UpdateStatus("Area Series (Data Bound) - Uses ItemsSource with X/Y data field mappings");
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

        // Create histogram bins (rangeStart, rangeEnd, area, count)
        histogramSeries.Items.Add(new HistogramItem(0, 200, 400, 2));
        histogramSeries.Items.Add(new HistogramItem(200, 400, 100, 1));
        histogramSeries.Items.Add(new HistogramItem(400, 600, 800, 4));
        histogramSeries.Items.Add(new HistogramItem(600, 800, 2000, 10));
        histogramSeries.Items.Add(new HistogramItem(800, 1000, 1200, 6));
        histogramSeries.Items.Add(new HistogramItem(1000, 1200, 600, 3));

        model.Series.Add(histogramSeries);
        PlotModel = model;
        UpdateStatus("Histogram Series - 6 bins showing frequency distribution");
    }

    /// <summary>
    /// Demonstrates Bar Series (vertical) with CategoryAxis.
    /// </summary>
    /// <remarks>
    /// In OxyPlot 2.x, ColumnSeries was replaced with BarSeries.
    /// BarSeries renders horizontal bars by default (category on Y-axis).
    /// </remarks>
    private void ColumnSeries_Create()
    {
        var model = new PlotModel
        {
            Title = "Bar Series (Vertical Demo)",
            Subtitle = "Test scores by subject",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        var categoryAxis = new CategoryAxis
        {
            Position = AxisPosition.Left,
            Title = "Subject",
            ItemsSource = new[] { "Math", "Science", "English", "History", "Art" }
        };
        model.Axes.Add(categoryAxis);

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Score",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot,
            Minimum = 0,
            Maximum = 100
        });

        var barSeries = new BarSeries
        {
            Title = "Test Scores",
            FillColor = OxyColors.CornflowerBlue
        };

        barSeries.Items.Add(new BarItem(85));
        barSeries.Items.Add(new BarItem(92));
        barSeries.Items.Add(new BarItem(78));
        barSeries.Items.Add(new BarItem(88));
        barSeries.Items.Add(new BarItem(95));

        model.Series.Add(barSeries);
        PlotModel = model;
        UpdateStatus("Bar Series - 5 categories with score values using CategoryAxis");
    }

    /// <summary>
    /// Demonstrates Bar Series with data binding and grouped bars.
    /// </summary>
    /// <remarks>
    /// In OxyPlot 2.x, ColumnSeries was replaced with BarSeries.
    /// </remarks>
    private void ColumnSeriesBound_Create()
    {
        var model = new PlotModel
        {
            Title = "Bar Series (Data Bound)",
            Subtitle = "Grouped bars comparing two test periods",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        var categoryAxis = new CategoryAxis
        {
            Position = AxisPosition.Left,
            Title = "Subject",
            ItemsSource = _categoryItems.Select(c => c.Category).ToArray()
        };
        model.Axes.Add(categoryAxis);

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Score",
            MajorGridlineStyle = LineStyle.Solid,
            Minimum = 0,
            Maximum = 100
        });

        // First period scores
        var series1 = new BarSeries
        {
            Title = "Period 1",
            FillColor = OxyColors.SteelBlue,
            ItemsSource = _categoryItems,
            ValueField = nameof(CategoryDataItem.Value)
        };

        // Second period scores
        var series2 = new BarSeries
        {
            Title = "Period 2",
            FillColor = OxyColors.IndianRed,
            ItemsSource = _categoryItems,
            ValueField = nameof(CategoryDataItem.Value2)
        };

        model.Series.Add(series1);
        model.Series.Add(series2);
        PlotModel = model;
        UpdateStatus("Bar Series (Data Bound) - Grouped bars using ItemsSource and ValueField");
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
            MinorGridlineStyle = LineStyle.Dot,
            Minimum = 0,
            Maximum = 40
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
        UpdateStatus("Bar Series - Horizontal bars with percentage labels inside bars");
    }

    /// <summary>
    /// Demonstrates Bar Series with data binding.
    /// </summary>
    private void BarSeriesBound_Create()
    {
        var model = new PlotModel
        {
            Title = "Bar Series (Data Bound)",
            Subtitle = "Grouped horizontal bars from data source",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        var categoryAxis = new CategoryAxis
        {
            Position = AxisPosition.Left,
            Title = "Subject",
            ItemsSource = _categoryItems.Select(c => c.Category).ToArray()
        };
        model.Axes.Add(categoryAxis);

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Score",
            MajorGridlineStyle = LineStyle.Solid,
            Minimum = 0,
            Maximum = 100
        });

        var series1 = new BarSeries
        {
            Title = "Test 1",
            FillColor = OxyColors.DarkCyan,
            ItemsSource = _categoryItems,
            ValueField = nameof(CategoryDataItem.Value)
        };

        var series2 = new BarSeries
        {
            Title = "Test 2",
            FillColor = OxyColors.DarkOrange,
            ItemsSource = _categoryItems,
            ValueField = nameof(CategoryDataItem.Value2)
        };

        model.Series.Add(series1);
        model.Series.Add(series2);
        PlotModel = model;
        UpdateStatus("Bar Series (Data Bound) - Grouped bars using ItemsSource binding");
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
            ItemsSource = new[] { "Q1", "Q2", "Q3", "Q4" }
        };
        model.Axes.Add(categoryAxis);

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Value",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        var boxPlotSeries = new BoxPlotSeries
        {
            Title = "Quarterly Data",
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

        boxPlotSeries.Items.Add(new BoxPlotItem(3, 12, 15, 17, 30, 40)
        {
            Outliers = new List<double> { 5, 55 }
        });

        model.Series.Add(boxPlotSeries);
        PlotModel = model;
        UpdateStatus("BoxPlot Series - 4 box plots with whiskers and outlier points");
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
        UpdateStatus("HeatMap Series - 100x100 grid with 2D Gaussian distribution, bitmap rendering");
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

        var colorAxis = new LinearColorAxis
        {
            Position = AxisPosition.Right,
            Title = "Measurement",
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
        UpdateStatus("ScatterError Series - 30 points with X/Y error bars and color mapping");
    }

    /// <summary>
    /// Demonstrates DateTime Series with DateTimeAxis.
    /// </summary>
    private void DateTimeSeries_Create()
    {
        var model = new PlotModel
        {
            Title = "DateTime Series",
            Subtitle = "Seasonal temperature variation over one year",
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
            Title = "Temperature (F)",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        var lineSeries = new LineSeries
        {
            Title = "Daily Temperature",
            Color = OxyColors.OrangeRed,
            StrokeThickness = 1.5,
            MarkerType = MarkerType.None
        };

        lineSeries.Points.AddRange(CreateTimeSeriesData());
        model.Series.Add(lineSeries);

        PlotModel = model;
        UpdateStatus("DateTime Series - 365 days of temperature data with seasonal pattern");
    }

    /// <summary>
    /// Demonstrates Logarithmic Axis with exponential data.
    /// </summary>
    private void LogarithmicAxis_Create()
    {
        var model = new PlotModel
        {
            Title = "Logarithmic Axis",
            Subtitle = "Exponential growth displayed on logarithmic scale",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Time (Linear Scale)",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        model.Axes.Add(new LogarithmicAxis
        {
            Position = AxisPosition.Left,
            Title = "Value (Log Scale)",
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot,
            Base = 10,
            Minimum = 1
        });

        var lineSeries = new LineSeries
        {
            Title = "Exponential Growth (1.1^x)",
            Color = OxyColors.DarkBlue,
            StrokeThickness = 2
        };

        lineSeries.Points.AddRange(CreateExponentialData());
        model.Series.Add(lineSeries);

        // Add reference lines
        var lineSeries2 = new LineSeries
        {
            Title = "Linear Reference (10x)",
            Color = OxyColors.Gray,
            StrokeThickness = 1,
            LineStyle = LineStyle.Dash
        };

        for (int i = 1; i <= 100; i++)
        {
            lineSeries2.Points.Add(new DataPoint(i, i * 10));
        }
        model.Series.Add(lineSeries2);

        PlotModel = model;
        UpdateStatus("Logarithmic Axis - Exponential function appears linear on log scale");
    }

    /// <summary>
    /// Demonstrates various annotation types.
    /// </summary>
    private void Annotations_Create()
    {
        var model = new PlotModel
        {
            Title = "Annotations Demo",
            Subtitle = "Various annotation types: Line, Text, Rectangle, Ellipse, Arrow, Polygon",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "X Axis",
            Minimum = 0,
            Maximum = 100,
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Y Axis",
            Minimum = 0,
            Maximum = 100,
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Dot
        });

        // Line annotation (horizontal)
        model.Annotations.Add(new LineAnnotation
        {
            Type = LineAnnotationType.Horizontal,
            Y = 50,
            Color = OxyColors.Red,
            StrokeThickness = 2,
            Text = "Horizontal Line at Y=50",
            TextColor = OxyColors.Red
        });

        // Line annotation (vertical)
        model.Annotations.Add(new LineAnnotation
        {
            Type = LineAnnotationType.Vertical,
            X = 25,
            Color = OxyColors.Blue,
            StrokeThickness = 2,
            Text = "Vertical Line",
            TextColor = OxyColors.Blue
        });

        // Rectangle annotation
        model.Annotations.Add(new RectangleAnnotation
        {
            MinimumX = 60,
            MaximumX = 90,
            MinimumY = 60,
            MaximumY = 90,
            Fill = OxyColor.FromAColor(80, OxyColors.Green),
            Stroke = OxyColors.DarkGreen,
            StrokeThickness = 2,
            Text = "Rectangle Region"
        });

        // Ellipse annotation
        model.Annotations.Add(new EllipseAnnotation
        {
            X = 40,
            Y = 30,
            Width = 30,
            Height = 20,
            Fill = OxyColor.FromAColor(80, OxyColors.Orange),
            Stroke = OxyColors.DarkOrange,
            StrokeThickness = 2,
            Text = "Ellipse"
        });

        // Arrow annotation
        model.Annotations.Add(new ArrowAnnotation
        {
            StartPoint = new DataPoint(10, 80),
            EndPoint = new DataPoint(30, 60),
            Color = OxyColors.Purple,
            StrokeThickness = 2,
            HeadLength = 10,
            HeadWidth = 4,
            Text = "Arrow"
        });

        // Text annotation
        model.Annotations.Add(new TextAnnotation
        {
            TextPosition = new DataPoint(75, 25),
            Text = "Text Annotation\n(Multi-line)",
            TextColor = OxyColors.DarkCyan,
            FontSize = 14,
            FontWeight = 700, // Bold weight in OxyPlot (uses double, not FontWeights)
            Stroke = OxyColors.Transparent
        });

        // Polygon annotation
        var polygonAnnotation = new PolygonAnnotation
        {
            Fill = OxyColor.FromAColor(100, OxyColors.Magenta),
            Stroke = OxyColors.DarkMagenta,
            StrokeThickness = 2,
            Text = "Polygon"
        };
        polygonAnnotation.Points.Add(new DataPoint(10, 10));
        polygonAnnotation.Points.Add(new DataPoint(20, 5));
        polygonAnnotation.Points.Add(new DataPoint(25, 15));
        polygonAnnotation.Points.Add(new DataPoint(15, 20));
        model.Annotations.Add(polygonAnnotation);

        // Point annotation
        model.Annotations.Add(new PointAnnotation
        {
            X = 50,
            Y = 75,
            Size = 10,
            Fill = OxyColors.Gold,
            Stroke = OxyColors.Black,
            StrokeThickness = 2,
            Text = "Point",
            TextColor = OxyColors.Black
        });

        PlotModel = model;
        UpdateStatus("Annotations Demo - 8 different annotation types displayed on the plot");
    }

    /// <summary>
    /// Demonstrates multiple axes on a single plot.
    /// </summary>
    private void MultipleAxes_Create()
    {
        var model = new PlotModel
        {
            Title = "Multiple Axes",
            Subtitle = "Two Y-axes with different scales for different data series",
            Background = OxyColors.White,
            PlotAreaBorderColor = OxyColors.Black,
            PlotAreaBorderThickness = new OxyThickness(1)
        };

        // Shared X-axis
        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "X Axis (Shared)",
            Key = "x",
            MajorGridlineStyle = LineStyle.Solid
        });

        // Left Y-axis (for temperature)
        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Temperature (C)",
            Key = "y1",
            AxislineColor = OxyColors.Blue,
            TextColor = OxyColors.Blue,
            TicklineColor = OxyColors.Blue,
            TitleColor = OxyColors.Blue,
            MajorGridlineStyle = LineStyle.Solid,
            Minimum = -20,
            Maximum = 40
        });

        // Right Y-axis (for humidity)
        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Right,
            Title = "Humidity (%)",
            Key = "y2",
            AxislineColor = OxyColors.Green,
            TextColor = OxyColors.Green,
            TicklineColor = OxyColors.Green,
            TitleColor = OxyColors.Green,
            Minimum = 0,
            Maximum = 100
        });

        // Temperature series (uses left axis)
        var tempSeries = new LineSeries
        {
            Title = "Temperature",
            Color = OxyColors.Blue,
            StrokeThickness = 2,
            XAxisKey = "x",
            YAxisKey = "y1"
        };

        for (int i = 0; i < 50; i++)
        {
            double x = i;
            double y = 15 + 10 * Math.Sin(i * 0.2) + (_random.NextDouble() - 0.5) * 5;
            tempSeries.Points.Add(new DataPoint(x, y));
        }
        model.Series.Add(tempSeries);

        // Humidity series (uses right axis)
        var humiditySeries = new LineSeries
        {
            Title = "Humidity",
            Color = OxyColors.Green,
            StrokeThickness = 2,
            XAxisKey = "x",
            YAxisKey = "y2"
        };

        for (int i = 0; i < 50; i++)
        {
            double x = i;
            double y = 60 + 20 * Math.Cos(i * 0.15) + (_random.NextDouble() - 0.5) * 10;
            humiditySeries.Points.Add(new DataPoint(x, y));
        }
        model.Series.Add(humiditySeries);

        PlotModel = model;
        UpdateStatus("Multiple Axes - Left axis (Temperature) and Right axis (Humidity) with different scales");
    }

    #endregion

    #region UI Event Handlers

    /// <summary>
    /// Handles the SelectionChanged event of the DemoTypeComboBox.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments.</param>
    private void DemoTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DemoTypeComboBox.SelectedItem is not ComboBoxItem item)
            return;

        try
        {
            switch (item.Content.ToString())
            {
                case "Line Series":
                    LineSeries_Create();
                    break;
                case "Line Series (Data Bound)":
                    LineSeriesBound_Create();
                    break;
                case "Scatter Series":
                    ScatterSeries_Create();
                    break;
                case "Scatter Series (Data Bound)":
                    ScatterSeriesBound_Create();
                    break;
                case "Area Series":
                    AreaSeries_Create();
                    break;
                case "Area Series (Data Bound)":
                    AreaSeriesBound_Create();
                    break;
                case "Histogram Series":
                    HistogramSeries_Create();
                    break;
                case "Column Series":
                    ColumnSeries_Create();
                    break;
                case "Column Series (Data Bound)":
                    ColumnSeriesBound_Create();
                    break;
                case "Bar Series":
                    BarSeries_Create();
                    break;
                case "Bar Series (Data Bound)":
                    BarSeriesBound_Create();
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
                case "Logarithmic Axis":
                    LogarithmicAxis_Create();
                    break;
                case "Annotations Demo":
                    Annotations_Create();
                    break;
                case "Multiple Axes":
                    MultipleAxes_Create();
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

    /// <summary>
    /// Handles the Click event of the SaveSettings button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments.</param>
    private void SaveSettings_Click(object sender, RoutedEventArgs e)
    {
        if (PlotModel == null)
        {
            UpdateStatus("No plot to save. Please select a demo first.", isError: true);
            return;
        }

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

    /// <summary>
    /// Handles the Click event of the LoadSettings button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments.</param>
    private void LoadSettings_Click(object sender, RoutedEventArgs e)
    {
        if (PlotModel == null)
        {
            UpdateStatus("No plot to apply settings to. Please select a demo first.", isError: true);
            return;
        }

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
                    PlotModel.InvalidatePlot(true);
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

    /// <summary>
    /// Handles the Changed event of the ShowPropertiesCheckBox.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments.</param>
    private void ShowPropertiesCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        // Avoid issues during initialization before controls are fully loaded
        if (!_isLoaded)
            return;

        SetPropertiesPanelVisibility(ShowPropertiesCheckBox.IsChecked == true);
    }

    /// <summary>
    /// Handles the Click event of the ClosePropertiesPanel button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments.</param>
    private void ClosePropertiesPanel_Click(object sender, RoutedEventArgs e)
    {
        SetPropertiesPanelVisibility(false);
        ShowPropertiesCheckBox.IsChecked = false;
    }

    /// <summary>
    /// Handles the ClosePropertiesCalled event of the PropertiesControl.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments.</param>
    private void PropertiesControl_ClosePropertiesCalled(object? sender, EventArgs e)
    {
        SetPropertiesPanelVisibility(false);
        ShowPropertiesCheckBox.IsChecked = false;
    }

    /// <summary>
    /// Handles the PropertiesCalled event of the PlotToolbar.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments.</param>
    private void PlotToolbar_PropertiesCalled(object? sender, EventArgs e)
    {
        // Toggle properties panel visibility
        var newVisibility = PropertiesPanel.Visibility != Visibility.Visible;
        SetPropertiesPanelVisibility(newVisibility);
        ShowPropertiesCheckBox.IsChecked = newVisibility;
    }

    /// <summary>
    /// Handles the Click event of the RefreshPlot button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments.</param>
    private void RefreshPlot_Click(object sender, RoutedEventArgs e)
    {
        PlotModel?.InvalidatePlot(true);
        UpdateStatus("Plot refreshed");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Sets the visibility of the properties panel.
    /// </summary>
    /// <param name="visible">True to show the panel, false to hide it.</param>
    private void SetPropertiesPanelVisibility(bool visible)
    {
        if (visible)
        {
            this.PropertiesPanel.Visibility = Visibility.Visible;
            this.PropertiesColumn.Width = new GridLength(380);
            this.PropertiesColumn.MinWidth = 300;
        }
        else
        {
            this.PropertiesPanel.Visibility = Visibility.Collapsed;
            this.PropertiesColumn.Width = new GridLength(0);
            this.PropertiesColumn.MinWidth = 0;
        }
    }

    /// <summary>
    /// Updates the status text display.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="isError">True if this is an error message.</param>
    private void UpdateStatus(string message, bool isError = false)
    {
        // Guard against calls before XAML is fully initialized
        if (StatusText == null)
            return;

        StatusText.Text = $"{DateTime.Now:HH:mm:ss} - {message}";
        StatusText.Foreground = isError
            ? System.Windows.Media.Brushes.Red
            : System.Windows.Media.Brushes.Green;
    }

    /// <summary>
    /// Updates the plot statistics display.
    /// </summary>
    /// <param name="model">The plot model to analyze.</param>
    private void UpdatePlotStats(PlotModel model)
    {
        // Guard against calls before XAML is fully initialized
        if (StatsText == null)
            return;

        if (model == null)
        {
            StatsText.Text = string.Empty;
            return;
        }

        var seriesCount = model.Series.Count;
        var axisCount = model.Axes.Count;
        var annotationCount = model.Annotations.Count;

        var totalPoints = 0;
        foreach (var series in model.Series)
        {
            // Note: AreaSeries must be checked before LineSeries because AreaSeries extends LineSeries
            totalPoints += series switch
            {
                AreaSeries ars => ars.Points.Count,
                LineSeries ls => ls.Points.Count,
                ScatterSeries ss => ss.Points.Count,
                ScatterErrorSeries ses => ses.Points.Count,
                BarSeries bs => bs.Items.Count,
                BoxPlotSeries bps => bps.Items.Count,
                HistogramSeries hs => hs.Items.Count,
                HeatMapSeries hms => hms.Data?.Length ?? 0,
                _ => 0
            };
        }

        StatsText.Text = $"Series: {seriesCount} | Axes: {axisCount} | Annotations: {annotationCount} | Data Points: {totalPoints:N0}";
    }

    #endregion
}
