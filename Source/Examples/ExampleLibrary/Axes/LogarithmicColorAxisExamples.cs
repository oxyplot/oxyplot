// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearColorAxisExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System.Linq;

    [Examples("LogarithmicColorAxis"), Tags("Axes")]
    public class LogarithmicColorAxisExamples
    {
        private static PlotModel ConvertToLogarithmic(PlotModel model)
        {
            var linearAxis = model.Axes.OfType<LinearColorAxis>().Single();
            model.Axes.Remove(linearAxis);
            var logarithmicAxis = new LogarithmicColorAxis() 
            { 
                Position = linearAxis.Position,
                Palette = linearAxis.Palette,
                StartPosition = linearAxis.StartPosition,
                EndPosition = linearAxis.EndPosition,
                HighColor = linearAxis.HighColor,
                LowColor = linearAxis.LowColor,
                InvalidNumberColor = linearAxis.InvalidNumberColor,
            };

            var series = model.Series.OfType<HeatMapSeries>().Single();
            for (int x = 0; x < series.Data.GetLength(0); x++)
            for (int y = 0; y < series.Data.GetLength(1); y++)
            {
                series.Data[x, y] += 6.6;
            }

            model.Axes.Add(logarithmicAxis);
            return model;
        }

        [Example("Peaks")]
        public static PlotModel Peaks()
        {
            return ConvertToLogarithmic(LinearColorAxisExamples.Peaks());
        }

        [Example("Peaks - RenderAsImage")]
        public static PlotModel PeaksRenderAsImage()
        {
            return ConvertToLogarithmic(LinearColorAxisExamples.PeaksRenderAsImage());
        }

        [Example("6 colors")]
        public static PlotModel Horizontal6()
        {
            return ConvertToLogarithmic(LinearColorAxisExamples.Horizontal6());
        }

        [Example("6 colors - RenderAsImage")]
        public static PlotModel Horizontal6RenderAsImage()
        {
            return ConvertToLogarithmic(LinearColorAxisExamples.Horizontal6RenderAsImage());
        }

        [Example("Short")]
        public static PlotModel Short()
        {
            return ConvertToLogarithmic(LinearColorAxisExamples.Short());
        }

        [Example("Short - RenderAsImage")]
        public static PlotModel ShortRenderAsImage()
        {
            return ConvertToLogarithmic(LinearColorAxisExamples.ShortRenderAsImage());
        }

        [Example("Position None")]
        public static PlotModel Position_None()
        {
            return ConvertToLogarithmic(LinearColorAxisExamples.Position_None());
        }
    }
}
