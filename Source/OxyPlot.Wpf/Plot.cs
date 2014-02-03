// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Plot.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents a WPF control that displays an OxyPlot plot.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;

    using OxyPlot.Series;

    using CursorType = OxyPlot.CursorType;

    /// <summary>
    /// Represents a WPF control that displays an OxyPlot plot.
    /// </summary>
    [ContentProperty("Series")]
    [TemplatePart(Name = PartGrid, Type = typeof(Grid))]
    public partial class Plot : RenderingControl, IPlotControl
    {
        /// <summary>
        /// The Grid PART constant.
        /// </summary>
        private const string PartGrid = "PART_Grid";

        /// <summary>
        /// The model lock.
        /// </summary>
        private readonly object modelLock = new object();

        /// <summary>
        /// The tracker definitions.
        /// </summary>
        private readonly ObservableCollection<TrackerDefinition> trackerDefinitions;

        /// <summary>
        /// The update model and visuals lock.
        /// </summary>
        private readonly object updateModelAndVisualsLock = new object();

        /// <summary>
        /// The canvas.
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// The current model.
        /// </summary>
        private PlotModel currentModel;

        /// <summary>
        /// The current tracker.
        /// </summary>
        private FrameworkElement currentTracker;

        /// <summary>
        /// The grid.
        /// </summary>
        private Grid grid;

        /// <summary>
        /// The internal model.
        /// </summary>
        private PlotModel internalModel;

        /// <summary>
        /// Invalidation flag (0: no update, 1: update visual elements only, 2:update data).
        /// </summary>
        private int isPlotInvalidated;

        /// <summary>
        /// The is rendering flag.
        /// </summary>
        private bool isRendering;

        /// <summary>
        /// The mouse down point.
        /// </summary>
        private ScreenPoint mouseDownPoint;

        /// <summary>
        /// The mouse manipulator.
        /// </summary>
        private ManipulatorBase mouseManipulator;

        /// <summary>
        /// The overlays.
        /// </summary>
        private Canvas overlays;

        /// <summary>
        /// The render context
        /// </summary>
        private ShapesRenderContext renderContext;

        /// <summary>
        /// The zoom control.
        /// </summary>
        private ContentControl zoomControl;

        /// <summary>
        /// Initializes static members of the <see cref="Plot"/> class.
        /// </summary>
        static Plot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Plot), new FrameworkPropertyMetadata(typeof(Plot)));
            PaddingProperty.OverrideMetadata(
                typeof(Plot), new FrameworkPropertyMetadata(new Thickness(8, 8, 16, 8), AppearanceChanged));
            ResetAxesCommand = new RoutedCommand();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plot"/> class.
        /// </summary>
        public Plot()
        {
            this.DisconnectCanvasWhileUpdating = true;
            this.series = new ObservableCollection<Series>();
            this.axes = new ObservableCollection<Axis>();
            this.annotations = new ObservableCollection<Annotation>();
            this.trackerDefinitions = new ObservableCollection<TrackerDefinition>();

            this.series.CollectionChanged += this.OnSeriesChanged;
            this.axes.CollectionChanged += this.OnAxesChanged;
            this.annotations.CollectionChanged += this.OnAnnotationsChanged;

            this.DataContextChanged += this.OnDataContextChanged;
            this.SizeChanged += this.OnSizeChanged;

            this.Loaded += this.PlotLoaded;
            this.Unloaded += this.PlotUnloaded;

            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, this.DoCopy));

#if !NET35
            this.IsManipulationEnabled = true;
