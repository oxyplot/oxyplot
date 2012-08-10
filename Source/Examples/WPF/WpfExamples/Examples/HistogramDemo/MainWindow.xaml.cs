// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using OxyPlot;

// Original code
// http://www.scottlogic.co.uk/blog/colin/2010/11/visiblox-charts-vs-silverlight-toolkit-charts-a-test-of-performance-2/

namespace HistogramDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LineSeries blueLine;
        private readonly LineSeries greenLine;
        private readonly LineSeries redLine;

        private bool lButtonDown;
        private int[] pixelData;

        public MainWindow()
        {
            InitializeComponent();

            LoadPixelData();

            var throttledEvent = new ThrottledMouseMoveEvent(image);
            throttledEvent.ThrottledMouseMove += ThrottledEvent_ThrottledMouseMove;
            var pm = new PlotModel("RGB histogram");
            redLine = new LineSeries(OxyColors.Red);
            greenLine = new LineSeries(OxyColors.Green);
            blueLine = new LineSeries(OxyColors.Blue);
            redLine.Smooth = true;
            greenLine.Smooth = true;
            blueLine.Smooth = true;
            pm.Series.Add(redLine);
            pm.Series.Add(greenLine);
            pm.Series.Add(blueLine);
            pm.PlotMargins = new OxyThickness(60,0,0,30);
            pm.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 1, 0.2, 0.05, "Frequency"));
            pm.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0, 100, "Lightness"));
            chart.Model = pm;
        }

        /// <summary>
        /// Obtains the image data once it is loaded
        /// </summary>
        private void LoadPixelData()
        {
            var bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Examples/HistogramDemo/hare.jpg"));

            int nStride = (bitmapImage.PixelWidth * bitmapImage.Format.BitsPerPixel + 7) / 8;
            var pixelByteArray = new byte[bitmapImage.PixelHeight * nStride];
            bitmapImage.CopyPixels(pixelByteArray, nStride, 0);
            pixelData = new int[pixelByteArray.Length / 4];
            Buffer.BlockCopy(pixelByteArray, 0, pixelData, 0, pixelByteArray.Length);
        }

        /// <summary>
        /// Handles mouse move to draw the line and intensity histograms
        /// </summary>
        private void ThrottledEvent_ThrottledMouseMove(object sender, MouseEventArgs e)
        {
            if (!lButtonDown)
                return;

            line.X2 = e.GetPosition(grid).X;
            line.Y2 = e.GetPosition(grid).Y;

            // compute distance between the points
            double distance = Math.Sqrt((line.X1 - line.X2) * (line.X1 - line.X2) +
                                        (line.Y1 - line.Y2) * (line.Y1 - line.Y2));


            redLine.Points.Clear();
            greenLine.Points.Clear();
            blueLine.Points.Clear();

            int b = 16;
            int[] histoR = new int[256 / b];
            int[] histoG = new int[256 / b];
            int[] histoB = new int[256 / b];

            // build the charts
            int n = 0;
            for (double pt = 0; pt < distance; pt++)
            {
                double xPos = line.X1 + (line.X2 - line.X1) * pt / distance;
                double yPos = line.Y1 + (line.Y2 - line.Y1) * pt / distance;

                var xIndex = (int)xPos;
                var yIndex = (int)yPos;

                int pixel = pixelData[xIndex + yIndex * 300];

                // the RGB values are 'packed' into an int, here we unpack them
                var B = (byte)(pixel & 0xFF);
                pixel >>= 8;
                var G = (byte)(pixel & 0xFF);
                pixel >>= 8;
                var R = (byte)(pixel & 0xFF);
                //pixel >>= 8;
                //var A = (byte)(pixel & 0xFF);

                //redLine.Points.Add(new OxyPlot.Point(pt, R));
                //greenLine.Points.Add(new OxyPlot.Point(pt, G));
                //blueLine.Points.Add(new OxyPlot.Point(pt, B));

                histoR[R / b]++;
                histoG[G / b]++;
                histoB[B / b]++;
                n++;
            }
            double xScale = 100.0 / histoR.Length;
            for (int i = 0; i < histoR.Length; i++)
            {
                double x = i * xScale;
                redLine.Points.Add(new DataPoint(x,(double)histoR[i] / n));
                greenLine.Points.Add(new DataPoint(x, (double)histoG[i] / n));
                blueLine.Points.Add(new DataPoint(x, (double)histoB[i] / n));
            }

            chart.InvalidatePlot();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            instructions.Visibility = Visibility.Collapsed;
            lButtonDown = true;
            line.X1 = line.X2 = e.GetPosition(grid).X;
            line.Y1 = line.Y2 = e.GetPosition(grid).Y;
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lButtonDown = false;
        }
    }
}