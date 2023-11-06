// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearColorAxisExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;
    using System.Linq;

    [Examples("LinearColorAxis"), Tags("Axes")]
    public class LinearColorAxisExamples
    {
        private static PlotModel EnableRenderAsImage(PlotModel plotModel)
        {
            var axis = plotModel.Axes.OfType<LinearColorAxis>().Single();
            axis.RenderAsImage = true;
            plotModel.Title += " - RenderAsImage";
            return plotModel;
        }

        [Example("Peaks")]
        public static PlotModel Peaks()
        {
            return HeatMapSeriesExamples.CreatePeaks(null, false);
        }

        [Example("Peaks - RenderAsImage")]
        public static PlotModel PeaksRenderAsImage()
        {
            return EnableRenderAsImage(Peaks());
        }

        [Example("6 Colors")]
        public static PlotModel Horizontal6()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(6), false);
        }

        [Example("6 Colors - RenderAsImage")]
        public static PlotModel Horizontal6RenderAsImage()
        {
            return EnableRenderAsImage(Horizontal6());
        }

        [Example("Short")]
        public static PlotModel Short()
        {
            var model = HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(600), false);
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.StartPosition = 0.02;
            colorAxis.EndPosition = 0.5;
            return model;
        }

        [Example("Short - RenderAsImage")]
        public static PlotModel ShortRenderAsImage()
        {
            return EnableRenderAsImage(Short());
        }

        [Example("Position None")]
        public static PlotModel Position_None()
        {
            var model = HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(600), false);
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.Position = AxisPosition.None;
            return model;
        }
    }
}
