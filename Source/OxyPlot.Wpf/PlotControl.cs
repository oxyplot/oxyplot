using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OxyPlot.Wpf
{
    [ContentProperty("Series")]
    public class PlotControl : Grid
    {
        public Thickness AxisMargin
        {
            get { return (Thickness)GetValue(AxisMarginProperty); }
            set { SetValue(AxisMarginProperty, value); }
        }

        public static readonly DependencyProperty AxisMarginProperty =
            DependencyProperty.Register("AxisMargin", typeof(Thickness), typeof(PlotControl), new UIPropertyMetadata(new Thickness(40)));


        public double BorderThickness
        {
            get { return (double)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(double), typeof(PlotControl), new UIPropertyMetadata(2.0));



        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(PlotModel), typeof(PlotControl),
                                        new UIPropertyMetadata(null, ModelChanged));

        public static readonly DependencyProperty SubtitleProperty =
            DependencyProperty.Register("Subtitle", typeof(string), typeof(PlotControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PlotControl),
                                        new UIPropertyMetadata(null, VisualChanged));

        private readonly Canvas canvas;
        private readonly Canvas overlays;
        private readonly Slider slider;

        private readonly System.Windows.Shapes.Rectangle zoomRectangle;

        private PlotModel internalModel;

        private PanAction panAction;
        private ZoomAction zoomAction;
        private SliderAction sliderAction;

        public PlotControl()
        {
            Background = Brushes.Transparent;

            panAction = new PanAction(this);
            zoomAction = new ZoomAction(this);
            sliderAction = new SliderAction(this);

            canvas = new Canvas();
            Children.Add(canvas);

            // Slider
            slider = new Slider(this);
            Children.Add(slider);

            // Overlays
            overlays = new Canvas();
            Children.Add(overlays);

            // Zoom rectangle
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

            // todo: subscribe to changes in Series and Axes collections?

            Loaded += PlotControl_Loaded;
            DataContextChanged += PlotControl_DataContextChanged;
            SizeChanged += PlotControl_SizeChanged;
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
                if (AxisMargin != null)
                {
                    internalModel.MarginLeft = AxisMargin.Left;
                    internalModel.MarginRight = AxisMargin.Right;
                    internalModel.MarginTop = AxisMargin.Top;
                    internalModel.MarginBottom = AxisMargin.Bottom;
                }
                internalModel.BorderThickness = BorderThickness;
            }
            else
                internalModel = Model;

            if (internalModel != null)
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
            canvas.Children.Clear();
            if (internalModel == null)
                return;

            if (Title != null)
                internalModel.Title = Title;

            if (Subtitle != null)
                internalModel.Subtitle = Subtitle;

            var wrc = new WpfRenderContext(canvas);
            internalModel.Render(wrc);
        }

        public void SaveBitmap(string fileName)
        {
            var height = (int)ActualHeight;
            var width = (int)ActualWidth;

            var bmp = new RenderTargetBitmap(width, height, 96, 96,
                                             PixelFormats.Pbgra32);
            bmp.Render(this);

            var encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using (FileStream s = File.Create(fileName))
            {
                encoder.Save(s);
            }
        }

        public System.Windows.Point Transform(Point pt, IAxis xaxis, IAxis yaxis)
        {
            return Transform(pt.X, pt.Y, xaxis, yaxis);
        }

        public System.Windows.Point Transform(double x, double y, IAxis xaxis, IAxis yaxis)
        {
            return new System.Windows.Point(xaxis.Transform(x), yaxis.Transform(y));
        }

        public Point InverseTransform(System.Windows.Point pt, IAxis xaxis, IAxis yaxis)
        {
            double x = 0;
            if (xaxis != null)
                x = xaxis.InverseTransform(pt.X);
            double y = 0;
            if (yaxis != null)
                y = yaxis.InverseTransform(pt.Y);
            return new Point(x, y);
        }

        public void GetAxesFromPoint(System.Windows.Point pt, out IAxis xaxis, out IAxis yaxis)
        {
            xaxis = yaxis = null;
            foreach (IAxis axis in internalModel.Axes)
            {
                double x = axis.InverseTransform(axis.IsHorizontal ? pt.X : pt.Y);
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

        public OxyPlot.DataSeries GetSeriesFromPoint(System.Windows.Point pt, double limit = 100)
        {
            double mindist = double.MaxValue;
            OxyPlot.DataSeries closest = null;
            foreach (OxyPlot.DataSeries s in internalModel.Series)
            {
                var dp = InverseTransform(pt, s.XAxis, s.YAxis);
                var np = s.GetNearestPointOnLine(dp);
                if (np == null)
                    continue;

                // find distance to this point on the screen
                var sp = Transform(np.Value, s.XAxis, s.YAxis);
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

        public void ShowSlider(OxyPlot.DataSeries s, Point dp)
        {
            slider.SetPosition(dp, s);
        }

        public void HideSlider()
        {
            slider.Hide();
        }
    }
}