// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotBase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using CursorType = OxyPlot.CursorType;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    public abstract partial class PlotBase : Control, IPlotView
    {
        /// <summary>
        /// The Grid PART constant.
        /// </summary>
        protected const string PartGrid = "PART_Grid";

        /// <summary>
        /// The tracker definitions.
        /// </summary>
        private readonly ObservableCollection<TrackerDefinition> trackerDefinitions;

        /// <summary>
        /// The render context
        /// </summary>
        private CanvasRenderContext renderContext;

        /// <summary>
        /// The canvas.
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// The current tracker.
        /// </summary>
        private FrameworkElement currentTracker;

        /// <summary>
        /// The grid.
        /// </summary>
        private Grid grid;

        /// <summary>
        /// Invalidation flag (0: no update, 1: update visual elements).
        /// </summary>
        private int isPlotInvalidated;

        /// <summary>
        /// The mouse down point.
        /// </summary>
        private ScreenPoint mouseDownPoint;

        /// <summary>
        /// The overlays.
        /// </summary>
        private Canvas overlays;

        /// <summary>
        /// The zoom control.
        /// </summary>
        private ContentControl zoomControl;

        /// <summary>
        /// The is visible to user cache.
        /// </summary>
        private bool isVisibleToUserCache;

        /// <summary>
        /// The cached parent.
        /// </summary>
        private FrameworkElement containerCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotBase" /> class.
        /// </summary>
        protected PlotBase()
        {
            this.DisconnectCanvasWhileUpdating = true;
            this.trackerDefinitions = new ObservableCollection<TrackerDefinition>();
            this.Loaded += this.OnLoaded;
            this.LayoutUpdated += this.OnLayoutUpdated;
            this.SizeChanged += this.OnSizeChanged;

            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, this.DoCopy));
            this.CommandBindings.Add(new CommandBinding(PlotCommands.ResetAxes, (s, e) => this.ResetAllAxes()));

            this.IsManipulationEnabled = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to disconnect the canvas while updating.
        /// </summary>
        /// <value><c>true</c> if canvas should be disconnected while updating; otherwise, <c>false</c>.</value>
        public bool DisconnectCanvasWhileUpdating { get; set; }

        /// <summary>
        /// Gets the actual model in the view.
        /// </summary>
        /// <value>
        /// The actual model.
        /// </value>
        Model IView.ActualModel
        {
            get
            {
                return this.ActualModel;
            }
        }

        /// <summary>
        /// Gets the actual model.
        /// </summary>
        /// <value>The actual model.</value>
        public abstract PlotModel ActualModel { get; }

        /// <summary>
        /// Gets the actual controller.
        /// </summary>
        /// <value>
        /// The actual <see cref="IController" />.
        /// </value>
        IController IView.ActualController
        {
            get
            {
                return this.ActualController;
            }
        }

        /// <summary>
        /// Gets the actual PlotView controller.
        /// </summary>
        /// <value>The actual PlotView controller.</value>
        public abstract IPlotController ActualController { get; }

        /// <summary>
        /// Gets the coordinates of the client area of the view.
        /// </summary>
        public OxyRect ClientArea
        {
            get
            {
                return new OxyRect(0, 0, this.ActualWidth, this.ActualHeight);
            }
        }

        /// <summary>
        /// Gets the tracker definitions.
        /// </summary>
        /// <value>The tracker definitions.</value>
        public ObservableCollection<TrackerDefinition> TrackerDefinitions
        {
            get
            {
                return this.trackerDefinitions;
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
        /// Pans all axes.
        /// </summary>
        /// <param name="delta">The delta.</param>
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
        /// <param name="factor">The zoom factor.</param>
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
        /// Invalidate the PlotView (not blocking the UI thread)
        /// </summary>
        /// <param name="updateData">The update Data.</param>
        public void InvalidatePlot(bool updateData = true)
        {
            if (this.ActualWidth <= 0 || this.ActualHeight <= 0)
            {
                return;
            }

            this.UpdateModel(updateData);

            if (Interlocked.CompareExchange(ref this.isPlotInvalidated, 1, 0) == 0)
            {
                // Invalidate the arrange state for the element.
                // After the invalidation, the element will have its layout updated,
                // which will occur asynchronously unless subsequently forced by UpdateLayout.
                this.BeginInvoke(this.InvalidateArrange);
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass)
        /// call <see cref="M:System.Windows.Controls.Control.ApplyTemplate" /> . In simplest terms, this means the method is called 
        /// just before a UI element displays in an application. For more information, see Remarks.
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
            this.renderContext = new CanvasRenderContext(this.canvas);

            this.overlays = new Canvas();
            this.grid.Children.Add(this.overlays);

            this.zoomControl = new ContentControl();
            this.overlays.Children.Add(this.zoomControl);

            // add additional grid on top of everthing else to fix issue of mouse events getting lost
            // it must be added last so it covers all other controls
            var mouseGrid = new Grid();
            mouseGrid.Background = Brushes.Transparent; // background must be set for hit test to work
            this.grid.Children.Add(mouseGrid); 
        }

        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">The cursor type.</param>
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
        /// <param name="trackerHitResult">The tracker data.</param>
        public void ShowTracker(TrackerHitResult trackerHitResult)
        {
            if (trackerHitResult == null)
            {
                this.HideTracker();
                return;
            }

            var trackerTemplate = this.DefaultTrackerTemplate;
            if (trackerHitResult.Series != null && !string.IsNullOrEmpty(trackerHitResult.Series.TrackerKey))
            {
                var match = this.TrackerDefinitions.FirstOrDefault(t => t.TrackerKey == trackerHitResult.Series.TrackerKey);
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

            // ReSharper disable once RedundantNameQualifier
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
        /// <param name="r">The rectangle.</param>
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
        /// Stores text on the clipboard.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetClipboardText(string text)
        {
            Clipboard.SetText(text);
        }

        /// <summary>
        /// Provides the behavior for the Arrange pass of Silverlight layout. Classes can override this method to define their own Arrange pass behavior.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this object should use to arrange itself and its children.</param>
        /// <returns>The actual size that is used after the element is arranged in layout.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.ActualWidth > 0 && this.ActualHeight > 0)
            {
                if (Interlocked.CompareExchange(ref this.isPlotInvalidated, 0, 1) == 1)
                {
                    this.UpdateVisuals();
                }
            }

            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// Updates the model.
        /// </summary>
        /// <param name="updateData">The update Data.</param>
        protected virtual void UpdateModel(bool updateData = true)
        {
            if (this.ActualModel != null)
            {
                ((IPlotModel)this.ActualModel).Update(updateData);
            }
        }

        /// <summary>
        /// Determines whether the plot is currently visible to the user.
        /// </summary>
        /// <returns><c>true</c> if the plot is currently visible to the user; otherwise, <c>false</c>.</returns>
        protected bool IsVisibleToUser()
        {
            var container = this.containerCache;
            if (container == null)
            {
                container = this.GetRelevantParent<FrameworkElement>(this);
                if (container != null)
                {
                    this.containerCache = container;
                }
            }

            if (container != null)
            {
                var visible = this.IsUserVisible(this, container);
                if (!visible)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified element is currently visible to the user.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="container">The container.</param>
        /// <returns><c>true</c> if if the specified element is currently visible to the user; otherwise, <c>false</c>.</returns>
        private bool IsUserVisible(FrameworkElement element, FrameworkElement container)
        {
            if (!container.IsVisible || !element.IsVisible)
            {
                return false;
            }

            var bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            var rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight);
        }

        /// <summary>
        /// Performs the copy operation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.ExecutedRoutedEventArgs" /> instance containing the event data.</param>
        private void DoCopy(object sender, ExecutedRoutedEventArgs e)
        {
            var background = this.ActualModel.Background.IsVisible() ? this.ActualModel.Background : this.Background.ToOxyColor();
            if (background.IsInvisible())
            {
                background = OxyColors.White;
            }

            var bitmap = PngExporter.ExportToBitmap(
                this.ActualModel, (int)this.ActualWidth, (int)this.ActualHeight, background);
            Clipboard.SetImage(bitmap);
        }

        /// <summary>
        /// Called when the control is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" /> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Make sure InvalidateArrange is called when the PlotView is invalidated
            Interlocked.Exchange(ref this.isPlotInvalidated, 0);
            this.InvalidatePlot();
        }

        /// <summary>
        /// Called when the layout is updated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            var oldValue = this.isVisibleToUserCache;
            var newValue = this.IsVisibleToUser();

            if (oldValue != newValue)
            {
                this.isVisibleToUserCache = newValue;
                Interlocked.Exchange(ref this.isPlotInvalidated, 1);
                this.InvalidateVisual();
            }
        }

        /// <summary>
        /// Called when the size of the control is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs" /> instance containing the event data.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height > 0 && e.NewSize.Width > 0)
            {
                this.InvalidatePlot(false);
            }
        }

        /// <summary>
        /// Gets the relevant parent.
        /// </summary>
        /// <typeparam name="T">Type of the relevant parent</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>The relevant parent.</returns>
        private FrameworkElement GetRelevantParent<T>(DependencyObject obj)
            where T : FrameworkElement
        {
            var container = VisualTreeHelper.GetParent(obj) as FrameworkElement;

            var contentPresenter = container as ContentPresenter;
            if (contentPresenter != null)
            {
                container = this.GetRelevantParent<T>(contentPresenter);
            }

            var panel = container as Panel;
            if (panel != null)
            {
                container = this.GetRelevantParent<ScrollViewer>(panel);
            }

            if (!(container is T) && (container != null))
            {
                container = this.GetRelevantParent<T>(container);
            }

            return container;
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

            if (!this.isVisibleToUserCache)
            {
                return;
            }

            // Clear the canvas
            this.canvas.Children.Clear();

            if (this.ActualModel != null && this.ActualModel.Background.IsVisible())
            {
                this.canvas.Background = this.ActualModel.Background.ToBrush();
            }
            else
            {
                this.canvas.Background = null;
            }

            if (this.ActualModel != null)
            {
                if (this.DisconnectCanvasWhileUpdating)
                {
                    // TODO: profile... not sure if this makes any difference
                    int idx = this.grid.Children.IndexOf(this.canvas);
                    if (idx != -1)
                    {
                        this.grid.Children.RemoveAt(idx);
                    }

                    ((IPlotModel)this.ActualModel).Render(this.renderContext, this.canvas.ActualWidth, this.canvas.ActualHeight);

                    // reinsert the canvas again
                    if (idx != -1)
                    {
                        this.grid.Children.Insert(idx, this.canvas);
                    }
                }
                else
                {
                    ((IPlotModel)this.ActualModel).Render(this.renderContext, this.canvas.ActualWidth, this.canvas.ActualHeight);
                }
            }
        }

        /// <summary>
        /// Invokes the specified action on the dispatcher, if necessary.
        /// </summary>
        /// <param name="action">The action.</param>
        private void BeginInvoke(Action action)
        {
            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, action);
            }
            else
            {
                action();
            }
        }
    }
}
