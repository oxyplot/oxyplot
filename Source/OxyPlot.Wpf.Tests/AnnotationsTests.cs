// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnotationsTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class AnnotationsTests
    {
        [Test]
        public void LineAnnotation()
        {
            var s1 = new OxyPlot.LineAnnotation();
            var s2 = new LineAnnotation();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }
   }
}