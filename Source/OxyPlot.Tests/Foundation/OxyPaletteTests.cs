// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPaletteTests.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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