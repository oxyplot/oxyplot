// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxesTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class AnnotationsTests
    {
        [Test]
        public void LineAnnotation()
        {
            var s1 = new OxyPlot.LineAnnotation();
            var s2 = new OxyPlot.Wpf.LineAnnotation();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }
   }
}