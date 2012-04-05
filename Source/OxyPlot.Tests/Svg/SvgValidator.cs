namespace OxyPlot.Tests
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;

    public static class SvgValidator
    {
        public static bool IsValid(string path)
        {
            return Validate(path) == null;
        }

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
        /// <param name="path">The path.</param>
        /// <param name="sc">The schema.</param>
        /// <returns>Number of errors and warnings, or null if the number of errors and warnings is zero.</returns>
        static string Validate(string path, XmlSchemaSet sc)
        {
            // http://msdn.microsoft.com/en-us/library/as3tta56.aspx
            var settings = new XmlReaderSettings
                {
                    ConformanceLevel = ConformanceLevel.Document,
                    DtdProcessing = DtdProcessing.Ignore,
                    ValidationType = ValidationType.Schema,
                    Schemas = sc,
                    ValidationFlags = XmlSchemaValidationFlags.ProcessSchemaLocation | XmlSchemaValidationFlags.ProcessInlineSchema,
                };

            int warnings = 0;
            int errors = 0;

            settings.ValidationEventHandler += (sender, e) =>
                {
                    Console.WriteLine(e.Message);
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
                using (var xvr = XmlReader.Create(input, settings))
                {
                    while (xvr.Read())
                    {
                        // do nothing
                    }

                    if (errors + warnings == 0)
                    {
                        return null;
                    }

                    return String.Format("Errors: {0}, Warnings: {1}", errors, warnings);
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
}