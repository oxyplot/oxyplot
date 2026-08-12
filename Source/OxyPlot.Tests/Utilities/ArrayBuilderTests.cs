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
            Assert.That(v.Length, Is.EqualTo(11));
            Assert.That(v[0], Is.EqualTo(0));
            Assert.That(v[3], Is.EqualTo(0.3));
            Assert.That(v[6], Is.EqualTo(0.6));
            Assert.That(v[7], Is.EqualTo(0.7));
            Assert.That(v[10], Is.EqualTo(1));
        }

        [Test]
        public void CreateVector_ByNumberOfSteps_ReturnsExpectedArray()
        {
            var v = ArrayBuilder.CreateVector(0, 1, 11);
            Assert.That(v.Length, Is.EqualTo(11));
            Assert.That(v[0], Is.EqualTo(0));
            Assert.That(v[3], Is.EqualTo(0.3));
            Assert.That(v[6], Is.EqualTo(0.6));
            Assert.That(v[7], Is.EqualTo(0.7));
            Assert.That(v[10], Is.EqualTo(1));
        }

        [Test]
        public void Evaluate()
        {
            var xvector = ArrayBuilder.CreateVector(0, 1, 0.1);
            var yvector = ArrayBuilder.CreateVector(0, 1, 0.1);
            var dvector = ArrayBuilder.Evaluate((x, y) => x * y, xvector, yvector);

            Assert.That(dvector.GetUpperBound(0), Is.EqualTo(10));
            Assert.That(dvector.GetUpperBound(1), Is.EqualTo(10));
            Assert.That(dvector[0, 0], Is.EqualTo(0));
            Assert.That(dvector[10, 10], Is.EqualTo(1));
            Assert.That(dvector[3, 4], Is.EqualTo(0.3 * 0.4));
        }

        [Test]
        public void Min2D()
        {
            var array1 = new double[,] { { 4, 2 } };
            Assert.That(array1.Min2D(), Is.EqualTo(2), "Min2D()");
            var array2 = new[,] { { 4, double.NaN } };
            Assert.That(array2.Min2D(), Is.EqualTo(double.NaN), "Min2D() with NaN");
            Assert.That(array2.Min2D(true), Is.EqualTo(4), "Min2D(true) with NaN");
            var array3 = new[] { 4, double.NaN };
            Assert.That(array3.Min(), Is.EqualTo(double.NaN), "LINQ Min()");
        }

        [Test]
        public void Max2D()
        {
            var array1 = new double[,] { { 4, 2 } };
            Assert.That(array1.Max2D(), Is.EqualTo(4), "Max2D()");
            var array2 = new[,] { { 4, double.NaN } };
            Assert.That(array2.Max2D(), Is.EqualTo(4), "Max2D() with NaN");
            var array3 = new[] { 4, double.NaN };
            Assert.That(array3.Max(), Is.EqualTo(4), "LINQ Max()");
        }
    }
}