// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionPathTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics;

    using NUnit.Framework;

    /// <summary>
    /// Unit tests for <see cref="ReflectionPath" />.
    /// </summary>
    [TestFixture]
    public class ReflectionPathTests
    {
        /// <summary>
        /// Given a structure, when reflecting for a property, then null is returned.
        /// </summary>
        [Test]
        public void Null()
        {
            var item = new Item();
            var rp = new ReflectionPath("SubItem");
            object result;
            Assert.IsTrue(rp.TryGetValue(item, out result));
            Assert.That(rp.GetValue(item), Is.Null);
        }

        /// <summary>
        /// Given a structure, when reflecting for a property, then null is returned.
        /// </summary>
        [Test]
        public void Null2()
        {
            var item = new Item();
            var rp = new ReflectionPath("SubItem.X");
            object result;
            Assert.IsTrue(rp.TryGetValue(item, out result));
            Assert.That(rp.GetValue(item), Is.Null);
        }

        /// <summary>
        /// Given a structure with properties, when reflecting for a simple property, then the value should be found.
        /// </summary>
        [Test]
        public void SimpleProperty()
        {
            var item = new Item { Number = 41 };
            var rp = new ReflectionPath("Number");
            object result;
            Assert.IsTrue(rp.TryGetValue(item, out result));
            Assert.That(rp.GetValue(item), Is.EqualTo(41));
        }

        /// <summary>
        /// Given a structure with a point, when reflecting for the X coordinate, then the value should be found.
        /// </summary>
        [Test]
        public void PointInStructure()
        {
            var item = new Item { Point = new DataPoint(1, 2) };
            var rp = new ReflectionPath("Point.X");
            object result;
            Assert.IsTrue(rp.TryGetValue(item, out result));
            Assert.That(rp.GetValue(item), Is.EqualTo(1));
        }

        /// <summary>
        /// Given a substructure with a point, when reflecting for the X coordinate, then the value should be found.
        /// </summary>
        [Test]
        public void PointInSubStructure()
        {
            var item = new Item { SubItem = new Item { Point = new DataPoint(1, 2) } };
            var rp = new ReflectionPath("SubItem.Point.X");
            Assert.That(rp.GetValue(item), Is.EqualTo(1));
        }

        /// <summary>
        /// Given a substructure with a point, when reflecting for the X coordinate, then the value should be found.
        /// </summary>
        [Test]
        public void Performance()
        {
            var item = new Item { SubItem = new Item { Point = new DataPoint(1, 2) } };
            var rp = new ReflectionPath("SubItem.Point.X");
            var w = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                rp.GetValue(item);
            }

            Assert.Less(w.ElapsedMilliseconds, 2000);
        }

        /// <summary>
        /// Given a structure with a point, when reflecting for the Z coordinate, an exception should be thrown.
        /// </summary>
        [Test]
        public void InvalidPath()
        {
            var item = new Item { Point = new DataPoint(1, 2) };
            var rp = new ReflectionPath("Point.Z");
            object result;
            Assert.IsFalse(rp.TryGetValue(item, out result));
            Assert.Throws<InvalidOperationException>(() => rp.GetValue(item));
        }

        /// <summary>
        /// Represents a test item.
        /// </summary>
        public class Item
        {
            /// <summary>
            /// Gets or sets the number.
            /// </summary>
            /// <value>
            /// The number.
            /// </value>
            public int Number { get; set; }

            /// <summary>
            /// Gets or sets the point.
            /// </summary>
            /// <value>
            /// The point.
            /// </value>
            public DataPoint Point { get; set; }

            /// <summary>
            /// Gets or sets the sub item.
            /// </summary>
            /// <value>
            /// The sub item.
            /// </value>
            public Item SubItem { get; set; }
        }
    }
}
