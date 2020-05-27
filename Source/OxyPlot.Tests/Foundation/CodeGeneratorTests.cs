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
        public void GenerateCodeForAllExamplesInExampleLibrary()
        {
            foreach (var ei in ExampleLibrary.Examples.GetList())
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
        }
    }
}
