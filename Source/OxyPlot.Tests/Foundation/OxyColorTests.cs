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
            Assert.AreEqual(OxyColors.Red, OxyColor.Parse("#FF0000"));
            Assert.AreEqual(OxyColors.Red, OxyColor.Parse("#FFFF0000"));
            Assert.AreEqual(OxyColors.Red, OxyColor.Parse("255,0,0"));
            Assert.AreEqual(OxyColors.Red, OxyColor.Parse("255,255,0,0"));
            Assert.AreEqual(OxyColors.Undefined, OxyColor.Parse(null));
            Assert.AreEqual(OxyColors.Undefined, OxyColor.Parse("None"));
            Assert.AreEqual(OxyColors.Automatic, OxyColor.Parse("Auto"));
            Assert.AreEqual(OxyColors.Undefined, OxyColor.Parse("#00000000"));
            Assert.AreEqual(OxyColors.Automatic, OxyColor.Parse("#00000001"));
            Assert.AreEqual(OxyColors.White, OxyColor.Parse("#FFF"));
        }

        [Test]
        public void ColorDifference()
        {
            Assert.AreEqual(1.1189122525867927d, OxyColor.ColorDifference(OxyColors.Red, OxyColors.Green), 1e-6);
        }

        [Test]
        public void FromAColor()
        {
            Assert.AreEqual(OxyColor.FromArgb(0x80, 0xff, 0, 0), OxyColor.FromAColor(0x80, OxyColors.Red));
        }

        [Test]
        public void FromHsv()
        {
            Assert.AreEqual(OxyColors.Red, OxyColor.FromHsv(0, 1, 1));
        }

        [Test]
        public void FromRgb()
        {
            Assert.AreEqual(OxyColors.Red, OxyColor.FromRgb(255, 0, 0));
        }

        [Test]
        public void FromArgb()
        {
            Assert.AreEqual(OxyColors.Red, OxyColor.FromArgb(255, 255, 0, 0));
        }

        [Test]
        public void FromUInt32()
        {
            Assert.AreEqual(OxyColors.Red, OxyColor.FromUInt32(0xFFFF0000));
        }

        [Test]
        public void GetColorName()
        {
            Assert.AreEqual("Red", OxyColors.Red.GetColorName());
        }

        [Test]
        public void ChangeIntensity()
        {
            Assert.AreEqual(OxyColor.FromArgb(255, 127, 0, 0), OxyColors.Red.ChangeIntensity(0.5));
        }

        [Test]
        public void ChangeSaturation()
        {
            Assert.AreEqual(OxyColor.FromArgb(255, 255, 127, 127), OxyColors.Red.ChangeSaturation(0.5));
        }

        [Test]
        public void ChangeSaturation_OverSaturate()
        {
            Assert.AreEqual(OxyColor.FromArgb(255, 255, 0, 0), OxyColors.Red.ChangeSaturation(2));
        }

        [Test]
        public void Complementary()
        {
            Assert.AreEqual(OxyColors.Cyan, OxyColors.Red.Complementary());
        }

        [Test]
        public void ToByteString()
        {
            Assert.AreEqual("255,255,0,0", OxyColors.Red.ToByteString());
        }

        [Test]
        public void ToCode()
        {
            Assert.AreEqual("OxyColors.Red", OxyColors.Red.ToCode());
            Assert.AreEqual("OxyColor.FromArgb(1, 2, 3, 4)", OxyColor.FromArgb(0x01, 0x02, 0x03, 0x04).ToCode());
        }

        [Test]
        public void ToHsv()
        {
            Assert.AreEqual(new[] { 0, 1, 1 }, OxyColors.Red.ToHsv());
        }

        [Test]
        public new void ToString()
        {
            Assert.AreEqual("#ffff0000", OxyColors.Red.ToString());
            Assert.AreEqual("#00000001", OxyColors.Automatic.ToString());
            Assert.AreEqual("#00000000", OxyColors.Undefined.ToString());
        }

        [Test]
        public void ToUint()
        {
            Assert.AreEqual(0xFFFF0000, OxyColors.Red.ToUint());
        }

        [Test]
        public void HueDifference()
        {
            Assert.AreEqual(1.0 / 3, OxyColor.HueDifference(OxyColors.Red, OxyColors.Blue), 1e-6);
        }
    }
}
