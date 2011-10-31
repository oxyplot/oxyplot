// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfReportWriter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf
{
    using System;
    using System.IO;

    using MigraDoc.DocumentObjectModel;
    using MigraDoc.DocumentObjectModel.Tables;
    using MigraDoc.Rendering;

    using OxyPlot.Reporting;

    using Paragraph = MigraDoc.DocumentObjectModel.Paragraph;
    using Table = OxyPlot.Reporting.Table;

    /// <summary>
    /// PDF report writer using MigraDoc.
    /// </summary>
    public class PdfReportWriter : IDisposable, IReportWriter
    {
        #region Constants and Fields

        /// <summary>
        ///   The doc.
        /// </summary>
        protected Document doc;

        /// <summary>
        ///   The filename.
        /// </summary>
        protected string filename;

        /// <summary>
        ///   The current section.
        /// </summary>
        private Section currentSection;

        /// <summary>
        ///   The style.
        /// </summary>
        private ReportStyle style;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfReportWriter"/> class.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        public PdfReportWriter(string filename)
        {
            this.filename = filename;
            this.doc = new Document();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets CurrentSection.
        /// </summary>
        private Section CurrentSection
        {
            get
            {
                if (this.currentSection == null)
                {
                    this.currentSection = this.doc.AddSection();
                }

                return this.currentSection;
            }

            set
            {
                this.currentSection = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Defines the styles used in the document.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <param name="reportStyle">
        /// The report Style.
        /// </param>
        public static void DefineStyles(Document document, ReportStyle reportStyle)
        {
            SetStyle(document.Styles["Normal"], reportStyle.BodyTextStyle);

            // Heading1 to Heading9 are predefined styles with an outline level. An outline level
            // other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
            // in PDF.
            for (int i = 0; i < reportStyle.HeaderStyles.Length; i++)
            {
                SetStyle(document.Styles["Heading" + (i + 1)], reportStyle.HeaderStyles[i]);
            }

            // style = document.Styles[StyleNames.Header];
            // style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            // style = document.Styles[StyleNames.Footer];
            // style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);
            Style style = document.Styles.AddStyle("TableHeader", "Normal");
            SetStyle(style, reportStyle.TableHeaderStyle);

            style = document.Styles.AddStyle("TableCaption", "Normal");
            SetStyle(style, reportStyle.TableCaptionStyle);

            style = document.Styles.AddStyle("TableText", "Normal");
            SetStyle(style, reportStyle.TableTextStyle);

            style = document.Styles.AddStyle("FigureText", "Normal");
            SetStyle(style, reportStyle.FigureTextStyle);
        }

        /// <summary>
        /// The close.
        /// </summary>
        public virtual void Close()
        {
            var r = new PdfDocumentRenderer { Document = this.doc };
            r.RenderDocument();
            r.PdfDocument.Save(this.filename);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// The write drawing.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        public void WriteDrawing(DrawingFigure d)
        {
            Paragraph p = this.WriteStartFigure(d);
            this.CurrentSection.AddParagraph("Drawings are not implemented.");
            this.WriteEndFigure(d, p);
        }

        /// <summary>
        /// The write end figure.
        /// </summary>
        /// <param name="f">
        /// The f.
        /// </param>
        /// <param name="pa">
        /// The pa.
        /// </param>
        public void WriteEndFigure(Figure f, Paragraph pa)
        {
            pa.AddLineBreak();
            pa.AddFormattedText(f.GetFullCaption(this.style), "FigureText");
        }

        /// <summary>
        /// The write equation.
        /// </summary>
        /// <param name="equation">
        /// The equation.
        /// </param>
        public void WriteEquation(Equation equation)
        {
            Paragraph p = this.CurrentSection.AddParagraph();
            p.AddText("Equations are not supported.");
        }

        /// <summary>
        /// The write header.
        /// </summary>
        /// <param name="h">
        /// The h.
        /// </param>
        public void WriteHeader(Header h)
        {
            if (h.Text == null)
            {
                return;
            }

            if (h.Level == 1)
            {
                this.currentSection = null;
            }

            this.CurrentSection.AddParagraph(h.Text, "Heading" + h.Level);
        }

        /// <summary>
        /// The write image.
        /// </summary>
        /// <param name="i">
        /// The i.
        /// </param>
        public void WriteImage(Image i)
        {
            Paragraph p = this.WriteStartFigure(i);
            if (i.Source != null)
            {
                MigraDoc.DocumentObjectModel.Shapes.Image pi = p.AddImage(Path.GetFullPath(i.Source));
                pi.Width = Unit.FromCentimeter(15);
            }

            this.WriteEndFigure(i, p);
        }

        /// <summary>
        /// The write paragraph.
        /// </summary>
        /// <param name="p">
        /// The p.
        /// </param>
        public void WriteParagraph(Reporting.Paragraph p)
        {
            this.CurrentSection.AddParagraph(p.Text ?? string.Empty);
        }

        /// <summary>
        /// The write plot.
        /// </summary>
        /// <param name="plot">
        /// The plot.
        /// </param>
        public void WritePlot(PlotFigure plot)
        {
            Paragraph p = this.WriteStartFigure(plot);
            p.AddText("Plot drawing is not implemented yet.");
            this.WriteEndFigure(plot, p);
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
            this.style = style;
            DefineStyles(this.doc, style);
            report.Write(this);
        }

        /// <summary>
        /// The write start figure.
        /// </summary>
        /// <param name="f">
        /// The f.
        /// </param>
        /// <returns>
        /// </returns>
        public Paragraph WriteStartFigure(Figure f)
        {
            return this.CurrentSection.AddParagraph();
        }

        /// <summary>
        /// The write table.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        public void WriteTable(Table t)
        {
            if (t.Rows == null)
            {
                return;
            }

            var table = new MigraDoc.DocumentObjectModel.Tables.Table { Borders = { Width = 0.75 } };

            int columns = t.Columns.Count;

            for (int j = 0; j < columns; j++)
            {
                Column column = table.AddColumn();
                column.Width = Unit.FromMillimeter(t.Columns[j].ActualWidth);
                column.Format.Alignment = ConvertToParagraphAlignment(t.Columns[j].Alignment);
            }

            foreach (var tr in t.Rows)
            {
                Row row = table.AddRow();
                for (int j = 0; j < columns; j++)
                {
                    bool isHeader = tr.IsHeader || t.Columns[j].IsHeader;

                    TableCell c = tr.Cells[j];
                    Cell cell = row.Cells[j];
                    cell.AddParagraph(c.Content ?? string.Empty);
                    cell.Style = isHeader ? "TableHeader" : "TableText";
                }
            }

            // table.SetEdge(0, 0, t.Columns.Count, t.Items.Count(), Edge.Box, BorderStyle.Single, 1.5, Colors.Black);
            Paragraph pa = this.CurrentSection.AddParagraph();
            pa.AddFormattedText(t.GetFullCaption(this.style), "TableCaption");

            this.CurrentSection.Add(table);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The convert to paragraph alignment.
        /// </summary>
        /// <param name="alignment">
        /// The alignment.
        /// </param>
        /// <returns>
        /// </returns>
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

        /// <summary>
        /// The set style.
        /// </summary>
        /// <param name="style">
        /// The style.
        /// </param>
        /// <param name="ps">
        /// The ps.
        /// </param>
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

        /// <summary>
        /// The to migra doc color.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        /// <returns>
        /// </returns>
        private static Color ToMigraDocColor(OxyColor c)
        {
            return new Color(c.A, c.R, c.G, c.B);
        }

        #endregion
    }
}