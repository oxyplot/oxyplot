// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RangeColorAxisExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("RangeColorAxis"), Tags("Axes")]
    public class RangeColorAxisExamples
    {
        [Example("ScatterSeries with RangeColorAxis")]
        public static PlotModel RangeColorAxis()
        {
            int n = 1000;
            var model = new PlotModel
                {
                    Title = string.Format("ScatterSeries and RangeColorAxis (n={0})", n),
                    Background = OxyColors.LightGray
                };

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            var rca = new RangeColorAxis { Position = AxisPosition.Right, Maximum = 2, Minimum = -2 };
            rca.AddRange(0, 0.5, OxyColors.Blue);
            rca.AddRange(-0.2, -0.1, OxyColors.Red);
            model.Axes.Add(rca);

            var s1 = new ScatterSeries { MarkerType = MarkerType.Square, MarkerSize = 6, };

            var random = new Random(13);
            for (int i = 0; i < n; i++)
            {
                double x = (random.NextDouble() * 2.2) - 1.1;
                s1.Points.Add(new ScatterPoint(x, random.NextDouble()) { Value = x });
            }

            model.Series.Add(s1);
            return model;
        }
    }
}