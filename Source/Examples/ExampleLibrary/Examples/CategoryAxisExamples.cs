// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAxisExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Collections.Generic;

    using OxyPlot;

    [Examples("CategoryAxis")]
    public static class CategoryAxisExamples
    {
        [Example("Standard")]
        public static PlotModel StandardCategoryAxis()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = "Standard";
            var catAxis = new CategoryAxis();
            catAxis.Labels = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            plotModel1.Axes.Add(catAxis);
            var linearAxis = new LinearAxis();
            linearAxis.Position = AxisPosition.Left;
            plotModel1.Axes.Add(linearAxis);
            return plotModel1;
        }

        [Example("MajorStep")]
        public static PlotModel MajorStepCategoryAxis()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = "Major Step = 4, IsTickCentered = true";
            var catAxis = new CategoryAxis();
            catAxis.IsTickCentered = true;
            catAxis.MajorStep = 4;
            catAxis.Labels = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            plotModel1.Axes.Add(catAxis);
            var linearAxis = new LinearAxis();
            linearAxis.Position = AxisPosition.Left;
            plotModel1.Axes.Add(linearAxis);
            return plotModel1;
        }

    }
}
