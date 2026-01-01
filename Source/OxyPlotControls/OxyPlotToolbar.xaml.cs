using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Wpf;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.IO;
using OxyPlotControls.Dialogs;

namespace OxyPlotControls;

/// <summary>
/// Interactive toolbar for OxyPlot with Pan/Zoom and annotation drawing capabilities.
/// Modernized C# implementation of the VB OxyplotToolbar with PlotView/PlotModel architecture.
/// </summary>
public partial class OxyPlotToolbar : UserControl
{
    #region Fields

    private PlotView? _plotView;
    private Annotation? _targetAddAnnotation;
    private AddToolMode _addAnnotationToolMode = AddToolMode.None;
    private bool _doubleClicked;
    private bool _showPoints;
    private ContextMenu? _contextMenu;

    // Cursors
    private Cursor? _panHandCursor;
    private Cursor? _panHandClosedCursor;
    private Cursor? _zoomCursor;
    private Cursor? _movePointsCursor;
    private Cursor? _addPointCursor;

    #endregion

    #region Enums

    /// <summary>
    /// Modes for adding annotations to the plot.
    /// </summary>
    private enum AddToolMode
    {
        None,
        AddArrowAnnotation,
        AddTextAnnotation,
        AddVerticalLineAnnotation,
        AddHorizontalLineAnnotation,
        AddRectangleAnnotation,
        AddEllipseAnnotation,
        AddPointAnnotation,
        AddPolygonAnnotation,
        AddPolylineAnnotation
    }

    #endregion

    #region Dependency Properties

    /// <summary>
    /// Dependency property for the PlotView.
    /// </summary>
    public static readonly DependencyProperty PlotViewProperty =
        DependencyProperty.Register(
            nameof(PlotView),
            typeof(PlotView),
            typeof(OxyPlotToolbar),
            new PropertyMetadata(null, OnPlotViewChanged));

    /// <summary>
    /// Gets or sets the PlotView that this toolbar controls.
    /// </summary>
    public PlotView? PlotView
    {
        get => (PlotView?)GetValue(PlotViewProperty);
        set => SetValue(PlotViewProperty, value);
    }

