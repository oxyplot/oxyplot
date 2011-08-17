// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Plot.cs">
//   
// </copyright>
// <summary>
//   The Silverlight Plot control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Silverlight
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
    /// The Silverlight Plot control.
    /// </summary>
    [ContentProperty("Series")]
    [TemplatePart(Name = PART_GRID, Type = typeof(Grid))]
    public class Plot : Control, IPlotControl
    {
        #region Constants and Fields

        /// <summary>
        ///   The data context watcher property.
        /// </summary>
        public static readonly DependencyProperty DataContextWatcherProperty =
            DependencyProperty.Register(
                "DataContextWatcher", typeof(Object), typeof(Plot), new PropertyMetadata(DataContextChanged));

        /// <summary>
        ///   The default tracker property.
        /// </summary>
        public static readonly DependencyProperty DefaultTrackerTemplateProperty =
            DependencyProperty.Register(
                "DefaultTrackerTemplate", typeof(ControlTemplate), typeof(Plot), new PropertyMetadata(null));

        /// <summary>
        ///   The handle right clicks property.
        /// </summary>
        public static readonly DependencyProperty HandleRightClicksProperty =
            DependencyProperty.Register("HandleRightClicks", typeof(bool), typeof(Plot), new PropertyMetadata(true));

        /// <summary>
        ///   The model property.
        /// </summary>
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
            "Model", typeof(PlotModel), typeof(Plot), new PropertyMetadata(null, ModelChanged));

        /// <summary>
        ///   The zoom rectangle template property.
        /// </summary>
        public static readonly DependencyProperty ZoomRectangleTemplateProperty =
            DependencyProperty.Register(
                "ZoomRectangleTemplate", typeof(ControlTemplate), typeof(Plot), new PropertyMetadata(null));

        /// <summary>
        ///   The Grid PART constant.
        /// </summary>
        private const string PART_GRID = "PART_Grid";

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
        ///   The overlays.
        /// </summary>
        private Canvas overlays;

        /// <summary>
        ///   The zoom control.
        /// </summary>
        private ContentControl zoomControl;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Plot" /> class.
        /// </summary>
        public Plot()
        {
#if SILVERLIGHT
            this.DefaultStyleKey = typeof(Plot);
#endif

            this.panAction = new PanAction(this);
            this.zoomAction = new ZoomAction(this);
            this.trackerAction = new TrackerAction(this);

            this.MouseActions = new List<OxyMouseAction> { this.panAction, this.zoomAction, this.trackerAction };

            this.trackerDefinitions = new ObservableCollection<TrackerDefinition>();
            this.Loaded += this.OnLoaded;
            this.SizeChanged += this.OnSizeChanged;
            CompositionTarget.Rendering += this.CompositionTargetRendering;

#if SILVERLIGHT

            // http://nuggets.hammond-turner.org.uk/2009/01/quickie-simulating-datacontextchanged.html
            // TODO: doesn't work?
            this.SetBinding(DataContextWatcherProperty, new Binding());
#endif

#if WPF
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, DoCopy));
#endif
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
        ///   Gets or sets the default tracker template.
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
        ///   Gets or sets a value indicating whether to handle right clicks.
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
        ///   Gets or sets the model.
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
        /// Gets or sets the mouse actions.
        /// </summary>
        /// <value>The mouse actions.</value>
        public List<OxyMouseAction> MouseActions { get; private set; }

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
        public void GetAxesFromPoint(ScreenPoint pt, out IAxis xaxis, out IAxis yaxis)
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
        public ISeries GetSeriesFromPoint(ScreenPoint pt, double limit)
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
        public void InvalidatePlot()
        {
            lock (this)
            {
                this.isPlotInvalidated = true;
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. In simplest terms, this means the method is called just before a UI element displays in an application. For more information, see Remarks.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.grid = this.GetTemplateChild(PART_GRID) as Grid;
            if (this.grid != null)
            {
                this.canvas = new Canvas();
                this.grid.Children.Add(this.canvas);
                this.canvas.UpdateLayout();

                this.overlays = new Canvas();
                this.grid.Children.Add(this.overlays);

                this.zoomControl = new ContentControl();
                this.overlays.Children.Add(this.zoomControl);
            }
        }

        /// <summary>
        /// Pans the specified axis.
        /// </summary>
        /// <param name="axis">
        /// The axis.
        /// </param>
        /// <param name="x1">
        /// The x1.
        /// </param>
        /// <param name="x2">
        /// The x2.
        /// </param>
        public void Pan(IAxis axis, double x1, double x2)
        {
            axis.Pan(x1, x2);
        }

        /// <summary>
        /// Refresh the plot immediately (blocking UI thread)
        /// </summary>
        public void RefreshPlot()
        {
            // don't block ui thread on silverlight
            this.InvalidatePlot();
        }

        /// <summary>
        /// Resets the specified axis.
        /// </summary>
        /// <param name="axis">
        /// The axis.
        /// </param>
        public void Reset(IAxis axis)
        {
            axis.Reset();
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
        public void Zoom(IAxis axis, double p1, double p2)
        {
            axis.Zoom(p1, p2);
        }

        /// <summary>
        /// Zooms to fit all content of the plot.
        /// </summary>
        public void ZoomAll()
        {
            foreach (IAxis a in this.ActualModel.Axes)
            {
                a.Reset();
            }

            this.InvalidatePlot();
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
        public void ZoomAt(IAxis axis, double factor, double x)
        {
            axis.ZoomAt(factor, x);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.KeyDown"/> event occurs.
        /// </summary>
        /// <param name="e">
        /// The data for the event.
        /// </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            bool control = IsControlDown();
            bool alt = IsAltDown();

            if (e.Key == Key.A)
            {
                e.Handled = true;
                this.ZoomAll();
            }

            if (e.Key == Key.C && control)
            {
                e.Handled = true;
                // todo: Clipboard does not currently support copying image data
            }

            if (control && alt && ActualModel != null)
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
        /// Renders the plot to a bitmap.
        /// </summary>
        /// <returns>
        /// </returns>
        public WriteableBitmap ToBitmap()
        {
            throw new NotImplementedException();
            //var bmp = new RenderTargetBitmap(
            //    (int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            //bmp.Render(this);
            //return bmp;
        }

        /// <summary>
        /// Saves the plot as a bitmap.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        public void SaveBitmap(string fileName)
        {
            throw new NotImplementedException();
            
            // todo: Use imagetools.codeplex.com
            
            //var bmp = this.ToBitmap();

            //var encoder = new PngBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(bmp));

            //using (FileStream s = File.Create(fileName))
            //{
            //    encoder.Save(s);
            //}
        }


        /// <summary>
        /// Renders the plot to xaml.
        /// </summary>
        /// <returns>
        /// The to xaml.
        /// </returns>
        public string ToXaml()
        {
            throw new NotImplementedException();
            //var sb = new StringBuilder();
            //var tw = new StringWriter(sb);
            //XmlWriter xw = XmlWriter.Create(tw, new XmlWriterSettings { Indent = true });
            //if (this.canvas != null)
            //{
            //    XamlWriter.Save(this.canvas, xw);
            //}

            //xw.Close();
            //return sb.ToString();
        }

        /// <summary>
        /// Raises the <see cref="E:MouseButtonUp"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.
        /// </param>
        protected void OnMouseButtonUp(MouseButtonEventArgs e)
        {
            foreach (OxyMouseAction a in this.MouseActions)
            {
                a.OnMouseUp();
            }

            this.ReleaseMouseCapture();
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown"/> event occurs.
        /// </summary>
        /// <param name="e">
        /// The data for the event.
        /// </param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.OnMouseButtonDown(OxyMouseButton.Left, e);
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp"/> event occurs.
        /// </summary>
        /// <param name="e">
        /// The data for the event.
        /// </param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            this.OnMouseButtonUp(e);
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseMove"/> event occurs.
        /// </summary>
        /// <param name="e">
        /// The data for the event.
        /// </param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            bool isControlDown = IsControlDown();
            bool isShiftDown = IsShiftDown();
            bool isAltDown = IsAltDown();

            ScreenPoint p = e.GetPosition(this).ToScreenPoint();

            foreach (OxyMouseAction a in this.MouseActions)
            {
                a.OnMouseMove(p, isControlDown, isShiftDown, isAltDown);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.UIElement.MouseRightButtonDown"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data.
        /// </param>
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
        /// Raises the <see cref="E:System.Windows.UIElement.MouseRightButtonUp"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);
            this.OnMouseButtonUp(e);
            if (this.HandleRightClicks)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseWheel"/> event occurs to provide handling for the event in a derived class without attaching a delegate.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.Input.MouseWheelEventArgs"/> that contains the event data.
        /// </param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            bool isControlDown = IsControlDown();
            bool isShiftDown = IsShiftDown();
            bool isAltDown = IsAltDown();

            ScreenPoint p = e.GetPosition(this).ToScreenPoint();

            foreach (OxyMouseAction a in this.MouseActions)
            {
                a.OnMouseWheel(p, e.Delta, isControlDown, isShiftDown, isAltDown);
            }
        }

        /// <summary>
        /// Called when the DataContext is changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private static void DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((Plot)sender).OnDataContextChanged(sender, e);
        }

        /// <summary>
        /// Determines whether the alt key is down.
        /// </summary>
        /// <returns>
        /// <c>true</c> if alt is down; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsAltDown()
        {
            ModifierKeys keys = Keyboard.Modifiers;
            return (keys & ModifierKeys.Alt) != 0;
        }

        /// <summary>
        /// Determines whether the control key is down.
        /// </summary>
        /// <returns>
        /// <c>true</c> if control is down; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsControlDown()
        {
            ModifierKeys keys = Keyboard.Modifiers;
            return (keys & ModifierKeys.Control) != 0;
        }

        /// <summary>
        /// Determines whether the shift key is down.
        /// </summary>
        /// <returns>
        /// <c>true</c> if shift is down; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsShiftDown()
        {
            ModifierKeys keys = Keyboard.Modifiers;
            return (keys & ModifierKeys.Shift) != 0;
        }

        /// <summary>
        /// Called when the Model is changed.
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
        /// Called when the visual appearance changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void VisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Plot)d).UpdateVisuals();
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
        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            lock (this)
            {
                if (this.isPlotInvalidated)
                {
                    this.isPlotInvalidated = false;
                    this.UpdateModel();
                    this.UpdateVisuals();
                }
            }
        }

        /// <summary>
        /// Called when data context is changed.
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
        /// Called when a mouse button is pressed down.
        /// </summary>
        /// <param name="button">
        /// The button.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.
        /// </param>
        private void OnMouseButtonDown(OxyMouseButton button, MouseButtonEventArgs e)
        {
            this.Focus();
            this.CaptureMouse();
            ScreenPoint p = e.GetPosition(this).ToScreenPoint();
            bool isControlDown = IsControlDown();
            bool isShiftDown = IsShiftDown();
            bool isAltDown = IsAltDown();

            int clickCount = 1;
            if (MouseButtonHelper.IsDoubleClick(this, e))
            {
                clickCount = 2;
            }

            foreach (OxyMouseAction a in this.MouseActions)
            {
                a.OnMouseDown(p, button, clickCount, isControlDown, isShiftDown, isAltDown);
            }
        }

        /// <summary>
        /// Called when the size of the control is changed.
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
        /// Synchronize properties between the Silverlight control and the internal PlotModel (only if Model is undefined).
        /// </summary>
        private void SynchronizeProperties()
        {
        }

        /// <summary>
        /// Updates the model.
        /// </summary>
        private void UpdateModel()
        {
            this.internalModel = this.Model;

            if (this.ActualModel != null)
            {
                this.ActualModel.UpdateData();
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

                var wrc = new SilverlightRenderContext(this.canvas);
                this.ActualModel.Render(wrc);
            }
        }

        #endregion
    }
}