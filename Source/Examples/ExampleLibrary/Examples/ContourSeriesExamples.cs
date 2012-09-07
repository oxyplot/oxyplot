// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContourSeriesExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using OxyPlot;

namespace ExampleLibrary
{
    using System;

    [Examples("ContourSeries")]
    public class ContourSeriesExamples : ExamplesBase
    {
        [Example("Peaks")]
        public static PlotModel Peaks()
        {
            Func<double, double, double> peaks = (x, y) =>
               3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1))
               - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y)
               - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);

            var model = new PlotModel("Peaks");
            var cs = new ContourSeries
                {
                    ColumnCoordinates = ArrayHelper.CreateVector(-3, 3, 0.05),
                    RowCoordinates = ArrayHelper.CreateVector(-3.1, 3.1, 0.05)
                };
            cs.Data = ArrayHelper.Evaluate(peaks, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Series.Add(cs);
            return model;
        }

        [Example("Peaks (different contour colors)")]
        public static PlotModel PeaksWithColors()
        {
            Func<double, double, double> peaks = (x, y) =>
               3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1))
               - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y)
               - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);

            var model = new PlotModel("Peaks");
            var cs = new ContourSeries
            {
                ColumnCoordinates = ArrayHelper.CreateVector(-3, 3, 0.05),
                RowCoordinates = ArrayHelper.CreateVector(-3.1, 3.1, 0.05),
                ContourColors = new[] { OxyColors.SeaGreen, OxyColors.RoyalBlue, OxyColors.IndianRed }
            };
            cs.Data = ArrayHelper.Evaluate(peaks, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Series.Add(cs);
            return model;
        }
    }
}