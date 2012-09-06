// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyColorExtensionsTests.cs" company="OxyPlot">
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
    public class OxyColorExtensionsTests
    {
        [Test]
        public void ChangeAlpha()
        {
            var c = OxyColors.Red.ChangeAlpha(100);
            Assert.AreEqual(100,c.A);
            Assert.AreEqual(255,c.R);
        }
    }
}