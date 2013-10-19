namespace IconGenerator
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
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
            int[] sizes = { 256, 228, 195, 152, 144, 128, 120, 114, 96, 72, 64, 57, 48, 40, 32, 24, 20, 16 };

            var icon1 = new Icon1();
            var icon2 = new Icon2();
            foreach (var size in sizes)
            {
                // CreateBitmap(size, icon1, string.Format("v1_{0}.png", size));
                CreateBitmap(size, icon2, string.Format("OxyPlot_{0}.png", size));
                // CreateSvg(size, icon2, string.Format("OxyPlot_{0}.svg", size));
            }

            var ico = new IcoMaker.Icon();
            ico.AddImage("OxyPlot_16.png");
            ico.AddImage("OxyPlot_24.png");
            ico.AddImage("OxyPlot_32.png");
            ico.AddImage("OxyPlot_48.png");
            ico.AddImage("OxyPlot_64.png");
            ico.AddImage("OxyPlot_256.png");
            ico.Save("OxyPlot.ico");
        }

        static void CreateBitmap(int size, IconRenderer iconRenderer, string fileName)
        {
            using (var bm = new Bitmap(size, size))
            {
                using (var g = Graphics.FromImage(bm))
                {
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    //g.CompositingQuality = CompositingQuality.HighQuality;
                    //g.CompositingMode = CompositingMode.SourceOver;
                    var rc = new GraphicsRenderContext { RendersToScreen = false };
                    rc.SetGraphicsTarget(g);
                    iconRenderer.Render(rc, size);
                    bm.Save(fileName, ImageFormat.Png);
                }
            }
        }

        static void CreateSvg(int size, IconRenderer iconRenderer, string fileName)
        {
            var grx = new GraphicsRenderContext();
            using (var bm = new Bitmap(size, size))
            {
                grx.SetGraphicsTarget(Graphics.FromImage(bm));
                using (var s = File.OpenWrite(fileName))
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

    public class Icon1 : IconRenderer
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

    public class Icon2 : IconRenderer
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
            Func<double, double, double> peaks =
                (x, y) =>
                3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1))
                - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y)
                - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);
            var xvalues = ArrayHelper.CreateVector(x0, x1, 100);
            var yvalues = ArrayHelper.CreateVector(y0, y1, 100);
            var peaksData = ArrayHelper.Evaluate(peaks, xvalues, yvalues);

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
}
