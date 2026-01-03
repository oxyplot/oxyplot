using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
using DatabaseManager;

namespace OxyPlotControls;

/// <summary>
/// Interactive toolbar for OxyPlot with Pan/Zoom and annotation drawing capabilities.
/// Complete C# port of the VB OxyplotToolbar with PlotView/PlotModel architecture.
/// </summary>
public partial class OxyPlotToolbar : UserControl
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="OxyPlotToolbar"/> class.
    /// </summary>
    public OxyPlotToolbar()
    {
        InitializeComponent();
        InitializeCursors();
        InitializeLeaderLine();
    }

    #endregion

    #region Members

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

    /// <summary>
    /// Dependency property for the icon size.
    /// </summary>
    public static readonly DependencyProperty IconSizeProperty =
        DependencyProperty.Register(
            nameof(IconSize),
            typeof(double),
            typeof(OxyPlotToolbar),
            new PropertyMetadata(20.0));

    /// <summary>
    /// Gets or sets the toolbar icon size.
    /// </summary>
    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    /// <summary>
    /// Dependency property for the toolbar orientation.
    /// </summary>
    public static readonly DependencyProperty ToolBarOrientationProperty =
        DependencyProperty.Register(
            nameof(ToolBarOrientation),
            typeof(Orientation),
            typeof(OxyPlotToolbar),
            new PropertyMetadata(Orientation.Vertical));

    /// <summary>
    /// Gets or sets the toolbar orientation.
    /// </summary>
    public Orientation ToolBarOrientation
    {
        get => (Orientation)GetValue(ToolBarOrientationProperty);
        set => SetValue(ToolBarOrientationProperty, value);
    }

    // Cursors
    private Cursor? _movePointsCursor;
    private Cursor? _addPointCursor;
    private Cursor? _panHandCursor;
    private Cursor? _panHandClosedCursor;
    private Cursor? _zoomCursor;

    // Edit annotation variables
    private bool _doubleClicked;
    private bool _showPoints;
    private ScreenPoint _lastScreenPoint = ScreenPoint.Undefined;
    private bool _moveStartPoint;
    private bool _moveEndPoint;
    private int _movePointIndex = -1;
    private bool _scaleMaxX;
    private bool _scaleMaxY;
    private bool _scaleMinX;
    private bool _scaleMinY;
    private OxyColor _originalColor = OxyColors.White;

    // Adding annotations
    private AddToolMode _addAnnotationToolMode = AddToolMode.None;
    private Annotation? _targetAddAnnotation;

    // Context menu and text editing
    private TextBox? _textBox;
    private ContextMenu? _contextMenu;

    // Line annotation tooltip
    private ToolTip? _lineAnnotationTooltip;

    /// <summary>
    /// Enumeration for adding annotation tool mode.
    /// </summary>
    public enum AddToolMode
    {
        None,
        AddArrowAnnotation,
        AddTextAnnotation,
        AddRectangleAnnotation,
        AddEllipseAnnotation,
        AddPointAnnotation,
        AddPolygonAnnotation,
        AddPolylineAnnotation,
        AddVerticalLineAnnotation,
        AddHorizontalLineAnnotation
    }

    /// <summary>
    /// Enumeration for property expander types.
    /// </summary>
    public enum PropertyExpander
    {
        General_PlotTitle,
        General_PlotSubtitle,
        Axes_Title,
        Axes_Options,
        Axes_Display,
        Annotations_Text,
        Series_Options
    }

    /// <summary>
    /// Event arguments for PropertiesCalled event.
    /// </summary>
    public class PropertiesCalledEventArgs : EventArgs
    {
        public PlotView? TargetPlot { get; set; }
        public bool OpenProperties { get; set; }
        public PropertyExpander? PropertyExpander { get; set; }
        public object? SelectedObject { get; set; }
    }

    /// <summary>
    /// Event indicating the plot properties need to be opened.
    /// </summary>
    public event EventHandler<PropertiesCalledEventArgs>? PropertiesCalled;

    // Series types that cannot be swapped
    private static readonly HashSet<Type> NonSwapSeriesTypes = new()
    {
        typeof(HistogramSeries),
        typeof(OxyPlot.Series.BarSeries),
        typeof(ColumnSeries),
        typeof(HeatMapSeries)
    };

    #endregion

    #region Initialization

    private static void OnPlotViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is OxyPlotToolbar toolbar)
        {
            toolbar.InitializePlot(e.OldValue as PlotView, e.NewValue as PlotView);
        }
    }

    /// <summary>
    /// Initializes the plot when PlotView changes.
    /// </summary>
    private void InitializePlot(PlotView? oldPlot, PlotView? newPlot)
    {
        // Unsubscribe from old plot
        if (oldPlot?.ActualModel != null)
        {
            oldPlot.ActualModel.MouseDown -= PlotModelMouseDown;
            oldPlot.ActualModel.MouseMove -= PlotModelMouseMove;
            oldPlot.ActualModel.MouseUp -= PlotModelMouseUp;

            if (oldPlot.ActualModel.Annotations is INotifyCollectionChanged oldAnnotations)
            {
                oldAnnotations.CollectionChanged -= PlotModelAnnotationCollectionChanged;
            }
        }

        // Subscribe to new plot
        if (newPlot?.ActualModel != null)
        {
            newPlot.ActualModel.MouseDown += PlotModelMouseDown;
            newPlot.ActualModel.MouseMove += PlotModelMouseMove;
            newPlot.ActualModel.MouseUp += PlotModelMouseUp;

            // Set up custom cursors
            newPlot.ZoomHorizontalCursor = _zoomCursor ?? Cursors.Cross;
            newPlot.ZoomRectangleCursor = _zoomCursor ?? Cursors.Cross;
            newPlot.ZoomVerticalCursor = _zoomCursor ?? Cursors.Cross;
            newPlot.PanCursor = _panHandCursor ?? Cursors.Hand;

            // Set up initial controller bindings
            if (PointerButton.IsChecked == true) PointerButton_Click(this, new RoutedEventArgs());
            else if (ZoomButton.IsChecked == true) ZoomButton_Click(this, new RoutedEventArgs());
            else if (PanButton.IsChecked == true) PanButton_Click(this, new RoutedEventArgs());

            // Subscribe to annotation collection changes
            if (newPlot.ActualModel.Annotations is INotifyCollectionChanged annotations)
            {
                annotations.CollectionChanged += PlotModelAnnotationCollectionChanged;
            }

            // Set up existing annotations
            foreach (var annotation in newPlot.ActualModel.Annotations)
            {
                SetupAnnotationHandlers(annotation);
            }
        }

        SetCursor();
    }

    /// <summary>
    /// Initializes custom cursors from embedded resources.
    /// Falls back to standard cursors if resources are not available.
    /// </summary>
    private void InitializeCursors()
    {
        // Try to load custom cursors from embedded resources
        _panHandCursor = LoadCursorFromResource("Pan_Hand.cur") ?? Cursors.Hand;
        _panHandClosedCursor = LoadCursorFromResource("Pan_Hand_Closed.cur") ?? Cursors.Hand;
        _zoomCursor = LoadCursorFromResource("ZoomIn.cur") ?? Cursors.Cross;
        _movePointsCursor = LoadCursorFromResource("SelectPointCursor.cur") ?? Cursors.SizeAll;
        _addPointCursor = LoadCursorFromResource("AddPointCursor.cur") ?? Cursors.Cross;
    }

    /// <summary>
    /// Loads a cursor from a WPF resource.
    /// </summary>
    /// <param name="resourceName">The resource file name.</param>
    /// <returns>The cursor, or null if not found.</returns>
    private static Cursor? LoadCursorFromResource(string resourceName)
    {
        try
        {
            // Use pack URI format for WPF resources
            var resourceUri = new Uri($"pack://application:,,,/OxyPlotControls;component/Resources/{resourceName}");
            var resourceInfo = System.Windows.Application.GetResourceStream(resourceUri);
            if (resourceInfo != null)
            {
                return new Cursor(resourceInfo.Stream);
            }
        }
        catch
        {
            // Fallback to null, caller will use default cursor
        }

        return null;
    }

    /// <summary>
    /// Initializes the leader line for polygon/polyline drawing.
    /// </summary>
    private void InitializeLeaderLine()
    {
        LeaderLine.StrokeThickness = 2;
        LeaderLine.Visibility = Visibility.Collapsed;
        LeaderLine.Stroke = new SolidColorBrush(Colors.SkyBlue);
        LeaderLine.StrokeDashArray = new DoubleCollection(new[] { 4.0, 2.0, 1.0, 2.0 });
    }

    #endregion

    #region Pan & Zoom

    /// <summary>
    /// User clicked the pointer button.
    /// </summary>
    private void PointerButton_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();

        if (PlotView?.ActualController == null) return;

        var controller = PlotView.ActualController;
        controller.UnbindAll();
        controller.BindMouseDown(OxyMouseButton.Middle, PlotCommands.PanAt);
        controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.SnapTrack);
        controller.BindMouseWheel(PlotCommands.ZoomWheel);
        controller.BindKeyDown(OxyKey.Escape, PlotCommands.Reset);

        SetCursor();
    }

    /// <summary>
    /// User clicked the pan button.
    /// </summary>
    private void PanButton_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();

        if (PlotView?.ActualController == null) return;

        var controller = PlotView.ActualController;
        controller.UnbindAll();
        controller.BindMouseDown(OxyMouseButton.Middle, PlotCommands.PanAt);
        controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
        controller.BindMouseDown(OxyMouseButton.Right, PlotCommands.SnapTrack);
        controller.BindMouseWheel(PlotCommands.ZoomWheel);
        controller.BindKeyDown(OxyKey.Escape, PlotCommands.Reset);

        SetCursor();
    }

    /// <summary>
    /// User clicked zoom button.
    /// </summary>
    private void ZoomButton_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();

        if (PlotView?.ActualController == null) return;

        var controller = PlotView.ActualController;
        controller.UnbindAll();
        controller.BindMouseDown(OxyMouseButton.Middle, PlotCommands.PanAt);
        controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.ZoomRectangle);
        controller.BindMouseDown(OxyMouseButton.Right, PlotCommands.SnapTrack);
        controller.BindMouseWheel(PlotCommands.ZoomWheel);
        controller.BindKeyDown(OxyKey.Escape, PlotCommands.Reset);

        SetCursor();
    }

    /// <summary>
    /// User clicked zoom to extents.
    /// </summary>
    private void ZoomAllButton_Click(object sender, RoutedEventArgs e)
    {
        if (PlotView?.ActualModel == null) return;

        PlotView.ActualModel.ResetAllAxes();
        PlotView.InvalidatePlot(false);
        PlotView.Focus();
    }

    /// <summary>
    /// Sets the mouse cursor based on the current tool mode.
    /// </summary>
    private void SetCursor()
    {
        if (PlotView == null) return;

        if (_addAnnotationToolMode != AddToolMode.None)
        {
            PlotView.Cursor = _addPointCursor;
        }
        else if (PanButton.IsChecked == true)
        {
            PlotView.Cursor = _panHandCursor;
        }
        else if (PointerButton.IsChecked == true)
        {
            PlotView.Cursor = Cursors.Arrow;
        }
        else if (ZoomButton.IsChecked == true)
        {
            PlotView.Cursor = _zoomCursor;
        }
        else
        {
            PlotView.Cursor = Cursors.Arrow;
        }
    }

    #endregion

    #region Annotations

    /// <summary>
    /// When Add button is clicked, show context menu.
    /// </summary>
    private void AddAnnotationToggleButton_Click(object sender, RoutedEventArgs e)
    {
        if (AddAnnotationToggleButton.ContextMenu != null)
        {
            AddAnnotationToggleButton.ContextMenu.IsOpen = true;
        }
    }

    private void AddArrowAnnotationItem_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();
        PlotView?.ActualController?.UnbindAll();
        _addAnnotationToolMode = AddToolMode.AddArrowAnnotation;
        SetCursor();
    }

    private void AddTextAnnotationItem_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();
        PlotView?.ActualController?.UnbindAll();
        _addAnnotationToolMode = AddToolMode.AddTextAnnotation;
        SetCursor();
    }

    private void AddVerticalLineAnnotationItem_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();
        PlotView?.ActualController?.UnbindAll();
        _addAnnotationToolMode = AddToolMode.AddVerticalLineAnnotation;
        SetCursor();
    }

    private void AddHorizontalLineAnnotationItem_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();
        PlotView?.ActualController?.UnbindAll();
        _addAnnotationToolMode = AddToolMode.AddHorizontalLineAnnotation;
        SetCursor();
    }

    private void AddRectangleAnnotationItem_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();
        PlotView?.ActualController?.UnbindAll();
        _addAnnotationToolMode = AddToolMode.AddRectangleAnnotation;
        SetCursor();
    }

    private void AddEllipseAnnotationItem_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();
        PlotView?.ActualController?.UnbindAll();
        _addAnnotationToolMode = AddToolMode.AddEllipseAnnotation;
        SetCursor();
    }

    private void AddPointAnnotationItem_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();
        PlotView?.ActualController?.UnbindAll();
        _addAnnotationToolMode = AddToolMode.AddPointAnnotation;
        SetCursor();
    }

    private void AddPolygonAnnotationItem_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();
        _addAnnotationToolMode = AddToolMode.AddPolygonAnnotation;
        LeaderLine.Visibility = Visibility.Visible;
        LeaderLine.Points.Clear();
        SetCursor();
    }

    private void AddPolylineAnnotationItem_Click(object sender, RoutedEventArgs e)
    {
        StopAddAnnotation();
        _addAnnotationToolMode = AddToolMode.AddPolylineAnnotation;
        LeaderLine.Visibility = Visibility.Visible;
        LeaderLine.Points.Clear();
        SetCursor();
    }

    // Direct button handlers (for Toolbar buttons in XAML)
    private void AddArrowAnnotation_Click(object sender, RoutedEventArgs e) => AddArrowAnnotationItem_Click(sender, e);
    private void AddTextAnnotation_Click(object sender, RoutedEventArgs e) => AddTextAnnotationItem_Click(sender, e);
    private void AddVerticalLineAnnotation_Click(object sender, RoutedEventArgs e) => AddVerticalLineAnnotationItem_Click(sender, e);
    private void AddHorizontalLineAnnotation_Click(object sender, RoutedEventArgs e) => AddHorizontalLineAnnotationItem_Click(sender, e);
    private void AddRectangleAnnotation_Click(object sender, RoutedEventArgs e) => AddRectangleAnnotationItem_Click(sender, e);
    private void AddEllipseAnnotation_Click(object sender, RoutedEventArgs e) => AddEllipseAnnotationItem_Click(sender, e);
    private void AddPointAnnotation_Click(object sender, RoutedEventArgs e) => AddPointAnnotationItem_Click(sender, e);
    private void AddPolygonAnnotation_Click(object sender, RoutedEventArgs e) => AddPolygonAnnotationItem_Click(sender, e);
    private void AddPolylineAnnotation_Click(object sender, RoutedEventArgs e) => AddPolylineAnnotationItem_Click(sender, e);

    /// <summary>
    /// Stop adding the annotation.
    /// </summary>
    private void StopAddAnnotation()
    {
        if (_addAnnotationToolMode != AddToolMode.None)
        {
            if (_addAnnotationToolMode == AddToolMode.AddPolygonAnnotation ||
                _addAnnotationToolMode == AddToolMode.AddPolylineAnnotation)
            {
                LeaderLine.Visibility = Visibility.Collapsed;
                LeaderLine.Points.Clear();
                PlotView?.InvalidatePlot(false);
            }

            _doubleClicked = false;
            _addAnnotationToolMode = AddToolMode.None;
            _targetAddAnnotation = null;

            // Restore controller bindings
            if (PanButton.IsChecked == true) PanButton_Click(this, new RoutedEventArgs());
            else if (PointerButton.IsChecked == true) PointerButton_Click(this, new RoutedEventArgs());
            else if (ZoomButton.IsChecked == true) ZoomButton_Click(this, new RoutedEventArgs());

            SetCursor();
        }
    }

    /// <summary>
    /// A new annotation has been added to the plot. Adds the appropriate handlers.
    /// </summary>
    private void PlotModelAnnotationCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (var item in e.NewItems)
            {
                if (item is Annotation annotation)
                {
                    SetupAnnotationHandlers(annotation);
                }
            }
        }
    }

    /// <summary>
    /// Sets up mouse handlers for an annotation.
    /// </summary>
    private void SetupAnnotationHandlers(Annotation annotation)
    {
        switch (annotation)
        {
            case ArrowAnnotation arrow:
                SetupArrowAnnotationHandlers(arrow);
                break;
            case TextAnnotation text:
                SetupTextAnnotationHandlers(text);
                break;
            case RectangleAnnotation rect:
                SetupRectangleAnnotationHandlers(rect);
                break;
            case EllipseAnnotation ellipse:
                SetupEllipseAnnotationHandlers(ellipse);
                break;
            case PointAnnotation point:
                SetupPointAnnotationHandlers(point);
                break;
            case PolygonAnnotation polygon:
                SetupPolygonAnnotationHandlers(polygon);
                break;
            case PolylineAnnotation polyline:
                SetupPolylineAnnotationHandlers(polyline);
                break;
            case LineAnnotation line:
                SetupLineAnnotationHandlers(line);
                break;
        }
    }

    private void SetupArrowAnnotationHandlers(ArrowAnnotation arrow)
    {
        arrow.MouseDown += (s, e) =>
        {
            if (_addAnnotationToolMode != AddToolMode.None) return;
            if (e.ChangedButton != OxyMouseButton.Left) return;

            _lastScreenPoint = e.Position;
            _moveStartPoint = e.HitTestResult.Index != 2;
            _moveEndPoint = e.HitTestResult.Index != 1;
            _originalColor = arrow.Color;
            arrow.Color = OxyColors.Red;

            RaisePropertiesCalled(false, PropertyExpander.Annotations_Text, arrow);

            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        arrow.MouseMove += (s, e) =>
        {
            var dx = e.Position.X - _lastScreenPoint.X;
            var dy = e.Position.Y - _lastScreenPoint.Y;
            var startScreen = arrow.Transform(arrow.StartPoint);
            var endScreen = arrow.Transform(arrow.EndPoint);

            var newStartScreen = new ScreenPoint(startScreen.X + dx, startScreen.Y + dy);
            var newEndScreen = new ScreenPoint(endScreen.X + dx, endScreen.Y + dy);

            if (_moveStartPoint) arrow.StartPoint = arrow.InverseTransform(newStartScreen);
            if (_moveEndPoint) arrow.EndPoint = arrow.InverseTransform(newEndScreen);

            _lastScreenPoint = e.Position;
            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        arrow.MouseUp += (s, e) =>
        {
            arrow.Color = _originalColor;
            PlotView?.InvalidatePlot(false);
        };
    }

    private void SetupTextAnnotationHandlers(TextAnnotation text)
    {
        text.MouseDown += (s, e) =>
        {
            if (_addAnnotationToolMode != AddToolMode.None) return;
            if (e.ChangedButton != OxyMouseButton.Left) return;

            _lastScreenPoint = e.Position;
            _moveStartPoint = e.HitTestResult.Index == 0;
            _originalColor = text.Background;
            text.Background = OxyColors.Red;

            RaisePropertiesCalled(false, PropertyExpander.Annotations_Text, text);

            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        text.MouseMove += (s, e) =>
        {
            var dx = e.Position.X - _lastScreenPoint.X;
            var dy = e.Position.Y - _lastScreenPoint.Y;
            var screen = text.Transform(text.TextPosition);
            var newScreen = new ScreenPoint(screen.X + dx, screen.Y + dy);

            if (_moveStartPoint) text.TextPosition = text.InverseTransform(newScreen);

            _lastScreenPoint = e.Position;
            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        text.MouseUp += (s, e) =>
        {
            text.Background = _originalColor;
            PlotView?.InvalidatePlot(false);
        };
    }

    private void SetupRectangleAnnotationHandlers(RectangleAnnotation rect)
    {
        rect.MouseDown += (s, e) =>
        {
            if (_addAnnotationToolMode != AddToolMode.None) return;
            if (e.ChangedButton != OxyMouseButton.Left) return;

            _lastScreenPoint = e.Position;

            var upperRight = rect.Transform(rect.MaximumX, rect.MaximumY);
            var lowerLeft = rect.Transform(rect.MinimumX, rect.MinimumY);
            var topRight = new ScreenPoint(Math.Abs(upperRight.X - e.Position.X), Math.Abs(upperRight.Y - e.Position.Y));
            var bottomLeft = new ScreenPoint(Math.Abs(lowerLeft.X - e.Position.X), Math.Abs(lowerLeft.Y - e.Position.Y));

            _scaleMaxX = topRight.X < 10;
            _scaleMaxY = topRight.Y < 10;
            _scaleMinX = bottomLeft.X < 10;
            _scaleMinY = bottomLeft.Y < 10;

            if (e.HitTestResult.Index == 0)
            {
                _moveStartPoint = !_scaleMaxX && !_scaleMaxY && !_scaleMinX && !_scaleMinY;
            }

            _originalColor = rect.Fill;
            rect.Fill = OxyColors.Red;

            RaisePropertiesCalled(false, PropertyExpander.Annotations_Text, rect);

            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        rect.MouseMove += (s, e) =>
        {
            var dx = e.Position.X - _lastScreenPoint.X;
            var dy = e.Position.Y - _lastScreenPoint.Y;

            var upperRightScreen = rect.Transform(rect.MaximumX, rect.MaximumY);
            var lowerLeftScreen = rect.Transform(rect.MinimumX, rect.MinimumY);

            var newUpperRight = rect.InverseTransform(new ScreenPoint(upperRightScreen.X + dx, upperRightScreen.Y + dy));
            var newLowerLeft = rect.InverseTransform(new ScreenPoint(lowerLeftScreen.X + dx, lowerLeftScreen.Y + dy));

            if (_scaleMaxX) rect.MaximumX = newUpperRight.X;
            if (_scaleMaxY) rect.MaximumY = newUpperRight.Y;
            if (_scaleMinX) rect.MinimumX = newLowerLeft.X;
            if (_scaleMinY) rect.MinimumY = newLowerLeft.Y;

            if (_moveStartPoint)
            {
                rect.MaximumX = newUpperRight.X;
                rect.MaximumY = newUpperRight.Y;
                rect.MinimumX = newLowerLeft.X;
                rect.MinimumY = newLowerLeft.Y;
            }

            _lastScreenPoint = e.Position;
            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        rect.MouseUp += (s, e) =>
        {
            rect.Fill = _originalColor;
            _moveStartPoint = false;
            _scaleMaxX = _scaleMaxY = _scaleMinX = _scaleMinY = false;
            PlotView?.InvalidatePlot(false);
        };
    }

    private void SetupEllipseAnnotationHandlers(EllipseAnnotation ellipse)
    {
        ellipse.MouseDown += (s, e) =>
        {
            if (_addAnnotationToolMode != AddToolMode.None) return;
            if (e.ChangedButton != OxyMouseButton.Left) return;

            _lastScreenPoint = e.Position;

            var upperRight = ellipse.Transform(ellipse.X + ellipse.Width / 2, ellipse.Y + ellipse.Height / 2);
            var lowerLeft = ellipse.Transform(ellipse.X - ellipse.Width / 2, ellipse.Y - ellipse.Height / 2);
            var topRight = new ScreenPoint(Math.Abs(upperRight.X - e.Position.X), Math.Abs(upperRight.Y - e.Position.Y));
            var bottomLeft = new ScreenPoint(Math.Abs(lowerLeft.X - e.Position.X), Math.Abs(lowerLeft.Y - e.Position.Y));

            _scaleMaxX = topRight.X < 10;
            _scaleMaxY = topRight.Y < 10;
            _scaleMinX = bottomLeft.X < 10;
            _scaleMinY = bottomLeft.Y < 10;

            if (e.HitTestResult.Index == 0)
            {
                _moveStartPoint = !_scaleMaxX && !_scaleMaxY && !_scaleMinX && !_scaleMinY;
            }

            _originalColor = ellipse.Fill;
            ellipse.Fill = OxyColors.Red;

            RaisePropertiesCalled(false, PropertyExpander.Annotations_Text, ellipse);

            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        ellipse.MouseMove += (s, e) =>
        {
            var dx = e.Position.X - _lastScreenPoint.X;
            var dy = e.Position.Y - _lastScreenPoint.Y;

            var centerScreen = ellipse.Transform(ellipse.X, ellipse.Y);

            if (_moveStartPoint)
            {
                var newCenter = ellipse.InverseTransform(new ScreenPoint(centerScreen.X + dx, centerScreen.Y + dy));
                ellipse.X = newCenter.X;
                ellipse.Y = newCenter.Y;
            }
            else
            {
                // Resize
                var mouseData = ellipse.InverseTransform(e.Position);
                if (_scaleMaxX || _scaleMinX)
                {
                    ellipse.Width = Math.Abs(mouseData.X - ellipse.X) * 2;
                }
                if (_scaleMaxY || _scaleMinY)
                {
                    ellipse.Height = Math.Abs(mouseData.Y - ellipse.Y) * 2;
                }
            }

            _lastScreenPoint = e.Position;
            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        ellipse.MouseUp += (s, e) =>
        {
            ellipse.Fill = _originalColor;
            _moveStartPoint = false;
            _scaleMaxX = _scaleMaxY = _scaleMinX = _scaleMinY = false;
            PlotView?.InvalidatePlot(false);
        };
    }

    private void SetupPointAnnotationHandlers(PointAnnotation point)
    {
        point.MouseDown += (s, e) =>
        {
            if (_addAnnotationToolMode != AddToolMode.None) return;
            if (e.ChangedButton != OxyMouseButton.Left) return;

            _lastScreenPoint = e.Position;
            _moveStartPoint = e.HitTestResult.Index == 0;
            _originalColor = point.Fill;
            point.Fill = OxyColors.Red;

            RaisePropertiesCalled(false, PropertyExpander.Annotations_Text, point);

            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        point.MouseMove += (s, e) =>
        {
            var dx = e.Position.X - _lastScreenPoint.X;
            var dy = e.Position.Y - _lastScreenPoint.Y;
            var screen = point.Transform(new DataPoint(point.X, point.Y));
            var newData = point.InverseTransform(new ScreenPoint(screen.X + dx, screen.Y + dy));

            if (_moveStartPoint)
            {
                point.X = newData.X;
                point.Y = newData.Y;
            }

            _lastScreenPoint = e.Position;
            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        point.MouseUp += (s, e) =>
        {
            point.Fill = _originalColor;
            PlotView?.InvalidatePlot(false);
        };
    }

    private void SetupPolygonAnnotationHandlers(PolygonAnnotation polygon)
    {
        polygon.MouseDown += (s, e) =>
        {
            if (_addAnnotationToolMode != AddToolMode.None) return;
            if (e.ChangedButton != OxyMouseButton.Left) return;

            _lastScreenPoint = e.Position;

            var screenToData = polygon.InverseTransform(e.Position);
            var screen2ToData = polygon.InverseTransform(new ScreenPoint(e.Position.X - 10, e.Position.Y - 10));
            var dxy = new DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y));

            // Check if over a vertex
            _movePointIndex = -1;
            for (int i = 0; i < polygon.Points.Count; i++)
            {
                if (Math.Abs(screenToData.X - polygon.Points[i].X) < dxy.X &&
                    Math.Abs(screenToData.Y - polygon.Points[i].Y) < dxy.Y)
                {
                    _movePointIndex = i;
                    break;
                }
            }

            // Ctrl+click to insert a new point on the nearest edge
            if (e.ModifierKeys.HasFlag(OxyModifierKeys.Control) && _movePointIndex == -1 && polygon.Points.Count >= 3)
            {
                var insertIndex = FindNearestPolygonEdgeIndex(polygon, screenToData);
                if (insertIndex >= 0)
                {
                    polygon.Points.Insert(insertIndex + 1, screenToData);
                    _movePointIndex = insertIndex + 1;
                }
            }

            _originalColor = polygon.Fill;
            polygon.Fill = OxyColors.Red;

            RaisePropertiesCalled(false, PropertyExpander.Annotations_Text, polygon);

            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        polygon.MouseMove += (s, e) =>
        {
            var dx = e.Position.X - _lastScreenPoint.X;
            var dy = e.Position.Y - _lastScreenPoint.Y;

            if (_movePointIndex >= 0 && _movePointIndex < polygon.Points.Count)
            {
                // Move single vertex
                var screen = polygon.Transform(polygon.Points[_movePointIndex]);
                var newData = polygon.InverseTransform(new ScreenPoint(screen.X + dx, screen.Y + dy));
                polygon.Points[_movePointIndex] = newData;
            }
            else
            {
                // Move entire polygon
                var newPoints = new List<DataPoint>();
                foreach (var pt in polygon.Points)
                {
                    var screen = polygon.Transform(pt);
                    var newData = polygon.InverseTransform(new ScreenPoint(screen.X + dx, screen.Y + dy));
                    newPoints.Add(newData);
                }
                polygon.Points.Clear();
                foreach (var pt in newPoints) polygon.Points.Add(pt);
            }

            _lastScreenPoint = e.Position;
            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        polygon.MouseUp += (s, e) =>
        {
            polygon.Fill = _originalColor;
            _movePointIndex = -1;
            PlotView?.InvalidatePlot(false);
        };
    }

    private void SetupPolylineAnnotationHandlers(PolylineAnnotation polyline)
    {
        polyline.MouseDown += (s, e) =>
        {
            if (_addAnnotationToolMode != AddToolMode.None) return;
            if (e.ChangedButton != OxyMouseButton.Left) return;

            _lastScreenPoint = e.Position;

            var screenToData = polyline.InverseTransform(e.Position);
            var screen2ToData = polyline.InverseTransform(new ScreenPoint(e.Position.X - 10, e.Position.Y - 10));
            var dxy = new DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y));

            // Check if over a vertex
            _movePointIndex = -1;
            for (int i = 0; i < polyline.Points.Count; i++)
            {
                if (Math.Abs(screenToData.X - polyline.Points[i].X) < dxy.X &&
                    Math.Abs(screenToData.Y - polyline.Points[i].Y) < dxy.Y)
                {
                    _movePointIndex = i;
                    break;
                }
            }

            // Ctrl+click to insert a new point on the nearest segment
            if (e.ModifierKeys.HasFlag(OxyModifierKeys.Control) && _movePointIndex == -1 && polyline.Points.Count >= 2)
            {
                var insertIndex = FindNearestSegmentIndex(polyline, screenToData);
                if (insertIndex >= 0)
                {
                    polyline.Points.Insert(insertIndex + 1, screenToData);
                    _movePointIndex = insertIndex + 1;
                }
            }

            _originalColor = polyline.Color;
            polyline.Color = OxyColors.Red;

            RaisePropertiesCalled(false, PropertyExpander.Annotations_Text, polyline);

            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        polyline.MouseMove += (s, e) =>
        {
            var dx = e.Position.X - _lastScreenPoint.X;
            var dy = e.Position.Y - _lastScreenPoint.Y;

            if (_movePointIndex >= 0 && _movePointIndex < polyline.Points.Count)
            {
                // Move single vertex
                var screen = polyline.Transform(polyline.Points[_movePointIndex]);
                var newData = polyline.InverseTransform(new ScreenPoint(screen.X + dx, screen.Y + dy));
                polyline.Points[_movePointIndex] = newData;
            }
            else
            {
                // Move entire polyline
                var newPoints = new List<DataPoint>();
                foreach (var pt in polyline.Points)
                {
                    var screen = polyline.Transform(pt);
                    var newData = polyline.InverseTransform(new ScreenPoint(screen.X + dx, screen.Y + dy));
                    newPoints.Add(newData);
                }
                polyline.Points.Clear();
                foreach (var pt in newPoints) polyline.Points.Add(pt);
            }

            _lastScreenPoint = e.Position;
            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        polyline.MouseUp += (s, e) =>
        {
            polyline.Color = _originalColor;
            _movePointIndex = -1;
            PlotView?.InvalidatePlot(false);
        };
    }

    private void SetupLineAnnotationHandlers(LineAnnotation line)
    {
        line.MouseDown += (s, e) =>
        {
            if (_addAnnotationToolMode != AddToolMode.None) return;
            if (e.ChangedButton != OxyMouseButton.Left) return;

            _lastScreenPoint = e.Position;
            _moveStartPoint = e.HitTestResult.Index == 0;
            _originalColor = line.Color;
            line.Color = OxyColors.Red;

            RaisePropertiesCalled(false, PropertyExpander.Annotations_Text, line);

            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        line.MouseMove += (s, e) =>
        {
            if (_moveStartPoint)
            {
                var newData = line.InverseTransform(e.Position);

                if (line.Type == LineAnnotationType.Vertical)
                {
                    line.X = newData.X;
                }
                else if (line.Type == LineAnnotationType.Horizontal)
                {
                    line.Y = newData.Y;
                }
            }

            _lastScreenPoint = e.Position;
            PlotView?.InvalidatePlot(false);
            e.Handled = true;
        };

        line.MouseUp += (s, e) =>
        {
            line.Color = _originalColor;
            PlotView?.InvalidatePlot(false);
        };
    }

    #endregion

    #region Mouse Events

    /// <summary>
    /// Determines the behavior for the mouse down event.
    /// </summary>
    private void PlotModelMouseDown(object? sender, OxyMouseDownEventArgs e)
    {
        if (PlotView?.ActualModel == null) return;

        // Handle annotation creation mode
        if (_addAnnotationToolMode != AddToolMode.None)
        {
            HandleAnnotationCreation(e);
            return;
        }

        // Handle right-click context menu
        if (e.ChangedButton == OxyMouseButton.Right)
        {
            PlotView.Cursor = Cursors.Arrow;
            ShowContextMenu(e);
            return;
        }

        // Handle pan cursor
        if (PanButton.IsChecked == true || e.ChangedButton == OxyMouseButton.Middle)
        {
            PlotView.Cursor = _panHandClosedCursor;
        }
    }

    /// <summary>
    /// Determines the behavior for the mouse move event.
    /// </summary>
    private void PlotModelMouseMove(object? sender, OxyMouseEventArgs e)
    {
        if (PlotView?.ActualModel == null) return;

        // Handle annotation creation preview
        if (_addAnnotationToolMode != AddToolMode.None && _targetAddAnnotation != null)
        {
            UpdateAnnotationDuringCreation(e);
            return;
        }

        // Show edit point feedback when hovering over annotations
        UpdateEditPointFeedback(e);
    }

    /// <summary>
    /// Determines the behavior for the mouse up event.
    /// </summary>
    private void PlotModelMouseUp(object? sender, OxyMouseEventArgs e)
    {
        if (PlotView?.ActualModel == null) return;

        if (_addAnnotationToolMode == AddToolMode.AddPolygonAnnotation ||
            _addAnnotationToolMode == AddToolMode.AddPolylineAnnotation)
        {
            if (_doubleClicked) StopAddAnnotation();
        }
        else if (_addAnnotationToolMode == AddToolMode.AddHorizontalLineAnnotation ||
                 _addAnnotationToolMode == AddToolMode.AddVerticalLineAnnotation)
        {
            CloseLineAnnotationTooltip();
            StopAddAnnotation();
        }
        else if (_addAnnotationToolMode == AddToolMode.AddRectangleAnnotation ||
                 _addAnnotationToolMode == AddToolMode.AddEllipseAnnotation)
        {
            EnsureMinimumAnnotationSize(e);
            StopAddAnnotation();
        }
        else if (_addAnnotationToolMode == AddToolMode.AddArrowAnnotation)
        {
            EnsureArrowHasLength();
            StopAddAnnotation();
        }
        else if (_addAnnotationToolMode != AddToolMode.None)
        {
            StopAddAnnotation();
        }

        // Reset pan cursor
        if (PanButton.IsChecked == true)
        {
            PlotView.Cursor = _panHandCursor;
        }
    }

    /// <summary>
    /// Handles annotation creation during mouse down.
    /// </summary>
    private void HandleAnnotationCreation(OxyMouseDownEventArgs e)
    {
        if (PlotView?.ActualModel == null) return;

        var dataPoint = ConvertScreenToDataPoint(e.Position);

        switch (_addAnnotationToolMode)
        {
            case AddToolMode.AddArrowAnnotation:
                var arrow = new ArrowAnnotation
                {
                    StartPoint = dataPoint,
                    EndPoint = dataPoint,
                    Text = "Arrow Annotation",
                    Color = OxyColors.Blue,
                    StrokeThickness = 2
                };
                PlotView.ActualModel.Annotations.Add(arrow);
                _targetAddAnnotation = arrow;
                RaisePropertiesCalled(true, PropertyExpander.Annotations_Text, arrow);
                break;

            case AddToolMode.AddTextAnnotation:
                var text = new TextAnnotation
                {
                    TextPosition = dataPoint,
                    Text = "Text Annotation",
                    Stroke = OxyColors.Black,
                    StrokeThickness = 1
                };
                PlotView.ActualModel.Annotations.Add(text);
                _targetAddAnnotation = text;
                RaisePropertiesCalled(true, PropertyExpander.Annotations_Text, text);
                break;

            case AddToolMode.AddVerticalLineAnnotation:
                var vLine = new LineAnnotation
                {
                    Type = LineAnnotationType.Vertical,
                    X = dataPoint.X,
                    Text = "Vertical Line Annotation",
                    Color = OxyColors.Red,
                    StrokeThickness = 1
                };
                PlotView.ActualModel.Annotations.Add(vLine);
                _targetAddAnnotation = vLine;
                RaisePropertiesCalled(true, PropertyExpander.Annotations_Text, vLine);
                OpenLineAnnotationTooltip(vLine);
                break;

            case AddToolMode.AddHorizontalLineAnnotation:
                var hLine = new LineAnnotation
                {
                    Type = LineAnnotationType.Horizontal,
                    Y = dataPoint.Y,
                    Text = "Horizontal Line Annotation",
                    Color = OxyColors.Red,
                    StrokeThickness = 1
                };
                PlotView.ActualModel.Annotations.Add(hLine);
                _targetAddAnnotation = hLine;
                RaisePropertiesCalled(true, PropertyExpander.Annotations_Text, hLine);
                OpenLineAnnotationTooltip(hLine);
                break;

            case AddToolMode.AddRectangleAnnotation:
                var rect = new RectangleAnnotation
                {
                    MinimumX = dataPoint.X,
                    MaximumX = dataPoint.X,
                    MinimumY = dataPoint.Y,
                    MaximumY = dataPoint.Y,
                    Text = "Rectangle Annotation",
                    Fill = OxyColor.FromAColor(80, OxyColors.LightBlue),
                    Stroke = OxyColors.Blue,
                    StrokeThickness = 1
                };
                PlotView.ActualModel.Annotations.Add(rect);
                _targetAddAnnotation = rect;
                RaisePropertiesCalled(true, PropertyExpander.Annotations_Text, rect);
                break;

            case AddToolMode.AddEllipseAnnotation:
                var ellipse = new EllipseAnnotation
                {
                    X = dataPoint.X,
                    Y = dataPoint.Y,
                    Width = 0,
                    Height = 0,
                    Text = "Ellipse Annotation",
                    Fill = OxyColor.FromAColor(80, OxyColors.LightGreen),
                    Stroke = OxyColors.Green,
                    StrokeThickness = 1
                };
                PlotView.ActualModel.Annotations.Add(ellipse);
                _targetAddAnnotation = ellipse;
                RaisePropertiesCalled(true, PropertyExpander.Annotations_Text, ellipse);
                break;

            case AddToolMode.AddPointAnnotation:
                var point = new PointAnnotation
                {
                    X = dataPoint.X,
                    Y = dataPoint.Y,
                    Text = "Point Annotation",
                    Size = 5,
                    Fill = OxyColors.Red,
                    Stroke = OxyColors.Black,
                    StrokeThickness = 1
                };
                PlotView.ActualModel.Annotations.Add(point);
                _targetAddAnnotation = point;
                RaisePropertiesCalled(true, PropertyExpander.Annotations_Text, point);
                break;

            case AddToolMode.AddPolygonAnnotation:
                if (_targetAddAnnotation == null)
                {
                    var polygon = new PolygonAnnotation
                    {
                        Text = "Polygon Annotation",
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
                    _doubleClicked = e.ClickCount > 1;
                    var polygon = (PolygonAnnotation)_targetAddAnnotation;
                    if (polygon.Points.Count == 3)
                    {
                        PlotView.ActualModel.Annotations.Add(polygon);
                        RaisePropertiesCalled(true, PropertyExpander.Annotations_Text, polygon);
                    }
                    if (!_doubleClicked)
                    {
                        polygon.Points.Add(dataPoint);
                        LeaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
                    }
                }
                break;

            case AddToolMode.AddPolylineAnnotation:
                if (_targetAddAnnotation == null)
                {
                    var polyline = new PolylineAnnotation
                    {
                        Text = "Polyline Annotation",
                        Color = OxyColors.Purple,
                        StrokeThickness = 2
                    };
                    polyline.Points.Add(dataPoint);
                    PlotView.ActualModel.Annotations.Add(polyline);
                    LeaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
                    LeaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
                    _targetAddAnnotation = polyline;
                    RaisePropertiesCalled(true, PropertyExpander.Annotations_Text, polyline);
                }
                else
                {
                    _doubleClicked = e.ClickCount > 1;
                    if (!_doubleClicked)
                    {
                        ((PolylineAnnotation)_targetAddAnnotation).Points.Add(dataPoint);
                        LeaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
                    }
                }
                break;
        }

        PlotView.InvalidatePlot(false);
    }

    /// <summary>
    /// Updates annotation during creation (mouse move).
    /// </summary>
    private void UpdateAnnotationDuringCreation(OxyMouseEventArgs e)
    {
        if (PlotView?.ActualModel == null || _targetAddAnnotation == null) return;

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
                UpdateLineAnnotationTooltip(line);
                break;

            case LineAnnotation line when line.Type == LineAnnotationType.Horizontal:
                line.Y = dataPoint.Y;
                UpdateLineAnnotationTooltip(line);
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

            case PointAnnotation point:
                point.X = dataPoint.X;
                point.Y = dataPoint.Y;
                break;

            case PolygonAnnotation:
            case PolylineAnnotation:
                if (LeaderLine.Points.Count > 0)
                {
                    LeaderLine.Points[^1] = new Point(e.Position.X, e.Position.Y);
                }
                break;
        }

        PlotView.InvalidatePlot(false);
    }

    /// <summary>
    /// Updates visual edit point feedback when hovering over annotations.
    /// </summary>
    private void UpdateEditPointFeedback(OxyMouseEventArgs e)
    {
        if (PlotView?.ActualModel == null) return;

        Cursor? updatedCursor = null;

        foreach (var annotation in PlotView.ActualModel.Annotations)
        {
            var hitResult = annotation.HitTest(new HitTestArguments(e.Position, 10));
            if (hitResult == null) continue;

            switch (annotation)
            {
                case ArrowAnnotation:
                    updatedCursor = hitResult.Index == 0 ? Cursors.SizeAll : _movePointsCursor;
                    break;

                case TextAnnotation:
                    if (hitResult.Index == 0) updatedCursor = Cursors.SizeAll;
                    break;

                case RectangleAnnotation rect:
                    updatedCursor = GetRectangleCursor(rect, e.Position);
                    break;

                case EllipseAnnotation ellipse:
                    updatedCursor = GetEllipseCursor(ellipse, e.Position);
                    break;

                case PointAnnotation:
                    if (hitResult.Index == 0) updatedCursor = Cursors.SizeAll;
                    break;

                case PolygonAnnotation polygon:
                    updatedCursor = GetPolygonCursor(polygon, e.Position);
                    break;

                case PolylineAnnotation polyline:
                    updatedCursor = GetPolylineCursor(polyline, e.Position);
                    break;

                case LineAnnotation:
                    if (hitResult.Index == 0) updatedCursor = Cursors.SizeAll;
                    break;
            }

            if (updatedCursor != null) break;
        }

        PlotView.Cursor = updatedCursor ?? (PanButton.IsChecked == true ? _panHandCursor :
                                             ZoomButton.IsChecked == true ? _zoomCursor : Cursors.Arrow);
    }

    private Cursor GetRectangleCursor(RectangleAnnotation rect, ScreenPoint position)
    {
        var ur = rect.Transform(Math.Max(rect.MaximumX, rect.MinimumX), Math.Max(rect.MaximumY, rect.MinimumY));
        var ll = rect.Transform(Math.Min(rect.MinimumX, rect.MaximumX), Math.Min(rect.MinimumY, rect.MaximumY));
        var topRight = new ScreenPoint(Math.Abs(ur.X - position.X), Math.Abs(ur.Y - position.Y));
        var bottomLeft = new ScreenPoint(Math.Abs(ll.X - position.X), Math.Abs(ll.Y - position.Y));

        // Corners
        if (topRight.X < 10 && topRight.Y < 10) return Cursors.SizeNESW;
        if (bottomLeft.X < 10 && bottomLeft.Y < 10) return Cursors.SizeNESW;
        if (bottomLeft.X < 10 && topRight.Y < 10) return Cursors.SizeNWSE;
        if (topRight.X < 10 && bottomLeft.Y < 10) return Cursors.SizeNWSE;
        // Edges
        if (topRight.X < 10 || bottomLeft.X < 10) return Cursors.SizeWE;
        if (topRight.Y < 10 || bottomLeft.Y < 10) return Cursors.SizeNS;

        return Cursors.SizeAll;
    }

    private Cursor GetEllipseCursor(EllipseAnnotation ellipse, ScreenPoint position)
    {
        var ur = ellipse.Transform(ellipse.X + ellipse.Width / 2, ellipse.Y + ellipse.Height / 2);
        var ll = ellipse.Transform(ellipse.X - ellipse.Width / 2, ellipse.Y - ellipse.Height / 2);
        var topRight = new ScreenPoint(Math.Abs(ur.X - position.X), Math.Abs(ur.Y - position.Y));
        var bottomLeft = new ScreenPoint(Math.Abs(ll.X - position.X), Math.Abs(ll.Y - position.Y));

        // Corners
        if (topRight.X < 10 && topRight.Y < 10) return Cursors.SizeNESW;
        if (bottomLeft.X < 10 && bottomLeft.Y < 10) return Cursors.SizeNESW;
        if (bottomLeft.X < 10 && topRight.Y < 10) return Cursors.SizeNWSE;
        if (topRight.X < 10 && bottomLeft.Y < 10) return Cursors.SizeNWSE;
        // Edges
        if (topRight.X < 10 || bottomLeft.X < 10) return Cursors.SizeWE;
        if (topRight.Y < 10 || bottomLeft.Y < 10) return Cursors.SizeNS;

        return Cursors.SizeAll;
    }

    private Cursor GetPolygonCursor(PolygonAnnotation polygon, ScreenPoint position)
    {
        var screenToData = polygon.InverseTransform(position);
        var screen2ToData = polygon.InverseTransform(new ScreenPoint(position.X - 10, position.Y - 10));
        var dxy = new DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y));

        // Check if over a vertex
        if (polygon.Points.Any(p => Math.Abs(screenToData.X - p.X) < dxy.X && Math.Abs(screenToData.Y - p.Y) < dxy.Y))
        {
            return _movePointsCursor ?? Cursors.SizeAll;
        }

        return Cursors.SizeAll;
    }

    private Cursor GetPolylineCursor(PolylineAnnotation polyline, ScreenPoint position)
    {
        var screenToData = polyline.InverseTransform(position);
        var screen2ToData = polyline.InverseTransform(new ScreenPoint(position.X - 10, position.Y - 10));
        var dxy = new DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y));

        // Check if over a vertex
        if (polyline.Points.Any(p => Math.Abs(screenToData.X - p.X) < dxy.X && Math.Abs(screenToData.Y - p.Y) < dxy.Y))
        {
            return _movePointsCursor ?? Cursors.SizeAll;
        }

        return Cursors.SizeAll;
    }

    private void EnsureMinimumAnnotationSize(OxyMouseEventArgs e)
    {
        if (PlotView?.ActualModel == null) return;

        var plotArea = PlotView.ActualModel.PlotArea;

        if (_targetAddAnnotation is RectangleAnnotation rect)
        {
            var upperRight = rect.Transform(rect.MaximumX, rect.MaximumY);
            var lowerLeft = rect.Transform(rect.MinimumX, rect.MinimumY);
            var pixelWidth = Math.Abs(upperRight.X - lowerLeft.X);
            var pixelHeight = Math.Abs(upperRight.Y - lowerLeft.Y);

            if (pixelWidth < 10 || pixelHeight < 10)
            {
                var plotLL = rect.InverseTransform(new ScreenPoint(plotArea.Left, plotArea.Bottom));
                var plotUR = rect.InverseTransform(new ScreenPoint(plotArea.Right, plotArea.Top));
                var centerXShift = Math.Abs((plotUR.X - plotLL.X) * 0.1);
                var centerYShift = Math.Abs((plotUR.Y - plotLL.Y) * 0.1);
                var mouseData = rect.InverseTransform(e.Position);

                if (pixelWidth < 10)
                {
                    rect.MinimumX = mouseData.X - centerXShift;
                    rect.MaximumX = mouseData.X + centerXShift;
                }
                if (pixelHeight < 10)
                {
                    rect.MinimumY = mouseData.Y - centerYShift;
                    rect.MaximumY = mouseData.Y + centerYShift;
                }
            }
        }
        else if (_targetAddAnnotation is EllipseAnnotation ellipse)
        {
            if (ellipse.Width < 0.001 || ellipse.Height < 0.001)
            {
                var plotLL = ellipse.InverseTransform(new ScreenPoint(plotArea.Left, plotArea.Bottom));
                var plotUR = ellipse.InverseTransform(new ScreenPoint(plotArea.Right, plotArea.Top));
                ellipse.Width = Math.Abs((plotUR.X - plotLL.X) * 0.2);
                ellipse.Height = Math.Abs((plotUR.Y - plotLL.Y) * 0.2);
            }
        }
    }

    private void EnsureArrowHasLength()
    {
        if (_targetAddAnnotation is ArrowAnnotation arrow)
        {
            if (Math.Abs(arrow.StartPoint.X - arrow.EndPoint.X) < 0.000001 &&
                Math.Abs(arrow.StartPoint.Y - arrow.EndPoint.Y) < 0.000001)
            {
                if (PlotView?.ActualModel != null)
                {
                    var plotArea = PlotView.ActualModel.PlotArea;
                    var center = ConvertScreenToDataPoint(plotArea.Center);
                    var xShift = plotArea.Width * 0.05;
                    var shifted = ConvertScreenToDataPoint(new ScreenPoint(plotArea.Center.X + xShift, plotArea.Center.Y));
                    arrow.StartPoint = new DataPoint(arrow.StartPoint.X + (shifted.X - center.X), arrow.StartPoint.Y);
                }
            }
        }
    }

    #endregion

    #region Context Menu

    /// <summary>
    /// Shows context menu for right-click.
    /// </summary>
    private void ShowContextMenu(OxyMouseDownEventArgs e)
    {
        if (PlotView?.ActualModel == null) return;

        _contextMenu = new ContextMenu();

        // Check for annotation hit
        foreach (var annotation in PlotView.ActualModel.Annotations)
        {
            var hitResult = annotation.HitTest(new HitTestArguments(e.Position, 10));
            if (hitResult != null)
            {
                AddAnnotationContextMenuItems(annotation);
                break;
            }
        }

        if (_contextMenu.Items.Count > 0)
        {
            _contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
            _contextMenu.IsOpen = true;
        }
    }

    private void AddAnnotationContextMenuItems(Annotation annotation)
    {
        var annotationText = GetAnnotationText(annotation);

        var editItem = new MenuItem { Header = $"Edit Annotation Text: {annotationText}" };
        editItem.Click += (s, e) =>
        {
            RaisePropertiesCalled(false, PropertyExpander.Annotations_Text, annotation);
        };

        var formatItem = new MenuItem { Header = $"Format Annotation: {annotationText}" };
        formatItem.Click += (s, e) =>
        {
            RaisePropertiesCalled(true, PropertyExpander.Annotations_Text, annotation);
        };

        var deleteItem = new MenuItem { Header = $"Delete Annotation: {annotationText}" };
        deleteItem.Click += (s, e) =>
        {
            PlotView?.ActualModel?.Annotations.Remove(annotation);
            PlotView?.InvalidatePlot(false);
        };

        _contextMenu?.Items.Add(editItem);
        _contextMenu?.Items.Add(formatItem);
        _contextMenu?.Items.Add(deleteItem);
    }

    private string GetAnnotationText(Annotation annotation)
    {
        return annotation switch
        {
            ArrowAnnotation a => a.Text ?? "Arrow",
            TextAnnotation t => t.Text ?? "Text",
            RectangleAnnotation r => r.Text ?? "Rectangle",
            EllipseAnnotation e => e.Text ?? "Ellipse",
            PointAnnotation p => p.Text ?? "Point",
            PolygonAnnotation pg => pg.Text ?? "Polygon",
            PolylineAnnotation pl => pl.Text ?? "Polyline",
            LineAnnotation l => l.Text ?? "Line",
            _ => "Annotation"
        };
    }

    #endregion

    #region Line Annotation Tooltips

    private void OpenLineAnnotationTooltip(LineAnnotation line)
    {
        // Close any previous tooltip
        CloseLineAnnotationTooltip();

        _lineAnnotationTooltip = new ToolTip
        {
            IsOpen = true,
            Placement = System.Windows.Controls.Primitives.PlacementMode.Relative,
            PlacementTarget = PlotView,
            Background = System.Windows.Media.Brushes.White,
            BorderBrush = System.Windows.Media.Brushes.Transparent,
            Padding = new Thickness(2),
            Margin = new Thickness(0)
        };
        UpdateLineAnnotationTooltip(line);
    }

    private void UpdateLineAnnotationTooltip(LineAnnotation line)
    {
        if (_lineAnnotationTooltip == null || PlotView?.ActualModel == null) return;

        // Use axis-aware formatting (DateTimeAxis shows dates, CategoryAxis shows labels, etc.)
        if (line.Type == LineAnnotationType.Vertical)
        {
            var xAxis = line.XAxis ?? PlotView.ActualModel.DefaultXAxis;
            if (xAxis != null)
            {
                // Format the value using the axis's own formatting logic
                _lineAnnotationTooltip.Content = xAxis.FormatValue(line.X);

                // Position tooltip at the bottom of the line
                var dataPoint = xAxis.IsReversed
                    ? new DataPoint(line.X, line.YAxis?.ActualMaximum ?? 0)
                    : new DataPoint(line.X, line.YAxis?.ActualMinimum ?? 0);
                var screenPoint = line.Transform(dataPoint);
                _lineAnnotationTooltip.UpdateLayout();
                _lineAnnotationTooltip.VerticalOffset = screenPoint.Y;
                _lineAnnotationTooltip.HorizontalOffset = screenPoint.X - (_lineAnnotationTooltip.ActualWidth / 2);
            }
        }
        else if (line.Type == LineAnnotationType.Horizontal)
        {
            var yAxis = line.YAxis ?? PlotView.ActualModel.DefaultYAxis;
            if (yAxis != null)
            {
                // Format the value using the axis's own formatting logic
                _lineAnnotationTooltip.Content = yAxis.FormatValue(line.Y);

                // Position tooltip at the left of the line
                var dataPoint = line.XAxis?.IsReversed == true
                    ? new DataPoint(line.XAxis.ActualMaximum, line.Y)
                    : new DataPoint(line.XAxis?.ActualMinimum ?? 0, line.Y);
                var screenPoint = line.Transform(dataPoint);
                _lineAnnotationTooltip.UpdateLayout();
                _lineAnnotationTooltip.VerticalOffset = screenPoint.Y - (_lineAnnotationTooltip.ActualHeight / 2);
                _lineAnnotationTooltip.HorizontalOffset = screenPoint.X - _lineAnnotationTooltip.ActualWidth;
            }
        }
    }

    private void CloseLineAnnotationTooltip()
    {
        if (_lineAnnotationTooltip != null)
        {
            _lineAnnotationTooltip.IsOpen = false;
            _lineAnnotationTooltip = null;
        }
    }

    #endregion

    #region Properties Button

    private void PropertiesButton_Click(object sender, RoutedEventArgs e)
    {
        RaisePropertiesCalled(true, null, null);
    }

    private void RaisePropertiesCalled(bool openProperties, PropertyExpander? expander, object? selectedObject)
    {
        PropertiesCalled?.Invoke(this, new PropertiesCalledEventArgs
        {
            TargetPlot = PlotView,
            OpenProperties = openProperties,
            PropertyExpander = expander,
            SelectedObject = selectedObject
        });
    }

    #endregion

    #region Swap Axes

    private void SwapAxesButton_Click(object sender, RoutedEventArgs e)
    {
        if (PlotView?.ActualModel == null) return;

        // Check if any series type prevents swapping
        foreach (var series in PlotView.ActualModel.Series)
        {
            if (NonSwapSeriesTypes.Contains(series.GetType())) return;
        }

        // Swap axis positions
        foreach (var axis in PlotView.ActualModel.Axes)
        {
            axis.Position = axis.Position switch
            {
                AxisPosition.Bottom => AxisPosition.Left,
                AxisPosition.Left => AxisPosition.Bottom,
                AxisPosition.Top => AxisPosition.Right,
                AxisPosition.Right => AxisPosition.Top,
                _ => axis.Position
            };
        }

        // Swap data points in series
        foreach (var series in PlotView.ActualModel.Series)
        {
            SwapSeriesData(series);
        }

        PlotView.InvalidatePlot(true);
    }

    private void SwapSeriesData(OxyPlot.Series.Series series)
    {
        switch (series)
        {
            case OxyPlot.Series.LineSeries lineSeries:
                SwapDataPoints(lineSeries.Points);
                break;
            case OxyPlot.Series.ScatterSeries scatterSeries:
                SwapScatterPoints(scatterSeries.Points);
                break;
            case OxyPlot.Series.AreaSeries areaSeries:
                SwapDataPoints(areaSeries.Points);
                SwapDataPoints(areaSeries.Points2);
                break;
        }
    }

    private void SwapDataPoints(IList<DataPoint> points)
    {
        var swapped = points.Select(p => new DataPoint(p.Y, p.X)).ToList();
        points.Clear();
        foreach (var p in swapped) points.Add(p);
    }

    private void SwapScatterPoints(IList<ScatterPoint> points)
    {
        var swapped = points.Select(p => new ScatterPoint(p.Y, p.X, p.Size, p.Value)).ToList();
        points.Clear();
        foreach (var p in swapped) points.Add(p);
    }

    #endregion

    #region Export Series Data

    private static readonly string[] BadCharacters = { ":", "\\", "/", "?", "*", "[", "]" };

    private void ExportDataButton_Click(object sender, RoutedEventArgs e)
    {
        if (PlotView?.ActualModel == null) return;

        var tableList = new List<DataTable>();
        int tableCount = 0;

        foreach (var series in PlotView.ActualModel.Series)
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

                    foreach (var point in lineSeries.Points)
                    {
                        dataTable.Rows.Add(dataTable.Rows.Count + 1, point.X, point.Y);
                    }
                    break;

                case OxyPlot.Series.ScatterSeries scatterSeries:
                    seriesName = GetSeriesName(scatterSeries.Title, "ScatterSeries", tableCount);
                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_x", typeof(string));
                    dataTable.Columns.Add(seriesName + "_y", typeof(string));

                    foreach (var point in scatterSeries.Points)
                    {
                        dataTable.Rows.Add(dataTable.Rows.Count + 1, point.X, point.Y);
                    }
                    break;

                case OxyPlot.Series.AreaSeries areaSeries:
                    seriesName = GetSeriesName(areaSeries.Title, "AreaSeries", tableCount);
                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_x", typeof(string));
                    dataTable.Columns.Add(seriesName + "_y", typeof(string));

                    foreach (var point in areaSeries.Points)
                    {
                        dataTable.Rows.Add(dataTable.Rows.Count + 1, point.X, point.Y);
                    }
                    break;

                case OxyPlot.Series.HistogramSeries histogramSeries:
                    seriesName = GetSeriesName(histogramSeries.Title, "HistogramSeries", tableCount);
                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_rangeStart", typeof(string));
                    dataTable.Columns.Add(seriesName + "_rangeEnd", typeof(string));
                    dataTable.Columns.Add(seriesName + "_area", typeof(string));

                    foreach (var item in histogramSeries.Items)
                    {
                        dataTable.Rows.Add(dataTable.Rows.Count + 1, item.RangeStart, item.RangeEnd, item.Area);
                    }
                    break;

                case ColumnSeries columnSeries:
                    seriesName = GetSeriesName(columnSeries.Title, "ColumnSeries", tableCount);
                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_categoryIndex", typeof(string));
                    dataTable.Columns.Add(seriesName + "_value", typeof(string));

                    foreach (var item in columnSeries.Items)
                    {
                        dataTable.Rows.Add(dataTable.Rows.Count + 1, item.CategoryIndex, item.Value);
                    }
                    break;

                case OxyPlot.Series.BarSeries barSeries:
                    seriesName = GetSeriesName(barSeries.Title, "BarSeries", tableCount);
                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_categoryIndex", typeof(string));
                    dataTable.Columns.Add(seriesName + "_value", typeof(string));

                    foreach (var item in barSeries.Items)
                    {
                        dataTable.Rows.Add(dataTable.Rows.Count + 1, item.CategoryIndex, item.Value);
                    }
                    break;

                case OxyPlot.Series.BoxPlotSeries boxPlotSeries:
                    seriesName = GetSeriesName(boxPlotSeries.Title, "BoxPlotSeries", tableCount);
                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_position", typeof(string));
                    dataTable.Columns.Add(seriesName + "_lowerWhisker", typeof(string));
                    dataTable.Columns.Add(seriesName + "_boxBottom", typeof(string));
                    dataTable.Columns.Add(seriesName + "_median", typeof(string));
                    dataTable.Columns.Add(seriesName + "_boxTop", typeof(string));
                    dataTable.Columns.Add(seriesName + "_upperWhisker", typeof(string));

                    foreach (var item in boxPlotSeries.Items)
                    {
                        dataTable.Rows.Add(dataTable.Rows.Count + 1, item.X, item.LowerWhisker, item.BoxBottom, item.Median, item.BoxTop, item.UpperWhisker);
                    }
                    break;

                case HeatMapSeries heatMapSeries:
                    if (heatMapSeries.Data == null) continue;
                    seriesName = GetSeriesName(heatMapSeries.Title, "HeatMapSeries", tableCount);
                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add("y", typeof(string));

                    for (int x = 0; x < heatMapSeries.Data.GetLength(0); x++)
                    {
                        dataTable.Columns.Add($"x{x}", typeof(string));
                    }

                    var yCount = heatMapSeries.Data.GetLength(1);
                    for (int y = 0; y < yCount; y++)
                    {
                        var row = dataTable.NewRow();
                        row[0] = y + 1;
                        // Avoid division by zero when there's only one row
                        row[1] = yCount > 1
                            ? heatMapSeries.Y0 + y * (heatMapSeries.Y1 - heatMapSeries.Y0) / (yCount - 1)
                            : heatMapSeries.Y0;
                        for (int x = 0; x < heatMapSeries.Data.GetLength(0); x++)
                        {
                            row[x + 2] = heatMapSeries.Data[x, y];
                        }
                        dataTable.Rows.Add(row);
                    }
                    break;

                case OxyPlot.Series.ScatterErrorSeries scatterErrorSeries:
                    seriesName = GetSeriesName(scatterErrorSeries.Title, "ScatterErrorSeries", tableCount);
                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_x", typeof(string));
                    dataTable.Columns.Add(seriesName + "_y", typeof(string));
                    dataTable.Columns.Add(seriesName + "_errorX", typeof(string));
                    dataTable.Columns.Add(seriesName + "_errorY", typeof(string));

                    foreach (var point in scatterErrorSeries.Points)
                    {
                        if (point is ScatterErrorPoint errorPoint)
                        {
                            dataTable.Rows.Add(dataTable.Rows.Count + 1, errorPoint.X, errorPoint.Y, errorPoint.ErrorX, errorPoint.ErrorY);
                        }
                    }
                    break;

                case OxyPlot.Series.ContourSeries:
                    continue;

                default:
                    continue;
            }

            if (dataTable.Rows.Count > 0)
            {
                tableList.Add(dataTable);
            }
        }

        if (tableList.Count == 0)
        {
            MessageBox.Show("No exportable series data found.", "Export Data", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        ExportTables(tableList);
    }

    private string GetSeriesName(string? title, string defaultPrefix, int tableCount)
    {
        string name = !string.IsNullOrEmpty(title) ? title : $"{defaultPrefix}_{tableCount}";
        foreach (var badChar in BadCharacters)
        {
            name = name.Replace(badChar, "_");
        }
        return name;
    }

    private void ExportTables(List<DataTable> tableList)
    {
        var filters = "CSV (*.csv)|*.csv|Excel (*.xlsx)|*.xlsx|SQLite (*.sqlite)|*.sqlite";
        var saveDialog = new SaveFileDialog { Filter = filters, FilterIndex = 1 };

        if (saveDialog.ShowDialog() != true) return;

        try
        {
            var extension = Path.GetExtension(saveDialog.FileName).ToLower();

            switch (extension)
            {
                case ".csv":
                    // Combine all tables since CSV only supports one sheet/table
                    var mergedTable = MergeAllTables(tableList, "id");
                    var csvDataView = new InMemoryReader(mergedTable).GetTableManager(mergedTable.TableName);
                    csvDataView?.ExportToCsv(saveDialog.FileName);
                    break;

                case ".xlsx":
                    // Save each DataTable to the file as a new sheet
                    foreach (var dt in tableList)
                    {
                        var dataView = new InMemoryReader(dt).GetTableManager(dt.TableName);
                        dataView?.ExportToXlsx(saveDialog.FileName);
                    }
                    break;

                case ".sqlite":
                    // Save each DataTable to the file as a new table
                    foreach (var dt in tableList)
                    {
                        var dataView = new InMemoryReader(dt).GetTableManager(dt.TableName);
                        dataView?.ExportToSqlite(saveDialog.FileName, dataView.TableName);
                    }
                    break;

                default:
                    throw new NotSupportedException($"Selected file format extension '{extension}' is not supported for export.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private DataTable MergeAllTables(IList<DataTable> tables, string primaryKeyColumn)
    {
        if (!tables.Any()) throw new ArgumentException("Tables must not be empty", nameof(tables));
        if (tables.Count == 1) return tables[0];

        var merged = new DataTable("Merged");
        merged.BeginLoadData();

        foreach (var table in tables)
        {
            merged.Merge(table);
        }

        merged.EndLoadData();

        if (primaryKeyColumn != null)
        {
            var groups = merged.AsEnumerable().GroupBy(r => r[primaryKeyColumn]);
            var duplicates = groups.Where(g => g.Count() > 1);

            foreach (var group in duplicates)
            {
                var firstRow = group.First();
                foreach (DataColumn col in merged.Columns)
                {
                    if (firstRow.IsNull(col))
                    {
                        var nonNullRow = group.Skip(1).FirstOrDefault(r => !r.IsNull(col));
                        if (nonNullRow != null) firstRow[col] = nonNullRow[col];
                    }
                }

                foreach (var row in group.Skip(1).ToList())
                {
                    merged.Rows.Remove(row);
                }
            }
        }

        return merged;
    }

    private void ExportDataTableToCsv(DataTable dataTable, string fileName)
    {
        var sb = new StringBuilder();

        var columnNames = dataTable.Columns.Cast<DataColumn>().Select(c => EscapeCsvField(c.ColumnName));
        sb.AppendLine(string.Join(",", columnNames));

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

    #endregion

    #region Save Plot

    private void SaveImageButton_Click(object sender, RoutedEventArgs e)
    {
        if (PlotView == null) return;

        try
        {
            var dialog = new SavePlotImageDialog(PlotView)
            {
                Owner = Window.GetWindow(this)
            };
            dialog.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error opening save dialog: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region Helper Methods

    private DataPoint ConvertScreenToDataPoint(ScreenPoint screenPoint)
    {
        if (PlotView?.ActualModel == null)
            return new DataPoint(0, 0);

        var xAxis = PlotView.ActualModel.DefaultXAxis;
        var yAxis = PlotView.ActualModel.DefaultYAxis;

        if (xAxis == null || yAxis == null)
            return new DataPoint(0, 0);

        return xAxis.InverseTransform(screenPoint.X, screenPoint.Y, yAxis);
    }

    /// <summary>
    /// Finds the index of the nearest segment in a polyline to the given point.
    /// </summary>
    /// <param name="polyline">The polyline annotation.</param>
    /// <param name="point">The point to find the nearest segment to.</param>
    /// <returns>The index of the first point of the nearest segment, or -1 if not found.</returns>
    private static int FindNearestSegmentIndex(PolylineAnnotation polyline, DataPoint point)
    {
        if (polyline.Points.Count < 2) return -1;

        var minDistance = double.MaxValue;
        var nearestIndex = -1;

        for (int i = 0; i < polyline.Points.Count - 1; i++)
        {
            var p1 = polyline.Points[i];
            var p2 = polyline.Points[i + 1];
            var distance = PointToSegmentDistance(point, p1, p2);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestIndex = i;
            }
        }

        return nearestIndex;
    }

    /// <summary>
    /// Finds the index of the nearest edge in a polygon to the given point.
    /// Polygons are closed, so the last point connects to the first.
    /// </summary>
    /// <param name="polygon">The polygon annotation.</param>
    /// <param name="point">The point to find the nearest edge to.</param>
    /// <returns>The index of the first point of the nearest edge, or -1 if not found.</returns>
    private static int FindNearestPolygonEdgeIndex(PolygonAnnotation polygon, DataPoint point)
    {
        if (polygon.Points.Count < 3) return -1;

        var minDistance = double.MaxValue;
        var nearestIndex = -1;

        for (int i = 0; i < polygon.Points.Count; i++)
        {
            var p1 = polygon.Points[i];
            var p2 = polygon.Points[(i + 1) % polygon.Points.Count]; // Wrap around to first point
            var distance = PointToSegmentDistance(point, p1, p2);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestIndex = i;
            }
        }

        return nearestIndex;
    }

    /// <summary>
    /// Calculates the distance from a point to a line segment.
    /// </summary>
    private static double PointToSegmentDistance(DataPoint point, DataPoint segStart, DataPoint segEnd)
    {
        var dx = segEnd.X - segStart.X;
        var dy = segEnd.Y - segStart.Y;
        var lengthSquared = dx * dx + dy * dy;

        if (lengthSquared == 0)
        {
            // Segment is a point
            return Math.Sqrt(Math.Pow(point.X - segStart.X, 2) + Math.Pow(point.Y - segStart.Y, 2));
        }

        // Project point onto the line segment
        var t = Math.Max(0, Math.Min(1, ((point.X - segStart.X) * dx + (point.Y - segStart.Y) * dy) / lengthSquared));

        var projX = segStart.X + t * dx;
        var projY = segStart.Y + t * dy;

        return Math.Sqrt(Math.Pow(point.X - projX, 2) + Math.Pow(point.Y - projY, 2));
    }

    #endregion
}
