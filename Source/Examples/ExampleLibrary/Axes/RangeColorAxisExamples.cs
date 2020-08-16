// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RangeColorAxisExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("RangeColorAxis"), Tags("Axes")]
    public class RangeColorAxisExamples
    {
        [Example("ScatterSeries with Reversed RangeColorAxis (Horizontal)")]
        public static PlotModel ReversedHorizontalRangeColorAxis()
        {
            return RangeColorAxis(AxisPosition.Top, true);
        }

        [Example("ScatterSeries with Reversed  RangeColorAxis (Vertical)")]
        public static PlotModel ReversedVerticalRangeColorAxis()
        {
            return RangeColorAxis(AxisPosition.Right, true);
        }

        [Example("ScatterSeries with RangeColorAxis (Horizontal)")]
        public static PlotModel HorizontalRangeColorAxis()
        {
            return RangeColorAxis(AxisPosition.Top, false);
        }

        [Example("ScatterSeries with RangeColorAxis (Vertical)")]
        public static PlotModel VerticalRangeColorAxis()
        {
            return RangeColorAxis(AxisPosition.Right, false);
        }

        private static PlotModel RangeColorAxis(AxisPosition position, bool reverseAxis)
        {
            int n = 1000;

            string modelTitle = reverseAxis
                                    ? string.Format("ScatterSeries and Reversed RangeColorAxis (n={0})", n)
                                    : string.Format("ScatterSeries and RangeColorAxis (n={0})", n);

            var model = new PlotModel
            {
                Title = modelTitle,
                Background = OxyColors.LightGray
            };

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            var rca = new RangeColorAxis { Position = position, Maximum = 2, Minimum = -2 };
            rca.AddRange(0, 0.5, OxyColors.Blue);
            rca.AddRange(-0.2, -0.1, OxyColors.Red);

            if (reverseAxis)
            {
                rca.StartPosition = 1;
                rca.EndPosition = 0;
            }

            model.Axes.Add(rca);

            var s1 = new ScatterSeries { MarkerType = MarkerType.Square, MarkerSize = 6, };

            var random = new Random(13);
            for (int i = 0; i < n; i++)
            {
                double x = (random.NextDouble() * 2.2) - 1.1;
                s1.Points.Add(new ScatterPoint(x, random.NextDouble()) { Value = x });
            }

            model.Series.Add(s1);
            return model;
        }
    }
}
