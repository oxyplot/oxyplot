// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window3.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for Window3.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;

namespace RefreshDemo
{
    using System.Windows.Media;

    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public PlotModel PlotModel { get; set; }

        public Window3()
        {
            InitializeComponent();
            this.PlotModel = new PlotModel();
            this.PlotModel.Series.Add(new FunctionSeries());
            this.DataContext = this;
            CompositionTarget.Rendering += this.CompositionTarget_Rendering;
        }

        private double lastUpdateTime;

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            double seconds = ((RenderingEventArgs)e).RenderingTime.TotalSeconds;
            if (seconds > this.lastUpdateTime + 0.2)
            {
                this.UpdatePlot();
                this.lastUpdateTime = seconds;
            }
        }

        private double x;

        void UpdatePlot()
        {
            // No need to lock to sync root - this is running on the UI thread
            this.PlotModel.Title = "Plot updated: " + DateTime.Now;
            this.PlotModel.Series[0] = new FunctionSeries(Math.Sin, x, x + 4, 0.01);
            x += 0.1;

            // Refresh the PlotModel
            this.PlotModel.InvalidatePlot(true);
        }
    }
}