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
using System;
using System.ComponentModel;
using System.Diagnostics;
using OxyPlot;

namespace RealtimeDemo
{
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public enum SimulationType
    {
        Waves,
        TimeSimulation
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Stopwatch watch = new Stopwatch();
        private int N;

        private SimulationType _simulationType;

        public SimulationType SimulationType
        {
            get
            {
                return this._simulationType;
            }
            set
            {
                this._simulationType = value;
                this.RaisePropertyChanged("SimulationType");
                this.SetupModel();
            }
        }

        public MainViewModel()
        {
            // try to change this value to TimeSimulation
            SimulationType = SimulationType.Waves;
            // SimulationType = SimulationType.TimeSimulation;

            Function = (t, x, a) => Math.Cos(t * a) * (x == 0 ? 1 : Math.Sin(x * a) / x);

            SetupModel();
        }

        void SetupModel()
        {
            PlotModel = new PlotModel();
            PlotModel.Axes.Add(new LinearAxis(AxisPosition.Left, -2, 2));

            this.N = this.SimulationType == SimulationType.TimeSimulation ? 1 : 20;

            for (int i = 0; i < N; i++)
            {
                PlotModel.Series.Add(new LineSeries());
            }
            watch.Start();
        }

        public int TotalNumberOfPoints { get; set; }

        private Func<double, double, double, double> Function { get; set; }

        public PlotModel PlotModel { get; set; }

        public void Update()
        {
            double t = watch.ElapsedMilliseconds * 0.001;
            int n = 0;

            for (int i = 0; i < PlotModel.Series.Count; i++)
            {
                var s = PlotModel.Series[i] as LineSeries;
                s.LineStyle = LineStyle.Solid;
                switch (SimulationType)
                {
                    case SimulationType.TimeSimulation:
                        {
                            double x = s.Points.Count > 0 ? s.Points[s.Points.Count - 1].X + 1 : 0;
                            if (s.Points.Count >= 200)
                                s.Points.RemoveAt(0);
                            double y = 0;
                            int m = 80;
                            for (int j = 0; j < m; j++)
                                y += Math.Cos(0.001 * x * j * j);
                            y /= m;
                            s.Points.Add(new DataPoint(x, y));
                            break;
                        }
                    case SimulationType.Waves:
                        s.Points.Clear();
                        double a = 0.5 + i * 0.05;
                        for (double x = -5; x <= 5; x += 0.01)
                        {
                            s.Points.Add(new DataPoint(x, Function(t, x, a)));
                        }
                        break;
                }

                n += s.Points.Count;
            }
            TotalNumberOfPoints = n;
            RaisePropertyChanged("TotalNumberOfPoints");
            RaisePropertyChanged("PlotModel");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}