// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("Tracker")]
    public static class TrackerExamples
    {
        [Example("No interpolation")]
        public static PlotModel NoInterpolation()
        {
            var model = new PlotModel("No tracker interpolation", "Used for discrete values or scatter plots.") { LegendSymbolLength = 30 };
            var s1 = new LineSeries("Series 1")
                         {
                             CanTrackerInterpolatePoints = false,
                             Color = OxyColors.SkyBlue,
                             MarkerType = MarkerType.Circle,
                             MarkerSize = 6,
                             MarkerStroke = OxyColors.White,
                             MarkerFill = OxyColors.SkyBlue,
                             MarkerStrokeThickness = 1.5
                         };
            for (int i = 0; i < 63; i++)
                s1.Points.Add(new DataPoint((int)(Math.Sqrt(i) * Math.Cos(i * 0.1)), (int)(Math.Sqrt(i) * Math.Sin(i * 0.1))));
            model.Series.Add(s1);

            return model;
        }
    }
}