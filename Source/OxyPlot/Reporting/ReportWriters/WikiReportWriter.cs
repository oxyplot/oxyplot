// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WikiReportWriter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Implements a <see cref="IReportWriter" /> that writes to wiki format.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System;
    using System.IO;

    /// <summary>
    /// Implements a <see cref="IReportWriter" /> that writes to wiki format.
    /// </summary>
    /// <remarks>This will not write figures/images.</remarks>
    public class WikiReportWriter : StreamWriter, IReportWriter
    {
        /// <summary>
        /// The table cell separator.
        /// </summary>
        private const string TableCellSeparator = " | ";

        /// <summary>
        /// The table header cell separator.
        /// </summary>
        private const string TableHeaderCellSeparator = " || ";

        /// <summary>
        /// The table header row end.
        /// </summary>
        private const string TableHeaderRowEnd = " ||";

        /// <summary>
        /// The table header row start.
        /// </summary>
        private const string TableHeaderRowStart = "|| ";

        /// <summary>
        /// The table row end.
        /// </summary>
        private const string TableRowEnd = " |";

        /// <summary>
        /// The table row start.
        /// </summary>
        private const string TableRowStart = "| ";

        /// <summary>
        /// The table counter.
        /// </summary>
        private int tableCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WikiReportWriter" /> class.
        /// </summary>
        /// <param name="s">The s.</param>
        public WikiReportWriter(Stream s)
            : base(s)
        {
            this.MaxLineLength = 60;
        }

        /// <summary>
        /// Gets or sets MaxLineLength.
        /// </summary>
        public int MaxLineLength { get; set; }

        /// <summary>
        /// The write drawing.
        /// </summary>
        /// <param name="d">The d.</param>
        public void WriteDrawing(DrawingFigure d)
        {
        }

        /// <summary>
        /// The write equation.
        /// </summary>
        /// <param name="equation">The equation.</param>
        public void WriteEquation(Equation equation)
        {
        }

        /// <summary>
        /// The write header.
        /// </summary>
        /// <param name="h">The h.</param>
        public void WriteHeader(Header h)
        {
            if (h.Text == null)
            {
                return;
            }

            string prefix = string.Empty;
            for (int i = 0; i < h.Level; i++)
            {
                prefix += "!";
            }

            this.WriteLine(prefix + " " + h.Text);
        }

        /// <summary>
        /// The write image.
        /// </summary>
        /// <param name="i">The i.</param>
        public void WriteImage(Image i)
        {
        }

        /// <summary>
        /// The write paragraph.
        /// </summary>
        /// <param name="p">The p.</param>
        public void WriteParagraph(Paragraph p)
        {
            foreach (string line in p.Text.SplitLines(this.MaxLineLength))
            {
                this.WriteLine(line);
            }

            this.WriteLine();
        }

        /// <summary>
        /// The write plot.
        /// </summary>
        /// <param name="plot">The plot.</param>
        public void WritePlot(PlotFigure plot)
        {
        }

        /// <summary>
        /// The write report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="reportStyle">The style.</param>
        public void WriteReport(Report report, ReportStyle reportStyle)
        {
            report.Write(this);
        }

        /// <summary>
        /// The write table.
        /// </summary>
        /// <param name="t">The t.</param>
        public void WriteTable(Table t)
        {
            if (t.Caption != null)
            {
                this.tableCounter++;
                this.WriteLine("Table {0}. {1}", this.tableCounter, t.Caption);
            }

            this.WriteLine();
            int cols = t.Columns.Count;

            var columnWidth = new int[cols];
            for (int j = 0; j < cols; j++)
            {
                columnWidth[j] = 0;
                foreach (var tr in t.Rows)
                {
                    var cell = tr.Cells[j];
                    var text = cell.Content;
                    columnWidth[j] = Math.Max(columnWidth[j], text != null ? text.Length : 0);
                }
            }

            // WriteLine("-".Repeat(totalLength));
            foreach (var tr in t.Rows)
            {
                for (int j = 0; j < cols; j++)
                {
                    var cell = tr.Cells[j];
                    var text = cell.Content;
                    bool isHeader = tr.IsHeader || t.Columns[j].IsHeader;
                    this.Write(GetCellText(j, cols, PadString(text, t.Columns[j].Alignment, columnWidth[j]), isHeader));
                }

                this.WriteLine();
            }

            this.WriteLine();
        }

        /// <summary>
        /// Gets the formatted string for the specified cell.
        /// </summary>
        /// <param name="cellIndex">The cell index (column).</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="content">The content of the cell.</param>
        /// <param name="isHeader">if set to <c>true</c> the cell is a header.</param>
        /// <returns>The cell representation.</returns>
        private static string GetCellText(int cellIndex, int columns, string content, bool isHeader)
        {
            if (cellIndex == 0)
            {
                content = isHeader ? TableHeaderRowStart : TableRowStart + content;
            }

            if (cellIndex + 1 < columns)
            {
                content += isHeader ? TableHeaderCellSeparator : TableCellSeparator;
            }

            if (cellIndex == columns - 1)
            {
                content += isHeader ? TableHeaderRowEnd : TableRowEnd;
            }

            return content;
        }

        /// <summary>
        /// Aligns the specified string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="alignment">The alignment.</param>
        /// <param name="width">The width.</param>
        /// <returns>The padded string.</returns>
        private static string PadString(string text, Alignment alignment, int width)
        {
            if (text == null)
            {
                return string.Empty.PadLeft(width);
            }

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