// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayHelperTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using NUnit.Framework;
    using System.Globalization;
    using System;

    [TestFixture]
    public class StringHelperTests
    {
        [Test]
        public void Format_StandardFormatString()
        {
            Assert.AreEqual("3.1",StringHelper.Format(CultureInfo.InvariantCulture, "{0}", null, 3.1));
            Assert.AreEqual("3.14",StringHelper.Format(CultureInfo.InvariantCulture, "{0:0.00}", null, Math.PI));
            Assert.AreEqual("PI=3.14",StringHelper.Format(CultureInfo.InvariantCulture, "PI={0:0.00}", null, Math.PI));
        }
        
        [Test]
        public void Format_Item()
        {
            var item = new Item() { Text = "Hello World", Value = 3.14 };
            Assert.AreEqual("3.14 3 Hello World 3.140",
                StringHelper.Format(CultureInfo.InvariantCulture, "{0} {1:0} {Text} {Value:0.000}", item, item.Value, item.Value));
            Assert.AreEqual("Hello World",
                StringHelper.Format(CultureInfo.InvariantCulture, "{Text}", item));
        }

        public class Item
        {
            public string Text { get; set; }
            public double Value { get; set; }
        }
    }
}