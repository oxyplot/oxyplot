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

namespace OxyPlot.Wpf
{
    /// <summary>
    ///   Represents a control that displays a plot.
    /// </summary>
    [ContentProperty("Series")]
    [TemplatePart(Name = PART_GRID, Type = typeof(Grid))]
    public class Plot : Control, IPlotControl
    {
        private const string PART_GRID = "PART_Grid";

        public static readonly DependencyProperty PlotMarginsProperty =
            DependencyProperty.Register("PlotMargins", typeof(Thickness?), typeof(Plot),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty BoxColorProperty =
            DependencyProperty.Register("BoxColor", typeof(Color), typeof(Plot),
                                        new UIPropertyMetadata(Colors.Black));

        public static readonly DependencyProperty BoxThicknessProperty =
            DependencyProperty.Register("BoxThickness", typeof(double), typeof(Plot),
                                        new UIPropertyMetadata(1.0));

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(PlotModel), typeof(Plot),
                                        new UIPropertyMetadata(null, ModelChanged));

        public static readonly DependencyProperty SubtitleProperty =
            DependencyProperty.Register("Subtitle", typeof(string), typeof(Plot), new UIPropertyMetadata(null));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Plot),
                                        new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty TrackerTemplateProperty =
            DependencyProperty.Register("TrackerTemplate", typeof(DataTemplate), typeof(Plot),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty ZoomRectangleTemplateProperty =
            DependencyProperty.Register("ZoomRectangleTemplate", typeof(ControlTemplate), typeof(Plot),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty LegendPositionProperty =
            DependencyProperty.Register("LegendPosition", typeof(LegendPosition), typeof(Plot),
                                        new UIPropertyMetadata(LegendPosition.RightTop));

        private readonly ObservableCollection<Annotation> annotations;
        private readonly ObservableCollection<Axis> axes;
        private readonly PanAction panAction;
        private readonly ObservableCollection<DataSeries> series;
        private readonly TrackerAction trackerAction;
        private readonly ZoomAction zoomAction;

        private Canvas canvas;
        private Grid grid;

        private PlotModel internalModel;
        private bool isPlotInvalidated;
        private ScreenPoint mouseDownPoint;
        private Canvas overlays;

        // private PlotFrame plotAliasedFrame;
        // private PlotFrame plotFrame;
        private Tracker tracker;

        private ContentControl zoomControl;

        static Plot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Plot),
                                                     new FrameworkPropertyMetadata(typeof(Plot)));
        }

        public Plot()
        {
            Background = Brushes.White;

            panAction = new PanAction(this);
            zoomAction = new ZoomAction(this);
            trackerAction = new TrackerAction(this);

            MouseActions = new List<MouseAction> { panAction, zoomAction, trackerAction };

            series = new ObservableCollection<DataSeries>();
            axes = new ObservableCollection<Axis>();
            annotations = new ObservableCollection<Annotation>();

            series.CollectionChanged += OnSeriesChanged;
            axes.CollectionChanged += OnAxesChanged;
            annotations.CollectionChanged += OnAnnotationsChanged;

            Loaded += OnLoaded;
            DataContextChanged += OnDataContextChanged;
            SizeChanged += OnSizeChanged;

            CompositionTarget.Rendering += CompositionTargetRendering;

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, DoCopy));
        }

        private void DoCopy(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetImage(ToBitmap());
        }

        public LegendPosition LegendPosition
        {
            get { return (LegendPosition)GetValue(LegendPositionProperty); }
            set { SetValue(LegendPositionProperty, value); }
        }

        public List<MouseAction> MouseActions { get; private set; }

        public ControlTemplate ZoomRectangleTemplate
        {
            get { return (ControlTemplate)GetValue(ZoomRectangleTemplateProperty); }
            set { SetValue(ZoomRectangleTemplateProperty, value); }
        }

        public Color BoxColor
        {
            get { return (Color)GetValue(BoxColorProperty); }
            set { SetValue(BoxColorProperty, value); }
        }

        public DataTemplate TrackerTemplate
        {
            get { return (DataTemplate)GetValue(TrackerTemplateProperty); }
            set { SetValue(TrackerTemplateProperty, value); }
        }

        public Thickness? PlotMargins
        {
            get { return (Thickness?)GetValue(PlotMarginsProperty); }
            set { SetValue(PlotMarginsProperty, value); }
        }

        public double BoxThickness
        {
            get { return (double)GetValue(BoxThicknessProperty); }
            set { SetValue(BoxThicknessProperty, value); }
        }

        public ObservableCollection<DataSeries> Series
        {
            get { return series; }
        }

        public ObservableCollection<Axis> Axes
        {
            get { return axes; }
        }

        public ObservableCollection<Annotation> Annotations
        {
            get { return annotations; }
        }

        public PlotModel Model
        {
            get { return (PlotModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public string Subtitle
        {
            get { return (string)GetValue(SubtitleProperty); }
            set { SetValue(SubtitleProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public Tracker Tracker
        {
            get { return tracker; }
        }

        #region IPlotControl Members

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
            zoomControl.Visibility = Visibility.Hidden;
        }

        /// <summary>
        ///   Gets the series that is nearest the specified point (in screen coordinates).
        /// </summary>
        /// <param name = "pt">The point.</param>
        /// <param name = "limit">The maximum distance, if this is exceeded the method will return null.</param>
        /// <returns>The closest DataSeries</returns>
        public ISeries GetSeriesFromPoint(ScreenPoint pt, double limit = 100)
        {
            return internalModel.GetSeriesFromPoint(pt, limit);
        }

        public void ShowTracker(ISeries s, DataPoint dp)
        {
            var ds = s as OxyPlot.DataSeries;
            if (ds != null)
            {
                tracker.ContentTemplate = TrackerTemplate;
                tracker.SetPosition(dp, ds);
            }
            else
            {
                HideTracker();
            }
        }

        public void HideTracker()
        {
            tracker.Hide();
        }

        public void Pan(IAxis axis, double x0, double x1)
        {
            if (Model == null)
            {
                var a = FindModelAxis(axis);
                if (a != null)
                {
                    a.Pan(x0, x1);
                }
            }

            // Modify min/max of the PlotModel's axis
            axis.Pan(x0, x1);
        }
       
        public void Reset(IAxis axis)
        {
            if (Model == null)
            {
                var a = FindModelAxis(axis);
                if (a != null)
                {
                    a.Reset();
                }
            }
            else
            {
                axis.Reset();
                Model.UpdateAxisTransforms();
            }
        }

        public void Zoom(IAxis axis, double p1, double p2)
        {
            var a = FindModelAxis(axis);
            if (a != null)
            {
                a.Zoom(p1, p2);
            }
            else
            {
                axis.Zoom(p1, p2);
            }
            if (Model!=null)
                Model.UpdateAxisTransforms();
        }

        public void ZoomAt(IAxis axis, double factor, double x)
        {
            var a = FindModelAxis(axis);
            if (a != null)
            {
                a.ZoomAt(factor, x);
            }
            else
            {
                axis.ZoomAt(factor, x);
            }
            if (Model!=null)
                Model.UpdateAxisTransforms();
        }

        #endregion

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
            UpdateModel();
            UpdateVisuals();
        }

        public void InvalidatePlot()
        {
            lock (this)
            {
                isPlotInvalidated = true;
            }
        }

        private void OnSeriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SyncLogicalTree(e);
        }

        private void OnAxesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SyncLogicalTree(e);
        }

        private void OnAnnotationsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SyncLogicalTree(e);
        }

        private void SyncLogicalTree(NotifyCollectionChangedEventArgs e)
        {
            // In order to get DataContext and binding to work with the series, axes and annotations
            // we add the items to the logical tree
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    AddLogicalChild(item);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    RemoveLogicalChild(item);
                }
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            bool control = Keyboard.IsKeyDown(Key.LeftCtrl);
            bool shift = Keyboard.IsKeyDown(Key.LeftShift);
            bool alt = (Keyboard.IsKeyDown(Key.LeftAlt));
            var p = e.GetPosition(this).ToScreenPoint();
            foreach (var a in MouseActions)
            {
                a.OnMouseWheel(p, e.Delta, control, shift, alt);
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            Focus();
            CaptureMouse();

            bool control = Keyboard.IsKeyDown(Key.LeftCtrl);
            bool shift = Keyboard.IsKeyDown(Key.LeftShift);
            bool alt = (Keyboard.IsKeyDown(Key.LeftAlt));

            var button = OxyMouseButton.Left;
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

            var p = e.GetPosition(this).ToScreenPoint();
            foreach (var a in MouseActions)
            {
                a.OnMouseDown(p, button, e.ClickCount, control, shift, alt);
            }

            mouseDownPoint = p;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            bool control = Keyboard.IsKeyDown(Key.LeftCtrl);
            bool shift = Keyboard.IsKeyDown(Key.LeftShift);
            bool alt = (Keyboard.IsKeyDown(Key.LeftAlt));
            var p = e.GetPosition(this).ToScreenPoint();
            foreach (var a in MouseActions)
            {
                a.OnMouseMove(p, control, shift, alt);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            foreach (var a in MouseActions)
            {
                a.OnMouseUp();
            }

            ReleaseMouseCapture();
            var p = e.GetPosition(this).ToScreenPoint();

            double d = p.DistanceTo(mouseDownPoint);
            if (ContextMenu != null)
            {
                if (d == 0 && e.ChangedButton == MouseButton.Right)
                {
                    ContextMenu.Visibility = Visibility.Visible;

                    // todo: The contextmenu has the wrong placement after panning
                    // ContextMenu.Placement = PlacementMode.Relative;
                    // ContextMenu.PlacementTarget = this;
                    // ContextMenu.PlacementRectangle=new Rect(e.GetPosition(this),new Size(0,0));
                }
                else
                {
                    ContextMenu.Visibility = Visibility.Collapsed;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            bool control = Keyboard.IsKeyDown(Key.LeftCtrl);

            if (e.Key == Key.A)
            {
                e.Handled = true;
                ZoomAll();
            }
            if (e.Key == Key.R && control)
            {
                if (internalModel != null)
                    Clipboard.SetText(internalModel.CreateTextReport());
            }

            base.OnKeyDown(e);
        }

        public void ZoomAll()
        {
            foreach (var a in internalModel.Axes)
            {
                a.Reset();
            }

            InvalidatePlot();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            grid = Template.FindName(PART_GRID, this) as Grid;

            if (grid == null)
            {
                return;
            }

            canvas = new Canvas();
            grid.Children.Add(canvas);
            canvas.UpdateLayout();

            // Overlays
            overlays = new Canvas();
            grid.Children.Add(overlays);

            // Tracker
            tracker = new Tracker();
            overlays.Children.Add(tracker);

            zoomControl = new ContentControl();
            overlays.Children.Add(zoomControl);
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
            InvalidatePlot();
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
            // If no model is set, create an internal model and copy the 
            // axes/series/annotations and properties from the WPF objects to the internal model
            if (Model == null)
            {
                // Create an internal model
                if (internalModel == null)
                {
                    internalModel = new PlotModel();
                }

                // Transfer axes, series and properties from 
                // the WPF dependency objects to the internal model
                if (Series != null)
                {
                    internalModel.Series.Clear();
                    foreach (var s in Series)
                    {
                        internalModel.Series.Add(s.CreateModel());
                    }
                }

                if (Axes != null && Axes.Count > 0)
                {
                    internalModel.Axes.Clear();

                    foreach (var a in Axes)
                    {
                        a.UpdateModelProperties();
                        internalModel.Axes.Add(a.ModelAxis);
                    }
                }

                if (Annotations != null)
                {
                    internalModel.Annotations.Clear();

                    foreach (var a in Annotations)
                    {
                        a.UpdateModelProperties();
                        internalModel.Annotations.Add(a.ModelAnnotation);
                    }
                }

                if (PlotMargins.HasValue)
                {
                    internalModel.PlotMargins = new OxyThickness(
                        PlotMargins.Value.Left, PlotMargins.Value.Top,
                        PlotMargins.Value.Right, PlotMargins.Value.Bottom);
                }

                // Box around the plot area
                internalModel.BoxColor = BoxColor.ToOxyColor();
                internalModel.BoxThickness = BoxThickness;

                internalModel.LegendPosition = LegendPosition;
            }
            else
            {
                internalModel = Model;
            }

            internalModel.UpdateData();
        }

        private void UpdateVisuals()
        {
            // Clear the canvas
            if (canvas != null)
            {
                canvas.Children.Clear();
            }

            if (internalModel == null)
            {
                return;
            }

            TransferProperties();

            if (canvas != null)
            {
                // disconnecting the canvas whil updating
#if !NO_DISCONNECT
                int idx = grid.Children.IndexOf(canvas);
                grid.Children.RemoveAt(idx);
#endif
                var wrc = new ShapesRenderContext(canvas);
                internalModel.Render(wrc);

#if !NO_DISCONNECT
                // reinsert the canvas again
                grid.Children.Insert(idx, canvas);
#endif
            }
        }

        /// <summary>
        /// Transfers properties from the WPF control to the PlotModel.
        /// </summary>
        private void TransferProperties()
        {
            if (Title != null)
            {
                internalModel.Title = Title;
            }

            if (Subtitle != null)
            {
                internalModel.Subtitle = Subtitle;
            }
        }

        public void SaveBitmap(string fileName)
        {
            var bmp = ToBitmap();

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using (var s = File.Create(fileName))
            {
                encoder.Save(s);
            }
        }

        public void SaveXaml(string fileName)
        {
            using (var w = new StreamWriter(fileName))
            {
                w.Write(ToXaml());
            }
        }

        public RenderTargetBitmap ToBitmap()
        {
            var bmp = new RenderTargetBitmap((int)ActualWidth, (int)ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(this);
            return bmp;
        }

        public string ToXaml()
        {
            var sb = new StringBuilder();
            var tw = new StringWriter(sb);
            var xw = XmlWriter.Create(tw, new XmlWriterSettings { Indent = true });
            if (canvas != null)
            {
                XamlWriter.Save(canvas, xw);
            }

            xw.Close();
            return sb.ToString();
        }

        private Axis FindModelAxis(IAxis a)
        {
            return Axes.FirstOrDefault(axis => axis.ModelAxis == a);
        }
    }
}