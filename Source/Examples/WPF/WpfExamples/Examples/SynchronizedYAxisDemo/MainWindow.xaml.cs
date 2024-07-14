// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SynchronizedYAxisDemo
{
  using System;
  using System.Windows;

  using OxyPlot;
  using OxyPlot.Axes;
  using OxyPlot.Series;

  using WpfExamples;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  [Example("Shows how to keep several (can be more than 2) y-axes of the same PlotModel in sync.")]
  public partial class MainWindow : Window
  {
    public PlotModel Model { get; set; } = new PlotModel { Title = "Synchronized Y-Axis Demo" };
    private bool _axisIsInternalchange = false;

    public MainWindow()
    {
      InitializeComponent();
      DataContext = this;
            
      this.Model.Axes.Add(new LinearAxis
      {
        Key = "y_left",
        Position = AxisPosition.Left,
        Minimum = -10,
        MajorStep = 10,
        TextColor = OxyColor.FromRgb(0xFF, 0x00, 0x00),
        MajorGridlineStyle = LineStyle.Dot
      });

      this.Model.Axes.Add(new LinearAxis
      {
        Key = "y_right",
        Position = AxisPosition.Right,
        Minimum = 0,
        MajorStep = 20,
        TextColor = OxyColor.FromRgb(0x00, 0x00, 0xFF)
      });

      foreach (Axis axis in this.Model.Axes)
        axis.AxisChanged += OnYAxisChanged;

      var seriesLeft = new LineSeries 
      {
        YAxisKey = "y_left",
        Color = OxyColor.FromRgb(0xFF, 0x00, 0x00)
      };
      var seriesRight = new LineSeries 
      { 
        YAxisKey = "y_right",
        Color = OxyColor.FromRgb(0x00, 0x00, 0xFF)
      };

      for(int i = 0; i <= 100; i++)
      {
        seriesLeft.Points.Add(new DataPoint(i, 90 - i));
        seriesRight.Points.Add(new DataPoint(i, 2 * i));
      }

      this.Model.Series.Add(seriesLeft);
      this.Model.Series.Add(seriesRight);      
    }

    private void OnYAxisChanged(object? sender, EventArgs e)
    {
      if (this._axisIsInternalchange || sender is null || sender is not Axis)
        return;

      this._axisIsInternalchange = true;
      Axis axis = (Axis)sender;

      if (axis.Position == AxisPosition.Bottom || axis.Position == AxisPosition.Top)  // you should not add this event to an x-axis, but well...
        return;

      for (int i = 0; i < this.Model.Axes.Count; i++)
      {
        if (axis.Key == this.Model.Axes[i].Key)
        {
          for (int j = 0; j < this.Model.Axes.Count; j++)
          {
            if ((i != j) && (this.Model.Axes[j].Position == AxisPosition.Left || this.Model.Axes[j].Position == AxisPosition.Right))
            {
              this.Model.Axes[j].Zoom((axis.ActualMinimum - this.Model.Axes[i].Minimum) * this.Model.Axes[j].MajorStep / this.Model.Axes[i].MajorStep + this.Model.Axes[j].Minimum, (axis.ActualMaximum - this.Model.Axes[i].Minimum) * this.Model.Axes[j].MajorStep / this.Model.Axes[i].MajorStep + this.Model.Axes[j].Minimum);
            }

          }
          break;
        }
      }

      this._axisIsInternalchange = false;
    }
  }
}
