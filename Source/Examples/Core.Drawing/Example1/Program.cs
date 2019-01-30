namespace Example1
{
    using System;
    using System.Drawing;
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
            var outputUsingMemStream = "test-oxyplot-memstream.png";
            var outputToFile = "test-oxyplot-file.png";
            var outputExportFileStream = "test-oxyplot-stream-export.png";
            var outputExportFileOOP = "test-oxyplot-ExportToFile.png";

            var width = 1024;
            var height = 768;

            var model = BuildPlotModel();


            PngExporter.Export(model, outputToFile, width, height, Brushes.White);

            using (var stream = new MemoryStream())
            {
                PngExporter.Export(model, stream, width, height, OxyColors.White, 96);
                System.IO.File.WriteAllBytes(outputUsingMemStream, stream.ToArray());
            }

            using (var pngStream = PngExporter.ExportToStream(model, width, height, OxyColors.White))
            {
                var fileStream = new System.IO.FileStream(outputExportFileStream, FileMode.Create);
                pngStream.CopyTo(fileStream);
                fileStream.Flush();
            }

            var stream2 = new MemoryStream();
            var pngExporter = new PngExporter { Width = width, Height = height, Background = OxyColors.White };
            pngExporter.Export(model, stream2);

            // Write to a file, OOP
            var pngExporter2 = new PngExporter { Width = width, Height = height, Background = OxyColors.White };
            pngExporter2.ExportToFile(model, outputExportFileOOP);
        }

        private static IPlotModel BuildPlotModel()
        {
            var rand = new Random();

            var model = new PlotModel { Title = "Cake Type Popularity" };

            var cakePopularity = Enumerable.Range(1, 5).Select(i => rand.NextDouble()).ToArray();
            var sum = cakePopularity.Sum();
            var barSeries = new BarSeries
            {
                ItemsSource = cakePopularity.Select(cp => RandomBarItem(cp, sum)),
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
