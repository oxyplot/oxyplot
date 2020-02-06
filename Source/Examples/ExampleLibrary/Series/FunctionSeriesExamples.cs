// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    [Examples("FunctionSeries"), Tags("Series")]
    public class FunctionSeriesExamples
    {
        [Example("Square wave")]
        public static PlotModel SquareWave()
        {
            return CreateSquareWave(25);
        }

        private static PlotModel CreateSquareWave(int n = 25)
        {
            var plot = new PlotModel { Title = "Square wave (Gibbs phenomenon)" };

            Func<double, double> f = (x) =>
                {
                    double y = 0;
                    for (int i = 0; i < n; i++)
                    {
                        int j = i * 2 + 1;
                        y += Math.Sin(j * x) / j;
                    }
                    return y;
                };

            var fs = new FunctionSeries(f, -10, 10, 0.0001, "sin(x)+sin(3x)/3+sin(5x)/5+...+sin(" + (2 * n - 1) + ")/" + (2 * n - 1));

            plot.Series.Add(fs);
            plot.Subtitle = "n = " + fs.Points.Count;

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

            var m = new PlotModel
                        {
                            Title = "Rose curve",
                            PlotType = PlotType.Polar,
                            PlotAreaBorderThickness = new OxyThickness(0)
                        };

            m.Axes.Add(new AngleAxis
            {
                Minimum = 0,
                Maximum = Math.PI * 2,
                MajorStep = Math.PI / 4,
                MinorStep = Math.PI / 16,
                MajorGridlineStyle = LineStyle.Solid,
                FormatAsFractions = true,
                FractionUnit = Math.PI,
                FractionUnitSymbol = "π"
            });
            m.Axes.Add(new MagnitudeAxis() { MajorGridlineStyle = LineStyle.Solid });

            int d = 4;
            int n = 3;
            double k = (double)n / d;
            m.Series.Add(new FunctionSeries(t => Math.Sin(k * t), t => t, 0, Math.PI * 2 * d, 1000, string.Format("d={0}, n={1}", d, n)));

            return m;
        }

        [Example("Limaçon of Pascal")]
        [DocumentationExample("Series/FunctionSeries")]
        public static PlotModel LimaconOfPascal()
        {
            // http://en.wikipedia.org/wiki/Lima%C3%A7on

            var m = new PlotModel { Title = "Limaçon of Pascal", PlotType = PlotType.Cartesian };
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

        [Example("Folium of Descartes")]
        public static PlotModel DescartesFolium()
        {
            // http://www.wolframalpha.com/input/?i=folium+of+Descartes

            var m = new PlotModel { Title = "Folium of Descartes", PlotType = PlotType.Cartesian };
            m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -3, Maximum = 3 });
            m.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -3, Maximum = 3 });
            double a = 1;
            m.Series.Add(new FunctionSeries(t => 3 * a * t / (t * t * t + 1), t => 3 * a * t * t / (t * t * t + 1), -30, 30, 1001, string.Format("a={0}", a)));

            return m;
        }

        [Example("Trisectrix of Maclaurin")]
        public static PlotModel TrisectrixOfMaclaurin()
        {
            // http://en.wikipedia.org/wiki/Trisectrix_of_Maclaurin
            // http://mathworld.wolfram.com/MaclaurinTrisectrix.html

            var m = new PlotModel { Title = "Trisectrix of Maclaurin", PlotType = PlotType.Cartesian };
            double a = 1;
            m.Series.Add(new FunctionSeries(t => a * (t * t - 3) / (t * t + 1), t => a * t * (t * t - 3) / (t * t + 1), -5, 5, 1000));
            return m;
        }

        [Example("Fermat's spiral")]
        public static PlotModel FermatsSpiral()
        {
            // http://en.wikipedia.org/wiki/Fermat's_spiral
            // http://www.wolframalpha.com/input/?i=Fermat%27s+spiral
            var m = new PlotModel { Title = "Fermat's spiral", PlotType = PlotType.Cartesian };
            double a = 1;
            m.Series.Add(new FunctionSeries(t => a * Math.Sqrt(t) * Math.Cos(t), t => a * Math.Sqrt(t) * Math.Sin(t), 0, 20, 1000));
            m.Series.Add(new FunctionSeries(t => -a * Math.Sqrt(t) * Math.Cos(t), t => -a * Math.Sqrt(t) * Math.Sin(t), 0, 20, 1000));
            return m;
        }

        [Example("Fish curve")]
        public static PlotModel FishCurve()
        {
            // http://www.wolframalpha.com/input/?i=fish+curve
            var m = new PlotModel { Title = "Fish curve", PlotType = PlotType.Cartesian };
            for (double a = 0.1; a < 1; a += 0.1)
            {
                m.Series.Add(new FunctionSeries(t => a * (Math.Cos(t) - Math.Sin(t) * Math.Sin(t) / Math.Sqrt(2)), t => a * Math.Cos(t) * Math.Sin(t), 0, 2 * Math.PI, 1000));
            }

            return m;
        }

        [Example("Heaviside step function")]
        public static PlotModel HeavisideStepFunction()
        {
            // http://en.wikipedia.org/wiki/Heaviside_step_function

            var m = new PlotModel { Title = "Heaviside step function", PlotType = PlotType.Cartesian };
            m.Series.Add(new FunctionSeries(x =>
            {
                // make a gap in the curve at x=0
                if (Math.Abs(x) < 1e-8) return double.NaN;
                return x < 0 ? 0 : 1;
            }, -2, 2, 0.001));
            m.Annotations.Add(new LineAnnotation { Type = LineAnnotationType.Vertical, Color = m.DefaultColors[0], X = 0, MinimumY = 0, MaximumY = 1 });
            return m;
        }

        [Example("FunctionSeries")]
        public static PlotModel FunctionSeries()
        {
            var pm = new PlotModel
            {
                Title = "Trigonometric functions",
                Subtitle = "Example using the FunctionSeries",
                PlotType = PlotType.Cartesian,
                PlotAreaBackground = OxyColors.White
            };
            pm.Series.Add(new FunctionSeries(Math.Sin, -10, 10, 0.1, "sin(x)"));
            pm.Series.Add(new FunctionSeries(Math.Cos, -10, 10, 0.1, "cos(x)"));
            pm.Series.Add(new FunctionSeries(t => 5 * Math.Cos(t), t => 5 * Math.Sin(t), 0, 2 * Math.PI, 1000, "cos(t),sin(t)"));
            return pm;
        }

        [Example("Squirqle")]
        public static PlotModel Squirqle()
        {
            var plot = new PlotModel { Title = "Squirqle", PlotType = PlotType.Cartesian };
            plot.Series.Add(CreateSuperellipseSeries(4, 1, 1));

            return plot;
        }

        [Example("Superellipse n=20")]
        public static PlotModel Superellipse20()
        {
            var plot = new PlotModel { Title = "Superellipse", PlotType = PlotType.Cartesian };
            var s = CreateSuperellipseSeries(20, 1, 1);
            s.MarkerType = MarkerType.Circle;
            plot.Series.Add(s);

            return plot;
        }

        [Example("Lamé curves")]
        public static PlotModel LameCurves()
        {
            var plot = new PlotModel { Title = "Lamé curves", PlotType = PlotType.Cartesian };
            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside
            };

            plot.Legends.Add(l);

            for (double n = 0.25; n < 2; n += 0.25)
            {
                plot.Series.Add(CreateSuperellipseSeries(n, 1, 1));
            }

            for (double n = 2; n <= 8 + 1e-6; n += 1)
            {
                plot.Series.Add(CreateSuperellipseSeries(n, 1, 1));
            }

            return plot;
        }

        public static FunctionSeries CreateSuperellipseSeries(double n, double a, double b)
        {
            // http://en.wikipedia.org/wiki/Superellipse
            return new FunctionSeries(
                t => a * Math.Sign(Math.Cos(t)) * Math.Pow(Math.Abs(Math.Cos(t)), 2 / n),
                t => b * Math.Sign(Math.Sin(t)) * Math.Pow(Math.Abs(Math.Sin(t)), 2 / n),
                0,
                Math.PI * 2,
                101,
                string.Format("n={0}, a={1}, b={2}", n, a, b));
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
