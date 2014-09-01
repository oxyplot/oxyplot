// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistogramTestHarness.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Obtains the image data once it is loaded
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Histogram
{
  public partial class HistogramTestHarness : UserControl
  {
    private int[] pixelData;

    private WriteableBitmap bitmap;

    private bool lButtonDown = false;

    public HistogramTestHarness()
    {
      InitializeComponent();

#if !WPF
      image.ImageOpened += Image_ImageOpened;
#endif
      grid.MouseLeftButtonDown += image_MouseLeftButtonDown;
      grid.MouseLeftButtonUp += Grid_MouseLeftButtonUp;
      Test.Click += Test_Click;

      var throttledEvent = new ThrottledMouseMoveEvent(image);
      throttledEvent.ThrottledMouseMove += new MouseEventHandler(ThrottledEvent_ThrottledMouseMove);

      CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);

    }

    protected Image Image
    {
      get
      {
        return image;
      }
    }

    protected ContentPresenter ChartPresenter
    {
      get
      {
        return contentPresenter;
      }
    }

    /// <summary>
    /// Obtains the image data once it is loaded
    /// </summary>
    protected void Image_ImageOpened(object sender, RoutedEventArgs e)
    {
      if (bitmap == null)
      {
        bitmap = new WriteableBitmap((BitmapImage)image.Source);
        image.Source = bitmap;
#if WPF
        int w = bitmap.PixelWidth;
        int h = bitmap.PixelHeight;
        pixelData = new int[w * h];
        int widthInBytes = 4 * w;
        bitmap.CopyPixels(pixelData, widthInBytes, 0);
#else
        pixelData = bitmap.Pixels;
#endif
      }
    }

    /// <summary>
    /// Computes the R, G & B intensity histograms between the given points
    /// </summary>
    private void RenderHistogram(double x1, double y1, double x2, double y2)
    {
      // compute distance between the points
      double distance = Math.Sqrt((x1 - x2) * (x1 - x2) +
          (y1 - y2) * (y1 - y2));

      // create the series for the R, G & B components
      var dataR = new List<DataPoint>();
      var dataG = new List<DataPoint>();
      var dataB = new List<DataPoint>();

      // build the charts
      for (double pt = 0; pt < distance; pt++)
      {
        double xPos = x1 + (x2 - x1) * pt / distance;
        double yPos = y1 + (y2 - y1) * pt / distance;

        int xIndex = (int)xPos;
        int yIndex = (int)yPos;

        int pixel = pixelData[xIndex + yIndex * 300];

        // the RGB values are 'packed' into an int, here we unpack them
        byte B = (byte)(pixel & 0xFF);
        pixel >>= 8;
        byte G = (byte)(pixel & 0xFF);
        pixel >>= 8;
        byte R = (byte)(pixel & 0xFF);
        pixel >>= 8;

        // add each datapoint to the series
        dataR.Add(new DataPoint(pt, R));
        dataG.Add(new DataPoint(pt, G));
        dataB.Add(new DataPoint(pt, B));
      }

      var rgbData =  new List<List<DataPoint>>(){
       dataR,
       dataG,
       dataB
     };

      RenderDataToChart(rgbData);
    }

    protected virtual void RenderDataToChart(List<List<DataPoint>> rgbData)
    {
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

      RenderHistogram(line.X1, line.Y1, line.X2, line.Y2);
    }

    private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

    private int _frame = 0;

    private bool _performanceTesting = false;

    private DateTime _startTime;

    private void Test_Click(object sender, RoutedEventArgs e)
    {
      Test.IsEnabled = false;
      _frame = 0;
      _performanceTesting = true;
      _startTime = DateTime.Now;
    }

    private void CompositionTarget_Rendering(object sender, EventArgs e)
    {
      if (_performanceTesting)
      {

#if WPF
        Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => ExecutePerformanceTest()));
#else
        Dispatcher.BeginInvoke(ExecutePerformanceTest);
#endif
      }
    }

    private void ExecutePerformanceTest()
    {
      _frame += 1;
      TestResults.Text = string.Format("{0}/100", _frame);
      if (_frame > 100)
      {
        _performanceTesting = false;

        DateTime endTime = DateTime.Now;
        double fps = 1000.0 / ((endTime - _startTime).TotalMilliseconds / 100.0);
        TestResults.Text = string.Format("{0:n2} fps", fps);
        Test.IsEnabled = true;
      }

      RenderHistogram(0, 0, _frame, 100);
      line.X1 = 0;
      line.X2 = _frame;
      line.Y1 = 0;
      line.Y2 = 180;
    }

  }

  /// <summary>
  /// A value object used to present the data to the chart
  /// </summary>
  public class DataPoint
  {
    public double Location { get; private set; }
    public double Intensity { get; private set; }

    public DataPoint(double location, double intensity)
    {
      Location = location;
      Intensity = intensity;
    }
  }
}