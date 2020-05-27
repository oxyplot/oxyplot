// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngAssert.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides assertions on image files.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp.Tests
{
    using global::SkiaSharp;
    using NUnit.Framework;
    using OxyPlot.SkiaSharp;
    using System.IO;

    /// <summary>
    /// Provides assertions on image files.
    /// </summary>
    public static class PngAssert
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
            for (var i = 0; i < h; i++)
            {
                for (var j = 0; j < w; j++)
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
                    for (var i = 0; i < h; i++)
                    {
                        for (var j = 0; j < w; j++)
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

            using var bitmap = SKBitmap.Decode(path);
            var ret = new OxyColor[bitmap.Width, bitmap.Height];

            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    ret[x, y] = pixel.ToOxyColor();
                }
            }

            return ret;
        }
    }
}
