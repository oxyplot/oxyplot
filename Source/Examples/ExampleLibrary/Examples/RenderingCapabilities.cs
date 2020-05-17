// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingCapabilities.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides rendering capability examples.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using OxyPlot;
    using OxyPlot.Annotations;
    using System.Linq;

    /// <summary>
    /// Provides rendering capability examples.
    /// </summary>
    [Examples("9 Rendering capabilities")]
    public class RenderingCapabilities
    {
        /// <summary>
        /// Shows color capabilities for the DrawText method.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawText - Colors")]
        public static PlotModel DrawTextColors()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                const string Font = "Arial";
                const double FontSize = 32d;
                const double FontWeight = FontWeights.Bold;
                const double D = FontSize * 1.6;
                const double X = 20;
                double y = 20 - D;


                rc.DrawText(new ScreenPoint(X, y += D), "Black", OxyColors.Black, Font, FontSize, FontWeight);
                rc.DrawText(new ScreenPoint(X, y += D), "Red", OxyColors.Red, Font, FontSize, FontWeight);
                rc.DrawText(new ScreenPoint(X, y += D), "Green", OxyColors.Green, Font, FontSize, FontWeight);
                rc.DrawText(new ScreenPoint(X, y += D), "Blue", OxyColors.Blue, Font, FontSize, FontWeight);

                rc.FillRectangle(new OxyRect(X, y + D + 15, 200, 10), OxyColors.Black, EdgeRenderingMode.Adaptive);
                rc.DrawText(new ScreenPoint(X, y + D), "Yellow 50%", OxyColor.FromAColor(128, OxyColors.Yellow), Font, FontSize, FontWeight);
            }));
            return model;
        }

        /// <summary>
        /// Shows font capabilities for the DrawText method.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawText - Fonts")]
        public static PlotModel DrawTextFonts()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                const double FontSize = 20d;
                const double D = FontSize * 1.6;
                const double X = 20;
                double y = 20 - D;

                rc.DrawText(new ScreenPoint(X, y += D), "Default font", OxyColors.Black, null, FontSize);
                rc.DrawText(new ScreenPoint(X, y += D), "Helvetica", OxyColors.Black, "Helvetica", FontSize);
                rc.DrawText(new ScreenPoint(X, y += D), "Arial", OxyColors.Black, "Arial", FontSize);
                rc.DrawText(new ScreenPoint(X, y += D), "Courier", OxyColors.Black, "Courier", FontSize);
                rc.DrawText(new ScreenPoint(X, y += D), "Courier New", OxyColors.Black, "Courier New", FontSize);
                rc.DrawText(new ScreenPoint(X, y += D), "Times", OxyColors.Black, "Times", FontSize);
                rc.DrawText(new ScreenPoint(X, y + D), "Times New Roman", OxyColors.Black, "Times New Roman", FontSize);
            }));
            return model;
        }

        /// <summary>
        /// Shows font size capabilities for the DrawText method.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawText - Font sizes")]
        public static PlotModel DrawTextFontSizes()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                const double X = 20;
                double y = 20;

                // Font sizes
                foreach (var size in new[] { 10, 16, 24, 36, 48 })
                {
                    rc.DrawText(new ScreenPoint(X, y), size + "pt", OxyColors.Black, "Arial", size);
                    rc.DrawText(new ScreenPoint(X + 200, y), size + "pt", OxyColors.Black, "Arial", size, FontWeights.Bold);
                    y += size * 1.6;
                }
            }));
            return model;
        }

        /// <summary>
        /// Shows rotation capabilities for the DrawText method.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawText - Rotation")]
        public static PlotModel DrawTextRotation()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                var origin = new ScreenPoint(200, 200);
                rc.FillCircle(origin, 3, OxyColors.Blue, EdgeRenderingMode.Adaptive);
                for (int rotation = 0; rotation < 360; rotation += 45)
                {
                    rc.DrawText(origin, string.Format("Rotation {0}", rotation), OxyColors.Black, fontSize: 20d, rotation: rotation);
                }
            }));
            return model;
        }

        /// <summary>
        /// Shows alignment capabilities for the DrawText method.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawText - Alignment")]
        public static PlotModel DrawTextAlignment()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                const double FontSize = 20d;

                for (var ha = HorizontalAlignment.Left; ha <= HorizontalAlignment.Right; ha++)
                {
                    for (var va = VerticalAlignment.Top; va <= VerticalAlignment.Bottom; va++)
                    {
                        var origin = new ScreenPoint((((int)ha + 1) * 200) + 20, (((int)va + 1) * FontSize * 3) + 20);
                        rc.FillCircle(origin, 3, OxyColors.Blue, EdgeRenderingMode.Adaptive);
                        rc.DrawText(origin, ha + "-" + va, OxyColors.Black, fontSize: FontSize, horizontalAlignment: ha, verticalAlignment: va);
                    }
                }
            }));
            return model;
        }

        /// <summary>
        /// Shows alignment capabilities for the DrawText method with multi-line text.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawText - Multi-line Alignment")]
        public static PlotModel DrawTextMultiLineAlignment()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                const double FontSize = 20d;

                for (var ha = HorizontalAlignment.Left; ha <= HorizontalAlignment.Right; ha++)
                {
                    for (var va = VerticalAlignment.Top; va <= VerticalAlignment.Bottom; va++)
                    {
                        var origin = new ScreenPoint((((int)ha + 1) * 200) + 20, (((int)va + 1) * FontSize * 6) + 20);
                        rc.FillCircle(origin, 3, OxyColors.Blue, EdgeRenderingMode.Adaptive);
                        rc.DrawText(origin, ha + "\r\n" + va, OxyColors.Black, fontSize: FontSize, horizontalAlignment: ha, verticalAlignment: va);
                    }
                }
            }));
            return model;
        }

        /// <summary>
        /// Shows rotation capabilities for the DrawMathText method.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawMathText - Rotation")]
        public static PlotModel MathTextRotation()
        {
            var model = new PlotModel();
            var fontFamily = "Arial";
            var fontSize = 24;
            var fontWeight = FontWeights.Normal;
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                var origin = new ScreenPoint(200, 200);
                var origin2 = new ScreenPoint(400, 200);
                rc.FillCircle(origin, 3, OxyColors.Blue, EdgeRenderingMode.Adaptive);
                for (int rotation = 0; rotation < 360; rotation += 45)
                {
                    var text = "     A_{2}^{3}B";
                    rc.DrawMathText(origin, text, OxyColors.Black, fontFamily, fontSize, fontWeight, rotation, HorizontalAlignment.Left, VerticalAlignment.Middle);
                    var size = rc.MeasureMathText(text, fontFamily, fontSize, fontWeight);
                    var outline1 = size.GetPolygon(origin, rotation, HorizontalAlignment.Left, VerticalAlignment.Middle).ToArray();
                    rc.DrawPolygon(outline1, OxyColors.Undefined, OxyColors.Blue, 1, EdgeRenderingMode.Adaptive);

                    // Compare with normal text
                    var text2 = "     A B";
                    rc.DrawText(origin2, text2, OxyColors.Red, fontFamily, fontSize, fontWeight, rotation, HorizontalAlignment.Left, VerticalAlignment.Middle);
                    var size2 = rc.MeasureText(text2, fontFamily, fontSize, fontWeight);
                    var outline2 = size2.GetPolygon(origin2, rotation, HorizontalAlignment.Left, VerticalAlignment.Middle).ToArray();
                    rc.DrawPolygon(outline2, OxyColors.Undefined, OxyColors.Blue, 1, EdgeRenderingMode.Adaptive);
                }
            }));
            return model;
        }

        /// <summary>
        /// Shows alignment capabilities for the DrawMathText method.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawMathText - Alignment")]
        public static PlotModel DrawMathTextAlignment()
        {
            var text = "A_{2}^{3}B";
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                const string FontFamily = "Arial";
                const double FontSize = 20d;
                const double FontWeight = FontWeights.Normal;

                for (var ha = HorizontalAlignment.Left; ha <= HorizontalAlignment.Right; ha++)
                {
                    for (var va = VerticalAlignment.Top; va <= VerticalAlignment.Bottom; va++)
                    {
                        var origin = new ScreenPoint((((int)ha + 1) * 200) + 20, (((int)va + 1) * FontSize * 3) + 20);
                        rc.FillCircle(origin, 3, OxyColors.Blue, EdgeRenderingMode.Adaptive);
                        rc.DrawMathText(origin, text, OxyColors.Black, FontFamily, FontSize, FontWeight, 0, ha, va);
                    }
                }
            }));
            return model;
        }

        /// <summary>
        /// Shows alignment capabilities for the DrawText method.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawText - Alignment/Rotation")]
        public static PlotModel DrawTextAlignmentRotation()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                for (var ha = HorizontalAlignment.Left; ha <= HorizontalAlignment.Right; ha++)
                {
                    for (var va = VerticalAlignment.Top; va <= VerticalAlignment.Bottom; va++)
                    {
                        var origin = new ScreenPoint(((int)ha + 2) * 130, ((int)va + 2) * 130);
                        rc.FillCircle(origin, 3, OxyColors.Blue, EdgeRenderingMode.Adaptive);
                        for (int rotation = 0; rotation < 360; rotation += 90)
                        {
                            rc.DrawText(origin, string.Format("R{0:000}", rotation), OxyColors.Black, fontSize: 20d, rotation: rotation, horizontalAlignment: ha, verticalAlignment: va);
                        }
                    }
                }
            }));
            return model;
        }

        /// <summary>
        /// Shows multi-line alignment capabilities for the DrawText method.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawText - Multi-line Alignment/Rotation")]
        public static PlotModel DrawMultilineTextAlignmentRotation()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                for (var ha = HorizontalAlignment.Left; ha <= HorizontalAlignment.Right; ha++)
                {
                    for (var va = VerticalAlignment.Top; va <= VerticalAlignment.Bottom; va++)
                    {
                        var origin = new ScreenPoint(((int)ha + 2) * 170, ((int)va + 2) * 170);
                        rc.FillCircle(origin, 3, OxyColors.Blue, EdgeRenderingMode.Adaptive);
                        for (var rotation = 0; rotation < 360; rotation += 90)
                        {
                            rc.DrawText(origin, $"R{rotation:000}\n{ha}\n{va}", OxyColors.Black, fontSize: 20d, rotation: rotation, horizontalAlignment: ha, verticalAlignment: va);
                        }
                    }
                }
            }));
            return model;
        }

        /// <summary>
        /// Shows color capabilities for the DrawText method.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawText - MaxSize")]
        public static PlotModel DrawTextMaxSize()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
                {
                    const string Font = "Arial";
                    const double FontSize = 32d;
                    const double FontWeight = FontWeights.Bold;
                    const double D = FontSize * 1.6;
                    const double X = 20;
                    const double X2 = 200;
                    double y = 20;
                    var testStrings = new[] { "iii", "jjj", "OxyPlot", "Bottom", "100", "KML" };
                    foreach (var text in testStrings)
                    {
                        var maxSize = rc.MeasureText(text, Font, FontSize, FontWeight);
                        var p = new ScreenPoint(X, y);
                        rc.DrawText(p, text, OxyColors.Black, Font, FontSize, FontWeight, maxSize: maxSize);
                        var rect = new OxyRect(p, maxSize);
                        rc.DrawRectangle(rect, OxyColors.Undefined, OxyColors.Black, 1, EdgeRenderingMode.Adaptive);

                        var p2 = new ScreenPoint(X2, y);
                        var maxSize2 = new OxySize(maxSize.Width / 2, maxSize.Height / 2);
                        rc.DrawText(p2, text, OxyColors.Black, Font, FontSize, FontWeight, maxSize: maxSize2);
                        var rect2 = new OxyRect(p2, maxSize2);
                        rc.DrawRectangle(rect2, OxyColors.Undefined, OxyColors.Black, 1, EdgeRenderingMode.Adaptive);

                        y += D;
                    }
                }));
            return model;
        }

        /// <summary>
        /// Draws text and shows marks for ascent/descent/baseline/x-height and the expected bounding box.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawText - WPF metrics")]
        public static PlotModel DrawTextWithWpfMetrics()
        {
            return DrawTextWithMetrics("OxyPlot", "Arial", 60, 226, 69, 105, 73, 61, 116, 23, 228, "WPF");
        }

        /// <summary>
        /// Draws text and shows marks for ascent/descent/baseline/x-height and the expected bounding box.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawText - WinForms metrics (StringFormat = GenericDefault)")]
        public static PlotModel DrawTextWithWinFormsMetricsDefault()
        {
            return DrawTextWithMetrics("OxyPlot", "Arial", 60, 252.145812988281, 79.4999847412109, 108, 73, 61, 121, 34, 252, "WinForms");
        }

        /// <summary>
        /// Draws text and shows marks for ascent/descent/baseline/x-height and the expected bounding box.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawText - WinForms metrics (StringFormat = GenericTypographic)")]
        public static PlotModel DrawTextWithWinFormsMetricsTypographic()
        {
            return DrawTextWithMetrics("OxyPlot", "Arial", 60, 224.1, 71.5, 108, 73, 61, 121, 23, 242, "WinForms");
        }

        /// <summary>
        /// Shows capabilities for the MeasureText method.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("MeasureText")]
        public static PlotModel MeasureText()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                const string Font = "Arial";
                var strings = new[] { "OxyPlot", "MMM", "III", "jikq", "gh", "123", "!#$&" };
                var fontSizes = new[] { 10d, 20, 40, 60 };
                var x = 5d;
                foreach (double fontSize in fontSizes)
                {
                    var y = 5d;
                    var maxWidth = 0d;
                    foreach (var s in strings)
                    {
                        var size = rc.MeasureText(s, Font, fontSize);
                        maxWidth = Math.Max(maxWidth, size.Width);
                        rc.DrawRectangle(new OxyRect(x, y, size.Width, size.Height), OxyColors.LightYellow, OxyColors.Black, 1, EdgeRenderingMode.Adaptive);
                        rc.DrawText(new ScreenPoint(x, y), s, OxyColors.Black, Font, fontSize);
                        y += size.Height + 20;
                    }

                    x += maxWidth + 20;
                }
            }));
            return model;
        }

        /// <summary>
        /// Draws text with metrics.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="expectedWidth">The expected width.</param>
        /// <param name="expectedHeight">The expected height.</param>
        /// <param name="baseline">The baseline position.</param>
        /// <param name="xheight">The x-height position.</param>
        /// <param name="ascent">The ascent position.</param>
        /// <param name="descent">The descent position.</param>
        /// <param name="before">The before position.</param>
        /// <param name="after">The after position.</param>
        /// <param name="platform">The platform.</param>
        /// <returns>
        /// A plot model.
        /// </returns>
        public static PlotModel DrawTextWithMetrics(string text, string font, double fontSize, double expectedWidth, double expectedHeight, double baseline, double xheight, double ascent, double descent, double before, double after, string platform)
        {
            // http://msdn.microsoft.com/en-us/library/ms742190(v=vs.110).aspx
            // http://msdn.microsoft.com/en-us/library/xwf9s90b(v=vs.110).aspx
            // http://msdn.microsoft.com/en-us/library/windows/desktop/ms533824(v=vs.85).aspx
            // https://developer.apple.com/library/mac/documentation/TextFonts/Conceptual/CocoaTextArchitecture/FontHandling/FontHandling.html
            var model = new PlotModel();
            model.Annotations.Add(
                new DelegateAnnotation(
                    rc =>
                    {
                        var size = rc.MeasureText(text, font, fontSize);
                        var expectedSize = new OxySize(expectedWidth, expectedHeight);
                        rc.DrawText(new ScreenPoint(300, 50), "Font size: " + fontSize, OxyColors.Black, font, 12);
                        rc.DrawText(new ScreenPoint(300, 70), "Actual size: " + size.ToString("0.00", CultureInfo.InvariantCulture), OxyColors.Black, font, 12);
                        rc.DrawText(new ScreenPoint(300, 90), "Size on " + platform + ": " + expectedSize.ToString("0.00", CultureInfo.InvariantCulture), OxyColors.Green, font, 12);

                        var p = new ScreenPoint(20, 50);
                        rc.DrawText(p, text, OxyColors.Black, font, fontSize);

                        rc.FillCircle(p, 3, OxyColors.Black, EdgeRenderingMode.Adaptive);

                        // actual bounds
                        rc.DrawRectangle(new OxyRect(p, size), OxyColors.Undefined, OxyColors.Black, 1, EdgeRenderingMode.Adaptive);

                        // Expected bounds (WPF)
                        rc.DrawRectangle(new OxyRect(p, expectedSize), OxyColors.Undefined, OxyColors.Green, 1, EdgeRenderingMode.Adaptive);

                        var color = OxyColor.FromAColor(180, OxyColors.Red);
                        var pen = new OxyPen(color);

                        // Expected vertical positions (WPF)
                        var x1 = p.X - 10;
                        var x2 = p.X + expectedSize.Width + 10;
                        rc.DrawLine(x1, baseline, x2, baseline, pen, EdgeRenderingMode.Adaptive);
                        rc.DrawLine(x1, xheight, x2, xheight, pen, EdgeRenderingMode.Adaptive);
                        rc.DrawLine(x1, ascent, x2, ascent, pen, EdgeRenderingMode.Adaptive);
                        rc.DrawLine(x1, descent, x2, descent, pen, EdgeRenderingMode.Adaptive);

                        // Expected horizonal positions (WPF)
                        var y1 = p.Y - 10;
                        var y2 = p.Y + expectedSize.Height + 10;
                        rc.DrawLine(before, y1, before, y2, pen, EdgeRenderingMode.Adaptive);
                        rc.DrawLine(after, y1, after, y2, pen, EdgeRenderingMode.Adaptive);
                    }));

            model.MouseDown += (s, e) => Debug.WriteLine(e.Position);

            return model;
        }

        [Example("Clipping")]
        public static PlotModel Clipping()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                // reset a couple of times without any clipping being set to see that nothing explodes
                rc.ResetClip();
                rc.ResetClip();
                rc.ResetClip();

                var p1 = new ScreenPoint(150, 150);
                var clip = 100d;
                var radiusFactor = 1.1;
                var clipShrink = .15 * clip;

                var rect = new OxyRect(p1.X - clip, p1.Y - clip, clip * 2, clip * 2);
                rc.DrawRectangle(rect, OxyColors.Undefined, OxyColors.Red, 1, EdgeRenderingMode.Automatic);
                rc.SetClip(rect);
                rc.DrawCircle(p1, clip * radiusFactor, OxyColors.Red, OxyColors.Undefined, 0, EdgeRenderingMode.Automatic);

                clip -= clipShrink;
                rect = new OxyRect(p1.X - clip, p1.Y - clip, clip * 2, clip * 2);
                rc.DrawRectangle(rect, OxyColors.Undefined, OxyColors.Green, 1, EdgeRenderingMode.Automatic);
                rc.SetClip(rect);
                rc.DrawCircle(p1, clip * radiusFactor, OxyColors.Green, OxyColors.Undefined, 0, EdgeRenderingMode.Automatic);

                clip -= clipShrink;
                rect = new OxyRect(p1.X - clip, p1.Y - clip, clip * 2, clip * 2);
                rc.DrawRectangle(rect, OxyColors.Undefined, OxyColors.Blue, 1, EdgeRenderingMode.Automatic);
                rc.SetClip(rect);
                rc.DrawCircle(p1, clip * radiusFactor, OxyColors.Blue, OxyColors.Undefined, 0, EdgeRenderingMode.Automatic);
                rc.ResetClip();

                rc.DrawText(p1, "Clipped Circles", OxyColors.White, fontSize: 12, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: VerticalAlignment.Middle);
                
                rc.DrawText(new ScreenPoint(p1.X * 2, 50), "Not clipped", OxyColors.Black, fontSize: 40);

                rect = new OxyRect(p1.X * 2 + 10, 100, 80, 60);
                rc.DrawRectangle(rect, OxyColors.Undefined, OxyColors.Black, 1, EdgeRenderingMode.Automatic);

                // set the same clipping a couple of times to see that nothing explodes
                rc.SetClip(rect);
                rc.SetClip(rect);
                using (rc.AutoResetClip(rect))
                {
                    rc.DrawText(new ScreenPoint(p1.X * 2, 100), "Clipped", OxyColors.Black, fontSize: 40);
                }
            }));

            return model;
        }

        private const double GRID_SIZE = 40;
        private const double TILE_SIZE = 30;
        private const int THICKNESS_STEPS = 10;
        private const double THICKNESS_STEP = .5;
        private const double OFFSET_LEFT = 150;
        private const double OFFSET_TOP = 20;
        private static readonly OxyColor FILL_COLOR = OxyColors.LightBlue;

        [Example("Rectangles - EdgeRenderingMode")]
        public static PlotModel Rectangles()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                for (int i = 0; i < THICKNESS_STEPS; i++)
                {
                    var left = OFFSET_LEFT + i * GRID_SIZE + TILE_SIZE / 2;
                    var strokeThickness = i * THICKNESS_STEP;
                    rc.DrawText(new ScreenPoint(left, OFFSET_TOP / 2), strokeThickness.ToString(), OxyColors.Black, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: VerticalAlignment.Middle);
                }

                foreach (EdgeRenderingMode edgeRenderingMode in Enum.GetValues(typeof(EdgeRenderingMode)))
                {
                    var top = OFFSET_TOP + (int)edgeRenderingMode * GRID_SIZE;
                    rc.DrawText(new ScreenPoint(10, top + 10), edgeRenderingMode.ToString(), OxyColors.Black, verticalAlignment: VerticalAlignment.Middle);
                    for (int i = 0; i < THICKNESS_STEPS; i++)
                    {
                        var left = OFFSET_LEFT + i * GRID_SIZE;
                        var rect = new OxyRect(left, top, TILE_SIZE, TILE_SIZE);
                        var strokeThickness = i * THICKNESS_STEP;
                        rc.DrawRectangle(rect, FILL_COLOR, OxyColors.Black, strokeThickness, edgeRenderingMode);
                    }
                }
                
            }));
            return model;
        }

        [Example("Lines - EdgeRenderingMode")]
        public static PlotModel Lines()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                for (int i = 0; i < THICKNESS_STEPS; i++)
                {
                    var left = OFFSET_LEFT + i * GRID_SIZE + TILE_SIZE / 2;
                    var strokeThickness = i * THICKNESS_STEP;
                    rc.DrawText(new ScreenPoint(left, OFFSET_TOP / 2), strokeThickness.ToString(), OxyColors.Black, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: VerticalAlignment.Middle);
                }

                foreach (EdgeRenderingMode edgeRenderingMode in Enum.GetValues(typeof(EdgeRenderingMode)))
                {
                    var top = OFFSET_TOP + (int)edgeRenderingMode * GRID_SIZE;
                    rc.DrawText(new ScreenPoint(10, top + 10), edgeRenderingMode.ToString(), OxyColors.Black, verticalAlignment: VerticalAlignment.Middle);
                    for (int i = 0; i < THICKNESS_STEPS; i++)
                    {
                        var left = OFFSET_LEFT + i * GRID_SIZE;
                        var topLeft = new ScreenPoint(left, top);
                        var bottomLeft = new ScreenPoint(left, top + TILE_SIZE);
                        var topRight = new ScreenPoint(left + TILE_SIZE, top);
                        var bottomRight = new ScreenPoint(left + TILE_SIZE, top + TILE_SIZE);
                        var middleLeft = new ScreenPoint(left, top + TILE_SIZE / 2);
                        var strokeThickness = i * THICKNESS_STEP;

                        rc.DrawLine(new[] { bottomLeft, topLeft, topRight }, OxyColors.Black, strokeThickness, edgeRenderingMode);
                        rc.DrawLine(new[] { middleLeft, bottomRight, topLeft }, OxyColors.Black, strokeThickness, edgeRenderingMode, lineJoin: LineJoin.Bevel);
                    }
                }

            }));
            return model;
        }

        [Example("Polygons - EdgeRenderingMode")]
        public static PlotModel Polygons()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                for (int i = 0; i < THICKNESS_STEPS; i++)
                {
                    var left = OFFSET_LEFT + i * GRID_SIZE + TILE_SIZE / 2;
                    var strokeThickness = i * THICKNESS_STEP;
                    rc.DrawText(new ScreenPoint(left, OFFSET_TOP / 2), strokeThickness.ToString(), OxyColors.Black, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: VerticalAlignment.Middle);
                }

                foreach (EdgeRenderingMode edgeRenderingMode in Enum.GetValues(typeof(EdgeRenderingMode)))
                {
                    var top = OFFSET_TOP + (int)edgeRenderingMode * GRID_SIZE;
                    rc.DrawText(new ScreenPoint(10, top + 10), edgeRenderingMode.ToString(), OxyColors.Black, verticalAlignment: VerticalAlignment.Middle);
                    for (int i = 0; i < THICKNESS_STEPS; i++)
                    {
                        var left = OFFSET_LEFT + i * GRID_SIZE;
                        var points = new []
                        {
                            new ScreenPoint(left + TILE_SIZE * .4, top),
                            new ScreenPoint(left + TILE_SIZE, top + TILE_SIZE * .2),
                            new ScreenPoint(left + TILE_SIZE * .9, top + TILE_SIZE * .8),
                            new ScreenPoint(left + TILE_SIZE * .5, top + TILE_SIZE),
                            new ScreenPoint(left, top + TILE_SIZE * .6),
                        };

                        var strokeThickness = i * THICKNESS_STEP;
                        rc.DrawPolygon(points, FILL_COLOR, OxyColors.Black, strokeThickness, edgeRenderingMode);
                    }
                }

            }));
            return model;
        }

        [Example("Ellipses - EdgeRenderingMode")]
        public static PlotModel Ellipses()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                for (int i = 0; i < THICKNESS_STEPS; i++)
                {
                    var left = OFFSET_LEFT + i * GRID_SIZE + TILE_SIZE / 2;
                    var strokeThickness = i * THICKNESS_STEP;
                    rc.DrawText(new ScreenPoint(left, OFFSET_TOP / 2), strokeThickness.ToString(), OxyColors.Black, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: VerticalAlignment.Middle);
                }

                foreach (EdgeRenderingMode edgeRenderingMode in Enum.GetValues(typeof(EdgeRenderingMode)))
                {
                    var top = OFFSET_TOP + (int)edgeRenderingMode * GRID_SIZE;
                    rc.DrawText(new ScreenPoint(10, top + 10), edgeRenderingMode.ToString(), OxyColors.Black, verticalAlignment: VerticalAlignment.Middle);
                    for (int i = 0; i < THICKNESS_STEPS; i++)
                    {
                        var left = OFFSET_LEFT + i * GRID_SIZE;
                        var rect = new OxyRect(left, top + TILE_SIZE * .1, TILE_SIZE, TILE_SIZE * .8);
                        var strokeThickness = i * THICKNESS_STEP;
                        rc.DrawEllipse(rect, FILL_COLOR, OxyColors.Black, strokeThickness, edgeRenderingMode);
                    }
                }

            }));
            return model;
        }

        /// <summary>
        /// Represents an annotation that renders by a delegate.
        /// </summary>
        public class DelegateAnnotation : Annotation
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DelegateAnnotation"/> class.
            /// </summary>
            /// <param name="rendering">The rendering delegate.</param>
            public DelegateAnnotation(Action<IRenderContext> rendering)
            {
                this.Rendering = rendering;
            }

            /// <summary>
            /// Gets the rendering delegate.
            /// </summary>
            /// <value>
            /// The rendering.
            /// </value>
            public Action<IRenderContext> Rendering { get; private set; }

            /// <summary>
            /// Renders the annotation on the specified context.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Render(IRenderContext rc)
            {
                base.Render(rc);
                this.Rendering(rc);
            }
        }
    }
}
