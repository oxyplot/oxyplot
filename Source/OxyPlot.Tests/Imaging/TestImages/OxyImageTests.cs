// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyImageTests.cs" company="OxyPlot">
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
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class OxyImageTests
    {
        [Test]
        [TestCase("test_8bit.png", ImageFormat.Png)]
        [TestCase("test_32bit.png", ImageFormat.Png)]
        [TestCase("test_8bit.bmp", ImageFormat.Bmp)]
        [TestCase("test_24bit.bmp", ImageFormat.Bmp)]
        [TestCase("test_32bit.bmp", ImageFormat.Bmp)]
        //// [TestCase("test.jpg", ImageFormat.Jpeg)]
        public void GetFormat_TestFiles_(string fileName, ImageFormat expectedImageFormat)
        {
            var image = new OxyImage(File.ReadAllBytes(@"Imaging\TestImages\" + fileName));
            Assert.AreEqual(expectedImageFormat, image.Format);
            Assert.AreEqual(137, image.Width);
            Assert.AreEqual(59, image.Height);
            Assert.AreEqual(72, Math.Round(image.DpiX));
            Assert.AreEqual(72, Math.Round(image.DpiY));
        }

        [Test]
        public void Create_SmallImageToPng()
        {
            var data = new OxyColor[2, 4];
            data[1, 0] = OxyColors.Blue;
            data[1, 1] = OxyColors.Green;
            data[1, 2] = OxyColors.Red;
            data[1, 3] = OxyColors.White;
            data[0, 0] = OxyColor.FromAColor(127, OxyColors.Yellow);
            data[0, 1] = OxyColor.FromAColor(127, OxyColors.Orange);
            data[0, 2] = OxyColor.FromAColor(127, OxyColors.Pink);
            data[0, 3] = OxyColors.Transparent;
            var img = OxyImage.Create(data, ImageFormat.Png);
            var bytes = img.GetData();
            File.WriteAllBytes(@"Imaging\SmallImage.png", bytes);
        }

        [Test]
        public void Create_LargeImageToPng()
        {
            int w = 266;
            int h = 40;
            var data = new OxyColor[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    data[x, y] = OxyColor.FromHsv((double)x / w, 1, 1);
                }
            }

            var img = OxyImage.Create(data, ImageFormat.Png);
            var bytes = img.GetData();
            File.WriteAllBytes(@"Imaging\LargeImage.png", bytes);
        }

        [Test]
        public void Create_SmallImageToBmp()
        {
            var data = new OxyColor[2, 4];
            data[1, 0] = OxyColors.Blue;
            data[1, 1] = OxyColors.Green;
            data[1, 2] = OxyColors.Red;
            data[1, 3] = OxyColors.White;
            data[0, 0] = OxyColor.FromAColor(127, OxyColors.Yellow);
            data[0, 1] = OxyColor.FromAColor(127, OxyColors.Orange);
            data[0, 2] = OxyColor.FromAColor(127, OxyColors.Pink);
            data[0, 3] = OxyColors.Transparent;
            var img = OxyImage.Create(data, ImageFormat.Bmp, new BmpEncoderOptions());
            var bytes = img.GetData();
            File.WriteAllBytes(@"Imaging\SmallImage.bmp", bytes);
        }

        [Test]
        public void Create_Indexed8bitToBmp()
        {
            var data = new byte[,] { { 0, 1, 2, 3 }, { 4, 5, 6, 7 } };

            var palette = new OxyColor[8];
            palette[4] = OxyColors.Blue;
            palette[5] = OxyColors.Green;
            palette[6] = OxyColors.Red;
            palette[7] = OxyColors.White;
            palette[0] = OxyColor.FromAColor(127, OxyColors.Yellow);
            palette[1] = OxyColor.FromAColor(127, OxyColors.Orange);
            palette[2] = OxyColor.FromAColor(127, OxyColors.Pink);
            palette[3] = OxyColors.Transparent;
            var img = OxyImage.Create(data, palette, ImageFormat.Bmp);
            var bytes = img.GetData();
            File.WriteAllBytes(@"Imaging\FromIndexed8.bmp", bytes);
        }

        [Test]
        public void Discussion453825_100()
        {
            var data = new byte[100, 100];
            int k = 0;
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    data[i, j] = (byte)(k++ % 256);
                }
            }

            var palette = OxyPalettes.Gray(256).Colors.ToArray();
            var im = OxyImage.Create(data, palette, ImageFormat.Bmp);
            File.WriteAllBytes(@"Imaging\Discussion453825.bmp", im.GetData());
        }

        [Test]
        public void Discussion453825_199()
        {
            int w = 199;
            int h = 256;
            var data = new byte[h, w];
            var data2 = new byte[h * w];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    data[i, j] = (byte)(i % 256);
                    data2[(i * w) + j] = (byte)(i % 256);
                }
            }

            var palette1 = OxyPalettes.Gray(256).Colors.ToArray();
            var im1 = OxyImage.Create(data, palette1, ImageFormat.Bmp);
            var bytes1 = im1.GetData();

            // The images should show a gradient image 199x256 - black at the bottom, white at the top
            File.WriteAllBytes(@"Imaging\Discussion453825_199a.bmp", bytes1);
        }
    }
}