#endif

            // this.CommandBindings.Add(new CommandBinding(CopyCode, this.DoCopyCode));
            // this.InputBindings.Add(new KeyBinding(CopyCode, Key.C, ModifierKeys.Control | ModifierKeys.Alt));
        }

        /// <summary>
        /// Gets the annotations.
        /// </summary>
        /// <value> The annotations. </value>
        public ObservableCollection<Annotation> Annotations
        {
            get
            {
                return this.annotations;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to disconnect the canvas while updating.
        /// </summary>
        /// <value>
        /// <c>true</c> if canvas should be disconnected while updating; otherwise, <c>false</c>.
        /// </value>
        public bool DisconnectCanvasWhileUpdating { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is being rendered.
        /// </summary>
        /// <remarks>
        /// When the visual is removed from the visual tree, this property should be set to false.
        /// </remarks>
        public bool IsRendering
        {
            get
            {
                return this.isRendering;
            }

            set
            {
                if (value != this.isRendering)
                {
                    this.isRendering = value;
                    if (this.isRendering)
                    {
                        this.SubscribeToRenderingEvent();
                    }
                    else
                    {
                        this.UnsubscribeRenderingEvent();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the tracker definitions.
        /// </summary>
        /// <value> The tracker definitions. </value>
        public ObservableCollection<TrackerDefinition> TrackerDefinitions
        {
            get
            {
                return this.trackerDefinitions;
            }
        }

        /// <summary>
        /// Gets the actual model.
        /// </summary>
        /// <value> The actual model. </value>
        public PlotModel ActualModel
        {
            get
            {
                return this.Model ?? this.internalModel;
            }
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
            if (updateData)
            {
                this.isPlotInvalidated = 2;
            }
            else
            {
                Interlocked.CompareExchange(ref this.isPlotInvalidated, 1, 0);
            }

            this.Invoke(this.InvalidateArrange);
        }

        /// <summary>
        /// Refresh the plot immediately (blocking UI thread)
        /// </summary>
        /// <param name="updateData">
        /// if set to <c>true</c> , the data collections will be updated.
        /// </param>
        public void RefreshPlot(bool updateData)
        {
            this.InvalidatePlot(updateData);
        }

        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">
        /// The cursor type.
        /// </param>
        public void SetCursorType(CursorType cursorType)
        {
            switch (cursorType)
            {
                case CursorType.Pan:
                    this.Cursor = this.PanCursor;
                    break;
                case CursorType.ZoomRectangle:
                    this.Cursor = this.ZoomRectangleCursor;
                    break;
                case CursorType.ZoomHorizontal:
                    this.Cursor = this.ZoomHorizontalCursor;
                    break;
                case CursorType.ZoomVertical:
                    this.Cursor = this.ZoomVerticalCursor;
                    break;
                default:
                    this.Cursor = Cursors.Arrow;
                    break;
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
            if (trackerHitResult == null)
            {
                this.HideTracker();
                return;
            }

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

            if (trackerTemplate == null)
            {
                this.HideTracker();
                return;
            }

            var tracker = new ContentControl { Template = trackerTemplate };

            if (!object.ReferenceEquals(tracker, this.currentTracker))
            {
                this.HideTracker();
                this.overlays.Children.Add(tracker);
                this.currentTracker = tracker;
            }

            if (this.currentTracker != null)
            {
                this.currentTracker.DataContext = trackerHitResult;
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
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/> .
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
            // this.canvas.CacheMode = new BitmapCache();
            this.grid.Children.Add(this.canvas);
            this.canvas.UpdateLayout();
            this.renderContext = new ShapesRenderContext(this.canvas);

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
        /// Pans all axes.
        /// </summary>
        /// <param name="delta">
        /// The delta.
        /// </param>
        public void PanAllAxes(Vector delta)
        {
            if (this.ActualModel != null)
            {
                this.ActualModel.PanAllAxes(delta.X, delta.Y);
            }

            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Zooms all axes.
        /// </summary>
        /// <param name="factor">
        /// The zoom factor.
        /// </param>
        public void ZoomAllAxes(double factor)
        {
            if (this.ActualModel != null)
            {
                this.ActualModel.ZoomAllAxes(factor);
            }

            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Resets all axes.
        /// </summary>
        public void ResetAllAxes()
        {
            if (this.ActualModel != null)
            {
                this.ActualModel.ResetAllAxes();
            }

            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Saves the plot as a bitmap.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="background">
        /// The background.
        /// </param>
        public void SaveBitmap(string fileName, int width, int height, OxyColor background)
        {
            // var tmp = this.Model;
            // this.Model = null;
            if (width == 0)
            {
                width = (int)this.ActualWidth;
            }

            if (height == 0)
            {
                height = (int)this.ActualHeight;
            }

            if (background.IsAutomatic())
            {
                background = this.Background.ToOxyColor();
            }

            PngExporter.Export(this.ActualModel, fileName, width, height, background);
        }

        /// <summary>
        /// Saves the plot as xaml.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        public void SaveXaml(string fileName)
        {
            XamlExporter.Export(
                this.ActualModel, fileName, this.ActualWidth, this.ActualHeight, this.Background.ToOxyColor());
        }

        /// <summary>
        /// Renders the plot to a bitmap.
        /// </summary>
        /// <returns>
        /// A bitmap.
        /// </returns>
        public BitmapSource ToBitmap()
        {
            return PngExporter.ExportToBitmap(
                this.ActualModel, (int)this.ActualWidth, (int)this.ActualHeight, this.Background.ToOxyColor());
        }

        /// <summary>
        /// Renders the plot to xaml.
        /// </summary>
        /// <returns>
        /// The xaml.
        /// </returns>
        public string ToXaml()
        {
            return XamlExporter.ExportToString(
                this.ActualModel, this.ActualWidth, this.ActualHeight, this.Background.ToOxyColor());
        }

        /// <summary>
        /// Called to arrange and size the content of a <see cref="T:System.Windows.Controls.Control" /> object.
        /// </summary>
        /// <param name="arrangeBounds">The computed size that is used to arrange the content.</param>
        /// <returns>
        /// The size of the control.
        /// </returns>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (this.ActualWidth > 0 && this.ActualHeight > 0)
            {
                if (Interlocked.CompareExchange(ref this.isPlotInvalidated, 0, 1) == 1)
                {
                    this.UpdateModelAndVisuals(false);
                }
                else
                {
                    if (Interlocked.CompareExchange(ref this.isPlotInvalidated, 0, 2) == 2)
                    {
                        this.UpdateModelAndVisuals(true);
                    }
                }
            }

            return base.ArrangeOverride(arrangeBounds);
        }

        /// <summary>
        /// Called when the visual appearance is changed.
        /// </summary>
        protected void OnAppearanceChanged()
        {
            this.InvalidatePlot();
        }

        /// <summary>
        /// Invoked when an unhandled KeyDown attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.KeyEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Handled)
            {
                return;
            }

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

                this.PanAllAxes(delta);
                e.Handled = true;
            }

            if (Math.Abs(zoom) > 1e-8)
            {
                if (control)
                {
                    zoom *= 0.2;
                }

                this.ZoomAllAxes(1 + (zoom * 0.12));
                e.Handled = true;
            }

            if (control && alt && this.ActualModel != null)
            {
                switch (e.Key)
                {
                    case Key.R:
                        Clipboard.SetText(this.ActualModel.CreateTextReport());
                        e.Handled = true;
                        break;
                    case Key.C:
                        Clipboard.SetText(this.ActualModel.ToCode());
                        e.Handled = true;
                        break;
                    case Key.X:
                        Clipboard.SetText(this.ToXaml());
                        e.Handled = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Invoked when an unhandled MouseDown attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state.
        /// </param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Handled)
            {
                return;
            }

            if (this.mouseManipulator != null)
            {
                return;
            }

            this.Focus();
            this.CaptureMouse();

            this.mouseDownPoint = e.GetPosition(this).ToScreenPoint();

            if (this.ActualModel != null)
            {
                var args = this.CreateMouseEventArgs(e);
                this.ActualModel.HandleMouseDown(this, args);
                if (args.Handled)
                {
                    return;
                }
            }

            this.mouseManipulator = this.GetManipulator(e);

            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Started(this.CreateManipulationEventArgs(e));
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when an unhandled MouseMove attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.MouseEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Handled)
            {
                return;
            }

            if (this.ActualModel != null)
            {
                var args = this.CreateMouseEventArgs(e);
                this.ActualModel.HandleMouseMove(this, args);
                if (args.Handled)
                {
                    return;
                }
            }

            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Delta(this.CreateManipulationEventArgs(e));
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when an unhandled MouseUp routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the mouse button was released.
        /// </param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Handled)
            {
                return;
            }

            this.ReleaseMouseCapture();

            if (this.ActualModel != null)
            {
                var args = this.CreateMouseEventArgs(e);
                this.ActualModel.HandleMouseUp(this, args);
                if (args.Handled)
                {
                    return;
                }
            }

            if (this.mouseManipulator != null)
            {
                this.mouseManipulator.Completed(this.CreateManipulationEventArgs(e));
                e.Handled = true;
            }

            this.mouseManipulator = null;

            var p = e.GetPosition(this).ToScreenPoint();
            double d = p.DistanceTo(this.mouseDownPoint);

            if (this.ContextMenu != null)
            {
                if (Math.Abs(d) < 1e-8 && e.ChangedButton == MouseButton.Right)
                {
                    // TODO: why is the data context not passed to the context menu??
                    this.ContextMenu.DataContext = this.DataContext;

                    this.ContextMenu.Visibility = Visibility.Visible;
                    this.ContextMenu.IsOpen = true;
                }
                else
                {
                    this.ContextMenu.Visibility = Visibility.Collapsed;
                    this.ContextMenu.IsOpen = false;
                }
            }
        }

        /// <summary>
        /// Invoked when an unhandled MouseWheel attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.MouseWheelEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Handled)
            {
                return;
            }

            if (!this.IsMouseWheelEnabled)
            {
                return;
            }

            bool isControlDown = Keyboard.IsKeyDown(Key.LeftCtrl);

            //// bool isShiftDown = Keyboard.IsKeyDown(Key.LeftShift);
            //// bool isAltDown = Keyboard.IsKeyDown(Key.LeftAlt);
            var m = new ZoomStepManipulator(this, e.Delta * 0.001, isControlDown);
            m.Started(new ManipulationEventArgs(e.GetPosition(this).ToScreenPoint()));
            e.Handled = true;
        }

        /// <summary>
        /// Called when the parent of visual object is changed.
        /// </summary>
        /// <param name="oldParent">A value of type <see cref="T:System.Windows.DependencyObject"/> that represents the previous parent of the <see cref="T:System.Windows.Media.Media3D.Visual3D"/> object. If the <see cref="T:System.Windows.Media.Media3D.Visual3D"/> object did not have a previous parent, the value of the parameter is null.</param>
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            var parent = VisualTreeHelper.GetParent(this);
            this.IsRendering = parent != null;
        }

        /// <summary>
        /// Handles the Loaded event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        protected virtual void PlotLoaded(object sender, RoutedEventArgs e)
        {
            this.IsRendering = true;
            this.OnModelChanged();
        }

        /// <summary>
        /// Handles the Unloaded event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        protected virtual void PlotUnloaded(object sender, RoutedEventArgs e)
        {
            this.IsRendering = false;

            if (this.currentModel != null)
            {
                this.currentModel.AttachPlotControl(null);
                this.currentModel = null;
            }
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
        /// Converts the changed button.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.
        /// </param>
        /// <returns>
        /// The mouse button.
        /// </returns>
        private static OxyMouseButton ConvertChangedButton(MouseEventArgs e)
        {
            var mbe = e as MouseButtonEventArgs;
            if (mbe != null)
            {
                switch (mbe.ChangedButton)
                {
                    case MouseButton.Left:
                        return OxyMouseButton.Left;
                    case MouseButton.Middle:
                        return OxyMouseButton.Middle;
                    case MouseButton.Right:
                        return OxyMouseButton.Right;
                    case MouseButton.XButton1:
                        return OxyMouseButton.XButton1;
                    case MouseButton.XButton2:
                        return OxyMouseButton.XButton2;
                }
            }

            return OxyMouseButton.Left;
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
        /// Creates the manipulation event arguments.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.
        /// </param>
        /// <returns>
        /// A manipulation event arguments object.
        /// </returns>
        private ManipulationEventArgs CreateManipulationEventArgs(MouseEventArgs e)
        {
            return new ManipulationEventArgs(e.GetPosition(this).ToScreenPoint());
        }

        /// <summary>
        /// Creates the mouse event arguments.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.
        /// </param>
        /// <returns>
        /// Mouse event arguments.
        /// </returns>
        private OxyMouseEventArgs CreateMouseEventArgs(MouseEventArgs e)
        {
            return new OxyMouseEventArgs
                {
                    ChangedButton = ConvertChangedButton(e),
                    Position = e.GetPosition(this).ToScreenPoint(),
                    IsShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift),
                    IsControlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl),
                    IsAltDown = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt),
                };
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
            var background = ReferenceEquals(this.Background, Brushes.Transparent) ? Brushes.White : this.Background;
            var bitmap = PngExporter.ExportToBitmap(
                this.ActualModel, (int)this.ActualWidth, (int)this.ActualHeight, background.ToOxyColor());
            Clipboard.SetImage(bitmap);
        }

        /// <summary>
        /// Gets the manipulator for the current mouse button and modifier keys.
        /// </summary>
        /// <param name="e">
        /// The event args.
        /// </param>
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
        /// Invokes the specified action on the dispatcher, if necessary.
        /// </summary>
        /// <param name="action">The action.</param>
        private void Invoke(Action action)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, action);
            }
            else
            {
                action();
            }
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
        /// Called when the model is changed.
        /// </summary>
        private void OnModelChanged()
        {
            lock (this.modelLock)
            {
                if (this.currentModel != null)
                {
                    this.currentModel.AttachPlotControl(null);
                }

                if (this.Model != null)
                {
                    if (this.Model.PlotControl != null)
                    {
                        throw new InvalidOperationException(
                            "This PlotModel is already in use by some other plot control.");
                    }

                    if (this.IsLoaded)
                    {
                        this.currentModel = this.Model;
                        this.currentModel.AttachPlotControl(this);
                    }
                }
            }

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
            this.InvalidatePlot(false);
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
        /// Updates the model. If Model==null, an internal model will be created. The ActualModel.UpdateModelAndVisuals will be called (updates all series data).
        /// </summary>
        /// <param name="updateData">
        /// if set to <c>true</c> , all data collections will be updated.
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
                    foreach (var s in this.Series)
                    {
                        this.internalModel.Series.Add(s.CreateModel());
                    }
                }

                if (this.Axes != null && this.Axes.Count > 0)
                {
                    this.internalModel.Axes.Clear();

                    foreach (var a in this.Axes)
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

        /// <summary>
        /// Updates the model. If Model==null, an internal model will be created. The ActualModel.UpdateModel will be called (updates all series data).
        /// </summary>
        /// <param name="updateData">
        /// if set to <c>true</c> , all data collections will be updated.
        /// </param>
        private void UpdateModelAndVisuals(bool updateData)
        {
            lock (this.updateModelAndVisualsLock)
            {
                this.UpdateModel(updateData);
                this.UpdateVisuals();
            }
        }

        /// <summary>
        /// Updates the visuals.
        /// </summary>
        private void UpdateVisuals()
        {
            if (this.canvas == null || this.renderContext == null)
            {
                return;
            }

            // Clear the canvas
            this.canvas.Children.Clear();

            if (this.ActualModel != null)
            {
                this.SynchronizeProperties();

                if (this.DisconnectCanvasWhileUpdating)
                {
                    // TODO: profile... not sure if this makes any difference                    
                    int idx = this.grid.Children.IndexOf(this.canvas);
                    if (idx != -1)
                    {
                        this.grid.Children.RemoveAt(idx);
                    }

                    this.ActualModel.Render(this.renderContext, this.canvas.ActualWidth, this.canvas.ActualHeight);

                    // reinsert the canvas again
                    if (idx != -1)
                    {
                        this.grid.Children.Insert(idx, this.canvas);
                    }
                }
                else
                {
                    this.ActualModel.Render(this.renderContext, this.canvas.ActualWidth, this.canvas.ActualHeight);
                }
            }
        }
    }
}