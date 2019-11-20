// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LegendExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    [Examples("Legends")]
    public static class LegendExamples
    {
        [Example("Legend at right top inside")]
        public static PlotModel LegendRightTopInside()
        {
            var model = CreateModel();
            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Inside,
                LegendPosition = LegendPosition.RightTop
            };

            model.Legends.Add(l);

            return model;
        }

        [Example("Legend at right top outside")]
        public static PlotModel LegendRightTopOutside()
        {
            var model = CreateModel();
            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.RightTop
            };

            model.Legends.Add(l);
            return model;
        }

        [Example("Legend at BottomLeft outside horizontal")]
        public static PlotModel LegendBottomLeftHorizontal()
        {
            var model = CreateModel(4);

            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomLeft,
                LegendOrientation = LegendOrientation.Horizontal
            };

            model.Legends.Add(l);
            return model;
        }

        [Example("Legend at TopLeft outside vertical")]
        public static PlotModel LegendTopLeftVertical()
        {
            var model = CreateModel(4);
            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopLeft,
                LegendOrientation = LegendOrientation.Vertical
            };

            model.Legends.Add(l);

            return model;
        }

        [Example("Legend at default position")]
        public static PlotModel LegendDefault()
        {
            var model = CreateModel();
            var l = new Legend();

            model.Legends.Add(l);

            return model;
        }

        [Example("LegendItemSpacing (only for horizontal orientation)")]
        public static PlotModel LegendItemSpacing()
        {
            var model = CreateModel();
            var l = new Legend
            {
                LegendItemSpacing = 100,
                LegendPosition = LegendPosition.BottomLeft,
                LegendOrientation = LegendOrientation.Horizontal
            };

            model.Legends.Add(l);

            return model;
        }

        [Example("LegendLineSpacing (vertical legend orientation)")]
        public static PlotModel LegendLineSpacingVertical()
        {
            var model = CreateModel();
            var l = new Legend
            {
                LegendLineSpacing = 30,
                LegendPosition = LegendPosition.TopLeft,
                LegendOrientation = LegendOrientation.Vertical
            };

            model.Legends.Add(l);
            return model;
        }

        [Example("LegendLineSpacing (horizontal legend orientation)")]
        public static PlotModel LegendLineSpacingHorizontal()
        {
            var model = CreateModel();
            var l = new Legend
            {
                LegendLineSpacing = 30,
                LegendPosition = LegendPosition.TopLeft,
                LegendOrientation = LegendOrientation.Horizontal
            };

            model.Legends.Add(l);
            return model;
        }

        [Example("LegendColumnSpacing (only for vertical orientation)")]
        public static PlotModel LegendColumnSpacing()
        {
            var model = CreateModel(60);
            var l = new Legend
            {
                LegendColumnSpacing = 100,
                LegendPosition = LegendPosition.TopRight,
                LegendOrientation = LegendOrientation.Vertical
            };

            model.Legends.Add(l);
            return model;
        }

        [Example("Hidden Legend")]
        public static PlotModel LegendHidden()
        {
            var model = CreateModel();
            model.IsLegendVisible = false;
            return model;
        }

        [Example("Grayscale colors")]
        public static PlotModel LegendGrayscale()
        {
            var model = CreateModel();
            model.DefaultColors = new List<OxyColor> { OxyColors.Black, OxyColors.Gray };
            var l = new Legend
            {
                LegendSymbolLength = 32
            };

            model.Legends.Add(l);
            return model;
        }

        [Example("Clipped legends")]
        public static PlotModel ClippedLegends()
        {
            var model = CreateModel(1);
            model.Series[0].Title = "1234567890 abcdefghijklmnopqrstuvwxyzæøå ABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ 1234567890 abcdefghijklmnopqrstuvwxyzæøå ABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ";
            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Inside,
                LegendPosition = LegendPosition.RightTop
            };

            model.Legends.Add(l);
            return model;
        }

        [Example("Clipped legends RightTop outside with MaxWidth")]
        public static PlotModel ClippedLegendsOutside()
        {
            var model = ClippedLegends();
            model.Legends[0].LegendPlacement = LegendPlacement.Outside;
            model.Legends[0].LegendPosition = LegendPosition.RightTop;
            model.Legends[0].LegendMaxWidth = 200;

            return model;
        }

        [Example("Clipped legends TopRight outside")]
        public static PlotModel ClippedLegendsRight()
        {
            var model = ClippedLegends();
            model.Legends[0].LegendPlacement = LegendPlacement.Outside;
            model.Legends[0].LegendPosition = LegendPosition.TopRight;

            return model;
        }

        [Example("LegendMaxHeight (vertical legend orientation)")]
        public static PlotModel LegendBottomCenterOutsideWithMaxHeight()
        {
            var model = CreateModel();
            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Vertical,
                LegendMaxHeight = 75.0
            };

            model.Legends.Add(l);
            return model;
        }

        [Example("Legend with DefaultFontSize")]
        public static PlotModel LegendDefaultFontSize()
        {
            var model = CreateModel();
            var l = new Legend
            {
                LegendFontSize = double.NaN,
                LegendTitle = "Title in DefaultFontSize"
            };

            model.Legends.Add(l);
            model.DefaultFontSize = 20;
            return model;
        }

        private static PlotModel CreateModel(int n = 20)
        {
            var model = new PlotModel { Title = "LineSeries" };
            //var l = new Legend
            //{
            //    LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
            //    LegendBorder = OxyColors.Black
            //};

            //model.Legends.Add(l);
            for (int i = 1; i <= n; i++)
            {
                var s = new LineSeries { Title = "Series " + i };
                model.Series.Add(s);
                for (double x = 0; x < 2 * Math.PI; x += 0.1)
                {
                    s.Points.Add(new DataPoint(x, (Math.Sin(x * i) / i) + i));
                }
            }

            return model;
        }
    }
}
