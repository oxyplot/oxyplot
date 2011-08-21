using OxyPlot;

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;

    [Examples("General Axis examples")]
    public class AxisExamples : ExamplesBase
    {
        [Example("AbsoluteMinimum and AbsoluteMaximum")]
        public static PlotModel AbsoluteMinimumAndMaximum()
        {
            var model = new PlotModel("AbsoluteMinimum=-17, AbsoluteMaximum=63", "Zooming and panning is limited to these values.");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { Minimum = 0, Maximum = 50, AbsoluteMinimum = -17, AbsoluteMaximum = 63 });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { Minimum = 0, Maximum = 50, AbsoluteMinimum = -17, AbsoluteMaximum = 63 });
            return model;
        }

        [Example("Title with unit")]
        public static PlotModel TitleWithUnit()
        {
            var model = new PlotModel("Axis titles with units");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { Title = "Speed", Unit = "km/h" });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { Title = "Mass", Unit = "kg" });
            return model;
        }

        [Example("Zooming disabled")]
        public static PlotModel ZoomingDisabled()
        {
            var model = new PlotModel("Zooming disabled");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { IsZoomEnabled = false });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { IsZoomEnabled = false });
            return model;
        }

        [Example("Panning disabled")]
        public static PlotModel PanningDisabled()
        {
            var model = new PlotModel("Panning disabled");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { IsPanEnabled = false });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { IsPanEnabled = false });
            return model;
        }

        [Example("Dense intervals")]
        public static PlotModel DenseIntervals()
        {
            var model = CreatePlotModel();
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { IntervalLength = 30 });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { IntervalLength = 20 });
            return model;
        }

        [Example("Graph Paper")]
        public static PlotModel GraphPaper()
        {
            var model = CreatePlotModel();
            var c = OxyColors.DarkBlue;
            model.PlotType = PlotType.Cartesian;
            model.Axes.Add(
                new LinearAxis(AxisPosition.Bottom, "X") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, "Y") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            return model;
        }

        [Example("Log-Log Paper")]
        public static PlotModel LogLogPaper()
        {
            var model = CreatePlotModel();
            var c = OxyColors.DarkBlue;
            model.Axes.Add(new LogarithmicAxis(AxisPosition.Bottom, "X") { Minimum = 0.1, Maximum = 1000, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            model.Axes.Add(new LogarithmicAxis(AxisPosition.Left, "Y") { Minimum = 0.1, Maximum = 1000, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c) });
            return model;
        }

        [Example("Black background")]
        public static PlotModel OnBlack()
        {
            var model = CreatePlotModel();
            model.Background = OxyColors.Black;
            model.TextColor = OxyColors.White;
            model.BoxColor = OxyColors.White;
            var c = OxyColors.White;
            model.PlotType = PlotType.Cartesian;
            model.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 2, 1000, "f(x)=sin(x)"));
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, "x") { MajorStep = Math.PI / 2, FormatAsFractions = true, FractionUnit = Math.PI, FractionUnitSymbol = "π", MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c), TicklineColor = OxyColors.White });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, "f(x)") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Solid, MajorGridlineColor = OxyColor.FromAColor(40, c), MinorGridlineColor = OxyColor.FromAColor(20, c), TicklineColor = OxyColors.White });
            return model;
        }

        [Example("Auto adjusting margins")]
        public static PlotModel AutoAdjustingMargins()
        {
            var model = CreatePlotModel();
            model.LegendPosition = LegendPosition.RightBottom;
            model.PlotMargins = new OxyThickness(0);
            model.AutoAdjustPlotMargins = true;
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X") { TickStyle = TickStyle.Outside });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, "Y") { TickStyle = TickStyle.Outside });
            model.Series.Add(new LineSeries("Butterfly curve") { ItemsSource = ButterflyCurve(0, Math.PI * 4, 1000) });
            return model;
        }

        private static IEnumerable<DataPoint> ButterflyCurve(double t0, double t1, int n)
        {
            // http://en.wikipedia.org/wiki/Butterfly_curve_(transcendental)
            double dt = (t1 - t0) / (n - 1);
            for (int i = 0; i < n; i++)
            {
                double t = t0 + dt * i;
                double r = (Math.Exp(Math.Cos(t)) - 2 * Math.Cos(4 * t) - Math.Pow(Math.Sin(t / 12), 5));
                double x = Math.Sin(t) * r;
                double y = Math.Cos(t) * r;
                yield return new DataPoint(x, y);
            }
        }
    }
}