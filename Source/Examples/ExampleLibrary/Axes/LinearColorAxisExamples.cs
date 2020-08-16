// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearColorAxisExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;

    [Examples("LinearColorAxis"), Tags("Axes")]
    public class LinearColorAxisExamples
    {
        [Example("Default palette")]
        public static PlotModel DefaultPalette()
        {
            var model = HeatMapSeriesExamples.CreatePeaks(null, false);
            model.Axes.Clear();
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right });
            return model;
        }

        [Example("Jet (200 colors) palette")]
        public static PlotModel Jet200()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(200), false);
        }

        [Example("Jet (20 colors) palette")]
        public static PlotModel Jet20()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(20), false);
        }

        [Example("Hue (400 colors) palette")]
        public static PlotModel Hue400()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Hue(400), false);
        }

        [Example("Hue distinct (200 colors) palette")]
        public static PlotModel HueDistinct200()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.HueDistinct(200), false);
        }

        [Example("Hue distinct reversed (200 colors) palette")]
        public static PlotModel HueDistinctReverse200()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.HueDistinct(200).Reverse(), false);
        }

        [Example("Hot (200 colors) palette")]
        public static PlotModel Hot200()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Hot(200), false);
        }

        [Example("Hot (64 colors) palette")]
        public static PlotModel Hot64()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Hot64, false);
        }

        [Example("Hot (30 colors) palette")]
        public static PlotModel Hot30()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Hot(30), false);
        }

        [Example("Blue-white-red (200 colors) palette")]
        public static PlotModel BlueWhiteRed200()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.BlueWhiteRed(200), false);
        }

        [Example("Blue-white-red (40 colors) palette")]
        public static PlotModel BlueWhiteRed40()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.BlueWhiteRed(40), false);
        }

        [Example("Black-white-red (500 colors) palette")]
        public static PlotModel BlackWhiteRed500()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.BlackWhiteRed(500), false);
        }

        [Example("Black-white-red (3 colors) palette")]
        public static PlotModel BlackWhiteRed3()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.BlackWhiteRed(3), false);
        }

        [Example("Cool (200 colors) palette")]
        public static PlotModel Cool200()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Cool(200), false);
        }

        [Example("Rainbow (200 colors) palette")]
        public static PlotModel Rainbow200()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Rainbow(200), false);
        }

        [Example("Viridis palette")]
        public static PlotModel Viridis()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Viridis(), false);
        }

        [Example("Plasma palette")]
        public static PlotModel Plasma()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Plasma(), false);
        }

        [Example("Magma palette")]
        public static PlotModel Magma()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Magma(), false);
        }

        [Example("Inferno palette")]
        public static PlotModel Inferno()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Inferno(), false);
        }

        [Example("Cividis palette")]
        public static PlotModel Cividis()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Cividis(), false);
        }

        [Example("Viridis (10 colors) palette")]
        public static PlotModel Viridis10()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Viridis(10), false);
        }

        [Example("Rainbow (7 colors) palette")]
        public static PlotModel Rainbow7()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Rainbow(7), false);
        }

        [Example("Vertical (6 colors)")]
        public static PlotModel Vertical_6()
        {
            return HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(6), false);
        }

        [Example("Vertical reverse (6 colors)")]
        public static PlotModel Vertical_Reverse_6()
        {
            var model = HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(6), false);
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.StartPosition = 1;
            colorAxis.EndPosition = 0;
            return model;
        }

        [Example("Horizontal (6 colors)")]
        public static PlotModel Horizontal_6()
        {
            var model = HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(6), false);
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.Position = AxisPosition.Top;
            return model;
        }

        [Example("Horizontal reverse (6 colors)")]
        public static PlotModel Horizontal_Reverse_6()
        {
            var model = HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(6), false);
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.Position = AxisPosition.Top;
            colorAxis.StartPosition = 1;
            colorAxis.EndPosition = 0;
            return model;
        }

        [Example("RenderAsImage (horizontal)")]
        public static PlotModel RenderAsImage_Horizontal()
        {
            var model = HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(1000), false);
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.RenderAsImage = true;
            colorAxis.Position = AxisPosition.Top;
            return model;
        }

        [Example("RenderAsImage (horizontal reversed)")]
        public static PlotModel RenderAsImage_Horizontal_Reversed()
        {
            var model = HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(1000), false);
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.RenderAsImage = true;
            colorAxis.Position = AxisPosition.Top;
            colorAxis.StartPosition = 1;
            colorAxis.EndPosition = 0;
            return model;
        }

        [Example("RenderAsImage (vertical)")]
        public static PlotModel RenderAsImage_Vertical()
        {
            var model = HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(1000), false);
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.RenderAsImage = true;
            return model;
        }

        [Example("RenderAsImage (vertical reversed)")]
        public static PlotModel RenderAsImage_Vertical_Reversed()
        {
            var model = HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(1000), false);
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.RenderAsImage = true;
            colorAxis.StartPosition = 1;
            colorAxis.EndPosition = 0;
            return model;
        }

        [Example("Short vertical")]
        public static PlotModel Vertical_Short()
        {
            var model = HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(600), false);
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.StartPosition = 0.02;
            colorAxis.EndPosition = 0.5;
            return model;
        }

        [Example("Position None")]
        public static PlotModel Position_None()
        {
            var model = HeatMapSeriesExamples.CreatePeaks(OxyPalettes.Jet(600), false);
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.Position = AxisPosition.None;
            return model;
        }
    }
}
