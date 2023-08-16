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

        [Example("IntervalBarSeries with various label types")]
        public static PlotModel IntervalBarSeriesWithLabels()
        {
            var model = new PlotModel { Title = "IntervalBarSeries" };
            var l = new Legend { LegendPlacement = LegendPlacement.Outside };

            model.Legends.Add(l);

            var s1 = new IntervalBarSeries { Title = "IntervalBarSeries 1", LabelFormatString = "{0} - {1}", LabelPlacement = LabelPlacement.Outside };
            s1.Items.Add(new IntervalBarItem { Start = 6, End = 8, CategoryIndex = 0 });
            s1.Items.Add(new IntervalBarItem { Start = 10, End = 12, CategoryIndex = 0 });
            model.Series.Add(s1);

            var s2 = new IntervalBarSeries { Title = "IntervalBarSeries 2", LabelFormatString = "{0} - {1}", LabelPlacement = LabelPlacement.Inside };
            s2.Items.Add(new IntervalBarItem { Start = 4, End = 8, CategoryIndex = 1 });
            s2.Items.Add(new IntervalBarItem { Start = 10, End = 12, CategoryIndex = 1 });
            model.Series.Add(s2);

            var s3 = new IntervalBarSeries { Title = "IntervalBarSeries 3", LabelFormatString = "{0} - {1}", LabelPlacement = LabelPlacement.Middle };
            s3.Items.Add(new IntervalBarItem { Start = 5, End = 11, CategoryIndex = 2 });
            s3.Items.Add(new IntervalBarItem { Start = 13, End = 17, CategoryIndex = 2 });
            model.Series.Add(s3);

            var s4 = new IntervalBarSeries { Title = "IntervalBarSeries 4", LabelFormatString = "{0} - {1}", LabelPlacement = LabelPlacement.Base };
            s4.Items.Add(new IntervalBarItem { Start = 4, End = 12, CategoryIndex = 3 });
            s4.Items.Add(new IntervalBarItem { Start = 13, End = 17, CategoryIndex = 3 });
            model.Series.Add(s4);

            var s5 = new IntervalBarSeries { Title = "IntervalBarSeries 5", LabelFormatString = "{0} - {1}", LabelPlacement = LabelPlacement.Outside, LabelAngle = -45 };
            s5.Items.Add(new IntervalBarItem { Start = 6, End = 8, CategoryIndex = 4 });
            s5.Items.Add(new IntervalBarItem { Start = 10, End = 12, CategoryIndex = 4 });
            model.Series.Add(s5);

            var s6 = new IntervalBarSeries { Title = "IntervalBarSeries 6", LabelFormatString = "{0} - {1}", LabelPlacement = LabelPlacement.Inside, LabelAngle = -45 };
            s6.Items.Add(new IntervalBarItem { Start = 4, End = 8, CategoryIndex = 5 });
            s6.Items.Add(new IntervalBarItem { Start = 10, End = 12, CategoryIndex = 5 });
            model.Series.Add(s6);

            var s7 = new IntervalBarSeries { Title = "IntervalBarSeries 7", LabelFormatString = "{0} - {1}", LabelPlacement = LabelPlacement.Middle, LabelAngle = -45 };
            s7.Items.Add(new IntervalBarItem { Start = 5, End = 11, CategoryIndex = 6 });
            s7.Items.Add(new IntervalBarItem { Start = 13, End = 17, CategoryIndex = 6 });
            model.Series.Add(s7);

            var s8 = new IntervalBarSeries { Title = "IntervalBarSeries 8", LabelFormatString = "{0} - {1}", LabelPlacement = LabelPlacement.Base, LabelAngle = -45 };
            s8.Items.Add(new IntervalBarItem { Start = 4, End = 12, CategoryIndex = 7 });
            s8.Items.Add(new IntervalBarItem { Start = 13, End = 17, CategoryIndex = 7 });
            model.Series.Add(s8);

            var categoryAxis = new CategoryAxis { Key = "CategoryAxis", Position = AxisPosition.Left, StartPosition = 1, EndPosition = 0 };
            categoryAxis.Labels.Add("Label Outside");
            categoryAxis.Labels.Add("Label Inside");
            categoryAxis.Labels.Add("Label Middle");
            categoryAxis.Labels.Add("Label Base");
            categoryAxis.Labels.Add("Label Outside (angled)");
            categoryAxis.Labels.Add("Label Inside (angled)");
            categoryAxis.Labels.Add("Label Middle (angled)");
            categoryAxis.Labels.Add("Label Base (angled)");
            
            var valueAxis = new LinearAxis { Key = "ValueAxis", Position = AxisPosition.Bottom, MinimumPadding = 0.1, MaximumPadding = 0.1 };

            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);

            return model;
        }
    }
}
