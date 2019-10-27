// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModelFactory.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using OxyPlot;

namespace ExportDemo
{
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public enum ModelType
    {
        SineWave,
        SmoothInterpolation,
        NormalDistribution,
        SquareWave,
        LogLog,
        LogLin,
        LinLog,
        Clover,
        KochSnowflake,
        KochSnowflake2,
        KochCurve,
        ZigZag,
        MathNotation,
        StraightLine
    }

    public class PlotModelFactory
    {
        public static PlotModel Create(ModelType type)
        {
            PlotModel model = null;
            switch (type)
            {
                case ModelType.SineWave:
                    model = CreateSineModel(0.002);
                    break;
                case ModelType.SmoothInterpolation:
                    model = CreateSineModel(1);
                    model.Title = "Smooth interpolation";
                    // Add markers to this plot
                    var ls = model.Series[0] as LineSeries;
                    if (ls == null) return null;
                    ls.MarkerType = MarkerType.Circle;
                    ls.Color = OxyColor.FromArgb(0xFF, 154, 6, 78);
                    ls.MarkerStroke = ls.Color;
                    ls.MarkerFill = OxyColor.FromAColor(0x70, ls.Color);
                    ls.MarkerStrokeThickness = 2;
                    ls.MarkerSize = 4;

                    var ls2 = CreateLineSeries(Math.Sin,
                                               0, 10, 1, "interpolated curve");
                    ls2.InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline;
                    model.Series.Add(ls2);
                    break;
                case ModelType.NormalDistribution:
                    model = CreateNormalDistributionModel();
                    break;
                case ModelType.SquareWave:
                    model = CreateSquareWave();
                    break;
                case ModelType.LogLog:
                    model = CreateLogLogPlot();
                    break;
                case ModelType.LogLin:
                    model = CreateLogLinPlot();
                    break;
                case ModelType.LinLog:
                    model = CreateLinLogPlot();
                    break;
                case ModelType.Clover:
                    // http://people.reed.edu/~jerry/Clover/cloverexcerpt.pdf
                    // http://www-math.bgsu.edu/z/calc3/vectorvalued1.html
                    model = CreateParametricPlot(
                        t => 2 * Math.Cos(2 * t) * Math.Cos(t),
                        t => 2 * Math.Cos(2 * t) * Math.Sin(t),
                        // t=>-4*Math.Sin(2*t)*Math.Cos(t)-2*Math.Cos(2*t)*Math.Sin(t),
                        // t=>-4*Math.Sin(2*t)*Math.Sin(t)+2*Math.Cos(2*t)*Math.Cos(t),))))
                        0, Math.PI * 2, 0.01,
                        "Parametric function",
                        "Using the CartesianAxes property",
                        "2cos(2t)cos(t) , 2cos(2t)sin(t)");
                    break;
                case ModelType.KochSnowflake:
                    model = CreateKochSnowflake(8);
                    break;
                case ModelType.KochSnowflake2:
                    model = CreateKochSnowflake(8, true);
                    break;
                case ModelType.KochCurve:
                    model = CreateKochCurve(4);
                    break;
                case ModelType.ZigZag:
                    model = CreateZigZagCurve(2000);
                    break;
                case ModelType.MathNotation:
                    model = CreateMathNotationPlot();
                    break;
                case ModelType.StraightLine:
                    model = CreateParametricPlot(
                        t => t,
                        t => 1 + t * 1e-8, 0, 10, 0.1,
                        "Straight line", null, null);
                    model.PlotType = PlotType.XY;
                    break;
            }

            return model;
        }

        private static DataPoint[] Fractalise(DataPoint[] data, DataPoint[] detail)
        {
            var result = new DataPoint[(data.Length - 1) * detail.Length + 1];
            int j = 0;
            for (int i = 0; i + 1 < data.Length; i++)
            {
                double dx = data[i + 1].X - data[i].X;
                double dy = data[i + 1].Y - data[i].Y;
                foreach (var uv in detail)
                    result[j++] = new DataPoint(data[i].X + dx * uv.X + dy * uv.Y, data[i].Y + dy * uv.X - dx * uv.Y);
            }
            result[j] = data.Last();
            return result;
        }

