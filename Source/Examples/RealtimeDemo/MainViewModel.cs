using System;
using System.ComponentModel;
using System.Diagnostics;
using OxyPlot;

namespace RealtimeDemo
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public int TotalNumberOfPoints { get; set; }
        private readonly Stopwatch watch = new Stopwatch();
        private int N = 20;

        public MainViewModel()
        {
            Function = (t, x, a) => Math.Cos(t*a)*(x == 0 ? 1 : Math.Sin(x*a)/x);

            PlotModel = new PlotModel();
            PlotModel.Axes.Add(new LinearAxis(AxisPosition.Left, -2, 2));

            for (int i = 0; i < N; i++)
            {
                // cos(at)*sin(ax)/x            }
                PlotModel.Series.Add(new LineSeries());
            }
            watch.Start();
        }

        private Func<double, double, double, double> Function { get; set; }
        public PlotModel PlotModel { get; set; }

        public void Update()
        {
            double t = watch.ElapsedMilliseconds*0.001;
            int n = 0;
            for (int i = 0; i < PlotModel.Series.Count; i++)
            {
                DataSeries s = PlotModel.Series[i];
                s.Points.Clear();
                double a = 0.5 + i*0.05;
                for (double x = -5; x <= 5; x += 0.1)
                {
                    s.Points.Add(new DataPoint(x, Function(t, x, a)));
                    n++;
                }
            }
            TotalNumberOfPoints = n;
            RaisePropertyChanged("TotalNumberOfPoints");
           // RaisePropertyChanged("PlotModel");
        }

        #region PropertyChanged Block

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }
}