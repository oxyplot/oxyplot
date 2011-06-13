using System;
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("LogarithmicAxis")]
    public static class LogarithmicAxisExamples
    {
        [Example("Amdahl's Law")]
        public static PlotModel AmdahlsLaw()
        {
            var model = new PlotModel("Amdahl's law") { LegendTitle = "Parallel portion" };

            // http://en.wikipedia.org/wiki/Amdahl's_law
            Func<double, int, double> maxSpeedup = (p, n) => 1.0 / ((1.0 - p) + (double)p / n);
            Func<double, LineSeries> createSpeedupCurve = p =>
            {
                var ls = new LineSeries(p.ToString("P0")) { Smooth = true };
                for (int n = 1; n <= 65536; n *= 2) ls.Points.Add(new DataPoint(n, maxSpeedup(p, n)));
                return ls;
            };
            model.Axes.Add(new LogarithmicAxis(AxisPosition.Bottom, "Number of processors") { MajorGridlineStyle = LineStyle.Solid, TickStyle = TickStyle.None });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 20, 2, 2, "Speedup") { StringFormat = "F2", MajorGridlineStyle = LineStyle.Solid, TickStyle = TickStyle.None });
            model.Series.Add(createSpeedupCurve(0.5));
            model.Series.Add(createSpeedupCurve(0.75));
            model.Series.Add(createSpeedupCurve(0.9));
            model.Series.Add(createSpeedupCurve(0.95));

            return model;
        }
    }
}