        private static readonly DataPoint[] KochDetail = new[]
                                                             {
                                                                 new DataPoint(0, 0),
                                                                 new DataPoint(1.0 / 3, 0),
                                                                 new DataPoint(0.5, Math.Sqrt(3.0 / 4.0) / 3),
                                                                 new DataPoint(2.0 / 3, 0)
                                                             };

        private static PlotModel CreateKochCurve(int n)
        {
            var data = new[]
                           {
                               new DataPoint(1, 0),
                               new DataPoint(0, 0)};
            for (int i = 0; i < n; i++)
                data = Fractalise(data, KochDetail);
            var model = new PlotModel { Title = "Koch curve" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0.1, MaximumPadding = 0.1 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0.1, MaximumPadding = 0.1 });
            model.PlotType = PlotType.Cartesian;
            var ls = new LineSeries { ItemsSource = data };
            model.Series.Add(ls);
            return model;
        }

        private static PlotModel CreateKochSnowflake(int n, bool areaSeries = false)
        {
            var data = new[]
                           {
                               new DataPoint(-1, 0),
                               new DataPoint(1, 0),
                               new DataPoint(0, Math.Sqrt(3)), new DataPoint(-1, 0) };
            for (int i = 0; i < n; i++)
                data = Fractalise(data, KochDetail);
            var model = new PlotModel { Title = "Koch Snowflake", PlotType = PlotType.Cartesian };
            if (areaSeries)
            {
                var s = new AreaSeries { ItemsSource = data, LineJoin = LineJoin.Bevel, Fill = OxyColors.LightGray };
                model.Series.Add(s);
            }
            else
            {
                var ls = new LineSeries { ItemsSource = data, LineJoin = LineJoin.Bevel };
                model.Series.Add(ls);
            }
            return model;
        }

