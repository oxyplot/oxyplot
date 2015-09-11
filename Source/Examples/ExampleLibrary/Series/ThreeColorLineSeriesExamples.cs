// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreeColorLineSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides examples for the ThreeColorLineSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary.Series
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System;

    /// <summary>
    /// Provides examples for the <see cref="ThreeColorLineSeries" />.
    /// </summary>
    [Examples("ThreeColorLineSeries"), Tags("Series")]
    public class ThreeColorLineSeriesExamples
    {
        /// <summary>
        /// Creates an example showing temperatures.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Temperatures")]
        public static PlotModel ThreeColorLineSeries()
        {
            var plotModel1 = new PlotModel();
            plotModel1.LegendSymbolLength = 24;
            plotModel1.Title = "ThreeColorLineSeries";
            var linearAxis1 = new LinearAxis();
            linearAxis1.Title = "Temperature";
            linearAxis1.Unit = "°C";
            linearAxis1.ExtraGridlines = new Double[1];
            linearAxis1.ExtraGridlines[0] = 0;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.Position = AxisPosition.Bottom;
            linearAxis2.Title = "Date";
            plotModel1.Axes.Add(linearAxis2);
            var twoColorLineSeries1 = new ThreeColorLineSeries();
            twoColorLineSeries1.MarkerSize = 4;
            twoColorLineSeries1.MarkerStroke = OxyColors.Black;
            twoColorLineSeries1.MarkerStrokeThickness = 1.5;
            twoColorLineSeries1.MarkerType = MarkerType.Circle;
            //twoColorLineSeries1.Smooth = true;
            twoColorLineSeries1.StrokeThickness = 3;
            twoColorLineSeries1.Title = "Temperature Example";
            twoColorLineSeries1.TrackerFormatString = "December {2:0}: {4:0.0} °C";
            twoColorLineSeries1.Points.Add(new DataPoint(1, 5));
            twoColorLineSeries1.Points.Add(new DataPoint(2, 0));
            twoColorLineSeries1.Points.Add(new DataPoint(3, 7));
            twoColorLineSeries1.Points.Add(new DataPoint(4, 7));
            twoColorLineSeries1.Points.Add(new DataPoint(5, 4));
            twoColorLineSeries1.Points.Add(new DataPoint(6, 3));
            twoColorLineSeries1.Points.Add(new DataPoint(7, 5));
            twoColorLineSeries1.Points.Add(new DataPoint(8, 5));
            twoColorLineSeries1.Points.Add(new DataPoint(9, 11));
            twoColorLineSeries1.Points.Add(new DataPoint(10, 4));
            twoColorLineSeries1.Points.Add(new DataPoint(11, 2));
            twoColorLineSeries1.Points.Add(new DataPoint(12, 3));
            twoColorLineSeries1.Points.Add(new DataPoint(13, 2));
            twoColorLineSeries1.Points.Add(new DataPoint(14, 1));
            twoColorLineSeries1.Points.Add(new DataPoint(15, 0));
            twoColorLineSeries1.Points.Add(new DataPoint(16, 2));
            twoColorLineSeries1.Points.Add(new DataPoint(17, -1));
            twoColorLineSeries1.Points.Add(new DataPoint(18, 0));
            twoColorLineSeries1.Points.Add(new DataPoint(19, 0));
            twoColorLineSeries1.Points.Add(new DataPoint(20, -3));
            twoColorLineSeries1.Points.Add(new DataPoint(21, -6));
            twoColorLineSeries1.Points.Add(new DataPoint(22, -13));
            twoColorLineSeries1.Points.Add(new DataPoint(23, -10));
            twoColorLineSeries1.Points.Add(new DataPoint(24, -10));
            twoColorLineSeries1.Points.Add(new DataPoint(25, 0));
            twoColorLineSeries1.Points.Add(new DataPoint(26, -4));
            twoColorLineSeries1.Points.Add(new DataPoint(27, -5));
            twoColorLineSeries1.Points.Add(new DataPoint(28, -4));
            twoColorLineSeries1.Points.Add(new DataPoint(29, 3));
            twoColorLineSeries1.Points.Add(new DataPoint(30, 0));
            twoColorLineSeries1.Points.Add(new DataPoint(31, -5));

            twoColorLineSeries1.Points.Add(new DataPoint(32, -20));
            twoColorLineSeries1.Points.Add(new DataPoint(33, -20));
            twoColorLineSeries1.Points.Add(new DataPoint(34, 20));
            twoColorLineSeries1.Points.Add(new DataPoint(35, 20));
            plotModel1.Series.Add(twoColorLineSeries1);
            return plotModel1;
        }
    }
}
