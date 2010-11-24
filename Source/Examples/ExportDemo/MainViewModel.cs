using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OxyPlot;
using OxyPlot.Pdf;
using OxyPlot.Reporting;

namespace ExportDemo
{
    public class MainViewModel : Observable
    {
        private PlotModel model;
        private bool renderToCanvas;

        public MainViewModel()
        {
            RenderToCanvas = true;
            ModelNames = new List<string>
                             {
                                 "Sine wave",
                                 "Smooth interpolation",
                                 "Normal distributions",
                                 "Square wave",
                                 "Log-log",
                                 "Log-lin",
                                 "Lin-log",
                                 "Clover"
                             };

            ChangeModel(0);
        }

        public List<String> ModelNames { get; set; }

        public bool RenderToCanvas
        {
            get { return renderToCanvas; }
            set
            {
                renderToCanvas = value;
                RaisePropertyChanged(() => RenderToCanvas);
            }
        }

        public PlotModel Model
        {
            get { return model; }
            set
            {
                if (model != value)
                {
                    model = value;
                    RaisePropertyChanged(() => Model);
                    RaisePropertyChanged(() => TotalNumberOfPoints);
                }
            }
        }

        public int TotalNumberOfPoints
        {
            get
            {
                if (Model == null) return 0;
                return Model.Series.Sum(ls => ls.Points.Count);
            }
        }


        public void ChangeModel(int index)
        {
            switch (index)
            {
                case 0:
                    Model = CreateSineModel(0.0001);
                    break;
                case 1:
                    PlotModel tmp = CreateSineModel(1);
                    tmp.Title = "Smooth interpolation";
                    // Add markers to this plot
                    var ls = tmp.Series[0] as LineSeries;
                    if (ls == null) return;
                    ls.MarkerType = MarkerType.Circle;
                    ls.Color = OxyColor.FromArgb(0xFF, 154, 6, 78);
                    ls.MarkerStroke = ls.Color;
                    ls.MarkerFill = OxyColor.FromAColor(0x70, ls.Color);
                    ls.MarkerStrokeThickness = 2;
                    ls.MarkerSize = 4;

                    LineSeries ls2 = CreateLineSeries(Math.Sin,
                                                      0, 10, 1, "interpolated curve");
                    ls2.Smooth = true;
                    tmp.Series.Add(ls2);

                    Model = tmp;
                    break;
                case 2:
                    Model = CreateNormalDistributionModel();
                    break;
                case 3:
                    Model = CreateSquareWave();
                    break;
                case 4:
                    Model = CreateLogLogPlot();
                    break;
                case 5:
                    Model = CreateLogLinPlot();
                    break;
                case 6:
                    Model = CreateLinLogPlot();
                    break;
                case 7:
                    // http://people.reed.edu/~jerry/Clover/cloverexcerpt.pdf
                    // http://www-math.bgsu.edu/z/calc3/vectorvalued1.html
                    Model = CreateParametricPlot(
                        t => 2 * Math.Cos(2 * t) * Math.Cos(t),
                        t => 2 * Math.Cos(2 * t) * Math.Sin(t),
                        // t=>-4*Math.Sin(2*t)*Math.Cos(t)-2*Math.Cos(2*t)*Math.Sin(t),
                        // t=>-4*Math.Sin(2*t)*Math.Sin(t)+2*Math.Cos(2*t)*Math.Cos(t),))))
                        0, Math.PI * 2, 0.01,
                        "Parametric function",
                        "Using the CartesianAxes property",
                        "2cos(2t)cos(t) , 2cos(2t)sin(t)");
                    break;
            }
        }


        private static PlotModel CreateSineModel(double stepSize)
        {
            var plot = new PlotModel
                           {
                               Title = "Sine wave"
                           };

            var ls = CreateLineSeries(Math.Sin, 0, 10, stepSize, "sin(x)");
            plot.Series.Add(ls);
            plot.Axes.Add(new LinearAxis
                              {
                                  Title = "Y",
                                  Position = AxisPosition.Left,
                                  MaximumPadding = 0.3,
                                  MinimumPadding = 0.3,
                                  MajorGridlineStyle = LineStyle.Solid
                              });
            plot.Axes.Add(new LinearAxis
                              {
                                  Title = "X",
                                  Position = AxisPosition.Bottom,
                                  MajorGridlineStyle = LineStyle.Solid
                              });

            return plot;
        }

