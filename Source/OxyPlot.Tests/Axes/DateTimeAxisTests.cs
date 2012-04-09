// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeAxisTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
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
    public class DateTimeAxisTests
    {
        [Test]
        public void ToDouble_ValidDate()
        {
            Assert.AreEqual(40616, DateTimeAxis.ToDouble(new DateTime(2011, 3, 15)));
        }

        [Test]
        public void ToDouble_NoDate()
        {
            Assert.AreEqual(-693594, DateTimeAxis.ToDouble(new DateTime()));
        }

        [Test]
        public void ToDateTime_ValidDate()
        {
            Assert.AreEqual(new DateTime(2011, 3, 15), DateTimeAxis.ToDateTime(40616));
        }

        [Test]
        public void ToDateTime_NoDate()
        {
            Assert.AreEqual(new DateTime(), DateTimeAxis.ToDateTime(-693594));
        }

        [Test]
        public void ToDateTime_NaN()
        {
            Assert.AreEqual(new DateTime(), DateTimeAxis.ToDateTime(double.NaN));
        }
    }
}