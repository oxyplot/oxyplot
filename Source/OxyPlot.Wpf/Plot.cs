// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Plot.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Represents a WPF control that displays an OxyPlot plot.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
        ///   The Grid PART constant.
        /// </summary>
        private const string PartGrid = "PART_Grid";

        /// <summary>
        ///   The pan action.
        /// </summary>
        private readonly PanAction panAction;

        /// <summary>
        ///   The tracker action.
        /// </summary>
        private readonly TrackerAction trackerAction;

        /// <summary>
        ///   The tracker definitions.
        /// </summary>
        private readonly ObservableCollection<TrackerDefinition> trackerDefinitions;

        /// <summary>
        ///   The zoom action.
        /// </summary>
        private readonly ZoomAction zoomAction;

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
        /// Flag to update data when the plot has been invalidated.
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
        }

#endif

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Plot" /> class.
        /// </summary>
        public Plot()
        {
            this.panAction = new PanAction(this);
            this.zoomAction = new ZoomAction(this);
            this.trackerAction = new TrackerAction(this);

            this.MouseActions = new List<OxyMouseAction> { this.panAction, this.zoomAction, this.trackerAction };

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
        /// Gets the annotations.
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
        ///   Gets the mouse actions.
        /// </summary>
        /// <value>The mouse actions.</value>
        public List<OxyMouseAction> MouseActions { get; private set; }

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
        }

        /// <summary>
        /// Zooms to fit all content of the plot.
        /// </summary>
        public void ZoomAll()
        {
            foreach (OxyPlot.IAxis a in this.ActualModel.Axes)
            {
                a.Reset();
            }

            this.RefreshPlot(false);
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
        public void ZoomAt(OxyPlot.IAxis axis, double factor, double x)
        {
            axis.ZoomAt(factor, x);
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
                this.ZoomAll();
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
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseDown"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state.
        /// </param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            this.Focus();
            this.CaptureMouse();

            bool control = Keyboard.IsKeyDown(Key.LeftCtrl);
            bool shift = Keyboard.IsKeyDown(Key.LeftShift);
            bool alt = Keyboard.IsKeyDown(Key.LeftAlt);

            OxyMouseButton button = OxyMouseButton.Left;
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                button = OxyMouseButton.Middle;
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                button = OxyMouseButton.Right;
            }

            if (e.XButton1 == MouseButtonState.Pressed)
            {
                button = OxyMouseButton.XButton1;
            }

            if (e.XButton2 == MouseButtonState.Pressed)
            {
                button = OxyMouseButton.XButton2;
            }

            ScreenPoint p = e.GetPosition(this).ToScreenPoint();
            foreach (OxyMouseAction a in this.MouseActions)
            {
                a.OnMouseDown(p, button, e.ClickCount, control, shift, alt);
            }

            this.mouseDownPoint = p;
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

            bool control = Keyboard.IsKeyDown(Key.LeftCtrl);
            bool shift = Keyboard.IsKeyDown(Key.LeftShift);
            bool alt = Keyboard.IsKeyDown(Key.LeftAlt);

            ScreenPoint p = e.GetPosition(this).ToScreenPoint();

            foreach (OxyMouseAction a in this.MouseActions)
            {
                a.OnMouseMove(p, control, shift, alt);
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

            foreach (OxyMouseAction a in this.MouseActions)
            {
                a.OnMouseUp();
            }

            this.ReleaseMouseCapture();
            ScreenPoint p = e.GetPosition(this).ToScreenPoint();

            double d = p.DistanceTo(this.mouseDownPoint);
            if (this.ContextMenu != null)
            {
                if (d == 0 && e.ChangedButton == MouseButton.Right)
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
            bool isShiftDown = Keyboard.IsKeyDown(Key.LeftShift);
            bool isAltDown = Keyboard.IsKeyDown(Key.LeftAlt);

            ScreenPoint p = e.GetPosition(this).ToScreenPoint();

            foreach (OxyMouseAction a in this.MouseActions)
            {
                a.OnMouseWheel(p, e.Delta, isControlDown, isShiftDown, isAltDown);
            }
        }

        /// <summary>
        /// Called when the model is changed.
        /// </summary>
        /// <param name="d">
        /// The d.
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
        /// Called when the visual appearance is changed.
        /// </summary>
        protected virtual void OnAppearanceChanged()
        {
            this.InvalidatePlot();
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
                foreach (object item in e.NewItems)
                {
                    this.AddLogicalChild(item);
                }
            }

            if (e.OldItems != null)
            {
                foreach (object item in e.OldItems)
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
        /// If Model==null, an internal model will be created.
        /// The ActualModel.UpdateModel will be called (updates all series data).
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

                    foreach (Annotation a in this.Annotations)
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