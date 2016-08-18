// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfExporterTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using ExampleLibrary;
    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class PdfExporterTests
    {
        [Test]
        public void Export_AllExamplesInExampleLibrary_CheckThatAllFilesExist()
        {
            const string DestinationDirectory = "PdfExporterTests_ExampleLibrary";
            if (!Directory.Exists(DestinationDirectory))
            {
                Directory.CreateDirectory(DestinationDirectory);
            }

            // A4
            const double Width = 297 / 25.4 * 72;
            const double Height = 210 / 25.4 * 72;

            foreach (var example in Examples.GetList())
            {
                if (example.PlotModel == null)
                {
                    continue;
                }

                var path = Path.Combine(DestinationDirectory, FileNameUtilities.CreateValidFileName(example.Category + " - " + example.Title, ".pdf"));
                using (var s = File.Create(path))
                {
                    try
                    {
                        PdfExporter.Export(example.PlotModel, s, Width, Height);
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail("Exception in " + example.Title + ":" + ex.Message);
                    }
                }

                Assert.IsTrue(File.Exists(path));
            }
        }
    }
}