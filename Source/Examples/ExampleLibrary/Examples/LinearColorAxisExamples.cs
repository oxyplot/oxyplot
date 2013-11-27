namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;

    [Examples("LinearColorAxis")]
    public class LinearColorAxisExamples : ExamplesBase
    {
        [Example("Vertical (6 colors)")]
        public static PlotModel Vertical_6()
        {
            var model = HeatMapSeriesExamples.Peaks();
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.Palette = OxyPalettes.Jet(6);
            return model;
        }

        [Example("Vertical reverse (6 colors)")]
        public static PlotModel Vertical_Reverse_6()
        {
            var model = HeatMapSeriesExamples.Peaks();
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.StartPosition = 1;
            colorAxis.EndPosition = 0;
            colorAxis.Palette = OxyPalettes.Jet(6);
            return model;
        }

        [Example("Horizontal (6 colors)")]
        public static PlotModel Horizontal_6()
        {
            var model = HeatMapSeriesExamples.Peaks();
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.Position = AxisPosition.Top;
            colorAxis.Palette = OxyPalettes.Jet(6);
            return model;
        }

        [Example("Horizontal reverse (6 colors)")]
        public static PlotModel Horizontal_Reverse_6()
        {
            var model = HeatMapSeriesExamples.Peaks();
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.Position = AxisPosition.Top;
            colorAxis.StartPosition = 1;
            colorAxis.EndPosition = 0;
            colorAxis.Palette = OxyPalettes.Jet(6);
            return model;
        }

        [Example("RenderAsImage (horizontal)")]
        public static PlotModel RenderAsImage_Horizontal()
        {
            var model = HeatMapSeriesExamples.Peaks();
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.RenderAsImage = true;
            colorAxis.Position = AxisPosition.Top;
            colorAxis.Palette = OxyPalettes.Jet(1000);
            return model;
        }

        [Example("RenderAsImage (vertical)")]
        public static PlotModel RenderAsImage_Vertical()
        {
            var model = HeatMapSeriesExamples.Peaks();
            var colorAxis = (LinearColorAxis)model.Axes[0];
            colorAxis.RenderAsImage = true;
            colorAxis.Palette = OxyPalettes.Jet(1000);
            return model;
        }
    }
}