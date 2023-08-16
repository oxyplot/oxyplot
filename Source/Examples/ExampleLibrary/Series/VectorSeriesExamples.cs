namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("VectorSeries"), Tags("Series")]
    public static class VectorSeriesExamples
    {
        private static readonly Func<double, double, double> dpeaksdx = (x, y) =>
            -10 * ((1 / 5 - 3 * Math.Pow(x, 2)) * Math.Exp(-Math.Pow(x, 2) - Math.Pow(y, 2)) - 2 * x * (x / 5 - Math.Pow(x, 3) - Math.Pow(y, 5)) * Math.Exp(-Math.Pow(x, 2) - Math.Pow(y, 2))) + 0.6666666666666666 * (1 + x) * Math.Exp(-Math.Pow(1 + x, 2) - Math.Pow(y, 2)) + 3 * (-2 * (1 - x) * Math.Exp(-Math.Pow(x, 2) - Math.Pow(1 + y, 2)) - 2 * x * Math.Pow(1 - x, 2) * Math.Exp(-Math.Pow(x, 2) - Math.Pow(1 + y, 2)));

        private static readonly Func<double, double, double> dpeaksdy = (x, y) =>
            -10 * (-5 * Math.Pow(y, 4) * Math.Exp(-Math.Pow(x, 2) - Math.Pow(y, 2)) - 2 * y * (x / 5 - Math.Pow(x, 3) - Math.Pow(y, 5)) * Math.Exp(-Math.Pow(x, 2) - Math.Pow(y, 2))) + 0.6666666666666666 * y * Math.Exp(-Math.Pow(1 + x, 2) - Math.Pow(y, 2)) - 6 * Math.Pow(1 - x, 2) * (1 + y) * Math.Exp(-Math.Pow(x, 2) - Math.Pow(1 + y, 2));

        [Example("VectorSeries")]
        public static PlotModel FromItems()
        {
            var model = GetModel(true, out _);
            return model;
        }

        [Example("VectorSeries (Veeness = 2)")]
        public static PlotModel FromItemsVeeness()
        {
            var model = GetModel(true, out var series);
            series.ArrowVeeness = 2;
            return model;
        }

        [Example("VectorSeries (Vector Origin and Label position)")]
        public static PlotModel FromItemsVectorOriginAndLabelPosition()
        {
            var model = GetModel(true, out var series);
            series.ArrowLabelPosition = 0.25;
            series.ArrowStartPosition = 0.5;
            return model;
        }

        [Example("VectorSeries (without ColorAxis)")]
        public static PlotModel FromItemsWithoutColorAxis()
        {
            var model = GetModel(false, out _);
            return model;
        }

        [Example("Vector Field")]
        [DocumentationExample("Series/VectorSeries")]
        public static PlotModel VectorField()
        {
            var model = new PlotModel { Title = "Peaks (Gradient)" };
            var vs = new VectorSeries();
            var columnCoordinates = ArrayBuilder.CreateVector(-3, 3, 0.25);
            var rowCoordinates = ArrayBuilder.CreateVector(-3.1, 3.1, 0.25);
            vs.ArrowVeeness = 1;
            vs.ArrowStartPosition = 0.5;
            vs.ItemsSource = columnCoordinates.SelectMany(x => rowCoordinates.Select(y => new VectorItem(new DataPoint(x, y), new DataVector(dpeaksdx(x, y) / 40, dpeaksdy(x, y) / 40), double.NaN))).ToList();
            model.Series.Add(vs);
            return model;
        }

        private static PlotModel GetModel(bool includeColorAxis, out VectorSeries series)
        {
            const int NumberOfItems = 100;
            var model = new PlotModel { Title = "VectorSeries (Veeness = 2)" };

            var rand = new Random(1);
            var w = 100.0;
            var h = 100.0;
            var max = 10.0;

            if (includeColorAxis)
            {
                model.Axes.Add(new LinearColorAxis
                {
                    Position = AxisPosition.Right,
                    Palette = new OxyPalette(OxyPalettes.Cool(10).Colors.Select(c => OxyColor.FromAColor(100, c))),
                    Minimum = 0.0,
                    Maximum = max,
                });
            }

            model.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = -max,
                Maximum = w + max,
            });

            model.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -max,
                Maximum = h + max,
            });

            series = new VectorSeries() { LabelFontSize = 12 };
            for (int i = NumberOfItems - 1; i >= 0; i--)
            {
                var ang = rand.NextDouble() * Math.PI * 2.0;
                var mag = rand.NextDouble() * max;

                var origin = new DataPoint(rand.NextDouble() * w, rand.NextDouble() * h);
                var direction = new DataVector(Math.Cos(ang) * mag, Math.Sin(ang) * mag);
                series.Items.Add(new VectorItem(origin, direction, mag));
            }

            model.Series.Add(series);

            return model;
        }

        [Example("VectorSeries on Log Axis")]
        [DocumentationExample("Series/VectorSeries")]
        public static PlotModel LogarithmicYAxis()
        {
            const int NumberOfItems = 100;
            var model = new PlotModel { Title = "VectorSeries" };

            var rand = new Random(1);
            var w = 100.0;
            var h = 100.0;
            var max = 50.0;

            model.Axes.Add(new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.Cool(10),
                Minimum = 0.0,
                Maximum = max,
            });

            model.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = -max,
                Maximum = w + max,
            });
            model.Axes.Add(new LogarithmicAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 1,
                Maximum = h + max,
            });

            var s = new VectorSeries() { LabelFontSize = 12 };
            for (int i = NumberOfItems - 1; i >= 0; i--)
            {
                var ang = rand.NextDouble() * Math.PI * 2.0;
                var mag = rand.NextDouble() * max;

                var origin = new DataPoint(rand.NextDouble() * w, rand.NextDouble() * h + 1);
                var direction = new DataVector(Math.Cos(ang) * mag, Math.Sin(ang) * mag);
                s.Items.Add(new VectorItem(origin, direction, mag));
            }

            model.Series.Add(s);

            return model;
        }
    }
}
