using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
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
    [ContentProperty("Series")]
    public class PlotControl : Control
    {
        public static readonly DependencyProperty AxisMarginsProperty =
            DependencyProperty.Register("AxisMargins", typeof(Thickness?), typeof(PlotControl),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty BoxColorProperty =
            DependencyProperty.Register("BoxColor", typeof(Color), typeof(PlotControl),
                                        new UIPropertyMetadata(Colors.Black));

        public static readonly DependencyProperty BoxThicknessProperty =
            DependencyProperty.Register("BoxThickness", typeof(double), typeof(PlotControl),
                                        new UIPropertyMetadata(1.0));

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(PlotModel), typeof(PlotControl),
                                        new UIPropertyMetadata(null, ModelChanged));

        public static readonly DependencyProperty SubtitleProperty =
            DependencyProperty.Register("Subtitle", typeof(string), typeof(PlotControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PlotControl),
                                        new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty RenderToCanvasProperty =
            DependencyProperty.Register("RenderToCanvas", typeof(bool), typeof(PlotControl),
                                        new UIPropertyMetadata(true, RenderToCanvasChanged));

        public static readonly DependencyProperty SliderTemplateProperty =
            DependencyProperty.Register("SliderTemplate", typeof(DataTemplate), typeof(PlotControl),
                                        new UIPropertyMetadata(null));

        public static readonly DependencyProperty ZoomRectangleTemplateProperty =
            DependencyProperty.Register("ZoomRectangleTemplate", typeof(ControlTemplate), typeof(PlotControl),
                                        new UIPropertyMetadata(null));


        private string PART_GRID = "PART_Grid";

        private Canvas canvas;
        private Grid grid;

        private PlotModel internalModel;
        private Canvas overlays;
        private PanAction panAction;

        private PlotFrame plotAliasedFrame;
        private PlotFrame plotFrame;
        private Slider slider;

        private SliderAction sliderAction;
        private ZoomAction zoomAction;
        private ContentControl zoomControl;

        static PlotControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlotControl),
                                                     new FrameworkPropertyMetadata(typeof(PlotControl)));
        }

        public PlotControl()
        {
            Background = Brushes.Transparent;

            panAction = new PanAction(this);
            zoomAction = new ZoomAction(this);
            sliderAction = new SliderAction(this);

            series = new ObservableCollection<DataSeries>();
            axes = new ObservableCollection<Axis>();
            series.CollectionChanged += OnSeriesChanged;
            axes.CollectionChanged += OnAxesChanged;
            
            Loaded += PlotControl_Loaded;
            DataContextChanged += PlotControl_DataContextChanged;
            SizeChanged += PlotControl_SizeChanged;
            KeyDown += PlotControl_KeyDown;
            MouseDown += PlotControl_MouseDown;

            // CommandBindings.Add(new KeyBinding())
        }

        private void OnSeriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SyncLogicalTree(e);
        }

        private void OnAxesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SyncLogicalTree(e);
        }

        private void SyncLogicalTree(NotifyCollectionChangedEventArgs e)
        {
            // In order to get DataContext and binding to work with the series and axes
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

        public DataTemplate SliderTemplate
        {
            get { return (DataTemplate)GetValue(SliderTemplateProperty); }
            set { SetValue(SliderTemplateProperty, value); }
        }

        public bool RenderToCanvas
        {
            get { return (bool)GetValue(RenderToCanvasProperty); }
            set { SetValue(RenderToCanvasProperty, value); }
        }

        public Thickness? AxisMargins
        {
            get { return (Thickness?)GetValue(AxisMarginsProperty); }
            set { SetValue(AxisMarginsProperty, value); }
        }

        public double BoxThickness
        {
            get { return (double)GetValue(BoxThicknessProperty); }
            set { SetValue(BoxThicknessProperty, value); }
        }

        private ObservableCollection<DataSeries> series;
        public ObservableCollection<DataSeries> Series
        {
            get { return series; }
        }

        private ObservableCollection<Axis> axes;
        public ObservableCollection<Axis> Axes
        {
            get { return axes; }
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

        private void PlotControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Focus();
        }

        private void PlotControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                e.Handled = true;
                ZoomAll();
            }
        }

        public void ZoomAll()
        {
            foreach (OxyPlot.Axis a in internalModel.Axes)
                a.Reset();
            Refresh();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            grid = Template.FindName(PART_GRID, this) as Grid;

            if (grid == null)
                return;

            OnRenderToCanvasChanged();

            // Overlays
            overlays = new Canvas();
            grid.Children.Add(overlays);

            // Slider
            slider = new Slider(this);
            overlays.Children.Add(slider);

            zoomControl = new ContentControl();
            overlays.Children.Add(zoomControl);
        }

        private static void RenderToCanvasChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotControl)d).OnRenderToCanvasChanged();
        }

        private void OnRenderToCanvasChanged()
        {
            if (grid == null)
                return;
            if (RenderToCanvas)
            {
                if (canvas == null)
                {
                    canvas = new Canvas();
                    grid.Children.Insert(0, canvas);
                    canvas.UpdateLayout();
                }
                if (plotFrame != null)
                {
                    grid.Children.Remove(plotFrame);
                    grid.Children.Remove(plotAliasedFrame);
                    plotFrame = null;
                    plotAliasedFrame = null;
                }
            }
            else
            {
                if (plotFrame == null)
                {
                    plotAliasedFrame = new PlotFrame(true);
                    plotFrame = new PlotFrame(false);
                    grid.Children.Insert(0, plotAliasedFrame);
                    grid.Children.Insert(1, plotFrame);
                    plotFrame.UpdateLayout();
                    plotAliasedFrame.UpdateLayout();
                }
                if (canvas != null)
                {
                    grid.Children.Remove(canvas);
                    canvas = null;
                }
            }
            UpdateVisuals();
        }

        private void PlotControl_Loaded(object sender, RoutedEventArgs e)
        {
            OnModelChanged();
        }

        private void PlotControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnModelChanged();
        }

        private void PlotControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateVisuals();
        }

        private static void VisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotControl)d).UpdateVisuals();
        }

        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotControl)d).OnModelChanged();
        }

        private void OnModelChanged()
        {
            if (Model == null)
            {
                // Create an internal model
                if (internalModel == null)
                    internalModel = new PlotModel();

                // Transfer axes, series and properties from 
                // the WPF dependency objects to the internal model
                UpdateModel(internalModel);

                if (AxisMargins.HasValue)
                {
                    internalModel.AxisMargins = new OxyThickness(
                        AxisMargins.Value.Left, AxisMargins.Value.Top,
                        AxisMargins.Value.Right, AxisMargins.Value.Bottom);
                }
                // Box around the plot area
                internalModel.BoxColor = BoxColor.ToOxyColor();
                internalModel.BoxThickness = BoxThickness;
                
                // internalModel.LegendPosition = LegendPosition;
            }
            else
                internalModel = Model;

            internalModel.UpdateData();
            UpdateVisuals();
        }

        private void UpdateModel(PlotModel p)
        {
            if (Series != null)
            {
                p.Series.Clear();
                foreach (DataSeries s in Series)
                {
                    p.Series.Add(s.CreateModel());
                }
            }
            if (Axes != null)
            {
                p.Axes.Clear();
                foreach (Axis a in Axes)
                {
                    a.UpdateModelProperties();
                    p.Axes.Add(a.ModelAxis);
                }
            }
        }

        public void Refresh()
        {
            OnModelChanged();
        }

        private void UpdateVisuals()
        {
            if (canvas != null)
                canvas.Children.Clear();

            if (internalModel == null)
                return;

            if (Title != null)
                internalModel.Title = Title;

            if (Subtitle != null)
                internalModel.Subtitle = Subtitle;

            if (canvas != null)
            {
                var wrc = new ShapesRenderContext(canvas);
                internalModel.Render(wrc);
            }

            if (plotFrame != null)
            {
                plotFrame.Model = internalModel;
                plotAliasedFrame.Model = internalModel;
                plotFrame.InvalidateVisual();
                plotAliasedFrame.InvalidateVisual();
            }
        }

        public void SaveBitmap(string fileName)
        {
            RenderTargetBitmap bmp = ToBitmap();

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using (FileStream s = File.Create(fileName))
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
            var height = (int)ActualHeight;
            var width = (int)ActualWidth;

            var bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(this);
            return bmp;
        }

        public string ToXaml()
        {
            var sb = new StringBuilder();
            var tw = new StringWriter(sb);
            var xw = new XmlTextWriter(tw) { Formatting = Formatting.Indented };
            if (canvas != null)
                XamlWriter.Save(canvas, xw);
            xw.Close();
            string xaml = sb.ToString();
            /*
                        xaml = xaml.Replace(string.Format(
                            "<PlotControl Height=\"{0}\" Width=\"{1}\" ",
                            this.ActualHeight, this.ActualWidth),
                                            "<Grid ");
                        */
            return xaml;
        }

        public Point Transform(DataPoint pt, OxyPlot.Axis xaxis, OxyPlot.Axis yaxis)
        {
            return Transform(pt.X, pt.Y, xaxis, yaxis);
        }

        public Point Transform(double x, double y, OxyPlot.Axis xaxis, OxyPlot.Axis yaxis)
        {
            return new Point(xaxis.TransformX(x), yaxis.TransformX(y));
        }

        public DataPoint InverseTransform(Point pt, OxyPlot.Axis xaxis, OxyPlot.Axis yaxis)
        {
            double x = 0;
            if (xaxis != null)
                x = xaxis.InverseTransformX(pt.X);
            double y = 0;
            if (yaxis != null)
                y = yaxis.InverseTransformX(pt.Y);
            return new DataPoint(x, y);
        }

        public void GetAxesFromPoint(Point pt, out OxyPlot.Axis xaxis, out OxyPlot.Axis yaxis)
        {
            xaxis = yaxis = null;
            foreach (OxyPlot.Axis axis in internalModel.Axes)
            {
                double x = axis.InverseTransformX(axis.IsHorizontal() ? pt.X : pt.Y);
                if (x >= axis.ActualMinimum && x <= axis.ActualMaximum)
                {
                    if (axis.IsHorizontal())
                    {
                        // todo: only accept axis if it is within plot area 
                        // or the axis area

                        xaxis = axis;
                    }
                    else
                        yaxis = axis;
                }
            }
        }

        public void ShowZoomRectangle(Rect r)
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

        public OxyPlot.DataSeries GetSeriesFromPoint(Point pt, double limit = 100)
        {
            double mindist = double.MaxValue;
            OxyPlot.DataSeries closest = null;
            foreach (OxyPlot.DataSeries s in internalModel.Series)
            {
                DataPoint dp = InverseTransform(pt, s.XAxis, s.YAxis);
                DataPoint? np = s.GetNearestPointOnLine(dp);
                if (np == null)
                    continue;

                // find distance to this point on the screen
                Point sp = Transform(np.Value, s.XAxis, s.YAxis);
                double dist = pt.DistanceTo(sp);
                if (dist < mindist)
                {
                    closest = s;
                    mindist = dist;
                }
            }
            if (mindist < limit)
                return closest;
            return null;
        }

        public void ShowSlider(OxyPlot.DataSeries s, DataPoint dp)
        {
            slider.ContentTemplate = SliderTemplate;
            slider.SetPosition(dp, s);
        }

        public void HideSlider()
        {
            slider.Hide();
        }
    }
}