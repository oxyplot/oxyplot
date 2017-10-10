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

            // Try with some invalid values.
            palette = null;
            palette = OxyPalette.Interpolate(-9, OxyColors.Blue, OxyColors.White, OxyColors.Red);
            Assert.AreEqual(0, palette.Colors.Count);

            palette = null;
            palette = OxyPalette.Interpolate(5, null);
            Assert.AreEqual(0, palette.Colors.Count);

            palette = null;
            palette = OxyPalette.Interpolate(0, null);
            Assert.AreEqual(0, palette.Colors.Count);

            // Try corner cases.
            palette = null;
            palette = OxyPalette.Interpolate(1, OxyColors.Blue, OxyColors.White, OxyColors.Red);
            Assert.AreEqual(1, palette.Colors.Count);
            Assert.AreEqual(OxyColors.Blue, palette.Colors[0]);

            palette = null;
            palette = OxyPalette.Interpolate(2, OxyColors.Blue, OxyColors.White, OxyColors.Red);
            Assert.AreEqual(2, palette.Colors.Count);
            Assert.AreEqual(OxyColors.Blue, palette.Colors[0]);
            Assert.AreEqual(OxyColors.Red, palette.Colors[1]);

            palette = null;
            palette = OxyPalette.Interpolate(4, OxyColors.Blue);
            Assert.AreEqual(4, palette.Colors.Count);
            Assert.AreEqual(OxyColors.Blue, palette.Colors[0]);
            Assert.AreEqual(OxyColors.Blue, palette.Colors[1]);
            Assert.AreEqual(OxyColors.Blue, palette.Colors[2]);
            Assert.AreEqual(OxyColors.Blue, palette.Colors[3]);
        }

        [Test]
        public void Constructor()
        {
            var palette = new OxyPalette(OxyColors.Blue, OxyColors.White, OxyColors.Red);
            Assert.AreEqual(3, palette.Colors.Count);
        }
    }
}