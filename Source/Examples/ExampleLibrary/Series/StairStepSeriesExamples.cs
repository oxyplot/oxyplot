// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StairStepSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides examples for the <see cref="StairStepSeries" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    /// <summary>
    /// Provides examples for the <see cref="StairStepSeries" />.
    /// </summary>
    [Examples("StairStepSeries"), Tags("Series")]
    public static class StairStepSeriesExamples
    {
        [Example("StairStepSeries")]
        [DocumentationExample("Series/StairStepSeries")]
        public static PlotModel StairStepSeries()
        {
            return CreateExampleModel(new StairStepSeries());
        }

        [Example("StairStepSeries with labels")]
        public static PlotModel StairStepSeriesWithLabels()
        {
            return CreateExampleModel(new StairStepSeries { LabelFormatString = "{1:0.00}" });
        }

        [Example("StairStepSeries with markers")]
        public static PlotModel StairStepSeriesWithMarkers()
        {
            return CreateExampleModel(new StairStepSeries
                         {
                             Color = OxyColors.SkyBlue,
                             MarkerType = MarkerType.Circle,
                             MarkerSize = 6,
                             MarkerStroke = OxyColors.White,
                             MarkerFill = OxyColors.SkyBlue,
                             MarkerStrokeThickness = 1.5
                         });
        }

        [Example("StairStepSeries with thin vertical lines")]
        public static PlotModel StairStepSeriesThinVertical()
        {
            return CreateExampleModel(new StairStepSeries
            {
                StrokeThickness = 3,
                VerticalStrokeThickness = 0.4,
                MarkerType = MarkerType.None
            });
        }

        [Example("StairStepSeries with dashed vertical lines")]
        public static PlotModel StairStepSeriesDashedVertical()
        {
            return CreateExampleModel(new StairStepSeries
            {
                VerticalLineStyle = LineStyle.Dash,
                MarkerType = MarkerType.None
            });
        }

        /// <summary>
        /// Creates an example model and fills the specified series with points.
        /// </summary>
        /// <param name="series">The series.</param>
        /// <returns>A plot model.</returns>
        private static PlotModel CreateExampleModel(DataPointSeries series)
        {
            var model = new PlotModel { Title = "StairStepSeries" };
            var l = new Legend
            {
                LegendSymbolLength = 24
            };

            model.Legends.Add(l);

            series.Title = "sin(x)";
            for (double x = 0; x < Math.PI * 2; x += 0.5)
            {
                series.Points.Add(new DataPoint(x, Math.Sin(x)));
            }

            model.Series.Add(series);
            return model;
        }
    }
}
