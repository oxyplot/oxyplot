using System;
using System.Collections.ObjectModel;

namespace BindingDemo
{
    public class MainViewModel
    {
        public Collection<Measurement> Measurements { get; private set; }

        public MainViewModel()
        {
            Measurements = new Collection<Measurement>();
            var r = new Random();
            for (int i = 0; i < 100; i++)
            {
                double t = r.NextDouble() * 10 + 20;
                Measurements.Add(new Measurement
                                     {
                                         Time = i * 2.5,
                                         Value = t,
                                         Maximum = t + r.NextDouble() * 5,
                                         Minimum = t - r.NextDouble() * 5
                                     });
            }
        }
    }
}