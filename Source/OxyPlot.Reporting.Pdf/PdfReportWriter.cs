// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfReportWriter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a report writer for portable document format using MigraDoc.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using MigraDoc.DocumentObjectModel;
    using MigraDoc.Rendering;

    using OxyPlot.Reporting;

    using Paragraph = MigraDoc.DocumentObjectModel.Paragraph;

    /// <summary>
    /// Provides a report writer for portable document format using MigraDoc.
    /// </summary>
    public class PdfReportWriter : IDisposable, IReportWriter
    {
        /// <summary>
        /// List of plot files created during report generation.
        /// </summary>
        private readonly List<string> temporaryPlotFiles = new List<string>();

        /// <summary>
        /// The current section.
        /// </summary>
        private Section currentSection;

        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Flag if the document has been closed.
        /// </summary>
        private bool isClosed;

        /// <summary>
        /// The style.
        /// </summary>
        private ReportStyle style;

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfReportWriter" /> class.
        /// </summary>
        /// <param name="filename">The FileName.</param>
        public PdfReportWriter(string filename)
            : this(File.Create(filename))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfReportWriter" /> class.
        /// </summary>
        /// <param name="output">The output <see cref="Stream" />.</param>
        public PdfReportWriter(Stream output)
        {
            this.Output = output;
            this.Document = new Document();
        }

        /// <summary>
        /// Gets or sets the pdf document.
        /// </summary>
        protected Document Document { get; set; }

        /// <summary>
        /// Gets the output stream.
        /// </summary>
        protected Stream Output { get; private set; }

        /// <summary>
        /// Gets the current section.
        /// </summary>
        private Section CurrentSection
        {
            get
            {
                return this.currentSection ?? (this.currentSection = this.Document.AddSection());
            }
        }

        /// <summary>
        /// Defines the styles used in the document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="reportStyle">The report Style.</param>
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
            var style = document.Styles.AddStyle("TableHeader", "Normal");
            SetStyle(style, reportStyle.TableHeaderStyle);

            style = document.Styles.AddStyle("TableCaption", "Normal");
            SetStyle(style, reportStyle.TableCaptionStyle);

            style = document.Styles.AddStyle("TableText", "Normal");
            SetStyle(style, reportStyle.TableTextStyle);

            style = document.Styles.AddStyle("FigureText", "Normal");
            SetStyle(style, reportStyle.FigureTextStyle);
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public virtual void Close()
        {
            if (this.isClosed)
            {
                return;
            }

            var r = new PdfDocumentRenderer { Document = this.Document };
            r.RenderDocument();
            r.PdfDocument.Save(this.Output);
            this.CleanTemporaryPlotFiles();
            this.Output.Close();
            this.isClosed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Writes the specified drawing.
        /// </summary>
        /// <param name="d">The drawing.</param>
        void IReportWriter.WriteDrawing(DrawingFigure d)
        {
            var p = this.WriteStartFigure(d);
            this.CurrentSection.AddParagraph("Drawings are not implemented.");
            this.WriteEndFigure(d, p);
        }

        /// <summary>
        /// Writes the specified equation.
        /// </summary>
        /// <param name="equation">The equation.</param>
        void IReportWriter.WriteEquation(Equation equation)
        {
            var p = this.CurrentSection.AddParagraph();
            p.AddText("Equations are not supported.");
        }

        /// <summary>
        /// Writes the specified header.
        /// </summary>
        /// <param name="h">The header.</param>
        void IReportWriter.WriteHeader(Header h)
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
        /// Writes the specified image.
        /// </summary>
        /// <param name="i">The image.</param>
        void IReportWriter.WriteImage(Image i)
        {
            var p = this.WriteStartFigure(i);
            if (i.Source != null)
            {
                MigraDoc.DocumentObjectModel.Shapes.Image pi = p.AddImage(Path.GetFullPath(i.Source));
                pi.Width = Unit.FromCentimeter(15);
            }

            this.WriteEndFigure(i, p);
        }

        /// <summary>
        /// Writes the specified paragraph.
        /// </summary>
        /// <param name="p">The paragraph.</param>
        void IReportWriter.WriteParagraph(Reporting.Paragraph p)
        {
            this.CurrentSection.AddParagraph(p.Text ?? string.Empty);
        }

        /// <summary>
        /// Writes the specified plot.
        /// </summary>
        /// <param name="plot">The plot.</param>
        void IReportWriter.WritePlot(PlotFigure plot)
        {
            var temporaryPlotFileName = Guid.NewGuid() + ".pdf";
            this.temporaryPlotFiles.Add(temporaryPlotFileName);
            PdfExporter.Export(plot.PlotModel, temporaryPlotFileName, plot.Width, plot.Height);

            var p = this.WriteStartFigure(plot);
            MigraDoc.DocumentObjectModel.Shapes.Image pi = p.AddImage(temporaryPlotFileName);
            pi.Width = Unit.FromCentimeter(15);

            this.WriteEndFigure(plot, p);
        }

        /// <summary>
        /// Writes the specified report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="reportStyle">The report style.</param>
        public void WriteReport(Report report, ReportStyle reportStyle)
        {
            this.style = reportStyle;
            DefineStyles(this.Document, reportStyle);
            report.Write(this);
        }

        /// <summary>
        /// Writes the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        void IReportWriter.WriteTable(Table table)
        {
            if (table.Rows == null)
            {
                return;
            }

            var pdfTable = new MigraDoc.DocumentObjectModel.Tables.Table
                {
                    Borders = { Width = 0.25, Left = { Width = 0.5 }, Right = { Width = 0.5 } },
                    Rows = { LeftIndent = 0 }
                };

            //// pdfTable.Style = "Table";
            //// pdfTable.Borders.Color = TableBorder;

            int columns = table.Columns.Count;

            for (int j = 0; j < columns; j++)
            {
                var pdfColumn = pdfTable.AddColumn();

                // todo: the widths are not working
                //// pdfColumn.Width = Unit.FromMillimeter(table.Columns[j].ActualWidth);
                pdfColumn.Format.Alignment = ConvertToParagraphAlignment(table.Columns[j].Alignment);
            }

            foreach (var tr in table.Rows)
            {
                var pdfRow = pdfTable.AddRow();
                for (int j = 0; j < columns; j++)
                {
                    bool isHeader = tr.IsHeader || table.Columns[j].IsHeader;

                    var c = tr.Cells[j];
                    var pdfCell = pdfRow.Cells[j];
                    pdfCell.AddParagraph(c.Content ?? string.Empty);
                    pdfCell.Style = isHeader ? "TableHeader" : "TableText";
                    pdfCell.Format.Alignment = ConvertToParagraphAlignment(table.Columns[j].Alignment);
                }
            }

            // table.SetEdge(0, 0, t.Columns.Count, t.Items.Count(), Edge.Box, BorderStyle.Single, 1.5, Colors.Black);
            if (table.Caption != null)
            {
                var pa = this.CurrentSection.AddParagraph();
                pa.AddFormattedText(table.GetFullCaption(this.style), "TableCaption");
            }

            this.CurrentSection.Add(pdfTable);
        }

        /// <summary>
        /// Writes the start of a figure.
        /// </summary>
        /// <param name="f">The figure.</param>
        /// <returns>A paragraph</returns>
        protected Paragraph WriteStartFigure(Figure f)
        {
            return this.CurrentSection.AddParagraph();
        }

        /// <summary>
        /// Writes the figure text.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <param name="pa">The paragraph.</param>
        protected void WriteEndFigure(Figure f, Paragraph pa)
        {
            if (f.FigureText == null)
            {
                return;
            }

            pa.AddLineBreak();
            pa.AddFormattedText(f.GetFullCaption(this.style), "FigureText");
        }

        /// <summary>
        /// Converts paragraph alignment.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>The pdf alignment.</returns>
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
        /// Sets a paragraph style.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="ps">The ps.</param>
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
        /// Converts an OxyColor to a migra doc color.
        /// </summary>
        /// <param name="c">The color.</param>
        /// <returns>The converted color.</returns>
        private static Color ToMigraDocColor(OxyColor c)
        {
            return new Color(c.A, c.R, c.G, c.B);
        }

        /// <summary>
        /// Erases the temporary plot files.
        /// </summary>
        private void CleanTemporaryPlotFiles()
        {
            foreach (var fileName in this.temporaryPlotFiles)
            {
                File.Delete(fileName);
            }
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
                    this.Close();
                }
            }

            this.disposed = true;
        }
    }
}