using System;
using System.IO;

namespace OxyPlot.Reporting
{
    /// <summary>
    ///  LaTeX2e writer.
    /// </summary>
    public class LatexReportWriter : StreamWriter, IReportWriter
    {
        public LatexReportWriter(Stream s, string title, string author, string fontsize = "12pt", string documentType = "report")
            : base(s)
        {
            WriteLatexHeader(title, author, fontsize, documentType);
        }

        public LatexReportWriter(string path, string title, string author, string fontsize = "12pt", string documentType = "report")
            : base(path)
        {
            WriteLatexHeader(title, author, fontsize, documentType);
        }

        private string indent = "";

        public void Indent()
        {
            indent += "  ";
        }

        public void UnIndent()
        {
            if (indent.Length >= 2)
                indent = indent.Remove(0, 2);
        }

        private void WriteIndent()
        {
            Write(indent);
        }

        private void WriteIndentedLine(string s)
        {
            WriteIndent();
            WriteLine(s);
        }


        public void WriteLatexHeader(string title, string author, string fontsize, string documentType)
        {
            WriteLine(@"\documentclass[" + fontsize + "]{" + documentType + "}");
            WriteLine(@"\usepackage{amsmath}");
            // WriteLine(@"\usepackage{longtabular}");
            WriteLine(@"\usepackage{graphicx}");
            WriteLine(@"\title{" + title + "}");
            WriteLine(@"\author{" + author + "}");
            WriteLine(@"\date{}");
            WriteLine(@"\begin{document}");
            Indent();
            WriteIndentedLine(@"\maketitle");
        }

        protected override void Dispose(bool disposing)
        {
            UnIndent();
            WriteLine(@"\end{document}");

            base.Dispose(disposing);
        }

        #region IReportWriter Members

        public void WriteHeader(Header h)
        {
            if (h.Text == null)
                return;
            string header = LatexEncodeText(h.Text);
            switch (h.Level)
            {
                case 1:
                    WriteIndentedLine(@"\chapter{" + header + "}");
                    break;
                case 2:
                    WriteIndentedLine(@"\section{" + header + "}");
                    break;
                case 3:
                    WriteIndentedLine(@"\subsection{" + header + "}");
                    break;
                case 4:
                    WriteIndentedLine(@"\subsubsection{" + header + "}");
                    break;
                case 5:
                    WriteIndentedLine(@"\paragraph{" + header + "}");
                    break;
                case 6:
                    WriteIndentedLine(@"\subparagraph{" + header + "}");
                    break;
            }
        }

        public void WriteParagraph(Paragraph p)
        {
            foreach (var line in p.Text.SplitLines())
            {
                WriteIndentedLine(LatexEncodeText(line));
            }
            WriteLine();
        }


        public void WriteTable(Table t)
        {
            WriteIndentedLine(@"\begin{table}[h]");
            Indent();
            var cells = t.ToArray();
            int rows = cells.GetUpperBound(0) + 1;
            int columns = cells.GetUpperBound(1) + 1;
            string cols = "|";

            foreach (var t1 in t.Columns)
            {
                cols += t1.Alignment.ToString().ToLower()[0];
                cols += "|";
            }

            WriteIndentedLine(@"\begin{center}");
            Indent();
            WriteIndentedLine(@"\begin{tabular}[h]{" + cols + "}");
            Indent();
            WriteIndentedLine(@"\hline");
            for (int i = 0; i < rows; i++)
            {
                WriteIndent();
                for (int j = 0; j < columns; j++)
                {
                    string text = LatexEncodeText(cells[i, j]);
                    Write(text);
                    if (j < columns - 1)
                        Write(" & ");
                }
                WriteLine(@"\\");
            }
            WriteIndentedLine(@"\hline");
            UnIndent();
            WriteIndentedLine(@"\end{tabular}");
            UnIndent();
            WriteIndentedLine(@"\end{center}");
            WriteIndentedLine(@"\caption{" + LatexEncodeText(t.Caption) + "}");
            // WriteLine(@"\label{ex:table}");
            UnIndent();
            WriteIndentedLine(@"\end{table}");
            WriteLine();
        }

        private static string LatexEncodeText(string t)
        {
            if (t == null)
                return t;
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

        public void WriteImage(Image i)
        {
            WriteIndentedLine(@"\begin{figure}[h]");
            WriteIndentedLine(@"\begin{center}");
            WriteIndentedLine(@"\includegraphics[width=0.8\textwidth]{"+i.Source+"}");
            WriteIndentedLine(@"\end{center}");
            WriteIndentedLine(@"\caption{" + LatexEncodeText(i.FigureText) + "}");
            WriteIndentedLine(@"\end{figure}");
            WriteLine();
        }

        public void WriteDrawing(Drawing d)
        {
        }

        public void WritePlot(Plot plot)
        {
            //string path = "plot" + plotNumber + ".pdf";
            //PdfExporter.Export(Model, pdfPlotFileName, 800, 500);                   
        }

        public void WriteEquation(Equation equation)
        {
            WriteIndentedLine(@"\begin{eqnarray*}");
            WriteIndentedLine(equation.Content);
            if (equation.Caption!=null)
                WriteIndentedLine(@"\caption{" + LatexEncodeText(equation.Caption) + "}");
            WriteIndentedLine(@"\end{eqnarray*}");
            WriteLine();

        }

        #endregion
    }
}