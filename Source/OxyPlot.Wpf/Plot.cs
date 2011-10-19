//-----------------------------------------------------------------------
// <copyright file="Plot.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Xml;

    /// <summary>
    /// Represents a WPF control that displays an OxyPlot plot.
    /// </summary>
    [ContentProperty("Series")]
    [TemplatePart(Name = PartGrid, Type = typeof(Grid))]
    public partial class Plot : Control, IPlotControl
    {
        #region Constants and Fields

        /// <summary>
        ///   The stack of manipulation events. This is used to try to avoid latency of the ManipulationDelta events.
        /// </summary>
        private readonly Stack<ManipulationDeltaEventArgs> manipulationQueue = new Stack<ManipulationDeltaEventArgs>();
        
        /// <summary>
        ///   The Grid PART constant.
        /// </summary>
        private const string PartGrid = "PART_Grid";

        /// <summary>
        ///   The tracker definitions.
        /// </summary>
        private readonly ObservableCollection<TrackerDefinition> trackerDefinitions;

        /// <summary>
        ///   The last cumulative manipulation scale.
        /// </summary>
        private Vector lastManipulationScale;

        /// <summary>
        ///   The mouse manipulator.
        /// </summary>
        private ManipulatorBase mouseManipulator;

        /// <summary>
        ///   The touch pan manipulator.
        /// </summary>
        private PanManipulator touchPan;

        /// <summary>
        ///   The touch zoom manipulator.
        /// </summary>
        private ZoomManipulator touchZoom;

        /// <summary>
        ///   The touch down point.
        /// </summary>
        private Point touchDownPoint;
      
        /// <summary>
        ///   The canvas.
        /// </summary>
        private Canvas canvas;

        /// <summary>
        ///   The current tracker.
        /// </summary>
        private FrameworkElement currentTracker;

        /// <summary>
        ///   The grid.
        /// </summary>
        private Grid grid;

        /// <summary>
        ///   The internal model.
        /// </summary>
        private PlotModel internalModel;

        /// <summary>
        ///   The is plot invalidated.
        /// </summary>
        private bool isPlotInvalidated;

        /// <summary>
        ///   Flag to update data when the plot has been invalidated.
        /// </summary>
        private bool invalidateUpdatesData;

        /// <summary>
        ///   The mouse down point.
        /// </summary>
        private ScreenPoint mouseDownPoint;

        /// <summary>
        ///   The overlays.
        /// </summary>
        private Canvas overlays;

        /// <summary>
        ///   The zoom control.
        /// </summary>
        private ContentControl zoomControl;

        #endregion

        #region Constructors and Destructors

#if WPF

        /// <summary>
        ///   Initializes static members of the <see cref = "Plot" /> class.
        /// </summary>
        static Plot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Plot), new FrameworkPropertyMetadata(typeof(Plot)));
            PaddingProperty.OverrideMetadata(
                typeof(Plot), new FrameworkPropertyMetadata(new Thickness(8, 8, 16, 8), AppearanceChanged));
            ResetAxesCommand = new RoutedCommand();
        }

