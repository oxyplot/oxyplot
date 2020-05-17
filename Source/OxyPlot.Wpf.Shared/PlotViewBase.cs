// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewBase.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using OxyPlot;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;
    using CursorType = OxyPlot.CursorType;

    /// <summary>
    /// Base class for WPF PlotView implementations.
    /// </summary>
    [TemplatePart(Name = PartGrid, Type = typeof(Grid))]
    public abstract partial class PlotViewBase : Control, IPlotView
    {
        /// <summary>
        /// The Grid PART constant.
        /// </summary>
        protected const string PartGrid = "PART_Grid";

        /// <summary>
        /// The grid.
        /// </summary>
        protected Grid grid;

        /// <summary>
        /// The plot presenter.
        /// </summary>
        protected FrameworkElement plotPresenter;

        /// <summary>
        /// The render context
        /// </summary>
        protected IRenderContext renderContext;

        /// <summary>
        /// The model lock.
        /// </summary>
        private readonly object modelLock = new object();

        /// <summary>
        /// The current tracker.
        /// </summary>
        private FrameworkElement currentTracker;

        /// <summary>
        /// The current tracker template.
        /// </summary>
        private ControlTemplate currentTrackerTemplate;

        /// <summary>
        /// The default plot controller.
        /// </summary>
        private IPlotController defaultController;

        /// <summary>
        /// Indicates whether the <see cref="PlotViewBase"/> was in the visual tree the last time <see cref="Render"/> was called.
        /// </summary>
        private bool isInVisualTree;

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
        /// Initializes static members of the <see cref="PlotViewBase" /> class.
        /// </summary>
        static PlotViewBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlotViewBase), new FrameworkPropertyMetadata(typeof(PlotViewBase)));
            PaddingProperty.OverrideMetadata(typeof(PlotViewBase), new FrameworkPropertyMetadata(new Thickness(8)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotViewBase" /> class.
        /// </summary>
        protected PlotViewBase()
        {
            this.TrackerDefinitions = new ObservableCollection<TrackerDefinition>();
            this.CommandBindings.Add(new CommandBinding(PlotCommands.ResetAxes, (s, e) => this.ResetAllAxes()));
            this.IsManipulationEnabled = true;
            this.LayoutUpdated += this.OnLayoutUpdated;
        }

        /// <summary>
        /// Gets the actual PlotView controller.
        /// </summary>
        /// <value>The actual PlotView controller.</value>
        public IPlotController ActualController => this.Controller ?? (this.defaultController ??= new PlotController());

        /// <inheritdoc/>
        IController IView.ActualController => this.ActualController;

        /// <summary>
        /// Gets the actual model.
        /// </summary>
        /// <value>The actual model.</value>
        public PlotModel ActualModel { get; private set; }

        /// <inheritdoc/>
        Model IView.ActualModel => this.ActualModel;

        /// <summary>
        /// Gets the coordinates of the client area of the view.
        /// </summary>
        public OxyRect ClientArea => new OxyRect(0, 0, this.ActualWidth, this.ActualHeight);

        /// <summary>
        /// Gets the tracker definitions.
        /// </summary>
        /// <value>The tracker definitions.</value>
        public ObservableCollection<TrackerDefinition> TrackerDefinitions { get; }

        /// <summary>
        /// Hides the tracker.
        /// </summary>
        public void HideTracker()
        {
            if (this.currentTracker != null)
            {
                this.overlays.Children.Remove(this.currentTracker);
                this.currentTracker = null;
                this.currentTrackerTemplate = null;
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
        /// Invalidate the PlotView (not blocking the UI thread)
        /// </summary>
        /// <param name="updateData">The update Data.</param>
        public void InvalidatePlot(bool updateData = true)
        {
            if (this.ActualModel == null)
            {
                return;
            }

            lock (this.ActualModel.SyncRoot)
            {
                ((IPlotModel)this.ActualModel).Update(updateData);
            }

            this.BeginInvoke(this.Render);
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.grid = this.GetTemplateChild(PartGrid) as Grid;
            if (this.grid == null)
            {
                return;
            }

            this.plotPresenter = this.CreatePlotPresenter();
            this.grid.Children.Add(this.plotPresenter);
            this.plotPresenter.UpdateLayout();
            this.renderContext = this.CreateRenderContext();

            this.overlays = new Canvas();
            this.grid.Children.Add(this.overlays);

            this.zoomControl = new ContentControl();
            this.zoomControl.Focusable = false;
            this.overlays.Children.Add(this.zoomControl);

            // add additional grid on top of everthing else to fix issue of mouse events getting lost
            // it must be added last so it covers all other controls
            var mouseGrid = new Grid
            {
                Background = Brushes.Transparent // background must be set for hit test to work
            };
            this.grid.Children.Add(mouseGrid);
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
        /// Stores text on the clipboard.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetClipboardText(string text)
        {
            Clipboard.SetText(text);
        }

        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">The cursor type.</param>
        public void SetCursorType(CursorType cursorType)
        {
            this.Cursor = cursorType switch
            {
                CursorType.Pan => this.PanCursor,
                CursorType.ZoomRectangle => this.ZoomRectangleCursor,
                CursorType.ZoomHorizontal => this.ZoomHorizontalCursor,
                CursorType.ZoomVertical => this.ZoomVerticalCursor,
                _ => Cursors.Arrow,
            };
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

            if (!ReferenceEquals(trackerTemplate, this.currentTrackerTemplate))
            {
                this.HideTracker();

                var tracker = new ContentControl { Template = trackerTemplate };
                this.overlays.Children.Add(tracker);
                this.currentTracker = tracker;
                this.currentTrackerTemplate = trackerTemplate;
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
        /// Clears the background of the plot presenter.
        /// </summary>
        protected abstract void ClearBackground();

        /// <summary>
        /// Creates the plot presenter.
        /// </summary>
        /// <returns>The plot presenter.</returns>
        protected abstract FrameworkElement CreatePlotPresenter();

        /// <summary>
        /// Creates the render context.
        /// </summary>
        /// <returns>The render context.</returns>
        protected abstract IRenderContext CreateRenderContext();

        /// <summary>
        /// Called when the model is changed.
        /// </summary>
        protected void OnModelChanged()
        {
            lock (this.modelLock)
            {
                if (this.ActualModel != null)
                {
                    ((IPlotModel)this.ActualModel).AttachPlotView(null);
                    this.ActualModel = null;
                }

                if (this.Model != null)
                {
                    ((IPlotModel)this.Model).AttachPlotView(this);
                    this.ActualModel = this.Model;
                }
            }

            this.InvalidatePlot();
        }

        /// <summary>
        /// Renders the plot model to the plot presenter.
        /// </summary>
        protected void Render()
        {
            if (this.plotPresenter == null || this.renderContext == null || !(this.isInVisualTree = this.IsInVisualTree()))
            {
                return;
            }

            this.RenderOverride();
        }

        /// <summary>
        /// Renders the plot model to the plot presenter.
        /// </summary>
        protected virtual void RenderOverride()
        {
            var dpiScale = this.UpdateDpi();
            this.ClearBackground();

            if (this.ActualModel != null)
            {
                // round width and height to full device pixels
                var width = ((int)(this.plotPresenter.ActualWidth * dpiScale)) / dpiScale;
                var height = ((int)(this.plotPresenter.ActualHeight * dpiScale)) / dpiScale;

                lock (this.ActualModel.SyncRoot)
                {
                    ((IPlotModel)this.ActualModel).Render(this.renderContext, new OxyRect(0, 0, width, height));
                }
            }
        }

        /// <summary>
        /// Updates the DPI scale of the render context.
        /// </summary>
        /// <returns>The DPI scale.</returns>
        protected virtual double UpdateDpi()
        {
            var transformMatrix = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformToDevice;
            var scale = transformMatrix == null ? 1 : (transformMatrix.Value.M11 + transformMatrix.Value.M22) / 2;
            return scale;
        }

        /// <summary>
        /// Called when the model is changed.
        /// </summary>
        /// <param name="d">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotViewBase)d).OnModelChanged();
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

        /// <summary>
        /// Gets a value indicating whether the <see cref="PlotViewBase"/> is connected to the visual tree.
        /// </summary>
        /// <returns><c>true</c> if the PlotViewBase is connected to the visual tree; <c>false</c> otherwise.</returns>
        private bool IsInVisualTree()
        {
            DependencyObject dpObject = this;
            while ((dpObject = VisualTreeHelper.GetParent(dpObject)) != null)
            {
                if (dpObject is Window)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This event fires every time Layout updates the layout of the trees associated with current Dispatcher.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            // if we were not in the visual tree the last time we tried to render but are now, we have to render
            if (!this.isInVisualTree && this.IsInVisualTree())
            {
                this.Render();
            }
        }
    }
}
