using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyPlot
{
    public static class FormattingExtensions
    {
        public static string ToNiceString(this double x)
        {
            return x.ToString();
        }

        public static string ToNiceString(this DateTime x)
        {
            return x.ToString();
        }

        public static string ToNiceString(this TimeSpan x)
        {
            return x.ToString();
        }
    }
}
