// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlowDocumentReportWriter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf
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
    /// XPS report writer using MigraDoc.
    /// </summary>
    public class FlowDocumentReportWriter : IDisposable, IReportWriter
    {
        #region Constants and Fields

        /// <summary>
        ///   The doc.
        /// </summary>
        private readonly FlowDocument doc;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "FlowDocumentReportWriter" /> class.
        /// </summary>
        public FlowDocumentReportWriter()
        {
            this.doc = new FlowDocument();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets FlowDocument.
        /// </summary>
        public FlowDocument FlowDocument
        {
            get
            {
                return this.doc;
            }
        }

        /// <summary>
        ///   Gets or sets Style.
        /// </summary>
        public ReportStyle Style { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// The print.
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
        /// The save.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public virtual void Save(string filename, double width = 816, double height = 1056)
        {
            using (Package package = Package.Open(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var doc = new XpsDocument(package))
                {
                    XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                    writer.Write(this.CreateFixedDocument(new Size(width, height)));
                }
            }
        }

        /// <summary>
        /// The write drawing.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        public void WriteDrawing(DrawingFigure d)
        {
        }

        /// <summary>
        /// The write equation.
        /// </summary>
        /// <param name="equation">
        /// The equation.
        /// </param>
        public void WriteEquation(Equation equation)
        {
        }

        /// <summary>
        /// The write header.
        /// </summary>
        /// <param name="h">
        /// The h.
        /// </param>
        public void WriteHeader(Header h)
        {
            var run = new Run { Text = h.Text };
            SetStyle(run, this.Style.HeaderStyles[h.Level - 1]);
            var p = new System.Windows.Documents.Paragraph(run);
            this.doc.Blocks.Add(p);
        }

        /// <summary>
        /// The write image.
        /// </summary>
        /// <param name="i">
        /// The i.
        /// </param>
        public void WriteImage(Image i)
        {
            // var figure = new Figure();
            var img = new System.Windows.Controls.Image();
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(Path.GetFullPath(i.Source), UriKind.Absolute);
            bi.EndInit();
            img.Source = bi;
            var c = new BlockUIContainer(img);
            c.Child = img;
            this.doc.Blocks.Add(c);
        }

        /// <summary>
        /// The write paragraph.
        /// </summary>
        /// <param name="pa">
        /// The pa.
        /// </param>
        public void WriteParagraph(Paragraph pa)
        {
            this.doc.Blocks.Add(this.CreateParagraph(pa.Text, this.Style.BodyTextStyle));
        }

        /// <summary>
        /// The write plot.
        /// </summary>
        /// <param name="plot">
        /// The plot.
        /// </param>
        public void WritePlot(PlotFigure plot)
        {
        }

        /// <summary>
        /// The write report.
        /// </summary>
        /// <param name="report">
        /// The report.
        /// </param>
        /// <param name="style">
        /// The style.
        /// </param>
        public void WriteReport(Report report, ReportStyle style)
        {
            this.Style = style;
            report.Write(this);
        }

        /// <summary>
        /// The write table.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
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

        #endregion

        #region Methods

        /// <summary>
        /// The set style.
        /// </summary>
        /// <param name="run">
        /// The run.
        /// </param>
        /// <param name="s">
        /// The s.
        /// </param>
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
        /// The add page body.
        /// </summary>
        /// <param name="sourceFlowDocPaginator">
        /// The source flow doc paginator.
        /// </param>
        /// <param name="pageNo">
        /// The page no.
        /// </param>
        /// <param name="pageCanvas">
        /// The page canvas.
        /// </param>
        /// <param name="margins">
        /// The margins.
        /// </param>
        private void AddPageBody(
            DocumentPaginator sourceFlowDocPaginator, int pageNo, Canvas pageCanvas, Thickness margins)
        {
            var dpv = new DocumentPageView();
            dpv.DocumentPaginator = sourceFlowDocPaginator;
            dpv.PageNumber = pageNo;
            Canvas.SetTop(dpv, margins.Top);
            Canvas.SetLeft(dpv, margins.Left);
            pageCanvas.Children.Add(dpv);
        }

        /// <summary>
        /// The add page to document.
        /// </summary>
        /// <param name="fixedDocument">
        /// The fixed document.
        /// </param>
        /// <param name="pageCanvas">
        /// The page canvas.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        private void AddPageToDocument(FixedDocument fixedDocument, Canvas pageCanvas, Size pageSize)
        {
            var fp = new FixedPage { Width = pageSize.Width, Height = pageSize.Height };
            fp.Children.Add(pageCanvas);
            var pc = new PageContent();
            ((IAddChild)pc).AddChild(fp);
            fixedDocument.Pages.Add(pc);
        }

        /// <summary>
        /// The build fixed document.
        /// </summary>
        /// <param name="sourceFlowDocPaginator">
        /// The source flow doc paginator.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <param name="margins">
        /// The margins.
        /// </param>
        /// <returns>
        /// </returns>
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
        /// The create fixed document.
        /// </summary>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <returns>
        /// </returns>
        private FixedDocument CreateFixedDocument(Size size)
        {
            IDocumentPaginatorSource dps = this.doc;
            DocumentPaginator sourceFlowDocPaginator = dps.DocumentPaginator;
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
        /// The create paragraph.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="style">
        /// The style.
        /// </param>
        /// <returns>
        /// </returns>
        private System.Windows.Documents.Paragraph CreateParagraph(string text, ParagraphStyle style)
        {
            var run = new Run { Text = text };
            if (style != null)
            {
                SetStyle(run, style);
            }

            return new System.Windows.Documents.Paragraph(run);
        }

        #endregion
    }
}