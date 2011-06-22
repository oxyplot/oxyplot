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

        [Example("Filtering invalid points")]
        public static PlotModel FilteringInvalidPoints()
        {
            var plot = new PlotModel("Filtering invalid points");
            plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis"));
            plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis"));

            var lsNaN = new LineSeries("NaN");
            lsNaN.Points.Add(new DataPoint(double.NaN, double.NaN));
            lsNaN.Points.Add(new DataPoint(1, 0));
            lsNaN.Points.Add(new DataPoint(2, 10));
            lsNaN.Points.Add(new DataPoint(double.NaN, 20));
            lsNaN.Points.Add(new DataPoint(3, 10));
            lsNaN.Points.Add(new DataPoint(4, 0));
            lsNaN.Points.Add(new DataPoint(4.5, double.NaN));
            lsNaN.Points.Add(new DataPoint(5, 0));
            lsNaN.Points.Add(new DataPoint(6, 10));
            lsNaN.Points.Add(new DataPoint(double.NaN, double.NaN));
            lsNaN.Points.Add(new DataPoint(7, 0));
            lsNaN.Points.Add(new DataPoint(double.NaN, double.NaN));
            plot.Series.Add(lsNaN);
            var lsPosInf = new LineSeries("PositiveInfinity");
            lsPosInf.Points.Add(new DataPoint(double.PositiveInfinity, double.PositiveInfinity));
            lsPosInf.Points.Add(new DataPoint(1, 1));
            lsPosInf.Points.Add(new DataPoint(2, 11));
            lsPosInf.Points.Add(new DataPoint(double.PositiveInfinity, 20));
            lsPosInf.Points.Add(new DataPoint(3, 11));
            lsPosInf.Points.Add(new DataPoint(4, 1));
            lsPosInf.Points.Add(new DataPoint(4.5, double.PositiveInfinity));
            lsPosInf.Points.Add(new DataPoint(5, 1));
            lsPosInf.Points.Add(new DataPoint(6, 11));
            lsPosInf.Points.Add(new DataPoint(double.PositiveInfinity, double.PositiveInfinity));
            lsPosInf.Points.Add(new DataPoint(7, 1));
            lsPosInf.Points.Add(new DataPoint(double.PositiveInfinity, double.PositiveInfinity));
            plot.Series.Add(lsPosInf);
            var lsNegInf = new LineSeries("NegativeInfinity");
            lsNegInf.Points.Add(new DataPoint(double.NegativeInfinity, double.NegativeInfinity));
            lsNegInf.Points.Add(new DataPoint(1, 2));
            lsNegInf.Points.Add(new DataPoint(2, 12));
            lsNegInf.Points.Add(new DataPoint(double.NegativeInfinity, 20));
            lsNegInf.Points.Add(new DataPoint(3, 12));
            lsNegInf.Points.Add(new DataPoint(4, 2));
            lsNegInf.Points.Add(new DataPoint(4.5, double.NegativeInfinity));
            lsNegInf.Points.Add(new DataPoint(5, 2));
            lsNegInf.Points.Add(new DataPoint(6, 12));
            lsNegInf.Points.Add(new DataPoint(double.NegativeInfinity, double.NegativeInfinity));
            lsNegInf.Points.Add(new DataPoint(7, 2));
            lsNegInf.Points.Add(new DataPoint(double.NegativeInfinity, double.NegativeInfinity));
            plot.Series.Add(lsNegInf);
            return plot;
        }

        [Example("Filtering invalid points (log axis)")]
        public static PlotModel FilteringInvalidPointsLog()
        {
            var plot = new PlotModel("Filtering invalid points on logarithmic axes");
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Bottom, "X-axis"));
            plot.Axes.Add(new LogarithmicAxis(AxisPosition.Left, "Y-axis"));

            var ls = new LineSeries();
            ls.Points.Add(new DataPoint(double.NaN, double.NaN));
            ls.Points.Add(new DataPoint(1, 1));
            ls.Points.Add(new DataPoint(10, 10));
            ls.Points.Add(new DataPoint(0, 20));
            ls.Points.Add(new DataPoint(100, 2));
            ls.Points.Add(new DataPoint(1000, 12));
            ls.Points.Add(new DataPoint(4.5, 0));
            ls.Points.Add(new DataPoint(10000, 4));
            ls.Points.Add(new DataPoint(100000, 14));
            ls.Points.Add(new DataPoint(double.NaN, double.NaN));
            ls.Points.Add(new DataPoint(1000000, 5));
            ls.Points.Add(new DataPoint(double.NaN, double.NaN));
            plot.Series.Add(ls);
            return plot;
        }
    
        [Example("Filtering points outside (-1,1)")]
        public static PlotModel FilteringPointsOutsideRange()
        {
            var plot = new PlotModel("Filtering points outside (-1,1)");
          plot.Axes.Add(new LinearAxis(AxisPosition.Bottom, "X-axis") { FilterMinValue=-1, FilterMaxValue=1});
          plot.Axes.Add(new LinearAxis(AxisPosition.Left, "Y-axis") { FilterMinValue = -1, FilterMaxValue = 1 });

            var ls = new LineSeries();
            for (double i = 0; i < 200;i+=0.01)
                ls.Points.Add(new DataPoint(0.01*i*Math.Sin(i), 0.01*i*Math.Cos(i)));
            plot.Series.Add(ls);
            return plot;
        }

    }
}
