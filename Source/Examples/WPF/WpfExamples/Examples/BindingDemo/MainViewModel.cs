// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BindingDemo
{
    using System;
    using System.Collections.ObjectModel;

    public class MainViewModel
    {
        public MainViewModel()
        {
            this.Measurements = new Collection<Measurement>();
            const int N = 50000;
            this.Subtitle = "N = " + N;

            var r = new Random(385);
            double dy = 0;
            double y = 0;
            for (int i = 0; i < N; i++)
            {
                dy += (r.NextDouble() * 2) - 1;
                y += dy;
                this.Measurements.Add(new Measurement
                                     {
                                         Time = 2.5 * i / (N - 1),
                                         Value = y / (N - 1),
                                         Maximum = (y / (N - 1)) + 5,
                                         Minimum = (y / (N - 1)) - 5
                                     });
            }
        }

        public Collection<Measurement> Measurements { get; private set; }

        public string Subtitle { get; set; }
    }
}