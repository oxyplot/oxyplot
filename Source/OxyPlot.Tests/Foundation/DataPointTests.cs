// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPointTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the <see cref="DataPoint" /> type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using NUnit.Framework;

    /// <summary>
    /// Provides unit tests for the <see cref="DataPoint" /> type.
    /// </summary>
    [TestFixture]
    public class DataPointTests
    {
        /// <summary>
        /// Given valid points, <see cref="DataPoint.IsDefined" /> should return <c>true</c>.
        /// </summary>
        [Test]
        public void ValidPoints()
        {
            Assert.That(new DataPoint(1, 2).IsDefined(), Is.True);
            Assert.That(new DataPoint(double.MaxValue, double.MaxValue).IsDefined(), Is.True);
            Assert.That(new DataPoint(double.MinValue, double.MinValue).IsDefined(), Is.True);
        }

        /// <summary>
        /// Given invalid points, <see cref="DataPoint.IsDefined" /> should return <c>false</c>.
        /// </summary>
        [Test]
        public void InvalidPoints()
        {
            Assert.That(new DataPoint(double.NaN, double.NaN).IsDefined(), Is.False);
            Assert.That(new DataPoint(double.NaN, 2).IsDefined(), Is.False);
            Assert.That(new DataPoint(2, double.NaN).IsDefined(), Is.False);
            var p = DataPoint.Undefined;
            Assert.That(p.IsDefined(), Is.False);
        }

        /// <summary>
        /// Tests the <see cref="DataPoint.Equals" /> method.
        /// </summary>
        [Test]
        public void Equals()
        {
            Assert.That(new DataPoint(1, 2).Equals(new DataPoint(1, 2)), Is.True);
            Assert.That(new DataPoint(1, 2).Equals(new DataPoint()), Is.False);
            Assert.That(DataPoint.Undefined.Equals(DataPoint.Undefined), Is.True);
        }
    }
}