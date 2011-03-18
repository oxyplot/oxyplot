using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace OxyPlot.Silverlight
{
    [TemplatePart(Name = PART_GRID, Type = typeof(Grid))]
    public class Plot : Control, IPlot
    {
        public override void OnApplyTemplate()
        {
            grid = GetTemplateChild(PART_GRID) as Grid;
            if (grid != null)
            {
                canvas = new Canvas();
                grid.Children.Insert(0, canvas);
                canvas.UpdateLayout();

                overlays = new Canvas();
                grid.Children.Add(overlays);

                tracker = new Tracker();
                overlays.Children.Add(tracker);

                zoomControl = new ContentControl();
                overlays.Children.Add(zoomControl);
            }

            base.OnApplyTemplate();
        }

        public static readonly DependencyProperty TrackerTemplateProperty =
            DependencyProperty.Register("TrackerTemplate", typeof(DataTemplate), typeof(Plot),
                                        new PropertyMetadata(null));

        public static readonly DependencyProperty ZoomRectangleTemplateProperty =
            DependencyProperty.Register("ZoomRectangleTemplate", typeof(ControlTemplate), typeof(Plot),
                                        new PropertyMetadata(null));

        public ControlTemplate ZoomRectangleTemplate
        {
            get { return (ControlTemplate)GetValue(ZoomRectangleTemplateProperty); }
            set { SetValue(ZoomRectangleTemplateProperty, value); }
        }

        public DataTemplate TrackerTemplate
        {
            get { return (DataTemplate)GetValue(TrackerTemplateProperty); }
            set { SetValue(TrackerTemplateProperty, value); }
        }

        public Plot()
        {
            DefaultStyleKey = typeof(Plot);

            Background = new SolidColorBrush(Colors.Transparent);

            panAction = new PanAction(this);
            zoomAction = new ZoomAction(this);
            trackerAction = new TrackerAction(this);

            MouseActions = new List<MouseAction> { panAction, zoomAction, trackerAction };

            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;

            // http://nuggets.hammond-turner.org.uk/2009/01/quickie-simulating-datacontextchanged.html
            // TODO: doesn't work?
            SetBinding(DataContextWatcherProperty, new Binding());

            CompositionTarget.Rendering += CompositionTargetRendering;
        }

        public static readonly DependencyProperty DataContextWatcherProperty =
            DependencyProperty.Register("DataContextWatcher",
                                        typeof(Object), typeof(Plot),
                                        new PropertyMetadata(DataContextChanged));

        private static void DataContextChanged(object sender,
                                               DependencyPropertyChangedEventArgs e)
        {
            ((Plot)sender).OnDataContextChanged(sender, e);
        }

        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            lock (this)
            {                    
                if (isPlotInvalidated)
                {
                    isPlotInvalidated = false;
                    UpdateModel();
                    UpdateVisuals();
                }
            }
        }

        public void RefreshPlot()
        {
            // don't block ui thread on silverlight
            InvalidatePlot();
        }

        public void InvalidatePlot()
        {
            lock (this)
            {
                isPlotInvalidated = true;
            }
        }

        public void UpdateAxisTransforms()
        {
            internalModel.UpdateAxisTransforms();
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(PlotModel), typeof(Plot),
                                        new PropertyMetadata(null, ModelChanged));

        public PlotModel Model
        {
            get { return (PlotModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        private const string PART_GRID = "PART_Grid";

        private Canvas canvas;
        private Grid grid;
        private Canvas overlays;
        private Tracker tracker;

        private PlotModel internalModel;

        public List<MouseAction> MouseActions { get; private set; }

        private readonly PanAction panAction;
        private readonly TrackerAction trackerAction;
        private readonly ZoomAction zoomAction;

        private ContentControl zoomControl;

        private bool isPlotInvalidated;

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            var p = e.GetPosition(this).ToScreenPoint();
            bool isControlDown = IsControlDown();
            bool isShiftDown = IsShiftDown();
            bool isAltDown = IsAltDown();
            foreach (var a in MouseActions)
                a.OnMouseWheel(p, e.Delta, isControlDown, isShiftDown, isAltDown);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            OnMouseButtonDown(OxyMouseButton.Left, e);
        }

        private void OnMouseButtonDown(OxyMouseButton button, MouseButtonEventArgs e)
        {
            Focus();
            CaptureMouse();
            var p = e.GetPosition(this).ToScreenPoint();
            bool isControlDown = IsControlDown();
            bool isShiftDown = IsShiftDown();
            bool isAltDown = IsAltDown();
            foreach (var a in MouseActions)
                a.OnMouseDown(p, button, 1, isControlDown, isShiftDown, isAltDown);
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            // TODO: Right click is showing silverlight context menu - find other solution??

            base.OnMouseRightButtonDown(e);
            OnMouseButtonDown(OxyMouseButton.Right, e);
            e.Handled = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var p = e.GetPosition(this).ToScreenPoint();
            bool isControlDown = IsControlDown();
            bool isShiftDown = IsShiftDown();
            bool isAltDown = IsAltDown();
            foreach (var a in MouseActions)
                a.OnMouseMove(p, isControlDown, isShiftDown, isAltDown);
        }

        private bool IsControlDown()
        {
            var keys = Keyboard.Modifiers;
            return (keys & ModifierKeys.Control) != 0;
        }

        private bool IsShiftDown()
        {
            var keys = Keyboard.Modifiers;
            return (keys & ModifierKeys.Shift) != 0;
        }

        private bool IsAltDown()
        {
            var keys = Keyboard.Modifiers;
            return (keys & ModifierKeys.Alt) != 0;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            OnMouseButtonUp(e);
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);
            OnMouseButtonUp(e);
            e.Handled = true;
        }

        protected void OnMouseButtonUp(MouseButtonEventArgs e)
        {
            foreach (var a in MouseActions)
                a.OnMouseUp();
            ReleaseMouseCapture();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                e.Handled = true;
                ZoomAll();
            }
            base.OnKeyDown(e);
        }

        public void ZoomAll()
        {
            foreach (var a in internalModel.Axes)
                a.Reset();
            InvalidatePlot();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnModelChanged();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnModelChanged();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateVisuals();
        }

        private static void VisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Plot)d).UpdateVisuals();
        }

        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Plot)d).OnModelChanged();
        }

        private void OnModelChanged()
        {
            InvalidatePlot();
        }

        private void UpdateModel()
        {
            internalModel = Model;
            if (Model != null)
                internalModel.UpdateData();
        }

        private void UpdateVisuals()
        {
            if (canvas == null)
                return;

            canvas.Children.Clear();

            if (internalModel == null)
                return;


            var wrc = new SilverlightRenderContext(canvas);
            internalModel.Render(wrc);
        }

        public void GetAxesFromPoint(ScreenPoint pt, out IAxis xaxis, out IAxis yaxis)
        {
            internalModel.GetAxesFromPoint(pt, out xaxis, out yaxis);
        }

        public void ShowZoomRectangle(OxyRect r)
        {
            zoomControl.Width = r.Width;
            zoomControl.Height = r.Height;
            Canvas.SetLeft(zoomControl, r.Left);
            Canvas.SetTop(zoomControl, r.Top);
            zoomControl.Template = ZoomRectangleTemplate;
            zoomControl.Visibility = Visibility.Visible;
        }

        public void HideZoomRectangle()
        {
            zoomControl.Visibility = Visibility.Collapsed;
        }

        public OxyRect GetPlotArea()
        {
            if (internalModel != null)
                return internalModel.PlotArea;
            return new OxyRect(0, 0, ActualWidth, ActualHeight);
        }

        /// <summary>
        /// Gets the series that is nearest the specified point (in screen coordinates).
        /// </summary>
        /// <param name="pt">The point.</param>
        /// <param name="limit">The maximum distance, if this is exceeded the method will return null.</param>
        /// <returns>The closest DataSeries</returns>
        public ISeries GetSeriesFromPoint(ScreenPoint pt, double limit = 100)
        {
            return internalModel.GetSeriesFromPoint(pt, limit);
        }

        public void ShowTracker(ISeries s, DataPoint dp)
        {
            var ds = s as DataSeries;
            if (ds != null)
            {
                tracker.ContentTemplate = TrackerTemplate;
                tracker.SetPosition(dp, ds);
            }
            else
                HideTracker();
        }

        public void HideTracker()
        {
            tracker.Hide();
        }

        public void Pan(IAxis axis, double x1, double x2)
        {
            axis.Pan(x1, x2);
        }

        public void Reset(IAxis axis)
        {
            axis.Reset();
        }

        public void Zoom(IAxis axis, double p1, double p2)
        {
            axis.Zoom(p1, p2);
        }

        public void ZoomAt(IAxis axis, double factor, double x)
        {
            axis.ZoomAt(factor, x);
        }
    }
}