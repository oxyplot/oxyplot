// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Series;

    [Examples("PieSeries"), Tags("Series")]
    public static class PieSeriesExamples
    {
        [Example("PieSeries")]
        [DocumentationExample("Series/PieSeries")]
        public static PlotModel PieSeries()
        {
            return CreateExample();
        }

        [Example("PieSeries with inside label color")]
        public static PlotModel InsideLabelColor()
        {
            var model = CreateExample();
            var series = (PieSeries)model.Series[0];
            series.InsideLabelColor = OxyColors.White;
            return model;
        }

        private static PlotModel CreateExample()
        {
            var model = new PlotModel { Title = "World population by continent" };

            var ps = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };

            // http://www.nationsonline.org/oneworld/world_population.htm
            // http://en.wikipedia.org/wiki/Continent
            ps.Slices.Add(new PieSlice("Africa", 1030) { IsExploded = true });
            ps.Slices.Add(new PieSlice("Americas", 929) { IsExploded = true });
            ps.Slices.Add(new PieSlice("Asia", 4157));
            ps.Slices.Add(new PieSlice("Europe", 739) { IsExploded = true });
            ps.Slices.Add(new PieSlice("Oceania", 35) { IsExploded = true });
            
            model.Series.Add(ps);
            return model;
        }
    }
}
