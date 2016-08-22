// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngAssert.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides assertions on image files.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using System;
    using System.IO;
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
        /// <param name="output">The output difference file.</param>
        public static void AreEqual(string expected, string actual, string message, string output)
        {
            var expectedImage = LoadImage(expected);
            var actualImage = LoadImage(actual);

            if (expectedImage == null)
            {
                EnsureFolder(expected);
                File.Copy(actual, expected);
                Assert.Fail("File not found: {0}", expected);
            }

            if (expectedImage.GetLength(0) != actualImage.GetLength(0))
            {
                Assert.Fail("Expected height: {0}\nActual height:{1}\n{2}", expectedImage.GetLength(0), actualImage.GetLength(0), message);
            }

            if (expectedImage.GetLength(1) != actualImage.GetLength(1))
            {
                Assert.Fail("Expected width: {0}\nActual width:{1}\n{2}", expectedImage.GetLength(1), actualImage.GetLength(1), message);
            }

            var w = expectedImage.GetLength(0);
            var h = expectedImage.GetLength(1);
            var differences = 0;
            var differenceImage = new OxyColor[w, h];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (!expectedImage[j, i].Equals(actualImage[j, i]))
                    {
                        differences++;
                        differenceImage[j, i] = OxyColors.Red;
                    }
                    else
                    {
                        differenceImage[j, i] = actualImage[j, i].ChangeIntensity(100);
                    }
                }
            }

            if (differences > 0)
            {
                if (output != null)
                {
                    for (int i = 0; i < h; i++)
                    {
                        for (int j = 0; j < w; j++)
                        {
                            if (!expectedImage[j, i].Equals(actualImage[j, i]))
                            {
                            }
                        }
                    }

                    EnsureFolder(output);
                    var encoder = new PngEncoder(new PngEncoderOptions());
                    File.WriteAllBytes(output, encoder.Encode(differenceImage));
                }

                Assert.Fail(
                    "{0}:\nPixel differences: {1}\nExpected image: {2}\nActual image: {3}\nDiff image: {4}",
                    message,
                    differences,
                    Path.GetFullPath(expected),
                    Path.GetFullPath(actual),
                    Path.GetFullPath(output));
            }
        }

        /// <summary>
        /// Ensures that the folder for the specified path exists.
        /// </summary>
        /// <param name="path">The path to a file.</param>
        private static void EnsureFolder(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// Loads an image from the specified path.
        /// </summary>
        /// <param name="path">The path to the image file.</param>
        /// <returns>The pixels as an array.</returns>
        private static OxyColor[,] LoadImage(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            // TODO: use OxyPlot to decode
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(path, UriKind.Relative);
            bitmapImage.EndInit();

            // Assumes ARGB
            int size = bitmapImage.PixelHeight * bitmapImage.PixelWidth * 4;
            var pixels = new byte[size];
            bitmapImage.CopyPixels(pixels, bitmapImage.PixelWidth * 4, 0);

            var r = new OxyColor[bitmapImage.PixelWidth, bitmapImage.PixelHeight];
            var index = 0;
            for (int i = 0; i < bitmapImage.PixelHeight; i++)
            {
                for (int j = 0; j < bitmapImage.PixelWidth; j++)
                {
                    byte red = pixels[index++];
                    byte green = pixels[index++];
                    byte blue = pixels[index++];
                    byte alpha = pixels[index++];
                    r[j, i] = OxyColor.FromArgb(alpha, red, green, blue);
                }
            }

            return r;
        }
    }
}