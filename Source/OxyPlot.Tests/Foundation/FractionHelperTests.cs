// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoubleExtensionsTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class FractionHelperTests
    {
        [Test]
        public void ConvertToFractionString()
        {
            Assert.AreEqual("3/4", FractionHelper.ConvertToFractionString(0.75));
        }

        [Test]
        public void ConvertToFractionString_WithUnit()
        {
            Assert.AreEqual("2pi", FractionHelper.ConvertToFractionString(Math.PI * 2, Math.PI, "pi"));
        }
    }
}