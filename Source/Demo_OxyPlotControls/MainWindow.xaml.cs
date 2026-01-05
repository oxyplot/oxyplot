using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
    /// Interaction logic for MainWindow.xaml.
    /// Demonstrates OxyPlot controls with various series types.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Dependency property for the test axis name binding property.
        /// </summary>
        public static readonly DependencyProperty TestAxisNameBindingProperty =
            DependencyProperty.Register(nameof(TestAxisNameBinding), typeof(string), typeof(MainWindow), new FrameworkPropertyMetadata("Test Y"));

        /// <summary>
        /// Gets or sets the test axis name binding value.
        /// </summary>
        public string TestAxisNameBinding
        {
            get { return (string)GetValue(TestAxisNameBindingProperty); }
            set { SetValue(TestAxisNameBindingProperty, value); }
        }

        /// <summary>
        /// Gets the first set of data points for testing.
        /// </summary>
        public ObservableCollection<DataPoint> Points1 { get; } = new ObservableCollection<DataPoint>(new[] { new DataPoint(0, 0), new DataPoint(1, 2), new DataPoint(2, 3), new DataPoint(4, 5) });

        /// <summary>
        /// Gets the second set of data points for testing.
        /// </summary>
        public ObservableCollection<DataPoint> Points2 { get; } = new ObservableCollection<DataPoint>(new[] { new DataPoint(0, 0), new DataPoint(1, 2), new DataPoint(2, 3), new DataPoint(4, 5) });

        /// <summary>
        /// Gets the third set of data points for testing.
        /// </summary>
        public ObservableCollection<DataPoint> Points3 { get; } = new ObservableCollection<DataPoint>(new[] { new DataPoint(0, 0), new DataPoint(1, 2), new DataPoint(2, 3), new DataPoint(4, 5) });

        /// <summary>
        /// Gets the fourth set of data points for testing.
        /// </summary>
        public ObservableCollection<DataPoint> Points4 { get; } = new ObservableCollection<DataPoint>(new[] { new DataPoint(0, 0), new DataPoint(1, 2), new DataPoint(2, 3), new DataPoint(4, 5) });

        /// <summary>
        /// Gets the area points collection for testing.
        /// </summary>
        public ObservableCollection<AreaPoint> AreaPoints { get; } = new ObservableCollection<AreaPoint>();

        /// <summary>
        /// Gets the scatter points collection for testing.
        /// </summary>
        public ObservableCollection<ScatterPoint> ScatterPoints { get; } = new ObservableCollection<ScatterPoint>();

        /// <summary>
        /// Registry to store demo data for series, keyed by series title.
        /// This allows restoring data after loading XML settings.
        /// </summary>
        private readonly Dictionary<string, object> _demoDataRegistry = new Dictionary<string, object>();

        /// <summary>
        /// Registry to store category axis labels, keyed by axis key.
        /// </summary>
        private readonly Dictionary<string, System.Collections.IEnumerable> _categoryAxisLabelsRegistry = new Dictionary<string, System.Collections.IEnumerable>();

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Trigger line series in combobox (unbound)
            Combobox1.SelectedIndex = 0;
        }

        private List<DataPoint> CreateNormalDist(double x0, double x1, double mean, double variance, int n = 1001)
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

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            // Content rendered event handler
        }

        private void PlotPropertiesUpdated(Plot targetPlot)
        {
            Console.WriteLine("Plot Properties");
        }

        private void OxyPlotToolBar_PropertiesCalled(OxyPlot.Wpf.Plot targetPlot, bool openProperties, OxyPlotControls.OxyPlotPropertiesControl.PropertyEXP? propertyExpander, object selectedObject)
        {
            if (propertyExpander.HasValue)
            {
                PropertiesControl.ExpandProperty(propertyExpander.Value, selectedObject);
            }
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            string saveFile = GenericControls.GeneralMethods.FileSaveDialog("Plot Settings(*.xml) |*.xml", true);
            if (string.IsNullOrEmpty(saveFile)) return;

            // Save plot data
            try
            {
                if (File.Exists(saveFile)) File.Delete(saveFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error attempting to delete existing file '" + saveFile + "'.\n\n" + ex.Message);
                return;
            }

            using (XmlWriter writer = XmlWriter.Create(saveFile, new XmlWriterSettings { Indent = true }))
            {
                OxyPlotControls.OxyPlotSettingsSerializer.ToXelement(TestPlot).WriteTo(writer);
            }
        }

        private void LoadSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            string fileToOpen = GenericControls.GeneralMethods.FileOpenDialog("Plot Settings(*.xml) |*.xml");
            if (string.IsNullOrEmpty(fileToOpen)) return;
            if (System.IO.Path.GetExtension(fileToOpen) != ".xml") return;

            // Open plot data
            if (File.Exists(fileToOpen))
            {
                var document = new XmlDocument();
                document.Load(fileToOpen);

                // For testing load - Clear the axes, annotations, and series
                TestPlot.Annotations.Clear();
                TestPlot.Series.Clear();
                TestPlot.Axes.Clear();

                OxyPlotControls.OxyPlotSettingsSerializer.FromXelement(TestPlot, XElement.Parse(document.GetElementsByTagName(OxyPlotControls.OxyPlotSettingsSerializer.OxyplotPropertiesTag)[0].OuterXml));

                // Repopulate series data from the registry
                RepopulateAllSeriesData();

                // Repopulate category axis labels
                RepopulateCategoryAxisLabels();

                TestPlot.ResetAllAxes();
                TestPlot.InvalidatePlot(true);
            }
        }

        /// <summary>
        /// Repopulates all series with demo data after loading XML settings.
        /// </summary>
        private void RepopulateAllSeriesData()
        {
            foreach (var series in TestPlot.Series)
            {
                RepopulateSeriesData(series);
            }
        }

        /// <summary>
        /// Repopulates category axis labels from the registry.
        /// </summary>
        private void RepopulateCategoryAxisLabels()
        {
            foreach (var axis in TestPlot.Axes)
            {
                if (axis is OxyPlot.Wpf.CategoryAxis catAxis)
                {
                    string axisKey = string.IsNullOrEmpty(catAxis.Key) ? "default" : catAxis.Key;

                    if (_categoryAxisLabelsRegistry.ContainsKey(axisKey))
                    {
                        catAxis.ItemsSource = _categoryAxisLabelsRegistry[axisKey];
                    }
                }
            }
        }

        /// <summary>
        /// Repopulates a single series with demo data.
        /// First tries to match by title, then falls back to type-based defaults.
        /// </summary>
        private void RepopulateSeriesData(OxyPlot.Wpf.Series series)
        {
            string title = series.Title;

            // Try to find data in registry by title
            if (!string.IsNullOrEmpty(title) && _demoDataRegistry.ContainsKey(title))
            {
                var storedData = _demoDataRegistry[title];
                ApplyDataToSeries(series, storedData);
                return;
            }

            // Fall back to generating type-appropriate demo data
            GenerateDefaultDemoData(series);
        }

        /// <summary>
        /// Applies stored data to a series.
        /// </summary>
        private void ApplyDataToSeries(OxyPlot.Wpf.Series series, object data)
        {
            switch (series)
            {
                // AreaSeries must come before LineSeries since it extends LineSeries
                case OxyPlot.Wpf.AreaSeries areaSeries:
                    if (data is List<DummyMultiPurposePoint> areaPoints)
                    {
                        areaSeries.ItemsSource = areaPoints;
                    }
                    else if (data is List<DataPoint> areaDataPoints)
                    {
                        var internalSeries = (OxyPlot.Series.AreaSeries)areaSeries.InternalSeries;
                        internalSeries.Points.Clear();
                        foreach (var p in areaDataPoints)
                        {
                            internalSeries.Points.Add(p);
                        }
                    }
                    break;

                case OxyPlot.Wpf.LineSeries lineSeries:
                    if (data is List<DummyMultiPurposePoint> linePoints)
                    {
                        lineSeries.ItemsSource = linePoints;
                    }
                    else if (data is List<DataPoint> dataPoints)
                    {
                        var internalSeries = (OxyPlot.Series.LineSeries)lineSeries.InternalSeries;
                        internalSeries.Points.Clear();
                        foreach (var p in dataPoints)
                        {
                            internalSeries.Points.Add(p);
                        }
                    }
                    else if (data is System.Collections.IEnumerable enumerable)
                    {
                        // General IEnumerable (e.g., TimeSeries) - use as ItemsSource
                        lineSeries.ItemsSource = enumerable;
                    }
                    break;

                case OxyPlot.Wpf.ScatterErrorSeries scatterErrorSeries:
                    if (data is List<DummyMultiPurposePoint> scatterErrorPoints)
                    {
                        scatterErrorSeries.ItemsSource = scatterErrorPoints;
                    }
                    else if (data is List<ScatterErrorPoint> errorPoints)
                    {
                        var internalSeries = (OxyPlot.Series.ScatterErrorSeries)scatterErrorSeries.InternalSeries;
                        internalSeries.Points.Clear();
                        foreach (var p in errorPoints)
                        {
                            internalSeries.Points.Add(p);
                        }
                    }
                    break;

                case OxyPlot.Wpf.ScatterPointSeries scatterSeries:
                    if (data is List<DummyMultiPurposePoint> scatterPoints)
                    {
                        scatterSeries.ItemsSource = scatterPoints;
                    }
                    else if (data is List<ScatterPoint> points)
                    {
                        var internalSeries = (OxyPlot.Series.ScatterSeries)scatterSeries.InternalSeries;
                        internalSeries.Points.Clear();
                        foreach (var p in points)
                        {
                            internalSeries.Points.Add(p);
                        }
                    }
                    break;

                case OxyPlot.Wpf.HistogramSeries histogramSeries:
                    if (data is List<HistogramItem> histogramItems)
                    {
                        histogramSeries.ItemsSource = histogramItems;
                    }
                    break;

                case OxyPlot.Wpf.ColumnSeries columnSeries:
                    if (data is List<ColumnItem> columnItems)
                    {
                        columnSeries.ItemsSource = columnItems;
                    }
                    break;

                case OxyPlot.Wpf.BarSeries barSeries:
                    if (data is List<DummyMultiPurposePoint> barPoints)
                    {
                        barSeries.ItemsSource = barPoints;
                    }
                    else if (data is List<BarItem> barItems)
                    {
                        barSeries.Items.Clear();
                        foreach (var item in barItems)
                        {
                            barSeries.Items.Add(item);
                        }
                    }
                    break;

                case OxyPlot.Wpf.BoxPlotSeries boxPlotSeries:
                    if (data is List<BoxPlotItem> boxPlotItems)
                    {
                        boxPlotSeries.ItemsSource = boxPlotItems;
                    }
                    break;

                case OxyPlot.Wpf.HeatMapSeries heatMapSeries:
                    if (data is double[,] heatMapData)
                    {
                        heatMapSeries.Data = heatMapData;
                    }
                    break;
            }
        }

        /// <summary>
        /// Generates default demo data for a series based on its type.
        /// </summary>
        private void GenerateDefaultDemoData(OxyPlot.Wpf.Series series)
        {
            switch (series)
            {
                // AreaSeries must come before LineSeries since it extends LineSeries
                case OxyPlot.Wpf.AreaSeries areaSeries:
                    var areaInternal = (OxyPlot.Series.AreaSeries)areaSeries.InternalSeries;
                    areaInternal.Points.Clear();
                    foreach (var p in CreateNormalDist(-5, 5, 0, 1))
                    {
                        areaInternal.Points.Add(p);
                    }
                    break;

                case OxyPlot.Wpf.LineSeries lineSeries:
                    var lineInternal = (OxyPlot.Series.LineSeries)lineSeries.InternalSeries;
                    lineInternal.Points.Clear();
                    foreach (var p in CreateNormalDist(-5, 5, 0, 1))
                    {
                        lineInternal.Points.Add(p);
                    }
                    break;

                case OxyPlot.Wpf.ScatterErrorSeries scatterErrorSeries:
                    var scatterErrorInternal = (OxyPlot.Series.ScatterErrorSeries)scatterErrorSeries.InternalSeries;
                    scatterErrorInternal.Points.Clear();
                    var r1 = new Random(314);
                    for (int i = 0; i < 30; i++)
                    {
                        double x = r1.NextDouble();
                        double y = r1.NextDouble();
                        int size = r1.Next(5, 15);
                        int colorValue = r1.Next(100, 1000);
                        scatterErrorInternal.Points.Add(new ScatterErrorPoint(x, y, x / 5, x / 10, y / 5, y / 10, size, colorValue));
                    }
                    break;

                case OxyPlot.Wpf.ScatterPointSeries scatterSeries:
                    var scatterInternal = (OxyPlot.Series.ScatterSeries)scatterSeries.InternalSeries;
                    scatterInternal.Points.Clear();
                    var r2 = new Random(314);
                    for (int i = 0; i < 50; i++)
                    {
                        double x = r2.NextDouble();
                        double y = r2.NextDouble();
                        int size = r2.Next(5, 15);
                        int colorValue = r2.Next(100, 1000);
                        scatterInternal.Points.Add(new ScatterPoint(x, y, size, colorValue));
                    }
                    break;

                case OxyPlot.Wpf.HistogramSeries histogramSeries:
                    var histItems = new List<HistogramItem>
                    {
                        new HistogramItem(0, 200, 400),
                        new HistogramItem(200, 400, 100),
                        new HistogramItem(400, 600, 800),
                        new HistogramItem(600, 800, 2000)
                    };
                    histogramSeries.ItemsSource = histItems;
                    break;

                case OxyPlot.Wpf.ColumnSeries columnSeries:
                    var colItems = new List<ColumnItem>
                    {
                        new ColumnItem(30, 0),
                        new ColumnItem(80, 1),
                        new ColumnItem(10, 2),
                        new ColumnItem(50, 3)
                    };
                    columnSeries.ItemsSource = colItems;
                    break;

                case OxyPlot.Wpf.BarSeries barSeries:
                    barSeries.Items.Clear();
                    barSeries.Items.Add(new BarItem(20));
                    barSeries.Items.Add(new BarItem(35));
                    barSeries.Items.Add(new BarItem(15));
                    barSeries.Items.Add(new BarItem(25));
                    barSeries.Items.Add(new BarItem(30));
                    break;

                case OxyPlot.Wpf.BoxPlotSeries boxPlotSeries:
                    var boxItems = new List<BoxPlotItem>
                    {
                        new BoxPlotItem(0, 3, 5, 7, 20, 30),
                        new BoxPlotItem(1, 4, 12, 14, 30, 40),
                        new BoxPlotItem(2, 7, 10, 12, 25, 35),
                        new BoxPlotItem(3, 12, 15, 17, 30, 40)
                    };
                    boxPlotSeries.ItemsSource = boxItems;
                    break;

                case OxyPlot.Wpf.HeatMapSeries heatMapSeries:
                    // Generate 2D normal distribution
                    var singleData = new double[100];
                    for (int x = 0; x < 100; x++)
                    {
                        singleData[x] = Math.Exp(((1.0 / 2) * -1) * Math.Pow(((double)x - 50) / 20, 2));
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

        /// <summary>
        /// Registers demo data for a series in the registry.
        /// </summary>
        private void RegisterDemoData(string title, object data)
        {
            if (string.IsNullOrEmpty(title)) return;
            _demoDataRegistry[title] = data;
        }

        /// <summary>
        /// Registers category axis labels in the registry.
        /// </summary>
        private void RegisterCategoryAxisLabels(string axisKey, System.Collections.IEnumerable labels)
        {
            if (string.IsNullOrEmpty(axisKey)) axisKey = "default";
            _categoryAxisLabelsRegistry[axisKey] = labels;
        }

        private void PropertiesControl_ClosePropertiesCalled(OxyPlotControls.OxyPlotPropertiesControl propertiesControl)
        {
            MessageBox.Show("I am not going to close. Sorry not sorry.");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)Combobox1.SelectedItem)?.Content == null)
            {
                return;
            }

            switch (((ComboBoxItem)Combobox1.SelectedItem).Content.ToString())
            {
                case "Line Series":
                    LineSeries_Create(false);
                    break;
                case "Line Series (Bound)":
                    LineSeries_Create(true);
                    break;
                case "Scatter Series":
                    ScatterSeries_Create(false);
                    break;
                case "Scatter Series (Bound)":
                    ScatterSeries_Create(true);
                    break;
                case "Histogram Series":
                    HistogramSeries_Create(false);
                    break;
                case "Histogram Series (Bound)":
                    HistogramSeries_Create(true);
                    break;
                case "Column Series":
                    ColumnSeries_Create(false);
                    break;
                case "Column Series (Bound)":
                    ColumnSeries_Create(true);
                    break;
                case "Bar Series":
                    BarSeries_Create(false);
                    break;
                case "Bar Series (Bound)":
                    BarSeries_Create(true);
                    break;
                case "Area Series":
                    AreaSeries_Create(false);
                    break;
                case "Area Series (Bound)":
                    AreaSeries_Create(true);
                    break;
                case "Heat Map Series":
                    HeatMapSeries_Create(false);
                    break;
                case "Heat Map Series (Bound)":
                    HeatMapSeries_Create(true);
                    break;
                case "Scatter Error Series":
                    ScatterErrorSeries_Create(false);
                    break;
                case "Scatter Error Series (Bound)":
                    ScatterErrorSeries_Create(true);
                    break;
                case "Box Plot Series":
                    BoxPlotSeries_Create(false);
                    break;
                case "Box Plot Series (Bound)":
                    BoxPlotSeries_Create(true);
                    break;
                case "Date Time Series":
                    DateTimeSeries_Create(false);
                    break;
            }
        }

        /// <summary>
        /// Dummy class for testing binding of series.
        /// </summary>
        private class DummyMultiPurposePoint
        {
            public double Xval { get; set; }
            public double Yval { get; set; }
            public double X2val { get; set; }
            public double Y2val { get; set; }
            public double SizeVal { get; set; }
            public double ColorVal { get; set; }
            public double XLowerError { get; set; }
            public double XUpperError { get; set; }
            public double YLowerError { get; set; }
            public double YUpperError { get; set; }
            public string LabelVal { get; set; }
            public double Position { get; set; }
            public double LowerWhisker { get; set; }
            public double BoxMinimum { get; set; }
            public double Median { get; set; }
            public double BoxMaximum { get; set; }
            public double UpperWhisker { get; set; }
            public OxyColor Color { get; set; }
        }

        private void LineSeries_Create(bool boundBool)
        {
            TestPlot.Series.Clear();
            TestPlot.Axes.Clear();
            TestPlot.ActualModel.Series.Clear();
            TestPlot.ActualModel.Axes.Clear();

            TestPlot.Title = "Line Series";

            var yAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                TitleFontSize = 20,
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test Y",
                Key = "y"
            };

            var xAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                Position = AxisPosition.Bottom,
                TitleFontSize = 20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test X",
                Key = "x"
            };

            TestPlot.Axes.Add(xAxis);
            TestPlot.Axes.Add(yAxis);

            var lineSeries1 = new OxyPlot.Wpf.LineSeries { Title = "Test Line Series 1" };

            if (boundBool)
            {
                var lst = new List<DummyMultiPurposePoint>();
                lineSeries1.DataFieldX = "Xval";
                lineSeries1.DataFieldY = "Yval";

                foreach (var p in CreateNormalDist(-10, 10, 0, 2))
                {
                    lst.Add(new DummyMultiPurposePoint { Xval = p.X, Yval = p.Y });
                }

                lineSeries1.ItemsSource = lst;
                RegisterDemoData(lineSeries1.Title, lst);
            }
            else
            {
                var points1 = new List<DataPoint>();
                foreach (var p in CreateNormalDist(-5, 5, 0, 1))
                {
                    ((OxyPlot.Series.LineSeries)lineSeries1.InternalSeries).Points.Add(p);
                    points1.Add(p);
                }
                RegisterDemoData(lineSeries1.Title, points1);
            }

            var lineSeries2 = new OxyPlot.Wpf.LineSeries { Title = "Test Line Series 2" };

            if (boundBool)
            {
                var lst = new List<DummyMultiPurposePoint>();
                lineSeries2.DataFieldX = "Xval";
                lineSeries2.DataFieldY = "Yval";

                foreach (var p in CreateNormalDist(-4, 4, 0, 0.5))
                {
                    lst.Add(new DummyMultiPurposePoint { Xval = p.X, Yval = p.Y });
                }

                lineSeries2.ItemsSource = lst;
                RegisterDemoData(lineSeries2.Title, lst);
            }
            else
            {
                var points2 = new List<DataPoint>();
                foreach (var p in CreateNormalDist(-2, 2, 0, 0.25))
                {
                    ((OxyPlot.Series.LineSeries)lineSeries2.InternalSeries).Points.Add(p);
                    points2.Add(p);
                }
                RegisterDemoData(lineSeries2.Title, points2);
            }

            TestPlot.Series.Add(lineSeries1);
            TestPlot.Series.Add(lineSeries2);

            TestPlot.ResetAllAxes();
            TestPlot.InvalidatePlot(true);
        }

        private void ScatterSeries_Create(bool boundBool)
        {
            TestPlot.Series.Clear();
            TestPlot.Axes.Clear();

            TestPlot.Title = "Scatter Series";

            var yAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                TitleFontSize = 20,
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test Y",
                Key = "y"
            };

            var xAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                Position = AxisPosition.Bottom,
                TitleFontSize = 20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test X",
                Key = "x"
            };

            TestPlot.Axes.Add(xAxis);
            TestPlot.Axes.Add(yAxis);

            var scatterSeries = new OxyPlot.Wpf.ScatterPointSeries
            {
                MarkerType = MarkerType.Circle,
                Title = "Scatter Series"
            };

            var r = new Random(314);

            if (boundBool)
            {
                var lst = new List<DummyMultiPurposePoint>();
                scatterSeries.DataFieldX = "Xval";
                scatterSeries.DataFieldY = "Yval";
                scatterSeries.DataFieldSize = "SizeVal";
                scatterSeries.DataFieldValue = "ColorVal";

                for (int i = 0; i < 50; i++)
                {
                    lst.Add(new DummyMultiPurposePoint
                    {
                        Xval = r.NextDouble(),
                        Yval = r.NextDouble(),
                        SizeVal = r.Next(1, 5),
                        ColorVal = r.Next(50, 500)
                    });
                }

                scatterSeries.ItemsSource = lst;
                RegisterDemoData(scatterSeries.Title, lst);
            }
            else
            {
                var points = new List<ScatterPoint>();
                for (int i = 0; i < 100; i++)
                {
                    double x = r.NextDouble();
                    double y = r.NextDouble();
                    int size = r.Next(5, 15);
                    int colorValue = r.Next(100, 1000);

                    var pt = new ScatterPoint(x, y, size, colorValue);
                    ((OxyPlot.Series.ScatterSeries)scatterSeries.InternalSeries).Points.Add(pt);
                    points.Add(pt);
                }
                RegisterDemoData(scatterSeries.Title, points);
            }

            TestPlot.Series.Add(scatterSeries);

            // Add the color Axis
            var lca = new OxyPlot.Wpf.LinearColorAxis { Position = AxisPosition.Right };
            lca.PaletteSize = 200;
            lca.LowColor = Colors.Blue;
            lca.HighColor = Colors.Red;

            var gList = new List<GradientStop>
            {
                new GradientStop(Colors.Red, 0),
                new GradientStop(Colors.Yellow, 0.5),
                new GradientStop(Colors.Green, 1)
            };
            lca.GradientStops = new GradientStopCollection(gList);
            TestPlot.Axes.Add(lca);

            TestPlot.ResetAllAxes();
            TestPlot.InvalidatePlot(true);
        }

        private void ScatterErrorSeries_Create(bool boundBool)
        {
            TestPlot.Series.Clear();
            TestPlot.Axes.Clear();

            TestPlot.Title = "Scatter Series";

            var yAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                TitleFontSize = 20,
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test Y",
                Key = "y"
            };

            var xAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                Position = AxisPosition.Bottom,
                TitleFontSize = 20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test X",
                Key = "x"
            };

            TestPlot.Axes.Add(xAxis);
            TestPlot.Axes.Add(yAxis);

            var scatterErrorSeries = new OxyPlot.Wpf.ScatterErrorSeries
            {
                MarkerType = MarkerType.Circle,
                Title = "Scatter Error Series"
            };

            var r = new Random(314);

            if (boundBool)
            {
                var lst = new List<DummyMultiPurposePoint>();
                scatterErrorSeries.DataFieldX = "Xval";
                scatterErrorSeries.DataFieldY = "Yval";
                scatterErrorSeries.DataFieldSize = "SizeVal";
                scatterErrorSeries.DataFieldValue = "ColorVal";
                scatterErrorSeries.DataFieldLowerErrorX = "XLowerError";
                scatterErrorSeries.DataFieldUpperErrorX = "XUpperError";
                scatterErrorSeries.DataFieldLowerErrorY = "YLowerError";
                scatterErrorSeries.DataFieldUpperErrorY = "YUpperError";

                for (int i = 0; i < 20; i++)
                {
                    var dsp = new DummyMultiPurposePoint
                    {
                        Xval = r.NextDouble(),
                        Yval = r.NextDouble(),
                        SizeVal = r.Next(5, 15),
                        ColorVal = r.Next(100, 1000)
                    };
                    dsp.XLowerError = dsp.Xval - (dsp.Xval / 10);
                    dsp.XUpperError = dsp.Xval + (dsp.Xval / 5);
                    dsp.YUpperError = dsp.Yval + (dsp.Yval / 5);
                    dsp.YLowerError = dsp.Yval - (dsp.Yval / 10);
                    lst.Add(dsp);
                }

                scatterErrorSeries.ItemsSource = lst;
                RegisterDemoData(scatterErrorSeries.Title, lst);
            }
            else
            {
                var points = new List<ScatterErrorPoint>();
                for (int i = 0; i < 50; i++)
                {
                    double x = r.NextDouble();
                    double y = r.NextDouble();
                    int size = r.Next(5, 15);
                    int colorValue = r.Next(100, 1000);

                    var pt = new ScatterErrorPoint(x, y, x - (x / 5), x + (x / 10), y - (y / 5), y + (y / 10), size, colorValue);
                    ((OxyPlot.Series.ScatterErrorSeries)scatterErrorSeries.InternalSeries).Points.Add(pt);
                    points.Add(pt);
                }
                RegisterDemoData(scatterErrorSeries.Title, points);
            }

            TestPlot.Series.Add(scatterErrorSeries);

            // Add the color Axis
            var lca = new OxyPlot.Wpf.LinearColorAxis { Position = AxisPosition.Right };
            lca.PaletteSize = 200;
            lca.LowColor = Colors.Blue;
            lca.HighColor = Colors.Red;

            var gList = new List<GradientStop>
            {
                new GradientStop(Colors.Red, 0),
                new GradientStop(Colors.Yellow, 0.5),
                new GradientStop(Colors.Green, 1)
            };
            lca.GradientStops = new GradientStopCollection(gList);
            TestPlot.Axes.Add(lca);

            TestPlot.ResetAllAxes();
            TestPlot.InvalidatePlot(true);
        }

        private void HeatMapSeries_Create(bool boundBool)
        {
            TestPlot.Series.Clear();
            TestPlot.Axes.Clear();

            TestPlot.Title = "Heat Map";

            var yAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                TitleFontSize = 20,
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test Y",
                Key = "y"
            };

            var xAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                Position = AxisPosition.Bottom,
                TitleFontSize = 20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test X",
                Key = "x"
            };

            TestPlot.Axes.Add(xAxis);
            TestPlot.Axes.Add(yAxis);

            // Add the color Axis
            var lca = new OxyPlot.Wpf.LinearColorAxis { Position = AxisPosition.Right };
            lca.PaletteSize = 200;
            lca.LowColor = Colors.Blue;
            lca.HighColor = Colors.Red;

            var gList = new List<GradientStop>
            {
                new GradientStop(Colors.Red, 0),
                new GradientStop(Colors.Yellow, 0.5),
                new GradientStop(Colors.Green, 1)
            };
            lca.GradientStops = new GradientStopCollection(gList);
            TestPlot.Axes.Add(lca);

            var wpfHeatMapSeries = new OxyPlot.Wpf.HeatMapSeries
            {
                X0 = 0,
                X1 = 99,
                Y0 = 0,
                Y1 = 99,
                Interpolate = true,
                Title = "Heat Map Series"
            };

            // Generate 1d normal distribution
            var singleData = new double[100];
            for (int x = 0; x < 100; x++)
            {
                singleData[x] = Math.Exp(((1.0 / 2) * -1) * Math.Pow(((double)x - 50) / 20, 2));
            }

            // Generate 2d normal distribution
            var data = new double[100, 100];
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    data[y, x] = singleData[x] * singleData[(y + 30) % 100] * 100;
                }
            }

            wpfHeatMapSeries.Data = data;
            RegisterDemoData(wpfHeatMapSeries.Title, data);

            // Change render method
            ((OxyPlot.Series.HeatMapSeries)wpfHeatMapSeries.InternalSeries).RenderMethod = HeatMapRenderMethod.Bitmap;

            TestPlot.Series.Add(wpfHeatMapSeries);

            TestPlot.ResetAllAxes();
            TestPlot.InvalidatePlot(true);
        }

        private void HistogramSeries_Create(bool boundBool)
        {
            TestPlot.Series.Clear();
            TestPlot.Axes.Clear();

            TestPlot.Title = "Histogram Series";

            var yAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                TitleFontSize = 20,
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test Y",
                Key = "y"
            };

            var xAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                Position = AxisPosition.Bottom,
                TitleFontSize = 20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test X",
                Key = "x"
            };

            TestPlot.Axes.Add(xAxis);
            TestPlot.Axes.Add(yAxis);

            var histogramSeries = new OxyPlot.Wpf.HistogramSeries { Title = "Histogram Series" };

            var vals = new List<HistogramItem>();

            if (boundBool)
            {
                vals.Add(new HistogramItem(200, 400, 100));
                vals.Add(new HistogramItem(0, 200, 500));
                vals.Add(new HistogramItem(400, 600, 800));
                vals.Add(new HistogramItem(600, 800, 2000));

                histogramSeries.ItemsSource = vals;
            }
            else
            {
                // Adding items to the WPF series doesn't work. They have to be added to the internal series.
                var internalSeries = (OxyPlot.Series.HistogramSeries)histogramSeries.InternalSeries;
                vals.Add(new HistogramItem(0, 200, 400));
                vals.Add(new HistogramItem(200, 400, 100));
                vals.Add(new HistogramItem(400, 600, 800));
                vals.Add(new HistogramItem(600, 800, 2000));
                foreach (var item in vals)
                {
                    internalSeries.Items.Add(item);
                }
            }

            RegisterDemoData(histogramSeries.Title, vals);
            TestPlot.Series.Add(histogramSeries);

            TestPlot.ResetAllAxes();
            TestPlot.InvalidatePlot(true);
        }

        private void BoxPlotSeries_Create(bool boundBool)
        {
            TestPlot.Series.Clear();
            TestPlot.Axes.Clear();

            TestPlot.Title = "Box Plot Series";

            var yAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                TitleFontSize = 20,
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test Y",
                Key = "y"
            };

            var categoryLabels = new[] { "Math", "Science", "English", "History" };
            var xAxis = new OxyPlot.Wpf.CategoryAxis
            {
                ItemsSource = categoryLabels,
                IsTickCentered = true,
                AxisTitleDistance = 20,
                Position = AxisPosition.Bottom,
                TitleFontSize = 20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test X",
                Key = "x"
            };

            RegisterCategoryAxisLabels(xAxis.Key, categoryLabels);

            var boxPlotSeries = new OxyPlot.Wpf.BoxPlotSeries
            {
                BoxWidth = 0.5,
                WhiskerWidth = 0.5,
                StrokeThickness = 2,
                Title = "Box Plot Series"
            };

            var items = new List<BoxPlotItem>();

            if (boundBool)
            {
                var outliers = new List<double> { 2, 45, 55, 60 };
                var outliers2 = new List<double> { 5, 52, 70 };

                items.Add(new BoxPlotItem(0, 12, 15, 17, 30, 40));
                items.Add(new BoxPlotItem(1, 7, 10, 12, 25, 35) { Outliers = outliers2 });
                items.Add(new BoxPlotItem(2, 4, 12, 14, 30, 40));
                items.Add(new BoxPlotItem(3, 3, 5, 7, 20, 30) { Outliers = outliers });

                boxPlotSeries.ItemsSource = items;
            }
            else
            {
                var outliers = new List<double> { 2, 50 };
                var outliers2 = new List<double> { 60 };

                items.Add(new BoxPlotItem(0, 3, 5, 7, 20, 30) { Outliers = outliers });
                items.Add(new BoxPlotItem(1, 4, 12, 14, 30, 40) { Outliers = outliers2 });
                items.Add(new BoxPlotItem(2, 7, 10, 12, 25, 35));
                items.Add(new BoxPlotItem(3, 12, 15, 17, 30, 40));

                foreach (var item in items)
                {
                    boxPlotSeries.Items.Add(item);
                }
            }

            RegisterDemoData(boxPlotSeries.Title, items);

            TestPlot.Axes.Add(xAxis);
            TestPlot.Axes.Add(yAxis);
            TestPlot.Series.Add(boxPlotSeries);

            TestPlot.ResetAllAxes();
            TestPlot.InvalidatePlot(true);
        }

        private void ColumnSeries_Create(bool boundBool)
        {
            TestPlot.Series.Clear();
            TestPlot.Axes.Clear();

            TestPlot.Title = "Column Series";

            var yAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                TitleFontSize = 20,
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test Y",
                Key = "y"
            };

            var categoryLabels = new[] { "Math", "Science", "English", "History" };
            var xAxis = new OxyPlot.Wpf.CategoryAxis
            {
                ItemsSource = categoryLabels,
                AxisTitleDistance = 20,
                Position = AxisPosition.Bottom,
                TitleFontSize = 20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test X",
                Key = "x"
            };

            RegisterCategoryAxisLabels(xAxis.Key, categoryLabels);

            TestPlot.Axes.Add(xAxis);
            TestPlot.Axes.Add(yAxis);

            var colSeries = new OxyPlot.Wpf.ColumnSeries { Title = "Column Series" };

            var vals = new List<ColumnItem>();

            if (boundBool)
            {
                colSeries.ValueField = "Value";
                colSeries.ColorField = "Color";

                vals.Add(new ColumnItem(50, 0) { Color = OxyColors.Red });
                vals.Add(new ColumnItem(10, 1));
                vals.Add(new ColumnItem(30, 2));
                vals.Add(new ColumnItem(20, 3));

                colSeries.ItemsSource = vals;
            }
            else
            {
                vals.Add(new ColumnItem(30, 0));
                vals.Add(new ColumnItem(80, 1));
                vals.Add(new ColumnItem(10, 2) { Color = OxyColors.Red });
                vals.Add(new ColumnItem(50, 3));

                foreach (var item in vals)
                {
                    colSeries.Items.Add(item);
                }
            }

            RegisterDemoData(colSeries.Title, vals);
            TestPlot.Series.Add(colSeries);

            TestPlot.ResetAllAxes();
            TestPlot.InvalidatePlot(true);
        }

        private void BarSeries_Create(bool boundBool)
        {
            TestPlot.Series.Clear();
            TestPlot.Axes.Clear();

            TestPlot.Title = "Bar Series";

            var yAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                TitleFontSize = 20,
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test Y",
                Key = "y"
            };

            var categoryLabels = new[] { "Apple cake", "Baumkuchen", "Bundt Cake", "Chocolate cake", "Carrot cake" };
            var xAxis = new OxyPlot.Wpf.CategoryAxis
            {
                ItemsSource = categoryLabels,
                AxisTitleDistance = 20,
                Position = AxisPosition.Left,
                TitleFontSize = 20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test X",
                Key = "x"
            };

            RegisterCategoryAxisLabels(xAxis.Key, categoryLabels);

            TestPlot.Axes.Add(xAxis);
            TestPlot.Axes.Add(yAxis);

            var barSeries = new OxyPlot.Wpf.BarSeries { Title = "Bar Series" };

            var rand = new Random(314);
            var cakePopularity = new double[5];

            for (int i = 0; i < 5; i++)
            {
                cakePopularity[i] = rand.NextDouble();
            }

            double sum = cakePopularity.Sum();

            if (boundBool)
            {
                barSeries.ValueField = "Xval";
                barSeries.ColorField = "Color";

                var lst = new List<DummyMultiPurposePoint>();

                for (int i = 0; i < 5; i++)
                {
                    lst.Add(new DummyMultiPurposePoint
                    {
                        Xval = cakePopularity[i] / sum * 100,
                        Color = OxyColors.Red
                    });
                }

                barSeries.ItemsSource = lst;
                barSeries.LabelPlacement = LabelPlacement.Inside;
                barSeries.LabelFormatString = "{0:.00}%";
                RegisterDemoData(barSeries.Title, lst);
            }
            else
            {
                var items = new List<BarItem>();
                items.Add(new BarItem(cakePopularity[0] / sum * 100));
                items.Add(new BarItem(cakePopularity[1] / sum * 100));
                items.Add(new BarItem(cakePopularity[2] / sum * 100));
                items.Add(new BarItem(cakePopularity[3] / sum * 100));
                items.Add(new BarItem(cakePopularity[4] / sum * 100));

                foreach (var item in items)
                {
                    barSeries.Items.Add(item);
                }
                RegisterDemoData(barSeries.Title, items);
            }

            TestPlot.Series.Add(barSeries);

            TestPlot.ResetAllAxes();
            TestPlot.InvalidatePlot(true);
        }

        private void AreaSeries_Create(bool boundBool)
        {
            TestPlot.Series.Clear();
            TestPlot.Axes.Clear();
            TestPlot.ActualModel.Series.Clear();
            TestPlot.ActualModel.Axes.Clear();

            TestPlot.Title = "Area Series";

            var yAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                TitleFontSize = 20,
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test Y",
                Key = "y"
            };

            var xAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                Position = AxisPosition.Bottom,
                TitleFontSize = 20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test X",
                Key = "x"
            };

            TestPlot.Axes.Add(xAxis);
            TestPlot.Axes.Add(yAxis);

            var areaSeries1 = new OxyPlot.Wpf.AreaSeries
            {
                Title = "Area Series",
                Fill = Colors.Green
            };

            if (boundBool)
            {
                var lst = new List<DummyMultiPurposePoint>();
                areaSeries1.DataFieldX = "Xval";
                areaSeries1.DataFieldY = "Yval";
                areaSeries1.DataFieldX2 = "X2val";
                areaSeries1.DataFieldY2 = "Y2val";

                foreach (var p in CreateNormalDist(-10, 10, 0, 2))
                {
                    lst.Add(new DummyMultiPurposePoint
                    {
                        Xval = p.X,
                        Yval = p.Y,
                        X2val = p.X / 2,
                        Y2val = p.Y / 2
                    });
                }

                areaSeries1.ItemsSource = lst;
                RegisterDemoData(areaSeries1.Title, lst);
            }
            else
            {
                var points = new List<DataPoint>();
                foreach (var p in CreateNormalDist(-5, 5, 0, 1))
                {
                    ((OxyPlot.Series.AreaSeries)areaSeries1.InternalSeries).Points.Add(p);
                    points.Add(p);
                }
                RegisterDemoData(areaSeries1.Title, points);
            }

            TestPlot.Series.Add(areaSeries1);

            TestPlot.ResetAllAxes();
            TestPlot.InvalidatePlot(true);
        }

        private void DateTimeSeries_Create(bool boundBool)
        {
            TestPlot.Series.Clear();
            TestPlot.Axes.Clear();

            TestPlot.Title = "Date Time Series";

            var yAxis = new OxyPlot.Wpf.LinearAxis
            {
                AxisTitleDistance = 20,
                TitleFontSize = 20,
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test Y",
                Key = "y"
            };

            var xAxis = new OxyPlot.Wpf.DateTimeAxis
            {
                AxisTitleDistance = 20,
                Position = AxisPosition.Bottom,
                TitleFontSize = 20,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dash,
                Title = "Test X",
                Key = "x"
            };

            TestPlot.Axes.Add(xAxis);
            TestPlot.Axes.Add(yAxis);

            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("Demo_OxyPlotControls.USGS_01134500.xml"))
            {
                var s = new StreamReader(resource);
                var t = new TimeSeries(XElement.Parse(s.ReadToEnd()));

                var l = new OxyPlot.Wpf.LineSeries
                {
                    Title = "Time Series Data",
                    Color = Colors.Blue,
                    MarkerFill = Colors.Transparent,
                    StrokeThickness = 1,
                    LineStyle = LineStyle.Solid
                };

                l.ItemsSource = t;
                l.DataFieldX = "Index";
                l.DataFieldY = "Value";
                RegisterDemoData(l.Title, t);

                TestPlot.Series.Add(l);
                TestPlot.InvalidatePlot(true);
            }
        }

        /// <summary>
        /// Represents an area point with two data points.
        /// </summary>
        public class AreaPoint
        {
            /// <summary>
            /// Gets or sets the X1 coordinate.
            /// </summary>
            public double X1 { get; set; }

            /// <summary>
            /// Gets or sets the X2 coordinate.
            /// </summary>
            public double X2 { get; set; }

            /// <summary>
            /// Gets or sets the Y1 coordinate.
            /// </summary>
            public double Y1 { get; set; }

            /// <summary>
            /// Gets or sets the Y2 coordinate.
            /// </summary>
            public double Y2 { get; set; }

            /// <summary>
            /// Initializes a new instance of the AreaPoint class.
            /// </summary>
            /// <param name="p1">The first data point.</param>
            /// <param name="p2">The second data point.</param>
            public AreaPoint(DataPoint p1, DataPoint p2)
            {
                X1 = p1.X;
                X2 = p2.X;
                Y1 = p1.Y;
                Y2 = p2.Y;
            }
        }
    }
}
