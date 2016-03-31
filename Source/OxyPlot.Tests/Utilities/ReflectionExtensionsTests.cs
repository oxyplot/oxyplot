// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensionsTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Globalization;

    using NUnit.Framework;

    [TestFixture]
    public class ReflectionExtensionsTests
    {
        [Test]
        public void FormatIntegers()
        {
            var items = new[] { 1, 2, 3 };
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, items.Format(null, null, CultureInfo.InvariantCulture));
            CollectionAssert.AreEqual(new[] { "01", "02", "03" }, items.Format("", "00", CultureInfo.InvariantCulture));
            CollectionAssert.AreEqual(new[] { "Item 1", "Item 2", "Item 3" }, items.Format(null, "Item {0}", CultureInfo.InvariantCulture));
        }

        [Test]
        public void FormatStrings()
        {
            var items = new[] { "One", "Two", "Three" };
            CollectionAssert.AreEqual(new[] { "3", "3", "5" }, items.Format("Length", null, CultureInfo.InvariantCulture));
            CollectionAssert.AreEqual(new[] { "Item One", "Item Two", "Item Three" }, items.Format(null, "Item {0}", CultureInfo.InvariantCulture));
        }        
    }
}