// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MiscExamples.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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

    using OxyPlot;

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

        public static PlotModel NumericOdeSolvers(string title, string subtitle, double t0, double y0, Func<double, double> exact, Func<double, double, double> f)
        {
            var model = new PlotModel(title, subtitle) { LegendPosition = LegendPosition.BottomCenter, LegendPlacement = LegendPlacement.Outside, LegendOrientation = LegendOrientation.Horizontal };
            model.Series.Add(new FunctionSeries(exact, 0, 4, 100) { Title = "Exact solution", StrokeThickness = 5 });

            model.Series.Add(new LineSeries("Euler, h=0.25")
                {
                    MarkerType = MarkerType.Circle,
                    MarkerFill = OxyColors.Black,
                    Points = Euler(f, t0, y0, 4, 0.25)
                });

            //model.Series.Add(new LineSeries("Euler, h=1")
            //    {
            //        MarkerType = MarkerType.Circle,
            //        MarkerFill = OxyColors.Black,
            //        Points = Euler(f, t0, y0, 4, 1)
            //    });

            model.Series.Add(new LineSeries("Heun, h=0.25")
            {
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Black,
                Points = Heun(f, t0, y0, 4, 0.25)
            });

            model.Series.Add(new LineSeries("Midpoint, h=0.25")
            {
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Black,
                Points = Midpoint(f, t0, y0, 4, 0.25)
            });

            model.Series.Add(new LineSeries("RK4, h=0.25")
            {
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Black,
                Points = RungeKutta4(f, t0, y0, 4, 0.25)
            });

            //model.Series.Add(new LineSeries("RK4, h=1")
            //{
            //    MarkerType = MarkerType.Circle,
            //    MarkerFill = OxyColors.Black,
            //    Points = RungeKutta4(f, t0, y0, 4, 1)
            //});

            model.Axes.Add(new LinearAxis(AxisPosition.Left));
            return model;
        }

        private static IList<IDataPoint> Euler(Func<double, double, double> f, double t0, double y0, double t1, double h)
        {
            var points = new List<IDataPoint>();
            double y = y0;
            for (double t = t0; t < t1 + h / 2; t += h)
            {
                points.Add(new DataPoint(t, y));
                y += h * f(t, y);
            }

            return points;
        }

        private static IList<IDataPoint> Heun(Func<double, double, double> f, double t0, double y0, double t1, double h)
        {
            var points = new List<IDataPoint>();
            double y = y0;
            for (double t = t0; t < t1 + h / 2; t += h)
            {
                points.Add(new DataPoint(t, y));
                double ytilde = y + h * f(t, y);
                y = y + h / 2 * (f(t, y) + f(t + h, ytilde));
            }

            return points;
        }

        private static IList<IDataPoint> Midpoint(Func<double, double, double> f, double t0, double y0, double t1, double h)
        {
            var points = new List<IDataPoint>();
            double y = y0;
            for (double t = t0; t < t1 + h / 2; t += h)
            {
                points.Add(new DataPoint(t, y));
                y += h * f(t + h / 2, y + h / 2 * f(t, y));
            }

            return points;
        }

        private static IList<IDataPoint> RungeKutta4(Func<double, double, double> f, double t0, double y0, double t1, double h)
        {
            var points = new List<IDataPoint>();
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

        [Example("Train schedule")]
        public static PlotModel TrainSchedule()
        {
            //// http://www.edwardtufte.com/tufte/books_vdqi
            //// http://marlenacompton.com/?p=103
            //// http://mbostock.github.com/protovis/ex/caltrain.html
            //// http://en.wikipedia.org/wiki/%C3%89tienne-Jules_Marey
            //// http://mbostock.github.com/protovis/ex/marey-train-schedule.jpg
            //// http://c82.net/posts.php?id=66

            var model = new PlotModel("Train schedule", "Bergensbanen (Oslo-Bergen, Norway)") { IsLegendVisible = false, PlotAreaBorderThickness = 0 };
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -20, 540, "Distance from Oslo S")
            {
                IsAxisVisible = false,
                StringFormat = "0"
            });
            model.Axes.Add(new TimeSpanAxis(AxisPosition.Bottom, 0, TimeSpanAxis.ToDouble(TimeSpan.FromHours(24)))
            {
                StringFormat = "hh",
                Title = "Time",
                MajorStep = TimeSpanAxis.ToDouble(TimeSpan.FromHours(1)),
                MinorStep = TimeSpanAxis.ToDouble(TimeSpan.FromMinutes(10)),
                TickStyle = TickStyle.None,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.LightGray,
                MinorGridlineStyle = LineStyle.Dot,
                MinorGridlineColor = OxyColors.LightGray
            });

            // Read the train schedule from a .csv resource
#if PCL
            var resources = typeof(MiscExamples).GetTypeInfo().Assembly.GetManifestResourceNames();
            using (var stream = typeof(MiscExamples).GetTypeInfo().Assembly.GetManifestResourceStream(resources[0]))
#else
            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resources[0]))
#endif

            using (var reader = new StreamReader(stream))
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
                    series[i] = new LineSeries(headerFields[3 + i])
                    {
                        Color = OxyColor.FromAColor(180, OxyColors.Black),
                        StrokeThickness = 2,
                        TrackerFormatString = "Train {0}\nTime: {2}\nDistance from Oslo S: {4:0.0} km",
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
                    if (!string.IsNullOrWhiteSpace(fields[0]))
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
                                    TextVerticalAlignment = VerticalTextAlign.Middle,
                                    TextPosition = 0,
                                    TextMargin = 4,
                                    TextHorizontalAlignment = HorizontalTextAlign.Right
                                });
                    }

                    for (int i = 0; i < series.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(fields[i + 3]))
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

        [Example("La Linea (AreaSeries)")]
        public static PlotModel LaLineaAreaSeries()
        {
            // http://en.wikipedia.org/wiki/La_Linea_(TV_series)
            var model = new PlotModel("La Linea") { PlotType = PlotType.Cartesian, Background = OxyColor.FromRGB(84, 98, 207) };
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -500, 1000));
            var series1 = new AreaSeries { Fill = OxyColors.White, StrokeThickness = 0 };
            series1.Points.Append(GetLineaPoints());
            model.Series.Add(series1);
            return model;
        }

        [Example("La Linea (LineSeries)")]
        public static PlotModel LaLinea()
        {
            // http://en.wikipedia.org/wiki/La_Linea_(TV_series)
            var model = new PlotModel("La Linea") { PlotType = PlotType.Cartesian, Background = OxyColor.FromRGB(84, 98, 207) };
            model.Axes.Add(new LinearAxis(AxisPosition.Left, -500, 1000));
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

        private static IEnumerable<IDataPoint> GetLineaPoints()
        {
            var points = new List<IDataPoint>();

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
    }
}