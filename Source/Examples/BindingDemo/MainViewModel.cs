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
            var r = new Random(385);
            double dy = 0;
            double y = 0;
            for (int i = 0; i < 100; i++)
            {
                dy += r.NextDouble() * 2 - 1;
                y += dy;
                Measurements.Add(new Measurement
                                     {
                                         Time = i * 2.5,
                                         Value = y,
                                         Maximum = y + 5,
                                         Minimum = y - 5
                                     });
            }
        }
    }
}