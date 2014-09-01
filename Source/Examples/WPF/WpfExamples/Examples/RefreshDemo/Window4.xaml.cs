// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window4.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for Window4.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;

namespace RefreshDemo
{
    using System.ComponentModel;
    using System.Threading;

    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// Interaction logic for Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        public PlotModel PlotModel { get; set; }

        public Window4()
        {
            InitializeComponent();
            this.PlotModel = new PlotModel();
            this.PlotModel.Series.Add(new FunctionSeries());
            DataContext = this;
            var worker = new BackgroundWorker { WorkerSupportsCancellation = true };
            double x = 0;
            worker.DoWork += (s, e) =>
            {
                while (!worker.CancellationPending)
                {
                    lock (this.PlotModel.SyncRoot)
                    {
                        this.PlotModel.Title = "Plot updated: " + DateTime.Now;
                        this.PlotModel.Series[0] = new FunctionSeries(Math.Sin, x, x + 4, 0.01);
                    }
                    x += 0.1;
                    PlotModel.InvalidatePlot(true);
                    Thread.Sleep(100);
                }
            };

            worker.RunWorkerAsync();
            this.Closed += (s, e) => worker.CancelAsync();
        }
    }
}