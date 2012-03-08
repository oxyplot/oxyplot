// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeriesExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using OxyPlot;

namespace ExampleLibrary
{
    [Examples("TornadoBarSeries")]
    public static class TornadoBarSeriesExamples
    {
        [Example("Tornado diagram 1")]
        public static PlotModel TornadoDiagram1()
        {
            // http://en.wikipedia.org/wiki/Tornado_diagram
            var model = new PlotModel("Tornado diagram 1") { LegendPlacement = LegendPlacement.Outside };

            var s1 = new BarSeries { Title = "High", IsStacked = true, FillColor = OxyColor.FromRGB(216, 82, 85), BaseValue = 7, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Values.Add(1);
            s1.Values.Add(1);
            s1.Values.Add(4);
            s1.Values.Add(5);
            var s2 = new BarSeries { Title = "Low", IsStacked = true, FillColor = OxyColor.FromRGB(84, 138, 209), BaseValue = 7, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Values.Add(-1);
            s2.Values.Add(-3);
            s2.Values.Add(-2);
            s2.Values.Add(-3);
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("F/X rate");
            categoryAxis.Labels.Add("Inflation");
            categoryAxis.Labels.Add("Price");
            categoryAxis.Labels.Add("Conversion");
            var valueAxis = new LinearAxis(AxisPosition.Bottom) { ExtraGridlines = new[] { 7.0 } };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

    }
}