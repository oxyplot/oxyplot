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
            var outputUsingMemStream = "static-Export-stream.png";
            var outputToFile = "static-Export-file.png";
            var outputExportFileStream = "static-ExportToStream.png";
            var outputExportStreamOOP = "Export-stream.png";
            var outputExportFileOOP = "ExportToFile.png";

            var width = 1024;
            var height = 768;
            var background = OxyColors.LightYellow;
            var resolution = 96d;

            var model = BuildPlotModel();


            PngExporter.Export(model, outputToFile, width, height, background, resolution);

            using (var stream = new MemoryStream())
            {
                PngExporter.Export(model, stream, width, height, background, resolution);
                System.IO.File.WriteAllBytes(outputUsingMemStream, stream.ToArray());
            }

            using (var pngStream = PngExporter.ExportToStream(model, width, height, background, resolution))
            {
                var fileStream = new System.IO.FileStream(outputExportFileStream, FileMode.Create);
                pngStream.CopyTo(fileStream);
                fileStream.Flush();
            }

            var stream2 = new MemoryStream();
            var pngExporter = new PngExporter { Width = width, Height = height, Background = background };
            pngExporter.Export(model, stream2);
            File.WriteAllBytes(outputExportStreamOOP, stream2.ToArray());

            // Write to a file, OOP
            var pngExporter2 = new PngExporter { Width = width, Height = height, Background = background };
            pngExporter2.ExportToFile(model, outputExportFileOOP);
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
