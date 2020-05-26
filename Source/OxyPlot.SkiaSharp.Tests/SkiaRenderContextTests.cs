// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkiaRenderContextTests.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp.Tests
{
    using ExampleLibrary;
    using global::SkiaSharp;
    using NUnit.Framework;
    using System.IO;

    [TestFixture]
    public class SkiaRenderContextTests
    {
        private string outputDirectory;

        [OneTimeSetUp]
        public void Setup()
        {
            this.outputDirectory = Path.Combine(TestContext.CurrentContext.WorkDirectory, "SkiaRenderContextTests");
            Directory.CreateDirectory(this.outputDirectory);
        }

        [Test]
        public void TestDisposal()
        {
            var plotModel = (IPlotModel)ShowCases.CreateNormalDistributionModel();
            ((PlotModel)plotModel).DefaultFont = "Arial";
            plotModel.Update(true);

            string ExportToBitmap(string fileName, SkiaRenderContext context)
            {
                using var bitmap = new SKBitmap(800, 600);
                using var canvas = new SKCanvas(bitmap);

                context.SkCanvas = canvas;
                canvas.Clear(SKColors.White);
                plotModel.Render(context, new OxyRect(0, 0, 800, 600));

                var path = Path.Combine(this.outputDirectory, fileName + ".png");
                using var fileStream = File.OpenWrite(path);
                using var skStream = new SKManagedWStream(fileStream);
                bitmap.Encode(skStream, SKEncodedImageFormat.Png, 0);

                return path;
            }

            using var renderContext = new SkiaRenderContext() { RenderTarget = RenderTarget.PixelGraphic };
            var path1 = ExportToBitmap("export1", renderContext);

            string path2;
            using (var renderContext2 = new SkiaRenderContext() { RenderTarget = RenderTarget.PixelGraphic })
            {
                path2 = ExportToBitmap("export2", renderContext2);
            }

            var path3 = ExportToBitmap("export3", renderContext);

            PngAssert.AreEqual(path1, path2, null, null);
            PngAssert.AreEqual(path1, path3, null, null);
        }
    }
}
