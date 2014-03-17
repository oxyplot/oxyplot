namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("RangeColorAxis")]
    public class RangeColorAxisExamples : ExamplesBase
    {
        [Example("ScatterSeries with RangeColorAxis")]
        public static PlotModel RangeColorAxis()
        {
            int n = 1000;
            var model = new PlotModel(string.Format("ScatterSeries and RangeColorAxis (n={0})", n))
                {
                    Background = OxyColors.LightGray
                };
            var rca = new RangeColorAxis { Position = AxisPosition.Right, Maximum = 2, Minimum = -2 };
            rca.AddRange(0, 0.5, OxyColors.Blue);
            rca.AddRange(-0.2, -0.1, OxyColors.Red);
            model.Axes.Add(rca);

            var s1 = new ScatterSeries { MarkerType = MarkerType.Square, MarkerSize = 6, };

            var random = new Random();
            for (int i = 0; i < n; i++)
            {
                double x = random.NextDouble() * 2.2 - 1.1;
                s1.Points.Add(new ScatterPoint(x, random.NextDouble()) { Value = x });
            }

            model.Series.Add(s1);
            return model;
        }
    }
}