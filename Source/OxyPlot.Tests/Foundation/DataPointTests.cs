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
            Assert.IsTrue(new DataPoint(1, 2).IsDefined());
            Assert.IsTrue(new DataPoint(double.MaxValue, double.MaxValue).IsDefined());
            Assert.IsTrue(new DataPoint(double.MinValue, double.MinValue).IsDefined());
        }

        /// <summary>
        /// Given invalid points, <see cref="DataPoint.IsDefined" /> should return <c>false</c>.
        /// </summary>
        [Test]
        public void InvalidPoints()
        {
            Assert.IsFalse(new DataPoint(double.NaN, double.NaN).IsDefined());
            Assert.IsFalse(new DataPoint(double.NaN, 2).IsDefined());
            Assert.IsFalse(new DataPoint(2, double.NaN).IsDefined());
            var p = DataPoint.Undefined;
            Assert.IsFalse(p.IsDefined());
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