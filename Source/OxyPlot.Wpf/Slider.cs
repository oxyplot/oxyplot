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
        private Line SliderLine1 = new Line();
        private Line SliderLine2 = new Line();
        private Path SliderPath = new Path();
        private TextBlock SliderText = new TextBlock();
        private PlotControl pc;

        public Slider(PlotControl pc)
        {
            this.pc = pc;
            Children.Add(SliderLine1);
            Children.Add(SliderLine2);
            Children.Add(SliderPath);
            Children.Add(SliderText);
            SliderLine1.StrokeThickness = 1;
            SliderLine2.StrokeThickness = 1;
            SliderLine1.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(80, 0, 0, 0));
            SliderLine2.Stroke = SliderLine1.Stroke;
            SliderPath.Fill = Brushes.White;
            SliderPath.Stretch = Stretch.Fill;
            SliderPath.Stroke = Brushes.Black;
            SliderPath.SnapsToDevicePixels = true;
            SliderLine1.SnapsToDevicePixels = true;
            SliderLine2.SnapsToDevicePixels = true;
            SliderLine1.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            SliderLine2.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            SliderPath.Data = Geometry.Parse("M0,0 L3,-7 L40,-7 L40,-25 L-40,-25 L-40,-7 L-3,-7 z");
            SliderText.Foreground = Brushes.Black;
            Hide();
        }

        private string SliderLabelFormat = "{0:0.###} {1:0.###}";

        public void SetPosition(Point dp, OxyPlot.DataSeries s)
        {
            string fmt = SliderLabelFormat;
            if (fmt == null)
                fmt = "{1:0.#####}";
            SliderText.Text = String.Format(CultureInfo.InvariantCulture, fmt, dp.X, dp.Y);

            System.Windows.Point pt0 = pc.Transform(dp.X, dp.Y, s.XAxis, s.YAxis);
            System.Windows.Point pt1 = pc.Transform(dp.X, s.YAxis.ActualMaximum, s.XAxis, s.YAxis);
            System.Windows.Point pt2 = pc.Transform(dp.X, s.YAxis.ActualMinimum, s.XAxis, s.YAxis);
            System.Windows.Point pt3 = pc.Transform(s.XAxis.ActualMinimum, dp.Y, s.XAxis, s.YAxis);
            System.Windows.Point pt4 = pc.Transform(s.XAxis.ActualMaximum, dp.Y, s.XAxis, s.YAxis);

            Canvas.SetLeft(SliderText, pt0.X - SliderPath.ActualWidth / 2);
            Canvas.SetTop(SliderText, pt0.Y - SliderPath.ActualHeight + 2);
            SliderText.Height = 18;
            SliderText.Width = SliderPath.ActualWidth;
            SliderText.TextAlignment = TextAlignment.Center;

            Canvas.SetLeft(SliderPath, pt0.X - 40);
            Canvas.SetTop(SliderPath, pt0.Y - SliderPath.ActualHeight);

            SliderLine1.X1 = pt1.X;
            SliderLine1.Y1 = pt1.Y;
            SliderLine1.X2 = pt2.X;
            SliderLine1.Y2 = pt2.Y;

            SliderLine2.X1 = pt3.X;
            SliderLine2.Y1 = pt3.Y;
            SliderLine2.X2 = pt4.X;
            SliderLine2.Y2 = pt4.Y;

            Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            Visibility = Visibility.Hidden;
        }


    }
}