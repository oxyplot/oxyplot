using System.Collections.Generic;
using System.Text;

namespace OxyPlot.Reporting
{
    public static class StringExtensions
    {
        public static string Repeat(this string source, int n)
        {
            var sb = new StringBuilder(n * source.Length);
            for (int i = 0; i < n; i++)
            {
                sb.Append(source);
            }
            return sb.ToString();
        }

        public static string[] SplitLines(this string s, int lineLength = 80)
        {
            var lines = new List<string>();

            int i = 0;
            while (i < s.Length)
            {
                int len = FindLineLength(s, i, lineLength);
                lines.Add(len == 0 ? s.Substring(i).Trim() : s.Substring(i, len).Trim());
                i += len;
                if (len == 0)
                    break;
            }
            return lines.ToArray();
        }

        private static int FindLineLength(string text, int i, int maxLineLength)
        {
            int i2 = i + 1;
            int len = 0;
            while (i2 < i + maxLineLength && i2 < text.Length)
            {
                i2 = text.IndexOfAny(" \n\r".ToCharArray(), i2 + 1);
                if (i2 == -1)
                    i2 = text.Length;
                if (i2 - i < maxLineLength)
                    len = i2 - i;
            }
            return len;
        }
    }
}