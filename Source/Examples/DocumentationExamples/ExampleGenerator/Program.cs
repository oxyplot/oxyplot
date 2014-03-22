// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
// --------------------------------------------------------------------------------------------------------------------

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
            Console.WriteLine(fileName);
            using (var stream = File.Create(fileName))
            {
                var exporter = new PngExporter { Width = 600, Height = 400 };
                exporter.Export(model, stream);
            }

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
                using (var exporter = new OxyPlot.WindowsForms.SvgExporter
                                       {
                                           Width = 600,
                                           Height = 400,
                                           IsDocument = true
                                       })
                {
                    exporter.Export(model, stream);
                }
            }
        }
    }
}