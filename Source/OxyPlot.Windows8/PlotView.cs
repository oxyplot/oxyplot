// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Windows
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;

    using global::Windows.ApplicationModel;
    using global::Windows.ApplicationModel.DataTransfer;
    using global::Windows.Devices.Input;
    using global::Windows.Foundation;
    using global::Windows.System;
    using global::Windows.UI.Core;
    using global::Windows.UI.Xaml;
    using global::Windows.UI.Xaml.Controls;
    using global::Windows.UI.Xaml.Input;
    using global::Windows.UI.Xaml.Media.Imaging;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    [TemplatePart(Name = PartGrid, Type = typeof(Grid))]
    public class PlotView : Control, IPlotView
    {
        /// <summary>
        /// Identifies the <see cref="Controller"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register("Controller", typeof(IPlotController), typeof(PlotView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="DefaultTrackerTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultTrackerTemplateProperty =
            DependencyProperty.Register(
                "DefaultTrackerTemplate", typeof(ControlTemplate), typeof(PlotView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="HandleRightClicks"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HandleRightClicksProperty =
            DependencyProperty.Register("HandleRightClicks", typeof(bool), typeof(PlotView), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="IsMouseWheelEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsMouseWheelEnabledProperty =
            DependencyProperty.Register("IsMouseWheelEnabled", typeof(bool), typeof(PlotView), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="Model"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
            "Model", typeof(PlotModel), typeof(PlotView), new PropertyMetadata(null, ModelChanged));

        /// <summary>
        /// Identifies the <see cref="ZoomRectangleTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomRectangleTemplateProperty =
            DependencyProperty.Register(
                "ZoomRectangleTemplate", typeof(ControlTemplate), typeof(PlotView), new PropertyMetadata(null));

        /// <summary>
        /// Flags if the cursor is not implemented (Windows Phone).
        /// </summary>
        private static bool cursorNotImplemented;

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
        /// The default controller.
        /// </summary>
        private IPlotController defaultController;

        /// <summary>
        /// The state of the Alt key.
        /// </summary>
        private bool isAltPressed;

        /// <summary>
        /// The state of the Windows key.
        /// </summary>
        private bool isWindowsPressed;

        /// <summary>
        /// The state of the Control key.
        /// </summary>
        private bool isControlPressed;

        /// <summary>
        /// The is PlotView invalidated.
        /// </summary>
        private int isPlotInvalidated;

        /// <summary>
        /// The is shift pressed.
        /// </summary>
        private bool isShiftPressed;

        /// <summary>
        /// The overlays.
        /// </summary>
        private Canvas overlays;

        /// <summary>
        /// The render context
        /// </summary>
        private RenderContext renderContext;

        /// <summary>
        /// The zoom control.
        /// </summary>
        private ContentControl zoomRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref = "PlotView" /> class.
        /// </summary>
        public PlotView()
        {
            this.DefaultStyleKey = typeof(PlotView);

            this.trackerDefinitions = new ObservableCollection<TrackerDefinition>();
            this.Loaded += this.OnLoaded;
            this.SizeChanged += this.OnSizeChanged;
            this.ManipulationMode = ManipulationModes.Scale | ManipulationModes.TranslateX
                                    | ManipulationModes.TranslateY;
        }

        /// <summary>
        /// Gets or sets the PlotView controller.
        /// </summary>
        /// <value>The PlotView controller.</value>
        public IPlotController Controller
        {
            get { return (IPlotController)this.GetValue(ControllerProperty); }
            set { this.SetValue(ControllerProperty, value); }
        }

        /// <summary>
        /// Gets or sets the default tracker template.
        /// </summary>
        public ControlTemplate DefaultTrackerTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(DefaultTrackerTemplateProperty);
            }

            set
            {
                this.SetValue(DefaultTrackerTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to handle right clicks.
        /// </summary>
        public bool HandleRightClicks
        {
            get
            {
                return (bool)this.GetValue(HandleRightClicksProperty);
            }

            set
            {
                this.SetValue(HandleRightClicksProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsMouseWheelEnabled.
        /// </summary>
        public bool IsMouseWheelEnabled
        {
            get
            {
                return (bool)this.GetValue(IsMouseWheelEnabledProperty);
            }

            set
            {
                this.SetValue(IsMouseWheelEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="PlotModel" /> to show.
        /// </summary>
        /// <value>The <see cref="PlotModel" />.</value>
        public PlotModel Model
        {
            get
            {
                return (PlotModel)this.GetValue(ModelProperty);
            }

            set
            {
                this.SetValue(ModelProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the zoom rectangle template.
        /// </summary>
        /// <value>The zoom rectangle template.</value>
        public ControlTemplate ZoomRectangleTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(ZoomRectangleTemplateProperty);
            }

            set
            {
                this.SetValue(ZoomRectangleTemplateProperty, value);
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
        /// Gets the actual model in the view.
        /// </summary>
        /// <value>
        /// The actual model.
        /// </value>
        Model IView.ActualModel
        {
            get
            {
                return this.Model;
            }
        }

        /// <summary>
        /// Gets the actual model.
        /// </summary>
        /// <value>The actual model.</value>
        public PlotModel ActualModel
        {
            get
            {
                return this.currentModel;
            }
        }

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
        /// Gets the actual PlotView controller.
        /// </summary>
        /// <value>The actual PlotView controller.</value>
        public IPlotController ActualController
        {
            get
            {
                return this.Controller ?? (this.defaultController ?? (this.defaultController = new PlotController()));
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
            this.zoomRectangle.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Invalidate the PlotView (not blocking the UI thread)
        /// </summary>
        /// <param name="update">if set to <c>true</c>, the data collections will be updated.</param>
        public void InvalidatePlot(bool update = true)
        {
            this.UpdateModel(update);

            if (DesignMode.DesignModeEnabled)
            {
                this.InvalidateArrange();
                return;
            }

            if (Interlocked.CompareExchange(ref this.isPlotInvalidated, 1, 0) == 0)
            {
                // Invalidate the arrange state for the element.
                // After the invalidation, the element will have its layout updated,
                // which will occur asynchronously unless subsequently forced by UpdateLayout.
                this.BeginInvoke(this.InvalidateArrange);
            }
        }

        /// <summary>
        /// Sets the cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public void SetCursorType(CursorType cursor)
        {
            if (cursorNotImplemented)
            {
                // setting the cursor has failed in a previous attempt, see code below
                return;
            }

            var type = CoreCursorType.Arrow;
            switch (cursor)
            {
                case CursorType.Default:
                    type = CoreCursorType.Arrow;
                    break;
                case CursorType.Pan:
                    type = CoreCursorType.Hand;
                    break;
                case CursorType.ZoomHorizontal:
                    type = CoreCursorType.SizeWestEast;
                    break;
                case CursorType.ZoomVertical:
                    type = CoreCursorType.SizeNorthSouth;
                    break;
                case CursorType.ZoomRectangle:
                    type = CoreCursorType.SizeNorthwestSoutheast;
                    break;
            }

            // TODO: determine if creating a CoreCursor is possible, do not use exception
            try
            {
                var newCursor = new CoreCursor(type, 1); // this line throws an exception on Windows Phone
                Window.Current.CoreWindow.PointerCursor = newCursor;
            }
            catch (NotImplementedException)
            {
                cursorNotImplemented = true;
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

            if (tracker != this.currentTracker)
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
            this.zoomRectangle.Width = r.Width;
            this.zoomRectangle.Height = r.Height;
            Canvas.SetLeft(this.zoomRectangle, r.Left);
            Canvas.SetTop(this.zoomRectangle, r.Top);
            this.zoomRectangle.Template = this.ZoomRectangleTemplate;
            this.zoomRectangle.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Renders the PlotView to a bitmap.
        /// </summary>
        /// <returns>A bitmap.</returns>
        public WriteableBitmap ToBitmap()
        {
            throw new NotImplementedException();

            // var bmp = new RenderTargetBitmap(
            // (int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            // bmp.Render(this);
            // return bmp;
        }

        /// <summary>
        /// Stores text on the clipboard.
        /// </summary>
        /// <param name="text">The text.</param>
        void IPlotView.SetClipboardText(string text)
        {
            var pkg = new DataPackage();
            pkg.SetText(text);

            // TODO: Clipboard.SetContent(pkg);
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.grid = this.GetTemplateChild(PartGrid) as Grid;
            if (this.grid == null)
            {
                return;
            }

            this.canvas = new Canvas { IsHitTestVisible = false };
            this.grid.Children.Add(this.canvas);
            this.canvas.UpdateLayout();

            this.renderContext = new RenderContext(this.canvas);

            this.overlays = new Canvas();
            this.grid.Children.Add(this.overlays);

            this.zoomRectangle = new ContentControl();
            this.overlays.Children.Add(this.zoomRectangle);
        }

        /// <summary>
        /// Called before the KeyDown event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Control:
                    this.isControlPressed = true;
                    break;
                case VirtualKey.Shift:
                    this.isShiftPressed = true;
                    break;
                case VirtualKey.Menu:
                    this.isAltPressed = true;
                    break;
                case VirtualKey.LeftWindows:
                case VirtualKey.RightWindows:
                    this.isWindowsPressed = true;
                    break;
            }

            var modifiers = OxyModifierKeys.None;
            if (this.isControlPressed)
            {
                modifiers |= OxyModifierKeys.Control;
            }

            if (this.isAltPressed)
            {
                modifiers |= OxyModifierKeys.Control;
            }

            if (this.isShiftPressed)
            {
                modifiers |= OxyModifierKeys.Shift;
            }

            if (this.isWindowsPressed)
            {
                modifiers |= OxyModifierKeys.Windows;
            }

            if (e.Handled)
            {
                return;
            }

            var args = new OxyKeyEventArgs
            {
                Key = e.Key.Convert(),
                ModifierKeys = modifiers,
            };

            e.Handled = this.ActualController.HandleKeyDown(this, args);
        }

        /// <summary>
        /// Called before the KeyUp event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.Key)
            {
                case VirtualKey.Control:
                    this.isControlPressed = false;
                    break;
                case VirtualKey.Shift:
                    this.isShiftPressed = false;
                    break;
                case VirtualKey.Menu:
                    this.isAltPressed = false;
                    break;
                case VirtualKey.LeftWindows:
                case VirtualKey.RightWindows:
                    this.isWindowsPressed = false;
                    break;
            }
        }

        /// <summary>
        /// Called before the ManipulationStarted event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
            base.OnManipulationStarted(e);

            if (e.Handled)
            {
                return;
            }

            if (e.PointerDeviceType == PointerDeviceType.Touch)
            {
                this.Focus(FocusState.Pointer);
                e.Handled = this.ActualController.HandleTouchStarted(this, e.ToTouchEventArgs(this));
            }
        }

        /// <summary>
        /// Called before the ManipulationDelta event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            base.OnManipulationDelta(e);

            if (e.Handled)
            {
                return;
            }

            if (e.PointerDeviceType == PointerDeviceType.Touch)
            {
                e.Handled = this.ActualController.HandleTouchDelta(this, e.ToTouchEventArgs(this));
            }
        }

        /// <summary>
        /// Called before the ManipulationCompleted event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            base.OnManipulationCompleted(e);

            if (e.Handled)
            {
                return;
            }

            if (e.PointerDeviceType == PointerDeviceType.Touch)
            {
                e.Handled = this.ActualController.HandleTouchCompleted(this, e.ToTouchEventArgs(this));
            }
        }

        /// <summary>
        /// Called before the PointerPressed event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (e.Handled)
            {
                return;
            }

            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                this.Focus(FocusState.Pointer);
                this.CapturePointer(e.Pointer);

                e.Handled = this.ActualController.HandleMouseDown(this, e.ToMouseDownEventArgs(this));
            }
            else if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
            {
                this.Focus(FocusState.Pointer);

                e.Handled = this.ActualController.HandleTouchStarted(this, e.ToTouchEventArgs(this));
            }
        }

        /// <summary>
        /// Called before the PointerMoved event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            base.OnPointerMoved(e);

            if (e.Handled)
            {
                return;
            }

            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                e.Handled = this.ActualController.HandleMouseMove(this, e.ToMouseEventArgs(this));
            }

            // Note: don't handle touch here, this is also handled when moving over when a touch device
        }

        /// <summary>
        /// Called before the PointerReleased event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);

            if (e.Handled)
            {
                return;
            }

            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                this.ReleasePointerCapture(e.Pointer);
                e.Handled = this.ActualController.HandleMouseUp(this, e.ToMouseEventArgs(this));
            }
            else if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
            {
                e.Handled = this.ActualController.HandleTouchCompleted(this, e.ToTouchEventArgs(this));
            }
        }

        /// <summary>
        /// Called before the PointerWheelChanged event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
            base.OnPointerWheelChanged(e);

            if (e.Handled || !this.IsMouseWheelEnabled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseWheel(this, e.ToMouseWheelEventArgs(this));
        }

        /// <summary>
        /// Called before the PointerEntered event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseEnter(this, e.ToMouseEventArgs(this));
        }

        /// <summary>
        /// Called before the PointerExited event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseLeave(this, e.ToMouseEventArgs(this));
        }

		/// <summary>
		/// A one time condition for update visuals so it is called no matter the state of the control
		/// Currently with out this, the plotview on Xamarin Forms UWP does not render until the app's window resizes
		/// </summary>
		private bool isUpdateVisualsCalledOnce = false;

		/// <summary>
		/// Provides the behavior for the Arrange pass of layout. Classes can override this method to define their own Arrange pass behavior.
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

			//see summary for isUpdateVisualsCalledOnce
			if (!isUpdateVisualsCalledOnce)
			{
				this.UpdateVisuals();

				isUpdateVisualsCalledOnce = true;
			}

			return base.ArrangeOverride(finalSize);
		}

		/// <summary>
		/// Called when the <see cref="Model" /> property is changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
		private static void ModelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((PlotView)sender).OnModelChanged();
        }

        /// <summary>
        /// Called when the control is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Make sure InvalidateArrange is called when the PlotView is invalidated
            Interlocked.Exchange(ref this.isPlotInvalidated, 0);
            this.InvalidatePlot();
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
                    ((IPlotModel)this.currentModel).AttachPlotView(null);
                    this.currentModel = null;
                }

                if (this.Model != null)
                {
                    ((IPlotModel)this.Model).AttachPlotView(this);
                    this.currentModel = this.Model;
                }
            }

            this.InvalidatePlot();
        }

        /// <summary>
        /// Called when the size of the control is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs" /> instance containing the event data.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Updates the model.
        /// </summary>
        /// <param name="update">if set to <c>true</c>, the data collections will be updated.</param>
        private void UpdateModel(bool update)
        {
            if (this.ActualModel != null)
            {
                ((IPlotModel)this.ActualModel).Update(update);
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

            if (this.ActualModel != null && !this.ActualModel.Background.IsUndefined())
            {
                this.canvas.Background = this.ActualModel.Background.ToBrush();
            }
            else
            {
                this.canvas.Background = null;
            }

            if (this.ActualModel != null)
            {
                ((IPlotModel)this.ActualModel).Render(this.renderContext, this.canvas.ActualWidth, this.canvas.ActualHeight);
            }
        }

        /// <summary>
        /// Invokes the specified action on the UI Thread (without blocking the calling thread).
        /// </summary>
        /// <param name="action">The action.</param>
        private void BeginInvoke(Action action)
        {
            if (!this.Dispatcher.HasThreadAccess)
            {
                // TODO: Fix warning?
                // Because this call is not awaited, execution of the current method continues before the call is completed.
                // Consider applying the 'await' operator to the result of the call.
#pragma warning disable 4014
                this.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => action());
#pragma warning restore 4014
            }
            else
            {
                action();
            }
        }
    }
}