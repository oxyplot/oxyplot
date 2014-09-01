// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace UserControlDemo
{
    using System;
    using System.Collections.Generic;

    using OxyPlot;

    public class ViewModel
    {
        public string Title { get; set; }

        public IList<DataPoint> Points { get; set; }

        static Random r = new Random(13);

        public ViewModel()
        {
            this.Points = new List<DataPoint>();
            for (int i = 0; i < 10; i++)
            {
                this.Points.Add(new DataPoint(i, r.NextDouble()));
            }
        }
    }
}