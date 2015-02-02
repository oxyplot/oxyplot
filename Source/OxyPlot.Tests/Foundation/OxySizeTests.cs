// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxySizeTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the <see cref="OxySize" /> class and it's extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests.Foundation
{
    using NUnit.Framework;

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
        public void CheckBoundsCalculate()
        {
            this.AssertRect(new OxyRect(-20, -25, 40, 50), 0, HorizontalAlignment.Center, VerticalAlignment.Middle);
            this.AssertRect(new OxyRect(-20, -25, 40, 50), 180, HorizontalAlignment.Center, VerticalAlignment.Middle);
            this.AssertRect(new OxyRect(-20, -25, 40, 50), 360, HorizontalAlignment.Center, VerticalAlignment.Middle);
            this.AssertRect(new OxyRect(-25, -20, 50, 40), 90, HorizontalAlignment.Center, VerticalAlignment.Middle);
            this.AssertRect(new OxyRect(-32, -32, 63, 63), 45, HorizontalAlignment.Center, VerticalAlignment.Middle);
            this.AssertRect(new OxyRect(-32, -32, 63, 63), -45, HorizontalAlignment.Center, VerticalAlignment.Middle);

            this.AssertRect(new OxyRect(0, 0, 40, 50), 0, HorizontalAlignment.Left, VerticalAlignment.Top);
            this.AssertRect(new OxyRect(-40, -50, 40, 50), 180, HorizontalAlignment.Left, VerticalAlignment.Top);
            this.AssertRect(new OxyRect(-50, 0, 50, 40), 90, HorizontalAlignment.Left, VerticalAlignment.Top);
            this.AssertRect(new OxyRect(-35, 0, 63, 63), 45, HorizontalAlignment.Left, VerticalAlignment.Top);
            this.AssertRect(new OxyRect(0, -28, 63, 63), -45, HorizontalAlignment.Left, VerticalAlignment.Top);

            this.AssertRect(new OxyRect(-40, -50, 40, 50), 0, HorizontalAlignment.Right, VerticalAlignment.Bottom);
            this.AssertRect(new OxyRect(-40, -50, 40, 50), 360, HorizontalAlignment.Right, VerticalAlignment.Bottom);
            this.AssertRect(new OxyRect(0, 0, 40, 50), 180, HorizontalAlignment.Right, VerticalAlignment.Bottom);
            this.AssertRect(new OxyRect(0, -40, 50, 40), 90, HorizontalAlignment.Right, VerticalAlignment.Bottom);
            this.AssertRect(new OxyRect(-28, -63, 63, 63), 45, HorizontalAlignment.Right, VerticalAlignment.Bottom);
            this.AssertRect(new OxyRect(-63, -35, 63, 63), -45, HorizontalAlignment.Right, VerticalAlignment.Bottom);

            this.AssertRect(new OxyRect(0, -50, 40, 50), 0, HorizontalAlignment.Left, VerticalAlignment.Bottom);
            this.AssertRect(new OxyRect(0, -35, 63, 63), 45, HorizontalAlignment.Left, VerticalAlignment.Bottom);

            this.AssertRect(new OxyRect(0, -25, 40, 50), 0, HorizontalAlignment.Left, VerticalAlignment.Middle);
            this.AssertRect(new OxyRect(-17, -17, 63, 63), 45, HorizontalAlignment.Left, VerticalAlignment.Middle);
        }

        /// <summary>
        /// Asserts that the rectangle rotated by the specified angle and alignment equals the specified expected rectangle.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="horizontalAlignment">The horizontal alignment.</param>
        /// <param name="verticalAlignment">The vertical alignment.</param>
        private void AssertRect(OxyRect expected, double angle, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            var actual = new OxySize(40, 50).GetBounds(angle, horizontalAlignment, verticalAlignment);
            const double Delta = 1;
            var errorMessage = string.Format("{0} is not equal to {1} rotated by {2} angle when aligned {3} and {4}", expected, actual, angle, horizontalAlignment, verticalAlignment);
            Assert.AreEqual(expected.Left, actual.Left, Delta, errorMessage);
            Assert.AreEqual(expected.Top, actual.Top, Delta, errorMessage);
            Assert.AreEqual(expected.Width, actual.Width, Delta, errorMessage);
            Assert.AreEqual(expected.Height, actual.Height, Delta, errorMessage);
        }
    }
}
