using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OxyPlot.Wpf
{
    public class Slider : Canvas
    {
        private readonly Line sliderLine1 = new Line();
        private readonly Line sliderLine2 = new Line();
        private readonly Path sliderPath = new Path();
        private readonly TextBlock sliderText = new TextBlock();
        private readonly PlotControl pc;
        public string SliderLabelFormat { get; set; }

        public Slider(PlotControl pc)
        {
            SliderLabelFormat = "{0:0.###} {1:0.###}";

            this.pc = pc;
            Children.Add(sliderLine1);
            Children.Add(sliderLine2);
            Children.Add(sliderPath);
            Children.Add(sliderText);
            sliderLine1.StrokeThickness = 1;
            sliderLine2.StrokeThickness = 1;
            sliderLine1.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(80, 0, 0, 0));
            sliderLine2.Stroke = sliderLine1.Stroke;
            sliderPath.Fill = Brushes.White;
            sliderPath.Stretch = Stretch.Fill;
            sliderPath.Stroke = Brushes.Black;
            sliderPath.SnapsToDevicePixels = true;
            sliderLine1.SnapsToDevicePixels = true;
            sliderLine2.SnapsToDevicePixels = true;
            sliderLine1.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            sliderLine2.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            sliderPath.Data = Geometry.Parse("M0,0 L3,-7 L40,-7 L40,-25 L-40,-25 L-40,-7 L-3,-7 z");
            sliderText.Foreground = Brushes.Black;
            Hide();
        }


        public void SetPosition(DataPoint dp, OxyPlot.DataSeries s)
        {
            string fmt = SliderLabelFormat;
            if (fmt == null)
                fmt = "{1:0.#####}";
            sliderText.Text = String.Format(CultureInfo.InvariantCulture, fmt, dp.X, dp.Y);

            var pt0 = pc.Transform(dp.X, dp.Y, s.XAxis, s.YAxis);
            var pt1 = pc.Transform(dp.X, s.YAxis.ActualMaximum, s.XAxis, s.YAxis);
            var pt2 = pc.Transform(dp.X, s.YAxis.ActualMinimum, s.XAxis, s.YAxis);
            var pt3 = pc.Transform(s.XAxis.ActualMinimum, dp.Y, s.XAxis, s.YAxis);
            var pt4 = pc.Transform(s.XAxis.ActualMaximum, dp.Y, s.XAxis, s.YAxis);

            SetLeft(sliderText, pt0.X - sliderPath.ActualWidth / 2);
            SetTop(sliderText, pt0.Y - sliderPath.ActualHeight + 2);
            sliderText.Height = 18;
            sliderText.Width = sliderPath.ActualWidth;
            sliderText.TextAlignment = TextAlignment.Center;

            SetLeft(sliderPath, pt0.X - 40);
            SetTop(sliderPath, pt0.Y - sliderPath.ActualHeight);

            sliderLine1.X1 = pt1.X;
            sliderLine1.Y1 = pt1.Y;
            sliderLine1.X2 = pt2.X;
            sliderLine1.Y2 = pt2.Y;

            sliderLine2.X1 = pt3.X;
            sliderLine2.Y1 = pt3.Y;
            sliderLine2.X2 = pt4.X;
            sliderLine2.Y2 = pt4.Y;

            Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            Visibility = Visibility.Hidden;
        }


    }
}