using System;
using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using OxyPlot.Reporting;
using Paragraph = OxyPlot.Reporting.Paragraph;
using Table = OxyPlot.Reporting.Table;

namespace OxyPlot.Pdf
{
    /// <summary>
    /// PDF report writer using MigraDoc.
    /// </summary>
    public class PdfReportWriter : IDisposable, IReportWriter
    {
        private Section currentSection;
        protected Document doc;
        protected string filename;

        public PdfReportWriter(string filename)
        {
            this.filename = filename;
            doc = new Document();

        }

        private Section CurrentSection
        {
            get
            {
                if (currentSection == null)
                    currentSection = doc.AddSection();
                return currentSection;
            }
            set { currentSection = value; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion

        #region IReportWriter Members

        private ReportStyle style;
        public void WriteReport(Report report, ReportStyle style)
        {
            this.style = style;
            DefineStyles(doc, style);
            report.Write(this);
        }

        public void WriteHeader(Header h)
        {
            if (h.Text == null)
                return;

            if (h.Level == 1)
                currentSection = null;

            CurrentSection.AddParagraph(h.Text, "Heading" + h.Level);
        }

        public void WriteParagraph(Paragraph p)
        {
            CurrentSection.AddParagraph(p.Text ?? "");
        }

        public void WriteTable(Table t)
        {
            if (t.Rows == null)
                return;

            var table = new MigraDoc.DocumentObjectModel.Tables.Table { Borders = { Width = 0.75 } };

            int columns = t.Columns.Count;

            for (int j = 0; j < columns; j++)
            {
                var column = table.AddColumn();
                column.Width = Unit.FromMillimeter(t.Columns[j].ActualWidth);
                column.Format.Alignment = ConvertToParagraphAlignment(t.Columns[j].Alignment);
            }

            foreach (var tr in t.Rows)
            {
                var row = table.AddRow();
                for (int j = 0; j < columns; j++)
                {
                    bool isHeader = tr.IsHeader || t.Columns[j].IsHeader;

                    var c = tr.Cells[j];
                    Cell cell = row.Cells[j];
                    cell.AddParagraph(c.Content ?? "");
                    cell.Style = isHeader ? "TableHeader" : "TableText";
                }
            }

            // table.SetEdge(0, 0, t.Columns.Count, t.Items.Count(), Edge.Box, BorderStyle.Single, 1.5, Colors.Black);
            MigraDoc.DocumentObjectModel.Paragraph pa = CurrentSection.AddParagraph();
            pa.AddFormattedText(t.GetFullCaption(style), "TableCaption");

            CurrentSection.Add(table);
        }

        private static ParagraphAlignment ConvertToParagraphAlignment(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Left:
                    return ParagraphAlignment.Left;
                case Alignment.Center:
                    return ParagraphAlignment.Center;
                case Alignment.Right:
                    return ParagraphAlignment.Right;
                default:
                    return ParagraphAlignment.Justify;
            }
        }

        public void WriteImage(Image i)
        {
            MigraDoc.DocumentObjectModel.Paragraph p = WriteStartFigure(i);
            if (i.Source != null)
            {
                MigraDoc.DocumentObjectModel.Shapes.Image pi = p.AddImage(Path.GetFullPath(i.Source));
                pi.Width = Unit.FromCentimeter(15);
            }
            WriteEndFigure(i, p);
        }

        public void WriteDrawing(DrawingFigure d)
        {
            MigraDoc.DocumentObjectModel.Paragraph p = WriteStartFigure(d);
            CurrentSection.AddParagraph("Drawings are not implemented.");
            WriteEndFigure(d, p);
        }

        public void WritePlot(PlotFigure plot)
        {
            MigraDoc.DocumentObjectModel.Paragraph p = WriteStartFigure(plot);
            p.AddText("Plot drawing is not implemented yet.");
            WriteEndFigure(plot, p);
        }

        public void WriteEquation(Equation equation)
        {
            MigraDoc.DocumentObjectModel.Paragraph p = CurrentSection.AddParagraph();
            p.AddText("Equations are not supported.");
        }

        #endregion

        public virtual void Close()
        {
            var r = new PdfDocumentRenderer { Document = doc };
            r.RenderDocument();
            r.PdfDocument.Save(filename);
        }

        /// <summary>
        /// Defines the styles used in the document.
        /// </summary>
        public static void DefineStyles(Document document, ReportStyle reportStyle)
        {
            SetStyle(document.Styles["Normal"], reportStyle.BodyTextStyle);

            // Heading1 to Heading9 are predefined styles with an outline level. An outline level
            // other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
            // in PDF.

            for (int i = 0; i < reportStyle.HeaderStyles.Length; i++)
                SetStyle(document.Styles["Heading" + (i + 1)], reportStyle.HeaderStyles[i]);

            //style = document.Styles[StyleNames.Header];
            //style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            //style = document.Styles[StyleNames.Footer];
            //style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            var style = document.Styles.AddStyle("TableHeader", "Normal");
            SetStyle(style, reportStyle.TableHeaderStyle);

            style = document.Styles.AddStyle("TableCaption", "Normal");
            SetStyle(style, reportStyle.TableCaptionStyle);

            style = document.Styles.AddStyle("TableText", "Normal");
            SetStyle(style, reportStyle.TableTextStyle);

            style = document.Styles.AddStyle("FigureText", "Normal");
            SetStyle(style, reportStyle.FigureTextStyle);

        }

        private static void SetStyle(Style style, ParagraphStyle ps)
        {
            style.Font.Name = ps.FontFamily;
            style.Font.Size = Unit.FromPoint(ps.FontSize);
            style.Font.Bold = ps.Bold;
            style.Font.Italic = ps.Italic;
            style.Font.Color = ToMigraDocColor(ps.TextColor);
            style.ParagraphFormat.PageBreakBefore = ps.PageBreakBefore;
            style.ParagraphFormat.LeftIndent = Unit.FromPoint(ps.LeftIndentation);
            style.ParagraphFormat.RightIndent = Unit.FromPoint(ps.RightIndentation);
            style.ParagraphFormat.LineSpacing = Unit.FromPoint(ps.LineSpacing);
            style.ParagraphFormat.SpaceBefore = Unit.FromPoint(ps.SpacingBefore);
            style.ParagraphFormat.SpaceAfter = Unit.FromPoint(ps.SpacingAfter);
        }

        private static Color ToMigraDocColor(OxyColor c)
        {
            return new Color(c.A, c.R, c.G, c.B);
        }

        public MigraDoc.DocumentObjectModel.Paragraph WriteStartFigure(Figure f)
        {
            return CurrentSection.AddParagraph();
        }

        public void WriteEndFigure(Figure f, MigraDoc.DocumentObjectModel.Paragraph pa)
        {
            pa.AddLineBreak();
            pa.AddFormattedText(f.GetFullCaption(style), "FigureText");
        }
    }
}