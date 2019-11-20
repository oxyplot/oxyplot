// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XkcdExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Plot examples in XKCD style.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    /// <summary>
    /// Plot examples in XKCD style.
    /// </summary>
    [Examples("XKCD")]
    public static class XkcdExamples
    {
        /// <summary>
        /// Xkcd style example #1.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Test #1")]
        public static PlotModel Test1()
        {
            var model = new PlotModel
            {
                Title = "XKCD style plot",
                Subtitle = "Install the 'Humor Sans' font for the best experience",
                RenderingDecorator = rc => new XkcdRenderingDecorator(rc)
            };
            model.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 50, "sin(x)"));
            return model;
        }


        /// <summary>
        /// Xkcd style example #2.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Test #2")]
        public static PlotModel Test2()
        {
            var model = new PlotModel
            {
                Title = "Test #2",
                RenderingDecorator = rc => new XkcdRenderingDecorator(rc)
            };

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 8, Title = "INTENSITY" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "TIME" });

            var s1 = new LineSeries
            {
                Color = OxyColors.Cyan,
                StrokeThickness = 4,
            };

            var s2 = new LineSeries
            {
                Color = OxyColors.White,
                StrokeThickness = 14,
            };

            var s3 = new LineSeries
            {
                Color = OxyColors.Red,
                StrokeThickness = 4,
            };

            int n = 257;
            double x0 = 1;
            double x1 = 9;
            for (int i = 0; i < n; i++)
            {
                var x = x0 + ((x1 - x0) * i / (n - 1));
                var y1 = 1.5 + (10.0 * (Math.Sin(x) * Math.Sin(x) / Math.Sqrt(x)) * Math.Exp(-0.5 * (x - 5.0) * (x - 5.0)));
                var y2 = 3.0 + (10.0 * (Math.Sin(x) * Math.Sin(x) / Math.Sqrt(x)) * Math.Exp(-0.5 * (x - 7.0) * (x - 7.0)));
                s1.Points.Add(new DataPoint(x, y1));
                s2.Points.Add(new DataPoint(x, y2));
                s3.Points.Add(new DataPoint(x, y2));
            }

            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Series.Add(s3);

            return model;
        }


        /// <summary>
        /// Xkcd style example #3.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Test #3")]
        public static PlotModel Test3()
        {
            var model = new PlotModel
            {
                Title = "Test #3",
                RenderingDecorator = rc => new XkcdRenderingDecorator(rc)
            };

            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            model.Legends.Add(l);
            var s1 = new BarSeries { Title = "Series 1", IsStacked = false, StrokeColor = OxyColors.Black, StrokeThickness = 1, XAxisKey = "x", YAxisKey = "y" };
            s1.Items.Add(new BarItem { Value = 25 });
            s1.Items.Add(new BarItem { Value = 137 });
            s1.Items.Add(new BarItem { Value = 18 });
            s1.Items.Add(new BarItem { Value = 40 });

            var s2 = new BarSeries { Title = "Series 2", IsStacked = false, StrokeColor = OxyColors.Black, StrokeThickness = 1, XAxisKey = "x", YAxisKey = "y" };
            s2.Items.Add(new BarItem { Value = 12 });
            s2.Items.Add(new BarItem { Value = 14 });
            s2.Items.Add(new BarItem { Value = 120 });
            s2.Items.Add(new BarItem { Value = 26 });

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom, Key = "y" };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            var valueAxis = new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0, Key = "x" };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }
    }
}
