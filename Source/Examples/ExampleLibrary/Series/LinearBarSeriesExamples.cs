namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("LinearBarSeries"), Tags("Series")]
    public class LinearBarSeriesExamples
    {
        //private static readonly Random Randomizer = new Random(13);

        [Example("Default style")]
        [DocumentationExample("Series/LinearBarSeries")]
        public static PlotModel DefaultStyle()
        {
            var model = new PlotModel { Title = "LinearBarSeries with default style" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            var linearBarSeries = CreateExampleLinearBarSeries();
            linearBarSeries.Title = "LinearBarSeries";
            model.Series.Add(linearBarSeries);

            return model;
        }

        [Example("With stroke")]
        public static PlotModel WithStroke()
        {
            var model = new PlotModel { Title = "LinearBarSeries with stroke" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            var linearBarSeries = CreateExampleLinearBarSeries();
            linearBarSeries.Title = "LinearBarSeries";
            linearBarSeries.FillColor = OxyColor.Parse("#454CAF50");
            linearBarSeries.StrokeColor = OxyColor.Parse("#4CAF50");
            linearBarSeries.StrokeThickness = 1;
            model.Series.Add(linearBarSeries);

            return model;
        }

        [Example("With negative colors")]
        public static PlotModel WithNegativeColors()
        {
            var model = new PlotModel { Title = "LinearBarSeries with stroke" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            var linearBarSeries = CreateExampleLinearBarSeriesWithNegativeValues();
            linearBarSeries.Title = "LinearBarSeries";
            linearBarSeries.FillColor = OxyColor.Parse("#454CAF50");
            linearBarSeries.StrokeColor = OxyColor.Parse("#4CAF50");
            linearBarSeries.NegativeFillColor = OxyColor.Parse("#45BF360C");
            linearBarSeries.NegativeStrokeColor = OxyColor.Parse("#BF360C");
            linearBarSeries.StrokeThickness = 1;
            model.Series.Add(linearBarSeries);

            return model;
        }

        /// <summary>
        /// Creates an example linear bar series.
        /// </summary>
        /// <returns>A linear bar series containing random points.</returns>
        private static LinearBarSeries CreateExampleLinearBarSeries()
        {
            var linearBarSeries = new LinearBarSeries();
            var r = new Random(31);
            var y = r.Next(10, 30);
            for (int x = 0; x <= 50; x++)
            {
                linearBarSeries.Points.Add(new DataPoint(x, y));
                y += r.Next(-5, 5);
            }
            return linearBarSeries;
        }

        /// <summary>
        /// Creates an example linear bar series with negative values.
        /// </summary>
        /// <returns>A linear bar series containing random points.</returns>
        private static LinearBarSeries CreateExampleLinearBarSeriesWithNegativeValues()
        {
            var linearBarSeries = new LinearBarSeries();
            var r = new Random(31);
            for (int x = 0; x <= 50; x++)
            {
                var y = -200 + r.Next(1000);
                linearBarSeries.Points.Add(new DataPoint(x, y));
            }
            return linearBarSeries;
        }
    }
}
