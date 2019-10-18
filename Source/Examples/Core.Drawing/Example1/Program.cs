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
            var outputToFileBrush = "test-oxyplot-file-brush.png";
            var outputExportFileStream = "test-oxyplot-stream-export.png";
            var outputExportBitmap = "test-oxyplot-exportobitmap.png";
            var outputExportBitmapBrush = "test-oxyplot-exportobitmap-brush.png";
            var outputExportStreamOOP = "test-oxyplot-ExportToStream.png";
            var outputExportFileOOP = "test-oxyplot-ExportToFile.png";

            var width = 1024;
            var height = 768;

            var model = BuildPlotModel();

            // export to file using static methods
            PngExporter.Export(model, outputToFile, width, height, OxyColors.LightGray);

            PngExporter.Export(model, outputToFileBrush, width, height, Brushes.LightGray);

            // export to stream using static methods
            using (var stream = new MemoryStream())
            {
                PngExporter.Export(model, stream, width, height, OxyColors.LightGray, 96);
                System.IO.File.WriteAllBytes(outputUsingMemStream, stream.ToArray());
            }

            using (var pngStream = PngExporter.ExportToStream(model, width, height, OxyColors.LightGray))
            {
                var fileStream = new System.IO.FileStream(outputExportFileStream, FileMode.Create);
                pngStream.CopyTo(fileStream);
                fileStream.Flush();
            }

            // export to bitmap using static methods
            using (var bm = PngExporter.ExportToBitmap(model, width, height, OxyColors.LightGray))
            {
                bm.Save(outputExportBitmap);
            }

            using (var bm = PngExporter.ExportToBitmap(model, width, height, Brushes.LightGray))
            {
                bm.Save(outputExportBitmapBrush);
            }

            // export using the instance methods
            using (var stream = new MemoryStream())
            {
                var pngExporter = new PngExporter { Width = width, Height = height, Background = OxyColors.LightGray };
                pngExporter.Export(model, stream);
                System.IO.File.WriteAllBytes(outputExportStreamOOP, stream.ToArray());
            }

            var pngExporter2 = new PngExporter { Width = width, Height = height, Background = OxyColors.LightGray };
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
