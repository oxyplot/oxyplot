// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DecimatorTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the <see cref="Decimator" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests.Rendering.Utilities
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    /// <summary>
    /// Provides unit tests for the <see cref="Decimator" /> class.
    /// </summary>
    [TestFixture]
    public class DecimatorTests
    {
        /// <summary>
        /// Tests decimation of a list of points.
        /// </summary>
        [Test]
        public void Decimate()
        {
            // TODO: write some better tests
            var input = new List<ScreenPoint>();
            var output = new List<ScreenPoint>();
            int n = 1000;
            for (int i = 0; i < n; i++)
            {
                input.Add(new ScreenPoint(Math.Round((double)i / n), Math.Sin(i)));
            }

            Decimator.Decimate(input, output);
            Assert.AreEqual(6, output.Count);
        }
    }
}