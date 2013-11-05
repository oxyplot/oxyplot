// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeAxisTests.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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