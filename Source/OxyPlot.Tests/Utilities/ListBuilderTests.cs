// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListBuilderTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using OxyPlot.Series;

    [TestFixture]
    public class ListBuilderTests
    {
        private IList<TestObject> src;

        [SetUp]
        public void Setup()
        {
            this.src = new List<TestObject> { new TestObject { A = 3.14 } };
        }

        [Test]
        public void Fill()
        {
            var target = new List<ScatterPoint>();

            var filler = new ListBuilder<ScatterPoint>();
            filler.Add("A", 0d);
            filler.Fill(target, this.src, args => new ScatterPoint(Convert.ToDouble(args[0]), 0));

            Assert.AreEqual(1, target.Count);
            Assert.AreEqual(3.14, target[0].X);
        }

        [Test]
        public void FillDataPoints()
        {
            var target = new List<DataPoint>();

            var filler = new ListBuilder<DataPoint>();
            filler.Add("A", 0d);
            filler.Fill(target, this.src, args => new DataPoint(Convert.ToDouble(args[0]), 0));

            Assert.AreEqual(1, target.Count);
            Assert.AreEqual(3.14, target[0].X);
        }

        [Test]
        public void Fill_InvalidProperty_UsesDefaultValue()
        {
            var target = new List<ScatterPoint>();

            var filler = new ListBuilder<ScatterPoint>();
            filler.Add("B", 0d);
            filler.Fill(target, this.src, args => new ScatterPoint(Convert.ToDouble(args[0]), 0));
        }

        [Test]
        public void Fill_NullProperty_DefaultValueIsExpected()
        {
            var target = new List<ScatterPoint>();

            var filler = new ListBuilder<ScatterPoint>();
            filler.Add(null, 42);
            filler.Fill(target, this.src, args => new ScatterPoint(Convert.ToDouble(args[0]), 0));

            Assert.AreEqual(1, target.Count);
            Assert.AreEqual(42, target[0].X);
        }

        private class TestObject
        {
            public double A { get; set; }
        }
    }
}