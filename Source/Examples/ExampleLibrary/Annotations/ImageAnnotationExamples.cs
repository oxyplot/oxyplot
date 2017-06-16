// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageAnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Reflection;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("ImageAnnotation"), Tags("Annotations")]
    public static class ImageAnnotationExamples
    {
        [Example("ImageAnnotation")]
        public static PlotModel ImageAnnotation()
        {
            var model = new PlotModel { Title = "ImageAnnotation", PlotMargins = new OxyThickness(60, 4, 4, 60) };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            OxyImage image;
            var assembly = typeof(ImageAnnotationExamples).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("ExampleLibrary.Resources.OxyPlot.png"))
            {
                image = new OxyImage(stream);
            }

            // Centered in plot area, filling width
            model.Annotations.Add(new ImageAnnotation
                                      {
                                          ImageSource = image,
                                          Opacity = 0.2,
                                          Interpolate = false,
                                          X = new PlotLength(0.5, PlotLengthUnit.RelativeToPlotArea),
                                          Y = new PlotLength(0.5, PlotLengthUnit.RelativeToPlotArea),
                                          Width = new PlotLength(1, PlotLengthUnit.RelativeToPlotArea),
                                          HorizontalAlignment = HorizontalAlignment.Center,
                                          VerticalAlignment = VerticalAlignment.Middle
                                      });

            // Relative to plot area, inside top/right corner, 120pt wide
            model.Annotations.Add(new ImageAnnotation
                                      {
                                          ImageSource = image,
                                          X = new PlotLength(1, PlotLengthUnit.RelativeToPlotArea),
                                          Y = new PlotLength(0, PlotLengthUnit.RelativeToPlotArea),
                                          Width = new PlotLength(120, PlotLengthUnit.ScreenUnits),
                                          HorizontalAlignment = HorizontalAlignment.Right,
                                          VerticalAlignment = VerticalAlignment.Top
                                      });

            // Relative to plot area, above top/left corner, 20pt high
            model.Annotations.Add(new ImageAnnotation
                                      {
                                          ImageSource = image,
                                          X = new PlotLength(0, PlotLengthUnit.RelativeToPlotArea),
                                          Y = new PlotLength(0, PlotLengthUnit.RelativeToPlotArea),
                                          OffsetY = new PlotLength(-5, PlotLengthUnit.ScreenUnits),
                                          Height = new PlotLength(20, PlotLengthUnit.ScreenUnits),
                                          HorizontalAlignment = HorizontalAlignment.Left,
                                          VerticalAlignment = VerticalAlignment.Bottom
                                      });

            // At the point (50,50), 200pt wide
            model.Annotations.Add(new ImageAnnotation
                                      {
                                          ImageSource = image,
                                          X = new PlotLength(50, PlotLengthUnit.Data),
                                          Y = new PlotLength(50, PlotLengthUnit.Data),
                                          Width = new PlotLength(200, PlotLengthUnit.ScreenUnits),
                                          HorizontalAlignment = HorizontalAlignment.Left,
                                          VerticalAlignment = VerticalAlignment.Top
                                      });

            // At the point (50,20), 50 x units wide
            model.Annotations.Add(new ImageAnnotation
                                      {
                                          ImageSource = image,
                                          X = new PlotLength(50, PlotLengthUnit.Data),
                                          Y = new PlotLength(20, PlotLengthUnit.Data),
                                          Width = new PlotLength(50, PlotLengthUnit.Data),
                                          HorizontalAlignment = HorizontalAlignment.Center,
                                          VerticalAlignment = VerticalAlignment.Top
                                      });

            // Relative to the viewport, centered at the bottom, with offset (could also use bottom vertical alignment)
            model.Annotations.Add(new ImageAnnotation
                                      {
                                          ImageSource = image,
                                          X = new PlotLength(0.5, PlotLengthUnit.RelativeToViewport),
                                          Y = new PlotLength(1, PlotLengthUnit.RelativeToViewport),
                                          OffsetY = new PlotLength(-35, PlotLengthUnit.ScreenUnits),
                                          Height = new PlotLength(30, PlotLengthUnit.ScreenUnits),
                                          HorizontalAlignment = HorizontalAlignment.Center,
                                          VerticalAlignment = VerticalAlignment.Top
                                      });

            // Changing opacity
            for (int y = 0; y < 10; y++)
            {
                model.Annotations.Add(
                    new ImageAnnotation
                        {
                            ImageSource = image,
                            Opacity = (y + 1) / 10.0,
                            X = new PlotLength(10, PlotLengthUnit.Data),
                            Y = new PlotLength(y * 2, PlotLengthUnit.Data),
                            Width = new PlotLength(100, PlotLengthUnit.ScreenUnits),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Bottom
                        });
            }

            return model;
        }

        [Example("ImageAnnotation - gradient backgrounds")]
        public static PlotModel ImageAnnotationAsBackgroundGradient()
        {
            // http://en.wikipedia.org/wiki/Chartjunk
            var model = new PlotModel { Title = "Using ImageAnnotations to draw a gradient backgrounds", Subtitle = "But do you really want this? This is called 'chartjunk'!", PlotMargins = new OxyThickness(60, 4, 4, 60) };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            // create a gradient image of height n
            int n = 256;
            var imageData1 = new OxyColor[1, n];
            for (int i = 0; i < n; i++)
            {
                imageData1[0, i] = OxyColor.Interpolate(OxyColors.Blue, OxyColors.Red, i / (n - 1.0));
            }

            var image1 = OxyImage.Create(imageData1, ImageFormat.Png); // png is required for silverlight

            // or create a gradient image of height 2 (requires bitmap interpolation to be supported)
            var imageData2 = new OxyColor[1, 2];
            imageData2[0, 0] = OxyColors.Yellow; // top color
            imageData2[0, 1] = OxyColors.Gray; // bottom color

            var image2 = OxyImage.Create(imageData2, ImageFormat.Png); // png is required for silverlight

            // gradient filling the viewport
            model.Annotations.Add(new ImageAnnotation
                                      {
                                          ImageSource = image2,
                                          Interpolate = true,
                                          Layer = AnnotationLayer.BelowAxes,
                                          X = new PlotLength(0, PlotLengthUnit.RelativeToViewport),
                                          Y = new PlotLength(0, PlotLengthUnit.RelativeToViewport),
                                          Width = new PlotLength(1, PlotLengthUnit.RelativeToViewport),
                                          Height = new PlotLength(1, PlotLengthUnit.RelativeToViewport),
                                          HorizontalAlignment = HorizontalAlignment.Left,
                                          VerticalAlignment = VerticalAlignment.Top
                                      });

            // gradient filling the plot area
            model.Annotations.Add(new ImageAnnotation
                                      {
                                          ImageSource = image1,
                                          Interpolate = true,
                                          Layer = AnnotationLayer.BelowAxes,
                                          X = new PlotLength(0, PlotLengthUnit.RelativeToPlotArea),
                                          Y = new PlotLength(0, PlotLengthUnit.RelativeToPlotArea),
                                          Width = new PlotLength(1, PlotLengthUnit.RelativeToPlotArea),
                                          Height = new PlotLength(1, PlotLengthUnit.RelativeToPlotArea),
                                          HorizontalAlignment = HorizontalAlignment.Left,
                                          VerticalAlignment = VerticalAlignment.Top
                                      });

            // verify that a series is rendered above the gradients
            model.Series.Add(new FunctionSeries(Math.Sin, 0, 7, 0.01));

            return model;
        }

        [Example("ImageAnnotation - normal axes")]
        public static PlotModel ImageAnnotation_NormalAxes()
        {
            var model = new PlotModel { Title = "ImageAnnotation - normal axes" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            // create an image
            var pixels = new OxyColor[2, 2];
            pixels[0, 0] = OxyColors.Blue;
            pixels[1, 0] = OxyColors.Yellow;
            pixels[0, 1] = OxyColors.Green;
            pixels[1, 1] = OxyColors.Red;

            var image = OxyImage.Create(pixels, ImageFormat.Png);

            model.Annotations.Add(
                new ImageAnnotation
                    {
                        ImageSource = image,
                        Interpolate = false,
                        X = new PlotLength(0, PlotLengthUnit.Data),
                        Y = new PlotLength(0, PlotLengthUnit.Data),
                        Width = new PlotLength(80, PlotLengthUnit.Data),
                        Height = new PlotLength(50, PlotLengthUnit.Data),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Bottom
                    });
            return model;
        }

        [Example("ImageAnnotation - reverse horizontal axis")]
        public static PlotModel ImageAnnotation_ReverseHorizontalAxis()
        {
            var model = new PlotModel { Title = "ImageAnnotation - reverse horizontal axis" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, StartPosition = 1, EndPosition = 0 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            // create an image
            var pixels = new OxyColor[2, 2];
            pixels[0, 0] = OxyColors.Blue;
            pixels[1, 0] = OxyColors.Yellow;
            pixels[0, 1] = OxyColors.Green;
            pixels[1, 1] = OxyColors.Red;

            var image = OxyImage.Create(pixels, ImageFormat.Png);

            model.Annotations.Add(
                new ImageAnnotation
                    {
                        ImageSource = image,
                        Interpolate = false,
                        X = new PlotLength(100, PlotLengthUnit.Data),
                        Y = new PlotLength(0, PlotLengthUnit.Data),
                        Width = new PlotLength(80, PlotLengthUnit.Data),
                        Height = new PlotLength(50, PlotLengthUnit.Data),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Bottom
                    });
            return model;
        }

        [Example("ImageAnnotation - reverse vertical axis")]
        public static PlotModel ImageAnnotation_ReverseVerticalAxis()
        {
            var model = new PlotModel { Title = "ImageAnnotation - reverse vertical axis" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StartPosition = 1, EndPosition = 0 });

            // create an image
            var pixels = new OxyColor[2, 2];
            pixels[0, 0] = OxyColors.Blue;
            pixels[1, 0] = OxyColors.Yellow;
            pixels[0, 1] = OxyColors.Green;
            pixels[1, 1] = OxyColors.Red;

            var image = OxyImage.Create(pixels, ImageFormat.Png);

            model.Annotations.Add(
                new ImageAnnotation
                    {
                        ImageSource = image,
                        Interpolate = false,
                        X = new PlotLength(0, PlotLengthUnit.Data),
                        Y = new PlotLength(100, PlotLengthUnit.Data),
                        Width = new PlotLength(80, PlotLengthUnit.Data),
                        Height = new PlotLength(50, PlotLengthUnit.Data),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Bottom
                    });
            return model;
        }
    }
}