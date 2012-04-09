// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoubleExtensionsTests.cs" company="OxyPlot">
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
    public class DoubleExtensionsTests
    {
        [Test]
        public void RemoveNoise()
        {
            double d1 = 3 * 0.1; // 0.30000000000000004            
            double d2 = d1.RemoveNoise();
            Assert.AreNotEqual(0.3, d1);
            Assert.AreEqual(0.3, d2);
        }

        [Test]
        public void RemoveNoise2()
        {
            double d1 = 3 * 0.1; // 0.30000000000000004
            double d2 = d1.RemoveNoise2();
            Assert.AreNotEqual(0.3, d1);
            Assert.AreEqual(0.3, d2);
        }
    }
}