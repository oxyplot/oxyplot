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
            Assert.That(palette.Colors.Count, Is.EqualTo(5));
            Assert.That(palette.Colors[0], Is.EqualTo(OxyColors.Blue));
            Assert.That(palette.Colors[1], Is.EqualTo(OxyColor.FromRgb(127, 127, 255)));
            Assert.That(palette.Colors[2], Is.EqualTo(OxyColors.White));
            Assert.That(palette.Colors[3], Is.EqualTo(OxyColor.FromRgb(255, 127, 127)));
            Assert.That(palette.Colors[4], Is.EqualTo(OxyColors.Red));

            // Try with some invalid values.
            palette = null;
            palette = OxyPalette.Interpolate(-9, OxyColors.Blue, OxyColors.White, OxyColors.Red);
            Assert.That(palette.Colors.Count, Is.EqualTo(0));

            palette = null;
            palette = OxyPalette.Interpolate(5, null);
            Assert.That(palette.Colors.Count, Is.EqualTo(0));

            palette = null;
            palette = OxyPalette.Interpolate(0, null);
            Assert.That(palette.Colors.Count, Is.EqualTo(0));

            // Try corner cases.
            palette = null;
            palette = OxyPalette.Interpolate(1, OxyColors.Blue, OxyColors.White, OxyColors.Red);
            Assert.That(palette.Colors.Count, Is.EqualTo(1));
            Assert.That(palette.Colors[0], Is.EqualTo(OxyColors.Blue));

            palette = null;
            palette = OxyPalette.Interpolate(2, OxyColors.Blue, OxyColors.White, OxyColors.Red);
            Assert.That(palette.Colors.Count, Is.EqualTo(2));
            Assert.That(palette.Colors[0], Is.EqualTo(OxyColors.Blue));
            Assert.That(palette.Colors[1], Is.EqualTo(OxyColors.Red));

            palette = null;
            palette = OxyPalette.Interpolate(4, OxyColors.Blue);
            Assert.That(palette.Colors.Count, Is.EqualTo(4));
            Assert.That(palette.Colors[0], Is.EqualTo(OxyColors.Blue));
            Assert.That(palette.Colors[1], Is.EqualTo(OxyColors.Blue));
            Assert.That(palette.Colors[2], Is.EqualTo(OxyColors.Blue));
            Assert.That(palette.Colors[3], Is.EqualTo(OxyColors.Blue));
        }

        [Test]
        public void Constructor()
        {
            var palette = new OxyPalette(OxyColors.Blue, OxyColors.White, OxyColors.Red);
            Assert.That(palette.Colors.Count, Is.EqualTo(3));
        }
    }
}