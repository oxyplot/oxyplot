using System;
using System.IO;

namespace OxyPlot.Reporting
{
    /// <summary>
    /// ANSI text writer.
    /// This will not write figures/images.
    /// </summary>
    public class TextReportWriter : StreamWriter, IReportWriter
    {
        private const string TableCellSeparator = " | ";
        private const string TableRowStart = "| ";
        private const string TableRowEnd = " |";
        private int tableCounter;

        public TextReportWriter(Stream s)
            : base(s)
        {
            MaxLineLength = 60;
        }

        public TextReportWriter(string path)
            : base(path)
        {
            MaxLineLength = 60;
        }

        public int MaxLineLength { get; set; }

        #region IReportWriter Members

        public void WriteReport(Report report, ReportStyle style)
        {
            report.Write(this);
        }

        public void WriteHeader(Header h)
        {
            if (h.Text == null)
                return;
            WriteLine(h);
            if (h.Level == 1)
            {
                WriteLine("=".Repeat(h.Text.Length));
            }
            WriteLine();
        }

        public void WriteParagraph(Paragraph p)
        {
            foreach (var line in p.Text.SplitLines(MaxLineLength))
            {
                WriteLine(line);
            }
            WriteLine();
        }

        public void WriteTable(Table t)
        {
            tableCounter++;
            WriteLine(String.Format("Table {0}. {1}", tableCounter, t.Caption));
            WriteLine();
            int rows = t.Rows.Count;
            int cols = t.Columns.Count;

            var columnWidth = new int[cols];
            int totalLength = 0;
            for (int j = 0; j < cols; j++)
            {
                columnWidth[j] = 0;
                foreach (var tr in t.Rows)
                {
                    var cell = tr.Cells[j];
                    string text = cell.Content;
                    columnWidth[j] = Math.Max(columnWidth[j], text != null ? text.Length : 0);
                }
                totalLength += columnWidth[j];
            }
            // WriteLine("-".Repeat(totalLength));
            foreach (var tr in t.Rows)
            {
                for (int j = 0; j < cols; j++)
                {
                    var cell = tr.Cells[j];
                    string text = cell.Content;
                    Write(GetCellText(j, cols, PadString(text, t.Columns[j].Alignment, columnWidth[j])));
                }
                WriteLine();
            }
            WriteLine();
        }

        public void WriteImage(Image i)
        {
        }

        public void WriteDrawing(DrawingFigure d)
        {
        }

        public void WritePlot(PlotFigure plot)
        {
        }

        public void WriteEquation(Equation equation)
        {
        }

        #endregion

        private static string GetCellText(int i, int count, string p)
        {
            if (i == 0)
                p = TableRowStart + p;
            if (i + 1 < count)
                p += TableCellSeparator;
            if (i == count - 1)
                p += TableRowEnd;
            return p;
        }


        private static string PadString(string text, Alignment alignment, int width)
        {
            if (text == null)
                return "".PadLeft(width);
            switch (alignment)
            {
                case Alignment.Left:
                    return text.PadRight(width);
                case Alignment.Right:
                    return text.PadLeft(width);
                case Alignment.Center:
                    text = text.PadRight((text.Length + width) / 2);
                    return text.PadLeft(width);
            }
            return null;
        }
    }
}