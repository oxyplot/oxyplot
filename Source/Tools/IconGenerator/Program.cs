// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="OxyPlot">
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
//   Generates OxyPlot icons in many different sizes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace IconGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.IO;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.WindowsForms;

    /// <summary>
    /// Generates OxyPlot icons in many different sizes.
    /// </summary>
    class Program
    {
        private static string pngOptimizer;

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var dir = args[0];
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                Directory.SetCurrentDirectory(dir);
            }

            // http://msdn.microsoft.com/en-us/library/windows/desktop/aa511280.aspx#size
            // https://github.com/audreyr/favicon-cheat-sheet
            // https://developer.apple.com/library/ios/documentation/userexperience/conceptual/mobilehig/IconMatrix.html
            var sizes = new List<int>();

            // Standard icon sizes
            sizes.AddRange(new[] { 256, 64, 48, 32, 24, 16 });

            // Windows store app
            sizes.AddRange(new[] { 150, 310, 126, 98, 70, 56, 270, 210, 120, 558, 434, 310, 248, 54, 42, 30, 24, 256, 48, 32, 16, 90, 70, 50, 43, 33, 24, 1116, 540, 868, 420, 620, 300 });

            // Apple OS X?
            sizes.AddRange(new[] { 1024, 44, 22, 25 });

            // Apple App icons
            sizes.Add(57); // iPhone iOS 5-6
            sizes.Add(114); // iPhone @2x iOS 5-6
            sizes.Add(120); // iPhone @2x iOS 7
            sizes.Add(72); // iPad iOS 5-6
            sizes.Add(144); // iPad @2x iOS 5-6
            sizes.Add(76); // iPad iOS 7
            sizes.Add(152); // iPad @2x iOS 7

            // Apple Spotlight & Settings icons
            sizes.Add(29); // iPhone Spotlight iOS 5,6 + Settings iOS 5-7
            sizes.Add(58); // iPhone Spotlight @2x iOS 5,6 + Settings @2x iOS 5-7
            sizes.Add(50); // iPad Spotlight iOS 5-6
            sizes.Add(100); // iPad Spotlight @2x iOS 5-6
            sizes.Add(40); // Spotlight iOS 7
            sizes.Add(80); // Spotlight @2x 7

            // Other
            sizes.AddRange(new[] { 512, 256, 248, 228, 195, 144, 128, 114, 100, 96, 72, 57, 42, 40, 33, 20 });
            sizes.AddRange(new[] { 2048 }); // Really big
            pngOptimizer = Path.GetFullPath(@"TruePNG.exe");

            var iconRenderer = new Candidate5();
            bool generateSvg = false;

            foreach (var size in sizes.Distinct().OrderBy(x => x))
            {
                // CreateBitmap(size, icon1, string.Format("v1_{0}.png", size));
                var pngFile = string.Format("OxyPlot_{0}.png", size);
                var svgFile = string.Format("OxyPlot_{0}.svg", size);
                Console.WriteLine("Generating {0}x{0} icon", size);
                if (generateSvg)
                {
                    Console.WriteLine("  Saving {0}", svgFile);
                    CreateSvg(size, iconRenderer, svgFile);
                }

                Console.WriteLine("  Saving {0}", pngFile);
                CreateBitmap(size, iconRenderer, pngFile);
                Console.WriteLine("  Optimizing {0}", pngFile);
                OptimizePng(pngFile);
                Console.WriteLine();
            }

            var icon = new IcoMaker.Icon();
            icon.AddImage("OxyPlot_16.png");
            icon.AddImage("OxyPlot_24.png");
            icon.AddImage("OxyPlot_32.png");
            icon.AddImage("OxyPlot_48.png");
            icon.AddImage("OxyPlot_64.png");
            icon.AddImage("OxyPlot_256.png");
            icon.Save("OxyPlot.ico");

            var favIcon = new IcoMaker.Icon();
            favIcon.AddImage("OxyPlot_16.png");
            favIcon.AddImage("OxyPlot_32.png");
            favIcon.AddImage("OxyPlot_48.png");
            favIcon.AddImage("OxyPlot_64.png");
            favIcon.Save("favicon.ico");

            // Process.Start("Explorer.exe", "/select," + Path.GetFullPath("OxyPlot.ico"));
        }

        private static void OptimizePng(string pngFile)
        {
            // /o max : optimization level
            // /nc : don't change ColorType and BitDepth
            // /md keep pHYs : keep pHYs metadata
            var psi = new ProcessStartInfo(pngOptimizer, pngFile + " /o max /nc /md keep pHYs")
                          {
                              CreateNoWindow = true,
                              WindowStyle = ProcessWindowStyle.Hidden
                          };
            var p = Process.Start(psi);
            p.WaitForExit();
        }

        private static GraphicsPath GetRoundedRect(RectangleF baseRect, float radius)
        {
            // Based on http://www.codeproject.com/Articles/5649/Extended-Graphics-An-implementation-of-Rounded-Rec
            // create the arc for the rectangle sides and declare
            // a graphics path object for the drawing
            float diameter = radius * 2f;
            var sizeF = new SizeF(diameter, diameter);
            var arc = new RectangleF(baseRect.Location, sizeF);
            var path = new GraphicsPath();

            // top left arc
            path.AddArc(arc, 180, 90);

            // top right arc
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        static void CreateBitmap(int size, IconRenderer iconRenderer, string fileName, double cornerRadius = 0)
        {
            using (var bm = new Bitmap(size, size, PixelFormat.Format32bppArgb))
            {
                using (var g = Graphics.FromImage(bm))
                {
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.CompositingMode = CompositingMode.SourceOver;
                    using (var rc = new GraphicsRenderContext(g) { RendersToScreen = false })
                    {
                        g.Clear(Color.Transparent);

                        if (cornerRadius > 0)
                        {
                            // TODO: no antialiasing on the clipping path?
                            var path = GetRoundedRect(new RectangleF(0, 0, size, size), (float)cornerRadius);
                            g.SetClip(path, CombineMode.Replace);
                        }

                        iconRenderer.Render(rc, size);
                    }

                    bm.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        static Bitmap Resize(Bitmap source, int width, int height)
        {
            var bmp = new Bitmap(width, height, source.PixelFormat);
            var g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.High;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawImage(source, new Rectangle(0, 0, width, height));
            return bmp;
        }

        static void CreateSvg(int size, IconRenderer iconRenderer, string fileName)
        {
            using (var bm = new Bitmap(size, size))
            {
                using (var grx = new GraphicsRenderContext(Graphics.FromImage(bm)))
                {
                    using (var s = File.Create(fileName))
                    {
                        using (var rc = new SvgRenderContext(s, size, size, true, grx, OxyColors.Transparent))
                        {
                            iconRenderer.Render(rc, size);
                        }
                    }
                }
            }
        }
    }

    public abstract class IconRenderer
    {
        /// <summary>
        /// Renders the icon on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="size">The size.</param>
        public abstract void Render(IRenderContext rc, double size);
    }

    /// <summary>
    /// Generates an icon by text.
    /// </summary>
    public class Candidate1 : IconRenderer
    {
        /// <summary>
        /// Renders the icon on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="size">The size.</param>
        public override void Render(IRenderContext rc, double size)
        {
            var m = size * 0.05;
            var rect = new OxyRect(m, m, size - m * 2, size - m * 2);
            rc.DrawEllipse(rect, OxyColors.Black, OxyColors.Black, 0);
            rc.DrawText(new ScreenPoint(size * 0.52, size * 0.32), "XY", OxyColors.White, "Arial", size * 0.25, FontWeights.Bold, 0, HorizontalAlignment.Center, VerticalAlignment.Middle);
            rc.DrawText(new ScreenPoint(size * 0.52, size * 0.64), "PLOT", OxyColors.White, "Arial", size * 0.25, FontWeights.Bold, 0, HorizontalAlignment.Center, VerticalAlignment.Middle);
        }
    }

    /// <summary>
    /// Generates a heatmap icon.
    /// </summary>
    public class Candidate2 : IconRenderer
    {
        /// <summary>
        /// Renders the icon on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="size">The size.</param>
        public override void Render(IRenderContext rc, double size)
        {
            var pm = new PlotModel
                         {
                             AutoAdjustPlotMargins = false,
                             Padding = new OxyThickness(size * 0.02),
                             PlotMargins = new OxyThickness(size * 0.05, size * 0.05, size * 0.07, size * 0.07)
                         };
            pm.Axes.Add(new LinearAxis { Position = AxisPosition.Left, TextColor = OxyColors.Transparent });
            pm.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, TextColor = OxyColors.Transparent });
            this.AddPeaks(pm);
            pm.Update();
            pm.Render(rc, size, size);
        }

        /// <summary>
        /// Adds a peaks HeatMapSeries to the plot model.
        /// </summary>
        /// <param name="pm">The plot model.</param>
        public void AddPeaks(PlotModel pm)
        {
            double x0 = -3.1;
            double x1 = 3.1;
            double y0 = -3;
            double y1 = 3;
            var xx = ArrayHelper.CreateVector(x0, x1, 100);
            var yy = ArrayHelper.CreateVector(y0, y1, 100);
            var peaksData = ArrayHelper.Evaluate(Functions.Peaks, xx, yy);

            pm.Axes.Add(
                new LinearColorAxis
                    {
                        Position = AxisPosition.Right,
                        Palette = OxyPalettes.Jet(500),
                        HighColor = OxyColors.Gray,
                        LowColor = OxyColors.Black,
                        IsAxisVisible = false
                    });

            var hms = new HeatMapSeries { X0 = x0, X1 = x1, Y0 = y0, Y1 = y1, Data = peaksData };
            pm.Series.Add(hms);
        }
    }

    /// <summary>
    /// Generates a heatmap icon based on the peaks function and a "jet" palette.
    /// </summary>
    public class Candidate3 : IconRenderer
    {
        /// <summary>
        /// Renders the icon on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="size">The size.</param>
        public override void Render(IRenderContext rc, double size)
        {
            var n = (int)size;
            var data = ArrayHelper.Evaluate(Functions.Peaks, ArrayHelper.CreateVector(-3.1, 3.1, n), ArrayHelper.CreateVector(3, -3, n));
            var palette = OxyPalettes.Jet(256);
            var min = data.Min2D();
            var max = data.Max2D();
            var pixels = new OxyColor[n, n];
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    var i = (int)((data[x, y] - min) / (max - min) * palette.Colors.Count);
                    i = Math.Min(Math.Max(i, 0), palette.Colors.Count - 1);
                    rc.DrawRectangle(new OxyRect(x, y, 1, 1), palette.Colors[i], OxyColors.Undefined, 0);
                }
            }

            var frameWidth = (int)Math.Max(Math.Round(size / 32), 1);
            rc.DrawRectangle(new OxyRect(0, 0, size, frameWidth), OxyColors.Black, OxyColors.Black, 0);
            rc.DrawRectangle(new OxyRect(0, size - frameWidth, size, frameWidth), OxyColors.Black, OxyColors.Undefined, 0);
            rc.DrawRectangle(new OxyRect(0, 0, frameWidth, size), OxyColors.Black, OxyColors.Undefined, 0);
            rc.DrawRectangle(new OxyRect(size - frameWidth, 0, frameWidth, size), OxyColors.Black, OxyColors.Undefined, 0);
        }
    }

    /// <summary>
    /// Generates a heatmap icon based on the peaks function and blue-white-red palette.
    /// </summary>
    public class Candidate4 : IconRenderer
    {
        /// <summary>
        /// Renders the icon on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="size">The size.</param>
        public override void Render(IRenderContext rc, double size)
        {
            var n = (int)size * 2;
            var data = ArrayHelper.Evaluate(Functions.Peaks, ArrayHelper.CreateVector(-3.1, 2.6, n), ArrayHelper.CreateVector(3.0, -3.4, n));
            var palette = OxyPalettes.BlueWhiteRed(5);
            var min = data.Min2D();
            var max = data.Max2D();
            var pixels = new OxyColor[n, n];
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    var i = (int)((data[x, y] - min) / (max - min) * palette.Colors.Count);
                    i = Math.Min(Math.Max(i, 0), palette.Colors.Count - 1);
                    pixels[y, n - 1 - x] = palette.Colors[i];
                }
            }

            var image = OxyImage.Create(pixels, OxyPlot.ImageFormat.Png);
            rc.DrawImage(image, 0, 0, n, n, 0, 0, size, size, 1, true);

            var frameWidth = (int)Math.Max(Math.Round(size / 32), 1);
            rc.DrawRectangle(new OxyRect(0, 0, size, frameWidth), OxyColors.Black, OxyColors.Black, 0);
            rc.DrawRectangle(new OxyRect(0, size - frameWidth, size, frameWidth), OxyColors.Black, OxyColors.Undefined, 0);
            rc.DrawRectangle(new OxyRect(0, 0, frameWidth, size), OxyColors.Black, OxyColors.Undefined, 0);
            rc.DrawRectangle(new OxyRect(size - frameWidth, 0, frameWidth, size), OxyColors.Black, OxyColors.Undefined, 0);
        }
    }

    /// <summary>
    /// Generates a icon based on the peaks function and a simple blue-white palette.
    /// </summary>
    public class Candidate5 : IconRenderer
    {
        /// <summary>
        /// Renders the icon on the specified render context.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="size">The size.</param>
        public override void Render(IRenderContext rc, double size)
        {
            var n = (int)size * 2;
            var data = ArrayHelper.Evaluate(Functions.Peaks, ArrayHelper.CreateVector(-3.1, 2.6, n), ArrayHelper.CreateVector(3.0, -3.4, n));
            var c = OxyColor.FromArgb(255, 90, 165, 200);
            var palette = OxyPalette.Interpolate(5, OxyColors.White, c, OxyColors.White);
            var min = data.Min2D();
            var max = data.Max2D();
            var pixels = new OxyColor[n, n];
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    var i = (int)((data[x, y] - min) / (max - min) * palette.Colors.Count);
                    i = Math.Min(Math.Max(i, 0), palette.Colors.Count - 1);
                    pixels[y, n - 1 - x] = palette.Colors[i];
                }
            }

            var image = OxyImage.Create(pixels, OxyPlot.ImageFormat.Png);

            // fix image interpolation artifacts on the edge
            rc.DrawImage(image, 0, 0, n, n, 0, 0, size, size, 1, true);
            rc.DrawRectangle(new OxyRect(0, 0, size, 2), c, OxyColors.Undefined, 0);
            rc.DrawRectangle(new OxyRect(0, size - 1, size, 1), c, OxyColors.Undefined, 0);
            rc.DrawRectangle(new OxyRect(0, 0, 2, size), c, OxyColors.Undefined, 0);
            rc.DrawRectangle(new OxyRect(size - 1, 0, 1, size), c, OxyColors.Undefined, 0);
        }
    }
}