// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListFillerTests.cs" company="OxyPlot">
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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    using OxyPlot.Series;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class ListFillerTests
    {
        private IList<TestObject> src;

        [SetUp]
        public void Setup()
        {
            src = new List<TestObject>();
            src.Add(new TestObject() { A = 3.14 });
        }

        [Test]
        public void Fill()
        {
            var target = new List<ScatterPoint>();

            var filler = new ListFiller<ScatterPoint>();
            filler.Add("A", (p, v) => p.X = Convert.ToDouble(v));
            filler.Fill(target, src);

            Assert.AreEqual(1, target.Count);
            Assert.AreEqual(3.14, target[0].X);
        }

        [Test, ExpectedException]
        public void Fill_InvalidProperty_ThrowsException()
        {
            var target = new List<ScatterPoint>();

            var filler = new ListFiller<ScatterPoint>();
            filler.Add("B", (p, v) => p.X = Convert.ToDouble(v));
            filler.Fill(target, src);
        }

        [Test]
        public void Fill_NullProperty_IsIgnored()
        {
            var target = new List<ScatterPoint>();

            var filler = new ListFiller<ScatterPoint>();
            filler.Add(null, (p, v) => { });
            filler.Fill(target, src);

            Assert.AreEqual(1, target.Count);
            Assert.AreEqual(0, target[0].X);
        }

        private class TestObject
        {
            public double A { get; set; }
        }
    }
}