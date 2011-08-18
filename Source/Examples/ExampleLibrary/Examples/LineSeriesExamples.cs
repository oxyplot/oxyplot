using System;
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("LineSeries")]
    public class LineSeriesExamples : ExamplesBase
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
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -1, 71, "Y-Axis"));
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, -1, 61, "X-Axis"));
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

        [Example("Marker types")]
        public static PlotModel MarkerTypes()
        {
            var model = new PlotModel("Marker types");
            model.Series.Add(CreateRandomLineSeries(10, "Circle", MarkerType.Circle));
            model.Series.Add(CreateRandomLineSeries(10, "Cross", MarkerType.Cross));
            model.Series.Add(CreateRandomLineSeries(10, "Diamond", MarkerType.Diamond));
            model.Series.Add(CreateRandomLineSeries(10, "Plus", MarkerType.Plus));
            model.Series.Add(CreateRandomLineSeries(10, "Square", MarkerType.Square));
            model.Series.Add(CreateRandomLineSeries(10, "Star", MarkerType.Star));
            model.Series.Add(CreateRandomLineSeries(10, "Triangle", MarkerType.Triangle));
            return model;
        }

        static readonly Random Randomizer = new Random();

        private static ISeries CreateRandomLineSeries(int n, string title, MarkerType markerType)
        {
            var s1 = new LineSeries { Title = title, MarkerType = markerType, MarkerStroke = OxyColors.Black, MarkerStrokeThickness = 1.0 };
            double x = 0;
            double y = 0;
            for (int i = 0; i < n; i++)
            {
                x += 2 + Randomizer.NextDouble() * 10;
                y += 1 + Randomizer.NextDouble();
                var p = new DataPoint(x, y);
                s1.Points.Add(p);
            }
            return s1;
        }

        [Example("Custom markers")]
        public static PlotModel CustomMarkers()
        {
            var model = new PlotModel("LineSeries with custom markers") { LegendSymbolLength = 30, PlotType = PlotType.Cartesian };
            const int N = 6;
            var customMarkerOutline = new ScreenPoint[N];
            for (int i = 0; i < N; i++)
            {
                double th = Math.PI * (4.0 * i / (N - 1) - 0.5);
                const double R = 1;
                customMarkerOutline[i] = new ScreenPoint(Math.Cos(th) * R, Math.Sin(th) * R);
            }

            var s1 = new LineSeries("Series 1")
                         {
                             Color = OxyColors.Red,
                             StrokeThickness = 2,
                             MarkerType = MarkerType.Custom,
                             MarkerOutline = customMarkerOutline,
                             MarkerFill = OxyColors.DarkRed,
                             MarkerStroke = OxyColors.Black,
                             MarkerStrokeThickness = 0,
                             MarkerSize = 10
                         };

            foreach (var pt in customMarkerOutline)
            {
                s1.Points.Add(new DataPoint(pt.X, -pt.Y));
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

        private static DataPointSeries CreateNormalDistributionSeries(double x0, double x1, double mean, double variance,
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

        [Example("Parametric function 1")]
        public static PlotModel Clover()
        {
            return CreateParametricPlot(
    t => 2 * Math.Cos(2 * t) * Math.Cos(t),
    t => 2 * Math.Cos(2 * t) * Math.Sin(t),
                // t=>-4*Math.Sin(2*t)*Math.Cos(t)-2*Math.Cos(2*t)*Math.Sin(t),
                // t=>-4*Math.Sin(2*t)*Math.Sin(t)+2*Math.Cos(2*t)*Math.Cos(t),))))
    0, Math.PI * 2, 1000,
    "Parametric function",
    "Using the CartesianAxes property",
    "2cos(2t)cos(t) , 2cos(2t)sin(t)");

        }

        [Example("Parametric function 2")]
        public static PlotModel ParametricFunction2()
        {
            return CreateParametricPlot(
    t => 3 * Math.Sin(5 * t),
    t => 3 * Math.Cos(3 * t),
    0, Math.PI * 2, 1000,
    "Parametric function",
    null,
    "3sin(5t) , 3cos(3t)");
        }

        [Example("Parametric function 3")]
        public static PlotModel ParametricFunction3()
        {
            return CreateParametricPlot(
    t => 2 * Math.Cos(t) + Math.Cos(8 * t),
    t => 2 * Math.Sin(t) + Math.Sin(8 * t),
    0, Math.PI * 2, 1000,
    "Parametric function",
    null,
    "2cos(t)+cos(8t) , 2sin(t)+sin(8t)");
        }

        [Example("Lemniscate of Bernoulli")]
        public static PlotModel LemniscateOfBernoulli()
        {
            // http://en.wikipedia.org/wiki/Lemniscate_of_Bernoulli
            double a = 1;
            return CreateParametricPlot(
                t => a * Math.Sqrt(2) * Math.Cos(t) / (Math.Sin(t) * Math.Sin(t) + 1),
                t => a * Math.Sqrt(2) * Math.Cos(t) * Math.Sin(t) / (Math.Sin(t) * Math.Sin(t) + 1),
                0, Math.PI * 2, 1000, "Lemniscate of Bernoulli");
        }

        [Example("Lemniscate of Gerono")]
        public static PlotModel LemniscateOfGerono()
        {
            // http://en.wikipedia.org/wiki/Lemniscate_of_Gerono
            return CreateParametricPlot(t => Math.Cos(t), t => Math.Sin(2 * t) / 2, 0, Math.PI * 2, 1000, "Lemniscate of Gerono");
        }

        [Example("Lissajous figure")]
        public static PlotModel LissajousFigure()
        {
            double a = 3;
            double b = 2;
            double delta = Math.PI / 2;
            // http://en.wikipedia.org/wiki/Lissajous_figure
            return CreateParametricPlot(t => Math.Sin(a * t + delta), t => Math.Sin(b * t), 0, Math.PI * 2, 1000, "Lissajous figure", null, "a=3, b=2, δ = π/2");
        }

        [Example("Rose curve")]
        public static PlotModel RoseCurve()
        {
            // http://en.wikipedia.org/wiki/Rose_curve

            var m = CreatePlotModel();
            m.PlotType = PlotType.Polar;
            m.BoxThickness = 0;

            m.Axes.Add(new LinearAxis(AxisPosition.Angle, 0, Math.PI * 2, Math.PI / 4, Math.PI / 16) { MajorGridlineStyle = LineStyle.Solid });
            m.Axes.Add(new LinearAxis(AxisPosition.Magnitude) { MajorGridlineStyle = LineStyle.Solid });

            int d = 4;
            int n = 3;
            double k = (double)n / d;
            m.Series.Add(new FunctionSeries(t => Math.Sin(k * t), t => t, 0, Math.PI * 2 * d, 1000, string.Format("d={0}, n={1}", d, n)));

            return m;
        }

        [Example("Limaçon of Pascal")]
        public static PlotModel LimaconOfPascal()
        {
            // http://en.wikipedia.org/wiki/Lima%C3%A7on

            var m = CreatePlotModel();
            m.PlotType = PlotType.Cartesian;
            for (int a = 4; a <= 4; a++)
                for (int b = 0; b <= 10; b++)
                {
                    m.Series.Add(
                        new FunctionSeries(
                            t => a / 2 + b * Math.Cos(t) + a / 2 * Math.Cos(2 * t),
                            t => b * Math.Sin(t) + a / 2 * Math.Sin(2 * t),
                            0,
                            Math.PI * 2,
                            1000,
                            string.Format("a={0}, b={1}", a, b)));
                }
            return m;
        }


        [Example("Trisectrix of Maclaurin")]
        public static PlotModel TrisectrixOfMaclaurin()
        {
            // http://en.wikipedia.org/wiki/Trisectrix_of_Maclaurin
            // http://mathworld.wolfram.com/MaclaurinTrisectrix.html

            var m = CreatePlotModel();
            m.PlotType = PlotType.Cartesian;
            double a = 1;
            m.Series.Add(new FunctionSeries(t => a * (t * t - 3) / (t * t + 1), t => a * t * (t * t - 3) / (t * t + 1), -5, 5, 1000));
            return m;
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
            pm.Series.Add(new FunctionSeries(t => 5 * Math.Cos(t), t => 5 * Math.Sin(t), 0, 2 * Math.PI, 1000, "cos(t),sin(t)"));
            return pm;
        }

        private static PlotModel CreateParametricPlot(Func<double, double> fx, Func<double, double> fy, double t0,
                                                      double t1, int n, string title, string subtitle = null,
                                                      string seriesTitle = null)
        {
            var plot = new PlotModel { Title = title, Subtitle = subtitle, PlotType = PlotType.Cartesian };
            plot.Series.Add(new FunctionSeries(fx, fy, t0, t1, n, seriesTitle));
            return plot;
        }

    }
}
