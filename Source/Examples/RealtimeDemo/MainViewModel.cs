using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace RealtimeDemo
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Func<double, double, double> Function1 { get; set; }
        private Func<double, double, double> Function2 { get; set; }

        private readonly Stopwatch watch = new Stopwatch();

        public MainViewModel()
        {
            Function1 = (t, x) => Math.Cos(t) * (x == 0 ? 1 : Math.Sin(x) / x);
            Function2 = (t, x) => Math.Sin(t * 1.2) * (x == 0 ? 1 : Math.Sin(x * 1.2) / x);
            Results1 = new Collection<Point>();
            Results2 = new Collection<Point>();
            watch.Start();
        }

        public Collection<Point> Results1 { get; private set; }
        public Collection<Point> Results2 { get; private set; }

        public void Update()
        {
            double t = watch.ElapsedMilliseconds * 0.001;
            Results1.Clear();
            Results2.Clear();
            for (double x = -5; x <= 5; x += 0.1)
            {
                Results1.Add(new Point(x, Function1(t, x)));
                Results2.Add(new Point(x, Function2(t, x)));
            }
            RaisePropertyChanged("Results1");
            RaisePropertyChanged("Results2");
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