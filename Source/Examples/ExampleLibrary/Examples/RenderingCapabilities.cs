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
    using System.Collections.Generic;

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
                void DrawClipRect(OxyRect clipRect)
                {
                    var pen = new OxyPen(OxyColors.Black, 2, LineStyle.Dash);
                    rc.DrawLine(clipRect.Left, clipRect.Top, clipRect.Right, clipRect.Top, pen, EdgeRenderingMode.Automatic);
                    rc.DrawLine(clipRect.Right, clipRect.Top, clipRect.Right, clipRect.Bottom, pen, EdgeRenderingMode.Automatic);
                    rc.DrawLine(clipRect.Right, clipRect.Bottom, clipRect.Left, clipRect.Bottom, pen, EdgeRenderingMode.Automatic);
                    rc.DrawLine(clipRect.Left, clipRect.Bottom, clipRect.Left, clipRect.Top, pen, EdgeRenderingMode.Automatic);
                }

                var currentLine = 20d;
                const double lineHeight = 60;
                const double clipRectSize = 40;
                const double clipRectMargin = 20;
                const double testCaseMargin = 20;
                const double descriptionMargin = 200;
                var rect = new OxyRect();

                void DrawCircle(ScreenPoint center)
                {
                    rc.DrawCircle(center, clipRectSize * 0.58, OxyColors.CornflowerBlue, OxyColors.Undefined, 0, EdgeRenderingMode.Automatic);
                }

                void DrawDescription(string text)
                {
                    var p = new ScreenPoint(clipRectMargin + clipRectSize + testCaseMargin + descriptionMargin, currentLine);
                    rc.DrawText(p, text, OxyColors.Black, fontSize: 12, verticalAlignment: VerticalAlignment.Middle);
                }

                void DrawTestCase(string text)
                {
                    var p = new ScreenPoint(clipRectMargin + clipRectSize + testCaseMargin, currentLine);
                    rc.DrawText(p, text, OxyColors.Black, fontSize: 12, verticalAlignment: VerticalAlignment.Middle);
                }

                void DrawHeader(string text, double offset)
                {
                    rc.DrawText(new ScreenPoint(offset, 15), text, OxyColors.Black, fontSize: 12, fontWeight: 700);
                }

                void NextLine()
                {
                    currentLine += lineHeight;
                    rect = new OxyRect(clipRectMargin, currentLine - clipRectSize / 2, clipRectSize, clipRectSize);
                }

                DrawHeader("Actual", clipRectMargin);
                DrawHeader("Test Case", clipRectMargin + clipRectSize + testCaseMargin);
                DrawHeader("Expected", clipRectMargin + clipRectSize + testCaseMargin + descriptionMargin);

                //-------------
                NextLine();
                rc.PushClip(rect);
                rc.PopClip();
                DrawCircle(rect.Center);

                DrawTestCase("1. Push clipping rectangle\n2. Pop clipping rectangle\n3. Draw circle");
                DrawDescription("The circle should be fully drawn.");

                //-------------
                NextLine();
                rc.PushClip(rect);
                DrawCircle(rect.Center);

                rc.PopClip();

                DrawClipRect(rect);
                DrawTestCase("1. Push clipping rectangle\n2. Draw Circle");
                DrawDescription("The circle should be clipped.");

                //-------------
                NextLine();
                var rect2 = rect.Deflate(new OxyThickness(rect.Height * 0.25));
                rc.PushClip(rect);
                rc.PushClip(rect2);

                DrawCircle(rect.Center);

                rc.PopClip();
                rc.PopClip();

                DrawClipRect(rect);
                DrawClipRect(rect2);
                DrawTestCase("1. Push large clipping rectangle\n2. Push small clipping rectangle\n3. Draw Circle");
                DrawDescription("The circle should be clipped to the small clipping rectangle.");

                //-------------
                NextLine();
                rect2 = rect.Deflate(new OxyThickness(rect.Height * 0.25));
                rc.PushClip(rect2);
                rc.PushClip(rect);

                DrawCircle(rect.Center);

                rc.PopClip();
                rc.PopClip();

                DrawClipRect(rect);
                DrawClipRect(rect2);
                DrawTestCase("1. Push small clipping rectangle\n2. Push large clipping rectangle\n3. Draw Circle");
                DrawDescription("The circle should be clipped to the small clipping rectangle.");

                //-------------
                NextLine();
                rect2 = rect.Offset(rect.Width / 2, rect.Height / 2).Deflate(new OxyThickness(rect.Height * 0.25));
                rc.PushClip(rect);
                rc.PushClip(rect2);

                DrawCircle(rect.Center);

                rc.PopClip();
                rc.PopClip();

                DrawClipRect(rect);
                DrawClipRect(rect2);
                DrawTestCase("1. Push large clipping rectangle\n2. Push small clipping rectangle\n3. Draw Circle");
                DrawDescription("The circle should be clipped to the intersection of the clipping rectangles.");

                //-------------
                NextLine();
                rect2 = rect.Offset(rect.Width / 2, rect.Height / 2).Deflate(new OxyThickness(rect.Height * 0.25));
                rc.PushClip(rect);
                rc.PushClip(rect2);

                rc.PopClip();

                DrawCircle(rect.Center);

                rc.PopClip();

                DrawClipRect(rect);
                DrawClipRect(rect2);
                DrawTestCase("1. Push large clipping rectangle\n2. Push small clipping rectangle\n3. Pop small clipping rectangle\n4. Draw Circle");
                DrawDescription("The circle should be clipped to the large clipping rectangle.");

                //-------------
                NextLine();
                var rect3 = rect.Offset(rect.Width / 3, rect.Height / 3).Deflate(new OxyThickness(rect.Height * 0.25));
                var rect4 = rect.Offset(-rect.Width / 3, -rect.Height / 3).Deflate(new OxyThickness(rect.Height * 0.25));
                rc.PushClip(rect3);
                rc.PushClip(rect4);

                DrawCircle(rect.Center);

                rc.PopClip();
                rc.PopClip();

                DrawClipRect(rect3);
                DrawClipRect(rect4);
                DrawTestCase("1. Push clipping rectangle\n2. Push second clipping rectangle\n3. Draw Circle");
                DrawDescription("The circle should not be drawn at all.");

                //-------------
                NextLine();
                using (rc.AutoResetClip(rect))
                {
                    rc.DrawText(rect.Center, "OxyPlot", OxyColors.CornflowerBlue, fontSize: 15, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: VerticalAlignment.Middle);
                }

                DrawClipRect(rect);
                DrawTestCase("1. Push clipping rectangle\n2. Draw Text");
                DrawDescription("The text should be clipped.");
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

        [Example("LineJoin")]
        public static PlotModel LineJoins()
        {
            const double STROKE_THICKNESS = 15;
            const double LINE_LENGTH = 60;
            var ANGLES = new[] { 135, 90, 45, 22.5 };
            const double COL_WIDTH = 140;
            const double ROW_HEIGHT = 90;
            const double ROW_HEADER_WIDTH = 50;
            const double COL_HEADER_HEIGHT = 50;


            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                var colCounter = 0;
                var rowCounter = 0;
                foreach (LineJoin lineJoin in Enum.GetValues(typeof(LineJoin)))
                {
                    var p = new ScreenPoint(COL_WIDTH * (colCounter + 0.5) + ROW_HEADER_WIDTH, COL_HEADER_HEIGHT / 2);
                    rc.DrawText(p, lineJoin.ToString(), OxyColors.Black, fontSize: 12, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: VerticalAlignment.Middle);
                    colCounter++;
                }

                foreach (var angle in ANGLES)
                {
                    colCounter = 0;
                    var y = ROW_HEIGHT * rowCounter + COL_HEADER_HEIGHT;
                    var halfAngle = angle / 2 / 360 * 2 * Math.PI;
                    var dx = Math.Sin(halfAngle) * LINE_LENGTH;
                    var dy = Math.Cos(halfAngle) * LINE_LENGTH;

                    var textP = new ScreenPoint(15, y);
                    rc.DrawText(textP, angle.ToString() + "°", OxyColors.Black, fontSize: 12);

                    foreach (LineJoin lineJoin in Enum.GetValues(typeof(LineJoin)))
                    {
                        var x = COL_WIDTH * (colCounter + 0.5) + ROW_HEADER_WIDTH;

                        var pMid = new ScreenPoint(x, y);
                        var p1 = new ScreenPoint(x - dx, y + dy);
                        var p2 = new ScreenPoint(x + dx, y + dy);

                        rc.DrawLine(new[] { p1, pMid, p2 }, OxyColors.CornflowerBlue, STROKE_THICKNESS, EdgeRenderingMode.PreferGeometricAccuracy, lineJoin: lineJoin);

                        colCounter++;
                    }

                    rowCounter++;
                }

            }));
            return model;
        }

        [Example("Ellipse Drawing")]
        public static PlotModel EllipseDrawing()
        {
            const double RADIUS_X = 300;
            const double RADIUS_Y = 100;
            const double CENTER_X = RADIUS_X * 1.2;
            const double CENTER_Y = RADIUS_Y * 1.2;

            var radiusXSquare = RADIUS_X * RADIUS_X;
            var radiusYSquare = RADIUS_Y * RADIUS_Y;
            var n = 200;

            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                var rect = new OxyRect(CENTER_X - RADIUS_X, CENTER_Y - RADIUS_Y, RADIUS_X * 2, RADIUS_Y * 2);

                var points = new ScreenPoint[n];
                var cx = (rect.Left + rect.Right) / 2;
                var cy = (rect.Top + rect.Bottom) / 2;
                var rx = (rect.Right - rect.Left) / 2;
                var ry = (rect.Bottom - rect.Top) / 2;
                for (var i = 0; i < n; i++)
                {
                    var a = Math.PI * 2 * i / (n - 1);
                    points[i] = new ScreenPoint(cx + (rx * Math.Cos(a)), cy + (ry * Math.Sin(a)));
                }

                rc.DrawPolygon(points, OxyColors.Undefined, OxyColors.Black, 4, EdgeRenderingMode.PreferGeometricAccuracy);
                rc.DrawEllipse(rect, OxyColors.Undefined, OxyColors.White, 2, EdgeRenderingMode.PreferGeometricAccuracy);
                rc.DrawText(new ScreenPoint(CENTER_X, CENTER_Y), "The white ellipse (drawn by Renderer) should match the black ellipse (drawn as Path).", OxyColors.Black, fontSize: 12, horizontalAlignment: HorizontalAlignment.Center, verticalAlignment: VerticalAlignment.Middle);
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

            /// <inheritdoc/>
            public override OxyRect GetClippingRect()
            {
                return OxyRect.Everything;
            }
        }
    }
}
