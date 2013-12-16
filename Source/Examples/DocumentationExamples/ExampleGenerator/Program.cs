namespace ExampleGenerator
{
    using System;
    using System.Drawing;
    using System.IO;

    using OxyPlot;
    using OxyPlot.WindowsForms;

    public class Program
    {
        public static string OutputDirectory { get; set; }

        static void Main(string[] args)
        {
            OutputDirectory = @"..\..\..\..\..\..\Documentation\Images\Series";
            if (args.Length > 0)
            {
                OutputDirectory = args[0];
            }

            Export(SeriesExamples.Example1(), "Example1");
            Export(SeriesExamples.LineSeries(), "LineSeries");
            Export(SeriesExamples.LineSeriesSmoothed(), "LineSeriesSmoothed");
            Export(SeriesExamples.TwoColorLineSeries(), "TwoColorLineSeries");
            Export(SeriesExamples.ScatterSeries(), "ScatterSeries");
            Export(SeriesExamples.HeatMapSeries(), "HeatMapSeries");
            Export(SeriesExamples.ContourSeries(), "ContourSeries");
            Export(SeriesExamples.BarSeries(), "BarSeries");
            Export(SeriesExamples.ColumnSeries(), "ColumnSeries");
        }

        private static void Export(PlotModel model, string name)
        {
            var fileName = Path.Combine(OutputDirectory, name + ".png");
            PngExporter.Export(model, fileName, 600, 400, Brushes.White);

            fileName = Path.ChangeExtension(fileName, ".pdf");
            Console.WriteLine(fileName);
            using (var stream = File.Create(fileName))
            {
                var exporter = new PdfExporter { Width = 600d * 72 / 96, Height = 400d * 72 / 96 };
                exporter.Export(model, stream);
            }

            fileName = Path.ChangeExtension(fileName, ".svg");
            Console.WriteLine(fileName);
            using (var stream = File.Create(fileName))
            {
                var exporter = new SvgExporter { Width = 600, Height = 400, IsDocument = true };
                exporter.Export(model, stream);
            }
        }
    }
}
