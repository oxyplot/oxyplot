// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyRectTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the <see cref="OxyRect" /> class and it's extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using NUnit.Framework;

    /// <summary>
    /// Unit tests for <see cref="OxyRect" />.
    /// </summary>
    [TestFixture]
    public class OxyRectTests
    {
        /// <summary>
        /// Tests the Equals method.
        /// </summary>
        [Test]
        public void Equals()
        {
            Assert.That(new OxyRect(1, 2, 3, 4).Equals(new OxyRect(1, 2, 3, 4)), Is.True);
            Assert.That(new OxyRect(1, 2, 3, 4).Equals(new OxyRect()), Is.False);
        }

        /// <summary>
        /// Tests the Inflate method.
        /// </summary>
        [Test]
        public void Inflate()
        {
            Assert.That(new OxyRect(1, 2, 3, 4).Inflate(0.1, 0.2), Is.EqualTo(new OxyRect(0.9, 1.8, 3.2, 4.4)));
            Assert.That(new OxyRect(10, 20, 30, 40).Inflate(new OxyThickness(1, 2, 3, 4)), Is.EqualTo(new OxyRect(9, 18, 34, 46)));
        }

        /// <summary>
        /// Tests the Deflate method.
        /// </summary>
        [Test]
        public void Deflate()
        {
            Assert.That(new OxyRect(10, 20, 30, 40).Deflate(new OxyThickness(1, 2, 3, 4)), Is.EqualTo(new OxyRect(11, 22, 26, 34)));
            Assert.That(new OxyRect(10, 20, 30, 40).Deflate(new OxyThickness(15)), Is.EqualTo(new OxyRect(25, 35, 0, 10)));
        }

        /// <summary>
        /// Tests the Offset method.
        /// </summary>
        [Test]
        public void Offset()
        {
            Assert.That(new OxyRect(1, 2, 3, 4).Offset(0.1, 0.2), Is.EqualTo(new OxyRect(1.1, 2.2, 3, 4)));
        }
    }
}