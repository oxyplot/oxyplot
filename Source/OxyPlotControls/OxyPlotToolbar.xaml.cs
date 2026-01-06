using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
using OxyPlot.Annotations;
using OxyPlot.Wpf;

namespace OxyPlotControls
{
    /// <summary>
    /// OxyPlot toolbar control providing pan, zoom, annotation, and export functionality.
    /// Supports modern PlotView architecture working directly with PlotModel and core OxyPlot types.
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

            _overlayCanvas.Children.Add(_leaderLine);
        }

        #endregion

        #region Members

        /// <summary>
        /// Dependency property for the PlotView property.
        /// </summary>
        public static readonly DependencyProperty PlotViewProperty = DependencyProperty.Register(
            nameof(PlotView), typeof(PlotView), typeof(OxyPlotToolbar), new PropertyMetadata(null, InitializePlotView));

        /// <summary>
        /// Gets and sets the OxyPlot PlotView associated with this toolbar.
        /// </summary>
        public PlotView PlotView
        {
            get => (PlotView)GetValue(PlotViewProperty);
            set => SetValue(PlotViewProperty, value);
        }

        /// <summary>
        /// Gets the PlotModel from the PlotView.
        /// </summary>
        private PlotModel Model => PlotView?.Model;

        /// <summary>
        /// Property changed callback for the PlotView property.
        /// </summary>
        private static void InitializePlotView(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not OxyPlotToolbar oxyToolBar) return;

            // Remove handlers from old plot view
            if (e.OldValue is PlotView oldPlotView && oldPlotView.Model != null)
            {
                oldPlotView.Model.MouseDown -= oxyToolBar.PlotModelMouseDown;
                oldPlotView.Model.MouseMove -= oxyToolBar.PlotModelMouseMove;
                oldPlotView.Model.MouseUp -= oxyToolBar.PlotModelMouseUp;
                oldPlotView.LayoutUpdated -= oxyToolBar.ToolBarLayoutUpdated;

                // Remove overlay canvas from the old PlotView's parent grid
                if (oxyToolBar._overlayCanvas.Parent is Panel oldParent)
                {
                    oldParent.Children.Remove(oxyToolBar._overlayCanvas);
                }
            }

            // Add handlers to new plot view
            if (e.NewValue is PlotView newPlotView)
            {
                // Wait for the model to be set
                if (newPlotView.Model != null)
                {
                    oxyToolBar.SetupPlotViewHandlers(newPlotView);
                }

                // Also listen for when the model changes
                var modelDescriptor = DependencyPropertyDescriptor.FromProperty(PlotView.ModelProperty, typeof(PlotView));
                modelDescriptor?.AddValueChanged(newPlotView, (s, args) =>
                {
                    if (newPlotView.Model != null)
                    {
                        oxyToolBar.SetupPlotViewHandlers(newPlotView);
                    }
                });
            }
        }

        /// <summary>
        /// Sets up event handlers for the PlotView.
        /// </summary>
        private void SetupPlotViewHandlers(PlotView plotView)
        {
            var model = plotView.Model;
            if (model == null) return;

            // Remove any existing handlers first
            model.MouseDown -= PlotModelMouseDown;
            model.MouseMove -= PlotModelMouseMove;
            model.MouseUp -= PlotModelMouseUp;

            // Set up the mouse events
            model.MouseDown += PlotModelMouseDown;
            model.MouseMove += PlotModelMouseMove;
            model.MouseUp += PlotModelMouseUp;

            // Set up annotation collection changed handler
            if (model.Annotations is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged -= PlotModelAnnotationCollectionChanged;
                notifyCollection.CollectionChanged += PlotModelAnnotationCollectionChanged;
            }

            plotView.ApplyTemplate();

            // Define the zooming cursor
            plotView.ZoomHorizontalCursor = _zoomCursor;
            plotView.ZoomRectangleCursor = _zoomCursor;
            plotView.ZoomVerticalCursor = _zoomCursor;

            // Define the pan cursor
            plotView.PanCursor = _panHandCursor;

            // Set up the mouse bindings
            if (PointerButton.IsChecked == true) PointerButton_Click(this, new RoutedEventArgs());
            if (ZoomButton.IsChecked == true) ZoomButton_Click(this, new RoutedEventArgs());
            if (PanButton.IsChecked == true) PanButton_Click(this, new RoutedEventArgs());

            plotView.LayoutUpdated += ToolBarLayoutUpdated;

            // Add overlay canvas to the PlotView's parent
            AddOverlayCanvas(plotView);

            // Set up handlers for existing annotations
            foreach (var annotation in model.Annotations)
            {
                SetupAnnotationHandlers(annotation);
            }
        }

        /// <summary>
        /// Adds the overlay canvas to the PlotView's visual tree.
        /// </summary>
        private void AddOverlayCanvas(PlotView plotView)
        {
            // Remove from any existing parent
            if (_overlayCanvas.Parent is Panel existingParent)
            {
                existingParent.Children.Remove(_overlayCanvas);
            }

            // Find the Grid parent of the PlotView's internal structure
            plotView.ApplyTemplate();
            var grid = FindVisualChild<Grid>(plotView);
            if (grid != null)
            {
                grid.Children.Add(_overlayCanvas);
            }
        }

        /// <summary>
        /// Finds a visual child of a specified type.
        /// </summary>
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    return typedChild;
                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }

        /// <summary>
        /// Updates the toolbar margin based on plot layout.
        /// </summary>
        private void ToolBarLayoutUpdated(object sender, EventArgs eventArgs)
        {
            if (Model == null) return;

            if (!string.IsNullOrEmpty(Model.Title))
            {
                OxyToolBar.Margin = new Thickness(OxyToolBar.Margin.Left, Model.ActualPlotMargins.Top + Model.TitleArea.Bottom - Model.TitlePadding, OxyToolBar.Margin.Right, OxyToolBar.Margin.Bottom);
            }
            else
            {
                OxyToolBar.Margin = new Thickness(OxyToolBar.Margin.Left, Model.ActualPlotMargins.Top + Model.Padding.Top, OxyToolBar.Margin.Right, OxyToolBar.Margin.Bottom);
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
        private Canvas _overlayCanvas = new Canvas();
        private ScreenPoint _lastScreenPoint = ScreenPoint.Undefined;
        private bool _moveStartPoint = false;
        private bool _moveEndPoint = false;
        private int _movePointIndex = -1;
        private bool _scaleMaxX = false;
        private bool _scaleMaxY = false;
        private bool _scaleMinX = false;
        private bool _scaleMinY = false;
        private OxyColor _originalColor = OxyColors.White;

        // Adding Annotations
        private AddToolMode _addAnnotationToolMode = AddToolMode.None;
        private OxyPlot.Annotations.Annotation _targetAddAnnotation = null;

        /// <summary>
        /// Delegate for the PropertiesCalled event.
        /// </summary>
        /// <param name="targetPlotView">The PlotView whose properties need to be opened.</param>
        /// <param name="openProperties">Boolean value indicating if plot properties should be opened.</param>
        /// <param name="propertyExpander">The property expander that needs to be expanded.</param>
        /// <param name="selectedObject">The selected plot object to edit.</param>
        public delegate void PropertiesCalledEventHandler(PlotView targetPlotView, bool openProperties, OxyPlotPropertiesControl.PropertyEXP? propertyExpander, object selectedObject);

        /// <summary>
        /// Event indicating the plot properties need to be opened.
        /// </summary>
        public event PropertiesCalledEventHandler PropertiesCalled;

        // Non-swappable series types
        private static readonly HashSet<Type> _nonSwapSeriesTypes = new HashSet<Type>
        {
            typeof(OxyPlot.Series.HistogramSeries),
            typeof(OxyPlot.Series.BarSeries),
            typeof(OxyPlot.Series.LinearBarSeries),
            typeof(OxyPlot.Series.HeatMapSeries)
        };

        #endregion

        #region Pan & Zoom

        /// <summary>
        /// User clicked the pointer button.
        /// </summary>
        private void PointerButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlotView == null) return;

            var controller = PlotView.ActualController;
            controller.UnbindAll();
            controller.BindMouseDown(OxyMouseButton.Middle, OxyPlot.PlotCommands.PanAt);
            controller.BindMouseDown(OxyMouseButton.Left, OxyPlot.PlotCommands.SnapTrack);
            controller.BindMouseWheel(OxyPlot.PlotCommands.ZoomWheel);
            controller.BindKeyDown(OxyKey.Escape, OxyPlot.PlotCommands.Reset);

            SetCursor();
        }

        /// <summary>
        /// User clicked the pan button.
        /// </summary>
        private void PanButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlotView == null) return;

            var controller = PlotView.ActualController;
            controller.UnbindAll();
            controller.BindMouseDown(OxyMouseButton.Middle, OxyPlot.PlotCommands.PanAt);
            controller.BindMouseDown(OxyMouseButton.Left, OxyPlot.PlotCommands.PanAt);
            controller.BindMouseDown(OxyMouseButton.Right, OxyPlot.PlotCommands.SnapTrack);
            controller.BindMouseWheel(OxyPlot.PlotCommands.ZoomWheel);
            controller.BindKeyDown(OxyKey.Escape, OxyPlot.PlotCommands.Reset);

            SetCursor();
        }

        /// <summary>
        /// User clicked zoom button.
        /// </summary>
        private void ZoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlotView == null) return;

            var controller = PlotView.ActualController;
            controller.UnbindAll();
            controller.BindMouseDown(OxyMouseButton.Middle, OxyPlot.PlotCommands.PanAt);
            controller.BindMouseDown(OxyMouseButton.Left, OxyPlot.PlotCommands.ZoomRectangle);
            controller.BindMouseDown(OxyMouseButton.Right, OxyPlot.PlotCommands.SnapTrack);
            controller.BindMouseWheel(OxyPlot.PlotCommands.ZoomWheel);
            controller.BindKeyDown(OxyKey.Escape, OxyPlot.PlotCommands.Reset);

            SetCursor();
        }

        /// <summary>
        /// User clicked zoom to extents.
        /// </summary>
        private void ZoomAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlotView == null || Model == null) return;

            Model.ResetAllAxes();
            Model.InvalidatePlot(false);
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
                PlotView.Cursor = _addPointCursor;
            }
            else if (PanButton.IsChecked == true)
            {
                PlotView.PanCursor = _panHandCursor;
                PlotView.Cursor = _panHandCursor;
                PlotView.Cursor = _panHandCursor;
            }
            else if (PointerButton.IsChecked == true)
            {
                PlotView.Cursor = Cursors.Arrow;
                PlotView.Cursor = Cursors.Arrow;
            }
            else if (ZoomButton.IsChecked == true)
            {
                PlotView.Cursor = _zoomCursor;
                PlotView.Cursor = _zoomCursor;
            }
            else
            {
                PlotView.Cursor = Cursors.Arrow;
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
            AddAnnotationToggleButton.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Add arrow annotation.
        /// </summary>
        private void AddArrowAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            PlotView.ActualController.UnbindAll();
            _addAnnotationToolMode = AddToolMode.AddArrowAnnotation;
            SetCursor();
        }

        /// <summary>
        /// Add text annotation.
        /// </summary>
        private void AddTextAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            PlotView.ActualController.UnbindAll();
            _addAnnotationToolMode = AddToolMode.AddTextAnnotation;
            SetCursor();
        }

        /// <summary>
        /// Add vertical line annotation.
        /// </summary>
        private void AddVerticalLineAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            PlotView.ActualController.UnbindAll();
            _addAnnotationToolMode = AddToolMode.AddVerticalLineAnnotation;
            SetCursor();
        }

        /// <summary>
        /// Add horizontal line annotation.
        /// </summary>
        private void AddHorizontalLineAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            PlotView.ActualController.UnbindAll();
            _addAnnotationToolMode = AddToolMode.AddHorizontalLineAnnotation;
            SetCursor();
        }

        /// <summary>
        /// Add rectangle annotation.
        /// </summary>
        private void AddRectangleAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            PlotView.ActualController.UnbindAll();
            _addAnnotationToolMode = AddToolMode.AddRectangleAnnotation;
            SetCursor();
        }

        /// <summary>
        /// Add ellipse annotation.
        /// </summary>
        private void AddEllipseAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            PlotView.ActualController.UnbindAll();
            _addAnnotationToolMode = AddToolMode.AddEllipseAnnotation;
            SetCursor();
        }

        /// <summary>
        /// Add point annotation.
        /// </summary>
        private void AddPointAnnotationItem_Click(object sender, RoutedEventArgs e)
        {
            StopAddAnnotation();
            PlotView.ActualController.UnbindAll();
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
                    Model?.InvalidatePlot(false);
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
                if (item is OxyPlot.Annotations.Annotation annotation)
                {
                    SetupAnnotationHandlers(annotation);
                }
            }
        }

        /// <summary>
        /// Sets up mouse event handlers for an annotation.
        /// </summary>
        private void SetupAnnotationHandlers(OxyPlot.Annotations.Annotation annotation)
        {
            if (annotation is ArrowAnnotation arrowAnnotation)
            {
                SetupArrowAnnotationHandlers(arrowAnnotation);
            }
            else if (annotation is TextAnnotation textAnnotation)
            {
                SetupTextAnnotationHandlers(textAnnotation);
            }
            else if (annotation is RectangleAnnotation rectangleAnnotation)
            {
                SetupRectangleAnnotationHandlers(rectangleAnnotation);
            }
            else if (annotation is EllipseAnnotation ellipseAnnotation)
            {
                SetupEllipseAnnotationHandlers(ellipseAnnotation);
            }
            else if (annotation is PointAnnotation pointAnnotation)
            {
                SetupPointAnnotationHandlers(pointAnnotation);
            }
            else if (annotation is PolygonAnnotation polygonAnnotation)
            {
                SetupPolygonAnnotationHandlers(polygonAnnotation);
            }
            else if (annotation is PolylineAnnotation polylineAnnotation)
            {
                SetupPolylineAnnotationHandlers(polylineAnnotation);
            }
            else if (annotation is LineAnnotation lineAnnotation)
            {
                SetupLineAnnotationHandlers(lineAnnotation);
            }
        }

        private void SetupArrowAnnotationHandlers(ArrowAnnotation arrow)
        {
            arrow.MouseDown += (s, ae) =>
            {
                if (_addAnnotationToolMode != AddToolMode.None) return;
                if (ae.ChangedButton != OxyMouseButton.Left) return;

                _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                _moveStartPoint = ae.HitTestResult.Index != 2;
                _moveEndPoint = ae.HitTestResult.Index != 1;
                _originalColor = arrow.Color;
                arrow.Color = OxyColors.Red;

                GetSelectedObjects(s, ae);

                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            arrow.MouseMove += (s, ae) =>
            {
                double dx = ae.Position.X - _lastScreenPoint.X;
                double dy = ae.Position.Y - _lastScreenPoint.Y;
                var startScreenPoint = arrow.Transform(new DataPoint(arrow.StartPoint.X, arrow.StartPoint.Y));
                var endScreenPoint = arrow.Transform(new DataPoint(arrow.EndPoint.X, arrow.EndPoint.Y));

                var startDataPoint = arrow.InverseTransform(new ScreenPoint(startScreenPoint.X + dx, startScreenPoint.Y + dy));
                var endDataPoint = arrow.InverseTransform(new ScreenPoint(endScreenPoint.X + dx, endScreenPoint.Y + dy));

                if (_moveStartPoint) arrow.StartPoint = startDataPoint;
                if (_moveEndPoint) arrow.EndPoint = endDataPoint;

                _lastScreenPoint = ae.Position;
                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            arrow.MouseUp += (s, ae) =>
            {
                arrow.Color = _originalColor;
            };
        }

        private void SetupTextAnnotationHandlers(TextAnnotation text)
        {
            text.MouseDown += (s, ae) =>
            {
                if (_addAnnotationToolMode != AddToolMode.None) return;
                if (ae.ChangedButton != OxyMouseButton.Left) return;

                _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                _moveStartPoint = ae.HitTestResult.Index == 0;
                _originalColor = text.Background;
                text.Background = OxyColors.Red;

                GetSelectedObjects(s, ae);

                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            text.MouseMove += (s, ae) =>
            {
                double dx = ae.Position.X - _lastScreenPoint.X;
                double dy = ae.Position.Y - _lastScreenPoint.Y;
                var theScreenPoint = text.Transform(new DataPoint(text.TextPosition.X, text.TextPosition.Y));
                var theDataPoint = text.InverseTransform(new ScreenPoint(theScreenPoint.X + dx, theScreenPoint.Y + dy));

                if (_moveStartPoint) text.TextPosition = theDataPoint;

                _lastScreenPoint = ae.Position;
                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            text.MouseUp += (s, ae) =>
            {
                text.Background = _originalColor;
            };
        }

        private void SetupRectangleAnnotationHandlers(RectangleAnnotation rect)
        {
            rect.MouseDown += (s, ae) =>
            {
                if (_addAnnotationToolMode != AddToolMode.None) return;
                if (ae.ChangedButton != OxyMouseButton.Left) return;

                _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                var upperRight = rect.Transform(rect.MaximumX, rect.MaximumY);
                var lowerLeft = rect.Transform(rect.MinimumX, rect.MinimumY);
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

                _originalColor = rect.Fill;
                rect.Fill = OxyColors.Red;

                GetSelectedObjects(s, ae);

                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            rect.MouseMove += (s, ae) =>
            {
                double dx = ae.Position.X - _lastScreenPoint.X;
                double dy = ae.Position.Y - _lastScreenPoint.Y;
                var upperRightScreenPoint = rect.Transform(rect.MaximumX, rect.MaximumY);
                var lowerLeftScreenPoint = rect.Transform(rect.MinimumX, rect.MinimumY);

                var upperRightDataPoint = rect.InverseTransform(new ScreenPoint(upperRightScreenPoint.X + dx, upperRightScreenPoint.Y + dy));
                var lowerLeftDataPoint = rect.InverseTransform(new ScreenPoint(lowerLeftScreenPoint.X + dx, lowerLeftScreenPoint.Y + dy));

                if (_scaleMaxX) rect.MaximumX = upperRightDataPoint.X;
                if (_scaleMaxY) rect.MaximumY = upperRightDataPoint.Y;
                if (_scaleMinX) rect.MinimumX = lowerLeftDataPoint.X;
                if (_scaleMinY) rect.MinimumY = lowerLeftDataPoint.Y;

                if (_moveStartPoint)
                {
                    rect.MaximumX = upperRightDataPoint.X;
                    rect.MaximumY = upperRightDataPoint.Y;
                    rect.MinimumX = lowerLeftDataPoint.X;
                    rect.MinimumY = lowerLeftDataPoint.Y;
                }

                _lastScreenPoint = ae.Position;
                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            rect.MouseUp += (s, ae) =>
            {
                rect.Fill = _originalColor;
            };
        }

        private void SetupEllipseAnnotationHandlers(EllipseAnnotation ellipse)
        {
            ellipse.MouseDown += (s, ae) =>
            {
                if (_addAnnotationToolMode != AddToolMode.None) return;
                if (ae.ChangedButton != OxyMouseButton.Left) return;

                _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                var upperRight = ellipse.Transform(ellipse.MaximumX, ellipse.MaximumY);
                var lowerLeft = ellipse.Transform(ellipse.MinimumX, ellipse.MinimumY);
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

                _originalColor = ellipse.Fill;
                ellipse.Fill = OxyColors.Red;

                GetSelectedObjects(s, ae);

                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            ellipse.MouseMove += (s, ae) =>
            {
                double dx = ae.Position.X - _lastScreenPoint.X;
                double dy = ae.Position.Y - _lastScreenPoint.Y;
                var upperRightScreenPoint = ellipse.Transform(ellipse.MaximumX, ellipse.MaximumY);
                var lowerLeftScreenPoint = ellipse.Transform(ellipse.MinimumX, ellipse.MinimumY);

                var upperRightDataPoint = ellipse.InverseTransform(new ScreenPoint(upperRightScreenPoint.X + dx, upperRightScreenPoint.Y + dy));
                var lowerLeftDataPoint = ellipse.InverseTransform(new ScreenPoint(lowerLeftScreenPoint.X + dx, lowerLeftScreenPoint.Y + dy));

                if (_scaleMaxX) ellipse.MaximumX = upperRightDataPoint.X;
                if (_scaleMaxY) ellipse.MaximumY = upperRightDataPoint.Y;
                if (_scaleMinX) ellipse.MinimumX = lowerLeftDataPoint.X;
                if (_scaleMinY) ellipse.MinimumY = lowerLeftDataPoint.Y;

                if (_moveStartPoint)
                {
                    ellipse.MaximumX = upperRightDataPoint.X;
                    ellipse.MaximumY = upperRightDataPoint.Y;
                    ellipse.MinimumX = lowerLeftDataPoint.X;
                    ellipse.MinimumY = lowerLeftDataPoint.Y;
                }

                _lastScreenPoint = ae.Position;
                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            ellipse.MouseUp += (s, ae) =>
            {
                ellipse.Fill = _originalColor;
            };
        }

        private void SetupPointAnnotationHandlers(PointAnnotation point)
        {
            point.MouseDown += (s, ae) =>
            {
                if (_addAnnotationToolMode != AddToolMode.None) return;
                if (ae.ChangedButton != OxyMouseButton.Left) return;

                _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                _moveStartPoint = ae.HitTestResult.Index == 0;
                _originalColor = point.Fill;
                point.Fill = OxyColors.Red;

                GetSelectedObjects(s, ae);

                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            point.MouseMove += (s, ae) =>
            {
                double dx = ae.Position.X - _lastScreenPoint.X;
                double dy = ae.Position.Y - _lastScreenPoint.Y;
                var theScreenPoint = point.Transform(new DataPoint(point.X, point.Y));
                var theDataPoint = point.InverseTransform(new ScreenPoint(theScreenPoint.X + dx, theScreenPoint.Y + dy));

                if (_moveStartPoint)
                {
                    point.X = theDataPoint.X;
                    point.Y = theDataPoint.Y;
                }

                _lastScreenPoint = ae.Position;
                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            point.MouseUp += (s, ae) =>
            {
                point.Fill = _originalColor;
            };
        }

        private void SetupPolygonAnnotationHandlers(PolygonAnnotation polygon)
        {
            polygon.MouseDown += (s, ae) =>
            {
                if (_addAnnotationToolMode != AddToolMode.None) return;
                if (ae.ChangedButton != OxyMouseButton.Left) return;

                _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                var screenToData = polygon.InverseTransform(ae.Position);
                var screen2ToData = polygon.InverseTransform(new ScreenPoint(ae.Position.X - 10, ae.Position.Y - 10));
                var dxy = new DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y));
                var dPoint = polygon.InverseTransform(ae.Position);

                _movePointIndex = -1;
                for (int i = 0; i < polygon.Points.Count; i++)
                {
                    if (Math.Abs(dPoint.X - polygon.Points[i].X) < dxy.X && Math.Abs(dPoint.Y - polygon.Points[i].Y) < dxy.Y)
                    {
                        _movePointIndex = i;
                        break;
                    }
                }

                if (_movePointIndex == -1)
                {
                    bool onLine = false;
                    for (int i = 0; i < polygon.Points.Count - 1; i++)
                    {
                        var p1 = polygon.Transform(polygon.Points[i]);
                        var p2 = polygon.Transform(polygon.Points[i + 1]);
                        var linePoint = ScreenPointHelper.FindPointOnLine(ae.Position, p1, p2);
                        if ((linePoint - ae.Position).Length < 10)
                        {
                            polygon.Points.Insert(i + 1, polygon.InverseTransform(linePoint));
                            onLine = true;
                            _movePointIndex = i + 1;
                            break;
                        }
                    }

                    if (!onLine)
                    {
                        var p1 = polygon.Transform(polygon.Points[0]);
                        var p2 = polygon.Transform(polygon.Points[polygon.Points.Count - 1]);
                        var linePoint = ScreenPointHelper.FindPointOnLine(ae.Position, p1, p2);
                        if ((linePoint - ae.Position).Length < 10)
                        {
                            polygon.Points.Add(polygon.InverseTransform(linePoint));
                            _movePointIndex = polygon.Points.Count - 1;
                        }
                        else
                        {
                            if (ae.HitTestResult.Index == 0) _moveStartPoint = true;
                        }
                    }
                }

                _originalColor = polygon.Fill;
                polygon.Fill = OxyColors.Red;

                GetSelectedObjects(s, ae);

                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            polygon.MouseMove += (s, ae) =>
            {
                double dx = ae.Position.X - _lastScreenPoint.X;
                double dy = ae.Position.Y - _lastScreenPoint.Y;

                if (_movePointIndex > -1)
                {
                    var screenPoint = polygon.Transform(new DataPoint(polygon.Points[_movePointIndex].X, polygon.Points[_movePointIndex].Y));
                    polygon.Points[_movePointIndex] = polygon.InverseTransform(new ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy));
                }
                else if (_moveStartPoint)
                {
                    for (int i = 0; i < polygon.Points.Count; i++)
                    {
                        var screenPoint = polygon.Transform(new DataPoint(polygon.Points[i].X, polygon.Points[i].Y));
                        polygon.Points[i] = polygon.InverseTransform(new ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy));
                    }
                }

                _lastScreenPoint = ae.Position;
                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            polygon.MouseUp += (s, ae) =>
            {
                polygon.Fill = _originalColor;
            };
        }

        private void SetupPolylineAnnotationHandlers(PolylineAnnotation polyline)
        {
            polyline.MouseDown += (s, ae) =>
            {
                if (_addAnnotationToolMode != AddToolMode.None) return;
                if (ae.ChangedButton != OxyMouseButton.Left) return;

                _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                var screenToData = polyline.InverseTransform(ae.Position);
                var screen2ToData = polyline.InverseTransform(new ScreenPoint(ae.Position.X - 10, ae.Position.Y - 10));
                var dxy = new DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y));
                var dPoint = polyline.InverseTransform(ae.Position);

                _movePointIndex = -1;
                for (int i = 0; i < polyline.Points.Count; i++)
                {
                    if (Math.Abs(dPoint.X - polyline.Points[i].X) < dxy.X && Math.Abs(dPoint.Y - polyline.Points[i].Y) < dxy.Y)
                    {
                        _movePointIndex = i;
                        break;
                    }
                }

                bool onLine = false;
                if (_movePointIndex == -1 && ae.IsControlDown)
                {
                    for (int i = 0; i < polyline.Points.Count - 1; i++)
                    {
                        var p1 = polyline.Transform(polyline.Points[i]);
                        var p2 = polyline.Transform(polyline.Points[i + 1]);
                        var linePoint = ScreenPointHelper.FindPointOnLine(ae.Position, p1, p2);
                        if ((linePoint - ae.Position).Length < 10)
                        {
                            polyline.Points.Insert(i + 1, polyline.InverseTransform(linePoint));
                            onLine = true;
                            _movePointIndex = i + 1;
                            break;
                        }
                    }
                }

                if (!onLine) _moveStartPoint = true;

                _originalColor = polyline.Color;
                polyline.Color = OxyColors.Red;

                GetSelectedObjects(s, ae);

                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            polyline.MouseMove += (s, ae) =>
            {
                double dx = ae.Position.X - _lastScreenPoint.X;
                double dy = ae.Position.Y - _lastScreenPoint.Y;

                if (_movePointIndex > -1)
                {
                    var screenPoint = polyline.Transform(new DataPoint(polyline.Points[_movePointIndex].X, polyline.Points[_movePointIndex].Y));
                    polyline.Points[_movePointIndex] = polyline.InverseTransform(new ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy));
                }
                else if (_moveStartPoint)
                {
                    for (int i = 0; i < polyline.Points.Count; i++)
                    {
                        var screenPoint = polyline.Transform(new DataPoint(polyline.Points[i].X, polyline.Points[i].Y));
                        polyline.Points[i] = polyline.InverseTransform(new ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy));
                    }
                }

                _lastScreenPoint = ae.Position;
                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            polyline.MouseUp += (s, ae) =>
            {
                polyline.Color = _originalColor;
            };
        }

        private void SetupLineAnnotationHandlers(LineAnnotation line)
        {
            line.MouseDown += (s, ae) =>
            {
                if (_addAnnotationToolMode != AddToolMode.None) return;
                if (ae.ChangedButton != OxyMouseButton.Left) return;

                _lastScreenPoint = new ScreenPoint(ae.Position.X, ae.Position.Y);
                _moveStartPoint = ae.HitTestResult.Index == 0;

                _originalColor = line.Color;
                line.Color = OxyColors.Red;

                GetSelectedObjects(s, ae);

                if ((Mouse.LeftButton == MouseButtonState.Pressed && PanButton.IsChecked == true) || Mouse.MiddleButton == MouseButtonState.Pressed)
                {
                    PlotView.PanCursor = _panHandClosedCursor;
                    PlotView.Cursor = _panHandClosedCursor;
                    PlotView.Cursor = _panHandClosedCursor;
                }

                OpenLineAnnotationTooltip(line);
                UpdateLineAnnotationTooltip(line);
                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            line.MouseMove += (s, ae) =>
            {
                double dx = ae.Position.X - _lastScreenPoint.X;
                double dy = ae.Position.Y - _lastScreenPoint.Y;
                var screenPoint = line.Transform(new DataPoint(line.X, line.Y));
                var dataPoint = line.InverseTransform(new ScreenPoint(screenPoint.X + dx, screenPoint.Y + dy));

                if (_moveStartPoint)
                {
                    if (line.Type == LineAnnotationType.LinearEquation)
                    {
                        dx = dataPoint.X - line.X;
                        dy = dataPoint.Y - line.Y;
                        line.Intercept += (dy - line.Slope * dx);
                    }
                    else
                    {
                        line.X = dataPoint.X;
                        line.Y = dataPoint.Y;
                    }
                    UpdateLineAnnotationTooltip(line);
                }

                _lastScreenPoint = ae.Position;
                Model?.InvalidatePlot(false);
                ae.Handled = true;
            };

            line.MouseUp += (s, ae) =>
            {
                line.Color = _originalColor;
                CloseLineAnnotationTooltip(line);
            };
        }

        /// <summary>
        /// Open the line annotation tooltip.
        /// </summary>
        private void OpenLineAnnotationTooltip(LineAnnotation lineAnnotation)
        {
            if (lineAnnotation.Tag is ToolTip existingTooltip)
            {
                existingTooltip.IsOpen = false;
            }

            var toolTip = new ToolTip
            {
                FontFamily = PlotView.FontFamily,
                FontSize = PlotView.FontSize,
                FontWeight = PlotView.FontWeight,
                Background = Brushes.White,
                BorderBrush = Brushes.Transparent,
                Placement = PlacementMode.Relative,
                PlacementTarget = PlotView,
                Padding = new Thickness(1),
                Margin = new Thickness(0),
                IsOpen = true
            };
            lineAnnotation.Tag = toolTip;
        }

        /// <summary>
        /// Update the line annotation tooltip.
        /// </summary>
        private void UpdateLineAnnotationTooltip(LineAnnotation lineAnnotation)
        {
            if (lineAnnotation.Tag is not ToolTip toolTip) return;

            switch (lineAnnotation.Type)
            {
                case LineAnnotationType.Horizontal:
                    {
                        DataPoint dataPoint;
                        if (!lineAnnotation.XAxis.IsReversed)
                        {
                            dataPoint = new DataPoint(lineAnnotation.XAxis.ActualMinimum, lineAnnotation.Y);
                        }
                        else
                        {
                            dataPoint = new DataPoint(lineAnnotation.XAxis.ActualMaximum, lineAnnotation.Y);
                        }
                        toolTip.Content = lineAnnotation.YAxis.FormatValue(lineAnnotation.Y);
                        toolTip.UpdateLayout();
                        toolTip.VerticalOffset = lineAnnotation.Transform(dataPoint).Y - toolTip.ActualHeight / 2;
                        toolTip.HorizontalOffset = lineAnnotation.Transform(dataPoint).X - toolTip.ActualWidth;
                    }
                    break;

                case LineAnnotationType.Vertical:
                    {
                        DataPoint dataPoint;
                        if (!lineAnnotation.YAxis.IsReversed)
                        {
                            dataPoint = new DataPoint(lineAnnotation.X, lineAnnotation.YAxis.ActualMinimum);
                        }
                        else
                        {
                            dataPoint = new DataPoint(lineAnnotation.X, lineAnnotation.YAxis.ActualMaximum);
                        }
                        toolTip.Content = lineAnnotation.XAxis.FormatValue(lineAnnotation.X);
                        toolTip.UpdateLayout();
                        toolTip.VerticalOffset = lineAnnotation.Transform(dataPoint).Y;
                        toolTip.HorizontalOffset = lineAnnotation.Transform(dataPoint).X - toolTip.ActualWidth / 2;
                    }
                    break;
            }
        }

        /// <summary>
        /// Close the line annotation tooltip.
        /// </summary>
        private void CloseLineAnnotationTooltip(LineAnnotation lineAnnotation)
        {
            if (lineAnnotation.Tag is ToolTip toolTip)
            {
                toolTip.IsOpen = false;
                lineAnnotation.Tag = null;
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
                PlotView.PanCursor = _panHandClosedCursor;
                PlotView.Cursor = _panHandClosedCursor;
                PlotView.Cursor = _panHandClosedCursor;
            }

            if (_addAnnotationToolMode != AddToolMode.None && Model != null)
            {
                HandleAddAnnotationMouseDown(e);
                return;
            }

            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                PlotView.Cursor = Cursors.Arrow;
                PlotView.Cursor = Cursors.Arrow;
                GetSelectedObjects(sender, e);
                return;
            }
            else
            {
                GetSelectedObjects(sender, e);
            }

            if (PanButton.IsChecked == true || Mouse.MiddleButton == MouseButtonState.Pressed)
            {
                PlotView.PanCursor = _panHandClosedCursor;
                PlotView.Cursor = _panHandClosedCursor;
                PlotView.Cursor = _panHandClosedCursor;
            }
        }

        /// <summary>
        /// Handles mouse down events when adding annotations.
        /// </summary>
        private void HandleAddAnnotationMouseDown(OxyMouseDownEventArgs e)
        {
            switch (_addAnnotationToolMode)
            {
                case AddToolMode.AddArrowAnnotation:
                    var newArrow = new ArrowAnnotation { Text = "Arrow Annotation" };
                    newArrow.StartPoint = ConvertScreenPointToDataPoint(e.Position);
                    newArrow.EndPoint = newArrow.StartPoint;
                    Model.Annotations.Add(newArrow);
                    SetupAnnotationHandlers(newArrow);
                    Model.InvalidatePlot(false);
                    PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newArrow);
                    _targetAddAnnotation = newArrow;
                    break;

                case AddToolMode.AddTextAnnotation:
                    var newText = new TextAnnotation { Text = "Text Annotation" };
                    newText.TextPosition = ConvertScreenPointToDataPoint(e.Position);
                    Model.Annotations.Add(newText);
                    SetupAnnotationHandlers(newText);
                    Model.InvalidatePlot(false);
                    PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newText);
                    _targetAddAnnotation = newText;
                    break;

                case AddToolMode.AddVerticalLineAnnotation:
                    var newVLine = new LineAnnotation { Text = "Vertical Line Annotation", Type = LineAnnotationType.Vertical };
                    var vDataPoint = ConvertScreenPointToDataPoint(e.Position);
                    newVLine.X = vDataPoint.X;
                    newVLine.Y = vDataPoint.Y;
                    Model.Annotations.Add(newVLine);
                    SetupAnnotationHandlers(newVLine);
                    Model.InvalidatePlot(false);
                    PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newVLine);
                    OpenLineAnnotationTooltip(newVLine);
                    UpdateLineAnnotationTooltip(newVLine);
                    _targetAddAnnotation = newVLine;
                    break;

                case AddToolMode.AddHorizontalLineAnnotation:
                    var newHLine = new LineAnnotation { Text = "Horizontal Line Annotation", Type = LineAnnotationType.Horizontal };
                    var hDataPoint = ConvertScreenPointToDataPoint(e.Position);
                    newHLine.X = hDataPoint.X;
                    newHLine.Y = hDataPoint.Y;
                    Model.Annotations.Add(newHLine);
                    SetupAnnotationHandlers(newHLine);
                    Model.InvalidatePlot(false);
                    PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newHLine);
                    OpenLineAnnotationTooltip(newHLine);
                    UpdateLineAnnotationTooltip(newHLine);
                    _targetAddAnnotation = newHLine;
                    break;

                case AddToolMode.AddRectangleAnnotation:
                    var newRectangle = new RectangleAnnotation { Text = "Rectangle Annotation" };
                    var rectDataPoint = ConvertScreenPointToDataPoint(e.Position);
                    newRectangle.MinimumX = rectDataPoint.X;
                    newRectangle.MaximumX = rectDataPoint.X;
                    newRectangle.MinimumY = rectDataPoint.Y;
                    newRectangle.MaximumY = rectDataPoint.Y;
                    Model.Annotations.Add(newRectangle);
                    SetupAnnotationHandlers(newRectangle);
                    Model.InvalidatePlot(false);
                    PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newRectangle);
                    _targetAddAnnotation = newRectangle;
                    break;

                case AddToolMode.AddEllipseAnnotation:
                    var newEllipse = new EllipseAnnotation { Text = "Ellipse Annotation" };
                    var ellipseDataPoint = ConvertScreenPointToDataPoint(e.Position);
                    newEllipse.MinimumX = ellipseDataPoint.X;
                    newEllipse.MaximumX = ellipseDataPoint.X;
                    newEllipse.MinimumY = ellipseDataPoint.Y;
                    newEllipse.MaximumY = ellipseDataPoint.Y;
                    Model.Annotations.Add(newEllipse);
                    SetupAnnotationHandlers(newEllipse);
                    Model.InvalidatePlot(false);
                    PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newEllipse);
                    _targetAddAnnotation = newEllipse;
                    break;

                case AddToolMode.AddPointAnnotation:
                    var newPoint = new PointAnnotation { Text = "Point Annotation", Size = 5 };
                    var pointDataPoint = ConvertScreenPointToDataPoint(e.Position);
                    newPoint.X = pointDataPoint.X;
                    newPoint.Y = pointDataPoint.Y;
                    Model.Annotations.Add(newPoint);
                    SetupAnnotationHandlers(newPoint);
                    Model.InvalidatePlot(false);
                    PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newPoint);
                    _targetAddAnnotation = newPoint;
                    break;

                case AddToolMode.AddPolygonAnnotation:
                    if (_targetAddAnnotation == null)
                    {
                        var newPolygon = new PolygonAnnotation { Text = "Polygon Annotation" };
                        newPolygon.Points = new List<DataPoint>();
                        var dataPointClicked = ConvertScreenPointToDataPoint(e.Position);
                        newPolygon.Points.Add(dataPointClicked);
                        _leaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
                        _leaderLine.Points.Add(new Point(e.Position.X, e.Position.Y));
                        _targetAddAnnotation = newPolygon;
                    }
                    else
                    {
                        _doubleClicked = e.ClickCount > 1;
                        var polyAnnotation = (PolygonAnnotation)_targetAddAnnotation;
                        if (polyAnnotation.Points.Count == 3)
                        {
                            Model.Annotations.Add(polyAnnotation);
                            SetupAnnotationHandlers(polyAnnotation);
                            PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, polyAnnotation);
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
                        var newPolyline = new PolylineAnnotation { Text = "Polyline Annotation" };
                        newPolyline.Points = new List<DataPoint>();
                        Model.Annotations.Add(newPolyline);
                        SetupAnnotationHandlers(newPolyline);
                        PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, newPolyline);
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
                            ((PolylineAnnotation)_targetAddAnnotation).Points.Add(ConvertScreenPointToDataPoint(e.Position));
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Plot model mouse move.
        /// </summary>
        private void PlotModelMouseMove(object sender, OxyMouseEventArgs e)
        {
            if (Model == null) return;

            // For adding annotations
            if (_addAnnotationToolMode != AddToolMode.None && _targetAddAnnotation != null)
            {
                HandleAddAnnotationMouseMove(e);
                return;
            }

            // Cursor update logic
            bool requiresRedraw = _showPoints;
            _showPoints = false;
            Cursor updatedCursor = null;

            foreach (var annotation in Model.Annotations)
            {
                var ht = annotation.HitTest(new HitTestArguments(e.Position, 10));
                if (ht == null) continue;

                if (annotation is ArrowAnnotation arrowAnnotation)
                {
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
                else if (annotation is TextAnnotation)
                {
                    if (ht.Index == 0) updatedCursor = Cursors.SizeAll;
                }
                else if (annotation is RectangleAnnotation rAnnotation)
                {
                    var ur = rAnnotation.Transform(Math.Max(rAnnotation.MaximumX, rAnnotation.MinimumX), Math.Max(rAnnotation.MaximumY, rAnnotation.MinimumY));
                    var ll = rAnnotation.Transform(Math.Min(rAnnotation.MinimumX, rAnnotation.MaximumX), Math.Min(rAnnotation.MinimumY, rAnnotation.MaximumY));

                    var topRight = new ScreenPoint(Math.Abs(ur.X - e.Position.X), Math.Abs(ur.Y - e.Position.Y));
                    var bottomLeft = new ScreenPoint(Math.Abs(ll.X - e.Position.X), Math.Abs(ll.Y - e.Position.Y));

                    if (topRight.X < 10 && topRight.Y < 10) { updatedCursor = Cursors.SizeNESW; continue; }
                    if (bottomLeft.X < 10 && bottomLeft.Y < 10) { updatedCursor = Cursors.SizeNESW; continue; }
                    if (bottomLeft.X < 10 && topRight.Y < 10) { updatedCursor = Cursors.SizeNWSE; continue; }
                    if (topRight.X < 10 && bottomLeft.Y < 10) { updatedCursor = Cursors.SizeNWSE; continue; }
                    if (topRight.X < 10 || bottomLeft.X < 10) { updatedCursor = Cursors.SizeWE; continue; }
                    if (topRight.Y < 10 || bottomLeft.Y < 10) { updatedCursor = Cursors.SizeNS; continue; }
                    if (ht.Index == 0) updatedCursor = Cursors.SizeAll;
                }
                else if (annotation is EllipseAnnotation eAnnotation)
                {
                    var ur = eAnnotation.Transform(Math.Max(eAnnotation.MaximumX, eAnnotation.MinimumX), Math.Max(eAnnotation.MaximumY, eAnnotation.MinimumY));
                    var ll = eAnnotation.Transform(Math.Min(eAnnotation.MinimumX, eAnnotation.MaximumX), Math.Min(eAnnotation.MinimumY, eAnnotation.MaximumY));

                    var topRight = new ScreenPoint(Math.Abs(ur.X - e.Position.X), Math.Abs(ur.Y - e.Position.Y));
                    var bottomLeft = new ScreenPoint(Math.Abs(ll.X - e.Position.X), Math.Abs(ll.Y - e.Position.Y));

                    if (topRight.X < 10 && topRight.Y < 10) { updatedCursor = Cursors.SizeNESW; continue; }
                    if (bottomLeft.X < 10 && bottomLeft.Y < 10) { updatedCursor = Cursors.SizeNESW; continue; }
                    if (bottomLeft.X < 10 && topRight.Y < 10) { updatedCursor = Cursors.SizeNWSE; continue; }
                    if (topRight.X < 10 && bottomLeft.Y < 10) { updatedCursor = Cursors.SizeNWSE; continue; }
                    if (topRight.X < 10 || bottomLeft.X < 10) { updatedCursor = Cursors.SizeWE; continue; }
                    if (topRight.Y < 10 || bottomLeft.Y < 10) { updatedCursor = Cursors.SizeNS; continue; }
                    if (ht.Index == 0) updatedCursor = Cursors.SizeAll;
                }
                else if (annotation is PointAnnotation)
                {
                    if (ht.Index == 0) updatedCursor = Cursors.SizeAll;
                }
                else if (annotation is PolygonAnnotation polyAnnotation)
                {
                    if (ht.Index == 0)
                    {
                        var screenToData = polyAnnotation.InverseTransform(e.Position);
                        var screen2ToData = polyAnnotation.InverseTransform(new ScreenPoint(e.Position.X - 10, e.Position.Y - 10));
                        var dxy = new DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y));
                        var dPoint = polyAnnotation.InverseTransform(e.Position);

                        if (polyAnnotation.Points.Any(o => Math.Abs(dPoint.X - o.X) < dxy.X && Math.Abs(dPoint.Y - o.Y) < dxy.Y))
                        {
                            updatedCursor = _movePointsCursor;
                        }
                        else
                        {
                            updatedCursor = Cursors.SizeAll;
                        }
                    }
                }
                else if (annotation is PolylineAnnotation polylineAnnotation)
                {
                    if (ht.Index == 0)
                    {
                        var screenToData = polylineAnnotation.InverseTransform(e.Position);
                        var screen2ToData = polylineAnnotation.InverseTransform(new ScreenPoint(e.Position.X - 10, e.Position.Y - 10));
                        var dxy = new DataPoint(Math.Abs(screenToData.X - screen2ToData.X), Math.Abs(screenToData.Y - screen2ToData.Y));
                        var dPoint = polylineAnnotation.InverseTransform(e.Position);

                        if (polylineAnnotation.Points.Any(o => Math.Abs(dPoint.X - o.X) < dxy.X && Math.Abs(dPoint.Y - o.Y) < dxy.Y))
                        {
                            updatedCursor = _movePointsCursor;
                        }
                        else
                        {
                            updatedCursor = e.IsControlDown ? _addPointCursor : Cursors.SizeAll;
                        }
                    }
                }
                else if (annotation is LineAnnotation)
                {
                    if (ht.Index == 0) updatedCursor = Cursors.SizeAll;
                }
            }

            if (requiresRedraw) Model.InvalidatePlot(false);

            if (updatedCursor != null)
            {
                PlotView.Cursor = updatedCursor;
                return;
            }

            if ((Mouse.LeftButton == MouseButtonState.Pressed && PanButton.IsChecked == true) || Mouse.MiddleButton == MouseButtonState.Pressed)
            {
                PlotView.PanCursor = _panHandClosedCursor;
                PlotView.Cursor = _panHandClosedCursor;
            }
        }

        /// <summary>
        /// Handles mouse move events when adding annotations.
        /// </summary>
        private void HandleAddAnnotationMouseMove(OxyMouseEventArgs e)
        {
            switch (_addAnnotationToolMode)
            {
                case AddToolMode.AddArrowAnnotation:
                    ((ArrowAnnotation)_targetAddAnnotation).EndPoint = ConvertScreenPointToDataPoint(e.Position);
                    break;

                case AddToolMode.AddTextAnnotation:
                    ((TextAnnotation)_targetAddAnnotation).TextPosition = ConvertScreenPointToDataPoint(e.Position);
                    break;

                case AddToolMode.AddVerticalLineAnnotation:
                    ((LineAnnotation)_targetAddAnnotation).X = ConvertScreenPointToDataPoint(e.Position).X;
                    UpdateLineAnnotationTooltip((LineAnnotation)_targetAddAnnotation);
                    break;

                case AddToolMode.AddHorizontalLineAnnotation:
                    ((LineAnnotation)_targetAddAnnotation).Y = ConvertScreenPointToDataPoint(e.Position).Y;
                    UpdateLineAnnotationTooltip((LineAnnotation)_targetAddAnnotation);
                    break;

                case AddToolMode.AddRectangleAnnotation:
                    {
                        var mouseDataPoint = ConvertScreenPointToDataPoint(e.Position);
                        var rect = (RectangleAnnotation)_targetAddAnnotation;
                        rect.MaximumX = mouseDataPoint.X;
                        rect.MaximumY = mouseDataPoint.Y;
                    }
                    break;

                case AddToolMode.AddEllipseAnnotation:
                    {
                        var mouseDataPoint = ConvertScreenPointToDataPoint(e.Position);
                        var ellipse = (EllipseAnnotation)_targetAddAnnotation;
                        ellipse.MaximumX = mouseDataPoint.X;
                        ellipse.MaximumY = mouseDataPoint.Y;
                    }
                    break;

                case AddToolMode.AddPointAnnotation:
                    {
                        var mouseDataPoint = ConvertScreenPointToDataPoint(e.Position);
                        var point = (PointAnnotation)_targetAddAnnotation;
                        point.X = mouseDataPoint.X;
                        point.Y = mouseDataPoint.Y;
                    }
                    break;

                case AddToolMode.AddPolygonAnnotation:
                    _leaderLine.Points[_leaderLine.Points.Count - 1] = new Point(e.Position.X, e.Position.Y);
                    Model.InvalidatePlot(true);
                    break;

                case AddToolMode.AddPolylineAnnotation:
                    {
                        var polyAnnotation = (PolylineAnnotation)_targetAddAnnotation;
                        _leaderLine.Points[0] = ConvertDataPointToPoint(polyAnnotation.Points[polyAnnotation.Points.Count - 1]);
                        _leaderLine.Points[_leaderLine.Points.Count - 1] = new Point(e.Position.X, e.Position.Y);
                        Model.InvalidatePlot(false);
                    }
                    break;
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
                CloseLineAnnotationTooltip((LineAnnotation)_targetAddAnnotation);
                StopAddAnnotation();
            }
            else if (_addAnnotationToolMode == AddToolMode.AddRectangleAnnotation)
            {
                var rectangle = (RectangleAnnotation)_targetAddAnnotation;
                ScreenPoint upperRight = rectangle.Transform(rectangle.MaximumX, rectangle.MaximumY);
                ScreenPoint lowerLeft = rectangle.Transform(rectangle.MinimumX, rectangle.MinimumY);
                double pixelWidth = Math.Abs(upperRight.X - lowerLeft.X);
                double pixelHeight = Math.Abs(upperRight.Y - lowerLeft.Y);
                if (pixelWidth < 10 || pixelHeight < 10)
                {
                    var plotLL = rectangle.InverseTransform(new ScreenPoint(Model.PlotArea.Left, Model.PlotArea.Bottom));
                    var plotUR = rectangle.InverseTransform(new ScreenPoint(Model.PlotArea.Right, Model.PlotArea.Top));
                    double centerXShift = Math.Abs((plotUR.X - plotLL.X) * 0.1);
                    double centerYShift = Math.Abs((plotUR.Y - plotLL.Y) * 0.1);
                    var mouseDataPoint = ConvertScreenPointToDataPoint(e.Position);
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
                var ellipse = (EllipseAnnotation)_targetAddAnnotation;
                ScreenPoint upperRight = ellipse.Transform(ellipse.MaximumX, ellipse.MaximumY);
                ScreenPoint lowerLeft = ellipse.Transform(ellipse.MinimumX, ellipse.MinimumY);
                double pixelWidth = Math.Abs(upperRight.X - lowerLeft.X);
                double pixelHeight = Math.Abs(upperRight.Y - lowerLeft.Y);
                if (pixelWidth < 10 || pixelHeight < 10)
                {
                    var plotLL = ellipse.InverseTransform(new ScreenPoint(Model.PlotArea.Left, Model.PlotArea.Bottom));
                    var plotUR = ellipse.InverseTransform(new ScreenPoint(Model.PlotArea.Right, Model.PlotArea.Top));
                    double centerXShift = Math.Abs((plotUR.X - plotLL.X) * 0.1);
                    double centerYShift = Math.Abs((plotUR.Y - plotLL.Y) * 0.1);
                    var mouseDataPoint = ConvertScreenPointToDataPoint(e.Position);
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
                    var arrow = (ArrowAnnotation)_targetAddAnnotation;
                    if (Math.Abs(arrow.StartPoint.X - arrow.EndPoint.X) < 0.000000001 && Math.Abs(arrow.StartPoint.Y - arrow.EndPoint.Y) < 0.000000001)
                    {
                        OxyRect plotArea = Model.PlotArea;
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
            if (Model == null) return;
            if (e.ChangedButton == OxyMouseButton.Middle) return;

            bool leftClickBool = e.ChangedButton == OxyMouseButton.Left;
            _contextMenu = new ContextMenu();

            if (!leftClickBool && Model.PlotArea.Contains(e.Position))
            {
                var formatPlotItem = new MenuItem { Header = "Format Plot Area", Icon = CreateMenuIcon("Format.png") };
                formatPlotItem.Click += (s, args) => PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.General_PlotArea, Model.PlotArea);
                _contextMenu.Items.Add(formatPlotItem);
            }

            // SERIES hit test
            var seriesHTRS = Model.HitTest(new HitTestArguments(e.Position, 10)).ToList();
            foreach (var htr in seriesHTRS)
            {
                var series = Model.Series.FirstOrDefault(s => s.Equals(htr.Element));
                if (series != null)
                {
                    if (leftClickBool)
                    {
                        PropertiesCalled?.Invoke(PlotView, false, OxyPlotPropertiesControl.PropertyEXP.Series_General, series);
                        return;
                    }
                    else
                    {
                        var seriesItem = new MenuItem { Header = "Format Series: " + series.Title, Icon = CreateMenuIcon("Format.png") };
                        seriesItem.Click += (s, args) => PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.Series_General, series);
                        _contextMenu.Items.Add(seriesItem);
                    }
                }
            }

            // Legend Area hit test
            var legendArea = Model.LegendArea;
            if (legendArea.Contains(e.Position))
            {
                if (leftClickBool)
                {
                    PropertiesCalled?.Invoke(PlotView, false, OxyPlotPropertiesControl.PropertyEXP.Legend_Title, legendArea);
                    return;
                }
                else
                {
                    var legendItem = new MenuItem { Header = "Format Legend", Icon = CreateMenuIcon("Format.png") };
                    legendItem.Click += (s, args) => PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.Legend_Title, legendArea);
                    _contextMenu.Items.Add(legendItem);
                }
            }

            // AXIS Areas hit test
            var plotArea = Model.PlotArea;
            foreach (var axis in Model.Axes)
            {
                double axLeft = 0, axTop = 0, axWidth = 0, axHeight = 0;

                switch (axis.Position)
                {
                    case OxyPlot.Axes.AxisPosition.Bottom:
                        axLeft = plotArea.Left;
                        axTop = plotArea.Bottom + axis.AxisDistance;
                        axWidth = plotArea.Width;
                        axHeight = axis.DesiredSize.Height;
                        break;
                    case OxyPlot.Axes.AxisPosition.Top:
                        axLeft = plotArea.Left;
                        axTop = plotArea.Top - axis.AxisDistance - axis.DesiredSize.Height;
                        axWidth = plotArea.Width;
                        axHeight = axis.DesiredSize.Height;
                        break;
                    case OxyPlot.Axes.AxisPosition.Left:
                        axLeft = plotArea.Left - axis.AxisDistance - axis.DesiredSize.Width;
                        axTop = plotArea.Top;
                        axWidth = axis.DesiredSize.Width;
                        axHeight = plotArea.Height;
                        break;
                    case OxyPlot.Axes.AxisPosition.Right:
                        axLeft = plotArea.Right + axis.AxisDistance;
                        axTop = plotArea.Top;
                        axWidth = axis.DesiredSize.Width;
                        axHeight = plotArea.Height;
                        break;
                }

                var axArea = new OxyRect(axLeft, axTop, axWidth, axHeight);
                if (axArea.Contains(e.Position))
                {
                    if (leftClickBool)
                    {
                        PropertiesCalled?.Invoke(PlotView, false, OxyPlotPropertiesControl.PropertyEXP.Axes_Options, axis);
                        PropertiesCalled?.Invoke(PlotView, false, OxyPlotPropertiesControl.PropertyEXP.Axes_Display, axis);
                        return;
                    }
                    else
                    {
                        var formatAxisItem = new MenuItem { Header = "Format Axis: " + axis.Title, Icon = CreateMenuIcon("Format.png") };
                        formatAxisItem.Click += (s, args) =>
                        {
                            PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.Axes_Options, axis);
                            PropertiesCalled?.Invoke(PlotView, false, OxyPlotPropertiesControl.PropertyEXP.Axes_Display, axis);
                        };
                        _contextMenu.Items.Add(formatAxisItem);
                    }
                }
            }

            // ANNOTATIONS hit test
            var annoHTRS = Model.HitTest(new HitTestArguments(e.Position, 10)).ToList();
            foreach (var htr in annoHTRS)
            {
                if (htr.Element is OxyPlot.Annotations.Annotation annotation)
                {
                    string annoText = "";
                    if (annotation is TextualAnnotation textual)
                        annoText = textual.Text ?? "";

                    if (leftClickBool)
                    {
                        PropertiesCalled?.Invoke(PlotView, false, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, annotation);
                        return;
                    }
                    else
                    {
                        var formatAnnoItem = new MenuItem { Header = "Format Annotation: " + annoText, Icon = CreateMenuIcon("Format.png") };
                        formatAnnoItem.Click += (s, args) => PropertiesCalled?.Invoke(PlotView, true, OxyPlotPropertiesControl.PropertyEXP.Annotations_Text, annotation);

                        var deleteAnnoItem = new MenuItem { Header = "Delete Annotation: " + annoText, Icon = CreateMenuIcon("Delete.png") };
                        deleteAnnoItem.Click += (s, args) =>
                        {
                            Model.Annotations.Remove(annotation);
                            Model.InvalidatePlot(false);
                        };

                        _contextMenu.Items.Add(formatAnnoItem);
                        _contextMenu.Items.Add(deleteAnnoItem);
                    }
                    break;
                }
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
            return Model.DefaultXAxis.InverseTransform(pt.X, pt.Y, Model.DefaultYAxis);
        }

        private ScreenPoint ConvertDataPointToScreenPoint(DataPoint pt)
        {
            return Model.DefaultXAxis.Transform(pt.X, pt.Y, Model.DefaultYAxis);
        }

        private Point ConvertDataPointToPoint(DataPoint pt)
        {
            var sp = Model.DefaultXAxis.Transform(pt.X, pt.Y, Model.DefaultYAxis);
            return new Point(sp.X, sp.Y);
        }

        private DataPoint ConvertLeaderLinePoint(int pointIndex)
        {
            return Model.DefaultXAxis.InverseTransform(_leaderLine.Points[pointIndex].X, _leaderLine.Points[pointIndex].Y, Model.DefaultYAxis);
        }

        /// <summary>
        /// Loads an image resource and returns it as a BitmapImage.
        /// </summary>
        private static BitmapImage LoadResourceImage(string resourceName)
        {
            var uri = new Uri($"pack://application:,,,/OxyPlotControls;component/Resources/{resourceName}", UriKind.Absolute);
            var bitmapImage = new BitmapImage(uri);
            return bitmapImage;
        }

        /// <summary>
        /// Creates an Image control with the specified resource image.
        /// </summary>
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
            if (Model == null) return;

            var tableList = new List<DataTable>();
            int tableCount = 0;
            string[] badCharacters = { ":", "\\", "/", "?", "*", "[", "]" };

            foreach (var series in Model.Series)
            {
                var dataTable = new DataTable("Series");
                string seriesName = "";
                tableCount++;

                if (series is OxyPlot.Series.LineSeries lineSeries)
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

                    if (lineSeries.ItemsSource != null)
                    {
                        var datalist = lineSeries.ItemsSource as IEnumerable<DataPoint>;
                        if (datalist != null)
                        {
                            foreach (var seriesValue in datalist.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y);
                            }
                        }
                        else
                        {
                            foreach (var obj in lineSeries.ItemsSource.Cast<object>())
                            {
                                PropertyInfo propX = obj.GetType().GetProperty(lineSeries.DataFieldX);
                                string xVal = Convert.ToString(propX.GetValue(obj, null));
                                PropertyInfo propY = obj.GetType().GetProperty(lineSeries.DataFieldY);
                                string yVal = Convert.ToString(propY.GetValue(obj, null));
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
                }
                else if (series is OxyPlot.Series.ScatterSeries scatterSeries)
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

                    if (scatterSeries.ItemsSource != null)
                    {
                        var datalist = scatterSeries.ItemsSource as IEnumerable<OxyPlot.Series.ScatterPoint>;
                        if (datalist != null)
                        {
                            foreach (var seriesValue in datalist.ToList())
                            {
                                dataTable.Rows.Add(dataTable.Rows.Count + 1, seriesValue.X, seriesValue.Y);
                            }
                        }
                        else
                        {
                            foreach (var obj in scatterSeries.ItemsSource.Cast<object>())
                            {
                                PropertyInfo propX = obj.GetType().GetProperty(scatterSeries.DataFieldX);
                                string xVal = Convert.ToString(propX.GetValue(obj, null));
                                PropertyInfo propY = obj.GetType().GetProperty(scatterSeries.DataFieldY);
                                string yVal = Convert.ToString(propY.GetValue(obj, null));
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
                }
                else if (series is OxyPlot.Series.AreaSeries areaSeries)
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
                else if (series is OxyPlot.Series.BoxPlotSeries boxPlotSeries)
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

                    foreach (var item in boxPlotSeries.Items)
                    {
                        dataTable.Rows.Add(dataTable.Rows.Count + 1, item.X, item.LowerWhisker, item.BoxBottom, item.Median, item.BoxTop, item.UpperWhisker, item.Tag);
                    }
                }

                if (dataTable.Rows.Count > 0)
                {
                    tableList.Add(dataTable);
                }
            }

            if (tableList.Count == 0)
            {
                MessageBox.Show("No series data to export.");
                return;
            }

            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV File (*.csv)|*.csv",
                DefaultExt = ".csv"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    using (var writer = new StreamWriter(saveDialog.FileName))
                    {
                        foreach (var table in tableList)
                        {
                            // Write column headers
                            var headers = string.Join(",", table.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                            writer.WriteLine(headers);

                            // Write rows
                            foreach (DataRow row in table.Rows)
                            {
                                var values = string.Join(",", row.ItemArray.Select(v => v.ToString()));
                                writer.WriteLine(values);
                            }
                            writer.WriteLine(); // Empty line between tables
                        }
                    }
                    MessageBox.Show("Data exported successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting data: " + ex.Message);
                }
            }
        }

        #endregion

        #region Properties and Swap Axes

        /// <summary>
        /// Open plot properties.
        /// </summary>
        private void PropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            PropertiesCalled?.Invoke(PlotView, true, null, null);
        }

        /// <summary>
        /// Swap the X and Y axes.
        /// </summary>
        private void SwapAxesButton_Click(object sender, RoutedEventArgs e)
        {
            if (Model == null) return;

            foreach (var s in Model.Series)
            {
                if (_nonSwapSeriesTypes.Contains(s.GetType())) return;
            }

            foreach (var axis in Model.Axes)
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

            foreach (var s in Model.Series)
            {
                if (s is OxyPlot.Series.DataPointSeries dps)
                {
                    SwapDataPoints(dps);
                }
                else if (s is OxyPlot.Series.ScatterSeries scatterSeries)
                {
                    SwapScatterSeries(scatterSeries);
                }
                else if (s is OxyPlot.Series.ScatterErrorSeries scatterErrorSeries)
                {
                    SwapScatterErrorSeries(scatterErrorSeries);
                }
                else if (s is OxyPlot.Series.BoxPlotSeries boxPlotSeries)
                {
                    foreach (var axis in Model.Axes)
                    {
                        if (axis is OxyPlot.Axes.CategoryAxis)
                        {
                            // BoxPlotSeries orientation is based on axis position
                            // No direct IsVertical property on core type
                        }
                    }
                }
            }

            Model.InvalidatePlot(true);
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

        private void SwapScatterSeries(OxyPlot.Series.ScatterSeries series)
        {
            if (series?.Points == null || series.Points.Count == 0) return;
            var pnts = series.Points.ToArray();
            series.Points.Clear();
            foreach (var p in pnts)
            {
                series.Points.Add(new OxyPlot.Series.ScatterPoint(p.Y, p.X, p.Size, p.Value, p.Tag));
            }
        }

        private void SwapScatterErrorSeries(OxyPlot.Series.ScatterErrorSeries series)
        {
            if (series?.Points == null || series.Points.Count == 0) return;
            var pnts = series.Points.ToArray();
            series.Points.Clear();
            foreach (var p in pnts)
            {
                series.Points.Add(new OxyPlot.Series.ScatterErrorPoint(p.Y, p.X, p.ErrorY, p.ErrorX, p.Size, p.Value, p.Tag));
            }
        }

        #endregion

        #region Save Image

        /// <summary>
        /// Save plot image.
        /// </summary>
        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlotView == null) return;
            var saveDialog = new SavePlotImageDialog(PlotView);
            saveDialog.Owner = Window.GetWindow(this);
            saveDialog.ShowDialog();
        }

        #endregion
    }
}
