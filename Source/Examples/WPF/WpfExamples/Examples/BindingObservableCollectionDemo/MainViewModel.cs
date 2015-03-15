// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace BindingObservableCollectionDemo
{
    using System;
    using System.Collections.ObjectModel;

    using global::BindingDemo;

    using WpfExamples;

    public class MainViewModel
    {
        private double dy = 0;

        private double y = 0;

        private Random random = new Random(385);

        private int i = 0;

        public MainViewModel()
        {
            this.Measurements = new ObservableCollection<Measurement>();
            const int N = 10;
            this.Subtitle = "N = " + 10;

            this.LoadCommand = new DelegateCommand(() => this.LoadNextPoint(N));
        }

        private void LoadNextPoint(int N)
        {
            for (int j = 0; j < N; j++)
            {
                this.dy += (this.random.NextDouble() * 2) - 1;
                this.y += this.dy;
                this.Measurements.Add(
                    new Measurement
                    {
                        Time = 2.5 * this.i / (N - 1),
                        Value = this.y / (N - 1),
                        Maximum = (this.y / (N - 1)) + 5,
                        Minimum = (this.y / (N - 1)) - 5
                    });
                i++;
            }
        }

        public ObservableCollection<Measurement> Measurements { get; private set; }

        public string Subtitle { get; set; }

        public DelegateCommand LoadCommand { get; private set; }
    }
}