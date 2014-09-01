// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AxesDemo
{
    using System.Collections.Generic;

    using OxyPlot;

    public class MainViewModel
    {
        public MainViewModel()
        {
            this.Data1 = new List<DataPoint> { new DataPoint(10, 4), new DataPoint(12, 7), new DataPoint(16, 3), new DataPoint(20, 9) };
        }

        public IList<DataPoint> Data1 { get; set; }
    }
}