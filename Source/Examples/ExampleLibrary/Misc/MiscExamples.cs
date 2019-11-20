// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MiscExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Appends the specified target.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    [Examples("Misc")]
    public static class MiscExamples
    {
        [Example("Numeric ODE solvers (y'=y)")]
        public static PlotModel NumericOdeSolvers1()
        {
            return NumericOdeSolvers("Numeric ODE solvers", "y'=y, y(0)=1", 0, 1, Math.Exp, (t, y) => y);
        }

        [Example("Numeric ODE solvers (y'=x)")]
        public static PlotModel NumericOdeSolvers2()
        {
            return NumericOdeSolvers("Numeric ODE solvers", "y'=x, y(0)=0", 0, 0, t => 0.5 * t * t, (t, y) => t);
        }

        [Example("Numeric ODE solvers (y'=cos(x))")]
        public static PlotModel NumericOdeSolvers3()
        {
            return NumericOdeSolvers("Numeric ODE solvers", "y'=cos(x), y(0)=0", 0, 0, Math.Sin, (t, y) => Math.Cos(t));
        }

        public static PlotModel NumericOdeSolvers(
            string title,
            string subtitle,
            double t0,
            double y0,
            Func<double, double> exact,
            Func<double, double, double> f)
        {
            var model = new PlotModel
            {
                Title = title,
                Subtitle = subtitle,
            };

            var l = new Legend
            {
                LegendPosition = LegendPosition.BottomCenter,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Horizontal
            };

            model.Legends.Add(l);
            model.Series.Add(new FunctionSeries(exact, 0, 4, 100) { Title = "Exact solution", StrokeThickness = 5 });
            var eulerSeries = new LineSeries
            {
                Title = "Euler, h=0.25",
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Black,
            };
            eulerSeries.Points.AddRange(Euler(f, t0, y0, 4, 0.25));
            model.Series.Add(eulerSeries);

            //model.Series.Add(new LineSeries("Euler, h=1")
            //    {
            //        MarkerType = MarkerType.Circle,
            //        MarkerFill = OxyColors.Black,
            //        Points = Euler(f, t0, y0, 4, 1)
            //    });
            var heunSeries = new LineSeries
            {
                Title = "Heun, h=0.25",
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Black,
            };
            heunSeries.Points.AddRange(Heun(f, t0, y0, 4, 0.25));
            model.Series.Add(heunSeries);

            var midpointSeries = new LineSeries
            {
                Title = "Midpoint, h=0.25",
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Black,
            };
            midpointSeries.Points.AddRange(Midpoint(f, t0, y0, 4, 0.25));
            model.Series.Add(midpointSeries);

            var rkSeries = new LineSeries
            {
                Title = "RK4, h=0.25",
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Black,
            };
            rkSeries.Points.AddRange(RungeKutta4(f, t0, y0, 4, 0.25));
            model.Series.Add(rkSeries);

            //model.Series.Add(new LineSeries("RK4, h=1")
            //{
            //    MarkerType = MarkerType.Circle,
            //    MarkerFill = OxyColors.Black,
            //    Points = RungeKutta4(f, t0, y0, 4, 1)
            //});

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        private static List<DataPoint> Euler(
            Func<double, double, double> f, double t0, double y0, double t1, double h)
        {
            var points = new List<DataPoint>();
            double y = y0;
            for (double t = t0; t < t1 + h / 2; t += h)
            {
                points.Add(new DataPoint(t, y));
                y += h * f(t, y);
            }

            return points;
        }

        private static IList<DataPoint> Heun(Func<double, double, double> f, double t0, double y0, double t1, double h)
        {
            var points = new List<DataPoint>();
            double y = y0;
            for (double t = t0; t < t1 + h / 2; t += h)
            {
                points.Add(new DataPoint(t, y));
                double ytilde = y + h * f(t, y);
                y = y + h / 2 * (f(t, y) + f(t + h, ytilde));
            }

            return points;
        }

        private static List<DataPoint> Midpoint(
            Func<double, double, double> f, double t0, double y0, double t1, double h)
        {
            var points = new List<DataPoint>();
            double y = y0;
            for (double t = t0; t < t1 + h / 2; t += h)
            {
                points.Add(new DataPoint(t, y));
                y += h * f(t + h / 2, y + h / 2 * f(t, y));
            }

            return points;
        }

        private static List<DataPoint> RungeKutta4(
            Func<double, double, double> f, double t0, double y0, double t1, double h)
        {
            var points = new List<DataPoint>();
            double y = y0;
            for (double t = t0; t < t1 + h / 2; t += h)
            {
                points.Add(new DataPoint(t, y));
                double k1 = h * f(t, y);
                double k2 = h * f(t + h / 2, y + k1 / 2);
                double k3 = h * f(t + h / 2, y + k2 / 2);
                double k4 = h * f(t + h, y + k3);
                y += (k1 + 2 * k2 + 2 * k3 + k4) / 6;
            }

            return points;
        }

        [Example("MatrixSeries (chemical process simulation problem)")]
        public static PlotModel MatrixSeriesWest0479()
        {
            // http://www.cise.ufl.edu/research/sparse/matrices/HB/west0479
            var model = new PlotModel();

            double[,] matrix = null;



            using (var reader = new StreamReader(typeof(MiscExamples).GetTypeInfo().Assembly.GetManifestResourceStream("ExampleLibrary.Resources.west0479.mtx")))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.StartsWith("%"))
                    {
                        continue;
                    }

                    var v = line.Split(' ');
                    if (matrix == null)
                    {
                        int m = int.Parse(v[0]);
                        int n = int.Parse(v[1]);
                        matrix = new double[m, n];
                        continue;
                    }

                    int i = int.Parse(v[0]) - 1;
                    int j = int.Parse(v[1]) - 1;
                    matrix[i, j] = double.Parse(v[2], CultureInfo.InvariantCulture);
                }
            }

            // Reverse the vertical axis
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StartPosition = 1, EndPosition = 0 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Series.Add(new MatrixSeries { Matrix = matrix, ShowDiagonal = true });

            return model;
        }

        [Example("Train schedule")]
        public static PlotModel TrainSchedule()
        {
            //// http://www.edwardtufte.com/tufte/books_vdqi
            //// http://marlenacompton.com/?p=103
            //// http://mbostock.github.com/protovis/ex/caltrain.html
            //// http://en.wikipedia.org/wiki/%C3%89tienne-Jules_Marey
            //// http://mbostock.github.com/protovis/ex/marey-train-schedule.jpg
            //// http://c82.net/posts.php?id=66

            var model = new PlotModel
            {
                Title = "Train schedule",
                Subtitle = "Bergensbanen (Oslo-Bergen, Norway)",
                IsLegendVisible = false,
                PlotAreaBorderThickness = new OxyThickness(0),
                PlotMargins = new OxyThickness(60, 4, 60, 40)
            };
            model.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Minimum = -20,
                    Maximum = 540,
                    Title = "Distance from Oslo S",
                    IsAxisVisible = true,
                    StringFormat = "0"
                });
            model.Axes.Add(
                new TimeSpanAxis
                {
                    Position = AxisPosition.Bottom,
                    Minimum = 0,
                    Maximum = TimeSpanAxis.ToDouble(TimeSpan.FromHours(24)),
                    StringFormat = "hh",
                    Title = "Time",
                    MajorStep = TimeSpanAxis.ToDouble(TimeSpan.FromHours(1)),
                    MinorStep = TimeSpanAxis.ToDouble(TimeSpan.FromMinutes(10)),
                    TickStyle = TickStyle.None,
                    MajorGridlineStyle = LineStyle.Solid,
                    MajorGridlineColor = OxyColors.LightGray,
                    MinorGridlineStyle = LineStyle.Solid,
                    MinorGridlineColor = OxyColor.FromArgb(255, 240, 240, 240)
                });

            // Read the train schedule from a .csv resource
            using (var reader = new StreamReader(GetResourceStream("Bergensbanen.csv")))
            {
                string header = reader.ReadLine();
                var headerFields = header.Split(';');
                int lines = headerFields.Length - 3;
                var stations = new LineSeries()
                {
                    StrokeThickness = 0,
                    MarkerType = MarkerType.Circle,
                    MarkerFill = OxyColor.FromAColor(200, OxyColors.Black),
                    MarkerSize = 4,
                };

                // Add the line series for each train line
                var series = new LineSeries[lines];
                for (int i = 0; i < series.Length; i++)
                {
                    series[i] = new LineSeries
                    {
                        Title = headerFields[3 + i],
                        Color =
                                            OxyColor.FromAColor(
                                                180, OxyColors.Black),
                        StrokeThickness = 2,
                        TrackerFormatString =
                                            "Train {0}\nTime: {2}\nDistance from Oslo S: {4:0.0} km",
                    };
                    model.Series.Add(series[i]);
                }

                // Parse the train schedule
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    // skip comments
                    if (line == null || line.StartsWith("//"))
                    {
                        continue;
                    }

                    var fields = line.Split(';');
                    double x = double.Parse(fields[1], CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(fields[0]))
                    {
                        // Add a horizontal annotation line for the station
                        model.Annotations.Add(
                            new LineAnnotation
                            {
                                Type = LineAnnotationType.Horizontal,
                                Y = x,
                                Layer = AnnotationLayer.BelowSeries,
                                LineStyle = LineStyle.Solid,
                                Color = OxyColors.LightGray,
                                Text = fields[0] + "  ",
                                TextVerticalAlignment = VerticalAlignment.Middle,
                                TextLinePosition = 1,
                                TextMargin = 0,
                                TextPadding = 4,
                                ClipText = false,
                                TextHorizontalAlignment = HorizontalAlignment.Left
                            });
                    }

                    for (int i = 0; i < series.Length; i++)
                    {
                        if (string.IsNullOrEmpty(fields[i + 3]))
                        {
                            continue;
                        }

                        // Convert time from hhmm to a time span
                        int hhmm = int.Parse(fields[i + 3]);
                        var span = new TimeSpan(0, hhmm / 100, (hhmm % 100), 0);
                        double t = TimeSpanAxis.ToDouble(span);

                        // Add the point to the line
                        series[i].Points.Add(new DataPoint(t, x));

                        // Add the point for the station
                        stations.Points.Add(new DataPoint(t, x));
                    }
                }

                // add points and NaN (to make a line break) when passing midnight
                double tmidnight = TimeSpanAxis.ToDouble(TimeSpan.FromHours(24));
                foreach (LineSeries s in model.Series)
                {
                    for (int i = 0; i + 1 < s.Points.Count; i++)
                    {
                        if (Math.Abs(s.Points[i].X - s.Points[i + 1].X) > tmidnight / 2)
                        {
                            double x0 = s.Points[i].X;
                            if (x0 > tmidnight / 2)
                            {
                                x0 -= tmidnight;
                            }

                            double x1 = s.Points[i + 1].X;
                            if (x1 > tmidnight / 2)
                            {
                                x1 -= tmidnight;
                            }

                            double y = s.Points[i].Y + (s.Points[i + 1].Y - s.Points[i].Y) / (x1 - x0) * (0 - x0);
                            s.Points.Insert(i + 1, new DataPoint(x0 < x1 ? 0 : tmidnight, y));
                            s.Points.Insert(i + 1, new DataPoint(double.NaN, y));
                            s.Points.Insert(i + 1, new DataPoint(x0 < x1 ? tmidnight : 0, y));
                            i += 3;
                        }
                    }
                }

                model.Series.Add(stations);
            }

            return model;
        }

        /*        [Example("World population")]
                public static PlotModel WorldPopulation()
                {
                    WorldPopulationDataSet dataSet;
                    using (var stream = GetResourceStream("WorldPopulation.xml"))
                    {
                        var serializer = new XmlSerializer(typeof(WorldPopulationDataSet));
                        dataSet = (WorldPopulationDataSet)serializer.Deserialize(stream);
                    }

                    var model = new PlotModel { Title = "World population" };
                    model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "millions" });
                    var series1 = new LineSeries { ItemsSource = dataSet.Items, DataFieldX = "Year", DataFieldY = "Population", StrokeThickness = 3, MarkerType = MarkerType.Circle };
                    model.Series.Add(series1);
                    return model;
                }*/

        [Example("La Linea (AreaSeries)")]
        public static PlotModel LaLineaAreaSeries()
        {
            // http://en.wikipedia.org/wiki/La_Linea_(TV_series)
            var model = new PlotModel
            {
                Title = "La Linea",
                PlotType = PlotType.Cartesian,
                Background = OxyColor.FromRgb(84, 98, 207)
            };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -500, Maximum = 1000 });
            var series1 = new AreaSeries { Fill = OxyColors.White, StrokeThickness = 0 };
            series1.Points.Append(GetLineaPoints());
            model.Series.Add(series1);
            return model;
        }

        [Example("La Linea (LineSeries)")]
        public static PlotModel LaLinea()
        {
            // http://en.wikipedia.org/wiki/La_Linea_(TV_series)
            var model = new PlotModel
            {
                Title = "La Linea",
                PlotType = PlotType.Cartesian,
                Background = OxyColor.FromRgb(84, 98, 207)
            };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -500, Maximum = 1000 });
            var series1 = new LineSeries { Color = OxyColors.White, StrokeThickness = 1.5 };
            series1.Points.Append(GetLineaPoints());
            model.Series.Add(series1);
            return model;
        }

        /// <summary>
        /// Appends the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void Append<T>(this IList<T> target, IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                target.Add(item);
            }
        }

        private static IEnumerable<DataPoint> GetLineaPoints()
        {
            var points = new List<DataPoint>();

            // The original image was vectorized by http://www.autotracer.org/
            // Then inkscape was used to convert from svg to xaml http://inkscape.org/
            // The xaml geometry was imported by Geometry.Parse and converted to a polyline
            // by Geometry.GetFlattenedPathGeometry();
            // The resulting points were output to the following code:
            points.Add(new DataPoint(589.3649979, 16.10595703));
            points.Add(new DataPoint(437.9979935, 16.10595703));
            points.Add(new DataPoint(400.4249954, 16.10595703));
            points.Add(new DataPoint(399.1255264, 16.05047607));
            points.Add(new DataPoint(397.463356, 15.92333984));
            points.Add(new DataPoint(393.5852432, 15.69073486));
            points.Add(new DataPoint(389.8589859, 15.88067627));
            points.Add(new DataPoint(388.3866653, 16.28186035));
            points.Add(new DataPoint(387.3529739, 16.96594238));
            points.Add(new DataPoint(386.8373489, 18.53930664));
            points.Add(new DataPoint(387.2163773, 20.51794434));
            points.Add(new DataPoint(387.9814529, 22.51843262));
            points.Add(new DataPoint(388.6240005, 24.15698242));
            points.Add(new DataPoint(395.958992, 45.09094238));
            points.Add(new DataPoint(399.2686234, 54.89562988));
            points.Add(new DataPoint(402.1330338, 64.90338135));
            points.Add(new DataPoint(404.5822525, 75.06884766));
            points.Add(new DataPoint(406.6462479, 85.34680176));
            points.Add(new DataPoint(409.7385635, 106.0593262));
            points.Add(new DataPoint(411.6500015, 126.6789551));
            points.Add(new DataPoint(412.0961685, 137.0930786));
            points.Add(new DataPoint(412.0253677, 147.4713135));
            points.Add(new DataPoint(411.4655228, 157.8103638));
            points.Add(new DataPoint(410.4446182, 168.1069336));
            points.Add(new DataPoint(408.9906387, 178.3577881));
            points.Add(new DataPoint(407.1315689, 188.5595703));
            points.Add(new DataPoint(404.8953629, 198.7091064));
            points.Add(new DataPoint(402.3099747, 208.8029785));
            points.Add(new DataPoint(392.9860001, 237.2509766));
            points.Add(new DataPoint(392.1175613, 240.0527954));
            points.Add(new DataPoint(391.2060013, 243.0959473));
            points.Add(new DataPoint(390.5509415, 246.1691284));
            points.Add(new DataPoint(390.4520035, 249.0609741));
            points.Add(new DataPoint(391.406044, 252.6694336));
            points.Add(new DataPoint(393.1980057, 256.0982056));
            points.Add(new DataPoint(395.3566971, 259.3631287));
            points.Add(new DataPoint(397.4109879, 262.47995));
            points.Add(new DataPoint(411.7649918, 287.7079468));
            points.Add(new DataPoint(426.5997696, 312.9102173));
            points.Add(new DataPoint(441.8913651, 337.8200684));
            points.Add(new DataPoint(457.3402786, 362.6333923));
            points.Add(new DataPoint(472.6469803, 387.5459595));
            points.Add(new DataPoint(478.0007401, 395.7557983));
            points.Add(new DataPoint(483.6958694, 403.8400879));
            points.Add(new DataPoint(489.2894974, 411.9628296));
            points.Add(new DataPoint(494.3389969, 420.2879639));
            points.Add(new DataPoint(494.9800491, 421.8480225));
            points.Add(new DataPoint(495.4455032, 423.6903687));
            points.Add(new DataPoint(496.1577225, 427.7724609));
            points.Add(new DataPoint(497.0915604, 431.6355591));
            points.Add(new DataPoint(497.8341141, 433.2041321));
            points.Add(new DataPoint(498.8629837, 434.3809509));
            points.Add(new DataPoint(500.0935135, 434.9877625));
            points.Add(new DataPoint(501.7391434, 435.3059082));
            points.Add(new DataPoint(505.7623367, 435.3148193));
            points.Add(new DataPoint(509.9061356, 434.8848267));
            points.Add(new DataPoint(511.7024612, 434.6542969));
            points.Add(new DataPoint(513.1439896, 434.4929504));
            points.Add(new DataPoint(520.0768509, 434.1251831));
            points.Add(new DataPoint(527.3961258, 434.1952209));
            points.Add(new DataPoint(534.6892776, 434.728363));
            points.Add(new DataPoint(541.544014, 435.7499695));
            points.Add(new DataPoint(544.2357864, 436.3025513));
            points.Add(new DataPoint(547.021492, 437.0792236));
            points.Add(new DataPoint(549.6099319, 438.2590027));
            points.Add(new DataPoint(551.7099686, 440.0209656));
            points.Add(new DataPoint(552.7028275, 441.4446106));
            points.Add(new DataPoint(553.2691116, 442.791626));
            points.Add(new DataPoint(553.4498978, 444.0619202));
            points.Add(new DataPoint(553.2864456, 445.2554626));
            points.Add(new DataPoint(552.0910721, 447.4119873));
            points.Add(new DataPoint(550.0122147, 449.2607117));
            points.Add(new DataPoint(547.3790359, 450.8010864));
            points.Add(new DataPoint(544.5206985, 452.0325928));
            points.Add(new DataPoint(541.766304, 452.9547119));
            points.Add(new DataPoint(539.445015, 453.5669556));
            points.Add(new DataPoint(539.445015, 454.6409607));
            points.Add(new DataPoint(542.6554031, 455.4246521));
            points.Add(new DataPoint(546.0063553, 455.8735962));
            points.Add(new DataPoint(549.2799149, 456.4869385));
            points.Add(new DataPoint(552.2580032, 457.7639465));
            points.Add(new DataPoint(554.3335648, 459.5966797));
            points.Add(new DataPoint(555.6600418, 461.8208313));
            points.Add(new DataPoint(556.278389, 464.282959));
            points.Add(new DataPoint(556.2294998, 466.8295898));
            points.Add(new DataPoint(555.55439, 469.307251));
            points.Add(new DataPoint(554.2939529, 471.5625));
            points.Add(new DataPoint(552.4892044, 473.4418945));
            points.Add(new DataPoint(550.1809769, 474.7919617));
            points.Add(new DataPoint(547.1414261, 475.8059387));
            points.Add(new DataPoint(543.8482132, 476.5288391));
            points.Add(new DataPoint(537.2979813, 477.0559692));
            points.Add(new DataPoint(535.5239944, 476.8666077));
            points.Add(new DataPoint(533.5114822, 476.5535889));
            points.Add(new DataPoint(531.5334549, 476.4162598));
            points.Add(new DataPoint(529.8629837, 476.7539673));
            points.Add(new DataPoint(529.0471268, 477.3421631));
            points.Add(new DataPoint(528.5394363, 478.1289673));
            points.Add(new DataPoint(528.1448441, 480.0927124));
            points.Add(new DataPoint(528.071846, 482.2338257));
            points.Add(new DataPoint(527.7129593, 484.1409607));
            points.Add(new DataPoint(526.901741, 485.4877014));
            points.Add(new DataPoint(525.8139114, 486.4950867));
            points.Add(new DataPoint(523.0528641, 487.6643372));
            points.Add(new DataPoint(519.9188919, 487.9933777));
            points.Add(new DataPoint(516.9010086, 487.8269653));
            points.Add(new DataPoint(511.7325516, 486.9451599));
            points.Add(new DataPoint(506.4563065, 485.4539185));
            points.Add(new DataPoint(501.155159, 483.4500427));
            points.Add(new DataPoint(495.912117, 481.0302124));
            points.Add(new DataPoint(485.9321365, 475.3295898));
            points.Add(new DataPoint(481.3610916, 472.2423096));
            points.Add(new DataPoint(477.1800003, 469.125946));
            points.Add(new DataPoint(465.3709793, 459.3179626));
            points.Add(new DataPoint(464.3509598, 458.3116455));
            points.Add(new DataPoint(463.1867142, 457.1624451));
            points.Add(new DataPoint(461.9141312, 456.2180176));
            points.Add(new DataPoint(460.5689774, 455.8259583));
            points.Add(new DataPoint(459.6923904, 456.0762939));
            points.Add(new DataPoint(458.8656693, 456.7503662));
            points.Add(new DataPoint(457.3631058, 458.907959));
            points.Add(new DataPoint(456.063179, 461.3753052));
            points.Add(new DataPoint(455.4898758, 462.436554));
            points.Add(new DataPoint(454.9679642, 463.2289734));
            points.Add(new DataPoint(453.0795364, 465.3183289));
            points.Add(new DataPoint(450.8528519, 467.2734985));
            points.Add(new DataPoint(448.3575516, 468.9848328));
            points.Add(new DataPoint(445.6630936, 470.3425903));
            points.Add(new DataPoint(442.83918, 471.2370605));
            points.Add(new DataPoint(439.9552689, 471.5585938));
            points.Add(new DataPoint(437.0810318, 471.1974487));
            points.Add(new DataPoint(434.2859879, 470.0439453));
            points.Add(new DataPoint(432.4744034, 468.6621399));
            points.Add(new DataPoint(431.3244705, 467.0726013));
            points.Add(new DataPoint(430.7551956, 465.3302612));
            points.Add(new DataPoint(430.6856155, 463.4900818));
            points.Add(new DataPoint(431.0347672, 461.6070251));
            points.Add(new DataPoint(431.7216873, 459.7360229));
            points.Add(new DataPoint(433.7849808, 456.2499695));
            points.Add(new DataPoint(438.1093216, 450.118988));
            points.Add(new DataPoint(441.4749832, 444.3893433));
            points.Add(new DataPoint(444.0351639, 438.28302));
            points.Add(new DataPoint(445.0610428, 434.845459));
            points.Add(new DataPoint(445.9430008, 431.0219727));
            points.Add(new DataPoint(446.6270218, 428.7687378));
            points.Add(new DataPoint(447.4476395, 426.4767151));
            points.Add(new DataPoint(447.9032059, 424.1760559));
            points.Add(new DataPoint(447.492012, 421.8969727));
            points.Add(new DataPoint(445.6156082, 418.2295837));
            points.Add(new DataPoint(443.3608475, 414.6139832));
            points.Add(new DataPoint(438.1008682, 407.5364685));
            points.Add(new DataPoint(432.48069, 400.6614685));
            points.Add(new DataPoint(427.2689896, 393.9859619));
            points.Add(new DataPoint(389.1699905, 339.2359619));
            points.Add(new DataPoint(374.5550003, 318.3009644));
            points.Add(new DataPoint(372.5515823, 314.8404541));
            points.Add(new DataPoint(370.2787552, 310.9485779));
            points.Add(new DataPoint(367.7467728, 307.2946777));
            points.Add(new DataPoint(366.3868484, 305.7660828));
            points.Add(new DataPoint(364.9659805, 304.5479736));
            points.Add(new DataPoint(363.9477615, 304.0406799));
            points.Add(new DataPoint(363.082222, 304.0159912));
            points.Add(new DataPoint(361.6236038, 304.9024658));
            points.Add(new DataPoint(360.2191849, 306.1834412));
            points.Add(new DataPoint(359.4213638, 306.651886));
            points.Add(new DataPoint(358.4979935, 306.8349609));
            points.Add(new DataPoint(356.6694107, 306.4464722));
            points.Add(new DataPoint(354.9371109, 305.5308228));
            points.Add(new DataPoint(353.2544937, 304.4515076));
            points.Add(new DataPoint(351.5749893, 303.5719604));
            points.Add(new DataPoint(343.4895706, 301.1234131));
            points.Add(new DataPoint(335.0169449, 299.8048401));
            points.Add(new DataPoint(326.3076553, 299.5128174));
            points.Add(new DataPoint(317.5122147, 300.1439514));
            points.Add(new DataPoint(308.7811966, 301.5948486));
            points.Add(new DataPoint(300.2651443, 303.762085));
            points.Add(new DataPoint(292.1145401, 306.5422668));
            points.Add(new DataPoint(284.4799881, 309.8319702));
            points.Add(new DataPoint(282.3371964, 310.7483521));
            points.Add(new DataPoint(279.925972, 311.644928));
            points.Add(new DataPoint(274.7942581, 313.5763245));
            points.Add(new DataPoint(270.0769119, 316.0212708));
            points.Add(new DataPoint(268.1836319, 317.559845));
            points.Add(new DataPoint(266.7659988, 319.3749695));
            points.Add(new DataPoint(271.6227798, 320.1968384));
            points.Add(new DataPoint(276.5877457, 321.7830811));
            points.Add(new DataPoint(281.472847, 323.7190247));
            points.Add(new DataPoint(286.090004, 325.5899658));
            points.Add(new DataPoint(298.3649979, 330.7419739));
            points.Add(new DataPoint(310.3880997, 336.8226929));
            points.Add(new DataPoint(321.8024063, 343.941803));
            points.Add(new DataPoint(332.2509842, 352.2089539));
            points.Add(new DataPoint(339.2033768, 358.533844));
            points.Add(new DataPoint(342.4841385, 361.901825));
            points.Add(new DataPoint(345.4439774, 365.5359497));
            points.Add(new DataPoint(346.445076, 367.0002136));
            points.Add(new DataPoint(347.3386307, 368.7592163));
            points.Add(new DataPoint(347.6443558, 370.5680847));
            points.Add(new DataPoint(347.4267044, 371.4147034));
            points.Add(new DataPoint(346.8819962, 372.1819458));
            points.Add(new DataPoint(345.387001, 372.7861023));
            points.Add(new DataPoint(343.5981216, 372.4627075));
            points.Add(new DataPoint(341.8064041, 371.7001953));
            points.Add(new DataPoint(340.3029861, 370.986969));
            points.Add(new DataPoint(336.1688919, 369.4472046));
            points.Add(new DataPoint(331.5998611, 368.1002197));
            points.Add(new DataPoint(326.9541702, 367.1023254));
            points.Add(new DataPoint(322.5899734, 366.6099548));
            points.Add(new DataPoint(324.6785049, 369.6851501));
            points.Add(new DataPoint(327.4601212, 372.3129578));
            points.Add(new DataPoint(330.467659, 374.7735291));
            points.Add(new DataPoint(333.2339859, 377.3469543));
            points.Add(new DataPoint(338.3369217, 383.6273193));
            points.Add(new DataPoint(342.70298, 390.7063293));
            points.Add(new DataPoint(344.4534683, 394.4726563));
            points.Add(new DataPoint(345.8323135, 398.3514099));
            points.Add(new DataPoint(346.7769547, 402.3135376));
            points.Add(new DataPoint(347.2249832, 406.3299561));
            points.Add(new DataPoint(346.8078384, 412.0097046));
            points.Add(new DataPoint(345.1297989, 416.5983276));
            points.Add(new DataPoint(342.383522, 420.1838074));
            points.Add(new DataPoint(338.7616043, 422.8540955));
            points.Add(new DataPoint(334.4566727, 424.6971741));
            points.Add(new DataPoint(329.6613541, 425.8010559));
            points.Add(new DataPoint(324.5682449, 426.2536621));
            points.Add(new DataPoint(319.3700027, 426.1429749));
            points.Add(new DataPoint(315.144783, 425.6447144));
            points.Add(new DataPoint(311.0141983, 424.7691345));
            points.Add(new DataPoint(303.0388565, 422.0056763));
            points.Add(new DataPoint(295.4478226, 418.0917969));
            points.Add(new DataPoint(288.2450027, 413.2668457));
            points.Add(new DataPoint(281.4342422, 407.769989));
            points.Add(new DataPoint(275.0194168, 401.8405762));
            points.Add(new DataPoint(269.0043716, 395.717804));
            points.Add(new DataPoint(263.393013, 389.6409607));
            points.Add(new DataPoint(255.0782547, 379.5436707));
            points.Add(new DataPoint(247.6409988, 368.3999634));
            points.Add(new DataPoint(244.4098587, 362.5210571));
            points.Add(new DataPoint(241.5882645, 356.4829712));
            points.Add(new DataPoint(239.2395096, 350.3198853));
            points.Add(new DataPoint(237.4270096, 344.0659485));
            points.Add(new DataPoint(236.2694168, 338.2407532));
            points.Add(new DataPoint(235.5486526, 332.3677368));
            points.Add(new DataPoint(235.0773697, 320.528717));
            points.Add(new DataPoint(235.3326797, 308.6495667));
            points.Add(new DataPoint(235.6340103, 296.8309631));
            points.Add(new DataPoint(233.1889725, 297.9562988));
            points.Add(new DataPoint(230.9140091, 299.4570923));
            points.Add(new DataPoint(226.5090103, 302.6929626));
            points.Add(new DataPoint(206.6489944, 315.875946));
            points.Add(new DataPoint(157.2670059, 346.7819519));
            points.Add(new DataPoint(136.8219986, 360.059967));
            points.Add(new DataPoint(132.2092514, 363.0340881));
            points.Add(new DataPoint(130.0033798, 364.6920166));
            points.Add(new DataPoint(128.1559982, 366.6599731));
            points.Add(new DataPoint(127.0190811, 368.5368958));
            points.Add(new DataPoint(126.298027, 370.4631348));
            points.Add(new DataPoint(125.9532547, 372.4280701));
            points.Add(new DataPoint(125.9451218, 374.4211121));
            points.Add(new DataPoint(126.7804031, 378.4490356));
            points.Add(new DataPoint(128.4870071, 382.4620972));
            points.Add(new DataPoint(130.748024, 386.3754272));
            points.Add(new DataPoint(133.2466202, 390.104248));
            points.Add(new DataPoint(135.6659012, 393.5636902));
            points.Add(new DataPoint(137.689003, 396.6689453));
            points.Add(new DataPoint(139.2043839, 400.197052));
            points.Add(new DataPoint(139.5524673, 402.0329285));
            points.Add(new DataPoint(139.5626297, 403.8374634));
            points.Add(new DataPoint(139.1861954, 405.551239));
            points.Add(new DataPoint(138.3745499, 407.1148682));
            points.Add(new DataPoint(137.0790329, 408.4689026));
            points.Add(new DataPoint(135.2509995, 409.5539551));
            points.Add(new DataPoint(132.8812943, 410.31427));
            points.Add(new DataPoint(130.5507584, 410.5262146));
            points.Add(new DataPoint(128.270546, 410.2424316));
            points.Add(new DataPoint(126.0518265, 409.5155334));
            points.Add(new DataPoint(123.9057388, 408.3982239));
            points.Add(new DataPoint(121.8434372, 406.9431458));
            points.Add(new DataPoint(118.0148697, 403.2301941));
            points.Add(new DataPoint(114.6553879, 398.7979126));
            points.Add(new DataPoint(111.8542252, 394.0675049));
            points.Add(new DataPoint(109.7006912, 389.4601135));
            points.Add(new DataPoint(108.2840042, 385.3969727));
            points.Add(new DataPoint(107.7778549, 382.5092468));
            points.Add(new DataPoint(107.3788681, 379.2887268));
            points.Add(new DataPoint(106.6309433, 376.2470703));
            points.Add(new DataPoint(105.0779953, 373.8959656));
            points.Add(new DataPoint(103.1701126, 372.6677246));
            points.Add(new DataPoint(100.7825394, 371.7599487));
            points.Add(new DataPoint(95.13137054, 370.6252136));
            points.Add(new DataPoint(89.25051117, 369.9303284));
            points.Add(new DataPoint(86.57584381, 369.5724182));
            points.Add(new DataPoint(84.26599884, 369.1139526));
            points.Add(new DataPoint(78.20024872, 367.3027649));
            points.Add(new DataPoint(71.70685577, 364.820343));
            points.Add(new DataPoint(65.1133194, 361.6892395));
            points.Add(new DataPoint(58.74712372, 357.9318237));
            points.Add(new DataPoint(52.93579102, 353.5706482));
            points.Add(new DataPoint(48.00682831, 348.6281433));
            points.Add(new DataPoint(45.97557068, 345.9458923));
            points.Add(new DataPoint(44.28772736, 343.1267395));
            points.Add(new DataPoint(42.98422241, 340.1734924));
            points.Add(new DataPoint(42.10599518, 337.0889587));
            points.Add(new DataPoint(41.90753937, 335.2592163));
            points.Add(new DataPoint(42.08698273, 333.7605286));
            points.Add(new DataPoint(43.18836975, 331.3730774));
            points.Add(new DataPoint(44.62782288, 329.1598206));
            points.Add(new DataPoint(45.6230011, 326.3539734));
            points.Add(new DataPoint(45.62973022, 324.9945984));
            points.Add(new DataPoint(45.33054352, 323.6192627));
            points.Add(new DataPoint(44.36862183, 320.8330994));
            points.Add(new DataPoint(43.98298645, 319.4284058));
            points.Add(new DataPoint(43.84563446, 318.0201111));
            points.Add(new DataPoint(44.09512329, 316.6112671));
            points.Add(new DataPoint(44.86999512, 315.2049561));
            points.Add(new DataPoint(45.80908966, 314.2088623));
            points.Add(new DataPoint(46.79941559, 313.5540161));
            points.Add(new DataPoint(48.9025116, 313.1113892));
            points.Add(new DataPoint(51.11682129, 313.5637512));
            points.Add(new DataPoint(53.37987518, 314.5978394));
            points.Add(new DataPoint(57.8022995, 317.1580811));
            points.Add(new DataPoint(59.83672333, 318.0577087));
            points.Add(new DataPoint(61.6700058, 318.2859497));
            points.Add(new DataPoint(62.82819366, 317.6254883));
            points.Add(new DataPoint(63.23600006, 316.3283386));
            points.Add(new DataPoint(63.24330902, 314.8067627));
            points.Add(new DataPoint(63.20000458, 313.4729614));
            points.Add(new DataPoint(63.68109894, 310.3050232));
            points.Add(new DataPoint(64.93375397, 307.6513367));
            points.Add(new DataPoint(67.09728241, 305.979187));
            points.Add(new DataPoint(68.56415558, 305.6572571));
            points.Add(new DataPoint(70.31099701, 305.7559509));
            points.Add(new DataPoint(73.2078476, 306.4935913));
            points.Add(new DataPoint(76.04866791, 307.6478271));
            points.Add(new DataPoint(81.46486664, 310.9243164));
            points.Add(new DataPoint(86.36489105, 315.0218811));
            points.Add(new DataPoint(90.55399323, 319.3769531));
            points.Add(new DataPoint(92.14154816, 321.4163208));
            points.Add(new DataPoint(93.72263336, 323.6399536));
            points.Add(new DataPoint(95.39614105, 325.7593689));
            points.Add(new DataPoint(97.26099396, 327.4859619));
            points.Add(new DataPoint(98.87421417, 328.2453918));
            points.Add(new DataPoint(100.5960007, 328.4575806));
            points.Add(new DataPoint(104.1259995, 328.1189575));
            points.Add(new DataPoint(107.9671097, 328.0540771));
            points.Add(new DataPoint(112.0256271, 328.345459));
            points.Add(new DataPoint(116.0258255, 328.4305725));
            points.Add(new DataPoint(119.6919937, 327.7469482));
            points.Add(new DataPoint(122.6980515, 326.2321777));
            points.Add(new DataPoint(125.5723801, 324.1764526));
            points.Add(new DataPoint(128.3242722, 321.9129944));
            points.Add(new DataPoint(130.9630051, 319.7749634));
            points.Add(new DataPoint(158.6139908, 297.5969543));
            points.Add(new DataPoint(183.9269943, 278.8919678));
            points.Add(new DataPoint(215.7729874, 251.3609619));
            points.Add(new DataPoint(222.6591263, 245.7957153));
            points.Add(new DataPoint(229.6908646, 240.2703247));
            points.Add(new DataPoint(236.5836563, 234.5932617));
            points.Add(new DataPoint(243.0529861, 228.572937));
            points.Add(new DataPoint(246.137764, 224.8230591));
            points.Add(new DataPoint(248.7661209, 220.4605103));
            points.Add(new DataPoint(251.0138321, 215.6484985));
            points.Add(new DataPoint(252.9567337, 210.5499878));
            points.Add(new DataPoint(256.2312393, 200.1459351));
            points.Add(new DataPoint(257.7144547, 195.1665039));
            points.Add(new DataPoint(259.1959915, 190.5529785));
            points.Add(new DataPoint(263.9845047, 175.0708008));
            points.Add(new DataPoint(267.8167191, 159.2056274));
            points.Add(new DataPoint(270.6394424, 143.0612183));
            points.Add(new DataPoint(272.3993607, 126.7412109));
            points.Add(new DataPoint(273.043251, 110.3493042));
            points.Add(new DataPoint(272.5178299, 93.98913574));
            points.Add(new DataPoint(270.7698441, 77.76446533));
            points.Add(new DataPoint(267.7460098, 61.77893066));
            points.Add(new DataPoint(260.9010086, 28.45196533));
            points.Add(new DataPoint(260.3377457, 25.5088501));
            points.Add(new DataPoint(259.7099991, 22.24182129));
            points.Add(new DataPoint(258.5879898, 19.265625));
            points.Add(new DataPoint(257.7073746, 18.07867432));
            points.Add(new DataPoint(256.5419998, 17.19494629));
            points.Add(new DataPoint(254.5226212, 16.44396973));
            points.Add(new DataPoint(252.1078568, 16.0401001));
            points.Add(new DataPoint(246.5816116, 15.96911621));
            points.Add(new DataPoint(240.9423294, 16.37298584));
            points.Add(new DataPoint(238.3862076, 16.56274414));
            points.Add(new DataPoint(236.1689835, 16.64294434));
            points.Add(new DataPoint(185.1770096, 17.17895508));
            points.Add(new DataPoint(0, 16.64294434));
            points.Add(new DataPoint(0, 0.53894043));
            points.Add(new DataPoint(188.9400101, 0.53894043));
            points.Add(new DataPoint(242.0799942, 0.53894043));
            points.Add(new DataPoint(244.6571732, 0.474975586));
            points.Add(new DataPoint(247.5546951, 0.33001709));
            points.Add(new DataPoint(253.8458633, 0.078552246));
            points.Add(new DataPoint(260.0235977, 0.347839355));
            points.Add(new DataPoint(262.779335, 0.853759766));
            points.Add(new DataPoint(265.1579971, 1.70098877));
            points.Add(new DataPoint(266.7366104, 2.746398926));
            points.Add(new DataPoint(268.0821915, 4.172119141));
            points.Add(new DataPoint(270.1900101, 7.797119141));
            points.Add(new DataPoint(271.7128067, 11.84075928));
            points.Add(new DataPoint(272.8819962, 15.56799316));
            points.Add(new DataPoint(276.034523, 25.44775391));
            points.Add(new DataPoint(279.0441055, 35.50836182));
            points.Add(new DataPoint(281.6616287, 45.66326904));
            points.Add(new DataPoint(283.6380081, 55.82598877));
            points.Add(new DataPoint(285.7761917, 72.32995605));
            points.Add(new DataPoint(287.0616837, 88.9072876));
            points.Add(new DataPoint(287.4597855, 105.5118408));
            points.Add(new DataPoint(286.9358597, 122.0977173));
            points.Add(new DataPoint(285.4552994, 138.6187744));
            points.Add(new DataPoint(282.9833755, 155.0289917));
            points.Add(new DataPoint(279.4855118, 171.2824097));
            points.Add(new DataPoint(274.9270096, 187.3329468));
            points.Add(new DataPoint(271.4097672, 198.2474976));
            points.Add(new DataPoint(267.3623734, 209.4165039));
            points.Add(new DataPoint(262.4050369, 220.1444092));
            points.Add(new DataPoint(259.4664688, 225.1257324));
            points.Add(new DataPoint(256.1579971, 229.7359619));
            points.Add(new DataPoint(249.3205032, 237.2365723));
            points.Add(new DataPoint(241.720253, 244.1188354));
            points.Add(new DataPoint(233.794136, 250.6709595));
            points.Add(new DataPoint(225.9790115, 257.1809692));
            points.Add(new DataPoint(195.5280228, 282.9415894));
            points.Add(new DataPoint(164.2490005, 307.6559448));
            points.Add(new DataPoint(141.1689987, 324.8109741));
            points.Add(new DataPoint(133.2555008, 330.7440796));
            points.Add(new DataPoint(129.0609055, 333.3323975));
            points.Add(new DataPoint(124.5289993, 335.2859497));
            points.Add(new DataPoint(122.1544113, 335.8482361));
            points.Add(new DataPoint(119.7157364, 336.0837402));
            points.Add(new DataPoint(114.7126236, 335.969696));
            points.Add(new DataPoint(109.6527023, 335.7345581));
            points.Add(new DataPoint(104.6689987, 336.1689453));
            points.Add(new DataPoint(102.779686, 336.7739563));
            points.Add(new DataPoint(100.6949997, 337.5380859));
            points.Add(new DataPoint(98.61257172, 337.9761353));
            points.Add(new DataPoint(96.73000336, 337.6029663));
            points.Add(new DataPoint(94.59128571, 335.8127136));
            points.Add(new DataPoint(92.66425323, 333.2740784));
            points.Add(new DataPoint(90.92359161, 330.5338745));
            points.Add(new DataPoint(89.34400177, 328.1389465));
            points.Add(new DataPoint(87.08150482, 325.4346313));
            points.Add(new DataPoint(84.48600006, 322.8813171));
            points.Add(new DataPoint(78.90499115, 318.3849487));
            points.Add(new DataPoint(77.3181076, 317.091217));
            points.Add(new DataPoint(75.22336578, 315.5139465));
            points.Add(new DataPoint(73.02420807, 314.5059509));
            points.Add(new DataPoint(72.01151276, 314.4819031));
            points.Add(new DataPoint(71.12400055, 314.9199524));
            points.Add(new DataPoint(70.59803009, 315.7275085));
            points.Add(new DataPoint(70.51831818, 316.7388916));
            points.Add(new DataPoint(71.29537201, 319.0906982));
            points.Add(new DataPoint(72.65048981, 321.4106445));
            points.Add(new DataPoint(73.77899933, 323.1339722));
            points.Add(new DataPoint(77.82125092, 329.0054626));
            points.Add(new DataPoint(80.16477203, 331.7309265));
            points.Add(new DataPoint(82.71099091, 334.177948));
            points.Add(new DataPoint(84.92420197, 335.9799805));
            points.Add(new DataPoint(87.10187531, 337.9599609));
            points.Add(new DataPoint(88.68236542, 340.2526855));
            points.Add(new DataPoint(89.07314301, 341.5584412));
            points.Add(new DataPoint(89.10399628, 342.9929504));
            points.Add(new DataPoint(89.61742401, 343.8485413));
            points.Add(new DataPoint(89.34513092, 344.4684448));
            points.Add(new DataPoint(88.5472641, 344.7538757));
            points.Add(new DataPoint(87.48400116, 344.605957));
            points.Add(new DataPoint(85.91378021, 343.8370972));
            points.Add(new DataPoint(84.39550018, 342.7818298));
            points.Add(new DataPoint(81.58899689, 340.4889526));
            points.Add(new DataPoint(78.80124664, 338.1679382));
            points.Add(new DataPoint(75.64672089, 335.4441223));
            points.Add(new DataPoint(68.57024384, 329.6803589));
            points.Add(new DataPoint(64.81476593, 327.0864868));
            points.Add(new DataPoint(61.02541351, 324.9821167));
            points.Add(new DataPoint(57.28540802, 323.5902405));
            points.Add(new DataPoint(53.6780014, 323.1339722));
            points.Add(new DataPoint(55.6113739, 326.1587524));
            points.Add(new DataPoint(58.03131866, 328.9006653));
            points.Add(new DataPoint(63.8632431, 333.7370911));
            points.Add(new DataPoint(70.23856354, 338.0457153));
            points.Add(new DataPoint(76.22199249, 342.2289734));
            points.Add(new DataPoint(77.03455353, 342.7995605));
            points.Add(new DataPoint(78.11525726, 343.5061951));
            points.Add(new DataPoint(80.57563019, 345.232605));
            points.Add(new DataPoint(82.59217072, 347.217926));
            points.Add(new DataPoint(83.11811066, 348.24823));
            points.Add(new DataPoint(83.15399933, 349.2719727));
            points.Add(new DataPoint(82.68766022, 350.0397949));
            points.Add(new DataPoint(81.87210846, 350.3486328));
            points.Add(new DataPoint(79.6248703, 349.9825745));
            points.Add(new DataPoint(77.27544403, 348.9604797));
            points.Add(new DataPoint(76.33216095, 348.4492188));
            points.Add(new DataPoint(75.68700409, 348.0689697));
            points.Add(new DataPoint(66.5876236, 342.1790771));
            points.Add(new DataPoint(61.90306854, 339.4937744));
            points.Add(new DataPoint(56.897995, 337.35495));
            points.Add(new DataPoint(55.17505646, 336.726532));
            points.Add(new DataPoint(53.01000214, 336.1444702));
            points.Add(new DataPoint(51.04743958, 336.2538757));
            points.Add(new DataPoint(50.34353638, 336.7695007));
            points.Add(new DataPoint(49.93199921, 337.6999512));
            points.Add(new DataPoint(50.0814743, 339.3566589));
            points.Add(new DataPoint(51.06142426, 341.0038757));
            points.Add(new DataPoint(52.65550232, 342.6039124));
            points.Add(new DataPoint(54.64737701, 344.1190796));
            points.Add(new DataPoint(58.95914459, 346.743988));
            points.Add(new DataPoint(60.84635162, 347.7783203));
            points.Add(new DataPoint(62.26599884, 348.5769653));
            points.Add(new DataPoint(70.1079483, 352.9052734));
            points.Add(new DataPoint(78.1873703, 356.6586914));
            points.Add(new DataPoint(86.4916153, 359.8837585));
            points.Add(new DataPoint(95.00800323, 362.6269531));
            points.Add(new DataPoint(96.84983063, 363.0866394));
            points.Add(new DataPoint(98.97579193, 363.5275574));
            points.Add(new DataPoint(103.6091232, 364.5426941));
            points.Add(new DataPoint(107.965889, 366.0517273));
            points.Add(new DataPoint(109.7461472, 367.1099854));
            points.Add(new DataPoint(111.1039963, 368.43396));
            points.Add(new DataPoint(112.0068741, 370.5414429));
            points.Add(new DataPoint(112.4139938, 373.21521));
            points.Add(new DataPoint(112.6441269, 375.9950867));
            points.Add(new DataPoint(113.0159988, 378.4209595));
            points.Add(new DataPoint(113.902565, 381.2496643));
            points.Add(new DataPoint(115.2284775, 384.6081543));
            points.Add(new DataPoint(116.9393082, 388.2300415));
            points.Add(new DataPoint(118.980629, 391.8488464));
            points.Add(new DataPoint(121.2979813, 395.1981506));
            points.Add(new DataPoint(123.8369522, 398.0115662));
            points.Add(new DataPoint(126.5430984, 400.022644));
            points.Add(new DataPoint(129.3619919, 400.9649658));
            points.Add(new DataPoint(128.5566483, 397.5397949));
            points.Add(new DataPoint(126.8852463, 394.5133362));
            points.Add(new DataPoint(124.8594742, 391.6261902));
            points.Add(new DataPoint(122.9910049, 388.6189575));
            points.Add(new DataPoint(120.504631, 382.5281982));
            points.Add(new DataPoint(119.1972427, 376.1178589));
            points.Add(new DataPoint(119.0547104, 372.875061));
            points.Add(new DataPoint(119.2897568, 369.6510315));
            points.Add(new DataPoint(119.9299698, 366.4787292));
            points.Add(new DataPoint(121.0029984, 363.3909607));
            points.Add(new DataPoint(122.9761124, 359.8041687));
            points.Add(new DataPoint(125.5510788, 356.6000061));
            points.Add(new DataPoint(128.5915298, 353.7098083));
            points.Add(new DataPoint(131.9611282, 351.0648499));
            points.Add(new DataPoint(139.1423569, 346.2359619));
            points.Add(new DataPoint(146.0040054, 341.5639648));
            points.Add(new DataPoint(153.3126602, 336.4172363));
            points.Add(new DataPoint(160.8510056, 331.6148376));
            points.Add(new DataPoint(176.060997, 322.2139587));
            points.Add(new DataPoint(223.2120132, 292.0619507));
            points.Add(new DataPoint(241.0090103, 281.0089722));
            points.Add(new DataPoint(244.4554214, 278.2832642));
            points.Add(new DataPoint(248.0213699, 275.5827026));
            points.Add(new DataPoint(251.8383865, 273.5125122));
            points.Add(new DataPoint(253.8821487, 272.9029541));
            points.Add(new DataPoint(256.038002, 272.677948));
            points.Add(new DataPoint(255.5765762, 275.73526));
            points.Add(new DataPoint(254.4421158, 278.3285828));
            points.Add(new DataPoint(252.9171219, 280.7803345));
            points.Add(new DataPoint(251.2840042, 283.4129639));
            points.Add(new DataPoint(249.1513138, 287.4288635));
            points.Add(new DataPoint(247.2273636, 291.7029724));
            points.Add(new DataPoint(245.725502, 296.1263123));
            points.Add(new DataPoint(244.8589859, 300.5899658));
            points.Add(new DataPoint(244.4298477, 307.2876587));
            points.Add(new DataPoint(244.5635757, 313.9535828));
            points.Add(new DataPoint(245.2291336, 320.570282));
            points.Add(new DataPoint(246.3955154, 327.1204224));
            points.Add(new DataPoint(248.0315933, 333.5866089));
            points.Add(new DataPoint(250.1063919, 339.951416));
            points.Add(new DataPoint(252.5888138, 346.1975098));
            points.Add(new DataPoint(255.4478531, 352.3074646));
            points.Add(new DataPoint(262.1715469, 364.0494385));
            points.Add(new DataPoint(270.0290298, 375.0382385));
            points.Add(new DataPoint(278.7719803, 385.1347656));
            points.Add(new DataPoint(288.1519852, 394.1999512));
            points.Add(new DataPoint(294.92173, 399.6752014));
            points.Add(new DataPoint(302.1428604, 404.5257263));
            points.Add(new DataPoint(309.7718277, 408.7135925));
            points.Add(new DataPoint(317.7649918, 412.2009583));
            points.Add(new DataPoint(322.0982132, 413.5901184));
            points.Add(new DataPoint(326.4935989, 414.2129517));
            points.Add(new DataPoint(328.6687698, 414.1019592));
            points.Add(new DataPoint(330.8044205, 413.6372986));
            points.Add(new DataPoint(332.8823013, 412.7649841));
            points.Add(new DataPoint(334.8839798, 411.4309692));
            points.Add(new DataPoint(336.700325, 409.4726868));
            points.Add(new DataPoint(337.756813, 407.2593689));
            points.Add(new DataPoint(338.1652908, 404.8675537));
            points.Add(new DataPoint(338.0374832, 402.3738403));
            points.Add(new DataPoint(337.4851761, 399.8547668));
            points.Add(new DataPoint(336.6201553, 397.3868713));
            points.Add(new DataPoint(334.3989944, 392.9109497));
            points.Add(new DataPoint(331.2719803, 388.0760193));
            points.Add(new DataPoint(327.7468643, 383.5949097));
            points.Add(new DataPoint(319.7087479, 375.5498352));
            points.Add(new DataPoint(310.6974869, 368.4870605));
            points.Add(new DataPoint(301.1259842, 362.1179504));
            points.Add(new DataPoint(289.341011, 355.3789673));
            points.Add(new DataPoint(288.3749161, 354.5166321));
            points.Add(new DataPoint(287.4871292, 353.3829651));
            points.Add(new DataPoint(287.1665115, 352.1690369));
            points.Add(new DataPoint(287.3716812, 351.5917053));
            points.Add(new DataPoint(287.9019852, 351.0659485));
            points.Add(new DataPoint(290.0786819, 350.5000305));
            points.Add(new DataPoint(292.8593826, 350.8098145));
            points.Add(new DataPoint(295.6623917, 351.5259399));
            points.Add(new DataPoint(297.9060135, 352.1789551));
            points.Add(new DataPoint(300.6676102, 353.0059814));
            points.Add(new DataPoint(303.7700882, 354.0613708));
            points.Add(new DataPoint(310.5398636, 356.3453369));
            points.Add(new DataPoint(313.9782486, 357.3178101));
            points.Add(new DataPoint(317.2997208, 358.0065918));
            points.Add(new DataPoint(320.3897781, 358.2836609));
            points.Add(new DataPoint(323.1339798, 358.0209656));
            points.Add(new DataPoint(311.4509048, 349.4853516));
            points.Add(new DataPoint(299.2507401, 341.6953125));
            points.Add(new DataPoint(286.5124283, 334.8171387));
            points.Add(new DataPoint(273.215004, 329.0169678));
            points.Add(new DataPoint(258.7229996, 323.8179626));
            points.Add(new DataPoint(257.2878494, 323.337738));
            points.Add(new DataPoint(255.7744827, 322.6670837));
            points.Add(new DataPoint(254.4943924, 321.7168884));
            points.Add(new DataPoint(253.7590103, 320.3979492));
            points.Add(new DataPoint(253.827034, 319.0723267));
            points.Add(new DataPoint(254.5322647, 317.7234802));
            points.Add(new DataPoint(255.711647, 316.3885193));
            points.Add(new DataPoint(257.2022476, 315.1045837));
            points.Add(new DataPoint(260.465126, 312.8383789));
            points.Add(new DataPoint(261.9114456, 311.9303589));
            points.Add(new DataPoint(263.0170059, 311.2219543));
            points.Add(new DataPoint(271.8112259, 305.8678284));
            points.Add(new DataPoint(281.198616, 301.1617126));
            points.Add(new DataPoint(290.8884659, 297.0829773));
            points.Add(new DataPoint(300.590004, 293.6109619));
            points.Add(new DataPoint(310.0823746, 291.1308289));
            points.Add(new DataPoint(319.4608536, 289.9150696));
            points.Add(new DataPoint(328.9314041, 289.7652893));
            points.Add(new DataPoint(338.6999893, 290.4829712));
            points.Add(new DataPoint(354.2659988, 293.3499451));
            points.Add(new DataPoint(355.8602982, 293.1119995));
            points.Add(new DataPoint(357.197731, 292.2803345));
            points.Add(new DataPoint(358.3595047, 291.0731506));
            points.Add(new DataPoint(359.4267349, 289.7085876));
            points.Add(new DataPoint(360.4805679, 288.4048157));
            points.Add(new DataPoint(361.6021194, 287.3800049));
            points.Add(new DataPoint(362.8725357, 286.8523254));
            points.Add(new DataPoint(364.3729935, 287.0399475));
            points.Add(new DataPoint(365.9158707, 287.8427734));
            points.Add(new DataPoint(367.3995438, 289.0066833));
            points.Add(new DataPoint(370.1699905, 292.0510864));
            points.Add(new DataPoint(372.6456985, 295.4399109));
            points.Add(new DataPoint(374.788002, 298.4399719));
            points.Add(new DataPoint(394.8579788, 325.8149719));
            points.Add(new DataPoint(397.3873978, 329.0663757));
            points.Add(new DataPoint(399.9987259, 332.6233521));
            points.Add(new DataPoint(402.1026993, 336.3991089));
            points.Add(new DataPoint(402.7802811, 338.3419189));
            points.Add(new DataPoint(403.109993, 340.3069458));
            points.Add(new DataPoint(403.7992325, 340.2913818));
            points.Add(new DataPoint(404.1172256, 340.3743286));
            points.Add(new DataPoint(404.2001114, 340.691864));
            points.Add(new DataPoint(404.1839981, 341.3799744));
            points.Add(new DataPoint(405.9170609, 342.5977478));
            points.Add(new DataPoint(407.5672379, 344.3135681));
            points.Add(new DataPoint(410.6323624, 348.6974487));
            points.Add(new DataPoint(413.4063187, 353.4481201));
            points.Add(new DataPoint(414.6925125, 355.6223755));
            points.Add(new DataPoint(415.9159927, 357.4819641));
            points.Add(new DataPoint(446.1409988, 402.5239563));
            points.Add(new DataPoint(447.8941116, 404.6854248));
            points.Add(new DataPoint(449.9755325, 406.9086609));
            points.Add(new DataPoint(454.483345, 411.5653381));
            points.Add(new DataPoint(456.5896683, 414.0111694));
            points.Add(new DataPoint(458.3842239, 416.5435791));
            points.Add(new DataPoint(459.7070389, 419.1687927));
            points.Add(new DataPoint(460.3980179, 421.8929749));
            points.Add(new DataPoint(460.330513, 423.2209473));
            points.Add(new DataPoint(459.8673782, 424.4043274));
            points.Add(new DataPoint(458.1563797, 426.4813232));
            points.Add(new DataPoint(456.0694046, 428.4118958));
            points.Add(new DataPoint(454.4110184, 430.4839478));
            points.Add(new DataPoint(453.6341629, 432.6287231));
            points.Add(new DataPoint(453.2503738, 434.9552002));
            points.Add(new DataPoint(452.6709671, 439.6079712));
            points.Add(new DataPoint(451.6133499, 443.3995361));
            points.Add(new DataPoint(450.0984573, 447.2178345));
            points.Add(new DataPoint(448.1988602, 450.8544312));
            points.Add(new DataPoint(445.9870071, 454.1009521));
            points.Add(new DataPoint(445.2327957, 454.9499512));
            points.Add(new DataPoint(444.120369, 456.1347046));
            points.Add(new DataPoint(441.6337357, 459.0782166));
            points.Add(new DataPoint(440.6658401, 460.6203003));
            points.Add(new DataPoint(440.1524734, 462.0648193));
            points.Add(new DataPoint(440.2967911, 463.3034973));
            points.Add(new DataPoint(441.3020096, 464.2279663));
            points.Add(new DataPoint(443.0089798, 464.4325867));
            points.Add(new DataPoint(444.7705154, 463.7559509));
            points.Add(new DataPoint(446.3872757, 462.6180725));
            points.Add(new DataPoint(447.6599808, 461.4389648));
            points.Add(new DataPoint(449.96418, 458.7336426));
            points.Add(new DataPoint(451.896492, 455.7789612));
            points.Add(new DataPoint(454.7979813, 449.3179626));
            points.Add(new DataPoint(455.2873001, 447.6248474));
            points.Add(new DataPoint(455.8775101, 445.8508301));
            points.Add(new DataPoint(456.8382034, 444.3796387));
            points.Add(new DataPoint(458.4389725, 443.5949707));
            points.Add(new DataPoint(460.0345535, 443.7620239));
            points.Add(new DataPoint(461.7698441, 444.6128235));
            points.Add(new DataPoint(463.5780106, 445.9676208));
            points.Add(new DataPoint(465.3921585, 447.6465759));
            points.Add(new DataPoint(468.7708817, 451.2580566));
            points.Add(new DataPoint(470.2016678, 452.8309631));
            points.Add(new DataPoint(471.3709793, 454.0089722));
            points.Add(new DataPoint(478.8518143, 460.0104675));
            points.Add(new DataPoint(486.9943924, 465.3340759));
            points.Add(new DataPoint(495.4878006, 470.1171265));
            points.Add(new DataPoint(504.0210037, 474.4969482));
            points.Add(new DataPoint(509.5540848, 477.1457214));
            points.Add(new DataPoint(515.2860184, 479.3189697));
            points.Add(new DataPoint(516.4008255, 479.6032104));
            points.Add(new DataPoint(517.6022415, 479.6870728));
            points.Add(new DataPoint(518.5872879, 479.3061523));
            points.Add(new DataPoint(519.0529861, 478.1959534));
            points.Add(new DataPoint(518.8626175, 476.8864441));
            points.Add(new DataPoint(518.1857986, 475.7019958));
            points.Add(new DataPoint(515.84095, 473.7122192));
            points.Add(new DataPoint(512.9545975, 472.234314));
            points.Add(new DataPoint(510.4629593, 471.2759705));
            points.Add(new DataPoint(507.6074905, 470.256958));
            points.Add(new DataPoint(504.0599442, 468.8930969));
            points.Add(new DataPoint(496.255867, 465.2143555));
            points.Add(new DataPoint(492.682869, 462.9411621));
            points.Add(new DataPoint(489.7850418, 460.4066467));
            points.Add(new DataPoint(487.9041214, 457.6316223));
            points.Add(new DataPoint(487.4518509, 456.1604309));
            points.Add(new DataPoint(487.3819656, 454.6369629));
            points.Add(new DataPoint(491.1286697, 455.6597595));
            points.Add(new DataPoint(494.7612381, 457.2131958));
            points.Add(new DataPoint(498.3274002, 458.9732666));
            points.Add(new DataPoint(501.8750076, 460.6159668));
            points.Add(new DataPoint(508.6896439, 463.2619934));
            points.Add(new DataPoint(515.7986526, 465.5415649));
            points.Add(new DataPoint(523.0577469, 467.2131042));
            points.Add(new DataPoint(530.3230057, 468.0349731));
            points.Add(new DataPoint(536.5177689, 468.0764465));
            points.Add(new DataPoint(542.669014, 467.3979492));
            points.Add(new DataPoint(543.9200516, 466.9833069));
            points.Add(new DataPoint(545.0242386, 466.2348328));
            points.Add(new DataPoint(545.5588455, 465.2004089));
            points.Add(new DataPoint(545.1009598, 463.927948));
            points.Add(new DataPoint(544.175972, 463.0953064));
            points.Add(new DataPoint(542.9511795, 462.4622803));
            points.Add(new DataPoint(539.9430008, 461.6712036));
            points.Add(new DataPoint(536.7585526, 461.3072815));
            points.Add(new DataPoint(534.0799637, 461.1229553));
            points.Add(new DataPoint(520.8706131, 459.9545898));
            points.Add(new DataPoint(514.2815628, 459.0859985));
            points.Add(new DataPoint(507.7789993, 457.8509521));
            points.Add(new DataPoint(506.4525833, 457.5542603));
            points.Add(new DataPoint(504.8121414, 457.1582336));
            points.Add(new DataPoint(501.1944656, 455.9819641));
            points.Add(new DataPoint(499.5198441, 455.1585083));
            points.Add(new DataPoint(498.1362991, 454.1494141));
            points.Add(new DataPoint(497.1952591, 452.9331055));
            points.Add(new DataPoint(496.8479691, 451.4879456));
            points.Add(new DataPoint(497.117012, 450.6549377));
            points.Add(new DataPoint(497.8393021, 450.1732788));
            points.Add(new DataPoint(500.1341019, 449.9840698));
            points.Add(new DataPoint(502.7133255, 450.3605652));
            points.Add(new DataPoint(503.7911453, 450.5859985));
            points.Add(new DataPoint(504.557991, 450.7429504));
            points.Add(new DataPoint(511.3823318, 451.3563843));
            points.Add(new DataPoint(518.3227615, 451.0169373));
            points.Add(new DataPoint(525.2244949, 449.9740295));
            points.Add(new DataPoint(531.932991, 448.4769592));
            points.Add(new DataPoint(532.8128738, 448.2760315));
            points.Add(new DataPoint(534.1250076, 447.9761353));
            points.Add(new DataPoint(537.3393631, 447.0833435));
            points.Add(new DataPoint(538.8882523, 446.4923706));
            points.Add(new DataPoint(540.1627884, 445.8063354));
            points.Add(new DataPoint(540.9862747, 445.0262146));
            points.Add(new DataPoint(541.1820145, 444.1529541));
            points.Add(new DataPoint(540.6489334, 443.4705505));
            points.Add(new DataPoint(539.4853592, 443.0022888));
            points.Add(new DataPoint(537.8792191, 442.7068176));
            points.Add(new DataPoint(536.0183792, 442.5428467));
            points.Add(new DataPoint(532.2842484, 442.4439392));
            points.Add(new DataPoint(530.7866898, 442.4263916));
            points.Add(new DataPoint(529.7860184, 442.3749695));
            points.Add(new DataPoint(522.7177811, 441.8120117));
            points.Add(new DataPoint(515.5401077, 441.6993408));
            points.Add(new DataPoint(501.2470169, 442.2559509));
            points.Add(new DataPoint(498.6232986, 442.5908508));
            points.Add(new DataPoint(495.6024857, 442.9595947));
            points.Add(new DataPoint(492.7076492, 442.7930298));
            points.Add(new DataPoint(491.4709549, 442.3311157));
            points.Add(new DataPoint(490.4619827, 441.5219727));
            points.Add(new DataPoint(489.3800125, 439.9010925));
            points.Add(new DataPoint(488.6179276, 438.0290833));
            points.Add(new DataPoint(487.6418533, 433.8266907));
            points.Add(new DataPoint(486.7113724, 429.5046997));
            points.Add(new DataPoint(486.0061722, 427.4831848));
            points.Add(new DataPoint(485.0039749, 425.6529541));
            points.Add(new DataPoint(464.5570145, 397.51297));
            points.Add(new DataPoint(441.859993, 359.0939636));
            points.Add(new DataPoint(419.5407486, 322.4837036));
            points.Add(new DataPoint(397.7539749, 285.5569458));
            points.Add(new DataPoint(392.6534195, 277.3995361));
            points.Add(new DataPoint(387.2727432, 269.1647034));
            points.Add(new DataPoint(382.4174271, 260.6722412));
            points.Add(new DataPoint(380.4385147, 256.2730713));
            points.Add(new DataPoint(378.8929825, 251.7419434));
            points.Add(new DataPoint(378.4342422, 247.8017578));
            points.Add(new DataPoint(378.8919754, 243.618103));
            points.Add(new DataPoint(379.8762283, 239.4705811));
            points.Add(new DataPoint(380.9969864, 235.6389771));
            points.Add(new DataPoint(388.6119766, 208.7999878));
            points.Add(new DataPoint(391.0815811, 198.9997559));
            points.Add(new DataPoint(393.3593521, 188.8963623));
            points.Add(new DataPoint(395.3594131, 178.5748901));
            points.Add(new DataPoint(396.9958572, 168.1204834));
            points.Add(new DataPoint(398.1828384, 157.6182251));
            points.Add(new DataPoint(398.8344498, 147.1533203));
            points.Add(new DataPoint(398.8647842, 136.8108521));
            points.Add(new DataPoint(398.1879959, 126.6759644));
            points.Add(new DataPoint(395.2633438, 103.3844604));
            points.Add(new DataPoint(393.3301468, 91.60638428));
            points.Add(new DataPoint(390.9952469, 79.85656738));
            points.Add(new DataPoint(388.1928177, 68.22253418));
            points.Add(new DataPoint(384.8570328, 56.79168701));
            points.Add(new DataPoint(380.9220352, 45.65136719));
            points.Add(new DataPoint(376.3219986, 34.88897705));
            points.Add(new DataPoint(371.1558609, 23.22259521));
            points.Add(new DataPoint(366.6760025, 11.27197266));
            points.Add(new DataPoint(365.8827591, 9.214355469));
            points.Add(new DataPoint(365.0445023, 6.805603027));
            points.Add(new DataPoint(364.7049942, 4.406799316));
            points.Add(new DataPoint(364.892189, 3.323974609));
            points.Add(new DataPoint(365.4079971, 2.378967285));
            points.Add(new DataPoint(366.5955276, 1.439331055));
            points.Add(new DataPoint(368.2348099, 0.826660156));
            points.Add(new DataPoint(372.3311234, 0.333557129));
            points.Add(new DataPoint(376.6218643, 0.40246582));
            points.Add(new DataPoint(378.5041885, 0.4921875));
            points.Add(new DataPoint(380.0319901, 0.535949707));
            points.Add(new DataPoint(420.2889786, 0));
            points.Add(new DataPoint(527.1040115, 0));
            points.Add(new DataPoint(558.0751419, 0.09362793));
            points.Add(new DataPoint(573.5140457, 0.204589844));
            points.Add(new DataPoint(588.6629715, 0.352966309));
            points.Add(new DataPoint(588.6629715, 0.352966309));
            return points;
        }

        [Example("Conway's Game of Life")]
        public static PlotModel ConwayLife()
        {
            // http://en.wikipedia.org/wiki/Conway's_Game_of_Life
            var model = new PlotModel { Title = "Conway's Game of Life", Subtitle = "Click the mouse to step to the next generation." };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StartPosition = 1, EndPosition = 0 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            int m = 40;
            int n = 40;
            var matrix = new double[m, n];
            var ms = new MatrixSeries { Matrix = matrix };

            Action<int, int> blinker = (i, j) => { matrix[i, j] = matrix[i, j + 1] = matrix[i, j + 2] = 1; };
            Action<int, int> glider = (i, j) => { matrix[i, j] = matrix[i + 1, j + 1] = matrix[i + 1, j + 2] = matrix[i + 2, j] = matrix[i + 2, j + 1] = 1; };
            Action<int, int> rpentomino = (i, j) => { matrix[i, j + 1] = matrix[i, j + 2] = matrix[i + 1, j] = matrix[i + 1, j + 1] = matrix[i + 2, j + 1] = 1; };

            blinker(2, 10);
            glider(2, 2);
            rpentomino(20, 20);

            model.Series.Add(ms);
            int g = 0;
            Action stepToNextGeneration = () =>
                {
                    var next = new double[m, n];
                    for (int i = 1; i < m - 1; i++)
                    {
                        for (int j = 1; j < n - 1; j++)
                        {
                            int k = (int)(matrix[i - 1, j - 1] + matrix[i - 1, j] + matrix[i - 1, j + 1] + matrix[i, j - 1]
                                    + matrix[i, j + 1] + matrix[i + 1, j - 1] + matrix[i + 1, j]
                                    + matrix[i + 1, j + 1]);

                            if (matrix[i, j].Equals(0) && k == 3)
                            {
                                // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                                next[i, j] = 1;
                                continue;
                            }

                            if (matrix[i, j].Equals(1) && (k == 2 || k == 3))
                            {
                                // Any live cell with two or three live neighbours lives on to the next generation.
                                next[i, j] = 1;
                            }

                            // Any live cell with fewer than two live neighbours dies, as if caused by under-population.
                            // Any live cell with more than three live neighbours dies, as if by overcrowding.
                        }
                    }

                    g++;
                    ms.Title = "Generation " + g;
                    ms.Matrix = matrix = next;
                    model.InvalidatePlot(true);
                };

            model.MouseDown += (s, e) =>
                {
                    if (e.ChangedButton == OxyMouseButton.Left)
                    {
                        stepToNextGeneration();
                        e.Handled = true;
                    }
                };

            return model;
        }

        [Example("Mandelbrot custom series")]
        public static PlotModel Mandelbrot()
        {
            // http://en.wikipedia.org/wiki/Mandelbrot_set
            var model = new PlotModel { Title = "The Mandelbrot set" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -1.4, Maximum = 1.4 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -2, Maximum = 1 });
            model.Axes.Add(
                new LinearColorAxis
                {
                    Position = AxisPosition.Right,
                    Minimum = 0,
                    Maximum = 64,
                    Palette = OxyPalettes.Jet(64),
                    HighColor = OxyColors.Black
                });
            model.Series.Add(new MandelbrotSetSeries());
            return model;
        }

        [Example("Julia set custom series")]
        public static PlotModel JuliaSet()
        {
            // http://en.wikipedia.org/wiki/Julia_set
            var model = new PlotModel { Subtitle = "Click and move the mouse" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -2, Maximum = 2 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -2.5, Maximum = 2.5 });
            model.Axes.Add(
                new LinearColorAxis
                {
                    Position = AxisPosition.Right,
                    Minimum = 0,
                    Maximum = 64,
                    Palette = OxyPalettes.Jet(64),
                    HighColor = OxyColors.Black
                });

            var jss = new JuliaSetSeries();

            // Delegate to set the c and title
            Action<double, double> setConstant = (c1, c2) =>
            {
                jss.C1 = c1;
                jss.C2 = c2;
                model.Title = string.Format("The Julia set, c={0:0.00000}+{1:0.00000}i", jss.C1, jss.C2);
            };

            // Update the c by the position where the mouse was clicked/moved
            Action<OxyMouseEventArgs> handleMouseEvent = e =>
            {
                var c = jss.InverseTransform(e.Position);
                setConstant(c.X, c.Y);
                model.InvalidatePlot(true);
                e.Handled = true;
            };
            jss.MouseDown += (s, e) => handleMouseEvent(e);
            jss.MouseMove += (s, e) => handleMouseEvent(e);

            // set the initial c
            setConstant(-0.726895347709114071439, 0.188887129043845954792);

            model.Series.Add(jss);
            return model;
        }

        [Example("Elephant curve")]
        public static PlotModel ElephantCurve()
        {
            // http://www.wolframalpha.com/input/?i=elephant+curve
            // See also https://gist.github.com/purem/4687549/raw/8dda1f16cc70469aedecec19af1530534eadbae3/Wolfram+alpha+named+parametric+curves
            Func<double, double> sin = Math.Sin;
            Func<double, double> x =
                t =>
                    -27d / 5 * sin(3d / 2 - 30 * t) - 16d / 3 * sin(9d / 8 - 29 * t) - 29d / 5 * sin(5d / 4 - 27 * t)
                    - 8d / 3 * sin(1d / 4 - 26 * t) - 25d / 7 * sin(1d / 3 - 25 * t) - 31d / 4 * sin(4d / 7 - 22 * t)
                    - 25d / 4 * sin(4d / 3 - 20 * t) - 33d / 2 * sin(2d / 3 - 19 * t) - 67d / 4 * sin(6d / 5 - 16 * t)
                    - 100d / 11 * sin(1d / 4 - 10 * t) - 425d / 7 * sin(1 - 4 * t) + 149d / 4 * sin(8 * t)
                    + 1172d / 3 * sin(t + 21d / 5) + 661d / 11 * sin(2 * t + 3) + 471d / 8 * sin(3 * t + 10d / 7)
                    + 211d / 7 * sin(5 * t + 13d / 4) + 39d / 4 * sin(6 * t + 10d / 7) + 139d / 10 * sin(7 * t + 7d / 6)
                    + 77d / 3 * sin(9 * t + 18d / 7) + 135d / 8 * sin(11 * t + 1d / 2) + 23d / 4 * sin(12 * t + 8d / 5)
                    + 95d / 4 * sin(13 * t + 4) + 31d / 4 * sin(14 * t + 3d / 5) + 67d / 11 * sin(15 * t + 7d / 3)
                    + 127d / 21 * sin(17 * t + 17d / 4) + 95d / 8 * sin(18 * t + 7d / 8)
                    + 32d / 11 * sin(21 * t + 8d / 3) + 81d / 10 * sin(23 * t + 45d / 11)
                    + 13d / 3 * sin(24 * t + 13d / 4) + 7d / 4 * sin(28 * t + 3d / 2) + 11d / 5 * sin(31 * t + 5d / 2)
                    + 1d / 3 * sin(32 * t + 12d / 5) + 13d / 4 * sin(33 * t + 22d / 5) + 14d / 3 * sin(34 * t + 9d / 4)
                    + 9d / 5 * sin(35 * t + 8d / 5) + 17d / 9 * sin(36 * t + 22d / 5) + 1d / 3 * sin(37 * t + 15d / 7)
                    + 3d / 2 * sin(38 * t + 39d / 10) + 4d / 3 * sin(39 * t + 7d / 2) + 5d / 3 * sin(40 * t + 17d / 6);
            Func<double, double> y =
                t =>
                    -13d / 7 * sin(1d / 2 - 40 * t) - 31d / 8 * sin(1d / 11 - 34 * t) - 12d / 5 * sin(1d / 4 - 31 * t)
                    - 9d / 4 * sin(4d / 3 - 29 * t) - 5d / 3 * sin(4d / 3 - 28 * t) - 11d / 2 * sin(6d / 5 - 26 * t)
                    - 17d / 7 * sin(3d / 2 - 25 * t) - 5d / 2 * sin(1 - 24 * t) - 39d / 7 * sin(1 - 19 * t)
                    - 59d / 5 * sin(2d / 3 - 18 * t) - 179d / 9 * sin(13d / 12 - 12 * t)
                    - 103d / 2 * sin(1d / 10 - 9 * t) - 356d / 5 * sin(1 - 5 * t) - 429d / 2 * sin(20d / 19 - t)
                    + 288d / 5 * sin(2 * t + 10d / 3) + 53d / 6 * sin(3 * t + 5d / 2) + 351d / 7 * sin(4 * t + 5d / 2)
                    + 201d / 4 * sin(6 * t + 17d / 7) + 167d / 3 * sin(7 * t + 19d / 5) + 323d / 5 * sin(8 * t + 1d / 4)
                    + 153d / 7 * sin(10 * t + 2d / 3) + 71d / 5 * sin(11 * t + 6d / 5)
                    + 47d / 12 * sin(13 * t + 11d / 5) + 391d / 26 * sin(14 * t + 2) + 164d / 11 * sin(15 * t + 1d / 7)
                    + 11d / 2 * sin(16 * t + 2d / 3) + 31d / 3 * sin(17 * t + 1d / 7) + 54d / 11 * sin(20 * t + 1d / 4)
                    + 43d / 5 * sin(21 * t + 13d / 3) + 13d / 5 * sin(22 * t + 3d / 2) + 17d / 5 * sin(23 * t + 11d / 5)
                    + 19d / 10 * sin(27 * t + 4) + 15d / 2 * sin(30 * t + 55d / 18) + 4d / 3 * sin(32 * t + 3d / 5)
                    + 5d / 3 * sin(33 * t + 4) + 27d / 7 * sin(35 * t + 13d / 6) + 1d / 4 * sin(36 * t + 43d / 11)
                    + 16d / 5 * sin(37 * t + 9d / 2) + 20d / 19 * sin(38 * t + 23d / 6) + 8d / 3 * sin(39 * t + 4d / 7);

            var model = new PlotModel { Title = "Elephant curve", PlotType = PlotType.Cartesian };
            model.Series.Add(new FunctionSeries(x, y, 0, Math.PI * 2, 1000));
            return model;
        }

        [Example("PI curve")]
        public static PlotModel PiCurve()
        {
            // http://www.wolframalpha.com/input/?i=pi+curve
            Func<double, double> sin = Math.Sin;
            Func<double, double> x =
                t => 17d / 31 * sin(235d / 57 - 32 * t) + 19d / 17 * sin(192d / 55 - 30 * t) + 47d / 32 * sin(69d / 25 - 29 * t) + 35d / 26 * sin(75d / 34 - 27 * t) + 6d / 31 * sin(23d / 10 - 26 * t) + 35d / 43 * sin(10d / 33 - 25 * t) + 126d / 43 * sin(421d / 158 - 24 * t) + 143d / 57 * sin(35d / 22 - 22 * t) + 106d / 27 * sin(84d / 29 - 21 * t) + 88d / 25 * sin(23d / 27 - 20 * t) + 74d / 27 * sin(53d / 22 - 19 * t) + 44d / 53 * sin(117d / 25 - 18 * t) + 126d / 25 * sin(88d / 49 - 17 * t) + 79d / 11 * sin(43d / 26 - 16 * t) + 43d / 12 * sin(41d / 17 - 15 * t) + 47d / 27 * sin(244d / 81 - 14 * t) + 8d / 5 * sin(79d / 19 - 13 * t) + 373d / 46 * sin(109d / 38 - 12 * t) + 1200d / 31 * sin(133d / 74 - 11 * t) + 67d / 24 * sin(157d / 61 - 10 * t) + 583d / 28 * sin(13d / 8 - 8 * t) + 772d / 35 * sin(59d / 16 - 7 * t) + 3705d / 46 * sin(117d / 50 - 6 * t) + 862d / 13 * sin(19d / 8 - 5 * t) + 6555d / 34 * sin(157d / 78 - 3 * t) + 6949d / 13 * sin(83d / 27 - t) - 6805d / 54 * sin(2 * t + 1d / 145) - 5207d / 37 * sin(4 * t + 49d / 74) - 1811d / 58 * sin(9 * t + 55d / 43) - 63d / 20 * sin(23 * t + 2d / 23) - 266d / 177 * sin(28 * t + 13d / 18) - 2d / 21 * sin(31 * t + 7d / 16);
            Func<double, double> y =
                t => 70d / 37 * sin(65d / 32 - 32 * t) + 11d / 12 * sin(98d / 41 - 31 * t) + 26d / 29 * sin(35d / 12 - 30 * t) + 54d / 41 * sin(18d / 7 - 29 * t) + 177d / 71 * sin(51d / 19 - 27 * t) + 59d / 34 * sin(125d / 33 - 26 * t) + 49d / 29 * sin(18d / 11 - 25 * t) + 151d / 75 * sin(59d / 22 - 24 * t) + 52d / 9 * sin(118d / 45 - 22 * t) + 52d / 33 * sin(133d / 52 - 21 * t) + 37d / 45 * sin(61d / 14 - 20 * t) + 143d / 46 * sin(144d / 41 - 19 * t) + 254d / 47 * sin(19d / 52 - 18 * t) + 246d / 35 * sin(92d / 25 - 17 * t) + 722d / 111 * sin(176d / 67 - 16 * t) + 136d / 23 * sin(3d / 19 - 15 * t) + 273d / 25 * sin(32d / 21 - 13 * t) + 229d / 33 * sin(117d / 28 - 12 * t) + 19d / 4 * sin(43d / 11 - 11 * t) + 135d / 8 * sin(23d / 10 - 10 * t) + 205d / 6 * sin(33d / 23 - 8 * t) + 679d / 45 * sin(55d / 12 - 7 * t) + 101d / 8 * sin(11d / 12 - 6 * t) + 2760d / 59 * sin(40d / 11 - 5 * t) + 1207d / 18 * sin(21d / 23 - 4 * t) + 8566d / 27 * sin(39d / 28 - 3 * t) + 12334d / 29 * sin(47d / 37 - 2 * t) + 15410d / 39 * sin(185d / 41 - t) - 596d / 17 * sin(9 * t + 3d / 26) - 247d / 28 * sin(14 * t + 25d / 21) - 458d / 131 * sin(23 * t + 21d / 37) - 41d / 36 * sin(28 * t + 7d / 8);

            var model = new PlotModel { Title = "PI curve", PlotType = PlotType.Cartesian };
            model.Series.Add(new FunctionSeries(x, y, 0, Math.PI * 2, 1000));
            return model;
        }

        [Example("Angelina Jolie curve")]
        public static PlotModel AngelinaJolieCurve()
        {
            // http://www.wolframalpha.com/input/?i=Angelina+Jolie+curve

            // Heaviside step function
            Func<double, double> theta = x => x < 0 ? 0 : 1;

            Func<double, double> sin = Math.Sin;
            double pi = Math.PI;
            Func<double, double> xt =
                t =>
                ((-23d / 4 * sin(29d / 20 - 41 * t) - 47d / 15 * sin(47d / 34 - 38 * t)
                  - 979d / 196 * sin(45d / 31 - 36 * t) - 59d / 6 * sin(25d / 17 - 33 * t)
                  - 259d / 26 * sin(104d / 69 - 32 * t) - 49d / 24 * sin(57d / 37 - 31 * t)
                  - 37d / 21 * sin(32d / 23 - 30 * t) - 324d / 31 * sin(85d / 57 - 27 * t)
                  - 47d / 26 * sin(24d / 17 - 25 * t) - 247d / 29 * sin(39d / 25 - 24 * t)
                  - 834d / 71 * sin(63d / 41 - 23 * t) - 475d / 49 * sin(62d / 41 - 22 * t)
                  - 20d / 7 * sin(29d / 20 - 17 * t) - 1286d / 61 * sin(98d / 65 - 16 * t)
                  - 2312d / 25 * sin(20d / 13 - 14 * t) - 2662d / 41 * sin(23d / 15 - 13 * t)
                  - 2292d / 131 * sin(92d / 61 - 11 * t) - 1690d / 37 * sin(48d / 31 - 8 * t)
                  + 993d / 23 * sin(t + 52d / 33) + 407d / 31 * sin(2 * t + 25d / 16)
                  + 1049d / 57 * sin(3 * t + 49d / 31) + 1385d / 42 * sin(4 * t + 80d / 17)
                  + 2929d / 40 * sin(5 * t + 49d / 31) + 500d / 39 * sin(6 * t + 164d / 35)
                  + 116d / 25 * sin(7 * t + 155d / 33) + 2593d / 26 * sin(9 * t + 43d / 27)
                  + 200d / 37 * sin(10 * t + 65d / 42) + 2866d / 39 * sin(12 * t + 133d / 83)
                  + 703d / 234 * sin(15 * t + 46d / 27) + 133d / 8 * sin(18 * t + 13d / 8)
                  + 716d / 33 * sin(19 * t + 27d / 17) + 180d / 53 * sin(20 * t + 47d / 30)
                  + 476d / 31 * sin(21 * t + 57d / 35) + 73d / 22 * sin(26 * t + 77d / 48)
                  + 549d / 49 * sin(28 * t + 44d / 27) + 657d / 68 * sin(29 * t + 27d / 17)
                  + 29d / 22 * sin(34 * t + 140d / 31) + 180d / 49 * sin(35 * t + 14d / 9)
                  + 43d / 4 * sin(37 * t + 77d / 46) + 68d / 23 * sin(39 * t + 39d / 25)
                  + 80d / 47 * sin(40 * t + 49d / 27) + 16829d / 29) * theta(119 * pi - t)
                 * theta(t - 115 * pi)
                 + (-43d / 40 * sin(39d / 29 - 62 * t) - 44d / 17 * sin(56d / 37 - 58 * t)
                    - 23d / 39 * sin(14d / 11 - 57 * t) - 59d / 10 * sin(49d / 32 - 51 * t)
                    - 215d / 31 * sin(20d / 13 - 46 * t) - 447d / 22 * sin(110d / 73 - 34 * t)
                    - 407d / 39 * sin(54d / 35 - 33 * t) - 432d / 35 * sin(14d / 9 - 29 * t)
                    - 175d / 33 * sin(35d / 24 - 28 * t) - 102d / 5 * sin(64d / 41 - 22 * t)
                    + 833d / 46 * sin(t + 131d / 28) + 1360d / 9 * sin(2 * t + 52d / 33)
                    + 139d / 3 * sin(3 * t + 43d / 27) + 108d / 29 * sin(4 * t + 244d / 53)
                    + 4269d / 41 * sin(5 * t + 113d / 24) + 7407d / 31 * sin(6 * t + 65d / 41)
                    + 2159d / 32 * sin(7 * t + 103d / 22) + 5476d / 51 * sin(8 * t + 37d / 23)
                    + 3855d / 37 * sin(9 * t + 60d / 37) + 2247d / 46 * sin(10 * t + 18d / 11)
                    + 216d / 31 * sin(11 * t + 81d / 40) + 3364d / 35 * sin(12 * t + 69d / 43)
                    + 1492d / 21 * sin(13 * t + 47d / 10) + 1981d / 29 * sin(14 * t + 21d / 13)
                    + 1852d / 35 * sin(15 * t + 18d / 11) + 255d / 23 * sin(16 * t + 72d / 41)
                    + 499d / 25 * sin(17 * t + 134d / 29) + 754d / 17 * sin(18 * t + 57d / 35)
                    + 203d / 31 * sin(19 * t + 35d / 19) + 1289d / 32 * sin(20 * t + 41d / 25)
                    + 65d / 21 * sin(21 * t + 55d / 13) + 731d / 31 * sin(23 * t + 34d / 21)
                    + 816d / 83 * sin(24 * t + 23d / 14) + 467d / 29 * sin(25 * t + 197d / 42)
                    + 496d / 37 * sin(26 * t + 64d / 39) + 34d / 9 * sin(27 * t + 40d / 27)
                    + 204d / 23 * sin(30 * t + 76d / 43) + 34d / 3 * sin(31 * t + 50d / 29)
                    + 1579d / 57 * sin(32 * t + 51d / 31) + 37d / 6 * sin(35 * t + 69d / 44)
                    + 128d / 21 * sin(36 * t + 21d / 13) + 194d / 83 * sin(37 * t + 52d / 27)
                    + 35d / 37 * sin(38 * t + 46d / 19) + 39d / 38 * sin(39 * t + 234d / 55)
                    + 113d / 16 * sin(40 * t + 71d / 43) + 126d / 101 * sin(41 * t + 145d / 83)
                    + 13d / 6 * sin(42 * t + 184d / 41) + 100d / 31 * sin(43 * t + 117d / 25)
                    + 355d / 36 * sin(44 * t + 48d / 29) + 148d / 57 * sin(45 * t + 30d / 17)
                    + 3d / 2 * sin(47 * t + 51d / 28) + 107d / 61 * sin(48 * t + 27d / 16)
                    + 72d / 13 * sin(49 * t + 93d / 56) + 55d / 37 * sin(50 * t + 144d / 31)
                    + 53d / 24 * sin(52 * t + 59d / 34) + 182d / 47 * sin(53 * t + 41d / 24)
                    + 481d / 103 * sin(54 * t + 110d / 61) + 97d / 29 * sin(55 * t + 89d / 19)
                    + 7d / 4 * sin(56 * t + 49d / 30) + 82d / 37 * sin(59 * t + 55d / 28)
                    + 20d / 13 * sin(60 * t + 45d / 23) + 147d / 34 * sin(61 * t + 77d / 45) - 27563d / 35)
                   * theta(115 * pi - t) * theta(t - 111 * pi)
                 + (-11d / 13 * sin(37d / 33 - 98 * t) - 13d / 32 * sin(69d / 44 - 97 * t)
                    - 33d / 19 * sin(38d / 29 - 88 * t) - 124d / 45 * sin(44d / 29 - 87 * t)
                    - 72d / 37 * sin(56d / 43 - 77 * t) - 78d / 77 * sin(197d / 131 - 72 * t)
                    - 39d / 58 * sin(7d / 33 - 54 * t) - 31d / 20 * sin(26d / 25 - 37 * t)
                    - 265d / 58 * sin(122d / 81 - 35 * t) - 4901d / 196 * sin(49d / 32 - 12 * t)
                    - 7950d / 49 * sin(91d / 58 - 8 * t) - 515d / 12 * sin(25d / 16 - 5 * t)
                    + 21289d / 23 * sin(t + 80d / 51) + 4245d / 8 * sin(2 * t + 36d / 23)
                    + 394321d / 930 * sin(3 * t + 113d / 24) + 4699d / 17 * sin(4 * t + 113d / 24)
                    + 1931d / 41 * sin(6 * t + 43d / 28) + 745d / 24 * sin(7 * t + 40d / 27)
                    + 861d / 8 * sin(9 * t + 113d / 24) + 1348d / 15 * sin(10 * t + 146d / 31)
                    + 1015d / 39 * sin(11 * t + 43d / 28) + 590d / 39 * sin(13 * t + 53d / 34)
                    + 271d / 37 * sin(14 * t + 48d / 31) + 268d / 29 * sin(15 * t + 63d / 38)
                    + 443d / 25 * sin(16 * t + 127d / 27) + 872d / 37 * sin(17 * t + 53d / 35)
                    + 2329d / 82 * sin(18 * t + 145d / 31) + 11d / 4 * sin(19 * t + 832d / 185)
                    + 1139d / 36 * sin(20 * t + 49d / 32) + 203d / 13 * sin(21 * t + 114d / 25)
                    + 2807d / 72 * sin(22 * t + 48d / 31) + 639d / 26 * sin(23 * t + 19d / 12)
                    + 163d / 23 * sin(24 * t + 43d / 26) + 517d / 36 * sin(25 * t + 75d / 47)
                    + 359d / 30 * sin(26 * t + 159d / 34) + 603d / 32 * sin(27 * t + 46d / 31)
                    + 1679d / 34 * sin(28 * t + 387d / 83) + 269d / 22 * sin(29 * t + 41d / 28)
                    + 94d / 39 * sin(30 * t + 56d / 51) + 1219d / 54 * sin(31 * t + 51d / 11)
                    + 535d / 29 * sin(32 * t + 61d / 41) + 17d / 52 * sin(33 * t + 54d / 49)
                    + 133d / 33 * sin(34 * t + 308d / 67) + 73d / 18 * sin(36 * t + 262d / 57)
                    + 131d / 20 * sin(38 * t + 134d / 29) + 391d / 72 * sin(39 * t + 428d / 93)
                    + 505d / 26 * sin(40 * t + 3d / 2) + 39d / 10 * sin(41 * t + 256d / 57)
                    + 76d / 21 * sin(42 * t + 22d / 13) + 341d / 37 * sin(43 * t + 16d / 11)
                    + 67d / 39 * sin(44 * t + 93d / 22) + 211d / 26 * sin(45 * t + 167d / 36)
                    + 161d / 20 * sin(46 * t + 19d / 13) + 175d / 23 * sin(47 * t + 124d / 27)
                    + 229d / 35 * sin(48 * t + 59d / 39) + 23d / 19 * sin(49 * t + 36d / 23)
                    + 59d / 34 * sin(50 * t + 40d / 9) + 399d / 46 * sin(51 * t + 75d / 52)
                    + 351d / 49 * sin(52 * t + 119d / 26) + 23d / 17 * sin(53 * t + 49d / 44)
                    + 179d / 17 * sin(55 * t + 308d / 67) + 74d / 25 * sin(56 * t + 84d / 61)
                    + 7d / 10 * sin(57 * t + 21d / 17) + 58d / 39 * sin(58 * t + 94d / 21)
                    + 44d / 23 * sin(59 * t + 97d / 54) + 265d / 36 * sin(60 * t + 47d / 32)
                    + 44d / 87 * sin(61 * t + 49d / 26) + 899d / 142 * sin(62 * t + 143d / 31)
                    + 109d / 29 * sin(63 * t + 93d / 70) + 391d / 55 * sin(64 * t + 51d / 11)
                    + 24d / 37 * sin(65 * t + 29d / 38) + 151d / 30 * sin(66 * t + 78d / 17)
                    + 291d / 50 * sin(67 * t + 44d / 29) + 124d / 29 * sin(68 * t + 82d / 55)
                    + 83d / 52 * sin(69 * t + 95d / 47) + 22d / 21 * sin(70 * t + 81d / 38)
                    + 301d / 47 * sin(71 * t + 13d / 9) + 341d / 28 * sin(73 * t + 239d / 52)
                    + 133d / 22 * sin(74 * t + 14d / 11) + 245d / 39 * sin(75 * t + 47d / 10)
                    + 136d / 29 * sin(76 * t + 537d / 115) + 250d / 43 * sin(78 * t + 25d / 17)
                    + 30d / 41 * sin(79 * t + 83d / 23) + 5d / 2 * sin(80 * t + 198d / 113)
                    + 129d / 29 * sin(81 * t + 21d / 13) + 169d / 32 * sin(82 * t + 73d / 47)
                    + 28d / 29 * sin(83 * t + 133d / 33) + 129d / 40 * sin(84 * t + 86d / 19)
                    + 34d / 11 * sin(85 * t + 56d / 43) + 143d / 32 * sin(86 * t + 163d / 35)
                    + 77d / 25 * sin(89 * t + 27d / 20) + 418d / 93 * sin(90 * t + 166d / 37)
                    + 103d / 37 * sin(91 * t + 55d / 39) + 16d / 13 * sin(92 * t + 45d / 26)
                    + 87d / 40 * sin(93 * t + 55d / 32) + 53d / 25 * sin(94 * t + 67d / 41)
                    + 58d / 43 * sin(95 * t + 26d / 15) + 67d / 30 * sin(96 * t + 149d / 99)
                    + 155d / 41 * sin(99 * t + 124d / 27) - 6637d / 21) * theta(111 * pi - t)
                   * theta(t - 107 * pi)
                 + (-27d / 23 * sin(5d / 12 - 35 * t) - 94d / 23 * sin(32d / 31 - 34 * t)
                    - 26d / 9 * sin(17d / 27 - 33 * t) - 226d / 31 * sin(43d / 33 - 32 * t)
                    - 211d / 50 * sin(38d / 25 - 29 * t) - 11d / 18 * sin(18d / 23 - 28 * t)
                    - 23d / 18 * sin(32d / 31 - 27 * t) - 255d / 94 * sin(27d / 20 - 25 * t)
                    - 41d / 31 * sin(65d / 47 - 21 * t) - 59d / 12 * sin(33d / 28 - 19 * t)
                    - 160d / 31 * sin(31d / 29 - 18 * t) - 859d / 78 * sin(165d / 124 - 17 * t)
                    - 137d / 13 * sin(37d / 26 - 16 * t) - 25d / 23 * sin(175d / 117 - 9 * t)
                    + 937d / 6 * sin(t + 41d / 26) + 41d / 9 * sin(2 * t + 163d / 36)
                    + 563d / 33 * sin(3 * t + 21d / 13) + 19d / 8 * sin(4 * t + 181d / 40)
                    + 53d / 15 * sin(5 * t + 77d / 46) + 10d / 31 * sin(6 * t + 73d / 23)
                    + 133d / 39 * sin(7 * t + 67d / 42) + 5d / 9 * sin(8 * t + 30d / 19)
                    + 22d / 13 * sin(10 * t + 47d / 27) + 108d / 37 * sin(11 * t + 90d / 53)
                    + 37d / 40 * sin(12 * t + 47d / 23) + 18d / 13 * sin(13 * t + 49d / 11)
                    + 63d / 40 * sin(14 * t + 139d / 31) + 453d / 34 * sin(15 * t + 65d / 41)
                    + 27d / 32 * sin(20 * t + 13d / 46) + 8d / 19 * sin(22 * t + 58d / 35)
                    + 19d / 30 * sin(23 * t + 157d / 34) + 10d / 23 * sin(24 * t + 7d / 27)
                    + 51d / 31 * sin(26 * t + 29d / 24) + 83d / 26 * sin(30 * t + 52d / 31)
                    + 179d / 45 * sin(31 * t + 56d / 37) + 14d / 23 * sin(36 * t + 45d / 29)
                    + 56d / 33 * sin(37 * t + 69d / 37) + 62d / 61 * sin(38 * t + 192d / 47)
                    + 4d / 5 * sin(39 * t + 102d / 47) + 7d / 43 * sin(40 * t + 67d / 18)
                    + 3d / 13 * sin(41 * t + 39d / 38) + 15d / 32 * sin(42 * t + 75d / 16)
                    + 34d / 31 * sin(43 * t + 53d / 29) + 31d / 32 * sin(44 * t + 52d / 25)
                    + 11d / 29 * sin(45 * t + 23d / 6) + 19d / 11 * sin(46 * t + 134d / 29)
                    + 55d / 27 * sin(47 * t + 30d / 17) + 8d / 25 * sin(48 * t + 185d / 44)
                    + 12d / 31 * sin(49 * t + 61d / 41) - 803d / 16) * theta(107 * pi - t)
                   * theta(t - 103 * pi)
                 + (-183d / 137 * sin(41d / 31 - 53 * t) - 3d / 2 * sin(45d / 32 - 51 * t)
                    - 21d / 37 * sin(17d / 24 - 50 * t) - 109d / 73 * sin(21d / 16 - 49 * t)
                    - 28d / 23 * sin(26d / 19 - 46 * t) - 499d / 95 * sin(49d / 38 - 44 * t)
                    - 79d / 37 * sin(37d / 28 - 41 * t) - 28d / 25 * sin(35d / 33 - 35 * t)
                    - 39d / 10 * sin(23d / 17 - 34 * t) - 17d / 6 * sin(27d / 22 - 33 * t)
                    - 40d / 29 * sin(4d / 3 - 32 * t) - 159d / 65 * sin(23d / 16 - 30 * t)
                    - 130d / 31 * sin(43d / 34 - 29 * t) - 182d / 27 * sin(37d / 26 - 28 * t)
                    - 19d / 13 * sin(35d / 23 - 26 * t) - 67d / 39 * sin(58d / 49 - 25 * t)
                    - 131d / 37 * sin(81d / 59 - 24 * t) - 29d / 18 * sin(53d / 40 - 23 * t)
                    - 77d / 51 * sin(14d / 11 - 22 * t) - 457d / 61 * sin(68d / 47 - 21 * t)
                    - 293d / 27 * sin(53d / 36 - 15 * t) - 80d / 39 * sin(23d / 15 - 14 * t)
                    - 65d / 17 * sin(31d / 21 - 11 * t) - 21d / 17 * sin(26d / 17 - 10 * t)
                    - 41d / 13 * sin(57d / 37 - 8 * t) + 17496d / 85 * sin(t + 41d / 26)
                    + 26d / 17 * sin(2 * t + 418d / 93) + 449d / 24 * sin(3 * t + 27d / 17)
                    + 221d / 63 * sin(4 * t + 51d / 11) + 283d / 43 * sin(5 * t + 69d / 43)
                    + 16d / 15 * sin(6 * t + 64d / 37) + 29d / 11 * sin(7 * t + 59d / 36)
                    + 201d / 44 * sin(9 * t + 59d / 37) + 97d / 83 * sin(12 * t + 35d / 22)
                    + 77d / 10 * sin(13 * t + 173d / 104) + 23d / 22 * sin(16 * t + 61d / 32)
                    + 349d / 32 * sin(17 * t + 61d / 36) + 60d / 17 * sin(18 * t + 79d / 46)
                    + 11d / 23 * sin(19 * t + 61d / 29) + 373d / 86 * sin(20 * t + 48d / 29)
                    + 99d / 16 * sin(27 * t + 67d / 40) + 217d / 38 * sin(31 * t + 46d / 27)
                    + 25d / 29 * sin(36 * t + 189d / 41) + 423d / 106 * sin(37 * t + 49d / 27)
                    + 11d / 12 * sin(38 * t + 59d / 29) + 184d / 105 * sin(39 * t + 65d / 34)
                    + 93d / 50 * sin(40 * t + 93d / 49) + 8d / 25 * sin(42 * t + 45d / 22)
                    + 13d / 11 * sin(43 * t + 46d / 29) + 19d / 28 * sin(45 * t + 27d / 28)
                    + 14d / 23 * sin(47 * t + 37d / 31) + 9d / 31 * sin(48 * t + 25d / 16)
                    + 52d / 31 * sin(52 * t + 56d / 31) - 20749d / 273) * theta(103 * pi - t)
                   * theta(t - 99 * pi)
                 + (-2d / 27 * sin(47d / 30 - 3 * t) + 321d / 16 * sin(t + 63d / 40)
                    + 219d / 31 * sin(2 * t + 41d / 26) + 69d / 47 * sin(4 * t + 30d / 19)
                    + 36d / 23 * sin(5 * t + 27d / 17) + 888d / 11) * theta(99 * pi - t)
                   * theta(t - 95 * pi)
                 + (-93d / 25 * sin(25d / 16 - 8 * t) - 85d / 33 * sin(25d / 16 - 7 * t)
                    - 217d / 38 * sin(47d / 30 - 6 * t) - 989d / 49 * sin(25d / 16 - 5 * t)
                    + 6925d / 39 * sin(t + 11d / 7) + 3173d / 138 * sin(2 * t + 52d / 33)
                    + 289d / 9 * sin(3 * t + 146d / 31) + 187d / 23 * sin(4 * t + 41d / 26)
                    + 16d / 7 * sin(9 * t + 164d / 35) + 19d / 22 * sin(10 * t + 429d / 92)
                    + 4d / 27 * sin(11 * t + 47d / 32) + 23d / 40 * sin(12 * t + 29d / 19) - 1249d / 41)
                   * theta(95 * pi - t) * theta(t - 91 * pi)
                 + (-14d / 51 * sin(65d / 43 - 4 * t) + 2519d / 13 * sin(t + 74d / 47)
                    + 14d / 11 * sin(2 * t + 151d / 33) + 601d / 28 * sin(3 * t + 30d / 19) - 5627d / 23)
                   * theta(91 * pi - t) * theta(t - 87 * pi)
                 + (-268d / 37 * sin(91d / 58 - t) + 1d / 47 * sin(2 * t + 31d / 22)
                    + 68d / 41 * sin(3 * t + 113d / 24) + 185d / 19 * sin(4 * t + 52d / 33)
                    + 133d / 53 * sin(5 * t + 65d / 41) + 3d / 35 * sin(6 * t + 122d / 65) - 14306d / 33)
                   * theta(87 * pi - t) * theta(t - 83 * pi)
                 + (-64d / 15 * sin(66d / 43 - 12 * t) - 244d / 63 * sin(38d / 25 - 10 * t)
                    - 6d / 5 * sin(17d / 12 - 9 * t) - 632d / 39 * sin(20d / 13 - 8 * t)
                    - 986d / 33 * sin(20d / 13 - 7 * t) - 123d / 23 * sin(31d / 20 - 6 * t)
                    - 923d / 32 * sin(36d / 23 - 4 * t) + 1270d / 19 * sin(t + 74d / 47)
                    + 70d / 19 * sin(2 * t + 53d / 32) + 1359d / 43 * sin(3 * t + 41d / 26)
                    + 854d / 27 * sin(5 * t + 30d / 19) + 32d / 13 * sin(11 * t + 19d / 12) + 15881d / 49)
                   * theta(83 * pi - t) * theta(t - 79 * pi)
                 + (-1009d / 28 * sin(36d / 23 - 6 * t) + 1362d / 19 * sin(t + 11d / 7)
                    + 113d / 11 * sin(2 * t + 174d / 37) + 2128d / 45 * sin(3 * t + 30d / 19)
                    + 3805d / 58 * sin(4 * t + 30d / 19) + 1316d / 51 * sin(5 * t + 41d / 26)
                    + 500d / 33 * sin(7 * t + 85d / 54) + 69d / 8 * sin(8 * t + 179d / 38)
                    + 337d / 37 * sin(9 * t + 19d / 12) + 59d / 18 * sin(10 * t + 53d / 33)
                    + 5d / 24 * sin(11 * t + 87d / 46) + 335d / 44 * sin(12 * t + 46d / 29) - 9149d / 24)
                   * theta(79 * pi - t) * theta(t - 75 * pi)
                 + (-3d / 32 * sin(37d / 27 - 14 * t) - 31d / 25 * sin(54d / 35 - 11 * t)
                    - 101d / 51 * sin(39d / 25 - 6 * t) + 4553d / 33 * sin(t + 63d / 40)
                    + 771d / 43 * sin(2 * t + 65d / 41) + 679d / 45 * sin(3 * t + 49d / 31)
                    + 79d / 19 * sin(4 * t + 65d / 41) + 468d / 67 * sin(5 * t + 68d / 43)
                    + 127d / 52 * sin(7 * t + 59d / 37) + 30d / 29 * sin(8 * t + 155d / 33)
                    + 89d / 49 * sin(9 * t + 35d / 22) + 50d / 99 * sin(10 * t + 19d / 12)
                    + 35d / 52 * sin(12 * t + 49d / 30) + 80d / 39 * sin(13 * t + 21d / 13)
                    + 25d / 23 * sin(15 * t + 34d / 21) + 13d / 38 * sin(16 * t + 96d / 61)
                    + 17d / 31 * sin(17 * t + 17d / 10) + 55d / 39 * sin(18 * t + 61d / 38)
                    + 17d / 32 * sin(19 * t + 179d / 38) + 16d / 13 * sin(20 * t + 55d / 34) + 10772d / 39)
                   * theta(75 * pi - t) * theta(t - 71 * pi)
                 + (-sin(54d / 35 - 16 * t) - 17d / 29 * sin(86d / 57 - 12 * t)
                    - 43d / 19 * sin(47d / 30 - 2 * t) + 4197d / 25 * sin(t + 11d / 7)
                    + 904d / 39 * sin(3 * t + 36d / 23) + 6d / 11 * sin(4 * t + 127d / 27)
                    + 209d / 31 * sin(5 * t + 85d / 54) + 12d / 61 * sin(6 * t + 41d / 34)
                    + 143d / 46 * sin(7 * t + 14d / 9) + 28d / 29 * sin(8 * t + 30d / 19)
                    + 63d / 32 * sin(9 * t + 36d / 23) + 17d / 43 * sin(10 * t + 149d / 32)
                    + 29d / 19 * sin(11 * t + 25d / 16) + 5d / 8 * sin(13 * t + 36d / 23)
                    + 21d / 17 * sin(14 * t + 61d / 39) + 28d / 57 * sin(15 * t + 39d / 25)
                    + 19d / 31 * sin(17 * t + 49d / 31) + 25d / 42 * sin(18 * t + 127d / 27)
                    + 17d / 25 * sin(19 * t + 74d / 47) + 5d / 19 * sin(20 * t + 32d / 21)
                    + 62d / 55 * sin(21 * t + 47d / 30) - 14987d / 44) * theta(71 * pi - t)
                   * theta(t - 67 * pi)
                 + (-82d / 31 * sin(35d / 23 - 23 * t) - 165d / 82 * sin(31d / 20 - 22 * t)
                    - 109d / 20 * sin(36d / 23 - 12 * t) - 74d / 11 * sin(14d / 9 - 11 * t)
                    - 12d / 29 * sin(89d / 59 - 10 * t) - 8d / 17 * sin(19d / 13 - 7 * t)
                    - 211d / 35 * sin(14d / 9 - 6 * t) - 59d / 23 * sin(25d / 16 - 5 * t)
                    - 148d / 13 * sin(80d / 51 - 4 * t) - 465d / 13 * sin(36d / 23 - 2 * t)
                    + 2488d / 25 * sin(t + 11d / 7) + 95d / 26 * sin(3 * t + 49d / 31)
                    + 12d / 29 * sin(8 * t + 69d / 44) + 197d / 58 * sin(9 * t + 49d / 31)
                    + 26d / 33 * sin(13 * t + 79d / 17) + 67d / 9 * sin(14 * t + 62d / 39)
                    + 400d / 37 * sin(15 * t + 35d / 22) + 355d / 43 * sin(16 * t + 59d / 37)
                    + 19d / 20 * sin(17 * t + 70d / 43) + 5d / 16 * sin(18 * t + 55d / 32)
                    + 31d / 18 * sin(19 * t + 34d / 21) + 26d / 19 * sin(20 * t + 76d / 47)
                    + 6d / 19 * sin(21 * t + 119d / 26) + 6093d / 19) * theta(67 * pi - t)
                   * theta(t - 63 * pi)
                 + (-73d / 52 * sin(54d / 35 - 23 * t) - 54d / 25 * sin(58d / 37 - 19 * t)
                    - 73d / 20 * sin(14d / 9 - 17 * t) - 5d / 33 * sin(43d / 28 - 14 * t)
                    - 52d / 21 * sin(25d / 16 - 12 * t) + 4675d / 43 * sin(t + 96d / 61)
                    + 353d / 28 * sin(2 * t + 30d / 19) + 3799d / 200 * sin(3 * t + 41d / 26)
                    + 28d / 5 * sin(4 * t + 41d / 26) + 200d / 31 * sin(5 * t + 11d / 7)
                    + 41d / 28 * sin(6 * t + 41d / 26) + 21d / 29 * sin(7 * t + 31d / 20)
                    + 95d / 46 * sin(8 * t + 99d / 62) + 49d / 99 * sin(9 * t + 74d / 47)
                    + 17d / 22 * sin(10 * t + 211d / 45) + 77d / 34 * sin(11 * t + 27d / 17)
                    + 40d / 17 * sin(13 * t + 155d / 97) + 20d / 27 * sin(15 * t + 43d / 26)
                    + 47d / 16 * sin(16 * t + 59d / 37) + 149d / 37 * sin(18 * t + 43d / 27)
                    + 30d / 17 * sin(20 * t + 8d / 5) + 3d / 31 * sin(21 * t + 92d / 41)
                    + 16d / 23 * sin(22 * t + 45d / 29) + 28d / 39 * sin(24 * t + 17d / 11) - 9671d / 24)
                   * theta(63 * pi - t) * theta(t - 59 * pi)
                 + (-278d / 19 * sin(47d / 30 - 3 * t) - 181d / 29 * sin(58d / 37 - 2 * t)
                    - 2421d / 26 * sin(69d / 44 - t) + 9898d / 27) * theta(59 * pi - t)
                   * theta(t - 55 * pi)
                 + (7671d / 67 * sin(t + 107d / 68) + 355d / 36 * sin(2 * t + 11d / 7)
                    + 377d / 24 * sin(3 * t + 49d / 31) - 28766d / 63) * theta(55 * pi - t)
                   * theta(t - 51 * pi)
                 + (-1345d / 28 * sin(80d / 51 - 2 * t) - 359d / 25 * sin(113d / 72 - t)
                    + 259d / 15 * sin(3 * t + 11d / 7) + 5273d / 19) * theta(51 * pi - t)
                   * theta(t - 47 * pi)
                 + (-10399d / 100 * sin(91d / 58 - t) + 1514d / 33 * sin(2 * t + 85d / 54) - 24712d / 67)
                   * theta(47 * pi - t) * theta(t - 43 * pi)
                 + (-14d / 37 * sin(13d / 11 - 10 * t) + 300d / 41 * sin(t + 77d / 36)
                    + 102d / 23 * sin(2 * t + 73d / 110) + 259d / 30 * sin(3 * t + 11d / 4)
                    + 323d / 63 * sin(4 * t + 87d / 28) + 150d / 41 * sin(5 * t + 18d / 25)
                    + 26d / 37 * sin(6 * t + 69d / 19) + 16d / 35 * sin(7 * t + 19d / 25)
                    + 18d / 25 * sin(8 * t + 17d / 4) + 11d / 19 * sin(9 * t + 79d / 28)
                    + 5d / 19 * sin(11 * t + 19d / 10) + 13d / 31 * sin(12 * t + 95d / 21) + 8431d / 36)
                   * theta(43 * pi - t) * theta(t - 39 * pi)
                 + (-3d / 13 * sin(5d / 4 - 12 * t) - 21d / 29 * sin(59d / 38 - 6 * t)
                    - 70d / 11 * sin(26d / 19 - t) + 76d / 11 * sin(2 * t + 19d / 14)
                    + 162d / 29 * sin(3 * t + 203d / 45) + 439d / 73 * sin(4 * t + 124d / 27)
                    + 14d / 33 * sin(5 * t + 38d / 29) + 34d / 33 * sin(7 * t + 24d / 11)
                    + 39d / 46 * sin(8 * t + 94d / 21) + 11d / 40 * sin(9 * t + 13d / 15)
                    + 9d / 19 * sin(10 * t + 229d / 51) + 4d / 35 * sin(11 * t + 22d / 7) - 2147d / 6)
                   * theta(39 * pi - t) * theta(t - 35 * pi)
                 + (769d / 16 * sin(t + 20d / 17) + 49d / 31 * sin(2 * t + 84d / 23)
                    + 129d / 41 * sin(3 * t + 13d / 37) + 11433d / 47) * theta(35 * pi - t)
                   * theta(t - 31 * pi)
                 + (-14d / 17 * sin(22d / 23 - 4 * t) - 21d / 25 * sin(37d / 56 - 2 * t)
                    + 1493d / 31 * sin(t + 48d / 37) + 49d / 27 * sin(3 * t + 5d / 17) - 8077d / 23)
                   * theta(31 * pi - t) * theta(t - 27 * pi)
                 + (2099d / 23 * sin(t + 112d / 67) + 25d / 32 * sin(2 * t + 68d / 23)
                    + 129d / 16 * sin(3 * t + 50d / 27) + 31d / 34 * sin(4 * t + 373d / 112)
                    + 38d / 17 * sin(5 * t + 21d / 11) + 38d / 53 * sin(6 * t + 32d / 9)
                    + 25d / 28 * sin(7 * t + 83d / 46) + 10d / 19 * sin(8 * t + 134d / 35) + 10721d / 42)
                   * theta(27 * pi - t) * theta(t - 23 * pi)
                 + (3072d / 29 * sin(t + 29d / 19) + 9d / 8 * sin(2 * t + 256d / 73)
                    + 254d / 25 * sin(3 * t + 43d / 28) + 19d / 21 * sin(4 * t + 87d / 25)
                    + 95d / 26 * sin(5 * t + 81d / 52) + 21d / 32 * sin(6 * t + 23d / 6)
                    + 86d / 53 * sin(7 * t + 45d / 29) + 23d / 38 * sin(8 * t + 243d / 67) - 3827d / 11)
                   * theta(23 * pi - t) * theta(t - 19 * pi)
                 + (-25d / 17 * sin(36d / 25 - 6 * t) - 99d / 46 * sin(18d / 29 - 4 * t)
                    + 3796d / 29 * sin(t + 43d / 31) + 5d / 2 * sin(2 * t + 5d / 28)
                    + 227d / 35 * sin(3 * t + 26d / 21) + 47d / 20 * sin(5 * t + 43d / 39)
                    + 23d / 17 * sin(7 * t + 11d / 13) + 4572d / 17) * theta(19 * pi - t)
                   * theta(t - 15 * pi)
                 + (6353d / 37 * sin(t + 37d / 21) + 430d / 59 * sin(2 * t + 165d / 37)
                    + 537d / 43 * sin(3 * t + 78d / 31) + 133d / 25 * sin(4 * t + 143d / 32)
                    + 128d / 25 * sin(5 * t + 78d / 23) + 218d / 61 * sin(6 * t + 174d / 37)
                    + 51d / 22 * sin(7 * t + 34d / 9) - 12821d / 38) * theta(15 * pi - t)
                   * theta(t - 11 * pi)
                 + (-36d / 23 * sin(13d / 18 - 12 * t) - 77d / 36 * sin(17d / 30 - 10 * t)
                    - 82d / 31 * sin(17d / 24 - 8 * t) - 255d / 62 * sin(26d / 37 - 6 * t)
                    - 192d / 37 * sin(9d / 16 - 4 * t) - 199d / 40 * sin(21d / 47 - 2 * t)
                    + 5791d / 22 * sin(t + 63d / 41) + 1140d / 47 * sin(3 * t + 34d / 23)
                    + 92d / 13 * sin(5 * t + 29d / 21) + 63d / 22 * sin(7 * t + 19d / 17)
                    + 31d / 28 * sin(9 * t + 21d / 23) + 13d / 19 * sin(11 * t + 54d / 35) - 3874d / 61)
                   * theta(11 * pi - t) * theta(t - 7 * pi)
                 + (-186d / 29 * sin(3d / 2 - 6 * t) - 452d / 43 * sin(81d / 65 - 4 * t)
                    - 487d / 39 * sin(31d / 34 - 2 * t) + 3854d / 13 * sin(t + 102d / 73)
                    + 313d / 17 * sin(3 * t + 31d / 30) + 56d / 23 * sin(5 * t + 4d / 13)
                    + 46d / 33 * sin(7 * t + 33d / 13) + 110d / 27 * sin(8 * t + 57d / 13)
                    + 67d / 38 * sin(9 * t + 71d / 33) + 138d / 59 * sin(10 * t + 101d / 26)
                    + 11d / 8 * sin(11 * t + 46d / 25) + 20d / 23 * sin(12 * t + 103d / 29) - 605d / 13)
                   * theta(7 * pi - t) * theta(t - 3 * pi)
                 + (-320d / 43 * sin(21d / 34 - 6 * t) - 205d / 29 * sin(299d / 298 - 4 * t)
                    + 47179d / 69 * sin(t + 6d / 13) + 307d / 32 * sin(2 * t + 46d / 51)
                    + 1152d / 19 * sin(3 * t + 60d / 37) + 107d / 8 * sin(5 * t + 143d / 37)
                    + 2626d / 175 * sin(7 * t + 45d / 26) + 131d / 22 * sin(8 * t + 143d / 40)
                    + 296d / 47 * sin(9 * t + 500d / 499) + 16d / 7 * sin(10 * t + 93d / 25) - 4117d / 31)
                 * theta(3 * pi - t) * theta(t + pi)) * theta(Math.Sqrt(Math.Sign(sin(t / 2))));
            Func<double, double> yt =
                t =>
                ((-52d / 5 * sin(54d / 37 - 34 * t) - 179d / 56 * sin(46d / 33 - 31 * t)
                  - 513d / 28 * sin(59d / 39 - 25 * t) - 316d / 105 * sin(43d / 28 - 22 * t)
                  - 259d / 16 * sin(102d / 65 - 17 * t) - 26239d / 64 * sin(36d / 23 - 3 * t)
                  - 18953d / 51 * sin(58d / 37 - t) + 11283d / 29 * sin(2 * t + 30d / 19)
                  + 29123d / 35 * sin(4 * t + 49d / 31) + 6877d / 14 * sin(5 * t + 65d / 41)
                  + 117d / 50 * sin(6 * t + 103d / 51) + 587d / 13 * sin(7 * t + 164d / 35)
                  + 102d / 13 * sin(8 * t + 236d / 51) + 975d / 14 * sin(9 * t + 101d / 63)
                  + 122d / 41 * sin(10 * t + 79d / 44) + 1900d / 139 * sin(11 * t + 117d / 25)
                  + 597d / 22 * sin(12 * t + 31d / 19) + 1219d / 39 * sin(13 * t + 8d / 5)
                  + 552d / 13 * sin(14 * t + 35d / 22) + 621d / 26 * sin(15 * t + 31d / 19)
                  + 705d / 28 * sin(16 * t + 212d / 45) + 579d / 11 * sin(18 * t + 34d / 21)
                  + 19d / 27 * sin(19 * t + 190d / 43) + 15d / 29 * sin(20 * t + 89d / 20)
                  + 497d / 23 * sin(21 * t + 128d / 77) + 205d / 51 * sin(23 * t + 57d / 34)
                  + 317d / 35 * sin(24 * t + 68d / 39) + 17d / 4 * sin(26 * t + 202d / 43)
                  + 927d / 34 * sin(27 * t + 33d / 20) + 31d / 23 * sin(28 * t + 154d / 123)
                  + 37d / 6 * sin(29 * t + 28d / 17) + 159d / 19 * sin(30 * t + 22d / 13)
                  + 87d / 23 * sin(32 * t + 82d / 49) + 57d / 28 * sin(33 * t + 103d / 52)
                  + 77d / 29 * sin(35 * t + 116d / 25) + 489d / 41 * sin(36 * t + 49d / 29)
                  + 61d / 30 * sin(37 * t + 62d / 39) + 313d / 47 * sin(38 * t + 69d / 40)
                  + 36d / 11 * sin(39 * t + 41d / 23) + 90d / 91 * sin(40 * t + 7d / 4)
                  + 47d / 19 * sin(41 * t + 101d / 63) - 8810d / 27) * theta(119 * pi - t)
                 * theta(t - 115 * pi)
                 + (-91d / 46 * sin(11d / 8 - 62 * t) - 59d / 28 * sin(39d / 34 - 61 * t)
                    - 11d / 2 * sin(89d / 67 - 60 * t) - 15d / 8 * sin(32d / 35 - 59 * t)
                    - 53d / 22 * sin(48d / 35 - 58 * t) - 175d / 176 * sin(13d / 12 - 54 * t)
                    - 71d / 15 * sin(30d / 23 - 53 * t) - 166d / 27 * sin(25d / 18 - 52 * t)
                    - 48d / 29 * sin(20d / 31 - 51 * t) - 547d / 114 * sin(43d / 31 - 50 * t)
                    - 271d / 41 * sin(51d / 37 - 46 * t) - 141d / 20 * sin(42d / 31 - 45 * t)
                    - 129d / 13 * sin(37d / 26 - 44 * t) - 164d / 23 * sin(37d / 26 - 41 * t)
                    - 451d / 52 * sin(31d / 21 - 40 * t) - 27d / 22 * sin(63d / 44 - 38 * t)
                    - 83d / 7 * sin(35d / 24 - 36 * t) - 874d / 93 * sin(7d / 5 - 35 * t)
                    - 117d / 22 * sin(22d / 17 - 34 * t) - 911d / 69 * sin(3d / 2 - 32 * t)
                    - 109d / 37 * sin(27d / 19 - 31 * t) - 31d / 13 * sin(16d / 19 - 27 * t)
                    - 383d / 10 * sin(64d / 43 - 26 * t) - 455d / 19 * sin(31d / 21 - 24 * t)
                    - 764d / 17 * sin(58d / 39 - 18 * t) - 5417d / 48 * sin(74d / 49 - 17 * t)
                    - 1557d / 35 * sin(80d / 53 - 16 * t) - 3366d / 25 * sin(71d / 46 - 12 * t)
                    - 2267d / 100 * sin(91d / 61 - 11 * t) - 1523d / 27 * sin(239d / 159 - 10 * t)
                    - 7929d / 44 * sin(36d / 23 - 7 * t) + 32716d / 55 * sin(t + 146d / 31)
                    + 26497d / 40 * sin(2 * t + 30d / 19) + 28971d / 35 * sin(3 * t + 19d / 12)
                    + 12399d / 44 * sin(4 * t + 117d / 73) + 3783d / 29 * sin(5 * t + 145d / 31)
                    + 21737d / 40 * sin(6 * t + 46d / 29) + 132d / 7 * sin(8 * t + 23d / 14)
                    + 887d / 32 * sin(9 * t + 25d / 16) + 963d / 14 * sin(13 * t + 67d / 41)
                    + 972d / 11 * sin(14 * t + 47d / 29) + 5575d / 77 * sin(15 * t + 53d / 33)
                    + 382d / 25 * sin(19 * t + 47d / 32) + 855d / 44 * sin(20 * t + 28d / 17)
                    + 701d / 42 * sin(21 * t + 33d / 20) + 109d / 23 * sin(22 * t + 14d / 3)
                    + 197d / 38 * sin(23 * t + 43d / 29) + 118d / 37 * sin(25 * t + 37d / 38)
                    + 272d / 31 * sin(28 * t + 44d / 27) + 5d / 23 * sin(29 * t + 25d / 26)
                    + 195d / 28 * sin(30 * t + 47d / 28) + 185d / 24 * sin(33 * t + 57d / 37)
                    + 103d / 13 * sin(37 * t + 46d / 29) + 43d / 9 * sin(39 * t + 41d / 26)
                    + 257d / 32 * sin(42 * t + 51d / 32) + 33d / 13 * sin(43 * t + 41d / 30)
                    + 67d / 15 * sin(47 * t + 31d / 19) + 39d / 14 * sin(48 * t + 44d / 25)
                    + 88d / 31 * sin(49 * t + 30d / 19) + 113d / 68 * sin(55 * t + 103d / 69)
                    + 1d / 54 * sin(56 * t + 197d / 58) + 79d / 24 * sin(57 * t + 167d / 100) + 287d / 29)
                   * theta(115 * pi - t) * theta(t - 111 * pi)
                 + (-8d / 25 * sin(17d / 31 - 90 * t) - 144d / 37 * sin(29d / 19 - 89 * t)
                    - 102d / 47 * sin(37d / 24 - 88 * t) - 74d / 43 * sin(24d / 17 - 87 * t)
                    - 95d / 17 * sin(25d / 16 - 49 * t) - 137d / 27 * sin(16d / 11 - 27 * t)
                    + 21445d / 52 * sin(t + 69d / 44) + 24223d / 27 * sin(2 * t + 113d / 24)
                    + 23911d / 28 * sin(3 * t + 146d / 31) + 2675d / 8 * sin(4 * t + 64d / 41)
                    + 5772d / 19 * sin(5 * t + 36d / 23) + 3258d / 17 * sin(6 * t + 11d / 7)
                    + 5245d / 37 * sin(7 * t + 174d / 37) + 355d / 16 * sin(8 * t + 117d / 73)
                    + 225d / 8 * sin(9 * t + 88d / 19) + 3255d / 26 * sin(10 * t + 11d / 7)
                    + 1137d / 19 * sin(11 * t + 39d / 25) + 1310d / 21 * sin(12 * t + 8d / 5)
                    + 943d / 21 * sin(13 * t + 29d / 18) + 9679d / 81 * sin(14 * t + 80d / 51)
                    + 113d / 20 * sin(15 * t + 47d / 28) + 1342d / 47 * sin(16 * t + 86d / 53)
                    + 3679d / 84 * sin(17 * t + 55d / 36) + 353d / 20 * sin(18 * t + 50d / 11)
                    + 939d / 50 * sin(19 * t + 201d / 43) + 4789d / 82 * sin(20 * t + 48d / 31)
                    + 2649d / 85 * sin(21 * t + 303d / 65) + 1753d / 59 * sin(22 * t + 36d / 23)
                    + 268d / 29 * sin(23 * t + 58d / 39) + 55d / 16 * sin(24 * t + 82d / 43)
                    + 228d / 37 * sin(25 * t + 353d / 235) + 8d / 25 * sin(26 * t + 49d / 31)
                    + 422d / 25 * sin(28 * t + 93d / 20) + 211d / 21 * sin(29 * t + 38d / 27)
                    + 60d / 41 * sin(30 * t + 131d / 29) + 162d / 11 * sin(31 * t + 65d / 14)
                    + 2489d / 63 * sin(32 * t + 35d / 23) + 688d / 27 * sin(33 * t + 129d / 28)
                    + 1447d / 62 * sin(34 * t + 55d / 36) + 233d / 26 * sin(35 * t + 85d / 57)
                    + 259d / 26 * sin(36 * t + 95d / 21) + 149d / 10 * sin(37 * t + 25d / 17)
                    + 235d / 57 * sin(38 * t + 50d / 11) + 185d / 38 * sin(39 * t + 183d / 41)
                    + 125d / 13 * sin(40 * t + 68d / 45) + 223d / 31 * sin(41 * t + 39d / 25)
                    + 61d / 12 * sin(42 * t + 355d / 79) + 1262d / 99 * sin(43 * t + 160d / 107)
                    + 309d / 40 * sin(44 * t + 41d / 26) + 307d / 38 * sin(45 * t + 221d / 48)
                    + 49d / 5 * sin(46 * t + 137d / 91) + 193d / 31 * sin(47 * t + 190d / 41)
                    + 77d / 54 * sin(48 * t + 95d / 63) + 20d / 33 * sin(50 * t + 148d / 51)
                    + 256d / 47 * sin(51 * t + 53d / 39) + 133d / 37 * sin(52 * t + 92d / 21)
                    + 19d / 7 * sin(53 * t + 121d / 81) + 39d / 4 * sin(54 * t + 71d / 47)
                    + 341d / 85 * sin(55 * t + 113d / 25) + 71d / 31 * sin(56 * t + 115d / 67)
                    + 31d / 10 * sin(57 * t + 36d / 25) + 11d / 5 * sin(58 * t + 30d / 7)
                    + 62d / 23 * sin(59 * t + 33d / 23) + 40d / 33 * sin(60 * t + 67d / 36)
                    + 13d / 40 * sin(61 * t + 32d / 19) + 47d / 6 * sin(62 * t + 80d / 53)
                    + 48d / 11 * sin(63 * t + 73d / 16) + 301d / 47 * sin(64 * t + 56d / 37)
                    + 361d / 120 * sin(65 * t + 191d / 41) + 59d / 31 * sin(66 * t + 83d / 52)
                    + 132d / 25 * sin(67 * t + 117d / 25) + 37d / 13 * sin(68 * t + 4d / 3)
                    + 167d / 35 * sin(69 * t + 183d / 40) + 67d / 41 * sin(70 * t + 4d / 3)
                    + 103d / 42 * sin(71 * t + 49d / 34) + 199d / 71 * sin(72 * t + 101d / 23)
                    + 200d / 31 * sin(73 * t + 77d / 53) + 81d / 58 * sin(74 * t + 169d / 38)
                    + 17d / 21 * sin(75 * t + 83d / 20) + 27d / 38 * sin(76 * t + 79d / 59)
                    + 59d / 29 * sin(77 * t + 47d / 33) + 175d / 44 * sin(78 * t + 114d / 25)
                    + 54d / 19 * sin(79 * t + 63d / 43) + 19d / 27 * sin(80 * t + 61d / 27)
                    + 149d / 26 * sin(81 * t + 16d / 11) + 68d / 29 * sin(82 * t + 102d / 25)
                    + 313d / 68 * sin(83 * t + 54d / 35) + 147d / 23 * sin(84 * t + 47d / 32)
                    + 73d / 12 * sin(85 * t + 107d / 24) + 191d / 51 * sin(86 * t + 38d / 27)
                    + 37d / 28 * sin(91 * t + 173d / 37) + 70d / 43 * sin(92 * t + 35d / 23)
                    + 44d / 35 * sin(93 * t + 53d / 13) + 164d / 35 * sin(94 * t + 107d / 67)
                    + 141d / 34 * sin(95 * t + 37d / 25) + 19d / 12 * sin(96 * t + 83d / 21)
                    + 22d / 23 * sin(97 * t + 47d / 31) + 152d / 39 * sin(98 * t + 60d / 43)
                    + 123d / 28 * sin(99 * t + 132d / 29) - 15137d / 22) * theta(111 * pi - t)
                   * theta(t - 107 * pi)
                 + (-43d / 40 * sin(102d / 65 - 45 * t) - 74d / 41 * sin(25d / 27 - 35 * t)
                    - 47d / 19 * sin(15d / 16 - 34 * t) - 67d / 32 * sin(21d / 25 - 33 * t)
                    - 161d / 39 * sin(27d / 20 - 32 * t) - 12d / 23 * sin(17d / 12 - 29 * t)
                    - 29d / 57 * sin(45d / 37 - 27 * t) - 19d / 16 * sin(117d / 88 - 25 * t)
                    - 11d / 36 * sin(15d / 22 - 24 * t) - 11d / 30 * sin(33d / 82 - 23 * t)
                    - 43d / 57 * sin(14d / 17 - 22 * t) - 87d / 65 * sin(49d / 41 - 21 * t)
                    - 50d / 21 * sin(64d / 47 - 20 * t) - 17d / 59 * sin(56d / 55 - 6 * t)
                    - 56d / 61 * sin(77d / 58 - 5 * t) + 127d / 28 * sin(t + 149d / 32)
                    + 15d / 2 * sin(2 * t + 21d / 13) + 35d / 29 * sin(3 * t + 49d / 29)
                    + 1d / 9 * sin(4 * t + 51d / 43) + 37d / 38 * sin(7 * t + 19d / 12)
                    + 18d / 31 * sin(8 * t + 57d / 13) + 38d / 31 * sin(9 * t + 55d / 12)
                    + 7d / 10 * sin(10 * t + 74d / 45) + 9d / 34 * sin(11 * t + 69d / 34)
                    + 39d / 19 * sin(12 * t + 41d / 26) + 89d / 26 * sin(13 * t + 67d / 42)
                    + 63d / 26 * sin(14 * t + 107d / 23) + 119d / 16 * sin(15 * t + 23d / 5)
                    + 311d / 18 * sin(16 * t + 56d / 33) + 1929d / 386 * sin(17 * t + 99d / 50)
                    + 79d / 41 * sin(18 * t + 64d / 29) + 34d / 41 * sin(19 * t + 67d / 35)
                    + 16d / 41 * sin(26 * t + 49d / 33) + 1d / 34 * sin(28 * t + 5d / 14)
                    + 36d / 43 * sin(30 * t + 193d / 41) + 173d / 42 * sin(31 * t + 30d / 19)
                    + 15d / 19 * sin(36 * t + 31d / 28) + 2d / 19 * sin(37 * t + 87d / 43)
                    + 22d / 39 * sin(38 * t + 33d / 14) + 13d / 23 * sin(39 * t + 19d / 6)
                    + 21d / 19 * sin(40 * t + 1641d / 820) + 29d / 28 * sin(41 * t + 172d / 39)
                    + 39d / 34 * sin(42 * t + 58d / 31) + 16d / 29 * sin(43 * t + 50d / 23)
                    + 24d / 25 * sin(44 * t + 41d / 23) + 17d / 23 * sin(46 * t + 289d / 62)
                    + 37d / 25 * sin(47 * t + 30d / 17) + 17d / 29 * sin(48 * t + 239d / 52)
                    + 29d / 73 * sin(49 * t + 41d / 26) - 8044d / 23) * theta(107 * pi - t)
                   * theta(t - 103 * pi)
                 + (-33d / 29 * sin(7d / 6 - 53 * t) - 67d / 44 * sin(62d / 45 - 51 * t)
                    - 32d / 65 * sin(1d / 14 - 50 * t) - 151d / 49 * sin(54d / 41 - 49 * t)
                    - 63d / 38 * sin(29d / 23 - 47 * t) - 59d / 103 * sin(14d / 23 - 46 * t)
                    - 11d / 30 * sin(8d / 23 - 45 * t) - 151d / 32 * sin(99d / 79 - 44 * t)
                    - 53d / 26 * sin(45d / 38 - 43 * t) - 67d / 27 * sin(38d / 29 - 36 * t)
                    - 113d / 32 * sin(39d / 29 - 35 * t) - 182d / 51 * sin(81d / 58 - 33 * t)
                    - 391d / 65 * sin(33d / 23 - 29 * t) - 48d / 13 * sin(31d / 20 - 25 * t)
                    - 59d / 32 * sin(24d / 17 - 19 * t) - 90d / 91 * sin(41d / 33 - 18 * t)
                    - 863d / 54 * sin(35d / 24 - 17 * t) - 384d / 43 * sin(73d / 51 - 16 * t)
                    - 149d / 26 * sin(19d / 13 - 13 * t) - 90d / 19 * sin(65d / 43 - 12 * t)
                    + 10d / 29 * sin(t + 47d / 32) + 15d / 17 * sin(2 * t + 77d / 45)
                    + 300d / 37 * sin(3 * t + 62d / 39) + 4d / 13 * sin(4 * t + 8d / 5)
                    + 7d / 29 * sin(5 * t + 109d / 25) + 46d / 27 * sin(6 * t + 49d / 30)
                    + 39d / 23 * sin(7 * t + 30d / 19) + 64d / 47 * sin(8 * t + 72d / 43)
                    + 19d / 20 * sin(9 * t + 63d / 37) + 14d / 5 * sin(10 * t + 41d / 25)
                    + 37d / 14 * sin(11 * t + 27d / 16) + 207d / 28 * sin(14 * t + 248d / 149)
                    + 288d / 25 * sin(15 * t + 58d / 35) + 123d / 52 * sin(20 * t + 85d / 46)
                    + 194d / 13 * sin(21 * t + 17d / 10) + 64d / 13 * sin(22 * t + 103d / 22)
                    + 335d / 27 * sin(23 * t + 31d / 18) + 103d / 18 * sin(24 * t + 13d / 7)
                    + 83d / 20 * sin(26 * t + 117d / 67) + 243d / 52 * sin(27 * t + 36d / 19)
                    + 1907d / 477 * sin(28 * t + 125d / 73) + 73d / 30 * sin(30 * t + 182d / 109)
                    + 182d / 109 * sin(31 * t + 43d / 23) + 53d / 23 * sin(32 * t + 147d / 92)
                    + 21d / 43 * sin(34 * t + 145d / 144) + 20d / 17 * sin(37 * t + 29d / 14)
                    + 110d / 27 * sin(38 * t + 79d / 44) + 22d / 27 * sin(39 * t + 74d / 17)
                    + 99d / 37 * sin(40 * t + 52d / 29) + 36d / 25 * sin(41 * t + 255d / 128)
                    + 11d / 15 * sin(42 * t + 124d / 69) + 31d / 17 * sin(48 * t + 67d / 41)
                    + 15d / 14 * sin(52 * t + 73d / 47) - 10105d / 36) * theta(103 * pi - t)
                   * theta(t - 99 * pi)
                 + (3683d / 48 * sin(t + 74d / 47) + 41d / 26 * sin(2 * t + 103d / 22)
                    + 149d / 17 * sin(3 * t + 85d / 54) + 6d / 13 * sin(4 * t + 51d / 11)
                    + 112d / 43 * sin(5 * t + 11d / 7) + 17294d / 53) * theta(99 * pi - t)
                   * theta(t - 95 * pi)
                 + (-19d / 40 * sin(29d / 20 - 11 * t) - 289d / 38 * sin(36d / 23 - 9 * t)
                    + 802d / 33 * sin(t + 179d / 38) + 3002d / 35 * sin(2 * t + 85d / 54)
                    + 159d / 41 * sin(3 * t + 61d / 39) + 1321d / 44 * sin(4 * t + 52d / 33)
                    + 131d / 22 * sin(5 * t + 113d / 24) + 52d / 35 * sin(6 * t + 69d / 44)
                    + 321d / 37 * sin(7 * t + 36d / 23) + 789d / 158 * sin(8 * t + 36d / 23)
                    + 56d / 113 * sin(10 * t + 50d / 33) + 237d / 118 * sin(12 * t + 39d / 25) - 703d / 42)
                   * theta(95 * pi - t) * theta(t - 91 * pi)
                 + (-121d / 68 * sin(25d / 16 - 4 * t) - 55d / 27 * sin(81d / 52 - 3 * t)
                    - 613d / 68 * sin(25d / 16 - 2 * t) - 505d / 19 * sin(47d / 30 - t) + 16067d / 23)
                   * theta(91 * pi - t) * theta(t - 87 * pi)
                 + (-83d / 19 * sin(47d / 30 - 5 * t) + 56d / 25 * sin(t + 193d / 41)
                    + 35d / 29 * sin(2 * t + 146d / 31) + 127d / 25 * sin(3 * t + 74d / 47)
                    + 143d / 35 * sin(4 * t + 63d / 40) + 33d / 50 * sin(6 * t + 107d / 68) + 17679d / 28)
                   * theta(87 * pi - t) * theta(t - 83 * pi)
                 + (-35d / 32 * sin(31d / 20 - 11 * t) - 55d / 48 * sin(61d / 39 - 9 * t)
                    - 30d / 17 * sin(50d / 33 - 6 * t) - 247d / 17 * sin(64d / 41 - 5 * t)
                    - 471d / 35 * sin(58d / 37 - 3 * t) + 405d / 23 * sin(t + 41d / 26)
                    + 829d / 69 * sin(2 * t + 11d / 7) + 279d / 17 * sin(4 * t + 30d / 19)
                    + 185d / 27 * sin(7 * t + 93d / 58) + 92d / 25 * sin(8 * t + 43d / 27)
                    + 1d / 17 * sin(10 * t + 56d / 31) + 64d / 35 * sin(12 * t + 37d / 23) + 3306d / 17)
                   * theta(83 * pi - t) * theta(t - 79 * pi)
                 + (-143d / 24 * sin(36d / 23 - t) + 9d / 22 * sin(2 * t + 47d / 10)
                    + 31d / 42 * sin(3 * t + 28d / 17) + 1287d / 103 * sin(4 * t + 30d / 19)
                    + 626d / 51 * sin(5 * t + 41d / 26) + 173d / 31 * sin(6 * t + 113d / 24)
                    + 138d / 19 * sin(7 * t + 19d / 12) + 15d / 4 * sin(8 * t + 27d / 17)
                    + 39d / 34 * sin(9 * t + 8d / 5) + 11d / 18 * sin(10 * t + 202d / 43)
                    + 23d / 25 * sin(11 * t + 71d / 44) + 133d / 34 * sin(12 * t + 27d / 17) + 19064d / 89)
                   * theta(79 * pi - t) * theta(t - 75 * pi)
                 + (-69d / 38 * sin(44d / 29 - 19 * t) - 49d / 17 * sin(119d / 79 - 16 * t)
                    - 65d / 23 * sin(83d / 55 - 11 * t) - 149d / 35 * sin(32d / 21 - 10 * t)
                    - 311d / 56 * sin(14d / 9 - 8 * t) - 83d / 40 * sin(20d / 13 - 7 * t)
                    - 35d / 6 * sin(57d / 37 - 6 * t) - 55d / 19 * sin(57d / 37 - 5 * t)
                    - 93d / 19 * sin(39d / 25 - 4 * t) - 87d / 8 * sin(39d / 25 - 3 * t)
                    - 517d / 30 * sin(25d / 16 - 2 * t) + 387d / 28 * sin(t + 91d / 58)
                    + 68d / 33 * sin(9 * t + 27d / 17) + 67d / 34 * sin(12 * t + 76d / 47)
                    + 13d / 20 * sin(13 * t + 45d / 26) + 2d / 25 * sin(14 * t + 127d / 28)
                    + 131d / 56 * sin(15 * t + 61d / 38) + 62d / 29 * sin(17 * t + 188d / 113)
                    + 26d / 37 * sin(18 * t + 77d / 46) + 8d / 33 * sin(20 * t + 43d / 38) + 5327d / 11)
                   * theta(75 * pi - t) * theta(t - 71 * pi)
                 + (-21d / 23 * sin(98d / 65 - 16 * t) - 22d / 23 * sin(47d / 30 - 9 * t)
                    - 13d / 19 * sin(25d / 16 - 7 * t) + 28d / 57 * sin(t + 87d / 56)
                    + 1265d / 47 * sin(2 * t + 179d / 38) + 223d / 20 * sin(3 * t + 11d / 7)
                    + 227d / 19 * sin(4 * t + 179d / 38) + 45d / 22 * sin(5 * t + 11d / 7)
                    + 188d / 53 * sin(6 * t + 127d / 27) + 116d / 35 * sin(8 * t + 179d / 38)
                    + 19d / 12 * sin(10 * t + 27d / 17) + 8d / 11 * sin(11 * t + 19d / 12)
                    + 29d / 13 * sin(12 * t + 179d / 38) + 69d / 43 * sin(13 * t + 25d / 16)
                    + 277d / 278 * sin(14 * t + 107d / 23) + 31d / 27 * sin(15 * t + 127d / 27)
                    + 63d / 22 * sin(17 * t + 155d / 33) + 13d / 19 * sin(18 * t + 43d / 27)
                    + 107d / 89 * sin(19 * t + 103d / 22) + 13d / 12 * sin(20 * t + 61d / 13)
                    + 22d / 17 * sin(21 * t + 39d / 25) + 8121d / 16) * theta(71 * pi - t)
                   * theta(t - 67 * pi)
                 + (-89d / 18 * sin(68d / 45 - 23 * t) - 27d / 17 * sin(26d / 17 - 20 * t)
                    - 109d / 39 * sin(73d / 47 - 18 * t) - 142d / 57 * sin(26d / 17 - 17 * t)
                    - 146d / 27 * sin(45d / 29 - 14 * t) - 43d / 9 * sin(31d / 20 - 13 * t)
                    - 164d / 33 * sin(95d / 61 - 12 * t) - 91d / 34 * sin(43d / 28 - 11 * t)
                    - 30d / 19 * sin(29d / 19 - 9 * t) - 55d / 17 * sin(59d / 38 - 8 * t)
                    - 183d / 29 * sin(64d / 41 - 6 * t) - 137d / 25 * sin(53d / 34 - 5 * t)
                    - 124d / 19 * sin(36d / 23 - 4 * t) - 655d / 44 * sin(47d / 30 - 2 * t)
                    - 472d / 13 * sin(58d / 37 - t) + 43d / 57 * sin(3 * t + 70d / 43)
                    + 41d / 12 * sin(7 * t + 85d / 54) + 29d / 115 * sin(10 * t + 38d / 23)
                    + 27d / 7 * sin(15 * t + 19d / 12) + 21d / 20 * sin(16 * t + 36d / 23)
                    + 26d / 51 * sin(19 * t + 36d / 23) + 2d / 31 * sin(21 * t + 67d / 42)
                    + 59d / 26 * sin(22 * t + 31d / 19) + 8662d / 25) * theta(67 * pi - t)
                   * theta(t - 63 * pi)
                 + (-40d / 27 * sin(86d / 57 - 24 * t) - 5d / 33 * sin(4d / 15 - 23 * t)
                    - 107d / 27 * sin(57d / 37 - 22 * t) - 163d / 12 * sin(37d / 24 - 18 * t)
                    - 80d / 43 * sin(73d / 47 - 15 * t) - 245d / 57 * sin(31d / 20 - 13 * t)
                    - 22d / 29 * sin(43d / 28 - 10 * t) - 99d / 20 * sin(59d / 38 - 8 * t)
                    - 18d / 11 * sin(59d / 38 - 7 * t) - 35d / 16 * sin(58d / 37 - 6 * t)
                    - 81d / 161 * sin(41d / 28 - 3 * t) - 271d / 13 * sin(58d / 37 - 2 * t)
                    + 148d / 7 * sin(t + 85d / 54) + 98d / 25 * sin(4 * t + 146d / 31)
                    + 67d / 24 * sin(5 * t + 30d / 19) + 8d / 25 * sin(9 * t + 179d / 38)
                    + 5d / 9 * sin(11 * t + 131d / 28) + 46d / 51 * sin(12 * t + 32d / 21)
                    + 108d / 31 * sin(14 * t + 69d / 43) + 149d / 29 * sin(16 * t + 45d / 28)
                    + 59d / 15 * sin(17 * t + 46d / 29) + 134d / 21 * sin(19 * t + 125d / 78)
                    + 158d / 27 * sin(20 * t + 44d / 27) + 35d / 29 * sin(21 * t + 31d / 19) + 9838d / 25)
                   * theta(63 * pi - t) * theta(t - 59 * pi)
                 + (-385d / 16 * sin(47d / 30 - 3 * t) - 21082d / 91 * sin(69d / 44 - t)
                    + 21d / 13 * sin(2 * t + 8d / 5) - 31214d / 91) * theta(59 * pi - t)
                   * theta(t - 55 * pi)
                 + (-860d / 41 * sin(39d / 25 - 3 * t) - 5675d / 26 * sin(69d / 44 - t)
                    + 265d / 43 * sin(2 * t + 46d / 29) - 33516d / 73) * theta(55 * pi - t)
                   * theta(t - 51 * pi)
                 + (-621d / 28 * sin(36d / 23 - 3 * t) - 244d / 31 * sin(91d / 58 - 2 * t)
                    - 4739d / 19 * sin(102d / 65 - t) - 20129d / 76) * theta(51 * pi - t)
                   * theta(t - 47 * pi)
                 + (-504d / 31 * sin(25d / 16 - 2 * t) - 6167d / 31 * sin(80d / 51 - t) - 4810d / 29)
                   * theta(47 * pi - t) * theta(t - 43 * pi)
                 + (-1d / 14 * sin(9d / 11 - 12 * t) - 21d / 41 * sin(29d / 25 - 11 * t)
                    - 7d / 10 * sin(48d / 35 - 9 * t) + 157d / 25 * sin(t + 118d / 41)
                    + 92d / 41 * sin(2 * t + 110d / 39) + 571d / 84 * sin(3 * t + 101d / 23)
                    + 146d / 37 * sin(4 * t + 211d / 48) + 71d / 34 * sin(5 * t + 45d / 26)
                    + 7d / 31 * sin(6 * t + 4d / 3) + 7d / 13 * sin(7 * t + 44d / 25)
                    + 9d / 11 * sin(8 * t + 37d / 53) + 12d / 29 * sin(10 * t + 31d / 18) + 16173d / 50)
                   * theta(43 * pi - t) * theta(t - 39 * pi)
                 + (-25d / 29 * sin(1d / 110 - 10 * t) - 81d / 71 * sin(11d / 29 - 6 * t)
                    - 101d / 19 * sin(16d / 49 - 4 * t) - 81d / 19 * sin(11d / 43 - 3 * t)
                    + 167d / 43 * sin(t + 175d / 41) + 181d / 37 * sin(2 * t + 58d / 17)
                    + 6d / 5 * sin(5 * t + 61d / 14) + 25d / 22 * sin(7 * t + 3)
                    + 37d / 30 * sin(8 * t + 1d / 25) + 31d / 40 * sin(9 * t + 61d / 22)
                    + 9d / 35 * sin(11 * t + 103d / 30) + 14d / 19 * sin(12 * t + 1d / 10) + 8453d / 25)
                   * theta(39 * pi - t) * theta(t - 35 * pi)
                 + (-151d / 36 * sin(17d / 23 - 3 * t) - 331d / 12 * sin(8d / 31 - t)
                    + 184d / 47 * sin(2 * t + 160d / 39) + 324) * theta(35 * pi - t) * theta(t - 31 * pi)
                 + (-50d / 19 * sin(6d / 17 - 3 * t) - 1333d / 36 * sin(1d / 5 - t)
                    + 23d / 19 * sin(2 * t + 23d / 9) + 29d / 36 * sin(4 * t + 67d / 24) + 13271d / 39)
                   * theta(31 * pi - t) * theta(t - 27 * pi)
                 + (-119d / 22 * sin(27d / 19 - 2 * t) + 1265d / 36 * sin(t + 4d / 29)
                    + 98d / 15 * sin(3 * t + 3d / 5) + 89d / 48 * sin(4 * t + 142d / 31)
                    + 55d / 19 * sin(5 * t + 32d / 37) + 16d / 17 * sin(6 * t + 89d / 25)
                    + 21d / 16 * sin(7 * t + 23d / 40) + 8d / 13 * sin(8 * t + 37d / 12) + 10562d / 33)
                   * theta(27 * pi - t) * theta(t - 23 * pi)
                 + (-71d / 46 * sin(1d / 11 - 7 * t) - 115d / 64 * sin(5d / 19 - 5 * t)
                    - 11d / 28 * sin(16d / 25 - 4 * t) - 44d / 17 * sin(33d / 43 - 3 * t)
                    - 1265d / 29 * sin(2d / 13 - t) + 59d / 16 * sin(2 * t + 211d / 53)
                    + 12d / 25 * sin(6 * t + 57d / 20) + 26d / 51 * sin(8 * t + 65d / 17) + 11467d / 34)
                   * theta(23 * pi - t) * theta(t - 19 * pi)
                 + (-23d / 41 * sin(81d / 58 - 5 * t) + 4367d / 48 * sin(t + 111d / 38)
                    + 49d / 16 * sin(2 * t + 173d / 39) + 134d / 45 * sin(3 * t + 70d / 29)
                    + 64d / 37 * sin(4 * t + 41d / 17) + 21d / 46 * sin(6 * t + 29d / 13)
                    + 10d / 19 * sin(7 * t + 53d / 37) + 10855d / 34) * theta(19 * pi - t)
                   * theta(t - 15 * pi)
                 + (-127d / 13 * sin(7d / 18 - 3 * t) + 2713d / 29 * sin(t + 2d / 37)
                    + 101d / 13 * sin(2 * t + 203d / 45) + 89d / 34 * sin(4 * t + 73d / 17)
                    + 26d / 15 * sin(5 * t + 26d / 41) + 2d / 9 * sin(6 * t + 177d / 50)
                    + 35d / 71 * sin(7 * t + 66d / 31) + 10805d / 33) * theta(15 * pi - t)
                   * theta(t - 11 * pi)
                 + (-20d / 23 * sin(38d / 29 - 11 * t) - 31d / 18 * sin(46d / 37 - 9 * t)
                    - 497d / 108 * sin(9d / 16 - 5 * t) - 2375d / 26 * sin(3d / 29 - t)
                    + 970d / 41 * sin(2 * t + 35d / 32) + 1011d / 92 * sin(3 * t + 7d / 38)
                    + 131d / 27 * sin(4 * t + 91d / 90) + 24d / 71 * sin(6 * t + 34d / 37)
                    + 90d / 29 * sin(7 * t + 14d / 23) + 21d / 16 * sin(8 * t + 25d / 12)
                    + 21d / 38 * sin(10 * t + 89d / 19) + 7d / 27 * sin(12 * t + 68d / 15) - 10719d / 37)
                   * theta(11 * pi - t) * theta(t - 7 * pi)
                 + (-11d / 37 * sin(4d / 7 - 6 * t) - 299d / 56 * sin(24d / 19 - 5 * t)
                    - 499d / 38 * sin(5d / 44 - 3 * t) - 6150d / 37 * sin(12d / 59 - t)
                    + 1019d / 38 * sin(2 * t + 28d / 37) + 183d / 53 * sin(4 * t + 85d / 71)
                    + 95d / 43 * sin(7 * t + 169d / 38) + 29d / 27 * sin(8 * t + 111d / 40)
                    + 21d / 22 * sin(9 * t + 16d / 29) + 194d / 83 * sin(10 * t + 164d / 57)
                    + 31d / 30 * sin(11 * t + 1d / 53) + 17d / 31 * sin(12 * t + 77d / 29) - 24819d / 82)
                   * theta(7 * pi - t) * theta(t - 3 * pi)
                 + (-14d / 13 * sin(4d / 45 - 10 * t) - 229d / 26 * sin(16d / 23 - 7 * t)
                    - 222d / 17 * sin(28d / 23 - 5 * t) - 851d / 29 * sin(10d / 51 - 3 * t)
                    - 12070d / 13 * sin(12d / 11 - t) + 270d / 31 * sin(2 * t + 62d / 15)
                    + 53d / 7 * sin(4 * t + 400d / 87) + 59d / 10 * sin(6 * t + 12d / 13)
                    + 141d / 31 * sin(8 * t + 30d / 17) + 79d / 40 * sin(9 * t + 121d / 26) + 5838d / 29)
                 * theta(3 * pi - t) * theta(t + pi)) * theta(Math.Sqrt(Math.Sign(sin(t / 2))));

            var model = new PlotModel { Title = "Angelina Jolie curve", PlotType = PlotType.Cartesian };
            var fs = new FunctionSeries(xt, yt, 0, Math.PI * 120, 10000);
            model.Series.Add(fs);

            // Insert breaks at discontinuities
            // TODO: this should be improved...
            for (int i = 0; i + 1 < fs.Points.Count; i++)
            {
                var dx = fs.Points[i + 1].X - fs.Points[i].X;
                var dy = fs.Points[i + 1].Y - fs.Points[i].Y;
                if ((dx * dx) + (dy * dy) > 100000)
                {
                    fs.Points.Insert(i + 1, new DataPoint(double.NaN, double.NaN));
                    i++;
                }
            }

            return model;
        }

        /// <summary>
        /// Gets the stream for the specified embedded resource.
        /// </summary>
        /// <param name="name">The name of the resource.</param>
        /// <returns>A stream.</returns>
        private static Stream GetResourceStream(string name)
        {
            return typeof(MiscExamples).GetTypeInfo().Assembly.GetManifestResourceStream("ExampleLibrary.Resources." + name);
        }

        /// <summary>
        /// Renders the Mandelbrot set as an image inside the current plot area.
        /// </summary>
        public class MandelbrotSetSeries : XYAxisSeries
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MandelbrotSetSeries"/> class.
            /// </summary>
            public MandelbrotSetSeries()
            {
                this.TrackerFormatString = "X: {0:0.000}\r\nY: {1:0.000}\r\nIterations: {2}";
            }

            /// <summary>
            /// Gets or sets the color axis.
            /// </summary>
            /// <value>The color axis.</value>
            /// <remarks>The Maximum value of the ColorAxis defines the maximum number of iterations.</remarks>
            public LinearColorAxis ColorAxis { get; protected set; }

            /// <summary>
            /// Gets or sets the color axis key.
            /// </summary>
            /// <value>The color axis key.</value>
            public string ColorAxisKey { get; set; }

            /// <summary>
            /// Gets the point on the series that is nearest the specified point.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
            /// <returns>A TrackerHitResult for the current hit.</returns>
            public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
            {
                var p = this.InverseTransform(point);
                var it = this.Solve(p.X, p.Y, (int)this.ColorAxis.ActualMaximum + 1);
                return new TrackerHitResult
                {
                    Series = this,
                    DataPoint = p,
                    Position = point,
                    Item = null,
                    Index = -1,
                    Text = StringHelper.Format(this.ActualCulture, this.TrackerFormatString, null, p.X, p.Y, it)
                };
            }

            /// <summary>
            /// Renders the series on the specified render context.
            /// </summary>
            /// <param name="rc">The rendering context.</param>
            public override void Render(IRenderContext rc)
            {
                var p0 = this.Transform(this.XAxis.ActualMinimum, this.YAxis.ActualMinimum);
                var p1 = this.Transform(this.XAxis.ActualMaximum, this.YAxis.ActualMaximum);
                var w = (int)(p1.X - p0.X);
                var h = (int)(p0.Y - p1.Y);
                int maxIterations = (int)this.ColorAxis.ActualMaximum + 1;
                var pixels = new OxyColor[w, h];

                ParallelFor(
                    0,
                    h,
                    i =>
                    {
                        double y = this.YAxis.ActualMaximum - ((double)i / (h - 1) * (this.YAxis.ActualMaximum - this.YAxis.ActualMinimum));
                        for (int j = 0; j < w; j++)
                        {
                            double x = this.XAxis.ActualMinimum
                                       + ((double)j / (w - 1)
                                          * (this.XAxis.ActualMaximum - this.XAxis.ActualMinimum));
                            var iterations = Solve(x, y, maxIterations);
                            pixels[j, i] = this.ColorAxis.GetColor((double)iterations);
                        }
                    });

                var bitmap = OxyImage.Create(pixels, ImageFormat.Png);
                rc.DrawImage(bitmap, p0.X, p1.Y, p1.X - p0.X, p0.Y - p1.Y, 1, true);
            }

            /// <summary>
            /// Calculates the escape time for the specified point.
            /// </summary>
            /// <param name="x0">The x0.</param>
            /// <param name="y0">The y0.</param>
            /// <param name="maxIterations">The max number of iterations.</param>
            /// <returns>The number of iterations.</returns>
            protected virtual int Solve(double x0, double y0, int maxIterations)
            {
                int iteration = 0;
                double x = 0;
                double y = 0;
                while ((x * x) + (y * y) <= 4 && iteration < maxIterations)
                {
                    double xtemp = (x * x) - (y * y) + x0;
                    y = (2 * x * y) + y0;
                    x = xtemp;
                    iteration++;
                }

                return iteration;
            }

            /// <summary>
            /// Ensures that the axes of the series is defined.
            /// </summary>
            protected override void EnsureAxes()
            {
                base.EnsureAxes();
                this.ColorAxis = this.ColorAxisKey != null ?
                                 this.PlotModel.GetAxis(this.ColorAxisKey) as LinearColorAxis :
                                 this.PlotModel.DefaultColorAxis as LinearColorAxis;
            }

            /// <summary>
            /// Executes a serial for loop.
            /// </summary>
            /// <param name="i0">The start index (inclusive).</param>
            /// <param name="i1">The end index (exclusive).</param>
            /// <param name="action">The action that is invoked once per iteration.</param>
            private static void SerialFor(int i0, int i1, Action<int> action)
            {
                for (int i = i0; i < i1; i++)
                {
                    action(i);
                }
            }

            /// <summary>
            /// Executes a parallel for loop using ThreadPool.
            /// </summary>
            /// <param name="i0">The start index (inclusive).</param>
            /// <param name="i1">The end index (exclusive).</param>
            /// <param name="action">The action that is invoked once per iteration.</param>
            private static void ParallelFor(int i0, int i1, Action<int> action)
            {
                // Environment.ProcessorCount is not available here. Use 4 processors.
                int p = 4;

                // Initialize wait handles
                var doneEvents = new WaitHandle[p];
                for (int i = 0; i < p; i++)
                {
                    doneEvents[i] = new ManualResetEvent(false);
                }

                // Invoke the action of a partition of the range
                Action<int, int, int> invokePartition = (k, j0, j1) =>
                    {
                        for (int i = j0; i < j1; i++)
                        {
                            action(i);
                        }

                        ((ManualResetEvent)doneEvents[k]).Set();
                    };

                // Start p background threads
                int n = (i1 - i0 + p - 1) / p;
                for (int i = 0; i < p; i++)
                {
                    int k = i;
                    int j0 = i0 + (i * n);
                    var j1 = Math.Min(j0 + n, i1);
                    Task.Factory.StartNew(
                        () => invokePartition(k, j0, j1),
                        CancellationToken.None,
                        TaskCreationOptions.LongRunning,
                        TaskScheduler.Default);
                }

                // Wait for the threads to finish
                foreach (var wh in doneEvents)
                {
                    wh.WaitOne();
                }
            }
        }


        private class JuliaSetSeries : MandelbrotSetSeries
        {
            public double C1 { get; set; }
            public double C2 { get; set; }

            protected override int Solve(double x0, double y0, int maxIterations)
            {
                int iteration = 0;
                double x = x0;
                double y = y0;
                double cr = this.C1;
                double ci = this.C2;
                while ((x * x) + (y * y) <= 4 && iteration < maxIterations)
                {
                    double xtemp = (x * x) - (y * y) + cr;
                    y = (2 * x * y) + ci;
                    x = xtemp;
                    iteration++;
                }

                return iteration;
            }
        }

        /*        [XmlRoot("DataSet")]
                [XmlInclude(typeof(Data))]
                public class WorldPopulationDataSet
                {
                    [XmlElement("Data")]
                    public List<Data> Items { get; set; }

                    public class Data
                    {
                        [XmlAttribute("Year")]
                        public int Year { get; set; }

                        [XmlAttribute("Population")]
                        public double Population { get; set; }
                    }
                }*/
    }
}
