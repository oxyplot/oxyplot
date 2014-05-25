// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
//   Represents a control that displays a PlotModel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WP8
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media.Imaging;

    using OxyPlot.Series;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    [ContentProperty("Series")]
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
        /// Identifies the <see cref="PanCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PanCursorProperty = DependencyProperty.Register(
            "PanCursor", typeof(Cursor), typeof(PlotView), new PropertyMetadata(Cursors.Hand));

        /// <summary>
        /// Identifies the <see cref="ZoomHorizontalCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomHorizontalCursorProperty =
            DependencyProperty.Register(
                "ZoomHorizontalCursor", typeof(Cursor), typeof(PlotView), new PropertyMetadata(Cursors.SizeWE));

        /// <summary>
        /// Identifies the <see cref="ZoomRectangleCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomRectangleCursorProperty =
            DependencyProperty.Register(
                "ZoomRectangleCursor", typeof(Cursor), typeof(PlotView), new PropertyMetadata(Cursors.SizeNWSE));

        /// <summary>
        /// Identifies the <see cref="ZoomRectangleTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomRectangleTemplateProperty =
            DependencyProperty.Register(
                "ZoomRectangleTemplate", typeof(ControlTemplate), typeof(PlotView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ZoomVerticalCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomVerticalCursorProperty =
            DependencyProperty.Register(
                "ZoomVerticalCursor", typeof(Cursor), typeof(PlotView), new PropertyMetadata(Cursors.SizeNS));

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
        /// The render context
        /// </summary>
        private PhoneRenderContext renderContext;

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
        /// Invalidation flag (0: no update, 1: update visual elements only, 2:update data).
        /// </summary>
        private int isPlotInvalidated;

        /// <summary>
        /// The overlays.
        /// </summary>
        private Canvas overlays;

        /// <summary>
        /// The zoom control.
        /// </summary>
        private ContentControl zoomControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotView" /> class.
        /// </summary>
        public PlotView()
        {
            this.DefaultStyleKey = typeof(PlotView);

            this.trackerDefinitions = new ObservableCollection<TrackerDefinition>();
            this.Loaded += this.OnLoaded;
            this.SizeChanged += this.OnSizeChanged;
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
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
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
        /// Gets or sets the pan cursor.
        /// </summary>
        /// <value>The pan cursor.</value>
        public Cursor PanCursor
        {
            get
            {
                return (Cursor)this.GetValue(PanCursorProperty);
            }

            set
            {
                this.SetValue(PanCursorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the horizontal zoom cursor.
        /// </summary>
        /// <value>The zoom horizontal cursor.</value>
        public Cursor ZoomHorizontalCursor
        {
            get
            {
                return (Cursor)this.GetValue(ZoomHorizontalCursorProperty);
            }

            set
            {
                this.SetValue(ZoomHorizontalCursorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the rectangle zoom cursor.
        /// </summary>
        /// <value>The zoom rectangle cursor.</value>
        public Cursor ZoomRectangleCursor
        {
            get
            {
                return (Cursor)this.GetValue(ZoomRectangleCursorProperty);
            }

            set
            {
                this.SetValue(ZoomRectangleCursorProperty, value);
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
        /// Gets or sets the vertical zoom cursor.
        /// </summary>
        /// <value>The zoom vertical cursor.</value>
        public Cursor ZoomVerticalCursor
        {
            get
            {
                return (Cursor)this.GetValue(ZoomVerticalCursorProperty);
            }

            set
            {
                this.SetValue(ZoomVerticalCursorProperty, value);
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
        /// Invalidate the PlotView (not blocking the UI thread)
        /// </summary>
        /// <param name="updateData">The update Data.</param>
        public void InvalidatePlot(bool updateData = true)
        {
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
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate" /> . In simplest terms, this means the method is called just before a UI element displays in an application. For more information, see Remarks.
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
            this.renderContext = new PhoneRenderContext(this.canvas);

            this.overlays = new Canvas();
            this.grid.Children.Add(this.overlays);

            this.zoomControl = new ContentControl();
            this.overlays.Children.Add(this.zoomControl);
        }

        /// <summary>
        /// Saves the PlotView as a bitmap.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void SaveBitmap(string fileName)
        {
            throw new NotImplementedException();

            // todo: Use imagetools.codeplex.com
            // or http://windowspresentationfoundation.com/Blend4Book/SaveTheImage.txt

            // var bmp = this.ToBitmap();

            // var encoder = new PngBitmapEncoder();
            // encoder.Frames.Add(BitmapFrame.Create(bmp));

            // using (FileStream s = File.Create(fileName))
            // {
            // encoder.Save(s);
            // }
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
        public void SetClipboardText(string text)
        {
            TrySetClipboardText(text);
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.KeyDown" /> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Handled)
            {
                return;
            }

            var args = new OxyKeyEventArgs { ModifierKeys = Keyboard.GetModifierKeys(), Key = e.Key.Convert() };
            e.Handled = this.ActualController.HandleKeyDown(this, args);
        }

        /// <summary>
        /// Called when the <see cref="E:System.Windows.UIElement.ManipulationStarted" /> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
        {
            base.OnManipulationStarted(e);
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleTouchStarted(this, e.ToTouchEventArgs(this));
        }

        /// <summary>
        /// Called when the <see cref="E:System.Windows.UIElement.ManipulationDelta" /> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            base.OnManipulationDelta(e);
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleTouchDelta(this, e.ToTouchEventArgs(this));
        }

        /// <summary>
        /// Called when the <see cref="E:System.Windows.UIElement.ManipulationCompleted" /> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            base.OnManipulationCompleted(e);
            if (e.Handled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleTouchCompleted(this, e.ToTouchEventArgs(this));
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.OnMouseButtonDown(OxyMouseButton.Left, e);
            e.Handled = true;
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp" /> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            this.OnMouseButtonUp(OxyMouseButton.Left, e);
            e.Handled = true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.UIElement.MouseRightButtonDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data.</param>
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
            this.OnMouseButtonDown(OxyMouseButton.Right, e);
            if (this.HandleRightClicks)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.UIElement.MouseRightButtonUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data.</param>
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);
            this.OnMouseButtonUp(OxyMouseButton.Right, e);
            if (this.HandleRightClicks)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Called when a mouse button is pressed down.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs" /> instance containing the event data.</param>
        protected void OnMouseButtonDown(OxyMouseButton button, MouseButtonEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }

            this.Focus();
            this.CaptureMouse();

            e.Handled = this.ActualController.HandleMouseDown(this, e.ToMouseDownEventArgs(button, this));
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseMove" /> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            this.ActualController.HandleMouseMove(this, e.ToMouseEventArgs(this));
        }

        /// <summary>
        /// Raises the MouseButtonUp event.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs" /> instance containing the event data.</param>
        protected void OnMouseButtonUp(OxyMouseButton button, MouseButtonEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }

            this.ReleaseMouseCapture();

            e.Handled = this.ActualController.HandleMouseUp(this, e.ToMouseUpEventArgs(button, this));
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseWheel" /> event occurs to provide handling for the event in a derived class without attaching a delegate.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Input.MouseWheelEventArgs" /> that contains the event data.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Handled || !this.IsMouseWheelEnabled)
            {
                return;
            }

            e.Handled = this.ActualController.HandleMouseWheel(this, e.ToMouseWheelEventArgs(this));
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseEnter" /> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.ActualController.HandleMouseEnter(this, e.ToMouseEventArgs(this));
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseLeave" /> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.ActualController.HandleMouseLeave(this, e.ToMouseEventArgs(this));
        }

        /// <summary>
        /// Provides the behavior for the Arrange pass of Silverlight layout. Classes can override this method to define their own Arrange pass behavior.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this object should use to arrange itself and its children.</param>
        /// <returns>The actual size that is used after the element is arranged in layout.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Interlocked.CompareExchange(ref this.isPlotInvalidated, 0, 1) == 1)
            {
                if (this.ActualWidth > 0 && this.ActualHeight > 0)
                {
                    this.UpdateVisuals();
                }
            }

            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// Called when the Model is changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotView)d).OnModelChanged();
        }

        /// <summary>
        /// Sets the clipboard text.
        /// </summary>
        /// <param name="text">The text.</param>
        private static void TrySetClipboardText(string text)
        {
            try
            {
                Clipboard.SetText(text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
        /// Called when the model is changed.
        /// </summary>
        private void OnModelChanged()
        {
            lock (this.modelLock)
            {
                if (this.currentModel != null)
                {
                    ((IPlotModel)this.currentModel).AttachPlotControl(null);
                    this.currentModel = null;
                }

                this.currentModel = this.Model;

                if (this.currentModel != null)
                {
                    if (this.currentModel.PlotControl != null)
                    {
                        throw new InvalidOperationException(
                            "This PlotModel is already in use by some other PlotView control.");
                    }

                    ((IPlotModel)this.currentModel).AttachPlotControl(this);
                }
            }

            this.InvalidatePlot();
        }

        /// <summary>
        /// Called when the size of the control is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs" /> instance containing the event data.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Updates the model.
        /// </summary>
        /// <param name="updateData">The update Data.</param>
        private void UpdateModel(bool updateData = true)
        {
            if (this.ActualModel != null)
            {
                ((IPlotModel)this.ActualModel).Update(updateData);
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
                ((IPlotModel)this.ActualModel).Render(this.renderContext, this.canvas.ActualWidth, this.canvas.ActualHeight);
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
                this.Dispatcher.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }
    }
}