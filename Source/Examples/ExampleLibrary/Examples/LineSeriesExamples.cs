using System;
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("LineSeries")]
    public static class LineSeriesExamples
    {
        [Example("LineSeries")]
        public static PlotModel OneSeries()
        {
            var model = new PlotModel("LineSeries");
            model.LegendSymbolLength = 24;
            var s1 = new LineSeries("Series 1")
            {
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 6,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5
            };
            s1.Points.Add(new DataPoint(0, 10));
            s1.Points.Add(new DataPoint(10, 40));
            s1.Points.Add(new DataPoint(40, 20));
            s1.Points.Add(new DataPoint(60, 30));
            model.Series.Add(s1);

            return model;
        }

        [Example("Two LineSeries")]
        public static PlotModel TwoLineSeries()
        {
            var model = new PlotModel("Two LineSeries");
            model.LegendSymbolLength = 24;
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -1, 71, "Y-Axis") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot });
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -1, 61, "X-Axis") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot });
            var s1 = new LineSeries("Series 1")
            {
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 6,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5
            };
            s1.Points.Add(new DataPoint(0, 10));
            s1.Points.Add(new DataPoint(10, 40));
            s1.Points.Add(new DataPoint(40, 20));
            s1.Points.Add(new DataPoint(60, 30));
            model.Series.Add(s1);

            var s2 = new LineSeries("Series 2")
            {
                Color = OxyColors.Teal,
                MarkerType = MarkerType.Diamond,
                MarkerSize = 6,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.Teal,
                MarkerStrokeThickness = 1.5
            };
            s2.Points.Add(new DataPoint(0, 4));
            s2.Points.Add(new DataPoint(10, 32));
            s2.Points.Add(new DataPoint(40, 14));
            s2.Points.Add(new DataPoint(60, 20));
            model.Series.Add(s2);
            return model;
        }

        [Example("Custom markers")]
        public static PlotModel CustomMarkers()
        {
            var model = new PlotModel("LineSeries with custom markers");
            model.LegendSymbolLength = 30;
            int n = 6;
            var marker = new ScreenPoint[n];
            for (int i = 0; i < n; i++)
            {
                double th = Math.PI * (4.0 * i / (n - 1) - 0.5);
                double r = 1;
                marker[i] = new ScreenPoint(Math.Cos(th) * r, Math.Sin(th) * r);
            }

            var s1 = new LineSeries("Series 1")
                         {
                             Color = OxyColors.Red,
                             StrokeThickness = 2,
                             MarkerType = MarkerType.Custom,
                             MarkerOutline = marker,
                             MarkerFill = OxyColors.DarkRed,
                             MarkerStroke = OxyColors.Black,
                             MarkerStrokeThickness = 0,
                             MarkerSize = 10
                         };
            s1.Points.Add(new DataPoint(0, 20));
            s1.Points.Add(new DataPoint(10, 40));
            s1.Points.Add(new DataPoint(20, 20));
            s1.Points.Add(new DataPoint(24, 20));
            s1.Points.Add(new DataPoint(40, 10));
            s1.Points.Add(new DataPoint(60, 30));
            model.Series.Add(s1);
            return model;
        }

        [Example("Scatter plot using markers only")]
        public static PlotModel MarkersOnly()
        {
            return MarkersOnly(31000);
        }

        public static PlotModel MarkersOnly(int n)
        {
            var model = new PlotModel(string.Format("LineSeries with markers only (n={0})", n));

            var s1 = new LineSeries("Series 1") { StrokeThickness = 0, MarkerType = MarkerType.Square, MarkerFill = OxyColors.Blue, MarkerStrokeThickness = 0 };
            var random = new Random();
            for (int i = 0; i < n; i++)
            {
                s1.Points.Add(new DataPoint(random.NextDouble(), random.NextDouble()));
            }
            model.Series.Add(s1);
            return model;
        }

        [Example("Normal distribution")]
        public static PlotModel CreateNormalDistributionModel()
        {
            // http://en.wikipedia.org/wiki/Normal_distribution

            var plot = new PlotModel
            {
                Title = "Normal distribution",
                Subtitle = "Probability density function"
            };

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = -0.05,
                Maximum = 1.05,
                MajorStep = 0.2,
                MinorStep = 0.05,
                TickStyle = TickStyle.Inside
            });
            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = -5.25,
                Maximum = 5.25,
                MajorStep = 1,
                MinorStep = 0.25,
                TickStyle = TickStyle.Inside
            });
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 0.2));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 1));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 5));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, -2, 0.5));
            return plot;
        }

        private static DataSeries CreateNormalDistributionSeries(double x0, double x1, double mean, double variance,
                                                                 int n = 1001)
        {
            var ls = new LineSeries
            {
                Title = String.Format("μ={0}, σ²={1}", mean, variance)
            };

            for (int i = 0; i < n; i++)
            {
                double x = x0 + (x1 - x0) * i / (n - 1);
                double f = 1.0 / Math.Sqrt(2 * Math.PI * variance) * Math.Exp(-(x - mean) * (x - mean) / 2 / variance);
                ls.Points.Add(new DataPoint(x, f));
            }
            return ls;
        }

        [Example("Square wave")]
        public static PlotModel CreateSquareWave()
        {
            return CreateSquareWave(25);
        }

        private static PlotModel CreateSquareWave(int n = 25)
        {
            var plot = new PlotModel { Title = "Square wave (Gibbs phenomenon)" };

            var ls = new LineSeries
            {
                Title = "sin(x)+sin(3x)/3+sin(5x)/5+...+sin(" + (2 * n - 1) + ")/" + (2 * n - 1)
            };

            for (double x = -10; x < 10; x += 0.0001)
            {
                double y = 0;
                for (int i = 0; i < n; i++)
                {
                    int j = i * 2 + 1;
                    y += Math.Sin(j * x) / j;
                }
                ls.Points.Add(new DataPoint(x, y));
            }
            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = -4,
                Maximum = 4
            });
            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom
            });

            plot.Series.Add(ls);

            return plot;
        }

        [Example("Parametric function")]
        public static PlotModel Clover()
        {
            return CreateParametricPlot(
    t => 2 * Math.Cos(2 * t) * Math.Cos(t),
    t => 2 * Math.Cos(2 * t) * Math.Sin(t),
                // t=>-4*Math.Sin(2*t)*Math.Cos(t)-2*Math.Cos(2*t)*Math.Sin(t),
                // t=>-4*Math.Sin(2*t)*Math.Sin(t)+2*Math.Cos(2*t)*Math.Cos(t),))))
    0, Math.PI * 2, 0.01,
    "Parametric function",
    "Using the CartesianAxes property",
    "2cos(2t)cos(t) , 2cos(2t)sin(t)");

        }

        [Example("FunctionSeries")]
        public static PlotModel FunctionSeries()
        {
            var pm = new PlotModel("Trigonometric functions", "Example using the FunctionSeries")
                         {
                             PlotType = PlotType.Cartesian,
                             Background = OxyColors.White
                         };
            pm.Series.Add(new FunctionSeries(Math.Sin, -10, 10, 0.1, "sin(x)"));
            pm.Series.Add(new FunctionSeries(Math.Cos, -10, 10, 0.1, "cos(x)"));
            pm.Series.Add(new FunctionSeries(t => 5 * Math.Cos(t), t => 5 * Math.Sin(t), 0, 2 * Math.PI, 0.1, "cos(t),sin(t)"));
            return pm;
        }

        private static PlotModel CreateParametricPlot(Func<double, double> fx, Func<double, double> fy, double t0,
                                                      double t1, double dt, string title, string subtitle,
                                                      string seriesTitle)
        {
            var plot = new PlotModel { Title = title, Subtitle = subtitle, PlotType = PlotType.Cartesian };

            var ls = new LineSeries { Title = seriesTitle };

            for (double t = t0; t <= t1; t += dt)
            {
                ls.Points.Add(new DataPoint(fx(t), fy(t)));
            }
            plot.Series.Add(ls);
            return plot;
        }
    }
}
