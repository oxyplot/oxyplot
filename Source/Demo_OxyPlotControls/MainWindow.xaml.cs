using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using Numerics.Data;

namespace Demo_OxyPlotControls
{
    /// <summary>
    /// Modern MVVM demo for OxyPlotControls.
    /// Uses PlotView with PlotModel binding instead of legacy OxyPlot.Wpf.Plot.
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties

        private PlotModel _plotModel;

        /// <summary>
        /// Gets or sets the PlotModel bound to the PlotView.
        /// This is the modern MVVM approach for OxyPlot.
        /// </summary>
        public PlotModel PlotModel
        {
            get => _plotModel;
            set
            {
                _plotModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Registry to store demo data for series, keyed by series title.
        /// </summary>
        private readonly Dictionary<string, object> _demoDataRegistry = new();

        /// <summary>
        /// Registry to store category axis labels, keyed by axis key.
        /// </summary>
        private readonly Dictionary<string, IEnumerable<string>> _categoryAxisLabelsRegistry = new();

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            // Initialize with a default PlotModel
            _plotModel = new PlotModel { Title = "OxyPlotControls Demo" };

            InitializeComponent();
            DataContext = this;

            // Trigger line series in combobox (unbound)
            Combobox1.SelectedIndex = 0;
        }

        #endregion

        #region Event Handlers

        private void OxyPlotToolBar_PropertiesCalled(PlotView targetPlotView, bool openProperties, OxyPlotControls.OxyPlotPropertiesControl.PropertyEXP? propertyExpander, object selectedObject)
        {
            if (propertyExpander.HasValue)
            {
                PropertiesControl.ExpandProperty(propertyExpander.Value, selectedObject);
            }
        }

        private void PropertiesControl_ClosePropertiesCalled(OxyPlotControls.OxyPlotPropertiesControl propertiesControl)
        {
            MessageBox.Show("Properties panel close requested.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            string saveFile = GenericControls.GeneralMethods.FileSaveDialog("Plot Settings(*.xml) |*.xml", true);
            if (string.IsNullOrEmpty(saveFile)) return;

            try
            {
                if (File.Exists(saveFile)) File.Delete(saveFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error attempting to delete existing file '{saveFile}'.\n\n{ex.Message}");
                return;
            }

            using var writer = XmlWriter.Create(saveFile, new XmlWriterSettings { Indent = true });
            OxyPlotControls.OxyPlotSettingsSerializer.ToXelement(PlotModel).WriteTo(writer);
            UpdateStatus($"Settings saved to {Path.GetFileName(saveFile)}");
        }

        private void LoadSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            string fileToOpen = GenericControls.GeneralMethods.FileOpenDialog("Plot Settings(*.xml) |*.xml");
            if (string.IsNullOrEmpty(fileToOpen)) return;
            if (Path.GetExtension(fileToOpen) != ".xml") return;

            if (File.Exists(fileToOpen))
            {
                var document = new XmlDocument();
                document.Load(fileToOpen);

                // Create a new PlotModel and apply the settings
                var newModel = new PlotModel();
                OxyPlotControls.OxyPlotSettingsSerializer.FromXelement(newModel, XElement.Parse(document.GetElementsByTagName(OxyPlotControls.OxyPlotSettingsSerializer.OxyplotPropertiesTag)[0].OuterXml));

                // Repopulate series data from the registry
                RepopulateAllSeriesData(newModel);

                // Repopulate category axis labels
                RepopulateCategoryAxisLabels(newModel);

                PlotModel = newModel;
                PlotModel.InvalidatePlot(true);
                UpdateStatus($"Settings loaded from {Path.GetFileName(fileToOpen)}");
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Combobox1.SelectedItem is not ComboBoxItem selectedItem) return;
            if (selectedItem.Content == null) return;

            var selection = selectedItem.Content.ToString();
            switch (selection)
            {
                case "Line Series":
                    CreateLineSeries(false);
                    break;
                case "Line Series (Bound)":
                    CreateLineSeries(true);
                    break;
                case "Scatter Series":
                    CreateScatterSeries(false);
                    break;
                case "Scatter Series (Bound)":
                    CreateScatterSeries(true);
                    break;
                case "Histogram Series":
                    CreateHistogramSeries(false);
                    break;
                case "Histogram Series (Bound)":
                    CreateHistogramSeries(true);
                    break;
                case "Bar Series":
                    CreateBarSeries(false);
                    break;
                case "Bar Series (Bound)":
                    CreateBarSeries(true);
                    break;
                case "Box Plot Series":
                    CreateBoxPlotSeries(false);
                    break;
                case "Box Plot Series (Bound)":
                    CreateBoxPlotSeries(true);
                    break;
                case "Area Series":
                    CreateAreaSeries(false);
                    break;
                case "Area Series (Bound)":
                    CreateAreaSeries(true);
                    break;
                case "Heat Map Series":
                    CreateHeatMapSeries(false);
                    break;
                case "Heat Map Series (Bound)":
                    CreateHeatMapSeries(true);
                    break;
                case "Scatter Error Series":
                    CreateScatterErrorSeries(false);
                    break;
                case "Scatter Error Series (Bound)":
                    CreateScatterErrorSeries(true);
                    break;
                case "Date Time Series":
                    CreateDateTimeSeries();
                    break;
                case "Pie Series":
                    CreatePieSeries();
                    break;
                case "Stem Series":
                    CreateStemSeries();
                    break;
                case "Two Color Line Series":
                    CreateTwoColorLineSeries();
                    break;
                case "Step Series":
                    CreateStepSeries();
                    break;
            }
        }

        #endregion

        #region Helper Methods

        private void UpdateStatus(string message)
        {
            if (StatusText != null)
            {
                StatusText.Text = message;
            }
        }

        private List<DataPoint> CreateNormalDistribution(double x0, double x1, double mean, double variance, int n = 1001)
        {
            var result = new List<DataPoint>();
            for (int i = 0; i < n; i++)
            {
                double x = x0 + ((x1 - x0) * i / (n - 1));
                double f = 1.0 / Math.Sqrt(2 * Math.PI * variance) * Math.Exp(-(x - mean) * (x - mean) / 2 / variance);
                result.Add(new DataPoint(x, f));
            }
            return result;
        }

        private void RegisterDemoData(string title, object data)
        {
            if (!string.IsNullOrEmpty(title))
            {
                _demoDataRegistry[title] = data;
            }
        }

        private void RegisterCategoryAxisLabels(string axisKey, IEnumerable<string> labels)
        {
            _categoryAxisLabelsRegistry[string.IsNullOrEmpty(axisKey) ? "default" : axisKey] = labels;
        }

        private void RepopulateAllSeriesData(PlotModel model)
        {
            foreach (var series in model.Series)
            {
                RepopulateSeriesData(series);
            }
        }

        private void RepopulateCategoryAxisLabels(PlotModel model)
        {
            foreach (var axis in model.Axes)
            {
                if (axis is CategoryAxis catAxis)
                {
                    string axisKey = string.IsNullOrEmpty(catAxis.Key) ? "default" : catAxis.Key;
                    if (_categoryAxisLabelsRegistry.TryGetValue(axisKey, out var labels))
                    {
                        catAxis.Labels.Clear();
                        foreach (var label in labels)
                        {
                            catAxis.Labels.Add(label);
                        }
                    }
                }
            }
        }

        private void RepopulateSeriesData(OxyPlot.Series.Series series)
        {
            string? title = series.Title;
            if (!string.IsNullOrEmpty(title) && _demoDataRegistry.TryGetValue(title, out var storedData))
            {
                ApplyDataToSeries(series, storedData);
                return;
            }
            GenerateDefaultDemoData(series);
        }

        private void ApplyDataToSeries(OxyPlot.Series.Series series, object data)
        {
            switch (series)
            {
                case AreaSeries areaSeries when data is List<DataPoint> points:
                    areaSeries.Points.Clear();
                    areaSeries.Points.AddRange(points);
                    break;

                case LineSeries lineSeries when data is List<DataPoint> points:
                    lineSeries.Points.Clear();
                    lineSeries.Points.AddRange(points);
                    break;

                case ScatterSeries scatterSeries when data is List<ScatterPoint> points:
                    scatterSeries.Points.Clear();
                    scatterSeries.Points.AddRange(points);
                    break;

                case ScatterErrorSeries scatterErrorSeries when data is List<ScatterErrorPoint> points:
                    scatterErrorSeries.Points.Clear();
                    scatterErrorSeries.Points.AddRange(points);
                    break;

                case HistogramSeries histogramSeries when data is List<HistogramItem> items:
                    histogramSeries.Items.Clear();
                    histogramSeries.Items.AddRange(items);
                    break;

                case BarSeries barSeries when data is List<BarItem> items:
                    barSeries.Items.Clear();
                    barSeries.Items.AddRange(items);
                    break;

                case BoxPlotSeries boxPlotSeries when data is List<BoxPlotItem> items:
                    boxPlotSeries.Items.Clear();
                    boxPlotSeries.Items.AddRange(items);
                    break;

                case HeatMapSeries heatMapSeries when data is double[,] heatData:
                    heatMapSeries.Data = heatData;
                    break;
            }
        }

        private void GenerateDefaultDemoData(OxyPlot.Series.Series series)
        {
            switch (series)
            {
                case AreaSeries areaSeries:
                    areaSeries.Points.Clear();
                    areaSeries.Points.AddRange(CreateNormalDistribution(-5, 5, 0, 1));
                    break;

                case LineSeries lineSeries:
                    lineSeries.Points.Clear();
                    lineSeries.Points.AddRange(CreateNormalDistribution(-5, 5, 0, 1));
                    break;

                case ScatterSeries scatterSeries:
                    scatterSeries.Points.Clear();
                    var r1 = new Random(314);
                    for (int i = 0; i < 50; i++)
                    {
                        scatterSeries.Points.Add(new ScatterPoint(r1.NextDouble(), r1.NextDouble(), r1.Next(5, 15), r1.Next(100, 1000)));
                    }
                    break;

                case ScatterErrorSeries scatterErrorSeries:
                    scatterErrorSeries.Points.Clear();
                    var r2 = new Random(314);
                    for (int i = 0; i < 30; i++)
                    {
                        double x = r2.NextDouble();
                        double y = r2.NextDouble();
                        scatterErrorSeries.Points.Add(new ScatterErrorPoint(x, y, x / 5, x / 10, y / 5, y / 10, r2.Next(5, 15), r2.Next(100, 1000)));
                    }
                    break;

                case HistogramSeries histogramSeries:
                    histogramSeries.Items.Clear();
                    histogramSeries.Items.Add(new HistogramItem(0, 200, 400));
                    histogramSeries.Items.Add(new HistogramItem(200, 400, 100));
                    histogramSeries.Items.Add(new HistogramItem(400, 600, 800));
                    histogramSeries.Items.Add(new HistogramItem(600, 800, 2000));
                    break;

                case BarSeries barSeries:
                    barSeries.Items.Clear();
                    barSeries.Items.Add(new BarItem(20));
                    barSeries.Items.Add(new BarItem(35));
                    barSeries.Items.Add(new BarItem(15));
                    barSeries.Items.Add(new BarItem(25));
                    barSeries.Items.Add(new BarItem(30));
                    break;

                case BoxPlotSeries boxPlotSeries:
                    boxPlotSeries.Items.Clear();
                    boxPlotSeries.Items.Add(new BoxPlotItem(0, 3, 5, 7, 20, 30));
                    boxPlotSeries.Items.Add(new BoxPlotItem(1, 4, 12, 14, 30, 40));
                    boxPlotSeries.Items.Add(new BoxPlotItem(2, 7, 10, 12, 25, 35));
                    boxPlotSeries.Items.Add(new BoxPlotItem(3, 12, 15, 17, 30, 40));
                    break;

                case HeatMapSeries heatMapSeries:
                    var singleData = new double[100];
                    for (int x = 0; x < 100; x++)
                    {
                        singleData[x] = Math.Exp(-0.5 * Math.Pow((x - 50.0) / 20, 2));
                    }
                    var heatData = new double[100, 100];
                    for (int x = 0; x < 100; x++)
                    {
                        for (int y = 0; y < 100; y++)
                        {
                            heatData[y, x] = singleData[x] * singleData[(y + 30) % 100] * 100;
                        }
                    }
                    heatMapSeries.Data = heatData;
                    break;
            }
        }

        #endregion

        #region Series Creation Methods

        private PlotModel CreateBasePlotModel(string title)
        {
            return new PlotModel
            {
                Title = title,
                IsLegendVisible = true
            };
        }

        private void AddStandardAxes(PlotModel model, string xTitle = "X Axis", string yTitle = "Y Axis")
        {
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = xTitle,
                Key = "x",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                AxisTitleDistance = 15
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = yTitle,
                Key = "y",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                TitleFontSize = 16,
                AxisTitleDistance = 15
            });
        }

