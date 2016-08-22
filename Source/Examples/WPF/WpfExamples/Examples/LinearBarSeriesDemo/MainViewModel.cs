// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace LinearBarSeriesDemo
{
    using System;
    using System.Collections.Generic;

    public class MainViewModel
    {
        public MainViewModel()
        {
            this.Pnls = new List<Pnl>();

            var random = new Random(31);
            var dateTime = DateTime.Today.Add(TimeSpan.FromHours(9));
            for (var pointIndex = 0; pointIndex < 50; pointIndex++)
            {
                this.Pnls.Add(new Pnl
                {
                    Time = dateTime,
                    Value = -200 + random.Next(1000),
                });
                dateTime = dateTime.AddMinutes(1);
            }
        }

        public List<Pnl> Pnls { get; private set; }
    }

    public class Pnl
    {
        public DateTime Time { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return String.Format("{0:HH:mm} {1:0.0}", this.Time, this.Value);
        }
    }
}