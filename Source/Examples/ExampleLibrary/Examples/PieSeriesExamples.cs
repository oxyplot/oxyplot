// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieSeriesExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using OxyPlot;

namespace ExampleLibrary
{
    [Examples("PieSeries")]
    public static class PieSeriesExamples
    {
        [Example("World population")]
        public static PlotModel LinearAxes()
        {
            var model = new PlotModel("World population by continent");
            // http://www.nationsonline.org/oneworld/world_population.htm
            // http://en.wikipedia.org/wiki/Continent

            var ps = new PieSeries();
            ps.Slices.Add(new PieSlice("Africa", 1030) { IsExploded = true });
            ps.Slices.Add(new PieSlice("Americas", 929) { IsExploded = true });
            ps.Slices.Add(new PieSlice("Asia", 4157));
            ps.Slices.Add(new PieSlice("Europe", 739) { IsExploded = true });
            ps.Slices.Add(new PieSlice("Oceania", 35) { IsExploded = true });
            ps.InnerDiameter = 0;
            ps.ExplodedDistance = 0.0;
            ps.Stroke = OxyColors.White;
            ps.StrokeThickness = 2.0;
            ps.InsideLabelPosition = 0.8;
            ps.AngleSpan = 360;
            ps.StartAngle = 0;
            model.Series.Add(ps);
            return model;
        }
    }
}