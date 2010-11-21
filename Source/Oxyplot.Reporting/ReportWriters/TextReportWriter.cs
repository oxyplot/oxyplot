using System;
using System.IO;

namespace OxyPlot.Reporting
{
    public class TextReportWriter : StreamWriter, IReportWriter
    {
        private string sep = " | ";
        private string sep0 = "| ";
        private string sepN = " |";
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

        public void WriteHeader(Header h)
        {
            if (h.Text == null)
                return;
            WriteLine(h);
            if (h.Level == 1)
            {
                for (int i = 0; i < h.Text.Length; i++)
                    Write('=');
                WriteLine();
            }
            WriteLine();
        }

        public void WriteParagraph(Paragraph p)
        {
            int i = 0;
            while (i < p.Text.Length)
            {
                int len = FindLineLength(p.Text, i);
                if (len == 0)
                    WriteLine(p.Text.Substring(i).Trim());
                else
                    WriteLine(p.Text.Substring(i, len).Trim());
                i += len;
                if (len == 0)
                    break;
            }
            WriteLine();
        }

        public void WriteTable(Table t)
        {
            tableCounter++;
            WriteLine(String.Format("Table {0}. {1}", tableCounter, t.Caption));
            WriteLine();
            var cells = t.ToArray();
            int rows = cells.GetUpperBound(0) + 1;
            int cols = cells.GetUpperBound(1) + 1;

            var columnWidth = new int[cols];
            int totalLength = 0;
            for (int j = 0; j < cols; j++)
            {
                columnWidth[j] = 0;
                for (int i = 0; i < rows; i++)
                {
                    string text = cells[i, j];
                    columnWidth[j] = Math.Max(columnWidth[j], text != null ? text.Length : 0);
                }
                totalLength += columnWidth[j];
            }
            // WriteLine("".PadRight(totalLength, '-'));
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    string text = cells[i, j];
                    Write(GetCellText(j, cols, Format(text, Alignment.Left,
                        columnWidth[j])));
                }
                WriteLine();
            }
            WriteLine();
        }

        public void WriteImage(Image i)
        {
        }

        public void WriteDrawing(Drawing d)
        {
        }

        public void WritePlot(Plot plot)
        {
        }

        #endregion

        private int FindLineLength(string text, int i)
        {
            int i2 = i + 1;
            int len = 0;
            while (i2 < i + MaxLineLength && i2 < text.Length)
            {
                i2 = text.IndexOfAny(" \n\r".ToCharArray(), i2 + 1);
                if (i2 == -1)
                    i2 = text.Length;
                if (i2 - i < MaxLineLength)
                    len = i2 - i;
            }
            return len;
        }

        private string GetCellText(int i, int count, string p)
        {
            if (i == 0)
                p = sep0 + p;
            if (i + 1 < count)
                p += sep;
            if (i == count - 1)
                p += sepN;
            return p;
        }


        private string Format(string text, Alignment alignment, int width)
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