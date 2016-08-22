// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExportDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    using Caliburn.Micro;

    using Microsoft.Win32;

    using OxyPlot;
    using OxyPlot.OpenXml;
    using OxyPlot.Pdf;
    using OxyPlot.Reporting;
    using OxyPlot.Wpf;

    using PropertyTools.Wpf;

    using SvgExporter = OxyPlot.SvgExporter;

    [Export(typeof(IShell))]
    public class ShellViewModel : PropertyChangedBase, IShell
    {

        private ModelType currentModel;

        private PlotModel model;



        public ShellViewModel()
        {
            this.CurrentModel = ModelType.SineWave;
        }



        public ModelType CurrentModel
        {
            get
            {
                return this.currentModel;
            }

            set
            {
                this.currentModel = value;
                this.Model = PlotModelFactory.Create(this.currentModel);
            }
        }

        public PlotModel Model
        {
            get
            {
                return this.model;
            }

            set
            {
                if (this.model != value)
                {
                    this.model = value;
                    this.NotifyOfPropertyChange(() => this.Model);
                    this.NotifyOfPropertyChange(() => this.TotalNumberOfPoints);
                }
            }
        }

        public Window Owner { get; private set; }

        public PlotView Plot { get; private set; }

        public int TotalNumberOfPoints
        {
            get
            {
                if (this.Model == null)
                {
                    return 0;
                }

                return this.Model.Series.Sum(ls => ((OxyPlot.Series.DataPointSeries)ls).Points.Count);
            }
        }



        public void Attach(Window owner, PlotView plot)
        {
            this.Owner = owner;
            this.Plot = plot;
        }

        public void CopyBitmap()
        {
            var bitmap = PngExporter.ExportToBitmap(
                this.Model, (int)this.Plot.ActualWidth, (int)this.Plot.ActualHeight, this.Model.Background);
            Clipboard.SetImage(bitmap);
        }

        public void CopySvg()
        {
            var rc = new CanvasRenderContext(null);
            var svg = SvgExporter.ExportToString(this.Model, this.Plot.ActualWidth, this.Plot.ActualHeight, true, rc);
            Clipboard.SetText(svg);
        }

        public void CopyXaml()
        {
            var xaml = XamlExporter.ExportToString(
                this.Model, this.Plot.ActualWidth, this.Plot.ActualHeight, this.Model.Background);
            Clipboard.SetText(xaml);
        }

        public void Exit()
        {
            this.Owner.Close();
        }

        public void HelpAbout()
        {
            var dlg = new AboutDialog(this.Owner)
                {
                    Title = "About OxyPlot ExportDemo",
                    Image = new BitmapImage(new Uri(@"pack://application:,,,/ExportDemo;component/Images/oxyplot.png"))
                };
            dlg.Show();
        }

        public void HelpDocumentation()
        {
            Process.Start("http://oxyplot.codeplex.com/documentation");
        }

        public void HelpHome()
        {
            Process.Start("http://oxyplot.codeplex.com");
        }

        public void Print()
        {
            XpsExporter.Print(this.Model, this.Plot.ActualWidth, this.Plot.ActualHeight);
        }

        public void SaveDocxReport()
        {
            var path = this.GetFilename("Word document (.docx)|*.docx", ".docx");
            if (path != null)
            {
                this.SaveReport(path);
                OpenContainingFolder(path);
            }
        }

        public void SaveHtmlReport()
        {
            var path = this.GetFilename(".html files|*.html", ".html");
            if (path != null)
            {
                this.SaveReport(path);
                OpenContainingFolder(path);
            }
        }

        public void SaveLatexReport()
        {
            var path = this.GetFilename(".tex files|*.tex", ".tex");
            if (path != null)
            {
                this.SaveReport(path);
                OpenContainingFolder(path);
            }
        }

        public void SavePdf()
        {
            var path = this.GetFilename(".pdf files|*.pdf", ".pdf");
            if (path != null)
            {
                using (var stream = File.Create(path))
                {
                    OxyPlot.PdfExporter.Export(this.Model, stream, this.Plot.ActualWidth, this.Plot.ActualHeight);
                }

                OpenContainingFolder(path);
            }
        }

        public void SavePdf_PdfSharp()
        {
            var path = this.GetFilename(".pdf files|*.pdf", ".pdf");
            if (path != null)
            {
                OxyPlot.Pdf.PdfExporter.Export(this.Model, path, this.Plot.ActualWidth, this.Plot.ActualHeight);
                OpenContainingFolder(path);
            }
        }

        public void SavePdfReport()
        {
            var path = this.GetFilename(".pdf files|*.pdf", ".pdf");
            if (path != null)
            {
                this.SaveReport(path);
                OpenContainingFolder(path);
            }
        }

        public void SavePng()
        {
            var path = this.GetFilename(".png files|*.png", ".png");
            if (path != null)
            {
                this.Plot.SaveBitmap(path, 0, 0, OxyColors.Automatic);
                OpenContainingFolder(path);
            }
        }

        public void SaveReport(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            if (ext == null)
            {
                return;
            }

            ext = ext.ToLower();

            var r = this.CreateReport(fileName);
            var reportStyle = new ReportStyle();

            switch (ext)
            {
                case ".txt":
                    using (var s = File.Create(fileName))
                    {
                        using (var w = new TextReportWriter(s))
                        {
                            r.Write(w);
                        }
                    }

                    break;

                case ".html":
                    using (var s = File.Create(fileName))
                    {
                        using (var w = new HtmlReportWriter(s))
                        {
                            w.WriteReport(r, reportStyle);
                        }
                    }

                    break;

                case ".pdf":
                    using (var w = new PdfReportWriter(fileName))
                    {
                        w.WriteReport(r, reportStyle);
                    }

                    break;

                case ".rtf":
                    using (var w = new RtfReportWriter(fileName))
                    {
                        w.WriteReport(r, reportStyle);
                    }

                    break;

                case ".tex":
                    using (var s = File.Create(fileName))
                    {
                        using (var w = new LatexReportWriter(s, "Example report", "oxyplot"))
                        {
                            w.WriteReport(r, reportStyle);
                        }
                    }

                    break;

                case ".xps":
                    using (var w = new FlowDocumentReportWriter())
                    {
                        w.WriteReport(r, reportStyle);
                        w.Save(fileName);
                    }

                    break;
                case ".docx":
                    using (var w = new WordDocumentReportWriter(fileName))
                    {
                        w.WriteReport(r, reportStyle);
                        w.Save();
                    }

                    break;
            }
        }

        public void SaveRtfReport()
        {
            var path = this.GetFilename(".rtf files|*.rtf", ".rtf");
            if (path != null)
            {
                this.SaveReport(path);
                OpenContainingFolder(path);
            }
        }

        public void SaveSvg()
        {
            var path = this.GetFilename(".svg files|*.svg", ".svg");
            if (path != null)
            {
                // Using a WPF render context to measure the text
                var textMeasurer = new CanvasRenderContext(new Canvas());
                using (var s = File.Create(path))
                {
                    var exporter = new SvgExporter
                    {
                        Width = this.Plot.ActualWidth,
                        Height = this.Plot.ActualHeight,
                        IsDocument = true,
                        TextMeasurer = textMeasurer
                    };
                    exporter.Export(this.Model, s);
                }

                OpenContainingFolder(path);
            }
        }

        public void SaveTextReport()
        {
            var path = this.GetFilename("Text files (*.txt)|*.txt", ".txt");
            if (path != null)
            {
                this.SaveReport(path);
                OpenContainingFolder(path);
            }
        }

        public void SaveXaml()
        {
            var path = this.GetFilename(".xaml files|*.xaml", ".xaml");
            if (path != null)
            {
                XamlExporter.Export(this.Model, path, this.Plot.ActualWidth, this.Plot.ActualHeight, this.Model.Background);
                OpenContainingFolder(path);
            }
        }

        public void SaveXps()
        {
            var path = this.GetFilename(".xps files|*.xps", ".xps");
            if (path != null)
            {
                XpsExporter.Export(this.Model, path, this.Plot.ActualWidth, this.Plot.ActualHeight, this.Model.Background);
                OpenContainingFolder(path);
            }
        }

        public void SaveXpsReport()
        {
            var path = this.GetFilename(".xps files|*.xps", ".xps");
            if (path != null)
            {
                this.SaveReport(path);
                OpenContainingFolder(path);
            }
        }



        private static void OpenContainingFolder(string fileName)
        {
            // var folder = Path.GetDirectoryName(fileName);
            var psi = new ProcessStartInfo("Explorer.exe", "/select," + fileName);
            Process.Start(psi);
        }

        private Report CreateReport(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            ext = ext.ToLower();

            var r = new Report();
            r.Title = "Oxyplot example report";

            var main = new ReportSection();

            r.AddHeader(1, "Example report from OxyPlot");

            // r.AddHeader(2, "Content");
            // r.AddTableOfContents(main);
            r.Add(main);

            main.AddHeader(2, "Introduction");
            main.AddParagraph("The content in this file was generated by OxyPlot.");
            main.AddParagraph("See http://oxyplot.codeplex.com for more information.");

            var dir = Path.GetDirectoryName(fileName);
            var name = Path.GetFileNameWithoutExtension(fileName);

            string fileNameWithoutExtension = Path.Combine(dir, name);

            main.AddHeader(2, "Plot");
            main.AddParagraph("This plot was rendered to a file and included in the report as a plot.");
            main.AddPlot(this.Model, "Plot", 800, 500);

            main.AddHeader(2, "Drawing");
            main.AddParagraph("Not yet implemented.");

            /*            switch (ext)
                        {
                            case ".html":
                                {
                                    main.AddHeader(2, "Plot (svg)");
                                    main.AddParagraph("This plot was rendered to a .svg file and included in the report.");
                                    main.AddPlot(Model, "SVG plot", 800, 500);

                                    //main.AddHeader(2, "Drawing (vector)");
                                    //main.AddParagraph("This plot was rendered to SVG and included in the report as a drawing.");
                                    //var svg = Model.ToSvg(800, 500);
                                    //main.AddDrawing(svg, "SVG plot as a drawing.");
                                    break;
                                }
                            case ".pdf":
                            case ".tex":
                                {
                                    main.AddHeader(2, "Plot (vector)");
                                    main.AddParagraph("This plot was rendered to a .pdf file and included in the report.");
                                    main.AddPlot(Model, "PDF plot", 800, 500);
                                    //var pdfPlotFileName = fileNameWithoutExtension + "_plot.pdf";
                                    //PdfExporter.Export(Model, pdfPlotFileName, 800, 500);
                                    //main.AddParagraph("This plot was rendered to PDF and embedded in the report.");
                                    //main.AddImage(pdfPlotFileName, "PDF plot");
                                    break;
                                }
                            case ".docx":
                                {
                                    main.AddHeader(2, "Plot");
                                    main.AddParagraph("This plot was rendered to a .png file and included in the report.");
                                    main.AddPlot(Model, "Plot", 800, 500);
                                }
                                break;
                        }*/
            main.AddHeader(2, "Image");
            main.AddParagraph("The plot is rendered to a .png file and included in the report as an image. Zoom in to see the difference.");

            string pngPlotFileName = fileNameWithoutExtension + "_Plot2.png";
            PngExporter.Export(this.Model, pngPlotFileName, 800, 500, OxyColors.Automatic);
            main.AddImage(pngPlotFileName, "Plot as image");

            main.AddHeader(2, "Data");
            int i = 1;
            foreach (OxyPlot.Series.DataPointSeries s in this.Model.Series)
            {
                main.AddHeader(3, "Data series " + (i++));
                var pt = main.AddPropertyTable("Properties of the " + s.GetType().Name, new[] { s });
                pt.Fields[0].Width = 50;
                pt.Fields[1].Width = 100;

                var fields = new List<ItemsTableField>
                    {
                        new ItemsTableField("X", "X") { Width = 60, StringFormat = "0.00" },
                        new ItemsTableField("Y", "Y") { Width = 60, StringFormat = "0.00" }
                    };
                main.Add(new ItemsTable { Caption = "Data", Fields = fields, Items = s.Points });
            }

            // main.AddHeader(3, "Equations");
            // main.AddEquation(@"E = m \cdot c^2");
            // main.AddEquation(@"\oint \vec{B} \cdot d\vec{S} = 0");
            return r;
        }

        private string GetFilename(string filter, string defaultExt)
        {
            var dlg = new SaveFileDialog { Filter = filter, DefaultExt = defaultExt };
            return dlg.ShowDialog(this.Owner).Value ? dlg.FileName : null;
        }

    }
}