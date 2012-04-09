// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CohenSutherlandClippingTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class CohenSutherlandClippingTests
    {
        [Test]
        public void IsInside()
        {
            var clipping = new CohenSutherlandClipping(0, 1, 0, 1);
            Assert.IsTrue(clipping.IsInside(0.5, 0.5));
            Assert.IsFalse(clipping.IsInside(-0.5, 0.5));
        }

        [Test]
        public void ClipLine_EndpointsOutsideArea()
        {
            var clipping = new CohenSutherlandClipping(0, 1, 0, 1);
            var p0 = new ScreenPoint(0.3, -0.2);
            var p1 = new ScreenPoint(0.6, 1.3);
            Assert.IsTrue(clipping.ClipLine(ref p0, ref p1));
            Assert.AreEqual(0, p0.Y);
            Assert.AreEqual(1, p1.Y);
        }

        [Test]
        public void ClipLine_EndpointsInsideArea()
        {
            var clipping = new CohenSutherlandClipping(0, 1, 0, 1);
            var p0 = new ScreenPoint(0.3, 0.2);
            var p1 = new ScreenPoint(0.6, 0.8);
            Assert.IsTrue(clipping.ClipLine(ref p0, ref p1));
            Assert.AreEqual(0.2, p0.Y);
            Assert.AreEqual(0.8, p1.Y);
        }

        [Test]
        public void ClipLine_LineOutsideArea()
        {
            var clipping = new CohenSutherlandClipping(0, 1, 0, 1);
            var p0 = new ScreenPoint(0.3, -0.2);
            var p1 = new ScreenPoint(0.6, -0.2);
            Assert.IsFalse(clipping.ClipLine(ref p0, ref p1));
            Assert.AreEqual(-0.2, p0.Y);
            Assert.AreEqual(-0.2, p1.Y);
        }
    }
}