#endif

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Plot" /> class.
        /// </summary>
        public Plot()
        {
            this.series = new ObservableCollection<Series>();
            this.axes = new ObservableCollection<Axis>();
            this.annotations = new ObservableCollection<Annotation>();
            this.trackerDefinitions = new ObservableCollection<TrackerDefinition>();

            this.series.CollectionChanged += this.OnSeriesChanged;
            this.axes.CollectionChanged += this.OnAxesChanged;
            this.annotations.CollectionChanged += this.OnAnnotationsChanged;

            this.Loaded += this.OnLoaded;
            this.DataContextChanged += this.OnDataContextChanged;
            this.SizeChanged += this.OnSizeChanged;

            CompositionTarget.Rendering += this.CompositionTargetRendering;

            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, this.DoCopy));

            // this.CommandBindings.Add(new CommandBinding(CopyCode, this.DoCopyCode));
            // this.InputBindings.Add(new KeyBinding(CopyCode, Key.C, ModifierKeys.Control | ModifierKeys.Alt));
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the actual model.
        /// </summary>
        /// <value>The actual model.</value>
        public PlotModel ActualModel
        {
            get
            {
                return this.Model ?? this.internalModel;
            }
        }

        /// <summary>
        ///   Gets the annotations.
        /// </summary>
        /// <value>The annotations.</value>
        public ObservableCollection<Annotation> Annotations
        {
            get
            {
                return this.annotations;
            }
        }

        /// <summary>
        ///   Gets the tracker definitions.
        /// </summary>
        /// <value>The tracker definitions.</value>
        public ObservableCollection<TrackerDefinition> TrackerDefinitions
        {
            get
            {
                return this.trackerDefinitions;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the axes from a point.
        /// </summary>
        /// <param name="pt">
        /// The point.
        /// </param>
        /// <param name="xaxis">
        /// The x-axis.
        /// </param>
        /// <param name="yaxis">
        /// The y-axis.
        /// </param>
        public void GetAxesFromPoint(ScreenPoint pt, out OxyPlot.IAxis xaxis, out OxyPlot.IAxis yaxis)
        {
            if (this.ActualModel != null)
            {
                this.ActualModel.GetAxesFromPoint(pt, out xaxis, out yaxis);
            }
            else
            {
                xaxis = null;
                yaxis = null;
            }
        }

        /// <summary>
        /// Gets the series that is nearest the specified point (in screen coordinates).
        /// </summary>
        /// <param name="pt">
        /// The point.
        /// </param>
        /// <param name="limit">
        /// The maximum distance, if this is exceeded the method will return null.
        /// </param>
        /// <returns>
        /// The closest DataSeries
        /// </returns>
        public OxyPlot.ISeries GetSeriesFromPoint(ScreenPoint pt, double limit)
        {
            return this.ActualModel != null ? this.ActualModel.GetSeriesFromPoint(pt, limit) : null;
        }

        /// <summary>
        /// Hides the tracker.
        /// </summary>
        public void HideTracker()
        {
            if (this.currentTracker != null)
            {
                this.overlays.Children.Remove(this.currentTracker);
                this.currentTracker = null;
            }
        }

        /// <summary>
        /// Hides the zoom rectangle.
        /// </summary>
        public void HideZoomRectangle()
        {
            this.zoomControl.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Invalidate the plot (not blocking the UI thread)
        /// </summary>
        /// <param name="updateData">
        /// The update Data.
        /// </param>
        public void InvalidatePlot(bool updateData = true)
        {
            lock (this)
            {
                this.isPlotInvalidated = true;
                this.invalidateUpdatesData = this.invalidateUpdatesData || updateData;
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.grid = this.GetTemplateChild(PartGrid) as Grid;

            if (this.grid == null)
            {
                return;
            }

            this.canvas = new Canvas();
            this.grid.Children.Add(this.canvas);
            this.canvas.UpdateLayout();

            this.overlays = new Canvas();
            this.grid.Children.Add(this.overlays);

            this.zoomControl = new ContentControl();
            this.overlays.Children.Add(this.zoomControl);

            this.CommandBindings.Add(new CommandBinding(ResetAxesCommand, this.ResetAxesHandler));

            var resetAxesInputBinding = new InputBindingX { Command = ResetAxesCommand };
            BindingOperations.SetBinding(
                resetAxesInputBinding, InputBindingX.GeztureProperty, new Binding("ResetAxesGesture") { Source = this });
            this.InputBindings.Add(resetAxesInputBinding);
        }

        /// <summary>
        /// Pans the specified axis.
        /// </summary>
        /// <param name="axis">
        /// The axis.
        /// </param>
        /// <param name="ppt">
        /// The previous point (screen coordinates).
        /// </param>
        /// <param name="cpt">
        /// The current point (screen coordinates).
        /// </param>
        public void Pan(OxyPlot.IAxis axis, ScreenPoint ppt, ScreenPoint cpt)
        {
            axis.Pan(ppt, cpt);
            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Pans all axes.
        /// </summary>
        /// <param name="delta">
        /// The delta.
        /// </param>
        public void PanAll(Vector delta)
        {
            foreach (OxyPlot.IAxis a in this.ActualModel.Axes)
            {
                a.Pan(a.IsHorizontal() ? delta.X : delta.Y);
            }

            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Refresh the plot immediately (blocking UI thread)
        /// </summary>
        /// <param name="updateData">
        /// if set to <c>true</c>, the data collections will be updated.
        /// </param>
        public void RefreshPlot(bool updateData)
        {
            this.UpdateModel(updateData);
            this.UpdateVisuals();
        }

        /// <summary>
        /// Resets the specified axis.
        /// </summary>
        /// <param name="axis">
        /// The axis.
        /// </param>
        public void Reset(OxyPlot.IAxis axis)
        {
            axis.Reset();
        }

        /// <summary>
        /// Saves the plot as a bitmap.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        public void SaveBitmap(string fileName)
        {
            RenderTargetBitmap bmp = this.ToBitmap();

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using (FileStream s = File.Create(fileName))
            {
                encoder.Save(s);
            }
        }

        /// <summary>
        /// Saves the plot as xaml.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        public void SaveXaml(string fileName)
        {
            using (var w = new StreamWriter(fileName))
            {
                w.Write(this.ToXaml());
            }
        }

        /// <summary>
        /// Shows the tracker.
        /// </summary>
        /// <param name="trackerHitResult">
        /// The tracker data.
        /// </param>
        public void ShowTracker(TrackerHitResult trackerHitResult)
        {
            if (trackerHitResult != null)
            {
                var ts = trackerHitResult.Series as ITrackableSeries;
                ControlTemplate trackerTemplate = this.DefaultTrackerTemplate;
                if (ts != null && !string.IsNullOrEmpty(ts.TrackerKey))
                {
                    TrackerDefinition match = this.TrackerDefinitions.FirstOrDefault(t => t.TrackerKey == ts.TrackerKey);
                    if (match != null)
                    {
                        trackerTemplate = match.TrackerTemplate;
                    }
                }

                var tracker = new ContentControl { Template = trackerTemplate };

                if (tracker != this.currentTracker)
                {
                    this.HideTracker();
                    if (trackerTemplate != null)
                    {
                        this.overlays.Children.Add(tracker);
                        this.currentTracker = tracker;
                    }
                }

                if (this.currentTracker != null)
                {
                    this.currentTracker.DataContext = trackerHitResult;
                }
            }
            else
            {
                this.HideTracker();
            }
        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="r">
        /// The rectangle.
        /// </param>
        public void ShowZoomRectangle(OxyRect r)
        {
            this.zoomControl.Width = r.Width;
            this.zoomControl.Height = r.Height;
            Canvas.SetLeft(this.zoomControl, r.Left);
            Canvas.SetTop(this.zoomControl, r.Top);
            this.zoomControl.Template = this.ZoomRectangleTemplate;
            this.zoomControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Renders the plot to a bitmap.
        /// </summary>
        /// <returns>
        /// A bitmap.
        /// </returns>
        public RenderTargetBitmap ToBitmap()
        {
            var bmp = new RenderTargetBitmap(
                (int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Pbgra32);

            var clientRect = new Rect(this.VisualOffset.X, this.VisualOffset.Y, this.ActualWidth, this.ActualHeight);

            // move the client area, otherwise the rendering may be at the wrong offset
            this.Arrange(new Rect(0, 0, bmp.Width, bmp.Height));
            bmp.Render(this);

            this.Arrange(clientRect);
            return bmp;
        }

        /// <summary>
        /// Renders the plot to xaml.
        /// </summary>
        /// <returns>
        /// The to xaml.
        /// </returns>
        public string ToXaml()
        {
            var sb = new StringBuilder();
            var tw = new StringWriter(sb);
            XmlWriter xw = XmlWriter.Create(tw, new XmlWriterSettings { Indent = true });
            if (this.canvas != null)
            {
                XamlWriter.Save(this.canvas, xw);
            }

            xw.Close();
            return sb.ToString();
        }

        /// <summary>
        /// Zooms the specified axis to the specified values.
        /// </summary>
        /// <param name="axis">
        /// The axis.
        /// </param>
        /// <param name="p1">
        /// The new minimum value.
        /// </param>
        /// <param name="p2">
        /// The new maximum value.
        /// </param>
        public void Zoom(OxyPlot.IAxis axis, double p1, double p2)
        {
            axis.Zoom(p1, p2);
            this.RefreshPlot(false);
        }

        /// <summary>
        /// Zooms all axes.
        /// </summary>
        /// <param name="delta">
        /// The delta.
        /// </param>
        public void ZoomAllAxes(double delta)
        {
            foreach (var a in this.ActualModel.Axes)
            {
                this.ZoomAt(a, delta);
            }

            this.RefreshPlot(false);
        }

        /// <summary>
        /// Reset all axes.
        /// </summary>
        public void ResetAllAxes()
        {
            foreach (OxyPlot.IAxis a in this.ActualModel.Axes)
            {
                a.Reset();
            }

            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Zooms at the specified position.
        /// </summary>
        /// <param name="axis">
        /// The axis.
        /// </param>
        /// <param name="factor">
        /// The zoom factor.
        /// </param>
        /// <param name="x">
        /// The position to zoom at.
        /// </param>
        public void ZoomAt(OxyPlot.IAxis axis, double factor, double x = double.NaN)
        {
            if (double.IsNaN(x))
            {
                double sx = (axis.Transform(axis.ActualMaximum) + axis.Transform(axis.ActualMinimum)) * 0.5;
                x = axis.InverseTransform(sx);
            }

            axis.ZoomAt(factor, x);
        }

        /// <summary>
        /// Sets the cursor.
        /// </summary>
        /// <param name="cursor">
        /// The cursor.
        /// </param>
        public void SetCursor(OxyCursor cursor)
        {
            switch (cursor)
            {
                case OxyCursor.Arrow:
                    this.Cursor = Cursors.Arrow;
                    break;
                case OxyCursor.Cross:
                    this.Cursor = Cursors.Cross;
                    break;
                case OxyCursor.SizeAll:
                    this.Cursor = Cursors.SizeAll;
                    break;
                case OxyCursor.SizeNWSE:
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                case OxyCursor.None:
                    this.Cursor = Cursors.None;
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.KeyDown"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.KeyEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            bool control = Keyboard.IsKeyDown(Key.LeftCtrl);
            bool alt = Keyboard.IsKeyDown(Key.LeftAlt);

            if (e.Key == Key.A)
            {
                e.Handled = true;
                this.ResetAllAxes();
            }

            var delta = new Vector();
            double zoom = 0;
            switch (e.Key)
            {
                case Key.Up:
                    delta = new Vector(0, -1);
                    break;
                case Key.Down:
                    delta = new Vector(0, 1);
                    break;
                case Key.Left:
                    delta = new Vector(-1, 0);
                    break;
                case Key.Right:
                    delta = new Vector(1, 0);
                    break;
                case Key.Add:
                case Key.OemPlus:
                case Key.PageUp:
                    zoom = 1;
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                case Key.PageDown:
                    zoom = -1;
                    break;
            }

            if (delta.Length > 0)
            {
                delta.X = delta.X * this.ActualModel.PlotArea.Width * this.KeyboardPanHorizontalStep;
                delta.Y = delta.Y * this.ActualModel.PlotArea.Height * this.KeyboardPanVerticalStep;

                // small steps if the user is pressing control
                if (control)
                {
                    delta *= 0.2;
                }

                this.PanAll(delta);
                e.Handled = true;
            }

            if (Math.Abs(zoom) > 1e-8)
            {
                if (control)
                {
                    zoom *= 0.2;
                }

                this.ZoomAllAxes(1 + zoom * 0.12);
                e.Handled = true;
            }

            if (control && alt && this.ActualModel != null)
            {
                switch (e.Key)
                {
                    case Key.R:
                        Clipboard.SetText(this.ActualModel.CreateTextReport());
                        break;
                    case Key.C:
                        Clipboard.SetText(this.ActualModel.ToCode());
                        break;
                    case Key.X:
                        Clipboard.SetText(this.ToXaml());
                        break;
                }
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Called when the <see cref="E:System.Windows.UIElement.ManipulationStarted"/> event occurs.
        /// </summary>
        /// <param name="e">
        /// The data for the event.
        /// </param>
        protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
        {
            base.OnManipulationStarted(e);
            this.touchPan = new PanManipulator(this);
            this.touchPan.Started(new ManipulationEventArgs(e.ManipulationOrigin.ToScreenPoint()));
            this.touchZoom = new ZoomManipulator(this);
            this.touchZoom.Started(new ManipulationEventArgs(e.ManipulationOrigin.ToScreenPoint()));
            this.touchDownPoint = e.ManipulationOrigin;
            this.lastManipulationScale = new Vector(1, 1);
            e.Handled = true;
        }

        /// <summary>
        /// Called when the <see cref="E:System.Windows.UIElement.ManipulationDelta"/> event occurs.
        /// </summary>
        /// <param name="e">
        /// The data for the event.
        /// </param>
        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            base.OnManipulationDelta(e);
            lock (this.manipulationQueue)
            {
                this.manipulationQueue.Push(e);
            }

            // this was the original code, but it seems to add latency to the manipulations...
            // var position = this.touchDownPoint + e.CumulativeManipulation.Translation;
            // this.touchPan.Delta(new ManipulationEventArgs(position.ToScreenPoint()));

            // this.touchZoom.Delta(
            // new ManipulationEventArgs(position.ToScreenPoint())
            // {
            // ScaleX = e.DeltaManipulation.Scale.X, ScaleY = e.DeltaManipulation.Scale.Y 
            // });
            e.Handled = true;
        }

        /// <summary>
        /// Called when the <see cref="E:System.Windows.UIElement.ManipulationCompleted"/> event occurs.
        /// </summary>
        /// <param name="e">
        /// The data for the event.
        /// </param>
        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            base.OnManipulationCompleted(e);
            var position = this.touchDownPoint + e.TotalManipulation.Translation;
            this.touchPan.Completed(new ManipulationEventArgs(position.ToScreenPoint()));
            e.Handled = true;
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseDown"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state.
        /// </param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (this.mouseManipulator != null)
            {
                return;
            }

            this.Focus();
            this.CaptureMouse();
            this.mouseDownPoint = e.GetPosition(this).ToScreenPoint();

            this.mouseManipulator = this.GetManipulator(e);

            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Started(this.CreateManipulationEventArgs(e));
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseMove"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.MouseEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Delta(this.CreateManipulationEventArgs(e));
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseUp"/> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the mouse button was released.
        /// </param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Completed(this.CreateManipulationEventArgs(e));
                e.Handled = true;
            }

            this.mouseManipulator = null;

            this.ReleaseMouseCapture();
            ScreenPoint p = e.GetPosition(this).ToScreenPoint();

            double d = p.DistanceTo(this.mouseDownPoint);
            if (this.ContextMenu != null)
            {
                if (Math.Abs(d) < 1e-8 && e.ChangedButton == MouseButton.Right)
                {
                    this.ContextMenu.Visibility = Visibility.Visible;

                    // todo: The contextmenu has the wrong placement after panning
                    // ContextMenu.Placement = PlacementMode.Relative;
                    // ContextMenu.PlacementTarget = this;
                    // ContextMenu.PlacementRectangle=new Rect(e.GetPosition(this),new Size(0,0));
                }
                else
                {
                    this.ContextMenu.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Called when the visual appearance is changed.
        /// </summary>
        protected virtual void OnAppearanceChanged()
        {
            this.InvalidatePlot();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseWheel"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.MouseWheelEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (!this.IsMouseWheelEnabled)
            {
                return;
            }

            bool isControlDown = Keyboard.IsKeyDown(Key.LeftCtrl);
            //// bool isShiftDown = Keyboard.IsKeyDown(Key.LeftShift);
            //// bool isAltDown = Keyboard.IsKeyDown(Key.LeftAlt);

            var m = new ZoomStepManipulator(this, e.Delta * 0.001, isControlDown);
            m.Started(new ManipulationEventArgs(e.GetPosition(this).ToScreenPoint()));
        }

        /// <summary>
        /// Called when the model is changed.
        /// </summary>
        /// <param name="d">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Plot)d).OnModelChanged();
        }

        /// <summary>
        /// Called when the visual appearance is changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Plot)d).OnAppearanceChanged();
        }

        /// <summary>
        /// The reset axes handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ResetAxesHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.ResetAllAxes();
        }

        /// <summary>
        /// Handles the stacked manipulation events.
        /// </summary>
        private void HandleStackedManipulationEvents()
        {
            ManipulationDeltaEventArgs e;
            lock (this.manipulationQueue)
            {
                if (this.manipulationQueue.Count == 0)
                {
                    return;
                }

                // Get the last manipulation event from the stack
                e = this.manipulationQueue.Pop();

                // Skip all older events
                this.manipulationQueue.Clear();
            }

            // Apply the last manipulation event to translation (pan) and scaling (zoom)
            var position = this.touchDownPoint + e.CumulativeManipulation.Translation;
            this.touchPan.Delta(new ManipulationEventArgs(position.ToScreenPoint()));

            double scaleX = e.CumulativeManipulation.Scale.X / this.lastManipulationScale.X;
            double scaleY = e.CumulativeManipulation.Scale.Y / this.lastManipulationScale.Y;
            this.touchZoom.Delta(
                new ManipulationEventArgs(position.ToScreenPoint()) { ScaleX = scaleX, ScaleY = scaleY });

            this.lastManipulationScale = e.CumulativeManipulation.Scale;
        }

        /// <summary>
        /// Gets the manipulator for the current mouse button and modifier keys.
        /// </summary>
        /// <param name="e">The event args.</param>
        /// <returns>
        /// A manipulator or null if no gesture was recognized.
        /// </returns>
        private ManipulatorBase GetManipulator(MouseButtonEventArgs e)
        {
            bool control = Keyboard.IsKeyDown(Key.LeftCtrl);
            bool shift = Keyboard.IsKeyDown(Key.LeftShift);
            bool alt = Keyboard.IsKeyDown(Key.LeftAlt);
            bool lmb = e.LeftButton == MouseButtonState.Pressed;
            bool rmb = e.RightButton == MouseButtonState.Pressed;
            bool mmb = e.MiddleButton == MouseButtonState.Pressed;
            bool xb1 = e.XButton1 == MouseButtonState.Pressed;
            bool xb2 = e.XButton2 == MouseButtonState.Pressed;

            // MMB / control RMB / control+alt LMB
            if (mmb || (control && rmb) || (control && alt && lmb))
            {
                if (e.ClickCount == 2)
                {
                    return new ResetManipulator(this);
                }

                return new ZoomRectangleManipulator(this);
            }

            // Right mouse button / alt+left mouse button
            if (rmb || (lmb && alt))
            {
                return new PanManipulator(this);
            }

            // Left mouse button
            if (lmb)
            {
                return new TrackerManipulator(this) { Snap = !control, PointsOnly = shift };
            }

            // XButtons are zoom-stepping
            if (xb1 || xb2)
            {
                double d = xb1 ? 0.05 : -0.05;
                return new ZoomStepManipulator(this, d, control);
            }

            return null;
        }

        /// <summary>
        /// Creates the manipulation event args.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.
        /// </param>
        /// <returns>
        /// A manipulation event args object.
        /// </returns>
        private ManipulationEventArgs CreateManipulationEventArgs(MouseEventArgs e)
        {
            return new ManipulationEventArgs(e.GetPosition(this).ToScreenPoint());
        }

        /// <summary>
        /// Compositions the target rendering.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            this.HandleStackedManipulationEvents();

            lock (this)
            {
                if (this.isPlotInvalidated)
                {
                    this.isPlotInvalidated = false;
                    if (this.ActualWidth > 0 && this.ActualHeight > 0)
                    {
                        this.UpdateModel(this.invalidateUpdatesData);
                        this.invalidateUpdatesData = false;
                        this.UpdateVisuals();
                    }
                }
            }
        }

        /// <summary>
        /// Performs the copy operation.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.ExecutedRoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void DoCopy(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetImage(this.ToBitmap());
        }

        /// <summary>
        /// Called when annotations is changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void OnAnnotationsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.SyncLogicalTree(e);
        }

        /// <summary>
        /// Called when axes is changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void OnAxesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.SyncLogicalTree(e);
        }

        /// <summary>
        /// Called when the data context is changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.OnModelChanged();
        }

        /// <summary>
        /// Called when the control is loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.OnModelChanged();
        }

        /// <summary>
        /// Called when the model is changed.
        /// </summary>
        private void OnModelChanged()
        {
            this.InvalidatePlot();
        }

        /// <summary>
        /// Called when series is changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void OnSeriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.SyncLogicalTree(e);
        }

        /// <summary>
        /// Called when size of the control is changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidatePlot();
        }

        /// <summary>
        /// Synchronizes the logical tree.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void SyncLogicalTree(NotifyCollectionChangedEventArgs e)
        {
            // In order to get DataContext and binding to work with the series, axes and annotations
            // we add the items to the logical tree
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    this.AddLogicalChild(item);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    this.RemoveLogicalChild(item);
                }
            }
        }

        /// <summary>
        /// Updates the visuals.
        /// </summary>
        private void UpdateVisuals()
        {
            if (this.canvas == null)
            {
                return;
            }

            // if (Model != null && internalModel.Background != null)
            // {
            // this.Background = internalModel.Background.ToBrush();
            // }
            // else
            // this.Background = null;

            // Clear the canvas
            this.canvas.Children.Clear();

            if (this.ActualModel != null)
            {
                this.SynchronizeProperties();

                // disconnecting the canvas while updating
#if !NO_DISCONNECT
                int idx = this.grid.Children.IndexOf(this.canvas);
                this.grid.Children.RemoveAt(idx);
#endif
                var wrc = new ShapesRenderContext(this.canvas);
                this.ActualModel.Render(wrc);

#if !NO_DISCONNECT

                // reinsert the canvas again
                this.grid.Children.Insert(idx, this.canvas);
#endif
            }
        }

        /// <summary>
        /// Updates the model.
        ///   If Model==null, an internal model will be created.
        ///   The ActualModel.UpdateModel will be called (updates all series data).
        /// </summary>
        /// <param name="updateData">
        /// if set to <c>true</c>, all data collections will be updated.
        /// </param>
        private void UpdateModel(bool updateData = true)
        {
            // If no model is set, create an internal model and copy the 
            // axes/series/annotations and properties from the WPF objects to the internal model
            if (this.Model == null)
            {
                // Create an internal model
                if (this.internalModel == null)
                {
                    this.internalModel = new PlotModel();
                }

                // Transfer axes, series and properties from 
                // the WPF dependency objects to the internal model
                if (this.Series != null)
                {
                    this.internalModel.Series.Clear();
                    foreach (ISeries s in this.Series)
                    {
                        this.internalModel.Series.Add(s.CreateModel());
                    }
                }

                if (this.Axes != null && this.Axes.Count > 0)
                {
                    this.internalModel.Axes.Clear();

                    foreach (IAxis a in this.Axes)
                    {
                        this.internalModel.Axes.Add(a.CreateModel());
                    }
                }

                if (this.Annotations != null)
                {
                    this.internalModel.Annotations.Clear();

                    foreach (var a in this.Annotations)
                    {
                        this.internalModel.Annotations.Add(a.CreateModel());
                    }
                }
            }

            this.ActualModel.Update(updateData);
        }

        #endregion      
    }
}
