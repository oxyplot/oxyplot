// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSeriesTests.cs" company="OxyPlot">
//   Copyright (c) 2017 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the <see cref="LineSeries" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using NUnit.Framework;
    using Series;
    using System.Collections.Generic;

    /// <summary>
    /// Provides unit tests for the <see cref="LineSeries" /> class.
    /// </summary>
    [TestFixture]
    public class LineSeriesTests
    {
        [Test]
        public void LineSeries_GetNearestPointInternal_No_points()
        {
            var ls = new LineSeries();

            ls.ItemsSource = new List<DataPoint>();
            Assert.IsNull(ls.GetNearestPoint(new ScreenPoint(10, 15), false));
        }
    }
}