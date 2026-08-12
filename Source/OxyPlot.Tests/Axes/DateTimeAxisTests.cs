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
            Assert.That(DateTimeAxis.ToDouble(date), Is.EqualTo(date.ToOADate()));
        }

        [Test]
        public void ToDouble_NoDate()
        {
            Assert.That(DateTimeAxis.ToDouble(new DateTime()), Is.EqualTo(-693593));
        }

        [Test]
        public void ToDateTime_ValidDate()
        {
            var date = new DateTime(2011, 3, 15);
            Assert.That(DateTimeAxis.ToDateTime(date.ToOADate(), DateTimeAxis.DefaultPrecision), Is.EqualTo(date));
        }

        [Test]
        public void ToDateTime_NoDate()
        {
            Assert.That(DateTimeAxis.ToDateTime(-693593, DateTimeAxis.DefaultPrecision), Is.EqualTo(new DateTime()));
        }

        [Test]
        public void ToDateTime_NaN()
        {
            Assert.That(DateTimeAxis.ToDateTime(double.NaN, DateTimeAxis.DefaultPrecision), Is.EqualTo(new DateTime()));
        }

        [Test]
        public void ToDateTime_VeryBigValue()
        {
            Assert.That(DateTimeAxis.ToDateTime(double.MaxValue, DateTimeAxis.DefaultPrecision), Is.EqualTo(new DateTime()));
        }

        [Test]
        public void ToDateTime_VerySmallValue()
        {
            Assert.That(DateTimeAxis.ToDateTime(double.MinValue, DateTimeAxis.DefaultPrecision), Is.EqualTo(new DateTime()));
        }
    }
}
