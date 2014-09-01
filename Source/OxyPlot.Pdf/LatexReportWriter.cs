// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LatexReportWriter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a report writer for <c>LaTeX2e</c>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf
{
    using System.IO;

    using OxyPlot.Reporting;

    /// <summary>
    /// Provides a report writer for <c>LaTeX2e</c>.
    /// </summary>
    public class LatexReportWriter : StreamWriter, IReportWriter
    {
        /// <summary>
        /// The title.
        /// </summary>
        private readonly string title;

        /// <summary>
        /// The author.
        /// </summary>
        private readonly string author;

        /// <summary>
        /// The font size.
        /// </summary>
        private readonly string fontsize;

        /// <summary>
        /// The document type.
        /// </summary>
        private readonly string documentType;

        /// <summary>
        /// Flag if the end of the document has been written.
        /// </summary>
        private bool isDocumentEnded;

        /// <summary>
        /// The indent.
        /// </summary>
        private string indent = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="LatexReportWriter" /> class.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="title">The title.</param>
        /// <param name="author">The author.</param>
        /// <param name="fontsize">The font size.</param>
        /// <param name="documentType">The document type.</param>
        public LatexReportWriter(Stream s, string title, string author, string fontsize = "12pt", string documentType = "report")
            : base(s)
        {
            this.title = title;
            this.author = author;
            this.fontsize = fontsize;
            this.documentType = documentType;
        }

        /// <summary>
        /// Begins the document.
        /// </summary>
        public void BeginDocument()
        {
            this.WriteLatexHeader();
        }

        /// <summary>
        /// Ends the document.
        /// </summary>
        public void EndDocument()
        {
            this.UnIndent();
            this.WriteLine(@"\end{document}");
            this.isDocumentEnded = true;
        }

        /// <summary>
        /// The indent.
        /// </summary>
        public void Indent()
        {
            this.indent += "  ";
        }

        /// <summary>
        /// The un indent.
        /// </summary>
        public void UnIndent()
        {
            if (this.indent.Length >= 2)
            {
                this.indent = this.indent.Remove(0, 2);
            }
        }

        /// <summary>
        /// Writes the drawing.
        /// </summary>
        /// <param name="d">The d.</param>
        public void WriteDrawing(DrawingFigure d)
        {
        }

        /// <summary>
        /// Writes the equation.
        /// </summary>
        /// <param name="equation">The equation.</param>
        public void WriteEquation(Equation equation)
        {
            this.WriteIndentedLine(@"\begin{eqnarray*}");
            this.WriteIndentedLine(equation.Content);
            if (equation.Caption != null)
            {
                this.WriteIndentedLine(@"\caption{" + LatexEncodeText(equation.Caption) + "}");
            }

            this.WriteIndentedLine(@"\end{eqnarray*}");
            this.WriteLine();
        }

        /// <summary>
        /// Writes the header.
        /// </summary>
        /// <param name="h">The h.</param>
        public void WriteHeader(Header h)
        {
            if (h.Text == null)
            {
                return;
            }

            string header = LatexEncodeText(h.Text);
            switch (h.Level)
            {
                case 1:
                    this.WriteIndentedLine(@"\chapter{" + header + "}");
                    break;
                case 2:
                    this.WriteIndentedLine(@"\section{" + header + "}");
                    break;
                case 3:
                    this.WriteIndentedLine(@"\subsection{" + header + "}");
                    break;
                case 4:
                    this.WriteIndentedLine(@"\subsubsection{" + header + "}");
                    break;
                case 5:
                    this.WriteIndentedLine(@"\paragraph{" + header + "}");
                    break;
                case 6:
                    this.WriteIndentedLine(@"\subparagraph{" + header + "}");
                    break;
            }
        }

        /// <summary>
        /// Writes the image.
        /// </summary>
        /// <param name="i">The i.</param>
        public void WriteImage(Image i)
        {
            this.WriteIndentedLine(@"\begin{figure}[h]");
            this.WriteIndentedLine(@"\begin{center}");
            this.WriteIndentedLine(@"\includegraphics[width=0.8\textwidth]{" + i.Source + "}");
            this.WriteIndentedLine(@"\end{center}");
            this.WriteIndentedLine(@"\caption{" + LatexEncodeText(i.FigureText) + "}");
            this.WriteIndentedLine(@"\end{figure}");
            this.WriteLine();
        }

        /// <summary>
        /// Writes the latex header.
        /// </summary>
        public void WriteLatexHeader()
        {
            this.WriteLine(@"\documentclass[" + this.fontsize + "]{" + this.documentType + "}");
            this.WriteLine(@"\usepackage{amsmath}");

            // WriteLine(@"\usepackage{longtabular}");
            this.WriteLine(@"\usepackage{graphicx}");
            this.WriteLine(@"\title{" + this.title + "}");
            this.WriteLine(@"\author{" + this.author + "}");
            this.WriteLine(@"\date{}");
            this.WriteLine(@"\begin{document}");
            this.Indent();
            this.WriteIndentedLine(@"\maketitle");
        }

        /// <summary>
        /// Writes the paragraph.
        /// </summary>
        /// <param name="p">The p.</param>
        public void WriteParagraph(Paragraph p)
        {
            foreach (string line in p.Text.SplitLines())
            {
                this.WriteIndentedLine(LatexEncodeText(line));
            }

            this.WriteLine();
        }

        /// <summary>
        /// Writes the plot.
        /// </summary>
        /// <param name="plot">The plot.</param>
        public void WritePlot(PlotFigure plot)
        {
            // string path = "plot" + plotNumber + ".pdf";
            // PdfExporter.Export(Model, pdfPlotFileName, 800, 500);
        }

        /// <summary>
        /// Writes the report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="reportStyle">The style.</param>
        public void WriteReport(Report report, ReportStyle reportStyle)
        {
            report.Write(this);
        }

        /// <summary>
        /// Writes the table.
        /// </summary>
        /// <param name="t">The t.</param>
        public void WriteTable(Table t)
        {
            this.WriteIndentedLine(@"\begin{table}[h]");
            this.Indent();
            int columns = t.Columns.Count;
            string cols = "|";

            foreach (var t1 in t.Columns)
            {
                cols += t1.Alignment.ToString().ToLower()[0];
                cols += "|";
            }

            this.WriteIndentedLine(@"\begin{center}");
            this.Indent();
            this.WriteIndentedLine(@"\begin{tabular}[h]{" + cols + "}");
            this.Indent();
            this.WriteIndentedLine(@"\hline");
            foreach (var row in t.Rows)
            {
                this.WriteIndent();
                for (int j = 0; j < columns; j++)
                {
                    var cell = row.Cells[j];
                    string text = LatexEncodeText(cell.Content);
                    this.Write(text);
                    if (j < columns - 1)
                    {
                        this.Write(" & ");
                    }
                }

                this.WriteLine(@"\\");
            }

            this.WriteIndentedLine(@"\hline");
            this.UnIndent();
            this.WriteIndentedLine(@"\end{tabular}");
            this.UnIndent();
            this.WriteIndentedLine(@"\end{center}");
            this.WriteIndentedLine(@"\caption{" + LatexEncodeText(t.Caption) + "}");

            // WriteLine(@"\label{ex:table}");
            this.UnIndent();
            this.WriteIndentedLine(@"\end{table}");
            this.WriteLine();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.StreamWriter" /> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <exception cref="T:System.Text.EncoderFallbackException">The current encoding does not support displaying half of a Unicode surrogate pair.</exception>
        protected override void Dispose(bool disposing)
        {
            if (!this.isDocumentEnded)
            {
                this.EndDocument();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Encodes text.
        /// </summary>
        /// <param name="t">The text to encode.</param>
        /// <returns>The encoded text.</returns>
        private static string LatexEncodeText(string t)
        {
            if (t == null)
            {
                return null;
            }

            t = t.Replace(@"\", @"\textbackslash");
            t = t.Replace("{", @"\{");
            t = t.Replace("}", @"\}");
            t = t.Replace("&", @"\&");
            t = t.Replace("$", @"\$");
            t = t.Replace("%", @"\%");
            t = t.Replace("_", @"\_");
            t = t.Replace("^", @"\^{}");
            t = t.Replace("~", @"\~{}");
            t = t.Replace("|", @"\textbar");
            t = t.Replace("#", @"\#");
            return t;
        }

        /// <summary>
        /// Writes the indent.
        /// </summary>
        private void WriteIndent()
        {
            this.Write(this.indent);
        }

        /// <summary>
        /// Writes the indented line.
        /// </summary>
        /// <param name="s">The s.</param>
        private void WriteIndentedLine(string s)
        {
            this.WriteIndent();
            this.WriteLine(s);
        }
    }
}