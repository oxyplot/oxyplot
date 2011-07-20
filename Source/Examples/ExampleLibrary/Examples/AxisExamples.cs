using OxyPlot;

namespace ExampleLibrary
{
    [Examples("General Axis examples")]
    public static class AxisExamples
    {
        [Example("AbsoluteMinimum and AbsoluteMaximum")]
        public static PlotModel AbsoluteMinimumAndMaximum()
        {
            var model = new PlotModel("AbsoluteMinimum=-17, AbsoluteMaximum=63","Zooming and panning is limited to these values.");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) {Minimum=0, Maximum=50, AbsoluteMinimum = -17, AbsoluteMaximum = 63});
            model.Axes.Add(new LinearAxis(AxisPosition.Left) {Minimum=0, Maximum=50, AbsoluteMinimum = -17, AbsoluteMaximum = 63});
            return model;
        }

        [Example("Title with unit")]
        public static PlotModel TitleWithUnit()
        {
            var model = new PlotModel("Axis titles with units");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { Title="Speed", Unit="km/h" });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { Title = "Mass", Unit = "kg" });
            return model;
        }

        [Example("Zooming disabled")]
        public static PlotModel ZoomingDisabled()
        {
            var model = new PlotModel("Zooming disabled");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) {IsZoomEnabled = false });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) {IsZoomEnabled = false});
            return model;
        }

        [Example("Panning disabled")]
        public static PlotModel PanningDisabled()
        {
            var model = new PlotModel("Panning disabled");
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom) { IsPanEnabled = false });
            model.Axes.Add(new LinearAxis(AxisPosition.Left) { IsPanEnabled = false });
            return model;
        }
    }
}