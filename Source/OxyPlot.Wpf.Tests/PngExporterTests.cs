// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporterTests.cs" company="OxyPlot">
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
namespace OxyPlot.Wpf.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using NUnit.Framework;
    using OxyPlot.Tests;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class PngExporterTests
    {
        [Test]
        public void ExportToFile_TestPlot_CheckThatFileExists()
        {
            var plotModel = TestModels.CreateTestModel1();
            const string FileName = "PngExporterTests_Plot1.png";
            PngExporter.Export(plotModel, FileName, 400, 300);
            Assert.IsTrue(File.Exists(FileName));
        }

        [Test]
        public void ExportToFile_AllExamples_CheckThatFilesExist()
        {
            const string DestinationDirectory = "ExampleLibrary";

            if (!Directory.Exists(DestinationDirectory))
            {
                Directory.CreateDirectory(DestinationDirectory);
            }

            foreach (var example in ExampleLibrary.Examples.GetList())
            {
                var path = Path.Combine(DestinationDirectory, StringHelper.CreateValidFileName(example.Category + " - " + example.Title, ".png"));
                PngExporter.Export(example.PlotModel, path, 800, 500, OxyColors.White);
            }
        }
    }
}