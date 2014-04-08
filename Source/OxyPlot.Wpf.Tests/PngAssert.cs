// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngAssert.cs" company="OxyPlot">
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
// <summary>
//   Provides assertions on image files.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using System;
    using System.Windows.Media.Imaging;

    using NUnit.Framework;

    /// <summary>
    /// Provides assertions on image files.
    /// </summary>
    public class PngAssert
    {
        /// <summary>
        /// Determines if the images at the specified paths are equal.
        /// </summary>
        /// <param name="expected">Path to the expected image.</param>
        /// <param name="actual">Path to the actual image.</param>
        /// <param name="message">The message.</param>
        public static void AreEqual(string expected, string actual, string message)
        {
            var expectedImage = LoadImage(expected);
            var actualImage = LoadImage(actual);

            if (expectedImage.GetLength(0) != actualImage.GetLength(0))
            {
                Assert.Fail("Expected height: {0}\nActual height:{1}\n{2}", expectedImage.GetLength(0), actualImage.GetLength(0), message);
            }

            if (expectedImage.GetLength(1) != actualImage.GetLength(1))
            {
                Assert.Fail("Expected width: {0}\nActual width:{1}\n{2}", expectedImage.GetLength(1), actualImage.GetLength(1), message);
            }

            var w = expectedImage.GetLength(1);
            var h = expectedImage.GetLength(0);
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (!expectedImage[i, j].Equals(actualImage[i, j]))
                    {
                        Assert.Fail("At pixel {0},{1}:\nExpected : {2}\nActual:{3}\n{4}", j, i, expectedImage[i, j], actualImage[i, j], message);
                    }
                }
            }
        }

        /// <summary>
        /// Loads an image from the specified path.
        /// </summary>
        /// <param name="path">The path to the image file.</param>
        /// <returns>The pixels as an array.</returns>
        private static OxyColor[,] LoadImage(string path)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(path, UriKind.Relative);
            bitmapImage.EndInit();

            // Assumes ARGB
            int size = bitmapImage.PixelHeight * bitmapImage.PixelWidth * 4;
            var pixels = new byte[size];
            bitmapImage.CopyPixels(pixels, bitmapImage.PixelWidth * 4, 0);

            var r = new OxyColor[bitmapImage.PixelHeight, bitmapImage.PixelWidth];
            var index = 0;
            for (int i = 0; i < bitmapImage.PixelHeight; i++)
            {
                for (int j = 0; j < bitmapImage.PixelWidth; j++)
                {
                    byte red = pixels[index++];
                    byte green = pixels[index++];
                    byte blue = pixels[index++];
                    byte alpha = pixels[index++];
                    r[i, j] = OxyColor.FromArgb(alpha, red, green, blue);
                }
            }

            return r;
        }
    }
}