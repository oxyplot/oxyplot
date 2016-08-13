// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotBase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Avalonia
{
    using global::Avalonia;
    using global::Avalonia.Controls;
    using global::Avalonia.Controls.Presenters;
    using global::Avalonia.Controls.Primitives;
    using global::Avalonia.Input;
    using global::Avalonia.Input.Platform;
    using global::Avalonia.Threading;
    using global::Avalonia.VisualTree;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using CursorType = OxyPlot.CursorType;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    public abstract partial class PlotBase : TemplatedControl, IPlotView
    {
        /// <summary>
        /// The Grid PART constant.
        /// </summary>
        protected const string PartPanel = "PART_Panel";

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
        private IControl currentTracker;

        /// <summary>
        /// The grid.
        /// </summary>
        private Panel panel;

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
        private Control containerCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotBase" /> class.
        /// </summary>
        protected PlotBase()
        {
            DisconnectCanvasWhileUpdating = true;
            trackerDefinitions = new ObservableCollection<TrackerDefinition>();
            new BoundsTracker().Track(this).Subscribe(bounds => OnSizeChanged(this, bounds?.Bounds.Size ?? new Size()));
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
                return ActualModel;
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
                return ActualController;
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
                return new OxyRect(0, 0, Bounds.Width, Bounds.Height);
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
                return trackerDefinitions;
            }
        }

        /// <summary>
        /// Hides the tracker.
        /// </summary>
        public void HideTracker()
        {
            if (currentTracker != null)
            {
                overlays.Children.Remove(currentTracker);
                currentTracker = null;
            }
        }

        /// <summary>
        /// Hides the zoom rectangle.
        /// </summary>
        public void HideZoomRectangle()
        {
            zoomControl.IsVisible = false;
        }

        /// <summary>
        /// Pans all axes.
        /// </summary>
        /// <param name="delta">The delta.</param>
        public void PanAllAxes(Vector delta)
        {
            if (ActualModel != null)
            {
                ActualModel.PanAllAxes(delta.X, delta.Y);
            }

            InvalidatePlot(false);
        }

        /// <summary>
        /// Zooms all axes.
        /// </summary>
        /// <param name="factor">The zoom factor.</param>
        public void ZoomAllAxes(double factor)
        {
            if (ActualModel != null)
            {
                ActualModel.ZoomAllAxes(factor);
            }

            InvalidatePlot(false);
        }

        /// <summary>
        /// Resets all axes.
        /// </summary>
        public void ResetAllAxes()
        {
            if (ActualModel != null)
            {
                ActualModel.ResetAllAxes();
            }

            InvalidatePlot(false);
        }

        /// <summary>
        /// Invalidate the PlotView (not blocking the UI thread)
        /// </summary>
        /// <param name="updateData">The update Data.</param>
        public void InvalidatePlot(bool updateData = true)
        {
            if (Width <= 0 || Height <= 0)
            {
                return;
            }

            UpdateModel(updateData);

            if (Interlocked.CompareExchange(ref isPlotInvalidated, 1, 0) == 0)
            {
                // Invalidate the arrange state for the element.
                // After the invalidation, the element will have its layout updated,
                // which will occur asynchronously unless subsequently forced by UpdateLayout.
                BeginInvoke(InvalidateArrange);
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass)
        /// call <see cref="M:System.Windows.Controls.Control.ApplyTemplate" /> . In simplest terms, this means the method is called 
        /// just before a UI element displays in an application. For more information, see Remarks.
        /// </summary>
        /// <param name="e">Event data for applying the template.</param>
        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);
            panel = e.NameScope.Find(PartPanel) as Panel;
            if (panel == null)
            {
                return;
            }

            canvas = new Canvas();
            panel.Children.Add(canvas);
            renderContext = new CanvasRenderContext(canvas);

            overlays = new Canvas { Name = "Overlays" };
            panel.Children.Add(overlays);

            zoomControl = new ContentControl();
            overlays.Children.Add(zoomControl);
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
                    Cursor = PanCursor;
                    break;
                case CursorType.ZoomRectangle:
                    Cursor = ZoomRectangleCursor;
                    break;
                case CursorType.ZoomHorizontal:
                    Cursor = ZoomHorizontalCursor;
                    break;
                case CursorType.ZoomVertical:
                    Cursor = ZoomVerticalCursor;
                    break;
                default:
                    Cursor = Cursor.Default;
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
                HideTracker();
                return;
            }

            var trackerTemplate = DefaultTrackerTemplate;
            if (trackerHitResult.Series != null && !string.IsNullOrEmpty(trackerHitResult.Series.TrackerKey))
            {
                var match = TrackerDefinitions.FirstOrDefault(t => t.TrackerKey == trackerHitResult.Series.TrackerKey);
                if (match != null)
                {
                    trackerTemplate = match.TrackerTemplate;
                }
            }

            if (trackerTemplate == null)
            {
                HideTracker();
                return;
            }

            var tracker = trackerTemplate.Build(new ContentControl());

            // ReSharper disable once RedundantNameQualifier
            if (!object.ReferenceEquals(tracker, currentTracker))
            {
                HideTracker();
                overlays.Children.Add(tracker);
                currentTracker = tracker;
            }

            if (currentTracker != null)
            {
                currentTracker.DataContext = trackerHitResult;
            }
        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        public void ShowZoomRectangle(OxyRect r)
        {
            zoomControl.Width = r.Width;
            zoomControl.Height = r.Height;
            Canvas.SetLeft(zoomControl, r.Left);
            Canvas.SetTop(zoomControl, r.Top);
            zoomControl.Template = ZoomRectangleTemplate;
            zoomControl.IsVisible = true;
        }

        /// <summary>
        /// Stores text on the clipboard.
        /// </summary>
        /// <param name="text">The text.</param>
        public async void SetClipboardText(string text)
        {
            await AvaloniaLocator.Current.GetService<IClipboard>().SetTextAsync(text);
        }

        /// <summary>
        /// Provides the behavior for the Arrange pass of Silverlight layout. Classes can override this method to define their own Arrange pass behavior.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this object should use to arrange itself and its children.</param>
        /// <returns>The actual size that is used after the element is arranged in layout.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var actualSize = base.ArrangeOverride(finalSize);
            if (actualSize.Width > 0 && actualSize.Height > 0)
            {
                if (Interlocked.CompareExchange(ref isPlotInvalidated, 0, 1) == 1)
                {
                    UpdateVisuals();
                }
            }

            return actualSize;
        }

        /// <summary>
        /// Updates the model.
        /// </summary>
        /// <param name="updateData">The update Data.</param>
        protected virtual void UpdateModel(bool updateData = true)
        {
            if (ActualModel != null)
            {
                ((IPlotModel)ActualModel).Update(updateData);
            }
        }

        /// <summary>
        /// Determines whether the plot is currently visible to the user.
        /// </summary>
        /// <returns><c>true</c> if the plot is currently visible to the user; otherwise, <c>false</c>.</returns>
        protected bool IsVisibleToUser()
        {
            return IsUserVisible(this);
        }

        /// <summary>
        /// Determines whether the specified element is currently visible to the user.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns><c>true</c> if if the specified element is currently visible to the user; otherwise, <c>false</c>.</returns>
        private static bool IsUserVisible(Control element)
        {
            return element.IsEffectivelyVisible && BoundsTracker.GetTransformedBounds(element).HasValue;
        }

        /// <summary>
        /// Called when the size of the control is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="size">The new size</param>
        private void OnSizeChanged(object sender, Size size)
        {
            if (size.Height > 0 && size.Width > 0)
            {
                InvalidatePlot(false);
            }
        }

        /// <summary>
        /// Gets the relevant parent.
        /// </summary>
        /// <typeparam name="T">Type of the relevant parent</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>The relevant parent.</returns>
        private Control GetRelevantParent<T>(IVisual obj)
            where T : Control
        {
            var container = obj.VisualParent;

            var contentPresenter = container as ContentPresenter;
            if (contentPresenter != null)
            {
                container = GetRelevantParent<T>(contentPresenter);
            }

            var panel = container as Panel;
            if (panel != null)
            {
                container = GetRelevantParent<ScrollViewer>(panel);
            }

            if (!(container is T) && (container != null))
            {
                container = GetRelevantParent<T>(container);
            }

            return (Control)container;
        }

        /// <summary>
        /// Updates the visuals.
        /// </summary>
        private void UpdateVisuals()
        {
            if (canvas == null || renderContext == null)
            {
                return;
            }

            if (!IsVisibleToUser())
            {
                return;
            }

            // Clear the canvas
            canvas.Children.Clear();

            if (ActualModel != null && ActualModel.Background.IsVisible())
            {
                canvas.Background = ActualModel.Background.ToBrush();
            }
            else
            {
                canvas.Background = null;
            }

            if (ActualModel != null)
            {
                if (DisconnectCanvasWhileUpdating)
                {
                    // TODO: profile... not sure if this makes any difference
                    var idx = panel.Children.IndexOf(canvas);
                    if (idx != -1)
                    {
                        panel.Children.RemoveAt(idx);
                    }

                    ((IPlotModel)ActualModel).Render(renderContext, canvas.Bounds.Width, canvas.Bounds.Height);

                    // reinsert the canvas again
                    if (idx != -1)
                    {
                        panel.Children.Insert(idx, canvas);
                    }
                }
                else
                {
                    ((IPlotModel)ActualModel).Render(renderContext, canvas.Bounds.Width, canvas.Bounds.Height);
                }
            }
        }

        /// <summary>
        /// Invokes the specified action on the dispatcher, if necessary.
        /// </summary>
        /// <param name="action">The action.</param>
        private static void BeginInvoke(Action action)
        {
            if (Dispatcher.UIThread.CheckAccess())
            {
                Dispatcher.UIThread.InvokeAsync(action, DispatcherPriority.Loaded);
            }
            else
            {
                action?.Invoke();
            }
        }
    }
}
