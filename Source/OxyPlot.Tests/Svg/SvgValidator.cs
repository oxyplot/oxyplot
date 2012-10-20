// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgValidator.cs" company="OxyPlot">
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
//   Provides validation by the Svg schema.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Tests
{
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;

    /// <summary>
    /// Provides validation by the Svg schema.
    /// </summary>
    public static class SvgValidator
    {
        /// <summary>
        /// Determines whether the specified file is a valid svg file.
        /// </summary>
        /// <param name="path">
        /// The path to the file.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified file is valid; otherwise, <c>false</c> .
        /// </returns>
        public static bool IsValid(string path)
        {
            return Validate(path) == null;
        }

        /// <summary>
        /// Validates the specified svg file.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// A validation result string.
        /// </returns>
        public static string Validate(string path)
        {
            var sc = new XmlSchemaSet();

            // Add the schema to the collection.
            string dir = @"svg\";
            sc.Add("http://www.w3.org/2000/svg", dir + "svg.xsd");
            return Validate(path, sc);
        }

        /// <summary>
        /// Validates the specified XML file against a XSL schema.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="sc">
        /// The schema.
        /// </param>
        /// <returns>
        /// Number of errors and warnings, or null if the number of errors and warnings is zero.
        /// </returns>
        private static string Validate(string path, XmlSchemaSet sc)
        {
            // http://msdn.microsoft.com/en-us/library/as3tta56.aspx
            var settings = new XmlReaderSettings
                {
                    ConformanceLevel = ConformanceLevel.Document,
                    DtdProcessing = DtdProcessing.Ignore,
                    ValidationType = ValidationType.Schema,
                    Schemas = sc,
                    ValidationFlags =
                        XmlSchemaValidationFlags.ProcessSchemaLocation | XmlSchemaValidationFlags.ProcessInlineSchema,
                };

            int warnings = 0;
            int errors = 0;

            settings.ValidationEventHandler += (sender, e) =>
                {
                    System.Diagnostics.Trace.WriteLine(e.Message);
                    if (e.Severity == XmlSeverityType.Warning)
                    {
                        warnings++;
                    }
                    else
                    {
                        errors++;
                    }
                };

            using (var input = File.OpenRead(path))
            {
                var xvr = XmlReader.Create(input, settings);
                while (xvr.Read())
                {
                    // do nothing
                }

                if (errors + warnings == 0)
                {
                    return null;
                }

                return string.Format("Errors: {0}, Warnings: {1}", errors, warnings);

                /*
                catch (XmlSchemaException e)
                {
                    Console.Error.WriteLine("Failed to read XML: {0}", e.Message);

                }
                catch (XmlException e)
                {
                    Console.Error.WriteLine("XML Error: {0}", e.Message);

                }
                catch (IOException e)
                {
                    Console.Error.WriteLine("IO error: {0}", e.Message);
                }*/

            }
        }

    }
}