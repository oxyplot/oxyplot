// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayHelperTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class ArrayHelperTests
    {
        [Test]
        public void CreateVector_ByDelta_ReturnsExpectedArray()
        {
            var v = ArrayHelper.CreateVector(0, 1, 0.1);
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
            var v = ArrayHelper.CreateVector(0, 1, 11);
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
            var xvector = ArrayHelper.CreateVector(0, 1, 0.1);
            var yvector = ArrayHelper.CreateVector(0, 1, 0.1);
            var dvector = ArrayHelper.Evaluate((x, y) => x * y, xvector, yvector);

            Assert.AreEqual(10, dvector.GetUpperBound(0));
            Assert.AreEqual(10, dvector.GetUpperBound(1));
            Assert.AreEqual(0, dvector[0, 0]);
            Assert.AreEqual(1, dvector[10, 10]);
            Assert.AreEqual(0.3 * 0.4, dvector[3, 4]);
        }
    }
}