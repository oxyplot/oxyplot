// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvDocument.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   A class for comma-separated value files
//   http://en.wikipedia.org/wiki/Comma-separated_values
//   Default is "USA/UK CSV" where the separator is ',' and decimal separator is '.'.
//   Todo: Support quoted values...
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace WorldStatisticsDemo
{
    using System.Text;

    /// <summary>
    /// A class for comma-separated value files
    /// http://en.wikipedia.org/wiki/Comma-separated_values
    /// Default is "USA/UK CSV" where the separator is ',' and decimal separator is '.'.
    /// Todo: Support quoted values...
    /// </summary>
    public class CsvDocument
    {
        public string[] Headers { get; private set; }
        public Collection<string[]> Items { get; private set; }

        /// <summary>
        /// Loads the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="separator">The separator (auto-detect if not specified).</param>
        public void Load(string fileName, char separator = '\0')
        {
            using (var r = new StreamReader(fileName,Encoding.UTF8))
            {
                var header = r.ReadLine();

                if (separator == '\0')
                {
                    // Auto detect
                    int commaCount = Count(header, ',');
                    int semicolonCount = Count(header, ';');
                    separator = commaCount > semicolonCount ? ',' : ';';
                }

                Headers = header.Split(separator);
                Items = new Collection<string[]>();

                while (!r.EndOfStream)
                {
                    var line = r.ReadLine();
                    if (line == null || line.StartsWith("%") || line.StartsWith("//"))
                        continue;
                    Items.Add(line.Split(separator));
                }
            }
        }

        private int Count(string s, char c)
        {
            return s.Count(ch => ch == c);
        }
    }
}