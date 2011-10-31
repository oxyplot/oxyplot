// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscreteDataSeriesExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("Discrete Data Series")]
    public static class DiscreteDataSeriesExamples
    {
        [Example("StairStepSeries")]
        public static PlotModel StairStepSeries()
        {
            var model = new PlotModel("StairStepSeries") { LegendSymbolLength = 24 };
            var s1 = new StairStepSeries("sin(x)")
                         {
                             Color = OxyColors.SkyBlue,
                             MarkerType = MarkerType.Circle,
                             MarkerSize = 6,
                             MarkerStroke = OxyColors.White,
                             MarkerFill = OxyColors.SkyBlue,
                             MarkerStrokeThickness = 1.5
                         };
            for (double x = 0; x < Math.PI * 2; x += 0.5)
                s1.Points.Add(new DataPoint(x, Math.Sin(x)));
            model.Series.Add(s1);

            return model;
        }

        [Example("StemSeries")]
        public static PlotModel StemSeries()
        {
            var model = new PlotModel("StemSeries") { LegendSymbolLength = 24 };
            var s1 = new StemSeries("sin(x)")
                         {
                             Color = OxyColors.SkyBlue,
                             MarkerType = MarkerType.Circle,
                             MarkerSize = 6,
                             MarkerStroke = OxyColors.White,
                             MarkerFill = OxyColors.SkyBlue,
                             MarkerStrokeThickness = 1.5
                         };
            for (double x = 0; x < Math.PI * 2; x += 0.1)
                s1.Points.Add(new DataPoint(x, Math.Sin(x)));
            model.Series.Add(s1);

            return model;
        }
    }
}