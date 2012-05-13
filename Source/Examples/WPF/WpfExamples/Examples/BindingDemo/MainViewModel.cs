// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;

namespace BindingDemo
{
    public class MainViewModel
    {
        public Collection<Measurement> Measurements { get; private set; }

        public string Subtitle { get; set; }
        public MainViewModel()
        {
            Measurements = new Collection<Measurement>();
            int N = 50000;
            Subtitle = "N = " + N;

            var r = new Random(385);
            double dy = 0;
            double y = 0;
            for (int i = 0; i < N; i++)
            {
                dy += r.NextDouble() * 2 - 1;
                y += dy;
                Measurements.Add(new Measurement
                                     {
                                         Time = 2.5 * i / (N - 1),
                                         Value = y / (N - 1),
                                         Maximum = (y ) / (N - 1)+5,
                                         Minimum = (y ) / (N - 1)-5
                                     });
            }
        }
    }
}