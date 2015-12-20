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
    }
}