// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HistogramDemo
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Plots a histogram of RGB components.")]
    public partial class MainWindow
    {
        private readonly LineSeries blueLine;
        private readonly LineSeries greenLine;
        private readonly LineSeries redLine;

        private bool leftButtonDown;
        private int[] pixelData;

        public MainWindow()
        {
            this.InitializeComponent();

            this.LoadPixelData();

            var throttledEvent = new ThrottledMouseMoveEvent(image);
            throttledEvent.ThrottledMouseMove += this.ThrottledEvent_ThrottledMouseMove;
            var pm = new PlotModel { Title = "RGB histogram" };
            this.redLine = new LineSeries { Color = OxyColors.Red };
            this.greenLine = new LineSeries { Color = OxyColors.Green };
            this.blueLine = new LineSeries { Color = OxyColors.Blue };
            this.redLine.InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline;
            this.greenLine.InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline;
            this.blueLine.InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline;
            pm.Series.Add(this.redLine);
            pm.Series.Add(this.greenLine);
            pm.Series.Add(this.blueLine);
            pm.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 1, MajorStep = 0.2, MinorStep = 0.05, Title = "Frequency" });
            pm.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 100, Title = "Lightness" });
            chart.Model = pm;
        }

        /// <summary>
        /// Obtains the image data once it is loaded
        /// </summary>
        private void LoadPixelData()
        {
            var bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Examples/HistogramDemo/hare.jpg"));

            int stride = ((bitmapImage.PixelWidth * bitmapImage.Format.BitsPerPixel) + 7) / 8;
            var pixelByteArray = new byte[bitmapImage.PixelHeight * stride];
            bitmapImage.CopyPixels(pixelByteArray, stride, 0);
            this.pixelData = new int[pixelByteArray.Length / 4];
            Buffer.BlockCopy(pixelByteArray, 0, this.pixelData, 0, pixelByteArray.Length);
        }

        /// <summary>
        /// Handles mouse move to draw the line and intensity histograms
        /// </summary>
        private void ThrottledEvent_ThrottledMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.leftButtonDown)
            {
                return;
            }

            line.X2 = e.GetPosition(grid).X;
            line.Y2 = e.GetPosition(grid).Y;

            // compute distance between the points
            double distance = Math.Sqrt((line.X1 - line.X2) * (line.X1 - line.X2) + (line.Y1 - line.Y2) * (line.Y1 - line.Y2));

            this.redLine.Points.Clear();
            this.greenLine.Points.Clear();
            this.blueLine.Points.Clear();

            int b = 16;
            var histoR = new int[256 / b];
            var histoG = new int[256 / b];
            var histoB = new int[256 / b];

            // build the charts
            int n = 0;
            for (double pt = 0; pt < distance; pt++)
            {
                double xPos = line.X1 + (line.X2 - line.X1) * pt / distance;
                double yPos = line.Y1 + (line.Y2 - line.Y1) * pt / distance;

                var xIndex = (int)xPos;
                var yIndex = (int)yPos;

                int pixel = this.pixelData[xIndex + (yIndex * 300)];

                // the RGB values are 'packed' into an int, here we unpack them
                var blue = (byte)(pixel & 0xFF);
                pixel >>= 8;
                var green = (byte)(pixel & 0xFF);
                pixel >>= 8;
                var red = (byte)(pixel & 0xFF);

                histoR[red / b]++;
                histoG[green / b]++;
                histoB[blue / b]++;
                n++;
            }

            double xScale = 100.0 / histoR.Length;
            for (int i = 0; i < histoR.Length; i++)
            {
                double x = i * xScale;
                this.redLine.Points.Add(new DataPoint(x, (double)histoR[i] / n));
                this.greenLine.Points.Add(new DataPoint(x, (double)histoG[i] / n));
                this.blueLine.Points.Add(new DataPoint(x, (double)histoB[i] / n));
            }

            chart.InvalidatePlot();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            instructions.Visibility = Visibility.Collapsed;
            this.leftButtonDown = true;
            line.X1 = line.X2 = e.GetPosition(grid).X;
            line.Y1 = line.Y2 = e.GetPosition(grid).Y;
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.leftButtonDown = false;
        }
    }
}