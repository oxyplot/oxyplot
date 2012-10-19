// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomSeriesExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using OxyPlot;

    [Examples("Custom series")]
    public static class CustomSeriesExamples
    {
        [Example("ErrorSeries")]
        public static PlotModel ErrorSeries()
        {
            int n = 20;

            var model = new PlotModel("ErrorSeries");

            var s1 = new ErrorSeries { Title = "Measurements" };
            var random = new Random();
            double x = 0;
            double y = 0;
            for (int i = 0; i < n; i++)
            {
                x += 2 + (random.NextDouble() * 10);
                y += 1 + random.NextDouble();
                double xe = 1 + (random.NextDouble() * 2);
                double ye = 1 + (random.NextDouble() * 2);
                s1.Points.Add(new ErrorItem(x, y, xe, ye));
            }

            model.Series.Add(s1);
            return model;
        }

        [Example("LineSegmentSeries")]
        public static PlotModel LineSegmentSeries()
        {
            var model = new PlotModel("LineSegmentSeries");

            var lss1 = new LineSegmentSeries { Title = "The first series" };

            // First segment
            lss1.Points.Add(new DataPoint(0, 3));
            lss1.Points.Add(new DataPoint(2, 3.2));

            // Second segment
            lss1.Points.Add(new DataPoint(2, 2.7));
            lss1.Points.Add(new DataPoint(7, 2.9));

            model.Series.Add(lss1);

            var lss2 = new LineSegmentSeries { Title = "The second series" };

            // First segment
            lss2.Points.Add(new DataPoint(1, -3));
            lss2.Points.Add(new DataPoint(2, 10));

            // Second segment
            lss2.Points.Add(new DataPoint(0, 4.8));
            lss2.Points.Add(new DataPoint(7, 2.3));

            // A very short segment
            lss2.Points.Add(new DataPoint(6, 4));
            lss2.Points.Add(new DataPoint(6, 4 + 1e-8));

            model.Series.Add(lss2);

            return model;
        }
    }
}