        private void CreateLineSeries(bool bound)
        {
            var model = CreateBasePlotModel("Line Series Demo");
            AddStandardAxes(model);

            var series1 = new LineSeries
            {
                Title = "Normal Distribution 1",
                Color = OxyColors.Blue,
                StrokeThickness = 2
            };

            var series2 = new LineSeries
            {
                Title = "Normal Distribution 2",
                Color = OxyColors.Red,
                StrokeThickness = 2,
                LineStyle = LineStyle.Dash
            };

            var points1 = CreateNormalDistribution(-5, 5, 0, 1);
            var points2 = CreateNormalDistribution(-4, 4, 0, 0.5);

            series1.Points.AddRange(points1);
            series2.Points.AddRange(points2);

            RegisterDemoData(series1.Title, points1);
            RegisterDemoData(series2.Title, points2);

            model.Series.Add(series1);
            model.Series.Add(series2);

            PlotModel = model;
            UpdateStatus($"Displaying Line Series{(bound ? " (Bound)" : "")} - 2 series with normal distributions");
        }

        private void CreateScatterSeries(bool bound)
        {
            var model = CreateBasePlotModel("Scatter Series Demo");
            AddStandardAxes(model);

            var series = new ScatterSeries
            {
                Title = "Random Scatter Points",
                MarkerType = MarkerType.Circle
            };

            var r = new Random(314);
            var points = new List<ScatterPoint>();

            for (int i = 0; i < 100; i++)
            {
                var pt = new ScatterPoint(r.NextDouble(), r.NextDouble(), r.Next(5, 15), r.Next(100, 1000));
                points.Add(pt);
                series.Points.Add(pt);
            }

            RegisterDemoData(series.Title, points);

            // Add color axis
            model.Axes.Add(new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.Rainbow(200),
                Title = "Color Value"
            });

