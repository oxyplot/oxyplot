// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoColorAreaSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides examples for the <see cref="TwoColorAreaSeries" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    /// <summary>
    /// Provides examples for the <see cref="TwoColorAreaSeries" />.
    /// </summary>
    [Examples("TwoColorAreaSeries"), Tags("Series")]
    public class TwoColorAreaSeriesExamples
    {
        /// <summary>
        /// Creates an example showing temperatures by a red/blue area chart.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Temperatures")]
        public static PlotModel TwoColorAreaSeries()
        {
            var model = new PlotModel { Title = "TwoColorAreaSeries" };
            var l = new Legend
            {
                LegendSymbolLength = 24
            };

            model.Legends.Add(l);

            var s1 = new TwoColorAreaSeries
            {
                Title = "Temperature at Eidesmoen, December 1986.",
                TrackerFormatString = "December {2:0}: {4:0.0} °C",
                Color = OxyColors.Tomato,
                Color2 = OxyColors.LightBlue,
                MarkerFill = OxyColors.Tomato,
                MarkerFill2 = OxyColors.LightBlue,
                StrokeThickness = 2,
                Limit = -1,
                MarkerType = MarkerType.Circle,
                MarkerSize = 3,
            };

            var temperatures = new[] { 5, 0, 7, 7, 4, 3, 5, 5, 11, 4, 2, 3, 2, 1, 0, 2, -1, 0, 0, -3, -6, -13, -10, -10, 0, -4, -5, -4, 3, 0, -5 };

            for (int i = 0; i < temperatures.Length; i++)
            {
                s1.Points.Add(new DataPoint(i + 1, temperatures[i]));
            }

            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Temperature", Unit = "°C", ExtraGridlines = new[] { 0.0 } });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Date" });

            return model;
        }

        /// <summary>
        /// Creates an example showing temperatures by a red/blue area chart.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Temperatures ver2")]
        [DocumentationExample("Series/TwoColorAreaSeries")]
        public static PlotModel TwoColorAreaSeries2()
        {
            var model = new PlotModel { Title = "TwoColorAreaSeries" };
            var l = new Legend
            {
                LegendSymbolLength = 24
            };

            model.Legends.Add(l);

            var s1 = new TwoColorAreaSeries
            {
                Title = "Temperature at Eidesmoen, December 1986.",
                TrackerFormatString = "December {2:0}: {4:0.0} °C",
                Color = OxyColors.Black,
                Color2 = OxyColors.Brown,
                MarkerFill = OxyColors.Red,
                Fill = OxyColors.Tomato,
                Fill2 = OxyColors.LightBlue,
                MarkerFill2 = OxyColors.Blue,
                MarkerStroke = OxyColors.Brown,
                MarkerStroke2 = OxyColors.Black,
                StrokeThickness = 2,
                Limit = 0,
                MarkerType = MarkerType.Circle,
                MarkerSize = 3,
            };

            var temperatures = new[] { 5, 0, 7, 7, 4, 3, 5, 5, 11, 4, 2, 3, 2, 1, 0, 2, -1, 0, 0, -3, -6, -13, -10, -10, 0, -4, -5, -4, 3, 0, -5 };

            for (int i = 0; i < temperatures.Length; i++)
            {
                s1.Points.Add(new DataPoint(i + 1, temperatures[i]));
            }

            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Temperature", Unit = "°C", ExtraGridlines = new[] { 0.0 } });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Date" });

            return model;
        }

        /// <summary>
        /// Creates an example showing temperatures by a red/blue area chart.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Temperatures ver3")]
        public static PlotModel TwoColorAreaSeries3()
        {
            var model = new PlotModel { Title = "TwoColorAreaSeries" };
            var l = new Legend
            {
                LegendSymbolLength = 24
            };

            model.Legends.Add(l);

            var s1 = new TwoColorAreaSeries
            {
                Title = "Temperature at Eidesmoen, December 1986.",
                TrackerFormatString = "December {2:0}: {4:0.0} °C",
                Color = OxyColors.Black,
                Color2 = OxyColors.Brown,
                MarkerFill = OxyColors.Red,
                Fill = OxyColors.Tomato,
                Fill2 = OxyColors.LightBlue,
                MarkerFill2 = OxyColors.Blue,
                MarkerStroke = OxyColors.Brown,
                MarkerStroke2 = OxyColors.Black,
                StrokeThickness = 1,
                Limit = 0,
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,
                MarkerType = MarkerType.Circle,
                MarkerSize = 1,
            };

            var temperatures = new[] { 5, 0, 7, 7, 4, 3, 5, 5, 11, 4, 2, 3, 2, 1, 0, 2, -1, 0, 0, -3, -6, -13, -10, -10, 0, -4, -5, -4, 3, 0, -5 };

            for (int i = 0; i < temperatures.Length; i++)
            {
                s1.Points.Add(new DataPoint(i + 1, temperatures[i]));
            }

            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Temperature", Unit = "°C", ExtraGridlines = new[] { 0.0 } });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Date" });

            return model;
        }

		/// <summary>
		/// Creates an example showing temperatures by a red/blue area chart.
		/// </summary>
		/// <returns>A <see cref="PlotModel" />.</returns>
		[Example("Two polygons")]
		public static PlotModel TwoColorAreaSeriesTwoPolygons()
		{
			var model = new PlotModel { Title = "Two polygons" };
            var l = new Legend
            {
                LegendSymbolLength = 24
            };

            model.Legends.Add(l);

            var s1 = new TwoColorAreaSeries
			{
				Color = OxyColors.Tomato,
				Color2 = OxyColors.LightBlue,
				MarkerFill = OxyColors.Tomato,
				MarkerFill2 = OxyColors.LightBlue,
				StrokeThickness = 2,
				MarkerType = MarkerType.Circle,
				MarkerSize = 3,
			};

			s1.Points.AddRange(new []{new DataPoint(0, 3), new DataPoint(1, 5), new DataPoint(2, 1), new DataPoint(3, 0), new DataPoint(4, 3) });
			s1.Points2.AddRange(new[] { new DataPoint(0, -3), new DataPoint(1, -1), new DataPoint(2, 0), new DataPoint(3, -6), new DataPoint(4, -4) });

			model.Series.Add(s1);
			model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
			model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom});

			return model;
		}
    }
}
