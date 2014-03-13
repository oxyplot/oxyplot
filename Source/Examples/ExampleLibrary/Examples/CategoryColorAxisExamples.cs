namespace ExampleLibrary
{
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("CategoryColorAxis")]
    public class CategoryColorAxisExamples : ExamplesBase
    {
        [Example("CategoryColorAxis")]
        public static PlotModel StandardCategoryColorAxis()
        {
            var plotModel1 = new PlotModel { Title = "CategoryColorAxis" };
            var catAxis = new CategoryColorAxis { Key = "ccc" };
            catAxis.Palette = OxyPalettes.BlackWhiteRed(12);
            catAxis.Labels = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            plotModel1.Axes.Add(catAxis);
            var linearAxis = new LinearAxis();
            linearAxis.Position = AxisPosition.Left;
            var ss = new ScatterSeries();
            ss.ColorAxisKey = catAxis.Key;
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
            var catAxis = new CategoryColorAxis();
            catAxis.Palette = OxyPalettes.BlackWhiteRed(3);
            catAxis.IsTickCentered = true;
            catAxis.MajorStep = 4;
            catAxis.Labels = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            plotModel1.Axes.Add(catAxis);
            var linearAxis = new LinearAxis();
            linearAxis.Position = AxisPosition.Left;
            plotModel1.Axes.Add(linearAxis);
            return plotModel1;
        }
    }
}