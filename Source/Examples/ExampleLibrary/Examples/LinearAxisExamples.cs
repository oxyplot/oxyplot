// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearAxisExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("LinearAxis")]
    public static class LinearAxisExamples
    {
        [Example("TickStyle: None")]
        public static PlotModel TickStyleNone()
        {
            return CreateTickStyleModel(TickStyle.None);
        }
        [Example("TickStyle: Crossing")]
        public static PlotModel TickStyleCrossing()
        {
            return CreateTickStyleModel(TickStyle.Crossing);
        }
        [Example("TickStyle: Inside")]
        public static PlotModel TickStyleInside()
        {
            return CreateTickStyleModel(TickStyle.Inside);
        }
        [Example("TickStyle: Outside")]
        public static PlotModel TickStyleOutside()
        {
            return CreateTickStyleModel(TickStyle.Outside);
        }

        private static PlotModel CreateTickStyleModel(TickStyle tickStyle)
        {
            var model = new PlotModel("TickStyle: " + tickStyle);
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom)
                               {
                                   TickStyle = tickStyle,
                                   MajorGridlineStyle = LineStyle.None,
                                   MinorGridlineStyle = LineStyle.None,
                                   MaximumPadding = 0,
                                   MinimumPadding = 0
                               });
            model.Axes.Add(new LinearAxis(AxisPosition.Left)
                               {
                                   TickStyle = tickStyle,
                                   MajorGridlineStyle = LineStyle.None,
                                   MinorGridlineStyle = LineStyle.None,
                                   MaximumPadding = 0,
                                   MinimumPadding = 0
                               });
            return model;
        }

        [Example("Gridlines: None")]
        public static PlotModel GridlinesNone()
        {
            return CreateGridlinesModel("None",LineStyle.None,LineStyle.None);
        }
        [Example("Gridlines: Horizontal")]
        public static PlotModel GridlinesHorizontal()
        {
            return CreateGridlinesModel("Horizontal", LineStyle.Solid, LineStyle.None);
        }
        [Example("Gridlines: Vertical")]
        public static PlotModel GridlinesVertical()
        {
            return CreateGridlinesModel("Vertical", LineStyle.None, LineStyle.Solid);
        }
        [Example("Gridlines: Both")]
        public static PlotModel GridlinesBoth()
        {
            return CreateGridlinesModel("Both", LineStyle.Solid, LineStyle.Solid);
        }

        private static PlotModel CreateGridlinesModel(string title, LineStyle horizontal, LineStyle vertical)
        {
            var model = new PlotModel("Gridlines: " + title);
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom)
            {
                MajorGridlineStyle = vertical,
                MinorGridlineStyle = vertical==LineStyle.Solid?LineStyle.Dot:LineStyle.None,
                MaximumPadding = 0,
                MinimumPadding = 0
            });
            model.Axes.Add(new LinearAxis(AxisPosition.Left)
            {
                MajorGridlineStyle = horizontal,
                MinorGridlineStyle = horizontal == LineStyle.Solid ? LineStyle.Dot : LineStyle.None,
                MaximumPadding = 0,
                MinimumPadding = 0
            });
            return model;
        }
    }
}