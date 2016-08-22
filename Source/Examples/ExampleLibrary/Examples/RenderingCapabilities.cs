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

                rc.FillRectangle(new OxyRect(X, y + D + 15, 200, 10), OxyColors.Black);
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
                rc.FillCircle(origin, 3, OxyColors.Blue);
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
                        rc.FillCircle(origin, 3, OxyColors.Blue);
                        rc.DrawText(origin, ha + "-" + va, OxyColors.Black, fontSize: FontSize, horizontalAlignment: ha, verticalAlignment: va);
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
                rc.FillCircle(origin, 3, OxyColors.Blue);
                for (int rotation = 0; rotation < 360; rotation += 45)
                {
                    var text = "     A_{2}^{3}B";
                    rc.DrawMathText(origin, text, OxyColors.Black, fontFamily, fontSize, fontWeight, rotation, HorizontalAlignment.Left, VerticalAlignment.Middle);
                    var size = rc.MeasureMathText(text, fontFamily, fontSize, fontWeight);
                    var outline1 = size.GetPolygon(origin, rotation, HorizontalAlignment.Left, VerticalAlignment.Middle).ToArray();
                    rc.DrawPolygon(outline1, OxyColors.Undefined, OxyColors.Blue);

                    // Compare with normal text
                    var text2 = "     A B";
                    rc.DrawText(origin2, text2, OxyColors.Red, fontFamily, fontSize, fontWeight, rotation, HorizontalAlignment.Left, VerticalAlignment.Middle);
                    var size2 = rc.MeasureText(text2, fontFamily, fontSize, fontWeight);
                    var outline2 = size2.GetPolygon(origin2, rotation, HorizontalAlignment.Left, VerticalAlignment.Middle).ToArray();
                    rc.DrawPolygon(outline2, OxyColors.Undefined, OxyColors.Blue);
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
                        rc.FillCircle(origin, 3, OxyColors.Blue);
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
                        rc.FillCircle(origin, 3, OxyColors.Blue);
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
                        rc.DrawRectangle(rect, OxyColors.Undefined, OxyColors.Black);

                        var p2 = new ScreenPoint(X2, y);
                        var maxSize2 = new OxySize(maxSize.Width / 2, maxSize.Height / 2);
                        rc.DrawText(p2, text, OxyColors.Black, Font, FontSize, FontWeight, maxSize: maxSize2);
                        var rect2 = new OxyRect(p2, maxSize2);
                        rc.DrawRectangle(rect2, OxyColors.Undefined, OxyColors.Black);

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
                        rc.DrawRectangle(new OxyRect(x, y, size.Width, size.Height), OxyColors.LightYellow, OxyColors.Black);
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
        private static PlotModel DrawTextWithMetrics(string text, string font, double fontSize, double expectedWidth, double expectedHeight, double baseline, double xheight, double ascent, double descent, double before, double after, string platform)
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

                        rc.FillCircle(p, 3, OxyColors.Black);

                        // actual bounds
                        rc.DrawRectangle(new OxyRect(p, size), OxyColors.Undefined, OxyColors.Black);

                        // Expected bounds (WPF)
                        rc.DrawRectangle(new OxyRect(p, expectedSize), OxyColors.Undefined, OxyColors.Green);

                        var color = OxyColor.FromAColor(180, OxyColors.Red);
                        var pen = new OxyPen(color);

                        // Expected vertical positions (WPF)
                        var x1 = p.X - 10;
                        var x2 = p.X + expectedSize.Width + 10;
                        rc.DrawLine(x1, baseline, x2, baseline, pen);
                        rc.DrawLine(x1, xheight, x2, xheight, pen);
                        rc.DrawLine(x1, ascent, x2, ascent, pen);
                        rc.DrawLine(x1, descent, x2, descent, pen);

                        // Expected horizonal positions (WPF)
                        var y1 = p.Y - 10;
                        var y2 = p.Y + expectedSize.Height + 10;
                        rc.DrawLine(before, y1, before, y2, pen);
                        rc.DrawLine(after, y1, after, y2, pen);
                    }));

            model.MouseDown += (s, e) => Debug.WriteLine(e.Position);

            return model;
        }

        /// <summary>
        /// Represents an annotation that renders by a delegate.
        /// </summary>
        private class DelegateAnnotation : Annotation
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