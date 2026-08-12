// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataRangeTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Tests the <see cref="DataRange" /> struct.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using NUnit.Framework;
    using OxyPlot.Series;

    /// <summary>
    /// Contains the unit tests for the <see cref="DataRange"/> struct.
    /// </summary>
    [TestFixture]
    public class DataRangeTests
    {
        /// <summary>
        /// Tests that the properties are initialized correctly.
        /// </summary>
        [Test]
        public void Initialize()
        {
            var range = new DataRange(3, 5);

            Assert.That(range.Minimum, Is.EqualTo(3));
            Assert.That(range.Maximum, Is.EqualTo(5));
        }

        /// <summary>
        /// Tests that an exception is thrown if one of the constructor parameters is NaN.
        /// </summary>
        [Test]
        public void Initialize_ThrowsArgumentException_NaN()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new DataRange(double.NaN, 3));
            Assert.That(ex.Message, Is.EqualTo("NaN values are not permitted"));
        }

        /// <summary>
        /// Tests that an exception is thrown if provided minimum in constructor is larger than maximum.
        /// </summary>
        [Test]
        public void Initialize_ThrowsArgumentException_MinGreaterMax()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new DataRange(5, 3));
            Assert.That(ex.Message, Is.EqualTo("max must be larger or equal min"));
        }

        /// <summary>
        /// Tests the range property.
        /// </summary>
        [Test]
        public void RangeProperty()
        {
            var range = new DataRange(3, 5);

            Assert.That(range.Range, Is.EqualTo(2));
        }

        /// <summary>
        /// Tests whether the <see cref="DataRange.IsDefined"/> method works correctly.
        /// </summary>
        [Test]
        public void IsDefined()
        {
            Assert.That(new DataRange(3, 5).IsDefined(), Is.True);
            Assert.That(new DataRange(double.NegativeInfinity, double.PositiveInfinity).IsDefined(), Is.True);
            Assert.That(new DataRange(double.MinValue, double.MaxValue).IsDefined(), Is.True);

#pragma warning disable SA1129 // Do not use default value type constructor
            Assert.That(new DataRange().IsDefined(), Is.False);
#pragma warning restore SA1129 // Do not use default value type constructor

            Assert.That(DataRange.Undefined.IsDefined(), Is.False);
        }

        /// <summary>
        /// Tests whether the default instance is undefined.
        /// </summary>
        [Test]
        public void DefaultIsUndefined()
        {
            Assert.That(default(DataRange).IsDefined(), Is.False);
        }

        /// <summary>
        /// Tests the method to determine whether a given value
        /// is inside the data range or not.
        /// </summary>
        [Test]
        public void Contains()
        {
            var range = new DataRange(3, 5);

            Assert.That(range.Contains(3), Is.True);
            Assert.That(range.Contains(4), Is.True);
            Assert.That(range.Contains(5), Is.True);

            Assert.That(range.Contains(2), Is.False);
            Assert.That(range.Contains(6), Is.False);
        }

        /// <summary>
        /// Tests the method to determine whether a data range instance overlaps
        /// with a second one or not.
        /// </summary>
        [Test]
        public void IntersectsWith()
        {
            var range = new DataRange(3, 5);

            Assert.That(range.IntersectsWith(new DataRange(1, 4)), Is.True);
            Assert.That(range.IntersectsWith(new DataRange(4, 6)), Is.True);
            Assert.That(range.IntersectsWith(new DataRange(4, 4)), Is.True);
            Assert.That(range.IntersectsWith(new DataRange(1, 6)), Is.True);

            Assert.That(range.IntersectsWith(new DataRange(1, 3)), Is.True);
            Assert.That(range.IntersectsWith(new DataRange(5, 6)), Is.True);

            Assert.That(range.IntersectsWith(new DataRange(-5, -3)), Is.False);
            Assert.That(range.IntersectsWith(new DataRange(13, 15)), Is.False);

            Assert.That(range.IntersectsWith(DataRange.Undefined), Is.False);
        }

        /// <summary>
        /// Tests whether the <see cref="DataRange.ToCode"/> method returns
        /// the expected string.
        /// </summary>
        [Test]
        public void ToCode()
        {
            Assert.That(new DataRange(3, 5).ToCode(), Is.EqualTo("new DataRange(3,5)"));
        }

        /// <summary>
        /// Tests whether the <see cref="DataRange.ToString"/> method returns
        /// the expected string.
        /// </summary>
        [Test]
        public void TestToString()
        {
            Assert.That(new DataRange(3, 5).ToString(), Is.EqualTo("[3, 5]"));
        }
    }
}
