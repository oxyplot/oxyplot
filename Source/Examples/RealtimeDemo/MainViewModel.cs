using System;
using System.ComponentModel;
using System.Diagnostics;
using OxyPlot;

namespace RealtimeDemo
{
    public enum SimulationType
    {
        Waves,
        TimeSimulation
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Stopwatch watch = new Stopwatch();
        private readonly int N = 20;
        public SimulationType SimulationType { get; set; }

        public MainViewModel()
        {
            // try to change this value to TimeSimulation
            SimulationType = SimulationType.Waves;
            // SimulationType = SimulationType.TimeSimulation;

            Function = (t, x, a) => Math.Cos(t*a)*(x == 0 ? 1 : Math.Sin(x*a)/x);

            PlotModel = new PlotModel();
            PlotModel.Axes.Add(new LinearAxis(AxisPosition.Left, -2, 2));

            if (SimulationType == SimulationType.TimeSimulation)
                N = 1;

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
            double t = watch.ElapsedMilliseconds*0.001;
            int n = 0;

            for (int i = 0; i < PlotModel.Series.Count; i++)
            {
                var s = PlotModel.Series[i] as DataSeries;

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
                                y += Math.Cos(0.001*x*j*j);
                            y /= m;
                            s.Points.Add(new DataPoint(x, y));
                            break;
                        }
                    case SimulationType.Waves:
                        s.Points.Clear();
                        double a = 0.5 + i*0.05;
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

        #region PropertyChanged Block

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }
}