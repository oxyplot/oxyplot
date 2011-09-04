//-----------------------------------------------------------------------
// <copyright file="DateTimeAxisExamples.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("DateTimeAxis")]
    public static class DateTimeAxisExamples
    {
        // [Example("DateTime Minimum bug")]
        public static PlotModel Example1()
        {
            var tmp = new PlotModel("Test");
            tmp.Axes.Add(new LinearAxis(AxisPosition.Left) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, TickStyle = TickStyle.Outside });
            DateTime dt = new DateTime(2010, 1, 1);
            tmp.Axes.Add(new DateTimeAxis(dt, dt.AddDays(1), AxisPosition.Bottom, null, null, DateTimeIntervalType.Hours)
            {
                MajorGridlineStyle = LineStyle.Solid,
                Angle = 90,
                StringFormat = "HH:mm",
                MajorStep = 1.0 / 24 / 2, // 1/24 = 1 hour, 1/24/2 = 30 minutes
                IsZoomEnabled = true,
                MaximumPadding = 0,
                MinimumPadding = 0,
                TickStyle = TickStyle.None
            });

            var ls = new LineSeries("Line1") { DataFieldX = "X", DataFieldY = "Y" };
            List<Item> ii = new List<Item>();

            for (int i = 0; i < 24; i++)
                ii.Add(new Item { X = dt.AddHours(i), Y = i * i });
            ls.ItemsSource = ii;
            tmp.Series.Add(ls);
            return tmp;
        }

        public class Item
        {
            public DateTime X { get; set; }
            public double Y { get; set; }
        }
    }
}
