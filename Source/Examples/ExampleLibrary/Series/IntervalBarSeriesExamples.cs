// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntervalBarSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    [Examples("IntervalBarSeries"), Tags("Series")]
    public static class IntervalBarSeriesExamples
    {
        [Example("IntervalBarSeries")]
        [DocumentationExample("Series/IntervalBarSeries")]
        public static PlotModel IntervalBarSeries()
        {
            var model = new PlotModel { Title = "IntervalBarSeries" };
            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside
            };

            model.Legends.Add(l);

            var s1 = new IntervalBarSeries { Title = "IntervalBarSeries 1", LabelFormatString = "{0} - {1}"};
            s1.Items.Add(new IntervalBarItem { Start = 6, End = 8 });
            s1.Items.Add(new IntervalBarItem { Start = 4, End = 8 });
            s1.Items.Add(new IntervalBarItem { Start = 5, End = 11 });
            s1.Items.Add(new IntervalBarItem { Start = 4, End = 12 });
            model.Series.Add(s1);
            var s2 = new IntervalBarSeries { Title = "IntervalBarSeries 2", LabelFormatString = "{0} - {1}" };
            s2.Items.Add(new IntervalBarItem { Start = 8, End = 9 });
            s2.Items.Add(new IntervalBarItem { Start = 8, End = 10 });
            s2.Items.Add(new IntervalBarItem { Start = 11, End = 12 });
            s2.Items.Add(new IntervalBarItem { Start = 12, End = 12.5 });
            model.Series.Add(s2);

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("Activity A");
            categoryAxis.Labels.Add("Activity B");
            categoryAxis.Labels.Add("Activity C");
            categoryAxis.Labels.Add("Activity D");
            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0.1, MaximumPadding = 0.1 };
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }
    }
}
