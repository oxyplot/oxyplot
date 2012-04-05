// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoubleExtensionsTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

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
            var points = new List<ScreenPoint>();
            points.Add(new ScreenPoint(-1, -1));
            points.Add(new ScreenPoint(1, -2));
            points.Add(new ScreenPoint(2, 2));
            points.Add(new ScreenPoint(-2, 3));
            return points;
        }
    }
}