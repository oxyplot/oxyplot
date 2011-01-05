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

            DefineStyles(doc);
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
            if (t.Items == null)
                return;

            var table = new MigraDoc.DocumentObjectModel.Tables.Table();
            table.Borders.Width = 0.75;

            var cells=t.ToArray();
            int rows = cells.GetUpperBound(0) + 1;
            int columns = cells.GetUpperBound(1) + 1;

            for (int j = 0; j < columns;j++ )
            {
                var column = table.AddColumn();
                //    column.Format.Alignment = ConvertToParagraphAlignment(c.Alignment);
            }

            for (int i = 0; i <rows; i++)
            {
                var row = table.AddRow();                
                for (int j = 0; j < columns; j++)
                {
                    Cell cell = row.Cells[j];
                    cell.AddParagraph(cells[i, j]??"");
                    // if (i == 0) cell.Style = "TableHeader";
                }
            }

/*            Row header = table.AddRow();
            int i = 0;
            foreach (TableColumn c in t.Columns)
            {
                MigraDoc.DocumentObjectModel.Paragraph hp = header.Cells[i++].AddParagraph();
                hp.AddFormattedText(c.Header, "TableHeader");
            }
            foreach (object item in t.Items)
            {
                Row row = table.AddRow();
                i = 0;
                foreach (TableColumn c in t.Columns)
                {
                    Cell cell = row.Cells[i++];
                    string content = c.GetText(item);
                    cell.AddParagraph(content ?? "");
                }
            }
 */
            // table.SetEdge(0, 0, t.Columns.Count, t.Items.Count(), Edge.Box, BorderStyle.Single, 1.5, Colors.Black);
            MigraDoc.DocumentObjectModel.Paragraph pa = CurrentSection.AddParagraph();
            pa.AddFormattedText(t.FullCaption, "TableCaption");

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

        public void WriteDrawing(Drawing d)
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
            var r = new PdfDocumentRenderer {Document = doc};
            r.RenderDocument();
            r.PdfDocument.Save(filename);
        }

        /// <summary>
        /// Defines the styles used in the document.
        /// </summary>
        public static void DefineStyles(Document document)
        {
            // Get the predefined style Normal.
            Style style = document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Arial";

            // Heading1 to Heading9 are predefined styles with an outline level. An outline level
            // other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
            // in PDF.

            style = document.Styles["Heading1"];
            style.Font.Name = "Tahoma";
            style.Font.Size = 14;
            style.Font.Bold = true;
            style.Font.Color = Colors.DarkBlue;
            style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = 6;

            style = document.Styles["Heading2"];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 6;

            style = document.Styles["Heading3"];
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 3;

            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called TextBox based on style Normal
            style = document.Styles.AddStyle("TextBox", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.Borders.Width = 2.5;
            style.ParagraphFormat.Borders.Distance = "3pt";
            style.ParagraphFormat.Shading.Color = Colors.SkyBlue;

            // Create a new style called TOC based on style Normal
            style = document.Styles.AddStyle("TOC", "Normal");
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right, TabLeader.Dots);
            style.ParagraphFormat.Font.Color = Colors.Blue;

            style = document.Styles.AddStyle("TableHeader", "Normal");
            style.Font.Bold = true;

            style = document.Styles.AddStyle("TableCaption", "Normal");
            style.Font.Italic = true;

            style = document.Styles.AddStyle("FigureCaption", "Normal");
            style.Font.Italic = true;
        }

        public MigraDoc.DocumentObjectModel.Paragraph WriteStartFigure(Figure f)
        {
            return CurrentSection.AddParagraph();
        }

        public void WriteEndFigure(Figure f, MigraDoc.DocumentObjectModel.Paragraph pa)
        {
            pa.AddLineBreak();
            pa.AddFormattedText(f.FullCaption, "FigureCaption");
        }
    }
}