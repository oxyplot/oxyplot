// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAxisExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;

    [Examples("CategoryAxis"), Tags("Axes")]
    public static class CategoryAxisExamples
    {
        [Example("Standard")]
        public static PlotModel StandardCategoryAxis()
        {
            var plotModel1 = new PlotModel { Title = "Standard" };
            var catAxis = new CategoryAxis();
            catAxis.Labels.AddRange(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            plotModel1.Axes.Add(catAxis);
            var linearAxis = new LinearAxis { Position = AxisPosition.Left };
            plotModel1.Axes.Add(linearAxis);
            return plotModel1;
        }

        [Example("ItemsSource - string[]")]
        public static PlotModel ItemsSourceStrings()
        {
            var model = new PlotModel { Title = "CategoryAxis with string[] as ItemsSource" };
            model.Axes.Add(new CategoryAxis
            {
                StringFormat = "Item {0}",
                ItemsSource = new[] { "A", "B", "C" }
            });
            var linearAxis = new LinearAxis { Position = AxisPosition.Left };
            model.Axes.Add(linearAxis);
            return model;
        }

        [Example("ItemsSource - int[]")]
        public static PlotModel ItemsSourceValues()
        {
            var model = new PlotModel { Title = "CategoryAxis with int[] as ItemsSource" };
            model.Axes.Add(new CategoryAxis
            {
                StringFormat = "Item {0}",
                ItemsSource = new[] { 10, 100, 123 }
            });
            var linearAxis = new LinearAxis { Position = AxisPosition.Left };
            model.Axes.Add(linearAxis);
            return model;
        }

        [Example("MajorStep")]
        public static PlotModel MajorStepCategoryAxis()
        {
            var plotModel1 = new PlotModel { Title = "Major Step = 4, IsTickCentered = true" };
            var catAxis = new CategoryAxis { IsTickCentered = true, MajorStep = 4 };
            catAxis.Labels.AddRange(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            plotModel1.Axes.Add(catAxis);
            var linearAxis = new LinearAxis { Position = AxisPosition.Left };
            plotModel1.Axes.Add(linearAxis);
            return plotModel1;
        }
    }
}