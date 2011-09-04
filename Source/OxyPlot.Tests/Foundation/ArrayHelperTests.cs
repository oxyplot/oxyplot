//-----------------------------------------------------------------------
// <copyright file="ArrayHelperTests.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ArrayHelperTests
    {
        [Test]
        public void CreateVector()
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
        public void CreateVector2()
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
            var X = ArrayHelper.CreateVector(0, 1, 0.1);
            var Y = ArrayHelper.CreateVector(0, 1, 0.1);
            var D = ArrayHelper.Evaluate((x, y) => x * y, X, Y);

            Assert.AreEqual(10, D.GetUpperBound(0));
            Assert.AreEqual(10, D.GetUpperBound(1));
            Assert.AreEqual(0, D[0, 0]);
            Assert.AreEqual(1, D[10, 10]);
            Assert.AreEqual(0.3 * 0.4, D[3, 4]);
        }
    }
}
