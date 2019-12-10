namespace Example1
{
    using System;
    using System.IO;
    using System.Linq;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Core.Drawing;
    using OxyPlot.Series;

    class Program
    {
        static void Main(string[] args)
        {
            var outputToFile = "test-oxyplot-static-export-file.png";
            var outputExportStreamOOP = "test-oxyplot-ExportToStream.png";
            var outputExportFileOOP = "test-oxyplot-ExportToFile.png";

            var width = 1024;
            var height = 768;
            var background = OxyColors.LightGray;
            var resolution = 96d;

            var model = BuildPlotModel();

            // export to file using static methods
            PngExporter.Export(model, outputToFile, width, height, resolution);

            // export using the instance methods
            using (var stream = new MemoryStream())
            {
                var pngExporter = new PngExporter { Width = width, Height = height, Resolution = resolution };
                pngExporter.Export(model, stream);
                System.IO.File.WriteAllBytes(outputExportStreamOOP, stream.ToArray());
            }

            var pngExporter2 = new PngExporter { Width = width, Height = height, Resolution = resolution };
            var bitmap = pngExporter2.ExportToBitmap(model);
            bitmap.Save(outputExportFileOOP, System.Drawing.Imaging.ImageFormat.Png);
            bitmap.Save(Path.ChangeExtension(outputExportFileOOP, ".gif"), System.Drawing.Imaging.ImageFormat.Gif);
        }

        private static IPlotModel BuildPlotModel()
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
