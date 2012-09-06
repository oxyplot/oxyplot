// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConrecTests.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class ConrecTests
    {
        [Test]
        public void Contour()
        {
            var x = ArrayHelper.CreateVector(0, 10, 100);
            var y = ArrayHelper.CreateVector(0, 10, 100);
            var z = ArrayHelper.CreateVector(-1, 1, 20);
            var data = ArrayHelper.Evaluate((x1, y1) => Math.Sin(x1 * y1), x, y);
            int segments = 0;
            Conrec.Contour(data, x, y, z, (x1, y1, x2, y2, elev) => { segments++; });
            Assert.AreEqual(134068, segments);
        }
    }
}