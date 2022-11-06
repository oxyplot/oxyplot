// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeGeneratorTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the CodeGenerator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using NUnit.Framework;

    /// <summary>
    /// Provides unit tests for the <see cref="CodeGenerator" />.
    /// </summary>
    [TestFixture]
    public class CodeGeneratorTests
    {
        /// <summary>
        /// Test that code can be generated for all examples in the example library.
        /// </summary>
        [Test]
        [TestCaseSource(typeof(ExampleLibrary.Examples), nameof(ExampleLibrary.Examples.GetListForAutomatedTest))]
        public void GenerateCodeForAllExamplesInExampleLibrary(ExampleLibrary.ExampleInfo ei)
        {
            void CheckCode(PlotModel model)
            {
                if (model == null)
                {
                    return;
                }

                var code = model.ToCode();
                Assert.That(code, Is.Not.Null, ei.Title);
            }

            CheckCode(ei.PlotModel);
        }

        /// <summary>
        /// Test that code generation properly handles escapes sequences in strings
        /// </summary>
        [Test]
        public void TestCodeGeneratorStringExtensions()
        {
            var plot = new PlotModel();

            // a custom tracker format string that shows x axis values in seconds as "m:ss.ff"
            var series = new Series.LineSeries()
            {
                TrackerFormatString = "{1}: {2:m\\:ss\\.ff}\n{3}: {4:0.##}",
            };
            plot.Series.Add(series);

            StringAssert.Contains(@"{1}: {2:m\\:ss\\.ff}\n{3}: {4:0.##}", plot.ToCode());
        }
    }
}
