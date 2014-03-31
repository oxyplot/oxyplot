// <copyright file="ScatterWithErrorBarSeriesExamples.cs" company="LS Instruments">
//     Copyright (c) 2014 LS Instruments. All rights reserved.
// </copyright>
namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Series;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Examples("ScatterWithErrorBarSeries")]
    public class ScatterWithErrorBarSeriesExamples : ExamplesBase
    {
        [Example("Random points and error")]
        public static PlotModel ScatterWithErrorBarSeriesExample()
        {
            const int n = 20;
            var model = new PlotModel(string.Format("Random data (n={0})", n)) { LegendPosition = LegendPosition.LeftTop };

            var s1 = new ScatterWithErrorBarSeries { Title = "Measurements" };
            var random = new Random();
            double x = 0;
            double y = 0;
            for (int i = 0; i < n; i++)
            {
                x += 2 + random.NextDouble() * 10;
                y += 1 + random.NextDouble();
                double error = random.NextDouble();

                var p = new ScatterErrorPoint(x, y, error);
                s1.Points.Add(p);
            }
            model.Series.Add(s1);
            return model;
        }
    }
}
