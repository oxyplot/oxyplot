// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SutherlandHodgmanClippingTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class SutherlandHodgmanClippingTests
    {
        [Test]
        public void ClipPolygon()
        {
            var bounds = new OxyRect(0, 0, 1, 1);
            var points = CreatePointList();
            var result = SutherlandHodgmanClipping.ClipPolygon(bounds, points);
            Assert.AreEqual(4, result.Count);
        }

        private static IList<ScreenPoint> CreatePointList()
        {
            return new List<ScreenPoint> { new ScreenPoint(-1, -1), new ScreenPoint(1, -2), new ScreenPoint(2, 2), new ScreenPoint(-2, 3) };
        }
    }
}