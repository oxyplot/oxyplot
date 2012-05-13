using System;
using System.Windows;

namespace RefreshDemo
{
    using System.ComponentModel;
    using System.Threading;

    using OxyPlot;

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
                    PlotModel.Title = "Plot updated: " + DateTime.Now;
                    this.PlotModel.Series[0] = new FunctionSeries(Math.Sin, x, x + 4, 0.01);
                    x += 0.1;
                    PlotModel.RefreshPlot(true);
                    Thread.Sleep(100);
                }
            };

            worker.RunWorkerAsync();
            this.Closed += (s, e) => worker.CancelAsync();
        }
    }
}
