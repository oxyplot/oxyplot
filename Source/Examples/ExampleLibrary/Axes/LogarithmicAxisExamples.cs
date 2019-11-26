// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogarithmicAxisExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    [Examples("LogarithmicAxis"), Tags("Axes")]
    public static class LogarithmicAxisExamples
    {
        [Example("LogarithmicAxis with default values")]
        public static PlotModel DefaultValues()
        {
            var m = new PlotModel();
            m.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Bottom });
            m.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Left});
            return m;
        }

        [Example("Amdahl's Law")]
        public static PlotModel AmdahlsLaw()
        {
            var model = new PlotModel { Title = "Amdahl's law" };

            Legend l = new Legend
            {
                LegendTitle = "Parallel portion"
            };
            model.Legends.Add(l);

            // http://en.wikipedia.org/wiki/Amdahl's_law
            Func<double, int, double> maxSpeedup = (p, n) => 1.0 / ((1.0 - p) + (double)p / n);
            Func<double, LineSeries> createSpeedupCurve = p =>
            {
                // todo: tracker does not work when smoothing = true (too few points interpolated on the left end of the curve)
                var ls = new LineSeries { Title = p.ToString("P0") };
                for (int n = 1; n <= 65536; n *= 2) ls.Points.Add(new DataPoint(n, maxSpeedup(p, n)));
                return ls;
            };
            model.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Bottom, Title = "Number of processors", Base = 2, MajorGridlineStyle = LineStyle.Solid, TickStyle = TickStyle.None });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 20, MinorStep = 2, MajorStep = 2, Title = "Speedup", StringFormat = "F2", MajorGridlineStyle = LineStyle.Solid, TickStyle = TickStyle.None });
            model.Series.Add(createSpeedupCurve(0.5));
            model.Series.Add(createSpeedupCurve(0.75));
            model.Series.Add(createSpeedupCurve(0.9));
            model.Series.Add(createSpeedupCurve(0.95));

            return model;
        }

        [Example("Richter magnitudes")]
        public static PlotModel RichterMagnitudes()
        {
            // http://en.wikipedia.org/wiki/Richter_magnitude_scale

            var model = new PlotModel
            {
                Title = "The Richter magnitude scale",
                PlotMargins = new OxyThickness(80, 0, 80, 40),
            };

            Legend l = new Legend
            {
                LegendPlacement = LegendPlacement.Inside,
                LegendPosition = LegendPosition.TopCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendSymbolLength = 24
            };

            model.Legends.Add(l);

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Richter magnitude scale", MajorGridlineStyle = LineStyle.None, TickStyle = TickStyle.None });

            var frequencyCurve = new LineSeries
            {
                Title = "Frequency",
                Color = OxyColor.FromUInt32(0xff3c6c9e),
                StrokeThickness = 3,
                MarkerStroke = OxyColor.FromUInt32(0xff3c6c9e),
                MarkerFill = OxyColors.White,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStrokeThickness = 3
            };

            frequencyCurve.Points.Add(new DataPoint(1.5, 8000 * 365 * 100));
            frequencyCurve.Points.Add(new DataPoint(2.5, 1000 * 365 * 100));
            frequencyCurve.Points.Add(new DataPoint(3.5, 49000 * 100));
            frequencyCurve.Points.Add(new DataPoint(4.5, 6200 * 100));
            frequencyCurve.Points.Add(new DataPoint(5.5, 800 * 100));
            frequencyCurve.Points.Add(new DataPoint(6.5, 120 * 100));
            frequencyCurve.Points.Add(new DataPoint(7.5, 18 * 100));
            frequencyCurve.Points.Add(new DataPoint(8.5, 1 * 100));
            frequencyCurve.Points.Add(new DataPoint(9.5, 1.0 / 20 * 100));
            model.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Left, Title = "Frequency / 100 yr", UseSuperExponentialFormat = true, MajorGridlineStyle = LineStyle.None, TickStyle = TickStyle.Outside });
            model.Series.Add(frequencyCurve);

            var energyCurve = new LineSeries
            {
                Title = "Energy",
                Color = OxyColor.FromUInt32(0xff9e6c3c),
                StrokeThickness = 3,
                MarkerStroke = OxyColor.FromUInt32(0xff9e6c3c),
                MarkerFill = OxyColors.White,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStrokeThickness = 3
            };

            energyCurve.Points.Add(new DataPoint(1.5, 11e6));
            energyCurve.Points.Add(new DataPoint(2.5, 360e6));
            energyCurve.Points.Add(new DataPoint(3.5, 11e9));
            energyCurve.Points.Add(new DataPoint(4.5, 360e9));
            energyCurve.Points.Add(new DataPoint(5.5, 11e12));
            energyCurve.Points.Add(new DataPoint(6.5, 360e12));
            energyCurve.Points.Add(new DataPoint(7.5, 11e15));
            energyCurve.Points.Add(new DataPoint(8.5, 360e15));
            energyCurve.Points.Add(new DataPoint(9.5, 11e18));
            energyCurve.YAxisKey = "energyAxis";

            model.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Right, Title = "Energy / J", Key = "energyAxis", UseSuperExponentialFormat = true, MajorGridlineStyle = LineStyle.None, TickStyle = TickStyle.Outside });
            model.Series.Add(energyCurve);

            return model;
        }

        [Example("LogarithmicAxis with AbsoluteMaximum")]
        public static PlotModel AbsoluteMaximum()
        {
            var model = new PlotModel { Title = "AbsoluteMaximum = 1000" };
            model.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Left, Minimum = 0.1, Maximum = 1000, AbsoluteMaximum = 1000 });
            model.Series.Add(new FunctionSeries(Math.Exp, 0, Math.Log(900), 100));
            return model;
        }

        [Example("LogarithmicAxis with AxisChanged event handler")]
        public static PlotModel AxisChangedEventHAndler()
        {
            var model = new PlotModel { Title = "AxisChanged event handler" };
            var logAxis = new LogarithmicAxis { Position = AxisPosition.Left, Minimum = 0.1, Maximum = 1000 };
            int n = 0;
            logAxis.AxisChanged += (s, e) => { model.Subtitle = "Changed " + (n++) + " times. ActualMaximum=" + logAxis.ActualMaximum; };
            model.Axes.Add(logAxis);
            model.Series.Add(new FunctionSeries(Math.Exp, 0, Math.Log(900), 100));
            return model;
        }

        [Example("Negative values")]
        public static PlotModel NegativeValues()
        {
            var model = new PlotModel { Title = "LogarithmicAxis", Subtitle = "LineSeries with negative values" };
            model.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Left });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Series.Add(new FunctionSeries(Math.Sin, 0, 40, 1000));
            return model;
        }

        [Example("Tick calculation")]
        public static PlotModel TickCalculation()
        {
            var model = new PlotModel { Title = "Tick calculation for different bases" };
            model.Axes.Add(new LogarithmicAxis { Title = "Base 10", Position = AxisPosition.Left, Minimum = 20, Maximum = 20000, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid });
            model.Axes.Add(new LogarithmicAxis { Title = "Base 7", Position = AxisPosition.Bottom, Base = 7, Minimum = 2, Maximum = 10000, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid });
            model.Axes.Add(new LogarithmicAxis { Title = "Base 5.5", Position = AxisPosition.Top, Base = 5.5, Minimum = 1, Maximum = 100 });
            model.Axes.Add(new LogarithmicAxis { Title = "Base 2", Position = AxisPosition.Right, Base = 2, Minimum = 1, Maximum = 1000000 });
            return model;
        }
    }
}
