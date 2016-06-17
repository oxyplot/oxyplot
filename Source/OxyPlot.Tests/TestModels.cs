// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestModels.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides methods to create test models.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.IO;
    using System.Reflection;

    using OxyPlot.Series;

    /// <summary>
    /// Provides methods to create test models.
    /// </summary>
    public static class TestModels
    {
        /// <summary>
        /// Creates a simple sin(x) function series plot model.
        /// </summary>
        /// <returns>A plot model.</returns>
        public static PlotModel CreateTestModel1()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().CodeBase;
            var sourceFolderPath = Path.GetDirectoryName(
                    GetParentDirectoryPath(
                        GetParentDirectoryPath(
                            GetParentDirectoryPath(assemblyPath.Substring(8)))));
            var defaultFont = sourceFolderPath + "/OxyPlot.Wpf.Tests/TestFonts/now/#Now";
            var model = new PlotModel { Title = "Test 1", DefaultFont = defaultFont };
            model.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 8, 200, "sin(x)"));
            return model;
        }

        /// <summary>
        /// Returns the parent directory path
        /// </summary>
        /// <param name="path">The current directory path</param>
        /// <returns>The parent directory path</returns>
        private static string GetParentDirectoryPath(string path)
        {
            return Directory.GetParent(path).FullName;
        }
    }
}