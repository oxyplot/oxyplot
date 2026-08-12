// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringHelperTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class StringHelperTests
    {
        [Test]
        public void Format_StandardFormatString()
        {
            Assert.That(StringHelper.Format(CultureInfo.InvariantCulture, "{0}", null, 3.1), Is.EqualTo("3.1"));
            Assert.That(StringHelper.Format(CultureInfo.InvariantCulture, "{0:0.00}", null, Math.PI), Is.EqualTo("3.14"));
            Assert.That(StringHelper.Format(CultureInfo.InvariantCulture, "PI={0:0.00}", null, Math.PI), Is.EqualTo("PI=3.14"));
        }

        [Test]
        public void Format_Item()
        {
            var item = new Item { Text = "Hello World", Value = 3.14 };
            Assert.That(StringHelper.Format(CultureInfo.InvariantCulture, "{0} {1:0} {Text} {Value:0.000}", item, item.Value, item.Value), Is.EqualTo("3.14 3 Hello World 3.140"));
            Assert.That(StringHelper.Format(CultureInfo.InvariantCulture, "{Text}", item), Is.EqualTo("Hello World"));
        }

        [Test]
        public void CreateValidFormatString()
        {
            Assert.That(StringHelper.CreateValidFormatString(null), Is.EqualTo("{0}"), "null");
            Assert.That(StringHelper.CreateValidFormatString(string.Empty), Is.EqualTo("{0}"), "empty");
            Assert.That(StringHelper.CreateValidFormatString("0.00"), Is.EqualTo("{0:0.00}"), "0.00");
            Assert.That(StringHelper.CreateValidFormatString("Item {0}"), Is.EqualTo("Item {0}"), "Item {0}");
        }

        [Test]
        public void FormatIntegers()
        {
            var items = new[] { 1, 2, 3 };
            Assert.That(items.Format(null, null, CultureInfo.InvariantCulture), Is.EqualTo(new[] { "1", "2", "3" }).AsCollection);
            Assert.That(items.Format("", "00", CultureInfo.InvariantCulture), Is.EqualTo(new[] { "01", "02", "03" }).AsCollection);
            Assert.That(items.Format(null, "Item {0}", CultureInfo.InvariantCulture), Is.EqualTo(new[] { "Item 1", "Item 2", "Item 3" }).AsCollection);
        }

        [Test]
        public void FormatStrings()
        {
            var items = new[] { "One", "Two", "Three" };
            Assert.That(items.Format("Length", null, CultureInfo.InvariantCulture), Is.EqualTo(new[] { "3", "3", "5" }).AsCollection);
            Assert.That(items.Format(null, "Item {0}", CultureInfo.InvariantCulture), Is.EqualTo(new[] { "Item One", "Item Two", "Item Three" }).AsCollection);
        }

        public class Item
        {
            public string Text { get; set; }

            public double Value { get; set; }
        }
    }
}
