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

        [Example("Bar series")]
        public static PlotModel Barseries()
        {
            var plotModel1 = new PlotModel();
            plotModel1.LegendPlacement = LegendPlacement.Outside;
            plotModel1.Title = "Bar series";
            var categoryAxis1 = new CategoryAxis();
            categoryAxis1.LabelField = "Label";
            categoryAxis1.MajorStep = 1;
            categoryAxis1.MinorStep = 1;
            plotModel1.Axes.Add(categoryAxis1);
            var linearAxis1 = new LinearAxis();
            linearAxis1.AbsoluteMinimum = 0;
            linearAxis1.MinimumPadding = 0;
            plotModel1.Axes.Add(linearAxis1);
            var barSeries1 = new BarSeries();
            barSeries1.FillColor = OxyColor.FromArgb(255, 78, 154, 6);
            barSeries1.ValueField = "Value1";
            barSeries1.Title = "2009";
            plotModel1.Series.Add(barSeries1);
            var barSeries2 = new BarSeries();
            barSeries2.FillColor = OxyColor.FromArgb(255, 200, 141, 0);
            barSeries2.ValueField = "Value2";
            barSeries2.Title = "2010";
            plotModel1.Series.Add(barSeries2);
            var barSeries3 = new BarSeries();
            barSeries3.FillColor = OxyColor.FromArgb(255, 204, 0, 0);
            barSeries3.ValueField = "Value3";
            barSeries3.Title = "2011";
            plotModel1.Series.Add(barSeries3);
            return plotModel1;
        }

    }
}
