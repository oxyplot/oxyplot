using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OxyPlot;
using Wpf = OxyPlot.Wpf;

namespace OxyPlotControls
{
    /// <summary>
    /// OxyPlot toolbar control providing pan, zoom, annotation, and export functionality.
    /// </summary>
    public partial class OxyPlotToolbar : UserControl
    {
        #region Construction

        /// <summary>
        /// Creates a new OxyPlot properties toolbar.
        /// </summary>
        public OxyPlotToolbar()
        {
            InitializeComponent();

            // Set up custom cursors from embedded resources
            using (var ms = new MemoryStream(Properties.Resources.SelectPointCursor))
            {
                _movePointsCursor = new Cursor(ms);
            }
            using (var ms = new MemoryStream(Properties.Resources.AddPointCursor))
            {
                _addPointCursor = new Cursor(ms);
            }
            using (var ms = new MemoryStream(Properties.Resources.Pan_Hand))
            {
                _panHandCursor = new Cursor(ms);
            }
            using (var ms = new MemoryStream(Properties.Resources.Pan_Hand_Closed))
            {
                _panHandClosedCursor = new Cursor(ms);
            }
            using (var ms = new MemoryStream(Properties.Resources.ZoomIn))
            {
                _zoomCursor = new Cursor(ms);
            }

            // Create leader line canvas and leader line
            _leaderLine.StrokeThickness = 2;
            _leaderLine.Visibility = Visibility.Collapsed;
            _leaderLine.Stroke = new SolidColorBrush(Colors.SkyBlue);
            _leaderLine.StrokeDashArray = new DoubleCollection(LineStyle.DashDashDot.GetDashArray());

            _c.Children.Add(_leaderLine);
        }

        #endregion

        #region Members

        /// <summary>
        /// Dependency property for the Plot property.
        /// </summary>
        public static readonly DependencyProperty PlotProperty = DependencyProperty.Register(
            nameof(Plot), typeof(Wpf.Plot), typeof(OxyPlotToolbar), new PropertyMetadata(null, InitializePlot));

        /// <summary>
        /// Gets and sets the OxyPlot Plot associated with this toolbar.
        /// </summary>
        public Wpf.Plot Plot
        {
            get => (Wpf.Plot)GetValue(PlotProperty);
            set => SetValue(PlotProperty, value);
        }

        /// <summary>
        /// Property changed callback for the Plot property.
        /// </summary>
        private static void InitializePlot(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            if (d.GetType() != typeof(OxyPlotToolbar)) return;

            var oxyToolBar = (OxyPlotToolbar)d;

            // Remove handlers from old plot
            if (e.OldValue != null && e.OldValue.GetType() == typeof(Wpf.Plot))
            {
                var oldPlot = (Wpf.Plot)e.OldValue;
                oldPlot.ActualModel.MouseDown -= oxyToolBar.PlotModelMouseDown;
                oldPlot.ActualModel.MouseMove -= oxyToolBar.PlotModelMouseMove;
                oldPlot.ActualModel.MouseUp -= oxyToolBar.PlotModelMouseUp;
                oldPlot.Annotations.CollectionChanged -= oxyToolBar.PlotModelAnnotationCollectionChanged;
                oldPlot.LayoutUpdated -= oxyToolBar.ToolBarLayoutUpdated;

                oldPlot.grid.Children.Remove(oxyToolBar._c);
            }

            // Add handlers to new plot
            if (e.NewValue != null && e.NewValue.GetType() == typeof(Wpf.Plot))
            {
                var newPlot = (Wpf.Plot)e.NewValue;

                // Set up the mouse events
                newPlot.ActualModel.MouseDown += oxyToolBar.PlotModelMouseDown;
                newPlot.ActualModel.MouseMove += oxyToolBar.PlotModelMouseMove;
                newPlot.ActualModel.MouseUp += oxyToolBar.PlotModelMouseUp;
                newPlot.Annotations.CollectionChanged += oxyToolBar.PlotModelAnnotationCollectionChanged;

                newPlot.ApplyTemplate(); // Needed to set the canvas

                // Define the zooming cursor
                newPlot.ZoomHorizontalCursor = oxyToolBar._zoomCursor;
                newPlot.ZoomRectangleCursor = oxyToolBar._zoomCursor;
                newPlot.ZoomVerticalCursor = oxyToolBar._zoomCursor;

                // Define the pan cursor
                newPlot.PanCursor = oxyToolBar._panHandCursor;

                // Set up the mouse bindings
                if (oxyToolBar.PointerButton.IsChecked == true) oxyToolBar.PointerButton_Click(oxyToolBar, new RoutedEventArgs());
                if (oxyToolBar.ZoomButton.IsChecked == true) oxyToolBar.ZoomButton_Click(oxyToolBar, new RoutedEventArgs());
                if (oxyToolBar.PanButton.IsChecked == true) oxyToolBar.PanButton_Click(oxyToolBar, new RoutedEventArgs());

                newPlot.LayoutUpdated += oxyToolBar.ToolBarLayoutUpdated;

                // Set up leader line for adding polyline and polygon annotations
                newPlot.grid.Children.Add(oxyToolBar._c);
            }
        }

        /// <summary>
        /// Updates the toolbar margin based on plot layout.
        /// </summary>
        private void ToolBarLayoutUpdated(object sender, EventArgs eventArgs)
        {
            var model = Plot.ActualModel;
            if (!string.IsNullOrEmpty(Plot.Title))
            {
                OxyToolBar.Margin = new Thickness(OxyToolBar.Margin.Left, model.ActualPlotMargins.Top + model.TitleArea.Bottom - model.TitlePadding, OxyToolBar.Margin.Right, OxyToolBar.Margin.Bottom);
            }
            else
            {
                OxyToolBar.Margin = new Thickness(OxyToolBar.Margin.Left, model.ActualPlotMargins.Top + model.Padding.Top, OxyToolBar.Margin.Right, OxyToolBar.Margin.Bottom);
            }
        }

        /// <summary>
        /// Dependency property for the icon size.
        /// </summary>
        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
            nameof(IconSize), typeof(double), typeof(OxyPlotToolbar), new PropertyMetadata(20.0));

        /// <summary>
        /// Gets and sets the toolbar icon size.
        /// </summary>
        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        /// <summary>
        /// Dependency property for the toolbar orientation.
        /// </summary>
        public static readonly DependencyProperty ToolBarOrientationProperty = DependencyProperty.Register(
            nameof(ToolBarOrientation), typeof(Orientation), typeof(OxyPlotToolbar), new UIPropertyMetadata(Orientation.Vertical));

        /// <summary>
        /// Gets and sets the toolbar orientation.
        /// </summary>
        public Orientation ToolBarOrientation
        {
            get => (Orientation)GetValue(ToolBarOrientationProperty);
            set => SetValue(ToolBarOrientationProperty, value);
        }

        private TextBox _textBox = null;
        private ContextMenu _contextMenu = null;

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

        // Custom Cursors
        private Cursor _movePointsCursor;
        private Cursor _addPointCursor;
        private Cursor _panHandCursor;
        private Cursor _panHandClosedCursor;
        private Cursor _zoomCursor;

        // Edit Annotation variables
        private bool _doubleClicked = false;
        private bool _showPoints = false;
        private Polyline _leaderLine = new Polyline();
        private Canvas _c = new Canvas();
        private ScreenPoint _lastScreenPoint = ScreenPoint.Undefined;
        private bool _moveStartPoint = false;
        private bool _moveEndPoint = false;
        private int _movePointIndex = -1;
        private bool _scaleMaxX = false;
        private bool _scaleMaxY = false;
        private bool _scaleMinX = false;
        private bool _scaleMinY = false;
        private Color _originalColor = Colors.White;

        // Adding Annotations
        private AddToolMode _addAnnotationToolMode = AddToolMode.None;
        private Wpf.Annotation _targetAddAnnotation = null;

        /// <summary>
        /// Delegate for the PropertiesCalled event.
        /// </summary>
        /// <param name="targetPlot">The plot whose properties need to be opened.</param>
        /// <param name="openProperties">Boolean value indicating if plot properties should be opened.</param>
        /// <param name="propertyExpander">The property expander that needs to be expanded.</param>
        /// <param name="selectedObject">The selected plot object to edit.</param>
        public delegate void PropertiesCalledEventHandler(Wpf.Plot targetPlot, bool openProperties, OxyPlotPropertiesControl.PropertyEXP? propertyExpander, object selectedObject);

        /// <summary>
        /// Event indicating the plot properties need to be opened.
        /// </summary>
        public event PropertiesCalledEventHandler PropertiesCalled;

        // Non-swappable series types
        private static readonly HashSet<Type> _nonSwapSeriesTypes = new HashSet<Type>
        {
            typeof(Wpf.HistogramSeries),
            typeof(Wpf.BarSeries),
            typeof(Wpf.ColumnSeries),
            typeof(Wpf.HeatMapSeries)
        };

        #endregion

        #region Pan & Zoom

        /// <summary>
        /// User clicked the pointer button.
        /// </summary>
        private void PointerButton_Click(object sender, RoutedEventArgs e)
        {
            if (Plot == null) return;

            var controller = Plot.ActualController;
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
            if (Plot == null) return;

            var controller = Plot.ActualController;
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
            if (Plot == null) return;

            var controller = Plot.ActualController;
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
            if (Plot == null) return;

            Plot.ResetAllAxes();
            Plot.InvalidatePlot(false);
            Plot.Focus();
        }

        /// <summary>
        /// Sets the mouse cursor based on the current tool mode.
        /// </summary>
        private void SetCursor()
        {
            if (_addAnnotationToolMode != AddToolMode.None)
            {
                Plot.DefaultPlotCursor = _addPointCursor;
                Plot.Cursor = _addPointCursor;
            }
            else if (PanButton.IsChecked == true)
            {
                Plot.PanCursor = _panHandCursor;
                Plot.DefaultPlotCursor = _panHandCursor;
                Plot.Cursor = _panHandCursor;
            }
            else if (PointerButton.IsChecked == true)
            {
                Plot.DefaultPlotCursor = Cursors.Arrow;
                Plot.Cursor = Cursors.Arrow;
            }
            else if (ZoomButton.IsChecked == true)
            {
                Plot.DefaultPlotCursor = _zoomCursor;
                Plot.Cursor = _zoomCursor;
            }
            else
            {
                Plot.DefaultPlotCursor = Cursors.Arrow;
                Plot.Cursor = Cursors.Arrow;
            }
        }

        #endregion

        #region Annotations

        /// <summary>
        /// When Add button is clicked, show context menu.
        /// </summary>
        private void AddAnnotationToggleButton_Click(object sender, RoutedEventArgs e)
        {
            AddAnnotationToggleButton.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Add arrow annotation.
        /// </summary>
        private void AddArrowAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            Plot.ActualController.UnbindAll();
            _addAnnotationToolMode = AddToolMode.AddArrowAnnotation;
            SetCursor();
        }

