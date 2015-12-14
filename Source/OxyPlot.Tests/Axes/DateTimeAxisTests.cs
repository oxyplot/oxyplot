// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeAxisTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    using OxyPlot.Axes;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class DateTimeAxisTests
    {
        [Test]
        public void ToDouble_ValidDate()
        {
            var date = new DateTime(2011, 3, 15);
            Assert.AreEqual(date.ToOADate(), DateTimeAxis.ToDouble(date));
        }

        [Test]
        public void ToDouble_NoDate()
        {
            Assert.AreEqual(-693593, DateTimeAxis.ToDouble(new DateTime()));
        }

        [Test]
        public void ToDateTime_ValidDate()
        {
            var date = new DateTime(2011, 3, 15);
            Assert.AreEqual(date, DateTimeAxis.ToDateTime(date.ToOADate()));
        }

        [Test]
        public void ToDateTime_NoDate()
        {
            Assert.AreEqual(new DateTime(), DateTimeAxis.ToDateTime(-693593));
        }

        [Test]
        public void ToDateTime_NaN()
        {
            Assert.AreEqual(new DateTime(), DateTimeAxis.ToDateTime(double.NaN));
        }

        [Test]
        public void ToDateTime_VeryBigValue()
        {
            Assert.AreEqual(new DateTime(), DateTimeAxis.ToDateTime(double.MaxValue));
        }

        [Test]
        public void ToDateTime_VerySmallValue()
        {
            Assert.AreEqual(new DateTime(), DateTimeAxis.ToDateTime(double.MinValue));
        }
    }
}