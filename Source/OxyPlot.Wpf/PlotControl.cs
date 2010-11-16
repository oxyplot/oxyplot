using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace OxyPlot.Wpf
{
    [ContentProperty("Series")]
    public class PlotControl : Grid
    {
        public static readonly DependencyProperty AxisMarginProperty =
            DependencyProperty.Register("AxisMargin", typeof(Thickness?), typeof(PlotControl),
                                        new UIPropertyMetadata(new Thickness(40)));


        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(double), typeof(PlotControl),
                                        new UIPropertyMetadata(2.0));

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

        private readonly Canvas overlays;
        private readonly Slider slider;

        private readonly System.Windows.Shapes.Rectangle zoomRectangle;
        private Canvas canvas;

        internal PlotModel internalModel;

        private PlotFrame plotAliasedFrame;
        private PlotFrame plotFrame;

        private PanAction panAction;
        private SliderAction sliderAction;
        private ZoomAction zoomAction;

        public PlotControl()
        {
            Background = Brushes.Transparent;

            panAction = new PanAction(this);
            zoomAction = new ZoomAction(this);
            sliderAction = new SliderAction(this);

            // create canvas or plotFrame
            OnRenderToCanvasChanged();


            // Overlays
            overlays = new Canvas();
            Children.Add(overlays);

            // Slider
            slider = new Slider(this);
            overlays.Children.Add(slider);

            // Zoom rectangle (must belong to a Canvas)
            zoomRectangle = new System.Windows.Shapes.Rectangle
                                {
                                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(20, 255, 255, 0)),
                                    Stroke = new SolidColorBrush(System.Windows.Media.Colors.Yellow),
                                    StrokeThickness = 1,
                                    Visibility = Visibility.Hidden
                                };
            overlays.Children.Add(zoomRectangle);

            Series = new ObservableCollection<DataSeries>();
            Axes = new ObservableCollection<Axis>();

            // todo: subscribe to changes in Series and Axes collections
            // todo: make datacontext propagate into series and axes

            Loaded += PlotControl_Loaded;
            DataContextChanged += PlotControl_DataContextChanged;
            SizeChanged += PlotControl_SizeChanged;
        }

        public bool RenderToCanvas
        {
            get { return (bool)GetValue(RenderToCanvasProperty); }
            set { SetValue(RenderToCanvasProperty, value); }
        }

        public Thickness? AxisMargin
        {
            get { return (Thickness?)GetValue(AxisMarginProperty); }
            set { SetValue(AxisMarginProperty, value); }
        }

        public double BorderThickness
        {
            get { return (double)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public ObservableCollection<DataSeries> Series { get; set; }

        public ObservableCollection<Axis> Axes { get; set; }

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

        private static void RenderToCanvasChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotControl)d).OnRenderToCanvasChanged();
        }

        private void OnRenderToCanvasChanged()
        {
            if (RenderToCanvas)
            {
                if (canvas == null)
                {
                    canvas = new Canvas();
                    Children.Insert(0, canvas);
                    canvas.UpdateLayout();
                }
                if (plotFrame != null)
                {
                    Children.Remove(plotFrame);
                    Children.Remove(plotAliasedFrame);
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
                    Children.Insert(0, plotAliasedFrame);
                    Children.Insert(1, plotFrame);
                    plotFrame.UpdateLayout();
                    plotAliasedFrame.UpdateLayout();
                }
                if (canvas != null)
                {
                    Children.Remove(canvas);
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
                if (internalModel == null)
                    internalModel = new PlotModel();
                UpdateModel(internalModel);

                if (AxisMargin.HasValue)
                {
                    internalModel.MarginLeft = AxisMargin.Value.Left;
                    internalModel.MarginRight = AxisMargin.Value.Right;
                    internalModel.MarginTop = AxisMargin.Value.Top;
                    internalModel.MarginBottom = AxisMargin.Value.Bottom;
                }
                internalModel.BorderThickness = BorderThickness;
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
                foreach (var s in Series)
                {
                    p.Series.Add(s.CreateModel());
                }
            }
            if (Axes != null)
            {
                p.Axes.Clear();
                foreach (var a in Axes)
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
                double x = axis.InverseTransformX(axis.IsHorizontal ? pt.X : pt.Y);
                if (x >= axis.ActualMinimum && x <= axis.ActualMaximum)
                {
                    if (axis.IsHorizontal)
                        xaxis = axis;
                    else
                        yaxis = axis;
                }
            }
        }

        public void ShowZoomRectangle(Rect r)
        {
            zoomRectangle.Width = r.Width;
            zoomRectangle.Height = r.Height;
            Canvas.SetLeft(zoomRectangle, r.Left);
            Canvas.SetTop(zoomRectangle, r.Top);
            zoomRectangle.Visibility = Visibility.Visible;
        }

        public void HideZoomRectangle()
        {
            zoomRectangle.Visibility = Visibility.Hidden;
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
            slider.SetPosition(dp, s);
        }

        public void HideSlider()
        {
            slider.Hide();
        }
    }
}