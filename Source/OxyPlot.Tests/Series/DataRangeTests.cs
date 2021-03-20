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

            Assert.AreEqual(3, range.Minimum);
            Assert.AreEqual(5, range.Maximum);
        }

        /// <summary>
        /// Tests that an exception is thrown if one of the constructor parameters is NaN.
        /// </summary>
        [Test]
        public void Initialize_ThrowsArgumentException_NaN()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new DataRange(double.NaN, 3));
            Assert.AreEqual("NaN values are not permitted", ex.Message);
        }

        /// <summary>
        /// Tests that an exception is thrown if provided minimum in constructor is larger than maximum.
        /// </summary>
        [Test]
        public void Initialize_ThrowsArgumentException_MinGreaterMax()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => new DataRange(5, 3));
            Assert.AreEqual("max must be larger or equal min", ex.Message);
        }

        /// <summary>
        /// Tests the range property.
        /// </summary>
        [Test]
        public void RangeProperty()
        {
            var range = new DataRange(3, 5);

            Assert.AreEqual(2, range.Range);
        }

        /// <summary>
        /// Tests whether the <see cref="DataRange.IsDefined"/> method works correctly.
        /// </summary>
        [Test]
        public void IsDefined()
        {
            Assert.IsTrue(new DataRange(3, 5).IsDefined());
            Assert.IsTrue(new DataRange(double.NegativeInfinity, double.PositiveInfinity).IsDefined());
            Assert.IsTrue(new DataRange(double.MinValue, double.MaxValue).IsDefined());

#pragma warning disable SA1129 // Do not use default value type constructor
            Assert.IsFalse(new DataRange().IsDefined());
#pragma warning restore SA1129 // Do not use default value type constructor

            Assert.IsFalse(DataRange.Undefined.IsDefined());
        }

        /// <summary>
        /// Tests whether the default instance is undefined.
        /// </summary>
        [Test]
        public void DefaultIsUndefined()
        {
            Assert.IsFalse(default(DataRange).IsDefined());
        }

        /// <summary>
        /// Tests the method to determine whether a given value
        /// is inside the data range or not.
        /// </summary>
        [Test]
        public void Contains()
        {
            var range = new DataRange(3, 5);

            Assert.IsTrue(range.Contains(3));
            Assert.IsTrue(range.Contains(4));
            Assert.IsTrue(range.Contains(5));

            Assert.IsFalse(range.Contains(2));
            Assert.IsFalse(range.Contains(6));
        }

        /// <summary>
        /// Tests the method to determine whether a data range instance overlaps
        /// with a second one or not.
        /// </summary>
        [Test]
        public void IntersectsWith()
        {
            var range = new DataRange(3, 5);

            Assert.IsTrue(range.IntersectsWith(new DataRange(1, 4)));
            Assert.IsTrue(range.IntersectsWith(new DataRange(4, 6)));
            Assert.IsTrue(range.IntersectsWith(new DataRange(4, 4)));
            Assert.IsTrue(range.IntersectsWith(new DataRange(1, 6)));

            Assert.IsTrue(range.IntersectsWith(new DataRange(1, 3)));
            Assert.IsTrue(range.IntersectsWith(new DataRange(5, 6)));

            Assert.IsFalse(range.IntersectsWith(new DataRange(-5, -3)));
            Assert.IsFalse(range.IntersectsWith(new DataRange(13, 15)));

            Assert.IsFalse(range.IntersectsWith(DataRange.Undefined));
        }

        /// <summary>
        /// Tests whether the <see cref="DataRange.ToCode"/> method returns
        /// the expected string.
        /// </summary>
        [Test]
        public void ToCode()
        {
            Assert.AreEqual("new DataRange(3,5)", new DataRange(3, 5).ToCode());
        }

        /// <summary>
        /// Tests whether the <see cref="DataRange.ToString"/> method returns
        /// the expected string.
        /// </summary>
        [Test]
        public void TestToString()
        {
            Assert.AreEqual("[3, 5]", new DataRange(3, 5).ToString());
        }
    }
}
