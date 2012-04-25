// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporterTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
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