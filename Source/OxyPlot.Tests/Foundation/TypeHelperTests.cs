// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeHelperTests.cs" company="OxyPlot">
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
    public class TypeHelperTests
    {
        [Test]
        public void GetTypeName()
        {
            Assert.AreEqual("TypeHelperTests", TypeHelper.GetTypeName(this.GetType()));
        }
    }
}