// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlowDocumentReportWriter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an XPS report writer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.IO;
    using System.IO.Packaging;
    using System.Printing;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Xps;
    using System.Windows.Xps.Packaging;

    using OxyPlot.Reporting;

    using Figure = System.Windows.Documents.Figure;
    using Image = OxyPlot.Reporting.Image;
    using Paragraph = OxyPlot.Reporting.Paragraph;
    using Table = OxyPlot.Reporting.Table;
    using TableCell = OxyPlot.Reporting.TableCell;
    using TableRow = System.Windows.Documents.TableRow;

    /// <summary>
    /// Provides an XPS report writer.
    /// </summary>
    public class FlowDocumentReportWriter : IDisposable, IReportWriter
    {
        /// <summary>
        /// The document.
        /// </summary>
        private readonly FlowDocument doc;

        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlowDocumentReportWriter" /> class.
        /// </summary>
        public FlowDocumentReportWriter()
        {
            this.doc = new FlowDocument();
        }

        /// <summary>
        /// Gets the FlowDocument.
        /// </summary>
        public FlowDocument FlowDocument
        {
            get
            {
                return this.doc;
            }
        }

        /// <summary>
        /// Gets or sets the report style.
        /// </summary>
        public ReportStyle Style { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Prints the document.
        /// </summary>
        public void Print()
        {
            PrintDocumentImageableArea area = null;
            XpsDocumentWriter xpsdw = PrintQueue.CreateXpsDocumentWriter(ref area);
            if (xpsdw != null)
            {
                xpsdw.Write(this.CreateFixedDocument(new Size(area.ExtentWidth, area.ExtentHeight)));
            }
        }

        /// <summary>
        /// Saves the document.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public virtual void Save(string filename, double width = 816, double height = 1056)
        {
            using (var package = Package.Open(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var xpsdoc = new XpsDocument(package))
                {
                    var writer = XpsDocument.CreateXpsDocumentWriter(xpsdoc);
                    writer.Write(this.CreateFixedDocument(new Size(width, height)));
                }
            }
        }

        /// <summary>
        /// Writes a drawing.
        /// </summary>
        /// <param name="d">The drawing.</param>
        public void WriteDrawing(DrawingFigure d)
        {
        }

        /// <summary>
        /// Writes an equation.
        /// </summary>
        /// <param name="equation">The equation.</param>
        public void WriteEquation(Equation equation)
        {
        }

        /// <summary>
        /// Writes a header.
        /// </summary>
        /// <param name="h">The header.</param>
        public void WriteHeader(Header h)
        {
            var run = new Run { Text = h.Text };
            SetStyle(run, this.Style.HeaderStyles[h.Level - 1]);
            var p = new System.Windows.Documents.Paragraph(run);
            this.doc.Blocks.Add(p);
        }

        /// <summary>
        /// Writes an image.
        /// </summary>
        /// <param name="i">The image.</param>
        public void WriteImage(Image i)
        {
            // var figure = new Figure();
            var img = new System.Windows.Controls.Image();
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(Path.GetFullPath(i.Source), UriKind.Absolute);
            bi.EndInit();
            img.Source = bi;
            var c = new BlockUIContainer(img) { Child = img };
            this.doc.Blocks.Add(c);
        }

        /// <summary>
        /// Writes a paragraph.
        /// </summary>
        /// <param name="pa">The paragraph.</param>
        public void WriteParagraph(Paragraph pa)
        {
            this.doc.Blocks.Add(this.CreateParagraph(pa.Text, this.Style.BodyTextStyle));
        }

        /// <summary>
        /// Writes a plot.
        /// </summary>
        /// <param name="plot">The plot.</param>
        public void WritePlot(PlotFigure plot)
        {
        }

        /// <summary>
        /// Writes a report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="reportStyle">The style.</param>
        public void WriteReport(Report report, ReportStyle reportStyle)
        {
            this.Style = reportStyle;
            report.Write(this);
        }

        /// <summary>
        /// Writes a table.
        /// </summary>
        /// <param name="t">The t.</param>
        public void WriteTable(Table t)
        {
            var p = new System.Windows.Documents.Paragraph();
            var figure = new Figure();
            var table = new System.Windows.Documents.Table();

            // if (t.HasHeader())
            // {
            // var trg1 = new TableRowGroup();
            // SetStyle(trg1, Style.TableHeaderStyle);
            // var r = new TableRow();
            // foreach (var c in columns)
            // {
            // var cell = new TableCell();
            // var run = new Run() { Text = c.Header };
            // cell.Blocks.Add(new System.Windows.Documents.Paragraph(run));
            // r.Cells.Add(cell);
            // }
            // trg1.Rows.Add(r);
            // table.RowGroups.Add(trg1);
            // }
            var trg2 = new TableRowGroup();

            // SetStyle(trg2, Style.TableTextStyle);
            foreach (var row in t.Rows)
            {
                var r = new TableRow();
                if (row.IsHeader)
                {
                    SetStyle(r, row.IsHeader ? this.Style.TableHeaderStyle : this.Style.TableTextStyle);
                }

                for (int j = 0; j < t.Columns.Count; j++)
                {
                    TableCell c = row.Cells[j];
                    var cell = new System.Windows.Documents.TableCell();
                    var run = new Run { Text = c.Content };
                    cell.Blocks.Add(new System.Windows.Documents.Paragraph(run));
                    r.Cells.Add(cell);
                }

                trg2.Rows.Add(r);
            }

            table.RowGroups.Add(trg2);

            figure.Blocks.Add(this.CreateParagraph(t.Caption, this.Style.FigureTextStyle));
            figure.Blocks.Add(table);
            p.Inlines.Add(figure);
            this.doc.Blocks.Add(p);
        }

        /// <summary>
        /// The set style.
        /// </summary>
        /// <param name="run">The run.</param>
        /// <param name="s">The s.</param>
        private static void SetStyle(TextElement run, ParagraphStyle s)
        {
            run.FontFamily = new FontFamily(s.FontFamily);
            run.FontSize = s.FontSize;
            run.FontWeight = s.Bold ? FontWeights.Bold : FontWeights.Normal;
            FontStyle fontStyle = FontStyles.Normal;
            if (s.Italic)
            {
                fontStyle = FontStyles.Italic;
            }

            run.FontStyle = fontStyle;
        }

        /// <summary>
        /// Adds a page body.
        /// </summary>
        /// <param name="sourceFlowDocPaginator">The source flow document paginator.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageCanvas">The page canvas.</param>
        /// <param name="margins">The margins.</param>
        private void AddPageBody(DocumentPaginator sourceFlowDocPaginator, int pageNumber, Canvas pageCanvas, Thickness margins)
        {
            var dpv = new DocumentPageView { DocumentPaginator = sourceFlowDocPaginator, PageNumber = pageNumber };
            Canvas.SetTop(dpv, margins.Top);
            Canvas.SetLeft(dpv, margins.Left);
            pageCanvas.Children.Add(dpv);
        }

        /// <summary>
        /// Adds a page to the document.
        /// </summary>
        /// <param name="fixedDocument">The fixed document.</param>
        /// <param name="pageCanvas">The page canvas.</param>
        /// <param name="pageSize">The page size.</param>
        private void AddPageToDocument(FixedDocument fixedDocument, Canvas pageCanvas, Size pageSize)
        {
            var fp = new FixedPage { Width = pageSize.Width, Height = pageSize.Height };
            fp.Children.Add(pageCanvas);
            var pc = new PageContent();
            ((IAddChild)pc).AddChild(fp);
            fixedDocument.Pages.Add(pc);
        }

        /// <summary>
        /// Builds the fixed document.
        /// </summary>
        /// <param name="sourceFlowDocPaginator">The source flow doc paginator.</param>
        /// <param name="size">The size.</param>
        /// <param name="margins">The margins.</param>
        /// <returns>The document.</returns>
        private FixedDocument BuildFixedDocument(DocumentPaginator sourceFlowDocPaginator, Size size, Thickness margins)
        {
            var fixedDocument = new FixedDocument();
            for (int pageNo = 0; pageNo < sourceFlowDocPaginator.PageCount; pageNo++)
            {
                var pageCanvas = new Canvas { Margin = margins };
                this.AddPageBody(sourceFlowDocPaginator, pageNo, pageCanvas, margins);
                this.AddPageToDocument(fixedDocument, pageCanvas, size);
            }

            return fixedDocument;
        }

        /// <summary>
        /// Creates the fixed document.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>The document.</returns>
        private FixedDocument CreateFixedDocument(Size size)
        {
            IDocumentPaginatorSource dps = this.doc;
            var sourceFlowDocPaginator = dps.DocumentPaginator;
            sourceFlowDocPaginator.PageSize = new Size(
                size.Width - this.Style.Margins.Left - this.Style.Margins.Right,
                size.Height - this.Style.Margins.Top - this.Style.Margins.Bottom);
            if (!sourceFlowDocPaginator.IsPageCountValid)
            {
                sourceFlowDocPaginator.ComputePageCount();
            }

            var margins = new Thickness(
                this.Style.Margins.Left, this.Style.Margins.Top, this.Style.Margins.Width, this.Style.Margins.Height);
            return this.BuildFixedDocument(sourceFlowDocPaginator, size, margins);
        }

        /// <summary>
        /// Creates a paragraph.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="style">The style.</param>
        /// <returns>A paragraph.</returns>
        private System.Windows.Documents.Paragraph CreateParagraph(string text, ParagraphStyle style)
        {
            var run = new Run { Text = text };
            if (style != null)
            {
                SetStyle(run, style);
            }

            return new System.Windows.Documents.Paragraph(run);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // nothing...
                }
            }

            this.disposed = true;
        }
    }
}