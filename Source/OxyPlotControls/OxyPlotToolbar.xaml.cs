using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Wpf;
using OxyPlot.Annotations;
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

    private void ExportDataButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Implement data export functionality
        MessageBox.Show("Export Data functionality will be implemented in the next phase.",
            "Export Data", MessageBoxButton.OK, MessageBoxImage.Information);
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
