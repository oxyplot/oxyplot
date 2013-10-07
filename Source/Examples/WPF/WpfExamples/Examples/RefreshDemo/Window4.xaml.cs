// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window4.xaml.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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