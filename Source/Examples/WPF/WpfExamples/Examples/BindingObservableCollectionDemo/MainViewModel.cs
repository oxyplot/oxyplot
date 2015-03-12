// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BindingObservableCollectionDemo
{
    using System;
    using System.Collections.ObjectModel;

    using global::BindingDemo;

    using WpfExamples;

    public class MainViewModel
    {
        public MainViewModel()
        {
            this.Measurements = new ObservableCollection<Measurement>();
            const int N = 500;
            this.Subtitle = "N = " + N;

            this.LoadCommand = new DelegateCommand(() => this.Load(N));
        }

        private void Load(int N)
        {
            var r = new Random(385);
            double dy = 0;
            double y = 0;
            for (int i = 0; i < N; i++)
            {
                dy += (r.NextDouble() * 2) - 1;
                y += dy;
                this.Measurements.Add(
                    new Measurement
                    {
                        Time = 2.5 * i / (N - 1),
                        Value = y / (N - 1),
                        Maximum = (y / (N - 1)) + 5,
                        Minimum = (y / (N - 1)) - 5
                    });
            }
        }

        public ObservableCollection<Measurement> Measurements { get; private set; }

        public string Subtitle { get; set; }

        public DelegateCommand LoadCommand { get; private set; }
    }
}