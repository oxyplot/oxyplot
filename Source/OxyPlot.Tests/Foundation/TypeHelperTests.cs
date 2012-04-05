// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoubleExtensionsTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class TypeHelperTests
    {
        [Test]
        public void GetTypeName()
        {
            Assert.AreEqual("TypeHelperTests", TypeHelper.GetTypeName(this.GetType()));
        }
    }
}