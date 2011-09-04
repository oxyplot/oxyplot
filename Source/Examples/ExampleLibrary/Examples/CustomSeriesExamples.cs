//-----------------------------------------------------------------------
// <copyright file="CustomSeriesExamples.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

using System;
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("Custom Series")]
    public static class CustomSeriesExamples
    {

        [Example("ErrorSeries")]
        public static PlotModel ErrorSeries()
        {
            int n = 20;
            var model = new PlotModel("ErrorSeries");

            var s1 = new ErrorSeries { Title = "Measurements" };
            var random = new Random();
            double x = 0;
            double y = 0;
            for (int i = 0; i < n; i++)
            {
                x += 2 + random.NextDouble() * 10;
                y += 1 + random.NextDouble();
                var p = new DataPoint(x, y);
                s1.Points.Add(p);
                s1.XErrors.Add(1 + random.NextDouble() * 2);
                s1.YErrors.Add(1 + random.NextDouble() * 2);
            }
            model.Series.Add(s1);
            return model;
        }
    }
}
