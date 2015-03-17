// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensionsTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class ReflectionExtensionsTests
    {
        [Test]
        public void AddFormattedRangeWithSimpleProperties()
        {
            var items = new List<Item> { new Item { A = 1, B = 2 } };
            var result = new List<string>();
            result.AddFormattedRange(items, "A", "0.0", CultureInfo.InvariantCulture);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo("1.0"));
        }

        [Test]
        public void AddRangeWithSimpleProperties()
        {
            var items = new List<Item> { new Item { A = 1, B = 2 } };
            var result = new List<double>();
            result.AddRange(items, "A");
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(1));
        }

        [Test]
        public void AddRangeWithPath()
        {
            var items = new List<Item> { new Item { Point = new DataPoint(1.1, 2.2) } };
            var result = new List<double>();
            result.AddRange(items, "Point.X");
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(1.1));
        }

        [Test]
        public void ListOfDataPointAddRangeWithSimpleProperties()
        {
            var items = new List<Item> { new Item { A = 1.1, B = 2.2 } };
            var result = new List<DataPoint>();
            result.AddRange(items, "A", "B");
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].X, Is.EqualTo(1.1));
            Assert.That(result[0].Y, Is.EqualTo(2.2));
        }

        [Test]
        public void ListOfDataPointAddRangeWithPath()
        {
            var items = new List<Item> { new Item { Point = new DataPoint(1.1, 2.2) } };
            var result = new List<DataPoint>();
            result.AddRange(items, "Point.X", "Point.Y");
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].X, Is.EqualTo(1.1));
            Assert.That(result[0].Y, Is.EqualTo(2.2));
        }

        /// <summary>
        /// Represents a test item.
        /// </summary>
        public class Item
        {
            /// <summary>
            /// Gets or sets a value.
            /// </summary>
            public double A { get; set; }

            /// <summary>
            /// Gets or sets the b value.
            /// </summary>
            public double B { get; set; }

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
            public ReflectionPathTests.Item SubItem { get; set; }
        }
    }
}