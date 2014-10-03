// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayBuilderTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class ArrayBuilderTests
    {
        [Test]
        public void CreateVector_ByDelta_ReturnsExpectedArray()
        {
            var v = ArrayBuilder.CreateVector(0, 1, 0.1);
            Assert.AreEqual(11, v.Length);
            Assert.AreEqual(0, v[0]);
            Assert.AreEqual(0.3, v[3]);
            Assert.AreEqual(0.6, v[6]);
            Assert.AreEqual(0.7, v[7]);
            Assert.AreEqual(1, v[10]);
        }

        [Test]
        public void CreateVector_ByNumberOfSteps_ReturnsExpectedArray()
        {
            var v = ArrayBuilder.CreateVector(0, 1, 11);
            Assert.AreEqual(11, v.Length);
            Assert.AreEqual(0, v[0]);
            Assert.AreEqual(0.3, v[3]);
            Assert.AreEqual(0.6, v[6]);
            Assert.AreEqual(0.7, v[7]);
            Assert.AreEqual(1, v[10]);
        }

        [Test]
        public void Evaluate()
        {
            var xvector = ArrayBuilder.CreateVector(0, 1, 0.1);
            var yvector = ArrayBuilder.CreateVector(0, 1, 0.1);
            var dvector = ArrayBuilder.Evaluate((x, y) => x * y, xvector, yvector);

            Assert.AreEqual(10, dvector.GetUpperBound(0));
            Assert.AreEqual(10, dvector.GetUpperBound(1));
            Assert.AreEqual(0, dvector[0, 0]);
            Assert.AreEqual(1, dvector[10, 10]);
            Assert.AreEqual(0.3 * 0.4, dvector[3, 4]);
        }

        [Test]
        public void Min2D()
        {
            var array1 = new double[,] { { 4, 2 } };
            Assert.AreEqual(2, array1.Min2D(), "Min2D()");
            var array2 = new[,] { { 4, double.NaN } };
            Assert.AreEqual(double.NaN, array2.Min2D(), "Min2D() with NaN");
            Assert.AreEqual(4, array2.Min2D(true), "Min2D(true) with NaN");
            var array3 = new[] { 4, double.NaN };
            Assert.AreEqual(double.NaN, array3.Min(), "LINQ Min()");
        }

        [Test]
        public void Max2D()
        {
            var array1 = new double[,] { { 4, 2 } };
            Assert.AreEqual(4, array1.Max2D(), "Max2D()");
            var array2 = new[,] { { 4, double.NaN } };
            Assert.AreEqual(4, array2.Max2D(), "Max2D() with NaN");
            var array3 = new[] { 4, double.NaN };
            Assert.AreEqual(4, array3.Max(), "LINQ Max()");
        }
    }
}