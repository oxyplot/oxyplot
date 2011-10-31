// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using OxyPlot;

namespace AxesDemo
{
    public class MainViewModel
    {
        public IList<DataPoint> Data1 { get; set; }

        public MainViewModel()
        {
            Data1 = new List<DataPoint> { new DataPoint(10, 4), new DataPoint(12, 7), new DataPoint(16, 3), new DataPoint(20, 9) };
        }
    }
}