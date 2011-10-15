//-----------------------------------------------------------------------
// <copyright file="LatexReportWriter.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System.IO;

    /// <summary>
    /// LaTeX2e writer.
    /// </summary>
    public class LatexReportWriter : StreamWriter, IReportWriter
    {
        #region Constants and Fields

        /// <summary>
        ///   The indent.
        /// </summary>
        private string indent = string.Empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LatexReportWriter"/> class.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="author">
        /// The author.
        /// </param>
        /// <param name="fontsize">
        /// The fontsize.
        /// </param>
        /// <param name="documentType">
        /// The document type.
        /// </param>
        public LatexReportWriter(
            Stream s, string title, string author, string fontsize = "12pt", string documentType = "report")
            : base(s)
        {
            this.WriteLatexHeader(title, author, fontsize, documentType);
        }

#if !METRO

        /// <summary>
        /// Initializes a new instance of the <see cref="LatexReportWriter"/> class.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="author">
        /// The author.
        /// </param>
        /// <param name="fontsize">
        /// The fontsize.
        /// </param>
        /// <param name="documentType">
        /// The document type.
        /// </param>
        public LatexReportWriter(
            string path, string title, string author, string fontsize = "12pt", string documentType = "report")
            : base(path)
        {
            this.WriteLatexHeader(title, author, fontsize, documentType);
        }

#endif

        #endregion

        #region Public Methods

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
        /// The write image.
        /// </summary>
        /// <param name="i">
        /// The i.
        /// </param>
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
        /// The write latex header.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="author">
        /// The author.
        /// </param>
        /// <param name="fontsize">
        /// The fontsize.
        /// </param>
        /// <param name="documentType">
        /// The document type.
        /// </param>
        public void WriteLatexHeader(string title, string author, string fontsize, string documentType)
        {
            this.WriteLine(@"\documentclass[" + fontsize + "]{" + documentType + "}");
            this.WriteLine(@"\usepackage{amsmath}");

            // WriteLine(@"\usepackage{longtabular}");
            this.WriteLine(@"\usepackage{graphicx}");
            this.WriteLine(@"\title{" + title + "}");
            this.WriteLine(@"\author{" + author + "}");
            this.WriteLine(@"\date{}");
            this.WriteLine(@"\begin{document}");
            this.Indent();
            this.WriteIndentedLine(@"\maketitle");
        }

        /// <summary>
        /// The write paragraph.
        /// </summary>
        /// <param name="p">
        /// The p.
        /// </param>
        public void WriteParagraph(Paragraph p)
        {
            foreach (string line in p.Text.SplitLines())
            {
                this.WriteIndentedLine(LatexEncodeText(line));
            }

            this.WriteLine();
        }

        /// <summary>
        /// The write plot.
        /// </summary>
        /// <param name="plot">
        /// The plot.
        /// </param>
        public void WritePlot(PlotFigure plot)
        {
            // string path = "plot" + plotNumber + ".pdf";
            // PdfExporter.Export(Model, pdfPlotFileName, 800, 500);                   
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
            this.WriteIndentedLine(@"\begin{table}[h]");
            this.Indent();
            int rows = t.Rows.Count;
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
                    TableCell cell = row.Cells[j];
                    string text = LatexEncodeText(cell.Content);
                    Write(text);
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

        #endregion

        #region Methods

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            this.UnIndent();
            this.WriteLine(@"\end{document}");

            base.Dispose(disposing);
        }

        /// <summary>
        /// The latex encode text.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <returns>
        /// The latex encode text.
        /// </returns>
        private static string LatexEncodeText(string t)
        {
            if (t == null)
            {
                return t;
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
        /// The write indent.
        /// </summary>
        private void WriteIndent()
        {
            this.Write(this.indent);
        }

        /// <summary>
        /// The write indented line.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        private void WriteIndentedLine(string s)
        {
            this.WriteIndent();
            WriteLine(s);
        }

        #endregion
    }
}
