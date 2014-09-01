// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPaletteTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class OxyPaletteTests
    {
        [Test]
        public void Interpolate()
        {
            var palette = OxyPalette.Interpolate(5, OxyColors.Blue, OxyColors.White, OxyColors.Red);
            Assert.AreEqual(5, palette.Colors.Count);
            Assert.AreEqual(OxyColors.Blue, palette.Colors[0]);
            Assert.AreEqual(OxyColor.FromRgb(127, 127, 255), palette.Colors[1]);
            Assert.AreEqual(OxyColors.White, palette.Colors[2]);
            Assert.AreEqual(OxyColor.FromRgb(255, 127, 127), palette.Colors[3]);
            Assert.AreEqual(OxyColors.Red, palette.Colors[4]);
        }

        [Test]
        public void Constructor()
        {
            var palette = new OxyPalette(OxyColors.Blue, OxyColors.White, OxyColors.Red);
            Assert.AreEqual(3, palette.Colors.Count);
        }
    }
}