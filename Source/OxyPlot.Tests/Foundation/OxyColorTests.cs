// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyColorTests.cs" company="OxyPlot">
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
    public class OxyColorTests
    {
        [Test]
        public void Parse()
        {
            Assert.That(OxyColor.Parse("#FF0000"), Is.EqualTo(OxyColors.Red));
            Assert.That(OxyColor.Parse("#FFFF0000"), Is.EqualTo(OxyColors.Red));
            Assert.That(OxyColor.Parse("255,0,0"), Is.EqualTo(OxyColors.Red));
            Assert.That(OxyColor.Parse("255,255,0,0"), Is.EqualTo(OxyColors.Red));
            Assert.That(OxyColor.Parse(null), Is.EqualTo(OxyColors.Undefined));
            Assert.That(OxyColor.Parse("None"), Is.EqualTo(OxyColors.Undefined));
            Assert.That(OxyColor.Parse("Auto"), Is.EqualTo(OxyColors.Automatic));
            Assert.That(OxyColor.Parse("#00000000"), Is.EqualTo(OxyColors.Undefined));
            Assert.That(OxyColor.Parse("#00000001"), Is.EqualTo(OxyColors.Automatic));
            Assert.That(OxyColor.Parse("#FFF"), Is.EqualTo(OxyColors.White));
        }

        [Test]
        public void ColorDifference()
        {
            Assert.That(OxyColor.ColorDifference(OxyColors.Red, OxyColors.Green), Is.EqualTo(1.1189122525867927d).Within(1e-6));
        }

        [Test]
        public void FromAColor()
        {
            Assert.That(OxyColor.FromAColor(0x80, OxyColors.Red), Is.EqualTo(OxyColor.FromArgb(0x80, 0xff, 0, 0)));
        }

        [Test]
        public void FromHsv()
        {
            Assert.That(OxyColor.FromHsv(0, 1, 1), Is.EqualTo(OxyColors.Red));
        }

        [Test]
        public void FromRgb()
        {
            Assert.That(OxyColor.FromRgb(255, 0, 0), Is.EqualTo(OxyColors.Red));
        }

        [Test]
        public void FromArgb()
        {
            Assert.That(OxyColor.FromArgb(255, 255, 0, 0), Is.EqualTo(OxyColors.Red));
        }

        [Test]
        public void FromUInt32()
        {
            Assert.That(OxyColor.FromUInt32(0xFFFF0000), Is.EqualTo(OxyColors.Red));
        }

        [Test]
        public void GetColorName()
        {
            Assert.That(OxyColors.Red.GetColorName(), Is.EqualTo("Red"));
        }

        [Test]
        public void ChangeIntensity()
        {
            Assert.That(OxyColors.Red.ChangeIntensity(0.5), Is.EqualTo(OxyColor.FromArgb(255, 127, 0, 0)));
        }

        [Test]
        public void ChangeSaturation()
        {
            Assert.That(OxyColors.Red.ChangeSaturation(0.5), Is.EqualTo(OxyColor.FromArgb(255, 255, 127, 127)));
        }

        [Test]
        public void ChangeSaturation_OverSaturate()
        {
            Assert.That(OxyColors.Red.ChangeSaturation(2), Is.EqualTo(OxyColor.FromArgb(255, 255, 0, 0)));
        }

        [Test]
        public void Complementary()
        {
            Assert.That(OxyColors.Red.Complementary(), Is.EqualTo(OxyColors.Cyan));
        }

        [Test]
        public void ToByteString()
        {
            Assert.That(OxyColors.Red.ToByteString(), Is.EqualTo("255,255,0,0"));
        }

        [Test]
        public void ToCode()
        {
            Assert.That(OxyColors.Red.ToCode(), Is.EqualTo("OxyColors.Red"));
            Assert.That(OxyColor.FromArgb(0x01, 0x02, 0x03, 0x04).ToCode(), Is.EqualTo("OxyColor.FromArgb(1, 2, 3, 4)"));
        }

        [Test]
        public void ToHsv()
        {
            Assert.That(OxyColors.Red.ToHsv(), Is.EqualTo(new[] { 0, 1, 1 }));
        }

        [Test]
        public new void ToString()
        {
            Assert.That(OxyColors.Red.ToString(), Is.EqualTo("#ffff0000"));
            Assert.That(OxyColors.Automatic.ToString(), Is.EqualTo("#00000001"));
            Assert.That(OxyColors.Undefined.ToString(), Is.EqualTo("#00000000"));
        }

        [Test]
        public void ToUint()
        {
            Assert.That(OxyColors.Red.ToUint(), Is.EqualTo(0xFFFF0000));
        }

        [Test]
        public void HueDifference()
        {
            Assert.That(OxyColor.HueDifference(OxyColors.Red, OxyColors.Blue), Is.EqualTo(1.0 / 3).Within(1e-6));
        }
    }
}
