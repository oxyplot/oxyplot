// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TornadoBarSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    [Examples("TornadoBarSeries"), Tags("Series")]
    public static class TornadoBarSeriesExamples
    {
        [Example("Tornado diagram 1")]
        [DocumentationExample("Series/TornadoBarSeries")]
        public static PlotModel TornadoDiagram1()
        {
            // http://en.wikipedia.org/wiki/Tornado_diagram
            var model = new PlotModel { Title = "Tornado diagram 1" };
            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside
            };

            model.Legends.Add(l);

            var s1 = new BarSeries
                {
                    Title = "High",
                    IsStacked = true,
                    FillColor = OxyColor.FromRgb(216, 82, 85),
                    BaseValue = 7,
                    StrokeColor = OxyColors.Black,
                    StrokeThickness = 1
                };
            s1.Items.Add(new BarItem(1));
            s1.Items.Add(new BarItem(1));
            s1.Items.Add(new BarItem(4));
            s1.Items.Add(new BarItem(5));

            var s2 = new BarSeries
                {
                    Title = "Low",
                    IsStacked = true,
                    FillColor = OxyColor.FromRgb(84, 138, 209),
                    BaseValue = 7,
                    StrokeColor = OxyColors.Black,
                    StrokeThickness = 1
                };
            s2.Items.Add(new BarItem(-1));
            s2.Items.Add(new BarItem(-3));
            s2.Items.Add(new BarItem(-2));
            s2.Items.Add(new BarItem(-3));

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("F/X rate");
            categoryAxis.Labels.Add("Inflation");
            categoryAxis.Labels.Add("Price");
            categoryAxis.Labels.Add("Conversion");
            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, ExtraGridlines = new[] { 7.0 } };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        [Example("Tornado diagram 2")]
        public static PlotModel TornadoDiagram2()
        {
            var model = new PlotModel { Title = "Tornado diagram 2" };
            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside
            };

            model.Legends.Add(l);
            var s1 = new TornadoBarSeries { Title = "TornadoBarSeries", BaseValue = 7 };
            s1.Items.Add(new TornadoBarItem { Minimum = 6, Maximum = 8 });
            s1.Items.Add(new TornadoBarItem { Minimum = 4, Maximum = 8 });
            s1.Items.Add(new TornadoBarItem { Minimum = 5, Maximum = 11 });
            s1.Items.Add(new TornadoBarItem { Minimum = 4, Maximum = 12 });

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("F/X rate");
            categoryAxis.Labels.Add("Inflation");
            categoryAxis.Labels.Add("Price");
            categoryAxis.Labels.Add("Conversion");
            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, ExtraGridlines = new[] { 7.0 }, MinimumPadding = 0.1, MaximumPadding = 0.1 };
            model.Series.Add(s1);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }
    }
}
