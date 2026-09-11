// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxySizeTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the <see cref="OxySize" /> class and it's extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using NUnit.Framework;
    using System.Linq;

    /// <summary>
    /// Unit tests for <see cref="OxySize" />.
    /// </summary>
    [TestFixture]
    public class OxySizeTests
    {
        /// <summary>
        /// Checks the rotating properties of GetBounds() method.
        /// </summary>
        [Test]
        public void CheckBoundsCalculation()
        {
            AssertBoundsRect(new OxyRect(-20, -25, 40, 50), 0, HorizontalAlignment.Center, VerticalAlignment.Middle);
            AssertBoundsRect(new OxyRect(-20, -25, 40, 50), 180, HorizontalAlignment.Center, VerticalAlignment.Middle);
            AssertBoundsRect(new OxyRect(-20, -25, 40, 50), 360, HorizontalAlignment.Center, VerticalAlignment.Middle);
            AssertBoundsRect(new OxyRect(-25, -20, 50, 40), 90, HorizontalAlignment.Center, VerticalAlignment.Middle);
            AssertBoundsRect(new OxyRect(-32, -32, 63, 63), 45, HorizontalAlignment.Center, VerticalAlignment.Middle);
            AssertBoundsRect(new OxyRect(-32, -32, 63, 63), -45, HorizontalAlignment.Center, VerticalAlignment.Middle);

            AssertBoundsRect(new OxyRect(0, 0, 40, 50), 0, HorizontalAlignment.Left, VerticalAlignment.Top);
            AssertBoundsRect(new OxyRect(-40, -50, 40, 50), 180, HorizontalAlignment.Left, VerticalAlignment.Top);
            AssertBoundsRect(new OxyRect(-50, 0, 50, 40), 90, HorizontalAlignment.Left, VerticalAlignment.Top);
            AssertBoundsRect(new OxyRect(-35, 0, 63, 63), 45, HorizontalAlignment.Left, VerticalAlignment.Top);
            AssertBoundsRect(new OxyRect(0, -28, 63, 63), -45, HorizontalAlignment.Left, VerticalAlignment.Top);

            AssertBoundsRect(new OxyRect(-40, -50, 40, 50), 0, HorizontalAlignment.Right, VerticalAlignment.Bottom);
            AssertBoundsRect(new OxyRect(-40, -50, 40, 50), 360, HorizontalAlignment.Right, VerticalAlignment.Bottom);
            AssertBoundsRect(new OxyRect(0, 0, 40, 50), 180, HorizontalAlignment.Right, VerticalAlignment.Bottom);
            AssertBoundsRect(new OxyRect(0, -40, 50, 40), 90, HorizontalAlignment.Right, VerticalAlignment.Bottom);
            AssertBoundsRect(new OxyRect(-28, -63, 63, 63), 45, HorizontalAlignment.Right, VerticalAlignment.Bottom);
            AssertBoundsRect(new OxyRect(-63, -35, 63, 63), -45, HorizontalAlignment.Right, VerticalAlignment.Bottom);

            AssertBoundsRect(new OxyRect(0, -50, 40, 50), 0, HorizontalAlignment.Left, VerticalAlignment.Bottom);
            AssertBoundsRect(new OxyRect(0, -35, 63, 63), 45, HorizontalAlignment.Left, VerticalAlignment.Bottom);

            AssertBoundsRect(new OxyRect(0, -25, 40, 50), 0, HorizontalAlignment.Left, VerticalAlignment.Middle);
            AssertBoundsRect(new OxyRect(-17, -17, 63, 63), 45, HorizontalAlignment.Left, VerticalAlignment.Middle);
        }

        /// <summary>
        /// Checks the calculation of the GetPolygon() method.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="ha">The horizontal alignment.</param>
        /// <param name="va">The vertical alignment.</param>
        /// <param name="expectedX0">The expected x0.</param>
        /// <param name="expectedY0">The expected y0.</param>
        /// <param name="expectedX2">The expected x2.</param>
        /// <param name="expectedY2">The expected y2.</param>
        [Test]
        [TestCase(0, 0, 0, HorizontalAlignment.Left, VerticalAlignment.Top, 0, 0, 40, 50)]
        [TestCase(0, 0, 0, HorizontalAlignment.Right, VerticalAlignment.Top, -40, 0, 0, 50)]
        [TestCase(0, 0, 0, HorizontalAlignment.Right, VerticalAlignment.Bottom, -40, -50, 0, 0)]
        [TestCase(0, 0, 0, HorizontalAlignment.Center, VerticalAlignment.Middle, -20, -25, 20, 25)]
        [TestCase(0, 0, 90, HorizontalAlignment.Center, VerticalAlignment.Middle, 25, -20, -25, 20)]
        [TestCase(0, 0, 180, HorizontalAlignment.Center, VerticalAlignment.Middle, 20, 25, -20, -25)]
        public void CheckPolygonCalculation(double x, double y, double angle, HorizontalAlignment ha, VerticalAlignment va, double expectedX0, double expectedY0, double expectedX2, double expectedY2)
        {
            const double Delta = 1;
            var size = new OxySize(40, 50);
            var p = size.GetPolygon(new ScreenPoint(x, y), angle, ha, va).ToArray();
            Assert.That(p[0].X, Is.EqualTo(expectedX0).Within(Delta));
            Assert.That(p[0].Y, Is.EqualTo(expectedY0).Within(Delta));
            Assert.That(p[2].X, Is.EqualTo(expectedX2).Within(Delta));
            Assert.That(p[2].Y, Is.EqualTo(expectedY2).Within(Delta));
        }

        /// <summary>
        /// Asserts that the rectangle rotated by the specified angle and alignment equals the specified expected rectangle.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="horizontalAlignment">The horizontal alignment.</param>
        /// <param name="verticalAlignment">The vertical alignment.</param>
        private static void AssertBoundsRect(OxyRect expected, double angle, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            var actual = new OxySize(40, 50).GetBounds(angle, horizontalAlignment, verticalAlignment);
            const double Delta = 1;
            var errorMessage = string.Format("{0} is not equal to {1} rotated by {2} angle when aligned {3} and {4}", expected, actual, angle, horizontalAlignment, verticalAlignment);
            Assert.AreEqual(expected.Left, actual.Left, Delta, errorMessage);
            Assert.AreEqual(expected.Top, actual.Top, Delta, errorMessage);
            Assert.AreEqual(expected.Width, actual.Width, Delta, errorMessage);
            Assert.AreEqual(expected.Height, actual.Height, Delta, errorMessage);
        }

        /// <summary>
        /// Tests the Equals method.
        /// </summary>
        [Test]
        public void Equals()
        {
            Assert.That(new OxySize(1, 2).Equals(new OxySize(1, 2)), Is.True);
            Assert.That(new OxySize(1, 2).Equals(new OxySize()), Is.False);
        }
    }
}
