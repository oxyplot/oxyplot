// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryColorAxisExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("CategoryColorAxis"), Tags("Axes")]
    public class CategoryColorAxisExamples
    {
        [Example("CategoryColorAxis")]
        public static PlotModel StandardCategoryColorAxis()
        {
            var plotModel1 = new PlotModel { Title = "CategoryColorAxis" };
            var catAxis = new CategoryColorAxis { Key = "ccc", Palette = OxyPalettes.BlackWhiteRed(12) };
            catAxis.Labels.AddRange(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            plotModel1.Axes.Add(catAxis);
            var linearAxis = new LinearAxis { Position = AxisPosition.Left };
            var ss = new ScatterSeries { ColorAxisKey = catAxis.Key };
            ss.Points.Add(new ScatterPoint(0, 0) { Value = 0 });
            ss.Points.Add(new ScatterPoint(3, 0) { Value = 3 });
            plotModel1.Series.Add(ss);
            plotModel1.Axes.Add(linearAxis);
            return plotModel1;
        }

        [Example("Centered ticks, MajorStep = 4")]
        public static PlotModel MajorStep4()
        {
            var plotModel1 = new PlotModel { Title = "Major Step = 4, IsTickCentered = true" };
            var catAxis = new CategoryColorAxis
            {
                Palette = OxyPalettes.BlackWhiteRed(3),
                IsTickCentered = true,
                MajorStep = 4
            };
            catAxis.Labels.AddRange(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            plotModel1.Axes.Add(catAxis);
            var linearAxis = new LinearAxis { Position = AxisPosition.Left };
            plotModel1.Axes.Add(linearAxis);
            return plotModel1;
        }
    }
}