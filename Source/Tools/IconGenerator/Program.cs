namespace IconGenerator
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.IO;

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
            int[] sizes = { 512, 256, 228, 195, 152, 144, 128, 120, 114, 96, 72, 64, 57, 48, 40, 32, 24, 20, 16 };
            pngOptimizer = Path.GetFullPath(@"..\..\..\..\..\Tools\TruePNG\TruePNG.exe");

            var iconRenderer = new Candidate5();

            foreach (var size in sizes)
            {
                // CreateBitmap(size, icon1, string.Format("v1_{0}.png", size));
                var pngFile = string.Format("OxyPlot_{0}.png", size);
                var svgFile = string.Format("OxyPlot_{0}.svg", size);
                Console.WriteLine("Generating {0}x{0} icon", size);
                Console.WriteLine("  Saving {0}", svgFile);
                CreateSvg(size, iconRenderer, svgFile);
                Console.WriteLine("  Saving {0}", pngFile);
                CreateBitmap(size, iconRenderer, pngFile);
                Console.WriteLine("  Optimizing {0}", pngFile);
                OptimizePng(pngFile);
                Console.WriteLine();
            }

            var ico = new IcoMaker.Icon();
            ico.AddImage("OxyPlot_16.png");
            ico.AddImage("OxyPlot_24.png");
            ico.AddImage("OxyPlot_32.png");
            ico.AddImage("OxyPlot_48.png");
            ico.AddImage("OxyPlot_64.png");
            ico.AddImage("OxyPlot_256.png");
            ico.Save("OxyPlot.ico");

            // Process.Start("Explorer.exe", "/select," + Path.GetFullPath("OxyPlot.ico"));
        }

        private static void OptimizePng(string pngFile)
        {
            var psi = new ProcessStartInfo(pngOptimizer, pngFile + " /o max");
            //  psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            var p = Process.Start(psi);
            if (p != null)
            {
                p.WaitForExit();
            }
        }

        static void CreateBitmap(int size, IconRenderer iconRenderer, string fileName)
        {
            using (var bm = new Bitmap(size, size))
            {
                using (var g = Graphics.FromImage(bm))
                {
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    //g.SmoothingMode = SmoothingMode.AntiAlias;
                    //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    //g.CompositingQuality = CompositingQuality.HighQuality;
                    //g.CompositingMode = CompositingMode.SourceOver;
                    var rc = new GraphicsRenderContext { RendersToScreen = false };
                    rc.SetGraphicsTarget(g);
                    iconRenderer.Render(rc, size);
                    bm.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        static void CreateSvg(int size, IconRenderer iconRenderer, string fileName)
        {
            var grx = new GraphicsRenderContext();
            using (var bm = new Bitmap(size, size))
            {
                grx.SetGraphicsTarget(Graphics.FromImage(bm));
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
            pm.Axes.Add(new LinearAxis(AxisPosition.Left) { TextColor = OxyColors.Transparent });
            pm.Axes.Add(new LinearAxis(AxisPosition.Bottom) { TextColor = OxyColors.Transparent });
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
                    pixels[x, y] = palette.Colors[i];
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
                    pixels[x, y] = palette.Colors[i];
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
