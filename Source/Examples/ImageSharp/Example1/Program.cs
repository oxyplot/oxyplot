namespace Example1
{
    using System;
    using System.IO;
    using System.Linq;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.ImageSharp;
    using OxyPlot.Series;

    class Program
    {
        static void Main(string[] args)
        {
            var outputToFile = "test-oxyplot-static-export-file";
            var outputExportStreamOOP = "test-oxyplot-export-stream";

            var width = 1024;
            var height = 768;
            var resolutions = new[] { 72d, 96d, 182d };

            var model = BuildPlotModel();

            foreach (var resolution in resolutions)
            {
                // export to file using static methods
                PngExporter.Export(model, $"{outputToFile}{resolution}.png", width, height, resolution);

                // export using the instance methods
                using (var stream = new MemoryStream())
                {
                    var pngExporter = new PngExporter(width, height, resolution);
                    pngExporter.Export(model, stream);
                    System.IO.File.WriteAllBytes($"{outputExportStreamOOP}{resolution}.png", stream.ToArray());
                }

                model.Background = OxyColors.White;

                // export to file using static methods
                JpegExporter.Export(model, $"{outputToFile}{resolution}.jpg", width, height, resolution);

                // export using the instance methods
                using (var stream = new MemoryStream())
                {
                    var jpegExporter = new JpegExporter(width, height, resolution);
                    jpegExporter.Export(model, stream);
                    System.IO.File.WriteAllBytes($"{outputExportStreamOOP}{resolution}.jpg", stream.ToArray());
                }
            }
        }

        private static PlotModel BuildPlotModel()
        {
            var rand = new Random(21);

            var model = new PlotModel { Title = "Cake Type Popularity" };

            var cakePopularity = Enumerable.Range(1, 5).Select(i => rand.NextDouble()).ToArray();
            var sum = cakePopularity.Sum();
            var barItems = cakePopularity.Select(cp => RandomBarItem(cp, sum)).ToArray();
            var barSeries = new BarSeries
            {
                ItemsSource = barItems,
                LabelPlacement = LabelPlacement.Base,
                LabelFormatString = "{0:.00}%"
            };

            model.Series.Add(barSeries);

            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                ItemsSource = new[]
                {
                    "Apple cake",
                    "Baumkuchen",
                    "Bundt Cake",
                    "Chocolate cake",
                    "Carrot cake"
                }
            });

            return model;
        }

        private static BarItem RandomBarItem(double cp, double sum)
            => new BarItem { Value = cp / sum * 100, Color = RandomColor() };

        private static OxyColor RandomColor()
        {
            var r = new Random();
            return OxyColor.FromRgb((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255));
        }
    }
}
