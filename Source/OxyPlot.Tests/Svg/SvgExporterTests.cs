// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgExporterTests.cs" company="OxyPlot">
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
    public class SvgExporterTests
    {
        [Test]
        public void ExportToString_TestPlot_ValidSvgDocument()
        {
            var plotModel = new PlotModel("Test plot");
            plotModel.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 8, 200, "Math.Sin"));
            var svg = SvgExporter.ExportToString(plotModel, 800, 500, true);
            SvgAssert.IsValidDocument(svg);
        }

        [Test]
        public void ExportToString_TestPlot_ValidSvgString()
        {
            var plotModel = new PlotModel("Test plot");
            plotModel.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 8, 200, "Math.Sin"));
            var svg = SvgExporter.ExportToString(plotModel, 800, 500);
            SvgAssert.IsValidElement(svg);
        }

        [Test]
        public void Export_TestPlot_ValidSvgString()
        {
            var plotModel = new PlotModel("Test plot");
            const string FileName = "SvgExporterTests_Plot1.svg";
            plotModel.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 8, 200, "Math.Sin"));
            SvgExporter.Export(plotModel, FileName, 800, 500);
            SvgAssert.IsValidFile(FileName);
        }

        [Test]
        public void Export_AllExamplesInExampleLibrary_CheckThatAllFilesExist()
        {
            const string DestinationDirectory = "SvgExporterTests_ExampleLibrary";
            if (!Directory.Exists(DestinationDirectory))
            {
                Directory.CreateDirectory(DestinationDirectory);
            }

            foreach (var example in Examples.GetList())
            {
                var path = Path.Combine(DestinationDirectory, StringHelper.CreateValidFileName(example.Category + " - " + example.Title, ".svg"));
                SvgExporter.Export(example.PlotModel, path, 800, 500);
                Assert.IsTrue(File.Exists(path));
            }
        }
    }
}