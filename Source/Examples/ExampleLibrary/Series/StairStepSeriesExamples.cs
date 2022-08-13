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

        [Example("StairStepSeries with invalid points")]
        public static PlotModel StairStepSeriesWithInvalidPoints()
        {
            var model = new PlotModel
            {
                Title = "StairStepSeries with invalid points",
                Subtitle = "Horizontal lines do not continue",
            };

            PopulateInvalidPointExampleModel(model, x => DataPoint.Undefined);

            return model;
        }

        [Example("StairStepSeries with invalid Y")]
        public static PlotModel StairStepSeriesWithInvalidY()
        {
            var model = new PlotModel
            {
                Title = "StairStepSeries with invalid Y",
                Subtitle = "Horizontal lines continue until X of point with invalid Y",
            };

            PopulateInvalidPointExampleModel(model, x => new DataPoint(x, double.NaN));

            return model;
        }

        [Example("StairStepSeries with non-monotonic X")]
        public static PlotModel StairStepSeriesWithNonmonotonicX()
        {
            var model = new PlotModel
            {
                Title = "StairStepSeries with non-monotonic X",
                Subtitle = "Lines form a boxed I-beam",
            };

            var iBeamSeries = new StairStepSeries
            {
                MarkerType = MarkerType.Circle,
                VerticalLineStyle = LineStyle.Dash,
                VerticalStrokeThickness = 4,
            };
            iBeamSeries.Points.Add(new DataPoint(1, 1));
            iBeamSeries.Points.Add(new DataPoint(3, 1));
            iBeamSeries.Points.Add(new DataPoint(2, 3));
            iBeamSeries.Points.Add(new DataPoint(1, 3));
            iBeamSeries.Points.Add(new DataPoint(3, 3));
            model.Series.Add(iBeamSeries);

            var boxBRSeries = new StairStepSeries
            {
                MarkerType = MarkerType.Circle,
                VerticalLineStyle = LineStyle.Dash,
                VerticalStrokeThickness = 4,
            };
            boxBRSeries.Points.Add(new DataPoint(1, 0));
            boxBRSeries.Points.Add(new DataPoint(2, 0));
            boxBRSeries.Points.Add(new DataPoint(0, 0));
            boxBRSeries.Points.Add(new DataPoint(4, 0));
            boxBRSeries.Points.Add(new DataPoint(4, 4));
            model.Series.Add(boxBRSeries);

            var boxTLSeries = new StairStepSeries
            {
                MarkerType = MarkerType.Circle,
                VerticalLineStyle = LineStyle.Dash,
                VerticalStrokeThickness = 4,
            };
            boxTLSeries.Points.Add(new DataPoint(3, 4));
            boxTLSeries.Points.Add(new DataPoint(2, 4));
            boxTLSeries.Points.Add(new DataPoint(4, 4));
            boxTLSeries.Points.Add(new DataPoint(0, 4));
            boxTLSeries.Points.Add(new DataPoint(0, 0));
            model.Series.Add(boxTLSeries);

            return model;
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

        private static void PopulateInvalidPointExampleModel(PlotModel model, Func<double, DataPoint> getInvalidPoint)
        {
            model.Legends.Add(new Legend()
            {
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
            });

            var series1 = new StairStepSeries
            {
                Title = "Invalid First Point",
                MarkerType = MarkerType.Circle,
            };
            series1.Points.Add(getInvalidPoint(0));
            series1.Points.Add(new DataPoint(1, 3.5));
            series1.Points.Add(new DataPoint(2, 4.0));
            series1.Points.Add(new DataPoint(3, 4.5));
            model.Series.Add(series1);

            var series2 = new StairStepSeries
            {
                Title = "Invalid Second Point",
                MarkerType = MarkerType.Circle,
            };
            series2.Points.Add(new DataPoint(0, 2.0));
            series2.Points.Add(getInvalidPoint(1));
            series2.Points.Add(new DataPoint(2, 3.0));
            series2.Points.Add(new DataPoint(3, 3.5));
            model.Series.Add(series2);

            var series3 = new StairStepSeries
            {
                Title = "Invalid Penultimate Point",
                MarkerType = MarkerType.Circle,
            };
            series3.Points.Add(new DataPoint(0, 1.0));
            series3.Points.Add(new DataPoint(1, 1.5));
            series3.Points.Add(getInvalidPoint(2));
            series3.Points.Add(new DataPoint(3, 2.5));
            model.Series.Add(series3);

            var series4 = new StairStepSeries
            {
                Title = "Invalid Last Point",
                MarkerType = MarkerType.Circle,
            };
            series4.Points.Add(new DataPoint(0, 0.0));
            series4.Points.Add(new DataPoint(1, 0.5));
            series4.Points.Add(new DataPoint(2, 1.0));
            series4.Points.Add(getInvalidPoint(3));
            model.Series.Add(series4);
        }
    }
}
