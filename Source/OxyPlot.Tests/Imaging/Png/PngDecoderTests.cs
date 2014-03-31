// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngDecoderTests.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class PngDecoderTests
    {
        [Test]
        [TestCase(@"Imaging\TestImages\test_32bit.png", 137, 59)]
        public void Decode_32bitTestImages(string path, int w, int h)
        {
            var d = new PngDecoder();
            var pixels = d.Decode(File.ReadAllBytes(path));
            Assert.AreEqual(w, pixels.GetLength(0));
            Assert.AreEqual(h, pixels.GetLength(1));
            Assert.IsNotNull(pixels);
            var e = new PngEncoder(new PngEncoderOptions());
            var encodedPixels = e.Encode(pixels);
            File.WriteAllBytes(Path.ChangeExtension(path, "out.png"), encodedPixels);
        }

        [Test]
        [TestCase(@"Imaging\TestImages\test_8bit.png", 137, 59)]
        public void Decode_8bitTestImages(string path, int w, int h)
        {
            var d = new PngDecoder();

            // Expect exception here, 8bit pngs are not yet supported
            Assert.Throws<NotImplementedException>(() => d.Decode(File.ReadAllBytes(path)));
        }
    }
}