        /// <summary>
        /// Add text annotation.
        /// </summary>
        private void AddTextAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            Plot.ActualController.UnbindAll();
            _addAnnotationToolMode = AddToolMode.AddTextAnnotation;
            SetCursor();
        }

        /// <summary>
        /// Add vertical line annotation.
        /// </summary>
        private void AddVerticalLineAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            Plot.ActualController.UnbindAll();
            _addAnnotationToolMode = AddToolMode.AddVerticalLineAnnotation;
            SetCursor();
        }

        /// <summary>
        /// Add horizontal line annotation.
        /// </summary>
        private void AddHorizontalLineAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            Plot.ActualController.UnbindAll();
            _addAnnotationToolMode = AddToolMode.AddHorizontalLineAnnotation;
            SetCursor();
        }

        /// <summary>
        /// Add rectangle annotation.
        /// </summary>
        private void AddRectangleAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            Plot.ActualController.UnbindAll();
            _addAnnotationToolMode = AddToolMode.AddRectangleAnnotation;
            SetCursor();
        }

        /// <summary>
        /// Add ellipse annotation.
        /// </summary>
        private void AddEllipseAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            Plot.ActualController.UnbindAll();
            _addAnnotationToolMode = AddToolMode.AddEllipseAnnotation;
            SetCursor();
        }

        /// <summary>
        /// Add point annotation.
        /// </summary>
        private void AddPointAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            Plot.ActualController.UnbindAll();
            _addAnnotationToolMode = AddToolMode.AddPointAnnotation;
            SetCursor();
        }

        /// <summary>
        /// Add polygon annotation.
        /// </summary>
        private void AddPolygonAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            _addAnnotationToolMode = AddToolMode.AddPolygonAnnotation;
            _leaderLine.Visibility = Visibility.Visible;
            _leaderLine.Points.Clear();
            SetCursor();
        }

        /// <summary>
        /// Add polyline annotation.
        /// </summary>
        private void AddPolylineAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            _addAnnotationToolMode = AddToolMode.AddPolylineAnnotation;
            _leaderLine.Visibility = Visibility.Visible;
            _leaderLine.Points.Clear();
            SetCursor();
        }

        /// <summary>
        /// Stop adding the annotation.
        /// </summary>
        private void StopAddAnnotation()
        {
            if (_addAnnotationToolMode != AddToolMode.None)
            {
                if (_addAnnotationToolMode == AddToolMode.AddPolygonAnnotation || _addAnnotationToolMode == AddToolMode.AddPolylineAnnotation)
                {
                    _leaderLine.Visibility = Visibility.Collapsed;
                    _leaderLine.Points.Clear();
                    Plot.InvalidatePlot(false);
                }

                _doubleClicked = false;
                _addAnnotationToolMode = AddToolMode.None;
                _targetAddAnnotation = null;

                if (PanButton.IsChecked == true)
                {
                    PanButton_Click(null, null);
                }
                else if (PointerButton.IsChecked == true)
                {
                    PointerButton_Click(null, null);
                }
                else if (ZoomButton.IsChecked == true)
                {
                    ZoomButton_Click(null, null);
                }
                SetCursor();
            }
        }

        /// <summary>
        /// A new annotation has been added to the plot. Adds the appropriate handlers.
        /// </summary>
        private void PlotModelAnnotationCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null) return;

            foreach (var item in e.NewItems)
            {
                if (item.GetType() == typeof(Wpf.ArrowAnnotation))
                {
                    var newArrow = (Wpf.ArrowAnnotation)item;

                    newArrow.InternalAnnotation.MouseDown += (s, ae) =>
                    {
                        if (!newArrow.IsEnabled) return;
                        if (_addAnnotationToolMode != AddToolMode.None) return;
                        if (ae.ChangedButton != OxyMouseButton.Left) return;

                        _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                        _moveStartPoint = ae.HitTestResult.Index != 2;
                        _moveEndPoint = ae.HitTestResult.Index != 1;
                        _originalColor = newArrow.Color;
                        newArrow.Color = Colors.Red;

                        GetSelectedObjects(s, ae);

                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newArrow.InternalAnnotation.MouseMove += (s, ae) =>
                    {
                        if (!newArrow.IsEnabled) return;

                        double dx = ae.Position.X - _lastScreenPoint.X;
                        double dy = ae.Position.Y - _lastScreenPoint.Y;
                        var startScreenPoint = newArrow.InternalAnnotation.Transform(new DataPoint(newArrow.StartPoint.X, newArrow.StartPoint.Y));
                        var endScreenPoint = newArrow.InternalAnnotation.Transform(new DataPoint(newArrow.EndPoint.X, newArrow.EndPoint.Y));

                        var startDataPoint = newArrow.InternalAnnotation.InverseTransform(new ScreenPoint(startScreenPoint.X + dx, startScreenPoint.Y + dy));
                        var endDataPoint = newArrow.InternalAnnotation.InverseTransform(new ScreenPoint(endScreenPoint.X + dx, endScreenPoint.Y + dy));

                        if (_moveStartPoint) newArrow.StartPoint = startDataPoint;
                        if (_moveEndPoint) newArrow.EndPoint = endDataPoint;

                        _lastScreenPoint = ae.Position;
                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newArrow.InternalAnnotation.MouseUp += (s, ae) =>
                    {
                        if (!newArrow.IsEnabled) return;
                        newArrow.Color = _originalColor;
                    };
                }
                else if (item.GetType() == typeof(Wpf.TextAnnotation))
                {
                    var newText = (Wpf.TextAnnotation)item;

                    newText.InternalAnnotation.MouseDown += (s, ae) =>
                    {
                        if (!newText.IsEnabled) return;
                        if (_addAnnotationToolMode != AddToolMode.None) return;
                        if (ae.ChangedButton != OxyMouseButton.Left) return;

                        _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                        _moveStartPoint = ae.HitTestResult.Index == 0;
                        _originalColor = newText.Background;
                        newText.Background = Colors.Red;

                        GetSelectedObjects(s, ae);

                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newText.InternalAnnotation.MouseMove += (s, ae) =>
                    {
                        if (!newText.IsEnabled) return;

                        double dx = ae.Position.X - _lastScreenPoint.X;
                        double dy = ae.Position.Y - _lastScreenPoint.Y;
                        var theScreenPoint = newText.InternalAnnotation.Transform(new DataPoint(newText.TextPosition.X, newText.TextPosition.Y));
                        var theDataPoint = newText.InternalAnnotation.InverseTransform(new ScreenPoint(theScreenPoint.X + dx, theScreenPoint.Y + dy));

                        if (_moveStartPoint) newText.TextPosition = theDataPoint;

                        _lastScreenPoint = ae.Position;
                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newText.InternalAnnotation.MouseUp += (s, ae) =>
                    {
                        if (!newText.IsEnabled) return;
                        newText.Background = _originalColor;
                    };
                }
                else if (item.GetType() == typeof(Wpf.RectangleAnnotation))
                {
                    var newRect = (Wpf.RectangleAnnotation)item;

                    newRect.InternalAnnotation.MouseDown += (s, ae) =>
                    {
                        if (!newRect.IsEnabled) return;
                        if (_addAnnotationToolMode != AddToolMode.None) return;
                        if (ae.ChangedButton != OxyMouseButton.Left) return;

                        _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                        var upperRight = newRect.InternalAnnotation.Transform(newRect.MaximumX, newRect.MaximumY);
                        var lowerLeft = newRect.InternalAnnotation.Transform(newRect.MinimumX, newRect.MinimumY);
                        var topRight = new ScreenPoint(Math.Abs(upperRight.X - ae.Position.X), Math.Abs(upperRight.Y - ae.Position.Y));
                        var bottomLeft = new ScreenPoint(Math.Abs(lowerLeft.X - ae.Position.X), Math.Abs(lowerLeft.Y - ae.Position.Y));

                        _scaleMaxX = topRight.X < 10;
                        _scaleMaxY = topRight.Y < 10;
                        _scaleMinX = bottomLeft.X < 10;
                        _scaleMinY = bottomLeft.Y < 10;

                        if (ae.HitTestResult.Index == 0)
                        {
                            _moveStartPoint = !_scaleMaxX && !_scaleMaxY && !_scaleMinX && !_scaleMinY;
                        }

                        _originalColor = newRect.Fill;
                        newRect.Fill = Colors.Red;

                        GetSelectedObjects(s, ae);

                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newRect.InternalAnnotation.MouseMove += (s, ae) =>
                    {
                        if (!newRect.IsEnabled) return;

                        double dx = ae.Position.X - _lastScreenPoint.X;
                        double dy = ae.Position.Y - _lastScreenPoint.Y;
                        var upperRightScreenPoint = newRect.InternalAnnotation.Transform(newRect.MaximumX, newRect.MaximumY);
                        var lowerLeftScreenPoint = newRect.InternalAnnotation.Transform(newRect.MinimumX, newRect.MinimumY);

                        var upperRightDataPoint = newRect.InternalAnnotation.InverseTransform(new ScreenPoint(upperRightScreenPoint.X + dx, upperRightScreenPoint.Y + dy));
                        var lowerLeftDataPoint = newRect.InternalAnnotation.InverseTransform(new ScreenPoint(lowerLeftScreenPoint.X + dx, lowerLeftScreenPoint.Y + dy));

                        if (_scaleMaxX) newRect.MaximumX = upperRightDataPoint.X;
                        if (_scaleMaxY) newRect.MaximumY = upperRightDataPoint.Y;
                        if (_scaleMinX) newRect.MinimumX = lowerLeftDataPoint.X;
                        if (_scaleMinY) newRect.MinimumY = lowerLeftDataPoint.Y;

                        if (_moveStartPoint)
                        {
                            newRect.MaximumX = upperRightDataPoint.X;
                            newRect.MaximumY = upperRightDataPoint.Y;
                            newRect.MinimumX = lowerLeftDataPoint.X;
                            newRect.MinimumY = lowerLeftDataPoint.Y;
                        }

                        _lastScreenPoint = ae.Position;
                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newRect.InternalAnnotation.MouseUp += (s, ae) =>
                    {
                        if (!newRect.IsEnabled) return;
                        newRect.Fill = _originalColor;
                    };
                }
                else if (item.GetType() == typeof(Wpf.EllipseAnnotation))
                {
                    var newEllipse = (Wpf.EllipseAnnotation)item;

                    newEllipse.InternalAnnotation.MouseDown += (s, ae) =>
                    {
                        if (!newEllipse.IsEnabled) return;
                        if (_addAnnotationToolMode != AddToolMode.None) return;
                        if (ae.ChangedButton != OxyMouseButton.Left) return;

                        _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                        var upperRight = newEllipse.InternalAnnotation.Transform(newEllipse.MaximumX, newEllipse.MaximumY);
                        var lowerLeft = newEllipse.InternalAnnotation.Transform(newEllipse.MinimumX, newEllipse.MinimumY);
                        var topRight = new ScreenPoint(Math.Abs(upperRight.X - ae.Position.X), Math.Abs(upperRight.Y - ae.Position.Y));
                        var bottomLeft = new ScreenPoint(Math.Abs(lowerLeft.X - ae.Position.X), Math.Abs(lowerLeft.Y - ae.Position.Y));

                        _scaleMaxX = topRight.X < 10;
                        _scaleMaxY = topRight.Y < 10;
                        _scaleMinX = bottomLeft.X < 10;
                        _scaleMinY = bottomLeft.Y < 10;

                        if (ae.HitTestResult.Index == 0)
                        {
                            _moveStartPoint = !_scaleMaxX && !_scaleMaxY && !_scaleMinX && !_scaleMinY;
                        }

                        _originalColor = newEllipse.Fill;
                        newEllipse.Fill = Colors.Red;

                        GetSelectedObjects(s, ae);

                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newEllipse.InternalAnnotation.MouseMove += (s, ae) =>
                    {
                        if (!newEllipse.IsEnabled) return;

                        double dx = ae.Position.X - _lastScreenPoint.X;
                        double dy = ae.Position.Y - _lastScreenPoint.Y;
                        var upperRightScreenPoint = newEllipse.InternalAnnotation.Transform(newEllipse.MaximumX, newEllipse.MaximumY);
                        var lowerLeftScreenPoint = newEllipse.InternalAnnotation.Transform(newEllipse.MinimumX, newEllipse.MinimumY);

                        var upperRightDataPoint = newEllipse.InternalAnnotation.InverseTransform(new ScreenPoint(upperRightScreenPoint.X + dx, upperRightScreenPoint.Y + dy));
                        var lowerLeftDataPoint = newEllipse.InternalAnnotation.InverseTransform(new ScreenPoint(lowerLeftScreenPoint.X + dx, lowerLeftScreenPoint.Y + dy));

                        if (_scaleMaxX) newEllipse.MaximumX = upperRightDataPoint.X;
                        if (_scaleMaxY) newEllipse.MaximumY = upperRightDataPoint.Y;
                        if (_scaleMinX) newEllipse.MinimumX = lowerLeftDataPoint.X;
                        if (_scaleMinY) newEllipse.MinimumY = lowerLeftDataPoint.Y;

                        if (_moveStartPoint)
                        {
                            newEllipse.MaximumX = upperRightDataPoint.X;
                            newEllipse.MaximumY = upperRightDataPoint.Y;
                            newEllipse.MinimumX = lowerLeftDataPoint.X;
                            newEllipse.MinimumY = lowerLeftDataPoint.Y;
                        }

                        _lastScreenPoint = ae.Position;
                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newEllipse.InternalAnnotation.MouseUp += (s, ae) =>
                    {
                        if (!newEllipse.IsEnabled) return;
                        newEllipse.Fill = _originalColor;
                    };
                }
                else if (item.GetType() == typeof(Wpf.PointAnnotation))
                {
                    var newPoint = (Wpf.PointAnnotation)item;

                    newPoint.InternalAnnotation.MouseDown += (s, ae) =>
                    {
                        if (!newPoint.IsEnabled) return;
                        if (_addAnnotationToolMode != AddToolMode.None) return;
                        if (ae.ChangedButton != OxyMouseButton.Left) return;

                        _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                        _moveStartPoint = ae.HitTestResult.Index == 0;
                        _originalColor = newPoint.Fill;
                        newPoint.Fill = Colors.Red;

                        GetSelectedObjects(s, ae);

                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newPoint.InternalAnnotation.MouseMove += (s, ae) =>
                    {
                        if (!newPoint.IsEnabled) return;

                        double dx = ae.Position.X - _lastScreenPoint.X;
                        double dy = ae.Position.Y - _lastScreenPoint.Y;
                        var theScreenPoint = newPoint.InternalAnnotation.Transform(new DataPoint(newPoint.X, newPoint.Y));
                        var theDataPoint = newPoint.InternalAnnotation.InverseTransform(new ScreenPoint(theScreenPoint.X + dx, theScreenPoint.Y + dy));

                        if (_moveStartPoint)
                        {
                            newPoint.X = theDataPoint.X;
                            newPoint.Y = theDataPoint.Y;
                        }

                        _lastScreenPoint = ae.Position;
                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newPoint.InternalAnnotation.MouseUp += (s, ae) =>
                    {
                        if (!newPoint.IsEnabled) return;
                        newPoint.Fill = _originalColor;
                    };
                }
                else if (item.GetType() == typeof(Wpf.PolygonAnnotation))
                {
                    var newPolygon = (Wpf.PolygonAnnotation)item;

                    newPolygon.InternalAnnotation.MouseDown += (s, ae) =>
                    {
                        if (!newPolygon.IsEnabled) return;
                        if (_addAnnotationToolMode != AddToolMode.None) return;
                        if (ae.ChangedButton != OxyMouseButton.Left) return;

                        _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                        var screenToData = newPolygon.InternalAnnotation.InverseTransform(ae.Position);
                        var screen2ToData = newPolygon.InternalAnnotation.InverseTransform(new ScreenPoint(ae.Position.X - 10, ae.Position.Y - 10));
                        var dxy = new DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y));
                        var dPoint = newPolygon.InternalAnnotation.InverseTransform(ae.Position);

                        _movePointIndex = -1;
                        for (int i = 0; i < newPolygon.Points.Count; i++)
                        {
                            if (Math.Abs(dPoint.X - newPolygon.Points[i].X) < dxy.X && Math.Abs(dPoint.Y - newPolygon.Points[i].Y) < dxy.Y)
                            {
                                _movePointIndex = i;
                                break;
                            }
                        }

                        if (_movePointIndex == -1)
                        {
                            bool onLine = false;
                            for (int i = 0; i < newPolygon.Points.Count - 1; i++)
                            {
                                var p1 = newPolygon.InternalAnnotation.Transform(newPolygon.Points[i]);
                                var p2 = newPolygon.InternalAnnotation.Transform(newPolygon.Points[i + 1]);
                                var linePoint = ScreenPointHelper.FindPointOnLine(ae.Position, p1, p2);
                                if ((linePoint - ae.Position).Length < 10)
                                {
                                    newPolygon.Points.Insert(i + 1, newPolygon.InternalAnnotation.InverseTransform(linePoint));
                                    onLine = true;
                                    _movePointIndex = i + 1;
                                    break;
                                }
                            }

                            if (!onLine)
                            {
                                var p1 = newPolygon.InternalAnnotation.Transform(newPolygon.Points[0]);
                                var p2 = newPolygon.InternalAnnotation.Transform(newPolygon.Points[newPolygon.Points.Count - 1]);
                                var linePoint = ScreenPointHelper.FindPointOnLine(ae.Position, p1, p2);
                                if ((linePoint - ae.Position).Length < 10)
                                {
                                    newPolygon.Points.Add(newPolygon.InternalAnnotation.InverseTransform(linePoint));
                                    _movePointIndex = newPolygon.Points.Count - 1;
                                }
                                else
                                {
                                    if (ae.HitTestResult.Index == 0) _moveStartPoint = true;
                                }
                            }
                        }

                        _originalColor = newPolygon.Fill;
                        newPolygon.Fill = Colors.Red;

                        GetSelectedObjects(s, ae);

                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newPolygon.InternalAnnotation.MouseMove += (s, ae) =>
                    {
                        if (!newPolygon.IsEnabled) return;

                        double dx = ae.Position.X - _lastScreenPoint.X;
                        double dy = ae.Position.Y - _lastScreenPoint.Y;

                        if (_movePointIndex > -1)
                        {
                            var screenPoint = newPolygon.InternalAnnotation.Transform(new DataPoint(newPolygon.Points[_movePointIndex].X, newPolygon.Points[_movePointIndex].Y));
                            newPolygon.Points[_movePointIndex] = newPolygon.InternalAnnotation.InverseTransform(new ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy));
                        }
                        else if (_moveStartPoint)
                        {
                            for (int i = 0; i < newPolygon.Points.Count; i++)
                            {
                                var screenPoint = newPolygon.InternalAnnotation.Transform(new DataPoint(newPolygon.Points[i].X, newPolygon.Points[i].Y));
                                newPolygon.Points[i] = newPolygon.InternalAnnotation.InverseTransform(new ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy));
                            }
                        }

                        _lastScreenPoint = ae.Position;
                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newPolygon.InternalAnnotation.MouseUp += (s, ae) =>
                    {
                        if (!newPolygon.IsEnabled) return;
                        newPolygon.Fill = _originalColor;
                    };
                }
                else if (item.GetType() == typeof(Wpf.PolylineAnnotation))
                {
                    var newPolyline = (Wpf.PolylineAnnotation)item;

                    newPolyline.InternalAnnotation.MouseDown += (s, ae) =>
                    {
                        if (!newPolyline.IsEnabled) return;
                        if (_addAnnotationToolMode != AddToolMode.None) return;
                        if (ae.ChangedButton != OxyMouseButton.Left) return;

                        _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                        var screenToData = newPolyline.InternalAnnotation.InverseTransform(ae.Position);
                        var screen2ToData = newPolyline.InternalAnnotation.InverseTransform(new ScreenPoint(ae.Position.X - 10, ae.Position.Y - 10));
                        var dxy = new DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y));
                        var dPoint = newPolyline.InternalAnnotation.InverseTransform(ae.Position);

                        _movePointIndex = -1;
                        for (int i = 0; i < newPolyline.Points.Count; i++)
                        {
                            if (Math.Abs(dPoint.X - newPolyline.Points[i].X) < dxy.X && Math.Abs(dPoint.Y - newPolyline.Points[i].Y) < dxy.Y)
                            {
                                _movePointIndex = i;
                                break;
                            }
                        }

                        bool onLine = false;
                        if (_movePointIndex == -1 && ae.IsControlDown)
                        {
                            for (int i = 0; i < newPolyline.Points.Count - 1; i++)
                            {
                                var p1 = newPolyline.InternalAnnotation.Transform(newPolyline.Points[i]);
                                var p2 = newPolyline.InternalAnnotation.Transform(newPolyline.Points[i + 1]);
                                var linePoint = ScreenPointHelper.FindPointOnLine(ae.Position, p1, p2);
                                if ((linePoint - ae.Position).Length < 10)
                                {
                                    newPolyline.Points.Insert(i + 1, newPolyline.InternalAnnotation.InverseTransform(linePoint));
                                    onLine = true;
                                    _movePointIndex = i + 1;
                                    break;
                                }
                            }
                        }

                        if (!onLine) _moveStartPoint = true;

                        _originalColor = newPolyline.Color;
                        newPolyline.Color = Colors.Red;

                        GetSelectedObjects(s, ae);

                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newPolyline.InternalAnnotation.MouseMove += (s, ae) =>
                    {
                        if (!newPolyline.IsEnabled) return;

                        double dx = ae.Position.X - _lastScreenPoint.X;
                        double dy = ae.Position.Y - _lastScreenPoint.Y;

                        if (_movePointIndex > -1)
                        {
                            var screenPoint = newPolyline.InternalAnnotation.Transform(new DataPoint(newPolyline.Points[_movePointIndex].X, newPolyline.Points[_movePointIndex].Y));
                            newPolyline.Points[_movePointIndex] = newPolyline.InternalAnnotation.InverseTransform(new ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy));
                        }
                        else if (_moveStartPoint)
                        {
                            for (int i = 0; i < newPolyline.Points.Count; i++)
                            {
                                var screenPoint = newPolyline.InternalAnnotation.Transform(new DataPoint(newPolyline.Points[i].X, newPolyline.Points[i].Y));
                                newPolyline.Points[i] = newPolyline.InternalAnnotation.InverseTransform(new ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy));
                            }
                        }

                        _lastScreenPoint = ae.Position;
                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newPolyline.InternalAnnotation.MouseUp += (s, ae) =>
                    {
                        if (!newPolyline.IsEnabled) return;
                        newPolyline.Color = _originalColor;
                    };
                }
                else if (item.GetType() == typeof(Wpf.LineAnnotation))
                {
                    var newLine = (Wpf.LineAnnotation)item;

                    newLine.InternalAnnotation.MouseDown += (s, ae) =>
                    {
                        if (!newLine.IsEnabled) return;
                        if (_addAnnotationToolMode != AddToolMode.None) return;
                        if (ae.ChangedButton != OxyMouseButton.Left) return;

                        _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                        _moveStartPoint = ae.HitTestResult.Index == 0;

                        _originalColor = newLine.Color;
                        newLine.Color = Colors.Red;

                        GetSelectedObjects(s, ae);

                        if ((Mouse.LeftButton == MouseButtonState.Pressed && PanButton.IsChecked == true) || Mouse.MiddleButton == MouseButtonState.Pressed)
                        {
                            Plot.PanCursor = _panHandClosedCursor;
                            Plot.DefaultPlotCursor = _panHandClosedCursor;
                            Plot.Cursor = _panHandClosedCursor;
                        }

                        OpenLineAnnotationTooltip(newLine);
                        UpdateLineAnnotationTooltip(newLine);
                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newLine.InternalAnnotation.MouseMove += (s, ae) =>
                    {
                        if (!newLine.IsEnabled) return;

                        double dx = ae.Position.X - _lastScreenPoint.X;
                        double dy = ae.Position.Y - _lastScreenPoint.Y;
                        var screenPoint = newLine.InternalAnnotation.Transform(new DataPoint(newLine.X, newLine.Y));
                        var dataPoint = newLine.InternalAnnotation.InverseTransform(new ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy));

                        if (_moveStartPoint)
                        {
                            if (newLine.Type == OxyPlot.Annotations.LineAnnotationType.LinearEquation)
                            {
                                dx = dataPoint.X - newLine.X;
                                dy = dataPoint.Y - newLine.Y;
                                newLine.Intercept += (dy - newLine.Slope * dx);
                            }
                            else
                            {
                                newLine.X = dataPoint.X;
                                newLine.Y = dataPoint.Y;
                            }
                            UpdateLineAnnotationTooltip(newLine);
                        }

                        _lastScreenPoint = ae.Position;
                        Plot.ActualModel.InvalidatePlot(false);
                        ae.Handled = true;
                    };

                    newLine.InternalAnnotation.MouseUp += (s, ae) =>
                    {
                        if (!newLine.IsEnabled) return;
                        newLine.Color = _originalColor;
                        CloseLineAnnotationTooltip(newLine);
                    };
                }
            }
        }

        /// <summary>
        /// Open the line annotation tooltip.
        /// </summary>
        private void OpenLineAnnotationTooltip(Wpf.LineAnnotation lineAnnotation)
        {
            if (lineAnnotation.ToolTip != null)
            {
                ((ToolTip)lineAnnotation.ToolTip).IsOpen = false;
            }

            var toolTip = new ToolTip
            {
                FontFamily = Plot.FontFamily,
                FontSize = Plot.FontSize,
                FontWeight = Plot.FontWeight,
                Background = Brushes.White,
                BorderBrush = Brushes.Transparent,
                Placement = PlacementMode.Relative,
                PlacementTarget = Plot.canvas,
                Padding = new Thickness(1),
                Margin = new Thickness(0),
                IsOpen = true
            };
            lineAnnotation.ToolTip = toolTip;
        }

        /// <summary>
        /// Update the line annotation tooltip.
        /// </summary>
        private void UpdateLineAnnotationTooltip(Wpf.LineAnnotation lineAnnotation)
        {
            switch (lineAnnotation.Type)
            {
                case OxyPlot.Annotations.LineAnnotationType.Horizontal:
                    if (lineAnnotation.ToolTip != null)
                    {
                        DataPoint dataPoint;
                        if (!lineAnnotation.InternalAnnotation.XAxis.IsReversed)
                        {
                            dataPoint = new DataPoint(lineAnnotation.InternalAnnotation.XAxis.ActualMinimum, lineAnnotation.Y);
                        }
                        else
                        {
                            dataPoint = new DataPoint(lineAnnotation.InternalAnnotation.XAxis.ActualMaximum, lineAnnotation.Y);
                        }
                        var toolTip = (ToolTip)lineAnnotation.ToolTip;
                        toolTip.Content = lineAnnotation.InternalAnnotation.YAxis.FormatValue(lineAnnotation.Y);
                        toolTip.UpdateLayout();
                        toolTip.VerticalOffset = lineAnnotation.InternalAnnotation.Transform(dataPoint).Y - toolTip.ActualHeight / 2;
                        toolTip.HorizontalOffset = lineAnnotation.InternalAnnotation.Transform(dataPoint).X - toolTip.ActualWidth;
                    }
                    break;

                case OxyPlot.Annotations.LineAnnotationType.Vertical:
                    if (lineAnnotation.ToolTip != null)
                    {
                        DataPoint dataPoint;
                        if (!lineAnnotation.InternalAnnotation.YAxis.IsReversed)
                        {
                            dataPoint = new DataPoint(lineAnnotation.X, lineAnnotation.InternalAnnotation.YAxis.ActualMinimum);
                        }
                        else
                        {
                            dataPoint = new DataPoint(lineAnnotation.X, lineAnnotation.InternalAnnotation.YAxis.ActualMaximum);
                        }
                        var toolTip = (ToolTip)lineAnnotation.ToolTip;
                        toolTip.Content = lineAnnotation.InternalAnnotation.XAxis.FormatValue(lineAnnotation.X);
                        toolTip.UpdateLayout();
                        toolTip.VerticalOffset = lineAnnotation.InternalAnnotation.Transform(dataPoint).Y;
                        toolTip.HorizontalOffset = lineAnnotation.InternalAnnotation.Transform(dataPoint).X - toolTip.ActualWidth / 2;
                    }
                    break;
            }
        }

        /// <summary>
        /// Close the line annotation tooltip.
        /// </summary>
        private void CloseLineAnnotationTooltip(Wpf.LineAnnotation lineAnnotation)
        {
            if (lineAnnotation.ToolTip != null)
            {
                ((ToolTip)lineAnnotation.ToolTip).IsOpen = false;
            }
        }

        #endregion

        #region Mouse Events

        /// <summary>
        /// Plot model mouse down.
        /// </summary>
        private void PlotModelMouseDown(object sender, OxyMouseDownEventArgs e)
        {
            if (_contextMenu != null)
            {
                _contextMenu.IsOpen = false;
            }
            if (_textBox != null)
            {
                System.Windows.Input.Keyboard.ClearFocus();
            }

            _moveStartPoint = false;
            _moveEndPoint = false;
            _movePointIndex = -1;
            _scaleMaxX = false;
            _scaleMaxY = false;
            _scaleMinX = false;
            _scaleMinY = false;

            if (e.ClickCount == 2)
            {
                _doubleClicked = true;
            }

            if ((Mouse.LeftButton == MouseButtonState.Pressed && PanButton.IsChecked == true) || Mouse.MiddleButton == MouseButtonState.Pressed)
            {
                Plot.PanCursor = _panHandClosedCursor;
                Plot.DefaultPlotCursor = _panHandClosedCursor;
                Plot.Cursor = _panHandClosedCursor;
            }

            if (_addAnnotationToolMode != AddToolMode.None)
            {
                switch (_addAnnotationToolMode)
                {
                    case AddToolMode.AddArrowAnnotation:
                        var newArrow = new Wpf.ArrowAnnotation { Text = "Arrow Annotation" };
                        Plot.Annotations.Add(newArrow);
                        Plot.ActualModel.InvalidatePlot(false);
                        PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newArrow);
                        newArrow.StartPoint = newArrow.InternalAnnotation.InverseTransform(e.Position);
                        newArrow.EndPoint = newArrow.StartPoint;
                        _targetAddAnnotation = newArrow;
                        break;

                    case AddToolMode.AddTextAnnotation:
                        var newText = new Wpf.TextAnnotation { Text = "Text Annotation" };
                        Plot.Annotations.Add(newText);
                        Plot.ActualModel.InvalidatePlot(false);
                        PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newText);
                        newText.TextPosition = newText.InternalAnnotation.InverseTransform(e.Position);
                        _targetAddAnnotation = newText;
                        break;

                    case AddToolMode.AddVerticalLineAnnotation:
                        var newVLine = new Wpf.LineAnnotation { Text = "Vertical Line Annotation" };
                        Plot.Annotations.Add(newVLine);
                        Plot.ActualModel.InvalidatePlot(false);
                        PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newVLine);

                        if (newVLine.InternalAnnotation.YAxis.IsReversed == false)
                        {
                            newVLine.TextLinePosition = 1;
                            newVLine.TextHorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                        }
                        else
                        {
                            newVLine.TextLinePosition = 0;
                            newVLine.TextHorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        }

                        {
                            OxyRect plotArea = Plot.ActualModel.PlotArea;
                            var plotLL = newVLine.InternalAnnotation.InverseTransform(new ScreenPoint(plotArea.Left, plotArea.Bottom));
                            var plotUR = newVLine.InternalAnnotation.InverseTransform(new ScreenPoint(plotArea.Right, plotArea.Top));
                            var dataPointClicked = newVLine.InternalAnnotation.InverseTransform(e.Position);
                            newVLine.X = dataPointClicked.X;
                            newVLine.Y = dataPointClicked.Y;
                            newVLine.Type = OxyPlot.Annotations.LineAnnotationType.Vertical;
                            newVLine.Intercept = dataPointClicked.Y;
                            newVLine.Slope = (plotUR.Y - plotLL.Y) / (plotUR.X - plotLL.X);
                            _targetAddAnnotation = newVLine;

                            OpenLineAnnotationTooltip(newVLine);
                            UpdateLineAnnotationTooltip(newVLine);
                            Plot.ActualModel.InvalidatePlot(false);
                        }
                        break;

                    case AddToolMode.AddHorizontalLineAnnotation:
                        var newHLine = new Wpf.LineAnnotation { Text = "Horizontal Line Annotation" };
                        Plot.Annotations.Add(newHLine);
                        Plot.ActualModel.InvalidatePlot(false);
                        PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newHLine);

                        if (newHLine.InternalAnnotation.XAxis.IsReversed == false)
                        {
                            newHLine.TextLinePosition = 0;
                            newHLine.TextHorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        }
                        else
                        {
                            newHLine.TextLinePosition = 1;
                            newHLine.TextHorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        }

                        {
                            OxyRect plotArea = Plot.ActualModel.PlotArea;
                            var plotLL = newHLine.InternalAnnotation.InverseTransform(new ScreenPoint(plotArea.Left, plotArea.Bottom));
                            var plotUR = newHLine.InternalAnnotation.InverseTransform(new ScreenPoint(plotArea.Right, plotArea.Top));
                            var dataPointClicked = newHLine.InternalAnnotation.InverseTransform(e.Position);
                            newHLine.X = dataPointClicked.X;
                            newHLine.Y = dataPointClicked.Y;
                            newHLine.Type = OxyPlot.Annotations.LineAnnotationType.Horizontal;
                            newHLine.Intercept = dataPointClicked.Y;
                            newHLine.Slope = (plotUR.Y - plotLL.Y) / (plotUR.X - plotLL.X);
                            _targetAddAnnotation = newHLine;

                            OpenLineAnnotationTooltip(newHLine);
                            UpdateLineAnnotationTooltip(newHLine);
                            Plot.ActualModel.InvalidatePlot(false);
                        }
                        break;

                    case AddToolMode.AddRectangleAnnotation:
                        var newRectangle = new Wpf.RectangleAnnotation { Text = "Rectangle Annotation" };
                        Plot.Annotations.Add(newRectangle);
                        Plot.ActualModel.InvalidatePlot(false);
                        PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newRectangle);
                        {
                            var dataPointClicked = newRectangle.InternalAnnotation.InverseTransform(e.Position);
                            newRectangle.MinimumX = dataPointClicked.X;
                            newRectangle.MaximumX = dataPointClicked.X;
                            newRectangle.MinimumY = dataPointClicked.Y;
                            newRectangle.MaximumY = dataPointClicked.Y;
                            _targetAddAnnotation = newRectangle;
                        }
                        break;

                    case AddToolMode.AddEllipseAnnotation:
                        var newEllipse = new Wpf.EllipseAnnotation { Text = "Ellipse Annotation" };
                        Plot.Annotations.Add(newEllipse);
                        Plot.ActualModel.InvalidatePlot(false);
                        PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newEllipse);
                        {
                            var dataPointClicked = newEllipse.InternalAnnotation.InverseTransform(e.Position);
                            newEllipse.MinimumX = dataPointClicked.X;
                            newEllipse.MaximumX = dataPointClicked.X;
                            newEllipse.MinimumY = dataPointClicked.Y;
                            newEllipse.MaximumY = dataPointClicked.Y;
                            _targetAddAnnotation = newEllipse;
                        }
                        break;

                    case AddToolMode.AddPointAnnotation:
                        var newPoint = new Wpf.PointAnnotation { Text = "Point Annotation", Size = 5 };
                        Plot.Annotations.Add(newPoint);
                        Plot.ActualModel.InvalidatePlot(false);
                        PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newPoint);
                        {
                            var dataPointClicked = newPoint.InternalAnnotation.InverseTransform(e.Position);
                            OxyRect plotArea = Plot.ActualModel.PlotArea;
                            var plotLL = newPoint.InternalAnnotation.InverseTransform(new ScreenPoint(plotArea.Left, plotArea.Bottom));
                            var plotUR = newPoint.InternalAnnotation.InverseTransform(new ScreenPoint(plotArea.Right, plotArea.Top));
                            var plotCenter = newPoint.InternalAnnotation.InverseTransform(Plot.ActualModel.PlotArea.Center);

                            newPoint.X = dataPointClicked.X;
                            newPoint.Y = dataPointClicked.Y;
                            _targetAddAnnotation = newPoint;
                        }
                        break;

                    case AddToolMode.AddPolygonAnnotation:
                        if (_targetAddAnnotation == null)
                        {
                            var newPolygon = new Wpf.PolygonAnnotation { Text = "Polygon Annotation" };
                            newPolygon.Points = new System.Collections.Generic.List<DataPoint>();
                            var dataPointClicked = ConvertScreenPointToDataPoint(e.Position);
                            newPolygon.Points.Add(dataPointClicked);
                            _leaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
                            _leaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
                            _targetAddAnnotation = newPolygon;
                        }
                        else
                        {
                            _doubleClicked = e.ClickCount > 1;
                            var polyAnnotation = (Wpf.PolygonAnnotation)_targetAddAnnotation;
                            if (polyAnnotation.Points.Count == 3)
                            {
                                Plot.Annotations.Add(polyAnnotation);
                                PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, polyAnnotation);
                            }
                            if (e.ClickCount < 2)
                            {
                                _leaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
                                polyAnnotation.Points.Add(ConvertScreenPointToDataPoint(e.Position));
                            }
                        }
                        break;

                    case AddToolMode.AddPolylineAnnotation:
                        if (_targetAddAnnotation == null)
                        {
                            var newPolyline = new Wpf.PolylineAnnotation { Text = "Polyline Annotation" };
                            newPolyline.Points = new System.Collections.Generic.List<DataPoint>();
                            Plot.Annotations.Add(newPolyline);
                            PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newPolyline);
                            var dataPointClicked = ConvertScreenPointToDataPoint(e.Position);
                            newPolyline.Points.Add(dataPointClicked);
                            _leaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
                            _leaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
                            _targetAddAnnotation = newPolyline;
                        }
                        else
                        {
                            _doubleClicked = e.ClickCount > 1;
                            if (e.ClickCount < 2)
                            {
                                ((Wpf.PolylineAnnotation)_targetAddAnnotation).Points.Add(_targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position));
                            }
                        }
                        break;
                }
                return;
            }

            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                Plot.DefaultPlotCursor = Cursors.Arrow;
                Plot.Cursor = Cursors.Arrow;
                GetSelectedObjects(sender, e);
                return;
            }
            else
            {
                GetSelectedObjects(sender, e);
            }

            if (PanButton.IsChecked == true || Mouse.MiddleButton == MouseButtonState.Pressed)
            {
                Plot.PanCursor = _panHandClosedCursor;
                Plot.DefaultPlotCursor = _panHandClosedCursor;
                Plot.Cursor = _panHandClosedCursor;
            }
        }

        /// <summary>
        /// Plot model mouse move.
        /// </summary>
        private void PlotModelMouseMove(object sender, OxyMouseEventArgs e)
        {
            // For adding annotations
            if (_addAnnotationToolMode != AddToolMode.None && _targetAddAnnotation != null)
            {
                switch (_addAnnotationToolMode)
                {
                    case AddToolMode.AddArrowAnnotation:
                        ((Wpf.ArrowAnnotation)_targetAddAnnotation).EndPoint = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position);
                        break;

                    case AddToolMode.AddTextAnnotation:
                        ((Wpf.TextAnnotation)_targetAddAnnotation).TextPosition = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position);
                        break;

                    case AddToolMode.AddVerticalLineAnnotation:
                        ((Wpf.LineAnnotation)_targetAddAnnotation).X = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position).X;
                        UpdateLineAnnotationTooltip((Wpf.LineAnnotation)_targetAddAnnotation);
                        break;

                    case AddToolMode.AddHorizontalLineAnnotation:
                        ((Wpf.LineAnnotation)_targetAddAnnotation).Y = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position).Y;
                        UpdateLineAnnotationTooltip((Wpf.LineAnnotation)_targetAddAnnotation);
                        break;

                    case AddToolMode.AddRectangleAnnotation:
                        {
                            var mouseDataPoint = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position);
                            var rect = (Wpf.RectangleAnnotation)_targetAddAnnotation;
                            rect.MaximumX = mouseDataPoint.X;
                            rect.MaximumY = mouseDataPoint.Y;
                        }
                        break;

                    case AddToolMode.AddEllipseAnnotation:
                        {
                            var mouseDataPoint = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position);
                            var ellipse = (Wpf.EllipseAnnotation)_targetAddAnnotation;
                            ellipse.MaximumX = mouseDataPoint.X;
                            ellipse.MaximumY = mouseDataPoint.Y;
                        }
                        break;

                    case AddToolMode.AddPointAnnotation:
                        {
                            var mouseDataPoint = _targetAddAnnotation.InternalAnnotation.InverseTransform(e.Position);
                            var point = (Wpf.PointAnnotation)_targetAddAnnotation;
                            point.X = mouseDataPoint.X;
                            point.Y = mouseDataPoint.Y;
                        }
                        break;

                    case AddToolMode.AddPolygonAnnotation:
                        _leaderLine.Points[_leaderLine.Points.Count - 1] = new Point(e.Position.X, e.Position.Y);
                        Plot.InvalidatePlot(true);
                        break;

                    case AddToolMode.AddPolylineAnnotation:
                        {
                            var polyAnnotation = (Wpf.PolylineAnnotation)_targetAddAnnotation;
                            _leaderLine.Points[0] = ConvertDataPointToPoint(polyAnnotation.Points[polyAnnotation.Points.Count - 1]);
                            _leaderLine.Points[_leaderLine.Points.Count - 1] = new Point(e.Position.X, e.Position.Y);
                            Plot.InvalidatePlot(false);
                        }
                        break;
                }
                return;
            }

            // Cursor update logic - show markers and update cursor when hovering over annotations
            bool requiresRedraw = _showPoints;
            _showPoints = false;
            var markerPoints = new List<ScreenPoint>();
            var markerSizes = new List<double>();
            Cursor updatedCursor = null;

            foreach (var a in Plot.Annotations)
            {
                if (!a.IsEnabled) continue;
                var ht = a.InternalAnnotation.HitTest(new HitTestArguments(e.Position, 10));
                if (ht == null) continue;

                var aType = a.GetType();
                if (aType == typeof(Wpf.ArrowAnnotation))
                {
                    var arrowAnnotation = (Wpf.ArrowAnnotation)a;
                    markerPoints.Add(arrowAnnotation.InternalAnnotation.Transform(arrowAnnotation.StartPoint));
                    markerPoints.Add(arrowAnnotation.InternalAnnotation.Transform(arrowAnnotation.EndPoint));
                    markerSizes.AddRange(new[] { 2.2, 2.2 });

                    switch (ht.Index)
                    {
                        case 0:
                            updatedCursor = Cursors.SizeAll;
                            break;
                        case 1:
                        case 2:
                            updatedCursor = _movePointsCursor;
                            break;
                    }
                }
                else if (aType == typeof(Wpf.TextAnnotation))
                {
                    if (ht.Index == 0) updatedCursor = Cursors.SizeAll;
                }
                else if (aType == typeof(Wpf.RectangleAnnotation))
                {
                    var rAnnotation = (Wpf.RectangleAnnotation)a;
                    var ur = rAnnotation.InternalAnnotation.Transform(Math.Max(rAnnotation.MaximumX, rAnnotation.MinimumX), Math.Max(rAnnotation.MaximumY, rAnnotation.MinimumY));
                    var ll = rAnnotation.InternalAnnotation.Transform(Math.Min(rAnnotation.MinimumX, rAnnotation.MaximumX), Math.Min(rAnnotation.MinimumY, rAnnotation.MaximumY));

                    markerPoints.Add(ur);
                    markerPoints.Add(ll);
                    markerPoints.Add(new ScreenPoint(ll.X, ur.Y));
                    markerPoints.Add(new ScreenPoint(ur.X, ll.Y));
                    markerPoints.Add(new ScreenPoint(ll.X, ll.Y + (ur.Y - ll.Y) / 2));
                    markerPoints.Add(new ScreenPoint(ur.X, ll.Y + (ur.Y - ll.Y) / 2));
                    markerPoints.Add(new ScreenPoint(ll.X + (ur.X - ll.X) / 2, ll.Y));
                    markerPoints.Add(new ScreenPoint(ll.X + (ur.X - ll.X) / 2, ur.Y));
                    markerSizes.AddRange(new[] { 2.0, 2.0, 2.0, 2.0, 2.0, 2.0, 2.0, 2.0 });

                    var topRight = new ScreenPoint(Math.Abs(ur.X - e.Position.X), Math.Abs(ur.Y - e.Position.Y));
                    var bottomLeft = new ScreenPoint(Math.Abs(ll.X - e.Position.X), Math.Abs(ll.Y - e.Position.Y));

                    // Corners
                    if (topRight.X < 10 && topRight.Y < 10) { updatedCursor = Cursors.SizeNESW; continue; }
                    if (bottomLeft.X < 10 && bottomLeft.Y < 10) { updatedCursor = Cursors.SizeNESW; continue; }
                    if (bottomLeft.X < 10 && topRight.Y < 10) { updatedCursor = Cursors.SizeNWSE; continue; }
                    if (topRight.X < 10 && bottomLeft.Y < 10) { updatedCursor = Cursors.SizeNWSE; continue; }
                    // Edges
                    if (topRight.X < 10 || bottomLeft.X < 10) { updatedCursor = Cursors.SizeWE; continue; }
                    if (topRight.Y < 10 || bottomLeft.Y < 10) { updatedCursor = Cursors.SizeNS; continue; }
                    // All
                    if (ht.Index == 0) updatedCursor = Cursors.SizeAll;
                }
                else if (aType == typeof(Wpf.EllipseAnnotation))
                {
                    var eAnnotation = (Wpf.EllipseAnnotation)a;
                    var ur = eAnnotation.InternalAnnotation.Transform(Math.Max(eAnnotation.MaximumX, eAnnotation.MinimumX), Math.Max(eAnnotation.MaximumY, eAnnotation.MinimumY));
                    var ll = eAnnotation.InternalAnnotation.Transform(Math.Min(eAnnotation.MinimumX, eAnnotation.MaximumX), Math.Min(eAnnotation.MinimumY, eAnnotation.MaximumY));

                    markerPoints.Add(ur);
                    markerPoints.Add(ll);
                    markerPoints.Add(new ScreenPoint(ll.X, ur.Y));
                    markerPoints.Add(new ScreenPoint(ur.X, ll.Y));
                    markerPoints.Add(new ScreenPoint(ll.X, ll.Y + (ur.Y - ll.Y) / 2));
                    markerPoints.Add(new ScreenPoint(ur.X, ll.Y + (ur.Y - ll.Y) / 2));
                    markerPoints.Add(new ScreenPoint(ll.X + (ur.X - ll.X) / 2, ll.Y));
                    markerPoints.Add(new ScreenPoint(ll.X + (ur.X - ll.X) / 2, ur.Y));
                    markerSizes.AddRange(new[] { 2.0, 2.0, 2.0, 2.0, 2.0, 2.0, 2.0, 2.0 });

                    var topRight = new ScreenPoint(Math.Abs(ur.X - e.Position.X), Math.Abs(ur.Y - e.Position.Y));
                    var bottomLeft = new ScreenPoint(Math.Abs(ll.X - e.Position.X), Math.Abs(ll.Y - e.Position.Y));

                    // Corners
                    if (topRight.X < 10 && topRight.Y < 10) { updatedCursor = Cursors.SizeNESW; continue; }
                    if (bottomLeft.X < 10 && bottomLeft.Y < 10) { updatedCursor = Cursors.SizeNESW; continue; }
                    if (bottomLeft.X < 10 && topRight.Y < 10) { updatedCursor = Cursors.SizeNWSE; continue; }
                    if (topRight.X < 10 && bottomLeft.Y < 10) { updatedCursor = Cursors.SizeNWSE; continue; }
                    // Edges
                    if (topRight.X < 10 || bottomLeft.X < 10) { updatedCursor = Cursors.SizeWE; continue; }
                    if (topRight.Y < 10 || bottomLeft.Y < 10) { updatedCursor = Cursors.SizeNS; continue; }
                    // All
                    if (ht.Index == 0) updatedCursor = Cursors.SizeAll;
                }
                else if (aType == typeof(Wpf.PointAnnotation))
                {
                    if (ht.Index == 0) updatedCursor = Cursors.SizeAll;
                }
                else if (aType == typeof(Wpf.PolygonAnnotation))
                {
                    if (ht.Index == 0)
                    {
                        var polyAnnotation = (Wpf.PolygonAnnotation)a;
                        var screenToData = polyAnnotation.InternalAnnotation.InverseTransform(e.Position);
                        var screen2ToData = polyAnnotation.InternalAnnotation.InverseTransform(new ScreenPoint(e.Position.X - 10, e.Position.Y - 10));
                        var dxy = new DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y));

                        foreach (var p in polyAnnotation.Points)
                        {
                            markerPoints.Add(polyAnnotation.InternalAnnotation.Transform(p));
                            markerSizes.Add(2);
                        }

                        var dPoint = polyAnnotation.InternalAnnotation.InverseTransform(e.Position);
                        // Check if cursor is over any points
                        if (polyAnnotation.Points.Any(o => Math.Abs(dPoint.X - o.X) < dxy.X && Math.Abs(dPoint.Y - o.Y) < dxy.Y))
                        {
                            updatedCursor = _movePointsCursor;
                        }
                        else
                        {
                            bool onLine = false;
                            for (int i = 0; i < polyAnnotation.Points.Count - 1; i++)
                            {
                                var p1 = polyAnnotation.InternalAnnotation.Transform(polyAnnotation.Points[i]);
                                var p2 = polyAnnotation.InternalAnnotation.Transform(polyAnnotation.Points[i + 1]);
                                var linePoint = ScreenPointHelper.FindPointOnLine(e.Position, p1, p2);
                                if ((linePoint - e.Position).Length < 10)
                                {
                                    onLine = true;
                                    updatedCursor = _addPointCursor;
                                    break;
                                }
                            }

                            if (!onLine)
                            {
                                var p1 = polyAnnotation.InternalAnnotation.Transform(polyAnnotation.Points[0]);
                                var p2 = polyAnnotation.InternalAnnotation.Transform(polyAnnotation.Points[polyAnnotation.Points.Count - 1]);
                                var linePoint = ScreenPointHelper.FindPointOnLine(e.Position, p1, p2);
                                if ((linePoint - e.Position).Length < 10)
                                {
                                    updatedCursor = _addPointCursor;
                                }
                                else
                                {
                                    updatedCursor = Cursors.SizeAll;
                                }
                            }
                        }
                    }
                }
                else if (aType == typeof(Wpf.PolylineAnnotation))
                {
                    if (ht.Index == 0)
                    {
                        var polylineAnnotation = (Wpf.PolylineAnnotation)a;
                        var screenToData = polylineAnnotation.InternalAnnotation.InverseTransform(e.Position);
                        var screen2ToData = polylineAnnotation.InternalAnnotation.InverseTransform(new ScreenPoint(e.Position.X - 10, e.Position.Y - 10));
                        var dxy = new DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y));

                        foreach (var p in polylineAnnotation.Points)
                        {
                            markerPoints.Add(polylineAnnotation.InternalAnnotation.Transform(p));
                            markerSizes.Add(2);
                        }

                        var dPoint = polylineAnnotation.InternalAnnotation.InverseTransform(e.Position);
                        // Check if cursor is over any points
                        if (polylineAnnotation.Points.Any(o => Math.Abs(dPoint.X - o.X) < dxy.X && Math.Abs(dPoint.Y - o.Y) < dxy.Y))
                        {
                            updatedCursor = _movePointsCursor;
                        }
                        else
                        {
                            if (e.IsControlDown)
                            {
                                updatedCursor = _addPointCursor;
                            }
                            else
                            {
                                updatedCursor = Cursors.SizeAll;
                            }
                        }
                    }
                }
                else if (aType == typeof(Wpf.LineAnnotation))
                {
                    if (ht.Index == 0) updatedCursor = Cursors.SizeAll;
                }
            }

            if (markerPoints.Count > 0)
            {
                _showPoints = true;
                RenderingExtensions.DrawMarkers(Plot.RenderContext, Plot.ActualModel.PlotArea, markerPoints, MarkerType.Square, new List<ScreenPoint>(), markerSizes, OxyColors.White, OxyColors.Black, 2);
            }
            else
            {
                if (requiresRedraw) Plot.InvalidatePlot(false);
            }

            if (updatedCursor == null)
            {
                Plot.Cursor = Plot.DefaultPlotCursor;
            }
            else
            {
                Plot.Cursor = updatedCursor;
                return;
            }

            // Set closed pan hand if needed
            if ((Mouse.LeftButton == MouseButtonState.Pressed && PanButton.IsChecked == true) || Mouse.MiddleButton == MouseButtonState.Pressed)
            {
                Plot.PanCursor = _panHandClosedCursor;
                Plot.DefaultPlotCursor = _panHandClosedCursor;
                Plot.Cursor = _panHandClosedCursor;
            }
        }

        /// <summary>
        /// Plot model mouse up.
        /// </summary>
        private void PlotModelMouseUp(object sender, OxyMouseEventArgs e)
        {
            if (_addAnnotationToolMode == AddToolMode.AddPolygonAnnotation || _addAnnotationToolMode == AddToolMode.AddPolylineAnnotation)
            {
                if (_doubleClicked) StopAddAnnotation();
            }
            else if (_addAnnotationToolMode == AddToolMode.AddHorizontalLineAnnotation || _addAnnotationToolMode == AddToolMode.AddVerticalLineAnnotation)
            {
                CloseLineAnnotationTooltip((Wpf.LineAnnotation)_targetAddAnnotation);
                StopAddAnnotation();
            }
            else if (_addAnnotationToolMode == AddToolMode.AddRectangleAnnotation)
            {
                // Check to see if the size of rectangle is at least 10 pixels in height and width
                var rectangle = (Wpf.RectangleAnnotation)_targetAddAnnotation;
                ScreenPoint upperRight = rectangle.InternalAnnotation.Transform(rectangle.MaximumX, rectangle.MaximumY);
                ScreenPoint lowerLeft = rectangle.InternalAnnotation.Transform(rectangle.MinimumX, rectangle.MinimumY);
                double pixelWidth = Math.Abs(upperRight.X - lowerLeft.X);
                double pixelHeight = Math.Abs(upperRight.Y - lowerLeft.Y);
                // Correct the height and width if necessary
                if (pixelWidth < 10 || pixelHeight < 10)
                {
                    var plotLL = rectangle.InternalAnnotation.InverseTransform(new ScreenPoint(Plot.ActualModel.PlotArea.Left, Plot.ActualModel.PlotArea.Bottom));
                    var plotUR = rectangle.InternalAnnotation.InverseTransform(new ScreenPoint(Plot.ActualModel.PlotArea.Right, Plot.ActualModel.PlotArea.Top));
                    double centerXShift = Math.Abs((plotUR.X - plotLL.X) * 0.1);
                    double centerYShift = Math.Abs((plotUR.Y - plotLL.Y) * 0.1);
                    var mouseDataPoint = rectangle.InternalAnnotation.InverseTransform(e.Position);
                    if (pixelWidth < 10)
                    {
                        rectangle.MinimumX = mouseDataPoint.X - centerXShift;
                        rectangle.MaximumX = mouseDataPoint.X + centerXShift;
                    }
                    if (pixelHeight < 10)
                    {
                        rectangle.MinimumY = mouseDataPoint.Y - centerYShift;
                        rectangle.MaximumY = mouseDataPoint.Y + centerYShift;
                    }
                }
                StopAddAnnotation();
            }
            else if (_addAnnotationToolMode == AddToolMode.AddEllipseAnnotation)
            {
                // Check to see if the size of the ellipse is at least 10 pixels in height and width
                var ellipse = (Wpf.EllipseAnnotation)_targetAddAnnotation;
                ScreenPoint upperRight = ellipse.InternalAnnotation.Transform(ellipse.MaximumX, ellipse.MaximumY);
                ScreenPoint lowerLeft = ellipse.InternalAnnotation.Transform(ellipse.MinimumX, ellipse.MinimumY);
                double pixelWidth = Math.Abs(upperRight.X - lowerLeft.X);
                double pixelHeight = Math.Abs(upperRight.Y - lowerLeft.Y);
                // Correct the height and width if necessary
                if (pixelWidth < 10 || pixelHeight < 10)
                {
                    var plotLL = ellipse.InternalAnnotation.InverseTransform(new ScreenPoint(Plot.ActualModel.PlotArea.Left, Plot.ActualModel.PlotArea.Bottom));
                    var plotUR = ellipse.InternalAnnotation.InverseTransform(new ScreenPoint(Plot.ActualModel.PlotArea.Right, Plot.ActualModel.PlotArea.Top));
                    double centerXShift = Math.Abs((plotUR.X - plotLL.X) * 0.1);
                    double centerYShift = Math.Abs((plotUR.Y - plotLL.Y) * 0.1);
                    var mouseDataPoint = ellipse.InternalAnnotation.InverseTransform(e.Position);
                    if (pixelWidth < 10)
                    {
                        ellipse.MinimumX = mouseDataPoint.X - centerXShift;
                        ellipse.MaximumX = mouseDataPoint.X + centerXShift;
                    }
                    if (pixelHeight < 10)
                    {
                        ellipse.MinimumY = mouseDataPoint.Y - centerYShift;
                        ellipse.MaximumY = mouseDataPoint.Y + centerYShift;
                    }
                }
                StopAddAnnotation();
            }
            else
            {
                if (_addAnnotationToolMode == AddToolMode.AddArrowAnnotation)
                {
                    var arrow = (Wpf.ArrowAnnotation)_targetAddAnnotation;
                    if (Math.Abs(arrow.StartPoint.X - arrow.EndPoint.X) < 0.000000001 && Math.Abs(arrow.StartPoint.Y - arrow.EndPoint.Y) < 0.000000001)
                    {
                        OxyRect plotArea = Plot.ActualModel.PlotArea;
                        var plotCenter = ConvertScreenPointToDataPoint(plotArea.Center);
                        double xShift = plotArea.Center.X + Math.Abs(plotArea.Right - plotArea.Left) * 0.05;
                        var centerXShifted = ConvertScreenPointToDataPoint(new ScreenPoint(xShift, plotArea.Center.Y));
                        arrow.StartPoint = new DataPoint(arrow.StartPoint.X + centerXShifted.X, arrow.StartPoint.Y);
                    }
                }
                StopAddAnnotation();
            }

            SetCursor();
        }

        /// <summary>
        /// Get the selected objects and build the right-click context menu.
        /// </summary>
        private void GetSelectedObjects(object sender, OxyMouseDownEventArgs e)
        {
            // Middle clicks initiate the Pan option
            if (e.ChangedButton == OxyMouseButton.Middle) return;

            // Left clicks try to edit/open the first thing clicked. Right clicks provide more context.
            bool leftClickBool = e.ChangedButton == OxyMouseButton.Left;

            _contextMenu = new ContextMenu();

            if (!leftClickBool && Plot.ActualModel.PlotArea.Contains(e.Position))
            {
                var formatPlotItem = new MenuItem { Header = "Format Plot Area", Icon = CreateMenuIcon("Format.png") };
                formatPlotItem.Click += (s, args) => PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.General_PlotArea, Plot.ActualModel.PlotArea);
                _contextMenu.Items.Add(formatPlotItem);
            }

            // SERIES hit test
            var seriesHTRS = Plot.ActualModel.HitTest(new HitTestArguments(e.Position, 10)).ToList();
            foreach (var htr in seriesHTRS)
            {
                var wpfSeries = Plot.Series.FirstOrDefault(d => d.InternalSeries.Equals(htr.Element));
                if (wpfSeries != null)
                {
                    if (leftClickBool)
                    {
                        PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.Series_General, wpfSeries);
                        return;
                    }
                    else
                    {
                        var seriesItem = new MenuItem { Header = "Format Series: " + wpfSeries.Title, Icon = CreateMenuIcon("Format.png") };
                        seriesItem.Click += (s, args) => PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.Series_General, wpfSeries);
                        _contextMenu.Items.Add(seriesItem);
                    }
                }
            }

            var plotAndAxisArea = Plot.ActualModel.PlotAndAxisArea;
            var plotArea = Plot.ActualModel.PlotArea;

            // Legend Area custom hit test
            var legendArea = Plot.ActualModel.LegendArea;
            if (legendArea.Contains(e.Position))
            {
                if (leftClickBool)
                {
                    PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.Legend_Title, legendArea);
                    return;
                }
                else
                {
                    var legendItem = new MenuItem { Header = "Format Legend", Icon = CreateMenuIcon("Format.png") };
                    legendItem.Click += (s, args) => PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.Legend_Title, legendArea);
                    _contextMenu.Items.Add(legendItem);
                }
            }

            // TEXT HIT TEST
            var textResult = Plot.canvas.InputHitTest(new Point(e.Position.X, e.Position.Y));
            if (textResult != null)
            {
                if (textResult.GetType() == typeof(TextBlock))
                {
                    var txtblock = (TextBlock)textResult;

                    // CHART TITLE SELECTED
                    if (Plot.Title == txtblock.Text && Plot.ActualModel.TitleArea.Contains(new ScreenPoint(e.Position.X, e.Position.Y)))
                    {
                        if (leftClickBool)
                        {
                            PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.General_PlotTitle, Plot.ActualModel.TitleArea);
                            CreateEditTBX(txtblock, Plot, Wpf.Plot.TitleProperty, 0, Plot.canvas);
                            return;
                        }
                        else
                        {
                            var editTitleItem = new MenuItem { Header = "Edit Plot Title", Icon = CreateMenuIcon("EditTextbox.png") };
                            editTitleItem.Click += (s, args) =>
                            {
                                PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.General_PlotTitle, Plot.ActualModel.TitleArea);
                                CreateEditTBX(txtblock, Plot, Wpf.Plot.TitleProperty, 0, Plot.canvas);
                            };
                            var formatTitleItem = new MenuItem { Header = "Format Plot Title", Icon = CreateMenuIcon("Format.png") };
                            formatTitleItem.Click += (s, args) =>
                            {
                                PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.General_PlotTitle, Plot.ActualModel.TitleArea);
                                PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.General_PlotSubtitle, Plot.ActualModel.TitleArea);
                            };
                            _contextMenu.Items.Add(editTitleItem);
                            _contextMenu.Items.Add(formatTitleItem);
                        }
                    }

                    // CHART SUBTITLE SELECTED
                    if (Plot.Subtitle == txtblock.Text && Plot.ActualModel.TitleArea.Contains(new ScreenPoint(e.Position.X, e.Position.Y)))
                    {
                        if (leftClickBool)
                        {
                            PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.General_PlotSubtitle, Plot.ActualModel.TitleArea);
                            CreateEditTBX(txtblock, Plot, Wpf.Plot.SubtitleProperty, 0, Plot.canvas);
                            return;
                        }
                        else
                        {
                            var editSubtitleItem = new MenuItem { Header = "Edit Plot Subtitle", Icon = CreateMenuIcon("EditTextbox.png") };
                            editSubtitleItem.Click += (s, args) =>
                            {
                                PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.General_PlotSubtitle, Plot.ActualModel.TitleArea);
                                CreateEditTBX(txtblock, Plot, Wpf.Plot.SubtitleProperty, 0, Plot.canvas);
                            };
                            var formatSubtitleItem = new MenuItem { Header = "Format Plot Subtitle", Icon = CreateMenuIcon("Format.png") };
                            formatSubtitleItem.Click += (s, args) =>
                            {
                                PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.General_PlotSubtitle, Plot.ActualModel.TitleArea);
                            };
                            _contextMenu.Items.Add(editSubtitleItem);
                            _contextMenu.Items.Add(formatSubtitleItem);
                        }
                    }

                    // AXES TITLES SELECTED
                    if (Plot.ActualModel.PlotAndAxisArea.Contains(new ScreenPoint(e.Position.X, e.Position.Y)))
                    {
                        var axes = Plot.Axes.Where(x => x.Title != null && txtblock.Text.Contains(x.Title)).ToList();

                        if (axes.Count > 1)
                        {
                            // Narrow it down to the selected axis area
                            OxyRect selectedAxisArea = default;

                            foreach (var ax in axes)
                            {
                                var dummyCanvas = new Canvas();
                                var crc = new Wpf.CanvasRenderContext(dummyCanvas);
                                var size = new Size(Plot.canvas.ActualWidth, Plot.canvas.ActualHeight);
                                dummyCanvas.Measure(size);
                                dummyCanvas.Arrange(new Rect(size));
                                dummyCanvas.UpdateLayout();

                                ax.InternalAxis.Render(crc, 1);
                                dummyCanvas.UpdateLayout();

                                foreach (var tbk in FindVisualChildren<TextBlock>(dummyCanvas))
                                {
                                    string title = ax.Title;
                                    if (ax.Unit != null)
                                    {
                                        title = string.Format(ax.TitleFormatString, ax.Title, ax.Unit);
                                    }

                                    if (title == tbk.Text)
                                    {
                                        double axLeft, axTop, axWidth, axHeight;
                                        if (ax.Position == OxyPlot.Axes.AxisPosition.Left || ax.Position == OxyPlot.Axes.AxisPosition.Right)
                                        {
                                            axLeft = GetPosition(tbk, dummyCanvas).X;
                                            axTop = GetPosition(tbk, dummyCanvas).Y - tbk.ActualWidth;
                                            axWidth = tbk.ActualHeight;
                                            axHeight = tbk.ActualWidth;
                                        }
                                        else
                                        {
                                            axLeft = GetPosition(tbk, dummyCanvas).X;
                                            axTop = GetPosition(tbk, dummyCanvas).Y;
                                            axWidth = tbk.ActualWidth;
                                            axHeight = tbk.ActualHeight;
                                        }

                                        selectedAxisArea = new OxyRect(axLeft, axTop, axWidth, axHeight);

                                        if (selectedAxisArea.Contains(e.Position))
                                        {
                                            axes = new List<Wpf.Axis> { ax };
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (axes.Count == 1)
                        {
                            var ax = axes.First();

                            if (leftClickBool)
                            {
                                PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.Axes_Title, ax);
                                if (ax.InternalAxis.IsVertical())
                                {
                                    CreateEditTBX(txtblock, ax, Wpf.Axis.TitleProperty, -90, Plot.canvas);
                                }
                                else
                                {
                                    CreateEditTBX(txtblock, ax, Wpf.Axis.TitleProperty, 0, Plot.canvas);
                                }
                                return;
                            }
                            else
                            {
                                var editAxisItem = new MenuItem { Header = "Edit Axis Title: " + ax.Title, Icon = CreateMenuIcon("EditTextbox.png") };
                                editAxisItem.Click += (s, args) =>
                                {
                                    PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.Axes_Title, ax);
                                    if (ax.InternalAxis.IsVertical())
                                    {
                                        CreateEditTBX(txtblock, ax, Wpf.Axis.TitleProperty, -90, Plot.canvas);
                                    }
                                    else
                                    {
                                        CreateEditTBX(txtblock, ax, Wpf.Axis.TitleProperty, 0, Plot.canvas);
                                    }
                                };
                                _contextMenu.Items.Add(editAxisItem);
                            }
                        }
                    }
                }
            }

            // AXIS Areas hit test
            foreach (var ax in Plot.Axes)
            {
                double axLeft = 0, axTop = 0, axWidth = 0, axHeight = 0;

                switch (ax.Position)
                {
                    case OxyPlot.Axes.AxisPosition.Bottom:
                        axLeft = plotArea.Left;
                        axTop = plotArea.Bottom + ax.AxisDistance;
                        axWidth = plotArea.Width;
                        axHeight = ax.InternalAxis.DesiredSize.Height;
                        break;
                    case OxyPlot.Axes.AxisPosition.Top:
                        axLeft = plotArea.Left;
                        axTop = plotArea.Top - ax.AxisDistance - ax.InternalAxis.DesiredSize.Height;
                        axWidth = plotArea.Width;
                        axHeight = ax.InternalAxis.DesiredSize.Height;
                        break;
                    case OxyPlot.Axes.AxisPosition.Left:
                        axLeft = plotArea.Left - ax.AxisDistance - ax.InternalAxis.DesiredSize.Width;
                        axTop = plotArea.Top;
                        axWidth = ax.InternalAxis.DesiredSize.Width;
                        axHeight = plotArea.Height;
                        break;
                    case OxyPlot.Axes.AxisPosition.Right:
                        axLeft = plotArea.Right + ax.AxisDistance;
                        axTop = plotArea.Top;
                        axWidth = ax.InternalAxis.DesiredSize.Width;
                        axHeight = plotArea.Height;
                        break;
                }

                var axArea1 = new OxyRect(axLeft, axTop, axWidth, axHeight);
                if (axArea1.Contains(e.Position))
                {
                    if (leftClickBool)
                    {
                        PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.Axes_Options, ax);
                        PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.Axes_Display, ax);
                        return;
                    }
                    else
                    {
                        var formatAxisItem = new MenuItem { Header = "Format Axis: " + ax.Title, Icon = CreateMenuIcon("Format.png") };
                        formatAxisItem.Click += (s, args) =>
                        {
                            PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.Axes_Options, ax);
                            PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.Axes_Display, ax);
                        };
                        _contextMenu.Items.Add(formatAxisItem);
                    }
                }
            }

            // ANNOTATIONS hit test
            var annoHTRS = Plot.ActualModel.HitTest(new HitTestArguments(e.Position, 10)).ToList();
            foreach (var htr in annoHTRS)
            {
                var theAnno = htr.Element as OxyPlot.Annotations.Annotation;
                if (theAnno != null)
                {
                    var wpfAnno = Plot.Annotations.FirstOrDefault(d => d.InternalAnnotation == theAnno);
                    if (wpfAnno != null)
                    {
                        var annoText = ((Wpf.TextualAnnotation)wpfAnno).Text;

                        if (leftClickBool)
                        {
                            PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, wpfAnno);
                            return;
                        }
                        else
                        {
                            var editAnnoItem = new MenuItem { Header = "Edit Annotation Text: " + annoText, Icon = CreateMenuIcon("EditTextbox.png") };
                            editAnnoItem.Click += (s, args) =>
                            {
                                PropertiesCalled?.Invoke(Plot, false, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, wpfAnno);

                                // Dummy canvas is used to render the item
                                var dummyCanvas = new Canvas();
                                var crc = new Wpf.CanvasRenderContext(dummyCanvas);
                                var size = new Size(Plot.canvas.ActualWidth, Plot.canvas.ActualHeight);
                                dummyCanvas.Measure(size);
                                dummyCanvas.Arrange(new Rect(size));
                                dummyCanvas.UpdateLayout();
                                wpfAnno.InternalAnnotation.Render(crc);
                                dummyCanvas.UpdateLayout();

                                foreach (var tbk in FindVisualChildren<TextBlock>(dummyCanvas))
                                {
                                    var annoType = wpfAnno.GetType();
                                    if (annoType == typeof(Wpf.ArrowAnnotation))
                                    {
                                        var anno = (Wpf.ArrowAnnotation)wpfAnno;
                                        if (tbk.Text == anno.Text)
                                        {
                                            CreateEditTBX(tbk, anno, Wpf.ArrowAnnotation.TextProperty, anno.TextRotation, dummyCanvas);
                                        }
                                    }
                                    else if (annoType == typeof(Wpf.TextAnnotation))
                                    {
                                        var anno = (Wpf.TextAnnotation)wpfAnno;
                                        if (tbk.Text == anno.Text)
                                        {
                                            CreateEditTBX(tbk, anno, Wpf.TextAnnotation.TextProperty, anno.TextRotation, dummyCanvas);
                                        }
                                    }
                                    else if (annoType == typeof(Wpf.RectangleAnnotation))
                                    {
                                        var anno = (Wpf.RectangleAnnotation)wpfAnno;
                                        if (tbk.Text == anno.Text)
                                        {
                                            CreateEditTBX(tbk, anno, Wpf.RectangleAnnotation.TextProperty, anno.TextRotation, dummyCanvas);
                                        }
                                    }
                                    else if (annoType == typeof(Wpf.EllipseAnnotation))
                                    {
                                        var anno = (Wpf.EllipseAnnotation)wpfAnno;
                                        if (tbk.Text == anno.Text)
                                        {
                                            CreateEditTBX(tbk, anno, Wpf.EllipseAnnotation.TextProperty, anno.TextRotation, dummyCanvas);
                                        }
                                    }
                                    else if (annoType == typeof(Wpf.PointAnnotation))
                                    {
                                        var anno = (Wpf.PointAnnotation)wpfAnno;
                                        if (tbk.Text == anno.Text)
                                        {
                                            CreateEditTBX(tbk, anno, Wpf.PointAnnotation.TextProperty, anno.TextRotation, dummyCanvas);
                                        }
                                    }
                                    else if (annoType == typeof(Wpf.PolygonAnnotation))
                                    {
                                        var anno = (Wpf.PolygonAnnotation)wpfAnno;
                                        if (tbk.Text == anno.Text)
                                        {
                                            CreateEditTBX(tbk, anno, Wpf.PolygonAnnotation.TextProperty, anno.TextRotation, dummyCanvas);
                                        }
                                    }
                                    else if (annoType == typeof(Wpf.PolylineAnnotation))
                                    {
                                        var anno = (Wpf.PolylineAnnotation)wpfAnno;
                                        if (tbk.Text == anno.Text)
                                        {
                                            CreateEditTBX(tbk, anno, Wpf.PolylineAnnotation.TextProperty, anno.TextRotation, dummyCanvas);
                                        }
                                    }
                                    else if (annoType == typeof(Wpf.LineAnnotation))
                                    {
                                        var anno = (Wpf.LineAnnotation)wpfAnno;
                                        if (tbk.Text == anno.Text)
                                        {
                                            CreateEditTBX(tbk, anno, Wpf.LineAnnotation.TextProperty, anno.TextRotation, dummyCanvas);
                                        }
                                    }
                                }
                            };

                            var formatAnnoItem = new MenuItem { Header = "Format Annotation: " + annoText, Icon = CreateMenuIcon("Format.png") };
                            formatAnnoItem.Click += (s, args) => PropertiesCalled?.Invoke(Plot, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, wpfAnno);

                            var deleteAnnoItem = new MenuItem { Header = "Delete Annotation: " + annoText, Icon = CreateMenuIcon("Delete.png") };
                            deleteAnnoItem.Click += (s, args) =>
                            {
                                Plot.Annotations.Remove(wpfAnno);
                                Plot.InvalidatePlot(false);
                            };

                            _contextMenu.Items.Add(editAnnoItem);
                            _contextMenu.Items.Add(formatAnnoItem);
                            _contextMenu.Items.Add(deleteAnnoItem);
                        }
                    }
                }

                // Only add one CM for annotations
                break;
            }

            if (_contextMenu.Items.Count == 0)
            {
                return;
            }

            _contextMenu.Placement = PlacementMode.MousePoint;
            _contextMenu.HorizontalOffset = 0;
            _contextMenu.VerticalOffset = 0;
            _contextMenu.IsOpen = true;
        }

        #endregion

        #region CreateEditTBX

        /// <summary>
        /// Creates an in-place text box for editing text on the plot.
        /// Supports plot titles, axis titles, and all annotation types.
        /// </summary>
        /// <param name="existingTextblock">The existing TextBlock element being edited.</param>
        /// <param name="dependencyObj">The dependency object containing the text property.</param>
        /// <param name="dependencyProp">The dependency property to bind the text to.</param>
        /// <param name="angle">The rotation angle for the text box.</param>
        /// <param name="canvas">The canvas for positioning.</param>
        private void CreateEditTBX(TextBlock existingTextblock, DependencyObject dependencyObj, DependencyProperty dependencyProp, double angle, Canvas canvas)
        {
            IInputElement txtblckAsInputElem = existingTextblock as IInputElement;
            Color currentTextColor = Colors.Black; // This is for all annotations (hiding text while editing)
            Color currentStrokeColor = Colors.Black; // This is just for the text annotation, which is a box by default

            Point point;
            try
            {
                point = GetPosition((Visual)txtblckAsInputElem, canvas);
            }
            catch
            {
                return;
            }

            double left = point.X;
            double top = point.Y;
            double width = existingTextblock.ActualWidth;
            double height = existingTextblock.ActualHeight;
            double fontsize = existingTextblock.FontSize;
            FontFamily fontFamily = existingTextblock.FontFamily;
            FontWeight fontWeight = existingTextblock.FontWeight;
            Brush foreColor = existingTextblock.Foreground;

            // Create canvas for the textbox overlay
            var canvasOverlay = new Canvas { Name = "TextBoxCanvas" };
            canvasOverlay.Background = new SolidColorBrush(Colors.Transparent);
            var dockPanel = new DockPanel();
            var plotParent = (Grid)Plot.canvas.Parent;
            plotParent.Children.Add(canvasOverlay);

            // Set initial text box settings
            _textBox = new TextBox();
            _textBox.Background = Plot.Background;
            _textBox.TextAlignment = TextAlignment.Center;
            _textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            _textBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            _textBox.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
            _textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;
            _textBox.Padding = new Thickness(-2);
            _textBox.FontSize = fontsize;
            _textBox.FontFamily = fontFamily;
            _textBox.FontWeight = fontWeight;
            _textBox.Foreground = foreColor;
            TextOptions.SetTextFormattingMode(_textBox, TextFormattingMode.Display);

            // Normalize all angles to 0-360
            if (angle < 0 || angle >= 360)
            {
                angle = angle % 360;
                if (angle < 0)
                {
                    angle += 360;
                }
            }

            // Determine what type of element was selected
            var depObjType = dependencyObj.GetType();

            if (depObjType == typeof(Wpf.Plot))
            {
                // It must be a title or subtitle, which are handled the same way
                dockPanel.RenderTransform = new RotateTransform(angle, 0, 0);
                dockPanel.Width = Plot.ActualModel.PlotArea.Width;
                dockPanel.Height = height;
                Canvas.SetLeft(dockPanel, Plot.ActualModel.PlotArea.Left);
                Canvas.SetTop(dockPanel, top);

                string title = Plot.Title;
                currentTextColor = Plot.TitleColor;
                Plot.TitleColor = Colors.Transparent;
                _textBox.Text = title;
            }
            else if (depObjType == typeof(Wpf.LogarithmicAxis) || depObjType == typeof(Wpf.LinearAxis) ||
                     depObjType == typeof(Wpf.DateTimeAxis) || depObjType == typeof(Wpf.CategoryAxis) ||
                     depObjType == typeof(Wpf.GumbelProbabilityAxis) || depObjType == typeof(Wpf.LinearColorAxis) ||
                     depObjType == typeof(Wpf.AngleAxis) || depObjType == typeof(Wpf.NormalProbabilityAxis) ||
                     depObjType == typeof(Wpf.TimeSpanAxis) || depObjType == typeof(Wpf.MagnitudeAxis))
            {
                if (angle == 0)
                {
                    dockPanel.RenderTransform = new RotateTransform(angle, 0, 0);
                    dockPanel.Width = Plot.ActualModel.PlotArea.Width;
                    dockPanel.Height = height;
                    Canvas.SetLeft(dockPanel, Plot.ActualModel.PlotArea.Left);
                    Canvas.SetTop(dockPanel, top);
                }
                else if (angle == 270) // Vertical text - flowing up
                {
                    dockPanel.RenderTransform = new RotateTransform(angle, 0, 0);
                    dockPanel.Width = Plot.ActualModel.PlotArea.Height;
                    dockPanel.Height = height;
                    Canvas.SetTop(dockPanel, Plot.ActualModel.PlotArea.Bottom);
                    Canvas.SetLeft(dockPanel, left);
                }
                // else angle not handled
            }
            else if (depObjType == typeof(Wpf.ArrowAnnotation))
            {
                dockPanel.RenderTransform = new RotateTransform(0, 0, 0);
                var annotation = (Wpf.ArrowAnnotation)dependencyObj;
                _textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.Padding = new Thickness(0);

                // Hide rotated annotation
                currentTextColor = annotation.TextColor;
                annotation.TextColor = Colors.Transparent;

                var startPoint = annotation.InternalAnnotation.Transform(annotation.StartPoint.X, annotation.StartPoint.Y);

                dockPanel.Width = existingTextblock.Width + 2;
                Canvas.SetTop(dockPanel, startPoint.Y - height);
                Canvas.SetLeft(dockPanel, startPoint.X);
            }
            else if (depObjType == typeof(Wpf.LineAnnotation))
            {
                var annotation = (Wpf.LineAnnotation)dependencyObj;
                _textBox.HorizontalAlignment = annotation.TextHorizontalAlignment;
                _textBox.HorizontalContentAlignment = annotation.TextHorizontalAlignment;
                _textBox.VerticalAlignment = annotation.TextVerticalAlignment;
                _textBox.VerticalContentAlignment = annotation.TextVerticalAlignment;
                _textBox.Padding = new Thickness(0);
                currentTextColor = annotation.TextColor;
                annotation.TextColor = Colors.Transparent;

                switch (annotation.Type)
                {
                    case OxyPlot.Annotations.LineAnnotationType.Horizontal:
                        dockPanel.RenderTransform = new RotateTransform(0, 0, 0);
                        dockPanel.Width = existingTextblock.ActualWidth + 2;
                        _textBox.Width = dockPanel.Width;
                        Canvas.SetLeft(dockPanel, left);
                        Canvas.SetTop(dockPanel, top);
                        break;
                    case OxyPlot.Annotations.LineAnnotationType.Vertical:
                        dockPanel.RenderTransform = new RotateTransform(0, 0, 0);
                        dockPanel.Width = existingTextblock.ActualWidth + 2;
                        _textBox.Width = dockPanel.Width;
                        Canvas.SetLeft(dockPanel, left);
                        Canvas.SetTop(dockPanel, top);
                        break;
                    default: // Linear equation
                        dockPanel.RenderTransform = new RotateTransform(0, 0, 0);
                        dockPanel.Width = existingTextblock.ActualWidth + 2;
                        _textBox.Width = dockPanel.Width;
                        Canvas.SetLeft(dockPanel, left);
                        Canvas.SetTop(dockPanel, top);
                        break;
                }
            }
            else if (depObjType == typeof(Wpf.PolygonAnnotation))
            {
                var annotation = (Wpf.PolygonAnnotation)dependencyObj;
                currentTextColor = annotation.TextColor;
                annotation.TextColor = Colors.Transparent;
                // Just make a generic textbox near the object
                _textBox.Width = existingTextblock.Width;
                _textBox.TextAlignment = TextAlignment.Left;
                _textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.Padding = new Thickness(0);
                dockPanel.RenderTransform = new RotateTransform(0, 0, 0);
                dockPanel.Width = _textBox.Width;
                dockPanel.Height = height;
                Canvas.SetLeft(dockPanel, left);
                Canvas.SetTop(dockPanel, top);
            }
            else if (depObjType == typeof(Wpf.PolylineAnnotation))
            {
                var annotation = (Wpf.PolylineAnnotation)dependencyObj;
                currentTextColor = annotation.TextColor;
                annotation.TextColor = Colors.Transparent;
                // Just make a generic textbox near the object
                _textBox.Width = existingTextblock.Width;
                _textBox.TextAlignment = TextAlignment.Left;
                _textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.Padding = new Thickness(0);
                dockPanel.RenderTransform = new RotateTransform(0, 0, 0);
                dockPanel.Width = _textBox.Width;
                dockPanel.Height = height;
                Canvas.SetLeft(dockPanel, left);
                Canvas.SetTop(dockPanel, top);
            }
            else if (depObjType == typeof(Wpf.PointAnnotation))
            {
                var annotation = (Wpf.PointAnnotation)dependencyObj;
                currentTextColor = annotation.TextColor;
                annotation.TextColor = Colors.Transparent;
                // Just make a generic textbox near the object
                _textBox.Width = existingTextblock.Width;
                _textBox.TextAlignment = TextAlignment.Left;
                _textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.Padding = new Thickness(0);
                dockPanel.RenderTransform = new RotateTransform(0, 0, 0);
                dockPanel.Width = _textBox.Width;
                dockPanel.Height = height;
                Canvas.SetLeft(dockPanel, left);
                Canvas.SetTop(dockPanel, top);
            }
            else if (depObjType == typeof(Wpf.RectangleAnnotation))
            {
                var annotation = (Wpf.RectangleAnnotation)dependencyObj;
                currentTextColor = annotation.TextColor;
                annotation.TextColor = Colors.Transparent;
                // Just make a generic text box near the object
                _textBox.Width = existingTextblock.Width;
                _textBox.TextAlignment = TextAlignment.Left;
                _textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.Padding = new Thickness(0);
                dockPanel.RenderTransform = new RotateTransform(0, 0, 0);
                dockPanel.Width = _textBox.Width;
                dockPanel.Height = height;
                Canvas.SetLeft(dockPanel, left);
                Canvas.SetTop(dockPanel, top);
            }
            else if (depObjType == typeof(Wpf.TextAnnotation))
            {
                var annotation = (Wpf.TextAnnotation)dependencyObj;
                currentTextColor = annotation.TextColor;
                annotation.TextColor = Colors.Transparent;
                currentStrokeColor = annotation.Stroke;
                annotation.Stroke = Colors.Transparent;
                // Just make a generic text box near the object
                _textBox.Width = existingTextblock.Width;
                _textBox.TextAlignment = TextAlignment.Left;
                _textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.Padding = new Thickness(0);
                dockPanel.RenderTransform = new RotateTransform(0, 0, 0);
                dockPanel.Width = _textBox.Width;
                dockPanel.Height = height;
                Canvas.SetLeft(dockPanel, left);
                Canvas.SetTop(dockPanel, top);
            }
            else if (depObjType == typeof(Wpf.EllipseAnnotation))
            {
                var annotation = (Wpf.EllipseAnnotation)dependencyObj;
                currentTextColor = annotation.TextColor;
                annotation.TextColor = Colors.Transparent;
                // Just make a generic text box near the object
                _textBox.Width = existingTextblock.Width;
                _textBox.TextAlignment = TextAlignment.Left;
                _textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                _textBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                _textBox.Padding = new Thickness(0);
                dockPanel.RenderTransform = new RotateTransform(0, 0, 0);
                dockPanel.Width = _textBox.Width;
                dockPanel.Height = height;
                Canvas.SetLeft(dockPanel, left);
                Canvas.SetTop(dockPanel, top);
            }

            // Add the textbox to the dock panel and canvas
            dockPanel.Children.Add(_textBox);
            canvasOverlay.Children.Add(dockPanel);

            // Focus color from template is controlling here
            _textBox.BorderThickness = new Thickness(1);
            _textBox.Focus();

            // Set up binding
            var binding = new Binding { Mode = BindingMode.OneWay, Source = _textBox, Path = new PropertyPath("Text") };

            // Check if dependency object is an axis, in which case, need to get initial title
            if (depObjType == typeof(Wpf.LogarithmicAxis) || depObjType == typeof(Wpf.LinearAxis) ||
                depObjType == typeof(Wpf.DateTimeAxis) || depObjType == typeof(Wpf.CategoryAxis) ||
                depObjType == typeof(Wpf.GumbelProbabilityAxis) || depObjType == typeof(Wpf.LinearColorAxis) ||
                depObjType == typeof(Wpf.AngleAxis) || depObjType == typeof(Wpf.NormalProbabilityAxis) ||
                depObjType == typeof(Wpf.TimeSpanAxis) || depObjType == typeof(Wpf.MagnitudeAxis))
            {
                var axis = (Wpf.Axis)dependencyObj;
                string title = axis.Title;
                currentTextColor = axis.TitleColor;
                axis.TitleColor = Colors.Transparent;
                _textBox.Text = title; // Set up initial text
            }
            else
            {
                _textBox.Text = existingTextblock.Text; // Set up initial text
            }

            // Put the cursor at the end of the textbox
            if (_textBox.Text != null && _textBox.Text.Length > 0)
            {
                _textBox.SelectionStart = _textBox.Text.Length;
            }
            BindingOperations.SetBinding(dependencyObj, dependencyProp, binding);

            // If the plot size changes, remove the textbox overlay
            // Note: Do NOT clear the binding here - the binding must remain in place for the text to be saved
            Plot.SizeChanged += (s, args) =>
            {
                plotParent.Children.Remove(canvasOverlay);
                // This will also fire the lost focus event below
            };

            // On key enter, remove the textbox overlay
            // Note: Do NOT clear the binding here - the binding must remain in place for the text to be saved
            _textBox.PreviewKeyDown += (s, args) =>
            {
                if (args.Key == Key.Enter)
                {
                    plotParent.Children.Remove(canvasOverlay);
                    // This will also fire the lost focus event below
                }
            };

            // On lost focus, remove the textbox overlay
            // Note: Do NOT clear the binding here - the binding must remain in place for the text to be saved
            _textBox.LostFocus += (s, args) =>
            {
                plotParent.Children.Remove(canvasOverlay);

                // Change the color of the text back from transparent for annotations
                if (depObjType == typeof(Wpf.RectangleAnnotation))
                {
                    var anno = (Wpf.RectangleAnnotation)dependencyObj;
                    anno.TextColor = currentTextColor;
                }
                else if (depObjType == typeof(Wpf.LineAnnotation))
                {
                    var anno = (Wpf.LineAnnotation)dependencyObj;
                    anno.TextColor = currentTextColor;
                }
                else if (depObjType == typeof(Wpf.PolygonAnnotation))
                {
                    var anno = (Wpf.PolygonAnnotation)dependencyObj;
                    anno.TextColor = currentTextColor;
                }
                else if (depObjType == typeof(Wpf.PolylineAnnotation))
                {
                    var anno = (Wpf.PolylineAnnotation)dependencyObj;
                    anno.TextColor = currentTextColor;
                }
                else if (depObjType == typeof(Wpf.EllipseAnnotation))
                {
                    var anno = (Wpf.EllipseAnnotation)dependencyObj;
                    anno.TextColor = currentTextColor;
                }
                else if (depObjType == typeof(Wpf.ArrowAnnotation))
                {
                    var anno = (Wpf.ArrowAnnotation)dependencyObj;
                    anno.TextColor = currentTextColor;
                }
                else if (depObjType == typeof(Wpf.PointAnnotation))
                {
                    var anno = (Wpf.PointAnnotation)dependencyObj;
                    anno.TextColor = currentTextColor;
                }
                else if (depObjType == typeof(Wpf.TextAnnotation))
                {
                    var anno = (Wpf.TextAnnotation)dependencyObj;
                    anno.TextColor = currentTextColor;
                    anno.Stroke = currentStrokeColor;
                }
                else if (depObjType == typeof(Wpf.LogarithmicAxis) || depObjType == typeof(Wpf.LinearAxis) ||
                         depObjType == typeof(Wpf.DateTimeAxis) || depObjType == typeof(Wpf.CategoryAxis) ||
                         depObjType == typeof(Wpf.GumbelProbabilityAxis) || depObjType == typeof(Wpf.LinearColorAxis) ||
                         depObjType == typeof(Wpf.AngleAxis) || depObjType == typeof(Wpf.NormalProbabilityAxis) ||
                         depObjType == typeof(Wpf.TimeSpanAxis) || depObjType == typeof(Wpf.MagnitudeAxis))
                {
                    var ax = (Wpf.Axis)dependencyObj;
                    ax.TitleColor = currentTextColor;
                }
                else if (depObjType == typeof(Wpf.Plot))
                {
                    Plot.TitleColor = currentTextColor;
                }
            };
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Support method for finding visual children.
        /// </summary>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private DataPoint ConvertScreenPointToDataPoint(ScreenPoint pt)
        {
            return Plot.ActualModel.DefaultXAxis.InverseTransform(pt.X, pt.Y, Plot.ActualModel.DefaultYAxis);
        }

        private ScreenPoint ConvertDataPointToScreenPoint(DataPoint pt)
        {
            return Plot.ActualModel.DefaultXAxis.Transform(pt.X, pt.Y, Plot.ActualModel.DefaultYAxis);
        }

        private Point ConvertDataPointToPoint(DataPoint pt)
        {
            var sp = Plot.ActualModel.DefaultXAxis.Transform(pt.X, pt.Y, Plot.ActualModel.DefaultYAxis);
            return new Point(sp.X, sp.Y);
        }

        private DataPoint ConvertLeaderLinePoint(int pointIndex)
        {
            return Plot.ActualModel.DefaultXAxis.InverseTransform(_leaderLine.Points[pointIndex].X, _leaderLine.Points[pointIndex].Y, Plot.ActualModel.DefaultYAxis);
        }

        private Size MeasureString(string candidate, FontFamily family, FontStyle style, FontWeight weight, FontStretch stretch, double size)
        {
            if (candidate == null)
            {
                return new Size(0, 0);
            }
            var formattedText = new FormattedText(
                candidate,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(family, style, weight, stretch),
                size,
                Brushes.Black,
                new NumberSubstitution());
            return new Size(formattedText.Width, formattedText.Height);
        }

        /// <summary>
        /// Gets the position of an element on the plot.
        /// </summary>
        /// <param name="element">The visual element.</param>
        /// <param name="canvas">The canvas to get position relative to.</param>
        /// <returns>The position of the element.</returns>
        private Point GetPosition(Visual element, Canvas canvas)
        {
            var positionTransform = element.TransformToAncestor(canvas);
            var areaPosition = positionTransform.Transform(new Point(0, 0));
            return areaPosition;
        }

        /// <summary>
        /// Loads an image resource and returns it as a BitmapImage.
        /// </summary>
        /// <param name="resourceName">The name of the resource file (e.g., "Format.png").</param>
        /// <returns>A BitmapImage that can be used as an Image source.</returns>
        private static BitmapImage LoadResourceImage(string resourceName)
        {
            var uri = new Uri($"pack://application:,,,/OxyPlotControls;component/Resources/{resourceName}", UriKind.Absolute);
            var bitmapImage = new BitmapImage(uri);
            return bitmapImage;
        }

        /// <summary>
        /// Creates an Image control with the specified resource image.
        /// </summary>
        /// <param name="resourceName">The name of the resource file (e.g., "Format.png").</param>
        /// <returns>An Image control with the resource as its source.</returns>
        private static Image CreateMenuIcon(string resourceName)
        {
            return new Image { Source = LoadResourceImage(resourceName), Width = 16, Height = 16 };
        }

        #endregion

        #region Export Series Data

        /// <summary>
        /// Export series data to file.
        /// </summary>
        private void ExportDataButton_Click(object sender, RoutedEventArgs e)
        {
            var tableList = new List<DataTable>();
            int tableCount = 0;
            string[] badCharacters = { ":", "\\", "/", "?", "*", "[", "]" };

            foreach (Wpf.Series series in Plot.Series)
            {
                var dataTable = new DataTable("Series");
                string seriesName = "";
                tableCount++;

                if (series.GetType() == typeof(Wpf.LineSeries))
                {
                    seriesName = !string.IsNullOrEmpty(series.Title) ? series.Title : "LineSeries_" + tableCount;
                    foreach (var badChar in badCharacters)
                    {
                        seriesName = seriesName.Replace(badChar, "_");
                    }

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_x", typeof(string));
                    dataTable.Columns.Add(seriesName + "_y", typeof(string));

                    var oxySeries = (OxyPlot.Series.LineSeries)series.InternalSeries;

                    if (oxySeries.ItemsSource != null)
                    {
                        var datalist = oxySeries.ItemsSource as IEnumerable<DataPoint>;
                        if (datalist != null)
                        {
                            foreach (var seriesValue in datalist.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y);
                            }
                        }
                        else
                        {
                            foreach (var obj in oxySeries.ItemsSource.Cast<object>())
                            {
                                PropertyInfo propX = obj.GetType().GetProperty(oxySeries.DataFieldX);
                                string xVal = Convert.ToString(propX.GetValue(obj, null));
                                PropertyInfo propY = obj.GetType().GetProperty(oxySeries.DataFieldY);
                                string yVal = Convert.ToString(propY.GetValue(obj, null));
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, xVal, yVal);
                            }
                        }
                    }
                    else
                    {
                        foreach (var seriesValue in oxySeries.Points)
                        {
                            dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y);
                        }
                    }
                }
                else if (series.GetType() == typeof(Wpf.ScatterPointSeries))
                {
                    seriesName = !string.IsNullOrEmpty(series.Title) ? series.Title : "ScatterSeries_" + tableCount;
                    foreach (var badChar in badCharacters)
                    {
                        seriesName = seriesName.Replace(badChar, "_");
                    }

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_x", typeof(string));
                    dataTable.Columns.Add(seriesName + "_y", typeof(string));

                    var oxySeries = (OxyPlot.Series.ScatterSeries)series.InternalSeries;

                    if (oxySeries.ItemsSource != null)
                    {
                        var datalist = oxySeries.ItemsSource as IEnumerable<OxyPlot.Series.ScatterPoint>;
                        if (datalist != null)
                        {
                            foreach (var seriesValue in datalist.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y);
                            }
                        }
                        else
                        {
                            foreach (var obj in oxySeries.ItemsSource.Cast<object>())
                            {
                                PropertyInfo propX = obj.GetType().GetProperty(oxySeries.DataFieldX);
                                string xVal = Convert.ToString(propX.GetValue(obj, null));
                                PropertyInfo propY = obj.GetType().GetProperty(oxySeries.DataFieldY);
                                string yVal = Convert.ToString(propY.GetValue(obj, null));
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, xVal, yVal);
                            }
                        }
                    }
                    else
                    {
                        foreach (var seriesValue in oxySeries.Points)
                        {
                            dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y);
                        }
                    }
                }
                else if (series.GetType() == typeof(Wpf.AreaSeries))
                {
                    seriesName = !string.IsNullOrEmpty(series.Title) ? series.Title : "AreaSeries_" + tableCount;
                    foreach (var badChar in badCharacters)
                    {
                        seriesName = seriesName.Replace(badChar, "_");
                    }

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_x", typeof(string));
                    dataTable.Columns.Add(seriesName + "_y", typeof(string));
                    dataTable.Columns.Add(seriesName + "_x2", typeof(string));
                    dataTable.Columns.Add(seriesName + "_y2", typeof(string));

                    var oxySeries = (OxyPlot.Series.AreaSeries)series.InternalSeries;

                    if (oxySeries.ItemsSource != null)
                    {
                        var datalist = oxySeries.ItemsSource as IEnumerable<DataPoint>;
                        if (datalist != null)
                        {
                            foreach (var seriesValue in datalist.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y);
                            }
                        }
                        else
                        {
                            foreach (var obj in oxySeries.ItemsSource.Cast<object>())
                            {
                                PropertyInfo propX = obj.GetType().GetProperty(oxySeries.DataFieldX);
                                string xVal = Convert.ToString(propX.GetValue(obj, null));
                                PropertyInfo propY = obj.GetType().GetProperty(oxySeries.DataFieldY);
                                string yVal = Convert.ToString(propY.GetValue(obj, null));

                                string xVal2 = "";
                                if (oxySeries.DataFieldX2 != null)
                                {
                                    PropertyInfo propX2 = obj.GetType().GetProperty(oxySeries.DataFieldX2);
                                    xVal2 = Convert.ToString(propX2.GetValue(obj, null));
                                }

                                string yVal2 = "";
                                if (oxySeries.DataFieldY2 != null)
                                {
                                    PropertyInfo propY2 = obj.GetType().GetProperty(oxySeries.DataFieldY2);
                                    yVal2 = Convert.ToString(propY2.GetValue(obj, null));
                                }

                                dataTable.Rows.Add(dataTable.Rows.Count + 1, xVal, yVal, xVal2, yVal2);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < oxySeries.Points.Count; i++)
                        {
                            if (oxySeries.Points2.Count > 0)
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, oxySeries.Points[i].X, oxySeries.Points[i].Y, oxySeries.Points2[i].X, oxySeries.Points2[i].Y);
                            }
                            else
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, oxySeries.Points[i].X, oxySeries.Points[i].Y, "", "");
                            }
                        }
                    }
                }
                else if (series.GetType() == typeof(Wpf.BoxPlotSeries))
                {
                    seriesName = !string.IsNullOrEmpty(series.Title) ? series.Title : "BoxPlotSeries_" + tableCount;
                    foreach (var badChar in badCharacters)
                    {
                        seriesName = seriesName.Replace(badChar, "_");
                    }

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_position", typeof(string));
                    dataTable.Columns.Add(seriesName + "_lowerWhisker", typeof(string));
                    dataTable.Columns.Add(seriesName + "_boxMinimum", typeof(string));
                    dataTable.Columns.Add(seriesName + "_median", typeof(string));
                    dataTable.Columns.Add(seriesName + "_boxMaximum", typeof(string));
                    dataTable.Columns.Add(seriesName + "_upperWhisker", typeof(string));
                    dataTable.Columns.Add(seriesName + "_label", typeof(string));

                    var oxySeries = (OxyPlot.Series.BoxPlotSeries)series.InternalSeries;

                    if (oxySeries.ItemsSource != null)
                    {
                        var datalist = oxySeries.ItemsSource as IEnumerable<OxyPlot.Series.BoxPlotItem>;
                        if (datalist != null)
                        {
                            foreach (var bpi in datalist)
                            {
                                var r = dataTable.Rows.Add(dataTable.Rows.Count + 1, bpi.Position, bpi.LowerWhisker, bpi.BoxMinimum, bpi.Median, bpi.BoxMaximum, bpi.UpperWhisker);

                                // Check if a X Axis Label is specified
                                if (oxySeries.XAxis != null)
                                {
                                    if (oxySeries.XAxis.GetType() == typeof(OxyPlot.Axes.CategoryAxis))
                                    {
                                        if (((OxyPlot.Axes.CategoryAxis)oxySeries.XAxis).LabelField != null)
                                        {
                                            r[seriesName + "_label"] = ((OxyPlot.Axes.CategoryAxis)oxySeries.XAxis).LabelField;
                                        }
                                    }
                                }

                                int j = 1;
                                foreach (var outlier in bpi.Outliers)
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
                    }
                    else
                    {
                        for (int i = 0; i < oxySeries.Items.Count; i++)
                        {
                            var r = dataTable.Rows.Add(dataTable.Rows.Count + 1, oxySeries.Items[i].Position, oxySeries.Items[i].LowerWhisker, oxySeries.Items[i].BoxMinimum, oxySeries.Items[i].Median, oxySeries.Items[i].BoxMaximum, oxySeries.Items[i].UpperWhisker);

                            // Check if a X Axis Label is specified
                            if (oxySeries.XAxis != null)
                            {
                                if (oxySeries.XAxis.GetType() == typeof(OxyPlot.Axes.CategoryAxis))
                                {
                                    if (((OxyPlot.Axes.CategoryAxis)oxySeries.XAxis).LabelField != null)
                                    {
                                        r[seriesName + "_label"] = ((OxyPlot.Axes.CategoryAxis)oxySeries.XAxis).LabelField;
                                    }
                                }
                            }

                            int j = 1;
                            foreach (var outlier in oxySeries.Items[i].Outliers)
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
                }
                else if (series.GetType() == typeof(Wpf.BarSeries))
                {
                    seriesName = !string.IsNullOrEmpty(series.Title) ? series.Title : "BarSeries_" + tableCount;
                    foreach (var badChar in badCharacters)
                    {
                        seriesName = seriesName.Replace(badChar, "_");
                    }

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_categoryIndex", typeof(string));
                    dataTable.Columns.Add(seriesName + "_color", typeof(string));
                    dataTable.Columns.Add(seriesName + "_value", typeof(string));

                    var oxySeries = (OxyPlot.Series.BarSeries)series.InternalSeries;

                    if (oxySeries.ItemsSource != null)
                    {
                        var datalist = oxySeries.ItemsSource as IEnumerable<OxyPlot.Series.BarItem>;
                        if (datalist != null)
                        {
                            foreach (var seriesItem in datalist.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesItem.CategoryIndex, seriesItem.Color.GetColorName(), seriesItem.Value);
                            }
                        }
                        else
                        {
                            int c = 0;
                            foreach (var obj in oxySeries.ItemsSource.Cast<object>())
                            {
                                string colorVal = "";
                                if (oxySeries.ColorField != null)
                                {
                                    PropertyInfo propColor = obj.GetType().GetProperty(oxySeries.ColorField);
                                    colorVal = Convert.ToString(propColor.GetValue(obj, null));
                                }

                                string valueVal = "";
                                if (oxySeries.ValueField != null)
                                {
                                    PropertyInfo propValue = obj.GetType().GetProperty(oxySeries.ValueField);
                                    valueVal = Convert.ToString(propValue.GetValue(obj, null));
                                }

                                dataTable.Rows.Add(dataTable.Rows.Count + 1, c, colorVal, valueVal);
                                c++;
                            }
                        }
                    }
                    else
                    {
                        foreach (var seriesItem in oxySeries.Items)
                        {
                            dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesItem.CategoryIndex, seriesItem.Color.GetColorName(), seriesItem.Value);
                        }
                    }
                }
                else if (series.GetType() == typeof(Wpf.ColumnSeries))
                {
                    seriesName = !string.IsNullOrEmpty(series.Title) ? series.Title : "ColumnSeries_" + tableCount;
                    foreach (var badChar in badCharacters)
                    {
                        seriesName = seriesName.Replace(badChar, "_");
                    }

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_categoryIndex", typeof(string));
                    dataTable.Columns.Add(seriesName + "_color", typeof(string));
                    dataTable.Columns.Add(seriesName + "_value", typeof(string));

                    var oxySeries = (OxyPlot.Series.ColumnSeries)series.InternalSeries;

                    if (oxySeries.ItemsSource != null)
                    {
                        var datalist = oxySeries.ItemsSource as IEnumerable<OxyPlot.Series.ColumnItem>;
                        if (datalist != null)
                        {
                            foreach (var seriesItem in datalist.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesItem.CategoryIndex, seriesItem.Color.GetColorName(), seriesItem.Value);
                            }
                        }
                        else
                        {
                            int c = 0;
                            foreach (var obj in oxySeries.ItemsSource.Cast<object>())
                            {
                                string colorVal = "";
                                if (oxySeries.ColorField != null)
                                {
                                    PropertyInfo propColor = obj.GetType().GetProperty(oxySeries.ColorField);
                                    colorVal = Convert.ToString(propColor.GetValue(obj, null));
                                }

                                string valueVal = "";
                                if (oxySeries.ValueField != null)
                                {
                                    PropertyInfo propValue = obj.GetType().GetProperty(oxySeries.ValueField);
                                    valueVal = Convert.ToString(propValue.GetValue(obj, null));
                                }

                                dataTable.Rows.Add(dataTable.Rows.Count + 1, c, colorVal, valueVal);
                                c++;
                            }
                        }
                    }
                    else
                    {
                        foreach (var seriesItem in oxySeries.Items)
                        {
                            dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesItem.CategoryIndex, seriesItem.Color.GetColorName(), seriesItem.Value);
                        }
                    }
                }
                else if (series.GetType() == typeof(Wpf.HistogramSeries))
                {
                    seriesName = !string.IsNullOrEmpty(series.Title) ? series.Title : "HistogramSeries_" + tableCount;
                    foreach (var badChar in badCharacters)
                    {
                        seriesName = seriesName.Replace(badChar, "_");
                    }

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_rangeStart", typeof(string));
                    dataTable.Columns.Add(seriesName + "_rangeEnd", typeof(string));
                    dataTable.Columns.Add(seriesName + "_area", typeof(string));

                    var oxySeries = (OxyPlot.Series.HistogramSeries)series.InternalSeries;

                    if (oxySeries.ItemsSource != null)
                    {
                        var datalist = oxySeries.ItemsSource as IEnumerable<OxyPlot.Series.HistogramItem>;
                        if (datalist != null)
                        {
                            foreach (var seriesValue in datalist.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.RangeStart, seriesValue.RangeEnd, seriesValue.Area);
                            }
                        }
                    }
                    else
                    {
                        foreach (var seriesItem in oxySeries.Items)
                        {
                            dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesItem.RangeStart, seriesItem.RangeEnd, seriesItem.Area);
                        }
                    }
                }
                else if (series.GetType() == typeof(Wpf.HeatMapSeries))
                {
                    seriesName = !string.IsNullOrEmpty(series.Title) ? series.Title : "HeatMapSeries_" + tableCount;
                    foreach (var badChar in badCharacters)
                    {
                        seriesName = seriesName.Replace(badChar, "_");
                    }

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add("xy", typeof(string));

                    var oxySeries = (OxyPlot.Series.HeatMapSeries)series.InternalSeries;

                    if (oxySeries.ItemsSource != null)
                    {
                        var datalist = oxySeries.ItemsSource as IEnumerable<double[,]>;
                        if (datalist != null)
                        {
                            // Add columns
                            double x0 = oxySeries.X0;
                            double x1 = oxySeries.X1;
                            int xN = datalist.ToArray().GetLength(0) - 1;
                            double xDelta = (x1 - x0) / xN;
                            dataTable.Columns.Add(x0.ToString(), typeof(string));
                            for (int i = 1; i < datalist.ToArray().GetLength(0); i++)
                            {
                                x0 += xDelta;
                                dataTable.Columns.Add(x0.ToString(), typeof(string));
                            }

                            // Add rows
                            double y0 = oxySeries.Y0;
                            double y1 = oxySeries.Y1;
                            int yN = datalist.ToArray().GetLength(1) - 1;
                            double yDelta = (y1 - y0) / yN;
                            dataTable.Rows.Add();
                            dataTable.Rows[0][0] = 1;
                            dataTable.Rows[0][1] = y0;

                            for (int j = 1; j < datalist.ToArray().GetLength(1); j++)
                            {
                                dataTable.Rows.Add();
                                y0 += yDelta;
                                dataTable.Rows[j][0] = j + 1;
                                dataTable.Rows[j][1] = y0;
                            }

                            // Fill in matrix
                            for (int x = 0; x < datalist.ToArray().GetLength(0); x++)
                            {
                                double[,] xy = datalist.ToArray().ElementAt(x);
                                for (int y = 0; y < datalist.ToArray().GetLength(1); y++)
                                {
                                    dataTable.Rows[y][x + 2] = xy[x, y];
                                }
                            }
                        }
                    }
                    else
                    {
                        // Add columns
                        double x0 = oxySeries.X0;
                        double x1 = oxySeries.X1;
                        int xN = oxySeries.Data.GetLength(0) - 1;
                        double xDelta = (x1 - x0) / xN;
                        dataTable.Columns.Add(x0.ToString(), typeof(string));
                        for (int i = 1; i < oxySeries.Data.GetLength(0); i++)
                        {
                            x0 += xDelta;
                            dataTable.Columns.Add(x0.ToString(), typeof(string));
                        }

                        // Add rows
                        double y0 = oxySeries.Y0;
                        double y1 = oxySeries.Y1;
                        int yN = oxySeries.Data.GetLength(1) - 1;
                        double yDelta = (y1 - y0) / yN;
                        dataTable.Rows.Add();
                        dataTable.Rows[0][0] = 1;
                        dataTable.Rows[0][1] = y0;

                        for (int j = 1; j < oxySeries.Data.GetLength(1); j++)
                        {
                            dataTable.Rows.Add();
                            y0 += yDelta;
                            dataTable.Rows[j][0] = j + 1;
                            dataTable.Rows[j][1] = y0;
                        }

                        // Fill in matrix
                        for (int x = 0; x < oxySeries.Data.GetLength(0); x++)
                        {
                            for (int y = 0; y < oxySeries.Data.GetLength(1); y++)
                            {
                                dataTable.Rows[y][x + 2] = oxySeries.Data[x, y];
                            }
                        }
                    }
                }
                else if (series.GetType() == typeof(Wpf.ScatterErrorSeries))
                {
                    seriesName = !string.IsNullOrEmpty(series.Title) ? series.Title : "ScatterErrorSeries_" + tableCount;
                    foreach (var badChar in badCharacters)
                    {
                        seriesName = seriesName.Replace(badChar, "_");
                    }

                    dataTable.TableName = seriesName;
                    dataTable.Columns.Add("id", typeof(int));
                    dataTable.Columns.Add(seriesName + "_xLower", typeof(string));
                    dataTable.Columns.Add(seriesName + "_x", typeof(string));
                    dataTable.Columns.Add(seriesName + "_xUpper", typeof(string));
                    dataTable.Columns.Add(seriesName + "_yLower", typeof(string));
                    dataTable.Columns.Add(seriesName + "_y", typeof(string));
                    dataTable.Columns.Add(seriesName + "_yUpper", typeof(string));

                    var oxySeries = (OxyPlot.Series.ScatterErrorSeries)series.InternalSeries;
                    var wpfSeries = (Wpf.ScatterErrorSeries)series;

                    if (oxySeries.ItemsSource != null)
                    {
                        var datalist = oxySeries.ItemsSource as IEnumerable<OxyPlot.Series.ScatterPoint>;
                        if (datalist != null)
                        {
                            foreach (OxyPlot.Series.ScatterErrorPoint seriesValue in datalist.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.LowerErrorX, seriesValue.X, seriesValue.UpperErrorX, seriesValue.LowerErrorY, seriesValue.Y, seriesValue.UpperErrorY);
                            }
                        }
                        else
                        {
                            foreach (var obj in oxySeries.ItemsSource.Cast<object>())
                            {
                                PropertyInfo propX = obj.GetType().GetProperty(oxySeries.DataFieldX);
                                string xVal = Convert.ToString(propX.GetValue(obj, null));

                                PropertyInfo propY = obj.GetType().GetProperty(oxySeries.DataFieldY);
                                string yVal = Convert.ToString(propY.GetValue(obj, null));

                                string xLower = "";
                                if (wpfSeries.DataFieldLowerErrorX != null)
                                {
                                    PropertyInfo propXlower = obj.GetType().GetProperty(wpfSeries.DataFieldLowerErrorX);
                                    xLower = Convert.ToString(propXlower.GetValue(obj, null));
                                }

                                string xUpper = "";
                                if (wpfSeries.DataFieldUpperErrorX != null)
                                {
                                    PropertyInfo propXupper = obj.GetType().GetProperty(wpfSeries.DataFieldUpperErrorX);
                                    xUpper = Convert.ToString(propXupper.GetValue(obj, null));
                                }

                                string yLower = "";
                                if (wpfSeries.DataFieldLowerErrorY != null)
                                {
                                    PropertyInfo propYlower = obj.GetType().GetProperty(wpfSeries.DataFieldLowerErrorY);
                                    yLower = Convert.ToString(propYlower.GetValue(obj, null));
                                }

                                string yUpper = "";
                                if (wpfSeries.DataFieldUpperErrorY != null)
                                {
                                    PropertyInfo propYupper = obj.GetType().GetProperty(wpfSeries.DataFieldUpperErrorY);
                                    yUpper = Convert.ToString(propYupper.GetValue(obj, null));
                                }

                                dataTable.Rows.Add(dataTable.Rows.Count + 1, xLower, xVal, xUpper, yLower, yVal, yUpper);
                            }
                        }
                    }
                    else
                    {
                        foreach (var seriesValue in oxySeries.Points)
                        {
                            dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.LowerErrorX, seriesValue.X, seriesValue.UpperErrorX, seriesValue.LowerErrorY, seriesValue.Y, seriesValue.UpperErrorY);
                        }
                    }
                }

                // Check for item source on a X category axis
                var xCat = ((OxyPlot.Series.XYAxisSeries)series.InternalSeries).XAxis as OxyPlot.Axes.CategoryAxis;
                if (xCat != null)
                {
                    dataTable.Columns.Add("Xcategory", typeof(string));

                    if (xCat.ItemsSource != null)
                    {
                        var xCatList = xCat.ItemsSource as IEnumerable<string>;
                        if (xCatList != null)
                        {
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                dataTable.Rows[i]["Xcategory"] = xCatList.ElementAt(i);
                            }
                        }
                        else
                        {
                            int i = 0;
                            foreach (var obj in xCat.ItemsSource.Cast<object>())
                            {
                                PropertyInfo propLabel = obj.GetType().GetProperty(xCat.LabelField);
                                string xVal = Convert.ToString(propLabel.GetValue(obj, null));
                                dataTable.Rows[i]["Xcategory"] = xVal;
                                i++;
                            }
                        }
                    }
                }
                else
                {
                    // Check for item source on a Y category axis
                    var yCat = ((OxyPlot.Series.XYAxisSeries)series.InternalSeries).YAxis as OxyPlot.Axes.CategoryAxis;
                    if (yCat != null)
                    {
                        dataTable.Columns.Add("Ycategory", typeof(string));

                        if (yCat.ItemsSource != null)
                        {
                            var yCatList = yCat.ItemsSource as IEnumerable<string>;
                            if (yCatList != null)
                            {
                                for (int i = 0; i < dataTable.Rows.Count; i++)
                                {
                                    dataTable.Rows[i]["Ycategory"] = yCatList.ElementAt(i);
                                }
                            }
                            else
                            {
                                int i = 0;
                                foreach (var obj in yCat.ItemsSource.Cast<object>())
                                {
                                    PropertyInfo propLabel = obj.GetType().GetProperty(yCat.LabelField);
                                    string yVal = Convert.ToString(propLabel.GetValue(obj, null));
                                    dataTable.Rows[i]["Ycategory"] = yVal;
                                    i++;
                                }
                            }
                        }
                    }
                }

                // Skip contour series for now
                if (series.GetType() == typeof(Wpf.ContourSeries)) continue;

                tableList.Add(dataTable);
            }

            // Show save dialog
            string filters = "comma delimited(*.csv) |*.csv|Excel(*.xlsx) |*.xlsx|Sqlite(*.sqlite) |*.sqlite";
            try
            {
                var saveFileBrowser = new Microsoft.Win32.SaveFileDialog { Filter = filters, FilterIndex = 1 };
                if (saveFileBrowser.ShowDialog() == true)
                {
                    string extension = System.IO.Path.GetExtension(saveFileBrowser.FileName);
                    switch (extension)
                    {
                        case ".csv":
                            // Combine all tables because CSVs only have one sheet/table
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
                                            theDT.Columns[theDT.Columns[i].ColumnName].ColumnName = theDT.Columns[i].ColumnName + "_" + uniqueCount;
                                        }
                                        colNames.Add(theDT.Columns[i].ColumnName);
                                    }
                                }
                            }

                            var totalDT = new DataTable("Exported_Data");
                            totalDT = MergeAll(tableList, "id");

                            var csvDataView = new DatabaseManager.InMemoryReader(totalDT).GetTableManager(totalDT.TableName);
                            csvDataView.ExportToCsv(saveFileBrowser.FileName);
                            break;

                        case ".xlsx":
                            // Save each DT to the file (as new sheet)
                            foreach (var dt in tableList)
                            {
                                var xlsxDataView = new DatabaseManager.InMemoryReader(dt).GetTableManager(dt.TableName);
                                if (xlsxDataView == null) continue;
                                xlsxDataView.ExportToXlsx(saveFileBrowser.FileName);
                            }
                            break;

                        case ".sqlite":
                            // Save each DT to the file (as new table)
                            foreach (var dt in tableList)
                            {
                                var sqliteDataView = new DatabaseManager.InMemoryReader(dt).GetTableManager(dt.TableName);
                                if (sqliteDataView == null) continue;
                                sqliteDataView.ExportToSqlite(saveFileBrowser.FileName, sqliteDataView.TableName);
                            }
                            break;

                        default:
                            throw new Exception("selected file format extension '" + System.IO.Path.GetExtension(saveFileBrowser.FileName) + "' is not supported for export.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Merge all data tables by a primary key column.
        /// </summary>
        public DataTable MergeAll(IList<DataTable> tables, string primaryKeyColumn)
        {
            if (!tables.Any()) throw new ArgumentException("Tables must not be empty", nameof(tables));

            if (primaryKeyColumn != null)
            {
                foreach (var t in tables)
                {
                    if (!t.Columns.Contains(primaryKeyColumn))
                        throw new ArgumentException("All tables must have the specified primarykey column " + primaryKeyColumn, nameof(primaryKeyColumn));
                }
            }

            if (tables.Count == 1) return tables[0];

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
                    DataRow firstRow = grpDup.First();

                    foreach (DataColumn c in table.Columns)
                    {
                        if (firstRow.IsNull(c))
                        {
                            DataRow firstNotNullRow = grpDup.Skip(1).FirstOrDefault(r => !r.IsNull(c));
                            if (firstNotNullRow != null) firstRow[c] = firstNotNullRow[c];
                        }
                    }

                    var rowsToRemove = grpDup.Skip(1).ToList();
                    foreach (DataRow rowToRemove in rowsToRemove)
                    {
                        table.Rows.Remove(rowToRemove);
                    }
                }
            }

            return table;
        }

        /// <summary>
        /// Merge two data tables by index (row-wise).
        /// </summary>
        public DataTable MergeTablesByIndex(DataTable t1, DataTable t2)
        {
            if (t1 == null || t2 == null) return null;
            var t3 = t1.Clone();

            foreach (DataColumn col in t2.Columns)
            {
                string newColumnName = col.ColumnName;
                int colNum = 1;

                while (t3.Columns.Contains(newColumnName))
                {
                    newColumnName = string.Format("{0}_{1}", col.ColumnName, System.Threading.Interlocked.Increment(ref colNum));
                }

                t3.Columns.Add(newColumnName, col.DataType);
            }

            var mergedRows = t1.AsEnumerable().Zip(t2.AsEnumerable(), (r1, r2) => r1.ItemArray.Concat(r2.ItemArray).ToArray());

            foreach (var rowFields in mergedRows)
            {
                t3.Rows.Add(rowFields);
            }

            return t3;
        }

        #endregion

        #region Save Plot

        /// <summary>
        /// On Click, open the save plot image dialog.
        /// </summary>
        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Plot == null) return;
            var saveImageDialog = new SavePlotImageDialog(Plot) { Owner = Window.GetWindow(this) };
            saveImageDialog.ShowDialog();
        }

        #endregion

        #region Properties and Swap Axes

        /// <summary>
        /// Open plot properties.
        /// </summary>
        private void PropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            PropertiesCalled?.Invoke(Plot, true, null, null);
        }

        /// <summary>
        /// Swap the X and Y axes.
        /// </summary>
        private void SwapAxesButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var s in Plot.Series)
            {
                if (_nonSwapSeriesTypes.Contains(s.GetType())) return;
            }

            foreach (var axis in Plot.Axes)
            {
                if (axis.Position == OxyPlot.Axes.AxisPosition.Bottom)
                {
                    axis.Position = OxyPlot.Axes.AxisPosition.Left;
                }
                else if (axis.Position == OxyPlot.Axes.AxisPosition.Left)
                {
                    axis.Position = OxyPlot.Axes.AxisPosition.Bottom;
                }
            }

            foreach (var s in Plot.Series)
            {
                if (typeof(Wpf.DataPointSeries).IsAssignableFrom(s.GetType()))
                {
                    SwapDataPointSeries((Wpf.DataPointSeries)s);
                }
                else if (s.GetType() == typeof(Wpf.ScatterPointSeries))
                {
                    SwapScatterSeries((Wpf.ScatterPointSeries)s);
                }
                else if (s.GetType() == typeof(Wpf.ScatterErrorSeries))
                {
                    SwapScatterErrorSeries((Wpf.ScatterErrorSeries)s);
                }
                else if (s.GetType() == typeof(Wpf.BoxPlotSeries))
                {
                    foreach (var axis in Plot.Axes)
                    {
                        if (axis.GetType() == typeof(Wpf.CategoryAxis))
                        {
                            ((Wpf.BoxPlotSeries)s).IsVertical = axis.Position == OxyPlot.Axes.AxisPosition.Bottom;
                        }
                    }
                }
            }

            Plot.InvalidatePlot(true);
        }

        private void SwapDataPointSeries(Wpf.DataPointSeries dps)
        {
            if (dps == null) return;
            if (dps.ItemsSource == null)
            {
                SwapDataPoints((OxyPlot.Series.DataPointSeries)dps.InternalSeries);
            }
            else
            {
                if (dps.DataFieldX == null && dps.DataFieldY == null)
                {
                    dps.DataFieldX = "Y";
                    dps.DataFieldY = "X";
                }
                else
                {
                    string dfx = dps.DataFieldX;
                    dps.DataFieldX = dps.DataFieldY;
                    dps.DataFieldY = dfx;
                }

                if (dps.GetType() == typeof(Wpf.AreaSeries))
                {
                    var areaSeries = (Wpf.AreaSeries)dps;
                    string dfx2 = areaSeries.DataFieldX2;
                    areaSeries.DataFieldX2 = areaSeries.DataFieldY2;
                    areaSeries.DataFieldY2 = dfx2;
                }
            }
        }

        private void SwapDataPoints(OxyPlot.Series.DataPointSeries dps)
        {
            if (dps?.Points == null || dps.Points.Count == 0) return;
            var pnts = dps.Points.ToArray();
            dps.Points.Clear();
            foreach (var p in pnts)
            {
                dps.Points.Add(new DataPoint(p.Y, p.X));
            }
        }

        private void SwapScatterSeries(Wpf.ScatterSeries<OxyPlot.Series.ScatterPoint> sps)
        {
            if (sps == null) return;
            if (sps.ItemsSource == null)
            {
                SwapScatterPoints((OxyPlot.Series.ScatterSeries)sps.InternalSeries);
            }
            else
            {
                string dfx = sps.DataFieldX;
                sps.DataFieldX = sps.DataFieldY;
                sps.DataFieldY = dfx;
            }
        }

        private void SwapScatterPoints(OxyPlot.Series.ScatterSeries dps)
        {
            if (dps?.Points == null || dps.Points.Count == 0) return;
            var pnts = dps.Points.ToArray();
            dps.Points.Clear();
            foreach (var p in pnts)
            {
                dps.Points.Add(new OxyPlot.Series.ScatterPoint(p.Y, p.X, p.Size, p.Value, p.Tag));
            }
        }

        private void SwapScatterErrorSeries(Wpf.ScatterErrorSeries sps)
        {
            if (sps == null) return;
            if (sps.ItemsSource == null)
            {
                SwapScatterErrorPoints((OxyPlot.Series.ScatterErrorSeries)sps.InternalSeries);
            }
            else
            {
                string dfx = sps.DataFieldX;
                sps.DataFieldX = sps.DataFieldY;
                sps.DataFieldY = dfx;

                dfx = sps.DataFieldLowerErrorX;
                sps.DataFieldLowerErrorX = sps.DataFieldLowerErrorY;
                sps.DataFieldLowerErrorY = dfx;

                dfx = sps.DataFieldUpperErrorX;
                sps.DataFieldUpperErrorX = sps.DataFieldUpperErrorY;
                sps.DataFieldUpperErrorY = dfx;
            }
        }

        private void SwapScatterErrorPoints(OxyPlot.Series.ScatterErrorSeries dps)
        {
            if (dps?.Points == null || dps.Points.Count == 0) return;
            var pnts = dps.Points.ToArray();
            dps.Points.Clear();
            foreach (var p in pnts)
            {
                dps.Points.Add(new OxyPlot.Series.ScatterErrorPoint(p.Y, p.X, p.LowerErrorY, p.UpperErrorY, p.LowerErrorX, p.UpperErrorX, p.Size, p.Value, p.Tag));
            }
        }

        #endregion
    }
}