    private static void OnPlotViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is OxyPlotToolbar toolbar)
        {
            toolbar.OnPlotViewChanged(e.OldValue as PlotView, e.NewValue as PlotView);
        }
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="OxyPlotToolbar"/> class.
    /// </summary>
    public OxyPlotToolbar()
    {
        InitializeComponent();
        InitializeCursors();
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes custom cursors from resources.
    /// </summary>
    private void InitializeCursors()
    {
        try
        {
            _panHandCursor = TryGetResource<Cursor>("PanHandCursor");
            _panHandClosedCursor = TryGetResource<Cursor>("PanHandClosedCursor");
            _zoomCursor = TryGetResource<Cursor>("ZoomCursor");
            _movePointsCursor = TryGetResource<Cursor>("MovePointsCursor");
            _addPointCursor = TryGetResource<Cursor>("AddPointCursor");
        }
        catch
        {
            // Fallback to standard cursors if custom cursors can't be loaded
            _panHandCursor = Cursors.Hand;
            _panHandClosedCursor = Cursors.Hand;
            _zoomCursor = Cursors.Cross;
            _movePointsCursor = Cursors.SizeAll;
            _addPointCursor = Cursors.Cross;
        }
    }

    private T? TryGetResource<T>(string key) where T : class
    {
        return Resources.Contains(key) ? Resources[key] as T : null;
    }

    /// <summary>
    /// Called when the PlotView changes.
    /// </summary>
    private void OnPlotViewChanged(PlotView? oldView, PlotView? newView)
    {
        // Unsubscribe from old view
        if (oldView?.ActualModel != null)
        {
            oldView.ActualModel.MouseDown -= PlotModel_MouseDown;
            oldView.ActualModel.MouseMove -= PlotModel_MouseMove;
            oldView.ActualModel.MouseUp -= PlotModel_MouseUp;
        }

        _plotView = newView;

        // Subscribe to new view
        if (newView?.ActualModel != null)
        {
            newView.ActualModel.MouseDown += PlotModel_MouseDown;
            newView.ActualModel.MouseMove += PlotModel_MouseMove;
            newView.ActualModel.MouseUp += PlotModel_MouseUp;

            // Add PlotView to container
            if (!PlotContainer.Children.Contains(newView))
            {
                PlotContainer.Children.Insert(0, newView);
            }
        }

        SetCursor();
    }

    #endregion

    #region Mouse Mode Buttons

    private void PointerButton_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();
        SetCursor();
    }

    private void PanButton_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();
        SetCursor();
    }

    private void ZoomButton_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();
        SetCursor();
    }

    private void ZoomAllButton_Click(object sender, RoutedEventArgs e)
    {
        if (_plotView?.ActualModel == null) return;

        _plotView.ActualModel.ResetAllAxes();
        _plotView.InvalidatePlot(false);
    }

    #endregion

    #region Annotation Add Buttons

    private void AddArrowAnnotation_Click(object sender, RoutedEventArgs e)
    {
        StartAddAnnotation(AddToolMode.AddArrowAnnotation);
    }

    private void AddTextAnnotation_Click(object sender, RoutedEventArgs e)
    {
        StartAddAnnotation(AddToolMode.AddTextAnnotation);
    }

    private void AddVerticalLineAnnotation_Click(object sender, RoutedEventArgs e)
    {
        StartAddAnnotation(AddToolMode.AddVerticalLineAnnotation);
    }

    private void AddHorizontalLineAnnotation_Click(object sender, RoutedEventArgs e)
    {
        StartAddAnnotation(AddToolMode.AddHorizontalLineAnnotation);
    }

    private void AddRectangleAnnotation_Click(object sender, RoutedEventArgs e)
    {
        StartAddAnnotation(AddToolMode.AddRectangleAnnotation);
    }

    private void AddEllipseAnnotation_Click(object sender, RoutedEventArgs e)
    {
        StartAddAnnotation(AddToolMode.AddEllipseAnnotation);
    }

    private void AddPointAnnotation_Click(object sender, RoutedEventArgs e)
    {
        StartAddAnnotation(AddToolMode.AddPointAnnotation);
    }

    private void AddPolygonAnnotation_Click(object sender, RoutedEventArgs e)
    {
        StartAddAnnotation(AddToolMode.AddPolygonAnnotation);
    }

    private void AddPolylineAnnotation_Click(object sender, RoutedEventArgs e)
    {
        StartAddAnnotation(AddToolMode.AddPolylineAnnotation);
    }

    #endregion

    #region Export Buttons

    private static readonly string[] BadCharacters = { ":", "\\", "/", "?", "*", "[", "]" };

    private void ExportDataButton_Click(object sender, RoutedEventArgs e)
    {
        if (_plotView?.ActualModel == null) return;

        var tableList = new List<DataTable>();
        int tableCount = 0;

        foreach (var series in _plotView.ActualModel.Series)
        {
            var dataTable = new DataTable("Series");
            string seriesName = "";
            tableCount++;

            switch (series)
            {
                case OxyPlot.Series.LineSeries lineSeries:
                    seriesName = GetSeriesName(lineSeries.Title, "LineSeries", tableCount);

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_x", typeof(string));
                    dataTable.Columns.Add(seriesName + "_y", typeof(string));

                    if (lineSeries.ItemsSource != null)
                    {
                        var dataList = lineSeries.ItemsSource as IEnumerable<DataPoint>;
                        if (dataList != null)
                        {
                            foreach (var seriesValue in dataList.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y);
                            }
                        }
                        else
                        {
                            foreach (var obj in lineSeries.ItemsSource.Cast<object>())
                            {
                                var propX = obj.GetType().GetProperty(lineSeries.DataFieldX);
                                var xVal = propX?.GetValue(obj, null)?.ToString() ?? "";
                                var propY = obj.GetType().GetProperty(lineSeries.DataFieldY);
                                var yVal = propY?.GetValue(obj, null)?.ToString() ?? "";
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, xVal, yVal);
                            }
                        }
                    }
                    else
                    {
                        foreach (var seriesValue in lineSeries.Points)
                        {
                            dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y);
                        }
                    }
                    break;

                case OxyPlot.Series.ScatterSeries scatterSeries:
                    seriesName = GetSeriesName(scatterSeries.Title, "ScatterSeries", tableCount);

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_x", typeof(string));
                    dataTable.Columns.Add(seriesName + "_y", typeof(string));

                    if (scatterSeries.ItemsSource != null)
                    {
                        var dataList = scatterSeries.ItemsSource as IEnumerable<ScatterPoint>;
                        if (dataList != null)
                        {
                            foreach (var seriesValue in dataList.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y);
                            }
                        }
                        else
                        {
                            foreach (var obj in scatterSeries.ItemsSource.Cast<object>())
                            {
                                var propX = obj.GetType().GetProperty(scatterSeries.DataFieldX);
                                var xVal = propX?.GetValue(obj, null)?.ToString() ?? "";
                                var propY = obj.GetType().GetProperty(scatterSeries.DataFieldY);
                                var yVal = propY?.GetValue(obj, null)?.ToString() ?? "";
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, xVal, yVal);
                            }
                        }
                    }
                    else
                    {
                        foreach (var seriesValue in scatterSeries.Points)
                        {
                            dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y);
                        }
                    }
                    break;

                case OxyPlot.Series.HistogramSeries histogramSeries:
                    seriesName = GetSeriesName(histogramSeries.Title, "HistogramSeries", tableCount);

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_rangeStart", typeof(string));
                    dataTable.Columns.Add(seriesName + "_rangeEnd", typeof(string));
                    dataTable.Columns.Add(seriesName + "_area", typeof(string));

                    if (histogramSeries.ItemsSource != null)
                    {
                        var dataList = histogramSeries.ItemsSource as IEnumerable<HistogramItem>;
                        if (dataList != null)
                        {
                            foreach (var seriesValue in dataList.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.RangeStart, seriesValue.RangeEnd, seriesValue.Area);
                            }
                        }
                    }
                    else
                    {
                        foreach (var seriesItem in histogramSeries.Items)
                        {
                            dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesItem.RangeStart, seriesItem.RangeEnd, seriesItem.Area);
                        }
                    }
                    break;

                case OxyPlot.Series.ColumnSeries columnSeries:
                    seriesName = GetSeriesName(columnSeries.Title, "ColumnSeries", tableCount);

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_categoryIndex", typeof(string));
                    dataTable.Columns.Add(seriesName + "_color", typeof(string));
                    dataTable.Columns.Add(seriesName + "_value", typeof(string));

                    if (columnSeries.ItemsSource != null)
                    {
                        var dataList = columnSeries.ItemsSource as IEnumerable<ColumnItem>;
                        if (dataList != null)
                        {
                            foreach (var seriesItem in dataList.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesItem.CategoryIndex, GetColorName(seriesItem.Color), seriesItem.Value);
                            }
                        }
                        else
                        {
                            int c = 0;
                            foreach (var obj in columnSeries.ItemsSource.Cast<object>())
                            {
                                string colorVal = "";
                                if (columnSeries.ColorField != null)
                                {
                                    var propColor = obj.GetType().GetProperty(columnSeries.ColorField);
                                    colorVal = propColor?.GetValue(obj, null)?.ToString() ?? "";
                                }

                                string valueVal = "";
                                if (columnSeries.ValueField != null)
                                {
                                    var propValue = obj.GetType().GetProperty(columnSeries.ValueField);
                                    valueVal = propValue?.GetValue(obj, null)?.ToString() ?? "";
                                }

                                dataTable.Rows.Add(dataTable.Rows.Count + 1, c, colorVal, valueVal);
                                c++;
                            }
                        }
                    }
                    else
                    {
                        foreach (var seriesItem in columnSeries.Items)
                        {
                            dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesItem.CategoryIndex, GetColorName(seriesItem.Color), seriesItem.Value);
                        }
                    }
                    break;

                case OxyPlot.Series.BarSeries barSeries:
                    seriesName = GetSeriesName(barSeries.Title, "BarSeries", tableCount);

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_categoryIndex", typeof(string));
                    dataTable.Columns.Add(seriesName + "_color", typeof(string));
                    dataTable.Columns.Add(seriesName + "_value", typeof(string));

                    if (barSeries.ItemsSource != null)
                    {
                        var dataList = barSeries.ItemsSource as IEnumerable<BarItem>;
                        if (dataList != null)
                        {
                            foreach (var seriesItem in dataList.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesItem.CategoryIndex, GetColorName(seriesItem.Color), seriesItem.Value);
                            }
                        }
                        else
                        {
                            int c = 0;
                            foreach (var obj in barSeries.ItemsSource.Cast<object>())
                            {
                                string colorVal = "";
                                if (barSeries.ColorField != null)
                                {
                                    var propColor = obj.GetType().GetProperty(barSeries.ColorField);
                                    colorVal = propColor?.GetValue(obj, null)?.ToString() ?? "";
                                }

                                string valueVal = "";
                                if (barSeries.ValueField != null)
                                {
                                    var propValue = obj.GetType().GetProperty(barSeries.ValueField);
                                    valueVal = propValue?.GetValue(obj, null)?.ToString() ?? "";
                                }

                                dataTable.Rows.Add(dataTable.Rows.Count + 1, c, colorVal, valueVal);
                                c++;
                            }
                        }
                    }
                    else
                    {
                        foreach (var seriesItem in barSeries.Items)
                        {
                            dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesItem.CategoryIndex, GetColorName(seriesItem.Color), seriesItem.Value);
                        }
                    }
                    break;

                case OxyPlot.Series.AreaSeries areaSeries:
                    seriesName = GetSeriesName(areaSeries.Title, "AreaSeries", tableCount);

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_x", typeof(string));
                    dataTable.Columns.Add(seriesName + "_y", typeof(string));
                    dataTable.Columns.Add(seriesName + "_x2", typeof(string));
                    dataTable.Columns.Add(seriesName + "_y2", typeof(string));

                    if (areaSeries.ItemsSource != null)
                    {
                        var dataList = areaSeries.ItemsSource as IEnumerable<DataPoint>;
                        if (dataList != null)
                        {
                            foreach (var seriesValue in dataList.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y, "", "");
                            }
                        }
                        else
                        {
                            foreach (var obj in areaSeries.ItemsSource.Cast<object>())
                            {
                                var propX = obj.GetType().GetProperty(areaSeries.DataFieldX);
                                var xVal = propX?.GetValue(obj, null)?.ToString() ?? "";

                                var propY = obj.GetType().GetProperty(areaSeries.DataFieldY);
                                var yVal = propY?.GetValue(obj, null)?.ToString() ?? "";

                                string xVal2 = "";
                                if (areaSeries.DataFieldX2 != null)
                                {
                                    var propX2 = obj.GetType().GetProperty(areaSeries.DataFieldX2);
                                    xVal2 = propX2?.GetValue(obj, null)?.ToString() ?? "";
                                }

                                string yVal2 = "";
                                if (areaSeries.DataFieldY2 != null)
                                {
                                    var propY2 = obj.GetType().GetProperty(areaSeries.DataFieldY2);
                                    yVal2 = propY2?.GetValue(obj, null)?.ToString() ?? "";
                                }

                                dataTable.Rows.Add(dataTable.Rows.Count + 1, xVal, yVal, xVal2, yVal2);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < areaSeries.Points.Count; i++)
                        {
                            if (areaSeries.Points2.Count > 0)
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, areaSeries.Points[i].X, areaSeries.Points[i].Y, areaSeries.Points2[i].X, areaSeries.Points2[i].Y);
                            }
                            else
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, areaSeries.Points[i].X, areaSeries.Points[i].Y, "", "");
                            }
                        }
                    }
                    break;

                case OxyPlot.Series.BoxPlotSeries boxPlotSeries:
                    seriesName = GetSeriesName(boxPlotSeries.Title, "BoxPlotSeries", tableCount);

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_position", typeof(string));
                    dataTable.Columns.Add(seriesName + "_lowerWhisker", typeof(string));
                    dataTable.Columns.Add(seriesName + "_boxMinimum", typeof(string));
                    dataTable.Columns.Add(seriesName + "_median", typeof(string));
                    dataTable.Columns.Add(seriesName + "_boxMaximum", typeof(string));
                    dataTable.Columns.Add(seriesName + "_upperWhisker", typeof(string));
                    dataTable.Columns.Add(seriesName + "_label", typeof(string));

                    if (boxPlotSeries.ItemsSource != null)
                    {
                        var dataList = boxPlotSeries.ItemsSource as IEnumerable<BoxPlotItem>;
                        if (dataList != null)
                        {
                            foreach (var bpi in dataList)
                            {
                                var r = dataTable.Rows.Add(dataTable.Rows.Count + 1, bpi.X, bpi.LowerWhisker, bpi.BoxBottom, bpi.Median, bpi.BoxTop, bpi.UpperWhisker, "");

                                // Check if an X Axis Label is specified
                                if (boxPlotSeries.XAxis != null)
                                {
                                    if (boxPlotSeries.XAxis is OxyPlot.Axes.CategoryAxis catAxis)
                                    {
                                        if (catAxis.LabelField != null)
                                        {
                                            r["label"] = catAxis.LabelField;
                                        }
                                    }
                                }

                                // Add any outliers as additional columns
                                int outlierIdx = 1;
                                foreach (var outlier in bpi.Outliers)
                                {
                                    if (!dataTable.Columns.Contains("outlier" + outlierIdx))
                                    {
                                        dataTable.Columns.Add("outlier" + outlierIdx, typeof(string));
                                    }
                                    r[dataTable.Columns.IndexOf("outlier" + outlierIdx)] = outlier;
                                    outlierIdx++;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < boxPlotSeries.Items.Count; i++)
                        {
                            var item = boxPlotSeries.Items[i];
                            var r = dataTable.Rows.Add(dataTable.Rows.Count + 1, item.X, item.LowerWhisker, item.BoxBottom, item.Median, item.BoxTop, item.UpperWhisker, "");

                            if (boxPlotSeries.XAxis != null)
                            {
                                if (boxPlotSeries.XAxis is OxyPlot.Axes.CategoryAxis catAxis)
                                {
                                    if (catAxis.LabelField != null)
                                    {
                                        r["label"] = catAxis.LabelField;
                                    }
                                }
                            }

                            int j = 1;
                            foreach (var outlier in item.Outliers)
                            {
                                if (!dataTable.Columns.Contains("outlier" + j))
                                {
                                    dataTable.Columns.Add("outlier" + j, typeof(string));
                                }
                                r[dataTable.Columns.IndexOf("outlier" + j)] = outlier;
                                j++;
                            }
                        }
                    }
                    break;

                case OxyPlot.Series.HeatMapSeries heatMapSeries:
                    seriesName = GetSeriesName(heatMapSeries.Title, "HeatMapSeries", tableCount);

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add("xy", typeof(string));

                    if (heatMapSeries.Data != null)
                    {
                        // Add columns
                        double x0 = heatMapSeries.X0;
                        double x1 = heatMapSeries.X1;
                        int xN = heatMapSeries.Data.GetLength(0) - 1;
                        double xDelta = xN > 0 ? (x1 - x0) / xN : 0;
                        dataTable.Columns.Add(x0.ToString(), typeof(string));
                        for (int i = 1; i < heatMapSeries.Data.GetLength(0); i++)
                        {
                            x0 += xDelta;
                            dataTable.Columns.Add(x0.ToString(), typeof(string));
                        }

                        // Add rows
                        double y0 = heatMapSeries.Y0;
                        double y1 = heatMapSeries.Y1;
                        int yN = heatMapSeries.Data.GetLength(1) - 1;
                        double yDelta = yN > 0 ? (y1 - y0) / yN : 0;
                        dataTable.Rows.Add();
                        dataTable.Rows[0][0] = 1;
                        dataTable.Rows[0][1] = heatMapSeries.Y0;

                        for (int j = 1; j < heatMapSeries.Data.GetLength(1); j++)
                        {
                            dataTable.Rows.Add();
                            y0 += yDelta;
                            dataTable.Rows[j][0] = j + 1;
                            dataTable.Rows[j][1] = y0;
                        }

                        // Fill in matrix
                        for (int x = 0; x < heatMapSeries.Data.GetLength(0); x++)
                        {
                            for (int y = 0; y < heatMapSeries.Data.GetLength(1); y++)
                            {
                                dataTable.Rows[y][x + 2] = heatMapSeries.Data[x, y];
                            }
                        }
                    }
                    break;

                case OxyPlot.Series.ScatterErrorSeries scatterErrorSeries:
                    seriesName = GetSeriesName(scatterErrorSeries.Title, "ScatterErrorSeries", tableCount);

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_xLower", typeof(string));
                    dataTable.Columns.Add(seriesName + "_x", typeof(string));
                    dataTable.Columns.Add(seriesName + "_xUpper", typeof(string));
                    dataTable.Columns.Add(seriesName + "_yLower", typeof(string));
                    dataTable.Columns.Add(seriesName + "_y", typeof(string));
                    dataTable.Columns.Add(seriesName + "_yUpper", typeof(string));

                    if (scatterErrorSeries.ItemsSource != null)
                    {
                        var dataList = scatterErrorSeries.ItemsSource as IEnumerable<ScatterErrorPoint>;
                        if (dataList != null)
                        {
                            foreach (var seriesValue in dataList.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.X - seriesValue.ErrorX, seriesValue.X, seriesValue.X + seriesValue.ErrorX, seriesValue.Y - seriesValue.ErrorY, seriesValue.Y, seriesValue.Y + seriesValue.ErrorY);
                            }
                        }
                        else
                        {
                            foreach (var obj in scatterErrorSeries.ItemsSource.Cast<object>())
                            {
                                var propX = obj.GetType().GetProperty(scatterErrorSeries.DataFieldX);
                                var xVal = propX?.GetValue(obj, null)?.ToString() ?? "";

                                var propY = obj.GetType().GetProperty(scatterErrorSeries.DataFieldY);
                                var yVal = propY?.GetValue(obj, null)?.ToString() ?? "";

                                string xLower = "";
                                if (scatterErrorSeries.DataFieldErrorX != null)
                                {
                                    var propXLower = obj.GetType().GetProperty(scatterErrorSeries.DataFieldErrorX);
                                    xLower = propXLower?.GetValue(obj, null)?.ToString() ?? "";
                                }

                                string xUpper = xLower; // Symmetric error

                                string yLower = "";
                                if (scatterErrorSeries.DataFieldErrorY != null)
                                {
                                    var propYLower = obj.GetType().GetProperty(scatterErrorSeries.DataFieldErrorY);
                                    yLower = propYLower?.GetValue(obj, null)?.ToString() ?? "";
                                }

                                string yUpper = yLower; // Symmetric error

                                dataTable.Rows.Add(dataTable.Rows.Count + 1, xLower, xVal, xUpper, yLower, yVal, yUpper);
                            }
                        }
                    }
                    else
                    {
                        foreach (var seriesValue in scatterErrorSeries.Points)
                        {
                            if (seriesValue is ScatterErrorPoint errorPoint)
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, errorPoint.X - errorPoint.ErrorX, errorPoint.X, errorPoint.X + errorPoint.ErrorX, errorPoint.Y - errorPoint.ErrorY, errorPoint.Y, errorPoint.Y + errorPoint.ErrorY);
                            }
                        }
                    }
                    break;

                case OxyPlot.Series.ContourSeries:
                    // Skip over contour series for now. It doesn't add any value at this time.
                    continue;

                default:
                    // Can't export this type
                    continue;
            }

            // Check for item source on an X category axis
            if (series is XYAxisSeries xyAxisSeries)
            {
                var xCat = xyAxisSeries.XAxis as OxyPlot.Axes.CategoryAxis;
                if (xCat != null)
                {
                    dataTable.Columns.Add("Xcategory", typeof(string));

                    if (xCat.ItemsSource != null)
                    {
                        var xCatList = xCat.ItemsSource as IEnumerable<string>;
                        if (xCatList != null)
                        {
                            int idx = 0;
                            foreach (var label in xCatList)
                            {
                                if (idx < dataTable.Rows.Count)
                                {
                                    dataTable.Rows[idx]["Xcategory"] = label;
                                }
                                idx++;
                            }
                        }
                        else
                        {
                            int idx = 0;
                            foreach (var obj in xCat.ItemsSource.Cast<object>())
                            {
                                var propLabel = obj.GetType().GetProperty(xCat.LabelField);
                                var xVal = propLabel?.GetValue(obj, null)?.ToString() ?? "";
                                if (idx < dataTable.Rows.Count)
                                {
                                    dataTable.Rows[idx]["Xcategory"] = xVal;
                                }
                                idx++;
                            }
                        }
                    }
                }
                else
                {
                    // Check for item source on a Y category axis
                    var yCat = xyAxisSeries.YAxis as OxyPlot.Axes.CategoryAxis;
                    if (yCat != null)
                    {
                        dataTable.Columns.Add("Ycategory", typeof(string));

                        if (yCat.ItemsSource != null)
                        {
                            var yCatList = yCat.ItemsSource as IEnumerable<string>;
                            if (yCatList != null)
                            {
                                int idx = 0;
                                foreach (var label in yCatList)
                                {
                                    if (idx < dataTable.Rows.Count)
                                    {
                                        dataTable.Rows[idx]["Ycategory"] = label;
                                    }
                                    idx++;
                                }
                            }
                            else
                            {
                                int idx = 0;
                                foreach (var obj in yCat.ItemsSource.Cast<object>())
                                {
                                    var propLabel = obj.GetType().GetProperty(yCat.LabelField);
                                    var yVal = propLabel?.GetValue(obj, null)?.ToString() ?? "";
                                    if (idx < dataTable.Rows.Count)
                                    {
                                        dataTable.Rows[idx]["Ycategory"] = yVal;
                                    }
                                    idx++;
                                }
                            }
                        }
                    }
                }
            }

            tableList.Add(dataTable);
        }

        if (tableList.Count == 0)
        {
            MessageBox.Show("No exportable series data found in the plot.", "Export Data", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        string filters = "comma delimited(*.csv) |*.csv|Excel(*.xlsx) |*.xlsx|Sqlite(*.sqlite) |*.sqlite";
        try
        {
            var saveFileBrowser = new SaveFileDialog { Filter = filters, FilterIndex = 1 };
            if (saveFileBrowser.ShowDialog() == true)
            {
                switch (System.IO.Path.GetExtension(saveFileBrowser.FileName.ToString()).ToLower())
                {
                    case ".csv":
                        // Combine all the tables b/c CSVs only have one sheet/table
                        int uniqueCount = 1;
                        var colNames = new List<string>();
                        foreach (var theDT in tableList)
                        {
                            for (int i = 0; i < theDT.Columns.Count; i++)
                            {
                                if (theDT.Columns[i].ColumnName == "id")
                                {
                                    continue;
                                }

                                if (!colNames.Contains(theDT.Columns[i].ColumnName))
                                {
                                    colNames.Add(theDT.Columns[i].ColumnName);
                                }
                                else
                                {
                                    while (colNames.Contains(theDT.Columns[i].ColumnName))
                                    {
                                        theDT.Columns[i].ColumnName = theDT.Columns[i].ColumnName + "_" + uniqueCount;
                                        uniqueCount++;
                                    }
                                    colNames.Add(theDT.Columns[i].ColumnName);
                                }
                            }
                        }

                        // Get the series data merged
                        var totalDT = MergeAll(tableList, "id");

                        // Export to CSV directly
                        ExportDataTableToCsv(totalDT, saveFileBrowser.FileName);
                        break;

                    case ".xlsx":
                        // XLSX export requires DatabaseManager library
                        // Save each DT to the file (as new sheet)
                        MessageBox.Show("Excel export requires the DatabaseManager library.\n\nPlease use CSV export, or ensure DatabaseManager is available and implement IXlsxExporter.",
                            "XLSX Export", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;

                    case ".sqlite":
                        // SQLite export requires DatabaseManager library
                        // Save each DT to the file (as new table)
                        MessageBox.Show("SQLite export requires the DatabaseManager library.\n\nPlease use CSV export, or ensure DatabaseManager is available and implement ISqliteExporter.",
                            "SQLite Export", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;

                    default:
                        throw new Exception("Selected file format extension '" + System.IO.Path.GetExtension(saveFileBrowser.FileName) + "' is not supported for export.");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private string GetSeriesName(string? title, string defaultPrefix, int tableCount)
    {
        string name = !string.IsNullOrEmpty(title) ? title : defaultPrefix + "_" + tableCount;
        foreach (var badChar in BadCharacters)
        {
            name = name.Replace(badChar, "_");
        }
        return name;
    }

    private string GetColorName(OxyColor color)
    {
        if (color.IsUndefined())
            return "";
        return color.ToString();
    }

    private DataTable MergeAll(IList<DataTable> tables, string primaryKeyColumn)
    {
        if (!tables.Any())
            throw new ArgumentException("Tables must not be empty", nameof(tables));

        if (primaryKeyColumn != null)
        {
            foreach (var t in tables)
            {
                if (!t.Columns.Contains(primaryKeyColumn))
                    throw new ArgumentException($"All tables must have the specified primary key column {primaryKeyColumn}", nameof(primaryKeyColumn));
            }
        }

        if (tables.Count == 1)
            return tables[0];

        var table = new DataTable("TblUnion");
        table.BeginLoadData();

        foreach (var t in tables)
        {
            table.Merge(t);
        }

        table.EndLoadData();

        if (primaryKeyColumn != null)
        {
            var pkGroups = table.AsEnumerable().GroupBy(r => r[primaryKeyColumn]);
            var dupGroups = pkGroups.Where(g => g.Count() > 1);

            foreach (var grpDup in dupGroups)
            {
                var firstRow = grpDup.First();

                foreach (DataColumn c in table.Columns)
                {
                    if (firstRow.IsNull(c))
                    {
                        var firstNotNullRow = grpDup.Skip(1).FirstOrDefault(r => !r.IsNull(c));
                        if (firstNotNullRow != null)
                            firstRow[c] = firstNotNullRow[c];
                    }
                }

                var rowsToRemove = grpDup.Skip(1).ToList();
                foreach (var rowToRemove in rowsToRemove)
                {
                    table.Rows.Remove(rowToRemove);
                }
            }
        }

        return table;
    }

    private void ExportDataTableToCsv(DataTable dataTable, string fileName)
    {
        var sb = new StringBuilder();

        // Write header
        var columnNames = dataTable.Columns.Cast<DataColumn>().Select(c => EscapeCsvField(c.ColumnName));
        sb.AppendLine(string.Join(",", columnNames));

        // Write data rows
        foreach (DataRow row in dataTable.Rows)
        {
            var fields = row.ItemArray.Select(f => EscapeCsvField(f?.ToString() ?? ""));
            sb.AppendLine(string.Join(",", fields));
        }

        File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);
    }

    private string EscapeCsvField(string field)
    {
        if (field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }

    private void SaveImageButton_Click(object sender, RoutedEventArgs e)
    {
        if (_plotView == null) return;

        try
        {
            var dialog = new SavePlotImageDialog(_plotView)
            {
                Owner = Window.GetWindow(this)
            };

            dialog.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error opening save dialog: {ex.Message}",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region Annotation Management

    private void StartAddAnnotation(AddToolMode mode)
    {
        if (_plotView?.ActualModel == null) return;

        _addAnnotationToolMode = mode;
        _targetAddAnnotation = null;
        LeaderLine.Visibility = Visibility.Visible;
        LeaderLine.Points.Clear();
        SetCursor();
    }

    private void StopAddAnnotation()
    {
        _addAnnotationToolMode = AddToolMode.None;
        _targetAddAnnotation = null;
        _doubleClicked = false;
        LeaderLine.Visibility = Visibility.Collapsed;
        LeaderLine.Points.Clear();
        SetCursor();
    }

    #endregion

    #region Mouse Event Handlers

    private void PlotModel_MouseDown(object? sender, OxyMouseDownEventArgs e)
    {
        if (_plotView?.ActualModel == null) return;

        // Handle annotation drawing mode
        if (_addAnnotationToolMode != AddToolMode.None)
        {
            HandleAnnotationDrawing(e);
            return;
        }

        // Handle selection and context menu
        // TODO: Implement hit testing and context menus in next phase
    }

    private void PlotModel_MouseMove(object? sender, OxyMouseEventArgs e)
    {
        if (_plotView?.ActualModel == null) return;

        // Handle annotation drawing
        if (_addAnnotationToolMode != AddToolMode.None && _targetAddAnnotation != null)
        {
            UpdateAnnotationDuringDrawing(e);
            return;
        }

        // Handle hover feedback
        // TODO: Implement annotation hover feedback in next phase
    }

    private void PlotModel_MouseUp(object? sender, OxyMouseEventArgs e)
    {
        if (_plotView?.ActualModel == null) return;

        // Finalize annotation drawing
        if (_addAnnotationToolMode != AddToolMode.None)
        {
            FinalizeAnnotationDrawing(e);
        }

        SetCursor();
    }

    #endregion

    #region Annotation Drawing Logic

    private void HandleAnnotationDrawing(OxyMouseDownEventArgs e)
    {
        if (_plotView?.ActualModel == null) return;

        var position = e.Position;
        var dataPoint = ConvertScreenToDataPoint(position);

        switch (_addAnnotationToolMode)
        {
            case AddToolMode.AddArrowAnnotation:
                CreateArrowAnnotation(dataPoint);
                break;

            case AddToolMode.AddTextAnnotation:
                CreateTextAnnotation(dataPoint);
                break;

            case AddToolMode.AddVerticalLineAnnotation:
                CreateVerticalLineAnnotation(dataPoint);
                break;

            case AddToolMode.AddHorizontalLineAnnotation:
                CreateHorizontalLineAnnotation(dataPoint);
                break;

            case AddToolMode.AddRectangleAnnotation:
                CreateRectangleAnnotation(dataPoint);
                break;

            case AddToolMode.AddEllipseAnnotation:
                CreateEllipseAnnotation(dataPoint);
                break;

            case AddToolMode.AddPointAnnotation:
                CreatePointAnnotation(dataPoint);
                break;

            case AddToolMode.AddPolygonAnnotation:
                HandlePolygonDrawing(e, dataPoint);
                break;

            case AddToolMode.AddPolylineAnnotation:
                HandlePolylineDrawing(e, dataPoint);
                break;
        }
    }

    private void CreateArrowAnnotation(DataPoint startPoint)
    {
        if (_plotView?.ActualModel == null) return;

        var arrow = new ArrowAnnotation
        {
            StartPoint = startPoint,
            EndPoint = startPoint,
            Text = "Arrow",
            Color = OxyColors.Blue,
            StrokeThickness = 2
        };

        _plotView.ActualModel.Annotations.Add(arrow);
        _targetAddAnnotation = arrow;
        _plotView.InvalidatePlot(false);
    }

    private void CreateTextAnnotation(DataPoint position)
    {
        if (_plotView?.ActualModel == null) return;

        var text = new TextAnnotation
        {
            TextPosition = position,
            Text = "Text Annotation",
            Stroke = OxyColors.Black,
            StrokeThickness = 1
        };

        _plotView.ActualModel.Annotations.Add(text);
        _targetAddAnnotation = text;
        _plotView.InvalidatePlot(false);
    }

    private void CreateVerticalLineAnnotation(DataPoint position)
    {
        if (_plotView?.ActualModel == null) return;

        var line = new LineAnnotation
        {
            Type = LineAnnotationType.Vertical,
            X = position.X,
            Text = "V-Line",
            Color = OxyColors.Red,
            StrokeThickness = 1
        };

        _plotView.ActualModel.Annotations.Add(line);
        _targetAddAnnotation = line;
        _plotView.InvalidatePlot(false);
    }

    private void CreateHorizontalLineAnnotation(DataPoint position)
    {
        if (_plotView?.ActualModel == null) return;

        var line = new LineAnnotation
        {
            Type = LineAnnotationType.Horizontal,
            Y = position.Y,
            Text = "H-Line",
            Color = OxyColors.Red,
            StrokeThickness = 1
        };

        _plotView.ActualModel.Annotations.Add(line);
        _targetAddAnnotation = line;
        _plotView.InvalidatePlot(false);
    }

    private void CreateRectangleAnnotation(DataPoint position)
    {
        if (_plotView?.ActualModel == null) return;

        var rect = new RectangleAnnotation
        {
            MinimumX = position.X,
            MaximumX = position.X,
            MinimumY = position.Y,
            MaximumY = position.Y,
            Text = "Rectangle",
            Fill = OxyColor.FromAColor(80, OxyColors.LightBlue),
            Stroke = OxyColors.Blue,
            StrokeThickness = 1
        };

        _plotView.ActualModel.Annotations.Add(rect);
        _targetAddAnnotation = rect;
        _plotView.InvalidatePlot(false);
    }

    private void CreateEllipseAnnotation(DataPoint position)
    {
        if (_plotView?.ActualModel == null) return;

        var ellipse = new EllipseAnnotation
        {
            X = position.X,
            Y = position.Y,
            Width = 0.1,
            Height = 0.1,
            Text = "Ellipse",
            Fill = OxyColor.FromAColor(80, OxyColors.LightGreen),
            Stroke = OxyColors.Green,
            StrokeThickness = 1
        };

        _plotView.ActualModel.Annotations.Add(ellipse);
        _targetAddAnnotation = ellipse;
        _plotView.InvalidatePlot(false);
    }

    private void CreatePointAnnotation(DataPoint position)
    {
        if (_plotView?.ActualModel == null) return;

        var point = new PointAnnotation
        {
            X = position.X,
            Y = position.Y,
            Text = "Point",
            Size = 8,
            Fill = OxyColors.Red,
            Stroke = OxyColors.Black,
            StrokeThickness = 1
        };

        _plotView.ActualModel.Annotations.Add(point);
        _targetAddAnnotation = point;
        _plotView.InvalidatePlot(false);
        StopAddAnnotation(); // Point annotations are immediate
    }

    private void HandlePolygonDrawing(OxyMouseDownEventArgs e, DataPoint dataPoint)
    {
        if (_targetAddAnnotation == null)
        {
            // Start new polygon
            var polygon = new PolygonAnnotation
            {
                Text = "Polygon",
                Fill = OxyColor.FromAColor(80, OxyColors.Yellow),
                Stroke = OxyColors.Orange,
                StrokeThickness = 1
            };

            polygon.Points.Add(dataPoint);
            LeaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
            LeaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));

            _targetAddAnnotation = polygon;
        }
        else
        {
            // Add point to existing polygon
            _doubleClicked = e.ClickCount > 1;
            var polygon = (PolygonAnnotation)_targetAddAnnotation;

            if (polygon.Points.Count == 3 && _plotView?.ActualModel != null)
            {
                _plotView.ActualModel.Annotations.Add(polygon);
            }

            if (!_doubleClicked)
            {
                polygon.Points.Add(dataPoint);
                LeaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
            }
        }
    }

    private void HandlePolylineDrawing(OxyMouseDownEventArgs e, DataPoint dataPoint)
    {
        if (_targetAddAnnotation == null)
        {
            // Start new polyline
            var polyline = new PolylineAnnotation
            {
                Text = "Polyline",
                Color = OxyColors.Purple,
                StrokeThickness = 2
            };

            polyline.Points.Add(dataPoint);
            LeaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
            LeaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));

            _plotView?.ActualModel?.Annotations.Add(polyline);
            _targetAddAnnotation = polyline;
        }
        else
        {
            // Add point to existing polyline
            _doubleClicked = e.ClickCount > 1;
            var polyline = (PolylineAnnotation)_targetAddAnnotation;

            if (!_doubleClicked)
            {
                polyline.Points.Add(dataPoint);
                LeaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
            }
        }
    }

    private void UpdateAnnotationDuringDrawing(OxyMouseEventArgs e)
    {
        if (_plotView?.ActualModel == null || _targetAddAnnotation == null) return;

        var dataPoint = ConvertScreenToDataPoint(e.Position);

        switch (_targetAddAnnotation)
        {
            case ArrowAnnotation arrow:
                arrow.EndPoint = dataPoint;
                break;

            case TextAnnotation text:
                text.TextPosition = dataPoint;
                break;

            case LineAnnotation line when line.Type == LineAnnotationType.Vertical:
                line.X = dataPoint.X;
                break;

            case LineAnnotation line when line.Type == LineAnnotationType.Horizontal:
                line.Y = dataPoint.Y;
                break;

            case RectangleAnnotation rect:
                rect.MaximumX = dataPoint.X;
                rect.MaximumY = dataPoint.Y;
                break;

            case EllipseAnnotation ellipse:
                var width = Math.Abs(dataPoint.X - ellipse.X);
                var height = Math.Abs(dataPoint.Y - ellipse.Y);
                ellipse.Width = width * 2;
                ellipse.Height = height * 2;
                break;

            case PolygonAnnotation:
            case PolylineAnnotation:
                if (LeaderLine.Points.Count > 0)
                {
                    LeaderLine.Points[^1] = new Point(e.Position.X, e.Position.Y);
                }
                break;
        }

        _plotView.InvalidatePlot(false);
    }

    private void FinalizeAnnotationDrawing(OxyMouseEventArgs e)
    {
        // Handle polygon/polyline completion
        if (_addAnnotationToolMode == AddToolMode.AddPolygonAnnotation ||
            _addAnnotationToolMode == AddToolMode.AddPolylineAnnotation)
        {
            if (_doubleClicked)
            {
                StopAddAnnotation();
            }
            return;
        }

        // Finalize other annotations
        StopAddAnnotation();
    }

    #endregion

    #region Helper Methods

    private void SetCursor()
    {
        if (_plotView == null) return;

        if (_addAnnotationToolMode != AddToolMode.None)
        {
            _plotView.Cursor = Cursors.Cross;
        }
        else if (PanButton.IsChecked == true)
        {
            _plotView.Cursor = _panHandCursor;
        }
        else if (ZoomButton.IsChecked == true)
        {
            _plotView.Cursor = _zoomCursor;
        }
        else
        {
            _plotView.Cursor = Cursors.Arrow;
        }
    }

    private DataPoint ConvertScreenToDataPoint(ScreenPoint screenPoint)
    {
        if (_plotView?.ActualModel == null)
            return new DataPoint(0, 0);

        var xAxis = _plotView.ActualModel.DefaultXAxis;
        var yAxis = _plotView.ActualModel.DefaultYAxis;

        return xAxis.InverseTransform(screenPoint.X, screenPoint.Y, yAxis);
    }

    #endregion
}