        private static LineSeries CreateLineSeries(Func<double, double> f, double x0, double x1, double dx,
                                                   string title)
        {
            var ls = new LineSeries { Title = title };
            for (double x = x0; x <= x1; x += dx)
                ls.Points.Add(new DataPoint(x, f(x)));
            return ls;
        }

        private static PlotModel CreateNormalDistributionModel()
        {
            // http://en.wikipedia.org/wiki/Normal_distribution

            var plot = new PlotModel
                           {
                               Title = "Normal distribution",
                               Subtitle = "Probability density function"
                           };

            plot.Axes.Add(new LinearAxis
                              {
                                  Position = AxisPosition.Left,
                                  Minimum = -0.05,
                                  Maximum = 1.05,
                                  MajorStep = 0.2,
                                  MinorStep = 0.05,
                                  TickStyle = TickStyle.Inside
                              });
            plot.Axes.Add(new LinearAxis
                              {
                                  Position = AxisPosition.Bottom,
                                  Minimum = -5.25,
                                  Maximum = 5.25,
                                  MajorStep = 1,
                                  MinorStep = 0.25,
                                  TickStyle = TickStyle.Inside
                              });
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 0.2));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 1));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 5));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, -2, 0.5));
            return plot;
        }

        private static DataSeries CreateNormalDistributionSeries(double x0, double x1, double mean, double variance,
                                                                 int n = 1001)
        {
            var ls = new LineSeries
                         {
                             Title = String.Format("μ={0}, σ²={1}", mean, variance)
                         };

            for (int i = 0; i < n; i++)
            {
                double x = x0 + (x1 - x0) * i / (n - 1);
                double f = 1.0 / Math.Sqrt(2 * Math.PI * variance) * Math.Exp(-(x - mean) * (x - mean) / 2 / variance);
                ls.Points.Add(new DataPoint(x, f));
            }
            return ls;
        }

        private static PlotModel CreateSquareWave(int n = 25)
        {
            var plot = new PlotModel { Title = "Square wave (Gibbs phenomenon)" };

            var ls = new LineSeries
                         {
                             Title = "sin(x)+sin(3x)/3+sin(5x)/5+...+sin(" + (2 * n - 1) + ")/" + (2 * n - 1)
                         };

            for (double x = -10; x < 10; x += 0.0001)
            {
                double y = 0;
                for (int i = 0; i < n; i++)
                {
                    int j = i * 2 + 1;
                    y += Math.Sin(j * x) / j;
                }
                ls.Points.Add(new DataPoint(x, y));
            }
            plot.Axes.Add(new LinearAxis
                              {
                                  Position = AxisPosition.Left,
                                  Minimum = -4,
                                  Maximum = 4
                              });
            plot.Axes.Add(new LinearAxis
                              {
                                  Position = AxisPosition.Bottom
                              });

            plot.Series.Add(ls);

            return plot;
        }

        private static PlotModel CreateParametricPlot(Func<double, double> fx, Func<double, double> fy, double t0,
                                                      double t1, double dt, string title, string subtitle,
                                                      string seriesTitle)
        {
            var plot = new PlotModel { Title = title, Subtitle = subtitle, PlotType = PlotType.Cartesian };

            var ls = new LineSeries { Title = seriesTitle };

            for (double t = t0; t <= t1; t += dt)
            {
                ls.Points.Add(new DataPoint(fx(t), fy(t)));
            }
            plot.Series.Add(ls);
            return plot;
        }

        private static PlotModel CreateLogLinPlot()
        {
            // http://en.wikipedia.org/wiki/Lin-log_graph

            var plot = new PlotModel { Title = "Log-Lin plot" };

            plot.Series.Add(CreateLineSeries(x => x, 0.1, 100, 0.1, "y=x"));
            plot.Series.Add(CreateLineSeries(x => x * x, 0.1, 100, 0.1, "y=x²"));
            plot.Series.Add(CreateLineSeries(x => x * x * x, 0.1, 100, 0.1, "y=x³"));

            plot.Axes.Add(new LogarithmicAxis
                              {
                                  Position = AxisPosition.Left,
                                  Minimum = 0.1,
                                  Maximum = 100,
                                  MajorGridlineStyle = LineStyle.Solid,
                                  MinorGridlineStyle = LineStyle.Solid
                              });
            plot.Axes.Add(new LinearAxis
                              {
                                  Position = AxisPosition.Bottom,
                                  Minimum = 0.1,
                                  Maximum = 100,
                                  MajorGridlineStyle = LineStyle.Solid,
                                  MinorGridlineStyle = LineStyle.Solid
                              });

            return plot;
        }

        private static PlotModel CreateLinLogPlot()
        {
            // http://en.wikipedia.org/wiki/Lin-log_graph
            var plot = new PlotModel { Title = "Lin-Log plot" };

            plot.Series.Add(CreateLineSeries(x => x, 0.1, 100, 0.1, "y=x"));
            plot.Series.Add(CreateLineSeries(x => x * x, 0.1, 100, 0.1, "y=x²"));
            plot.Series.Add(CreateLineSeries(x => x * x * x, 0.1, 100, 0.1, "y=x³"));

            plot.Axes.Add(new LogarithmicAxis
                              {
                                  Position = AxisPosition.Bottom,
                                  Minimum = 0.1,
                                  Maximum = 100,
                                  MajorGridlineStyle = LineStyle.Solid,
                                  MinorGridlineStyle = LineStyle.Solid
                              });
            plot.Axes.Add(new LinearAxis
                              {
                                  Position = AxisPosition.Left,
                                  Minimum = 0.1,
                                  Maximum = 100,
                                  MajorGridlineStyle = LineStyle.Solid,
                                  MinorGridlineStyle = LineStyle.Solid
                              });

            return plot;
        }

        private static PlotModel CreateLogLogPlot()
        {
            // http://en.wikipedia.org/wiki/Log-log_plot
            var plot = new PlotModel { Title = "Log-log plot" };

            plot.Series.Add(CreateLineSeries(x => x, 0.1, 100, 0.1, "y=x"));
            plot.Series.Add(CreateLineSeries(x => x * x, 0.1, 100, 0.1, "y=x²"));
            plot.Series.Add(CreateLineSeries(x => x * x * x, 0.1, 100, 0.1, "y=x³"));

            plot.Axes.Add(new LogarithmicAxis
                              {
                                  Position = AxisPosition.Left,
                                  Minimum = 0.1,
                                  Maximum = 100,
                                  MajorGridlineStyle = LineStyle.Solid,
                                  MinorGridlineStyle = LineStyle.Solid
                              });
            plot.Axes.Add(new LogarithmicAxis
                              {
                                  Position = AxisPosition.Bottom,
                                  Minimum = 0.1,
                                  Maximum = 100,
                                  MajorGridlineStyle = LineStyle.Solid,
                                  MinorGridlineStyle = LineStyle.Solid
                              });

            return plot;
        }

        public void SaveReport(string fileName)
        {
            var r = new Report();
            var main = new ReportSection();

            r.AddHeader(1, "Example report from OxyPlot");
            r.AddHeader(2, "Content");
            r.AddContent(main);
            r.Add(main);

            main.AddHeader(2, "Introduction");
            main.AddParagraph("The content in this file was generated by OxyPlot.");
            main.AddParagraph("See http://oxyplot.codeplex.com for more information.");

            main.AddHeader(2, "Plot");
            main.AddParagraph(
                "The plot is rendered to SVG and embedded in the HTML5 file. You can also save the plots to separate SVG or PNG files.");

            string svg = Model.ToSvg(800, 500);
            main.AddDrawing(svg, Model.Title);

            main.AddHeader(2, "Data");
            int i = 1;
            foreach (DataSeries s in Model.Series)
            {
                main.AddHeader(3, "Data series " + (i++));
                main.AddPropertyTable("Properties of the " + s.GetType().Name, new[] { s });
                var fields = new List<TableColumn>
                                 {
                                     new TableColumn("X", "X"), 
                                     new TableColumn("Y", "Y")
                                 };
                main.AddTable("Data", s.Points, fields);
            }

            const string style =
@"body { font-family: Verdana,Arial; margin:20pt; }
table { border: solid 1px black; margin: 8pt; border-collapse:collapse; }
td { padding: 0 2pt 0 2pt; border-left: solid 1px black; border-right: solid 1px black;}
thead { border:solid 1px black; }
.content, .content td { border: none; }
.figure { margin: 8pt;}
.table { margin: 8pt;}
.table caption { margin: 4pt;}
.table thead td { padding: 2pt;}";

            string ext = Path.GetExtension(fileName).ToLower();
            if (ext == ".html")
                using (var hw = new HtmlReportWriter(fileName, "OxyPlot example file", null, style))
                {
                    r.Write(hw);
                }

            if (ext == ".pdf")
                using (var pw = new PdfReportWriter(fileName))
                {
                    r.Write(pw);
                }

            using (var tw = new TextReportWriter(Path.ChangeExtension(fileName, ".txt")))
            {
                r.Write(tw);
            }
        }
    }
}