// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FractionHelperTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class FractionHelperTests
    {
        [Test]
        public void ConvertToFractionString()
        {
            Assert.That(FractionHelper.ConvertToFractionString(0.75), Is.EqualTo("3/4"));
        }

        [Test]
        public void ConvertToFractionString_WithUnit()
        {
            Assert.That(FractionHelper.ConvertToFractionString(Math.PI * 2, Math.PI, "pi"), Is.EqualTo("2pi"));
        }
    }
}