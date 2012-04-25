// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomSeriesExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using OxyPlot;

    [Examples("Custom series")]
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
                x += 2 + (random.NextDouble() * 10);
                y += 1 + random.NextDouble();
                double xe = 1 + (random.NextDouble() * 2);
                double ye = 1 + (random.NextDouble() * 2);
                s1.Points.Add(new ErrorItem(x, y, xe, ye));
            }

            model.Series.Add(s1);
            return model;
        }
    }
}