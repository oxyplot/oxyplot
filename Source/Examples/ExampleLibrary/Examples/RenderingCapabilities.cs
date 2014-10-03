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

    using OxyPlot;
    using OxyPlot.Annotations;

    /// <summary>
    /// Provides rendering capability examples.
    /// </summary>
    [Examples("Rendering capabilities")]
    public class RenderingCapabilities
    {
        /// <summary>
        /// Shows capabilities for the DrawText method.
        /// </summary>
        /// <returns>A plot model.</returns>
        [Example("DrawText")]
        public static PlotModel DrawText()
        {
            var model = new PlotModel();
            model.Annotations.Add(new DelegateAnnotation(rc =>
            {
                double x = 5;
                double y = 5;

                var fontSize = 20d;
                var dy = fontSize * 1.6;

                // Colors
                rc.DrawText(new ScreenPoint(x, y += dy), "Black", OxyColors.Black, "Arial", fontSize, FontWeights.Bold);
                rc.DrawText(new ScreenPoint(x, y += dy), "Red", OxyColors.Red, "Arial", fontSize, FontWeights.Bold);
                y += dy;

                // Fonts
                rc.DrawText(new ScreenPoint(x, y += dy), "Default font", OxyColors.Black, null, fontSize);
                rc.DrawText(new ScreenPoint(x, y += dy), "Helvetica", OxyColors.Black, "Helvetica", fontSize);
                rc.DrawText(new ScreenPoint(x, y += dy), "Arial", OxyColors.Black, "Arial", fontSize);
                rc.DrawText(new ScreenPoint(x, y += dy), "Times", OxyColors.Black, "Times", fontSize);
                rc.DrawText(new ScreenPoint(x, y += dy), "Times New Roman", OxyColors.Black, "Times New Roman", fontSize);
                y += dy * 2;

                // Font sizes
                foreach (var size in new[] { 10, 16, 24, 36 })
                {
                    rc.DrawText(new ScreenPoint(x, y), size + "pt", OxyColors.Black, "Arial", size);
                    rc.DrawText(new ScreenPoint(x + 100, y), size + "pt", OxyColors.Black, "Arial", size, FontWeights.Bold);
                    y += size * 1.6;
                }

                // Rotation
                x = 400;
                y = 400;
                rc.DrawText(new ScreenPoint(x, y), "Rotation 0", OxyColors.Black, fontSize: fontSize, rotation: 0);
                rc.DrawText(new ScreenPoint(x, y), "Rotation 30", OxyColors.Black, fontSize: fontSize, rotation: 30);
                rc.DrawText(new ScreenPoint(x, y), "Rotation 90", OxyColors.Black, fontSize: fontSize, rotation: 90);
                rc.DrawText(new ScreenPoint(x, y), "Rotation 180", OxyColors.Black, fontSize: fontSize, rotation: 180);
                rc.DrawText(new ScreenPoint(x, y), "Rotation 270", OxyColors.Black, fontSize: fontSize, rotation: 270);

                // Alignment
                var clippingRect = new OxyRect(0, 0, 1000, 1000);
                for (var ha = HorizontalAlignment.Left; ha <= HorizontalAlignment.Right; ha++)
                {
                    for (var va = VerticalAlignment.Top; va <= VerticalAlignment.Bottom; va++)
                    {
                        x = 400 + ((int)ha * 200);
                        y = 100 + ((int)va * dy * 2);
                        rc.DrawMarker(clippingRect, new ScreenPoint(x, y), MarkerType.Plus, null, 4, OxyColors.Undefined, OxyColors.Blue, 1);
                        rc.DrawText(new ScreenPoint(x, y), ha + "-" + va, OxyColors.Black, fontSize: fontSize, horizontalAlignment: ha, verticalAlignment: va);
                    }
                }
            }));
            return model;
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
                var font = "Arial";
                var strings = new[] { "OxyPlot", "MMM", "III", "jikq", "gh", "123", "!#$&" };
                var fontSizes = new[] { 10d, 20, 40, 60 };
                var x = 5d;
                foreach (double fontSize in fontSizes)
                {
                    var y = 5d;
                    var maxWidth = 0d;
                    foreach (var s in strings)
                    {
                        var size = rc.MeasureText(s, font, fontSize);
                        maxWidth = Math.Max(maxWidth, size.Width);
                        rc.DrawRectangle(new OxyRect(x, y, size.Width, size.Height), OxyColors.LightYellow, OxyColors.Black);
                        rc.DrawText(new ScreenPoint(x, y), s, OxyColors.Black, font, fontSize);
                        y += size.Height + 20;
                    }

                    x += maxWidth + 20;
                }
            }));
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
            /// Gets or sets the rendering delegate.
            /// </summary>
            /// <value>
            /// The rendering.
            /// </value>
            public Action<IRenderContext> Rendering { get; private set; }

            /// <summary>
            /// Renders the annotation on the specified context.
            /// </summary>
            /// <param name="rc">The render context.</param>
            /// <param name="model">The model.</param>
            public override void Render(IRenderContext rc, PlotModel model)
            {
                base.Render(rc, model);
                this.Rendering(rc);
            }
        }
    }
}