            model.Series.Add(series);
            PlotModel = model;
            UpdateStatus($"Displaying Scatter Series{(bound ? " (Bound)" : "")} - 100 random points with size and color mapping");
        }

        private void CreateScatterErrorSeries(bool bound)
        {
            var model = CreateBasePlotModel("Scatter Error Series Demo");
            AddStandardAxes(model);

            var series = new ScatterErrorSeries
            {
                Title = "Measurements with Error Bars",
                MarkerType = MarkerType.Circle
            };

            var r = new Random(314);
            var points = new List<ScatterErrorPoint>();

            for (int i = 0; i < 30; i++)
            {
                double x = r.NextDouble();
                double y = r.NextDouble();
                var pt = new ScatterErrorPoint(x, y, x / 5, x / 10, y / 5, y / 10, r.Next(5, 15), r.Next(100, 1000));
                points.Add(pt);
                series.Points.Add(pt);
            }

            RegisterDemoData(series.Title, points);

            model.Axes.Add(new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.Hot(200),
                Title = "Uncertainty Level"
            });

            model.Series.Add(series);
            PlotModel = model;
            UpdateStatus($"Displaying Scatter Error Series{(bound ? " (Bound)" : "")} - 30 points with X/Y error bars");
        }

        private void CreateHistogramSeries(bool bound)
        {
            var model = CreateBasePlotModel("Histogram Series Demo");
            AddStandardAxes(model, "Value Range", "Frequency");

            var series = new HistogramSeries
            {
                Title = "Sample Distribution",
                FillColor = OxyColors.SteelBlue,
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1
            };

            var items = new List<HistogramItem>
            {
                new HistogramItem(0, 200, 400),
                new HistogramItem(200, 400, 100),
                new HistogramItem(400, 600, 800),
                new HistogramItem(600, 800, 2000),
                new HistogramItem(800, 1000, 600)
            };

            series.Items.AddRange(items);
            RegisterDemoData(series.Title, items);

            model.Series.Add(series);
            PlotModel = model;
            UpdateStatus($"Displaying Histogram Series{(bound ? " (Bound)" : "")} - 5 bins showing frequency distribution");
        }

        private void CreateBarSeries(bool bound)
        {
            var model = CreateBasePlotModel("Bar Series Demo");

            var categoryLabels = new[] { "Apple Cake", "Baumkuchen", "Bundt Cake", "Chocolate Cake", "Carrot Cake" };

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "category",
                Title = "Cake Type",
                ItemsSource = categoryLabels
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Popularity (%)",
                MinimumPadding = 0,
                AbsoluteMinimum = 0
            });

            RegisterCategoryAxisLabels("category", categoryLabels);

            var series = new BarSeries
            {
                Title = "Cake Popularity",
                FillColor = OxyColors.CornflowerBlue
            };

            var r = new Random(314);
            var values = Enumerable.Range(0, 5).Select(_ => r.NextDouble()).ToArray();
            var sum = values.Sum();
            var items = values.Select(v => new BarItem(v / sum * 100)).ToList();

            series.Items.AddRange(items);
            RegisterDemoData(series.Title, items);

            model.Series.Add(series);
            PlotModel = model;
            UpdateStatus($"Displaying Bar Series{(bound ? " (Bound)" : "")} - 5 categories showing popularity percentages");
        }

        private void CreateBoxPlotSeries(bool bound)
        {
            var model = CreateBasePlotModel("Box Plot Series Demo");

            var categoryLabels = new[] { "Math", "Science", "English", "History" };

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Key = "category",
                Title = "Subject",
                IsTickCentered = true,
                ItemsSource = categoryLabels
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Score",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash
            });

            RegisterCategoryAxisLabels("category", categoryLabels);

            var series = new BoxPlotSeries
            {
                Title = "Test Scores by Subject",
                BoxWidth = 0.4,
                WhiskerWidth = 0.3,
                Stroke = OxyColors.Black,
                StrokeThickness = 1,
                Fill = OxyColors.LightSteelBlue
            };

            var items = new List<BoxPlotItem>
            {
                new BoxPlotItem(0, 60, 70, 75, 85, 95) { Outliers = new List<double> { 50, 98 } },
                new BoxPlotItem(1, 55, 65, 72, 82, 90),
                new BoxPlotItem(2, 50, 60, 68, 78, 88) { Outliers = new List<double> { 45, 92 } },
                new BoxPlotItem(3, 58, 68, 74, 84, 92)
            };

            series.Items.AddRange(items);
            RegisterDemoData(series.Title, items);

            model.Series.Add(series);
            PlotModel = model;
            UpdateStatus($"Displaying Box Plot Series{(bound ? " (Bound)" : "")} - 4 subjects with statistical summaries and outliers");
        }

        private void CreateAreaSeries(bool bound)
        {
            var model = CreateBasePlotModel("Area Series Demo");
            AddStandardAxes(model);

            var series = new AreaSeries
            {
                Title = "Probability Density",
                Fill = OxyColor.FromAColor(128, OxyColors.Green),
                Color = OxyColors.DarkGreen,
                StrokeThickness = 2
            };

            var points = CreateNormalDistribution(-5, 5, 0, 1);
            series.Points.AddRange(points);
            RegisterDemoData(series.Title, points);

            model.Series.Add(series);
            PlotModel = model;
            UpdateStatus($"Displaying Area Series{(bound ? " (Bound)" : "")} - Normal distribution with filled area");
        }

        private void CreateHeatMapSeries(bool bound)
        {
            var model = CreateBasePlotModel("Heat Map Series Demo");

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "X",
                Minimum = 0,
                Maximum = 99
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Y",
                Minimum = 0,
                Maximum = 99
            });

            model.Axes.Add(new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.Viridis(256),
                Title = "Intensity"
            });

            // Generate 2D Gaussian distribution
            var singleData = new double[100];
            for (int x = 0; x < 100; x++)
            {
                singleData[x] = Math.Exp(-0.5 * Math.Pow((x - 50.0) / 20, 2));
            }

            var data = new double[100, 100];
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    data[y, x] = singleData[x] * singleData[(y + 30) % 100] * 100;
                }
            }

            var series = new HeatMapSeries
            {
                Title = "2D Gaussian Distribution",
                X0 = 0,
                X1 = 99,
                Y0 = 0,
                Y1 = 99,
                Data = data,
                Interpolate = true,
                RenderMethod = HeatMapRenderMethod.Bitmap
            };

            RegisterDemoData(series.Title, data);

            model.Series.Add(series);
            PlotModel = model;
            UpdateStatus($"Displaying Heat Map Series{(bound ? " (Bound)" : "")} - 100x100 2D Gaussian distribution");
        }

        private void CreateDateTimeSeries()
        {
            var model = CreateBasePlotModel("Date Time Series Demo");

            model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Date",
                StringFormat = "yyyy-MM-dd",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Flow (cfs)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash
            });

            try
            {
                using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("Demo_OxyPlotControls.USGS_01134500.xml");
                if (resource != null)
                {
                    using var reader = new StreamReader(resource);
                    var timeSeries = new TimeSeries(XElement.Parse(reader.ReadToEnd()));

                    var series = new LineSeries
                    {
                        Title = "USGS Stream Flow Data",
                        Color = OxyColors.Blue,
                        StrokeThickness = 1
                    };

                    foreach (var item in timeSeries)
                    {
                        series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(item.Index), item.Value));
                    }

                    model.Series.Add(series);
                    UpdateStatus("Displaying Date Time Series - USGS stream flow data over time");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading time series data: {ex.Message}");
            }

            PlotModel = model;
        }

        private void CreatePieSeries()
        {
            var model = new PlotModel
            {
                Title = "Pie Series Demo"
            };

            var series = new PieSeries
            {
                StrokeThickness = 2,
                Stroke = OxyColors.White,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0
            };

            series.Slices.Add(new PieSlice("Desktop", 45) { Fill = OxyColors.SteelBlue });
            series.Slices.Add(new PieSlice("Mobile", 35) { Fill = OxyColors.Coral });
            series.Slices.Add(new PieSlice("Tablet", 15) { Fill = OxyColors.MediumSeaGreen });
            series.Slices.Add(new PieSlice("Other", 5) { Fill = OxyColors.DarkSlateGray });

            model.Series.Add(series);
            PlotModel = model;
            UpdateStatus("Displaying Pie Series - Device usage breakdown (4 slices)");
        }

        private void CreateStemSeries()
        {
            var model = CreateBasePlotModel("Stem Series Demo");
            AddStandardAxes(model, "Sample Index", "Value");

            var series = new StemSeries
            {
                Title = "Discrete Signal",
                Color = OxyColors.Navy,
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerFill = OxyColors.SteelBlue,
                MarkerStroke = OxyColors.Navy,
                StrokeThickness = 1.5
            };

            // Generate a discrete sine wave signal
            for (int i = 0; i < 30; i++)
            {
                double x = i;
                double y = Math.Sin(i * 0.3) * (1 + 0.5 * Math.Sin(i * 0.1));
                series.Points.Add(new DataPoint(x, y));
            }

            model.Series.Add(series);
            PlotModel = model;
            UpdateStatus("Displaying Stem Series - Discrete modulated sine wave signal (30 samples)");
        }

        private void CreateTwoColorLineSeries()
        {
            var model = CreateBasePlotModel("Two Color Line Series Demo");
            AddStandardAxes(model, "X", "Y");

            var series = new TwoColorLineSeries
            {
                Title = "Temperature Variation",
                Color = OxyColors.Red,
                Color2 = OxyColors.Blue,
                Limit = 0,
                StrokeThickness = 2
            };

            // Generate data that crosses zero
            for (int i = 0; i < 100; i++)
            {
                double x = i;
                double y = Math.Sin(i * 0.1) * 5 + Math.Cos(i * 0.05) * 3;
                series.Points.Add(new DataPoint(x, y));
            }

            // Add a reference line at the limit
            model.Annotations.Add(new OxyPlot.Annotations.LineAnnotation
            {
                Type = OxyPlot.Annotations.LineAnnotationType.Horizontal,
                Y = 0,
                Color = OxyColors.Gray,
                StrokeThickness = 1,
                LineStyle = LineStyle.Dash,
                Text = "Threshold (0)"
            });

            model.Series.Add(series);
            PlotModel = model;
            UpdateStatus("Displaying Two Color Line Series - Values above/below threshold shown in different colors");
        }

        private void CreateStepSeries()
        {
            var model = CreateBasePlotModel("Step Series Demo");
            AddStandardAxes(model, "Time", "Level");

            var series = new StairStepSeries
            {
                Title = "Digital Signal",
                Color = OxyColors.Purple,
                StrokeThickness = 2,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerFill = OxyColors.Purple
            };

            // Generate a step function (like a digital signal)
            var r = new Random(42);
            double currentLevel = 0;
            for (int i = 0; i < 20; i++)
            {
                series.Points.Add(new DataPoint(i, currentLevel));
                if (r.NextDouble() > 0.5)
                {
                    currentLevel = currentLevel == 0 ? 1 : 0;
                }
                series.Points.Add(new DataPoint(i + 0.999, currentLevel));
            }

            model.Series.Add(series);
            PlotModel = model;
            UpdateStatus("Displaying Step Series - Digital signal with step transitions (20 samples)");
        }

        #endregion
    }
}