        private static PlotModel CreateZigZagCurve(int n)
        {
            var model = new PlotModel { Title = "Zigzag curve" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0.1, MaximumPadding = 0.1 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0.1, MaximumPadding = 0.1 });
            var ls = new LineSeries();
            for (int i = 0; i < n; i++)
            {
                double x = 1.0 * i / (n - 1);
                double y = i % 2 == 0 ? 0 : 1;
                ls.Points.Add(new DataPoint(x, y));
            }
            model.Series.Add(ls);
            return model;
        }

        private static PlotModel CreateSineModel(double stepSize)
        {
            var plot = new PlotModel
            {
                Title = "Sine wave"
            };

            var ls = CreateLineSeries(Math.Sin, 0, 10, stepSize, "sin(x)");
            plot.Series.Add(ls);
            plot.Axes.Add(new LinearAxis
            {
                Title = "Y",
                Position = AxisPosition.Left,
                MaximumPadding = 0.3,
                MinimumPadding = 0.3,
                MajorGridlineStyle = LineStyle.Solid
            });
            plot.Axes.Add(new LinearAxis
            {
                Title = "X",
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid
            });

            return plot;
        }

        private static LineSeries CreateLineSeries(Func<double, double> f, double x0, double x1, double dx,
                                                   string title)
        {
            var ls = new LineSeries { Title = title };
            for (double x = x0; x <= x1; x += dx)
                ls.Points.Add(new DataPoint(x, f(x)));
            return ls;
        }

        private static PlotModel CreateNormalDistributionModel()
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
                                                                 int n = 1000)
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

        private static PlotModel CreateParametricPlot(Func<double, double> fx, Func<double, double> fy, double t0,
                                                      double t1, double dt, string title, string subtitle,
                                                      string seriesTitle)
        {
            var plot = new PlotModel { Title = title, Subtitle = subtitle, PlotType = PlotType.Cartesian };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StringFormat = "g10" });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            var ls = new LineSeries { Title = seriesTitle };

            for (double t = t0; t <= t1; t += dt)
            {
                ls.Points.Add(new DataPoint(fx(t), fy(t)));
            }
            plot.Series.Add(ls);
            return plot;
        }

        private static PlotModel CreateLogLinPlot()
        {
            // http://en.wikipedia.org/wiki/Lin-log_graph

            var plot = new PlotModel { Title = "Log-Lin plot" };

            plot.Series.Add(CreateLineSeries(x => x, 0.1, 100, 0.1, "y=x"));
            plot.Series.Add(CreateLineSeries(x => x * x, 0.1, 100, 0.1, "y=x²"));
            plot.Series.Add(CreateLineSeries(x => x * x * x, 0.1, 100, 0.1, "y=x³"));

            plot.Axes.Add(new LogarithmicAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0.1,
                Maximum = 100,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });
            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0.1,
                Maximum = 100,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });

            return plot;
        }

        private static PlotModel CreateLinLogPlot()
        {
            // http://en.wikipedia.org/wiki/Lin-log_graph
            var plot = new PlotModel { Title = "Lin-Log plot" };

            plot.Series.Add(CreateLineSeries(x => x, 0.1, 100, 0.1, "y=x"));
            plot.Series.Add(CreateLineSeries(x => x * x, 0.1, 100, 0.1, "y=x²"));
            plot.Series.Add(CreateLineSeries(x => x * x * x, 0.1, 100, 0.1, "y=x³"));

            plot.Axes.Add(new LogarithmicAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0.1,
                Maximum = 100,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });
            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0.1,
                Maximum = 100,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });

            return plot;
        }

        private static PlotModel CreateLogLogPlot()
        {
            // http://en.wikipedia.org/wiki/Log-log_plot
            var plot = new PlotModel { Title = "Log-log plot" };

            plot.Series.Add(CreateLineSeries(x => x, 0.1, 100, 0.1, "y=x"));
            plot.Series.Add(CreateLineSeries(x => x * x, 0.1, 100, 0.1, "y=x²"));
            plot.Series.Add(CreateLineSeries(x => x * x * x, 0.1, 100, 0.1, "y=x³"));

            plot.Axes.Add(new LogarithmicAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0.1,
                Maximum = 100,
                // UseSuperExponentialFormat = true,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });
            plot.Axes.Add(new LogarithmicAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0.1,
                Maximum = 100,
                // UseSuperExponentialFormat = true,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });

            return plot;
        }

        private static PlotModel CreateMathNotationPlot()
        {
            // http://en.wikipedia.org/wiki/Log-log_plot
            var plot = new PlotModel
            {
                Title = "E_{r}^{2} - (pc)^{2} = (m_{0}c^{2})^{2}",
                TitleFontSize = 24,
                LegendFontSize = 14,
                LegendPosition = LegendPosition.RightTop,
                LegendPlacement = LegendPlacement.Outside,
                PlotMargins = new OxyThickness(30, 4, 0, 30)
            };

            plot.Series.Add(CreateLineSeries(x => x, 0.1, 100, 0.1, "H_{2}O"));
            plot.Series.Add(CreateLineSeries(x => x * x, 0.1, 100, 0.1, "C_{6}H_{12}O_{6}"));
            plot.Series.Add(CreateLineSeries(x => x * x * x, 0.1, 100, 0.1, "A^{2}_{i,j}"));

            plot.Axes.Add(new LogarithmicAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0.1,
                Maximum = 100,
                UseSuperExponentialFormat = true,
                FontSize = 14,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });
            plot.Axes.Add(new LogarithmicAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0.1,
                Maximum = 100,
                FontSize = 14,
                UseSuperExponentialFormat = true,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });

            return plot;
        }

    }
}