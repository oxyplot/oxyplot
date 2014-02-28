// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
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
// --------------------------------------------------------------------------------------------------------------------
namespace TaskDemo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using OxyPlot;
    using OxyPlot.Series;

    public class MainViewModel
    {
        private Random randomizer = new Random();

        private IList<IDataPoint> points = new List<IDataPoint>();

        private CancellationTokenSource tokenSource;

        public PlotModel PlotModel { get; private set; }

        public LineSeries LineSeries1 { get; private set; }

        public MainViewModel()
        {
            // Create a plot model
            this.PlotModel = new PlotModel("Updating by task running on the UI thread");
            this.LineSeries1 = new LineSeries();
            this.points.Add(new DataPoint(0, 0));
            this.PlotModel.Series.Add(this.LineSeries1);

            System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.Name = "UI thread";
            Debug.WriteLine("The MainViewModel is created on: " + Thread.CurrentThread.Name);

            // Create a cancellation token source so we can cancel the worker task when
            // the window is closing
            this.tokenSource = new CancellationTokenSource();

            // this instance is created on the UI thread
            var context = SynchronizationContext.Current;

            // Start the point calculation worker task
            Task.Factory.StartNew(() => this.Work(this.tokenSource.Token, MaxPoints), this.tokenSource.Token);
            Task.Factory.StartNew(() => this.Update(context, this.tokenSource.Token), this.tokenSource.Token);
        }

        private const int MaxPoints = 20000;

        private void Work(CancellationToken token, int maxPoints)
        {
            // this loop runs on a worker thread
            while (!token.IsCancellationRequested)
            {
                lock (this.points)
                {
                    var point = this.CalculateNextPoint();
                    if (this.points.Count > maxPoints)
                    {
                        this.points.Clear();
                    }

                    this.points.Add(point);
                    Thread.Yield();
                }
            }
        }

        private void Update(SynchronizationContext context, CancellationToken token)
        {
            // this loop runs on a worker thread
            while (!token.IsCancellationRequested)
            {
                context.Post(_ => this.UpdatePlot(), null);
                Thread.Sleep(250);
            }
        }

        private IDataPoint CalculateNextPoint()
        {
            // this runs on a worker thread
            int i = this.points.Count - 1;
            double x = this.points[i].X;
            double y = this.points[i].Y;
            for (int j = 0; j < 20000; j++)
            {
                x += (this.randomizer.NextDouble() - 0.5) * 1e-3;
                y += (this.randomizer.NextDouble() - 0.5) * 1e-3;
            }

            return new DataPoint(x, y);
        }

        private void UpdatePlot()
        {
            Debug.WriteLine("Updating on: " + Thread.CurrentThread.Name);
            this.PlotModel.Subtitle = this.points.Count + " points added";

            lock (this.points)
            {
                // Create a copy of the points and give it to the LineSeries
                this.LineSeries1.Points = this.points.ToList();
            }

            this.PlotModel.RefreshPlot(true);
        }

        public void Closing()
        {
            // cancel the worker tasks
            this.tokenSource.Cancel();
        }
    }
}