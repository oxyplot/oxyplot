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
//   The plot control for Windows Store apps.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Metro
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Windows.Devices.Input;

    using OxyPlot.Series;

    using Windows.ApplicationModel.DataTransfer;
    using Windows.System;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;

    /// <summary>
    /// The plot control for Windows Store apps.
    /// </summary>
    [TemplatePart(Name = PartGrid, Type = typeof(Grid))]
    public partial class Plot : Control, IPlotControl
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
        /// The is plot invalidated.
        /// </summary>
        private bool isPlotInvalidated;

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
        private MetroRenderContext renderContext;

        /// <summary>
        /// Data has been updated.
        /// </summary>
        private bool updateData;

        /// <summary>
        /// The zoom control.
        /// </summary>
        private ContentControl zoomRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref = "Plot" /> class.
        /// </summary>
        public Plot()
        {
            this.DefaultStyleKey = typeof(Plot);

            this.trackerDefinitions = new ObservableCollection<TrackerDefinition>();
            this.Loaded += this.OnLoaded;
            this.SizeChanged += this.OnSizeChanged;
            this.ManipulationMode = ManipulationModes.Scale | ManipulationModes.TranslateX
                                    | ManipulationModes.TranslateY;
            CompositionTarget.Rendering += this.CompositionTargetRendering;
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
        /// Gets the actual model.
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
        /// Gets the actual plot controller.
        /// </summary>
        /// <value>
        /// The actual plot controller.
        /// </value>
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
        /// Invalidate the plot (not blocking the UI thread)
        /// </summary>
        /// <param name="update">
        /// if set to <c>true</c>, the data collections will be updated.
        /// </param>
        public void InvalidatePlot(bool update = true)
        {
            lock (this)
            {
                this.isPlotInvalidated = true;
                this.updateData = this.updateData || update;
            }
        }

        /// <summary>
        /// Refreshes the plot immediately (not blocking the UI thread).
        /// </summary>
        /// <param name="update">
        /// if set to <c>true</c>, the data collections will be updated.
        /// </param>
        public void RefreshPlot(bool update)
        {
            // don't block ui thread
            this.InvalidatePlot(update);
        }

        /// <summary>
        /// Sets the cursor.
        /// </summary>
        /// <param name="cursor">
        /// The cursor.
        /// </param>
        public void SetCursorType(CursorType cursor)
        {
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

            Window.Current.CoreWindow.PointerCursor = new CoreCursor(type, 1);
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
            var trackerTemplate = this.DefaultTrackerTemplate;
            if (ts != null && !string.IsNullOrEmpty(ts.TrackerKey))
            {
                var match = this.TrackerDefinitions.FirstOrDefault(t => t.TrackerKey == ts.TrackerKey);
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
        /// <param name="r">
        /// The rectangle.
        /// </param>
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
        /// Renders the plot to a bitmap.
        /// </summary>
        /// <returns>
        /// A bitmap.
        /// </returns>
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
        void IPlotControl.SetClipboardText(string text)
        {
            var pkg = new DataPackage();
            pkg.SetText(text);
            Clipboard.SetContent(pkg);
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

            this.canvas = new Canvas();
            this.canvas.IsHitTestVisible = false;
            this.grid.Children.Add(this.canvas);
            this.canvas.UpdateLayout();

            this.renderContext = new MetroRenderContext(this.canvas);

            this.overlays = new Canvas();
            this.grid.Children.Add(this.overlays);

            this.zoomRectangle = new ContentControl();
            this.overlays.Children.Add(this.zoomRectangle);
        }

        /// <summary>
        /// Called before the KeyDown event occurs.
        /// </summary>
        /// <param name="e">
        /// The data for the event.
        /// </param>
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
        /// <param name="e">
        /// The data for the event.
        /// </param>
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
        /// <param name="e">
        /// Event data for the event.
        /// </param>
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
        /// <param name="e">
        /// Event data for the event.
        /// </param>
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
        /// <param name="e">
        /// Event data for the event.
        /// </param>
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

        protected override void OnHolding(HoldingRoutedEventArgs e)
        {
            base.OnHolding(e);
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
        }

        protected override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            base.OnDoubleTapped(e);
        }

        /// <summary>
        /// Called before the PointerPressed event occurs.
        /// </summary>
        /// <param name="e">
        /// Event data for the event.
        /// </param>
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
        }

        /// <summary>
        /// Called before the PointerMoved event occurs.
        /// </summary>
        /// <param name="e">
        /// Event data for the event.
        /// </param>
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
        }

        /// <summary>
        /// Called before the PointerReleased event occurs.
        /// </summary>
        /// <param name="e">
        /// Event data for the event.
        /// </param>
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
        }

        /// <summary>
        /// Called before the PointerWheelChanged event occurs.
        /// </summary>
        /// <param name="e">
        /// Event data for the event.
        /// </param>
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
        /// Called when the Model property is changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private static void ModelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((Plot)sender).OnModelChanged();
        }

        /// <summary>
        /// Called when the composition target rendering event occurs.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        private void CompositionTargetRendering(object sender, object e)
        {
            lock (this)
            {
                if (this.isPlotInvalidated)
                {
                    this.isPlotInvalidated = false;
                    if (this.ActualWidth > 0 && this.ActualHeight > 0)
                    {
                        this.UpdateModel(this.updateData);
                        this.updateData = false;
                    }

                    this.UpdateVisuals();
                }
            }
        }

        /// <summary>
        /// Called when the control is loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing the event data.
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

                    this.Model.AttachPlotControl(this);
                    this.currentModel = this.Model;
                }
            }

            this.InvalidatePlot();
        }

        /// <summary>
        /// Called when the size of the control is changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="Windows.UI.Xaml.SizeChangedEventArgs"/> instance containing the event data.
        /// </param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidatePlot();
        }

        /// <summary>
        /// Updates the model.
        /// </summary>
        /// <param name="update">
        /// if set to <c>true</c>, the data collections will be updated.
        /// </param>
        private void UpdateModel(bool update)
        {
            this.internalModel = this.Model;

            if (this.ActualModel != null)
            {
                this.ActualModel.Update(update);
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
                this.ActualModel.Render(this.renderContext, this.canvas.ActualWidth, this.canvas.ActualHeight);
            }
        }
    }
}