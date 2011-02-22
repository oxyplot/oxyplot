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
using Drawing = OxyPlot.Reporting.Drawing;
using Figure = System.Windows.Documents.Figure;
using Image = OxyPlot.Reporting.Image;
using Paragraph = OxyPlot.Reporting.Paragraph;
using Table = OxyPlot.Reporting.Table;

namespace OxyPlot.Pdf
{
    /// <summary>
    /// XPS report writer using MigraDoc.
    /// </summary>
    public class FlowDocumentReportWriter : IDisposable, IReportWriter
    {
        protected string filename;
        private FlowDocument doc;
        public FlowDocument FlowDocument { get { return doc; } }
        public ReportStyle Style { get; set; }

        public FlowDocumentReportWriter(ReportStyle style)
        {
            Style = style;
            doc = new FlowDocument();
            Margins = new Thickness(30);
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IReportWriter Members

        public void WriteHeader(Header h)
        {
            var run = new Run() { Text = h.Text };
            SetStyle(run, Style.HeaderStyles[h.Level - 1]);
            var p = new System.Windows.Documents.Paragraph(run);
            doc.Blocks.Add(p);
        }

        private void SetStyle(TextElement run, ParagraphStyle s)
        {
            run.FontFamily = new FontFamily(s.FontFamily);
            run.FontSize = s.FontSize;
            run.FontWeight = s.Bold ? FontWeights.Bold : FontWeights.Normal;
            var fontStyle = FontStyles.Normal;
            if (s.Italic)
                fontStyle = FontStyles.Italic;
            run.FontStyle = fontStyle;
        }

        public void WriteParagraph(Paragraph pa)
        {
            doc.Blocks.Add(CreateParagraph(pa.Text,Style.BodyTextStyle));
        }

        private System.Windows.Documents.Paragraph CreateParagraph(string text, ParagraphStyle style)
        {
            var run = new Run() { Text = text };
            if (style!=null)
                SetStyle(run, style);
            return new System.Windows.Documents.Paragraph(run);
        }

        public void WriteTable(Table t)
        {
            var p = new System.Windows.Documents.Paragraph();
            var figure = new System.Windows.Documents.Figure();
            var table = new System.Windows.Documents.Table();
            WriteTableItems(t, table);
            figure.Blocks.Add(CreateParagraph(t.Caption, Style.FigureTextStyle));
            figure.Blocks.Add(table);
            p.Inlines.Add(figure);
            doc.Blocks.Add(p);
        }

        /// <summary>
        /// Writes the items.
        /// </summary>
        /// <param name="t">The t.</param>
        public void WriteTableItems(Table t, System.Windows.Documents.Table table)
        {
            var items = t.Items;
            var columns = t.Columns;


            if (t.HasHeader())
            {
                var trg1 = new TableRowGroup();
                SetStyle(trg1, Style.TableHeaderStyle);
                var r = new TableRow();
                foreach (var c in columns)
                {
                    var cell = new TableCell();
                    var run = new Run() { Text = c.Header };
                    cell.Blocks.Add(new System.Windows.Documents.Paragraph(run));
                    r.Cells.Add(cell);
                }
                trg1.Rows.Add(r);
                table.RowGroups.Add(trg1);
            }

            var trg2 = new TableRowGroup();
            SetStyle(trg2, Style.TableTextStyle);
            foreach (var item in items)
            {
                var r = new TableRow();
                foreach (var c in columns)
                {
                    var text = c.GetText(item);
                    var cell = new TableCell();
                    var run = new Run() { Text = text };
                    cell.Blocks.Add(new System.Windows.Documents.Paragraph(run));
                    r.Cells.Add(cell);
                }
                trg2.Rows.Add(r);
            }
            table.RowGroups.Add(trg2);
        }


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
            doc.Blocks.Add(c);
        }

        public void WriteDrawing(Drawing d)
        {
        }

        public void WritePlot(PlotFigure plot)
        {
        }

        public void WriteEquation(Equation equation)
        {
        }

        #endregion

        public Thickness Margins { get; set; }

        public void Print()
        {
            PrintDocumentImageableArea area = null;
            var xpsdw = PrintQueue.CreateXpsDocumentWriter(ref area);
            if (xpsdw != null)
            {
                xpsdw.Write(CreateFixedDocument(new Size(area.ExtentWidth, area.ExtentHeight)));
            }
        }

        private FixedDocument CreateFixedDocument(Size size)
        {
            IDocumentPaginatorSource dps = doc;
            DocumentPaginator sourceFlowDocPaginator = dps.DocumentPaginator;
            sourceFlowDocPaginator.PageSize = new Size(size.Width - Margins.Left - Margins.Right, size.Height - Margins.Top - Margins.Bottom);
            if (!sourceFlowDocPaginator.IsPageCountValid)
                sourceFlowDocPaginator.ComputePageCount();
            return BuildFixedDocument(sourceFlowDocPaginator, size, Margins);
        }

        public virtual void Save(string filename, double width = 816, double height = 1056)
        {
            using (var package = Package.Open(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var doc = new XpsDocument(package))
                {
                    var writer = XpsDocument.CreateXpsDocumentWriter(doc);
                    writer.Write(CreateFixedDocument(new Size(width, height)));
                }
            }
        }

        private FixedDocument BuildFixedDocument(DocumentPaginator sourceFlowDocPaginator, Size size, Thickness margins)
        {
            var fixedDocument = new FixedDocument();
            for (int pageNo = 0; pageNo < sourceFlowDocPaginator.PageCount; pageNo++)
            {
                var pageCanvas = new Canvas { Margin = margins };
                AddPageBody(sourceFlowDocPaginator, pageNo, pageCanvas, margins);
                AddPageToDocument(fixedDocument, pageCanvas, size);
            }
            return fixedDocument;
        }

        private void AddPageToDocument(FixedDocument fixedDocument, Canvas pageCanvas, Size pageSize)
        {
            var fp = new FixedPage { Width = pageSize.Width, Height = pageSize.Height };
            fp.Children.Add(pageCanvas);
            var pc = new PageContent();
            ((IAddChild)pc).AddChild(fp);
            fixedDocument.Pages.Add(pc);
        }

        private void AddPageBody(DocumentPaginator sourceFlowDocPaginator, int pageNo, Canvas pageCanvas, Thickness margins)
        {
            var dpv = new DocumentPageView();
            dpv.DocumentPaginator = sourceFlowDocPaginator;
            dpv.PageNumber = pageNo;
            Canvas.SetTop(dpv, margins.Top);
            Canvas.SetLeft(dpv, margins.Left);
            pageCanvas.Children.Add(dpv);
        }
    }
}