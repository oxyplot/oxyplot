//-----------------------------------------------------------------------
// <copyright file="BarSeriesExamples.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

using OxyPlot;

namespace ExampleLibrary
{
    [Examples("BarSeries")]
    public static class BarSeriesExamples
    {
        [Example("Simple BarSeries")]
        public static PlotModel SimpleBarSeries()
        {
            var model = CreateBarSeriesModel(false, false);
            return model;
        }

        [Example("Stacked BarSeries")]
        public static PlotModel StakcedBarSeries()
        {
            var model = CreateBarSeriesModel(true, false);
            return model;
        }

        [Example("Simple Horizontal BarSeries")]
        public static PlotModel SimpleHorizontalBarSeries()
        {
            var model = CreateBarSeriesModel(false, true);
            return model;
        }

        [Example("Stacked Horizontal BarSeries")]
        public static PlotModel StakcedHorizontalBarSeries()
        {
            var model = CreateBarSeriesModel(true, true);
            return model;
        }

        private static PlotModel CreateBarSeriesModel(bool stacked, bool horizontal)
        {
            var model = new PlotModel("BarSeries")
                          {
                              LegendPlacement = LegendPlacement.Outside,
                              LegendPosition = LegendPosition.BottomCenter,
                              LegendOrientation = LegendOrientation.Horizontal,
                              LegendBorderThickness = 0
                          };
            var s1 = new BarSeries { Title = "BarSeries 1", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Values.Add(25);
            s1.Values.Add(37);
            s1.Values.Add(18);
            s1.Values.Add(40);
            var s2 = new BarSeries { Title = "BarSeries 2", IsStacked = stacked, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Values.Add(12);
            s2.Values.Add(14);
            s2.Values.Add(20);
            s2.Values.Add(26);
            var categoryAxis = new CategoryAxis { Position = horizontal ? AxisPosition.Left : AxisPosition.Bottom };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(new LinearAxis(horizontal ? AxisPosition.Bottom : AxisPosition.Left) { MinimumPadding = 0, AbsoluteMinimum = 0 });
            return model;
        }
    }
}
