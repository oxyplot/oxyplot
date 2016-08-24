// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearAxisExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;

    [Examples("LinearAxis"), Tags("Axes")]
    public static class LinearAxisExamples
    {
        [Example("Default StringFormat ('g6')")]
        public static PlotModel StringFormat()
        {
            return CreateExample(1.2345678901234567890e5, 1.2345678901234567890e6, null);
        }

        [Example("StringFormat = 'g2'")]
        public static PlotModel StringFormatG2()
        {
            return CreateExample(1.2345678901234567890e5, 1.2345678901234567890e6, "g2");
        }

        [Example("StringFormat = 'g10'")]
        public static PlotModel StringFormatG10()
        {
            return CreateExample(1.2345678901234567890e5, 1.2345678901234567890e6, "g10");
        }

        [Example("StringFormat = 'f2'")]
        public static PlotModel StringFormatF2()
        {
            return CreateExample(1.2345678901234567890e5, 1.2345678901234567890e6, "f2");
        }

        private static PlotModel CreateExample(double min, double max, string stringFormat)
        {
            var m = new PlotModel();
            m.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = min, Maximum = max, StringFormat = stringFormat });
            m.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = min, Maximum = max, StringFormat = stringFormat });
            return m;
        }

        [Example("TickStyle: None")]
        public static PlotModel TickStyleNone()
        {
            return CreateTickStyleModel(TickStyle.None);
        }

        [Example("TickStyle: Crossing")]
        public static PlotModel TickStyleCrossing()
        {
            return CreateTickStyleModel(TickStyle.Crossing);
        }

        [Example("TickStyle: Inside")]
        public static PlotModel TickStyleInside()
        {
            return CreateTickStyleModel(TickStyle.Inside);
        }

        [Example("TickStyle: Outside")]
        public static PlotModel TickStyleOutside()
        {
            return CreateTickStyleModel(TickStyle.Outside);
        }

        private static PlotModel CreateTickStyleModel(TickStyle tickStyle)
        {
            var model = new PlotModel { Title = "TickStyle: " + tickStyle };
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                TickStyle = tickStyle,
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                MaximumPadding = 0,
                MinimumPadding = 0
            });
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                TickStyle = tickStyle,
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                MaximumPadding = 0,
                MinimumPadding = 0
            });
            return model;
        }

        [Example("Gridlines: None")]
        public static PlotModel GridlinesNone()
        {
            return CreateGridlinesModel("None", LineStyle.None, LineStyle.None);
        }

        [Example("Gridlines: Horizontal")]
        public static PlotModel GridlinesHorizontal()
        {
            return CreateGridlinesModel("Horizontal", LineStyle.Solid, LineStyle.None);
        }

        [Example("Gridlines: Vertical")]
        public static PlotModel GridlinesVertical()
        {
            return CreateGridlinesModel("Vertical", LineStyle.None, LineStyle.Solid);
        }

        [Example("Gridlines: Both")]
        public static PlotModel GridlinesBoth()
        {
            return CreateGridlinesModel("Both", LineStyle.Solid, LineStyle.Solid);
        }

        private static PlotModel CreateGridlinesModel(string title, LineStyle horizontal, LineStyle vertical)
        {
            var model = new PlotModel { Title = "Gridlines: " + title };
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = vertical,
                MinorGridlineStyle = vertical == LineStyle.Solid ? LineStyle.Dot : LineStyle.None,
                MaximumPadding = 0,
                MinimumPadding = 0
            });
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = horizontal,
                MinorGridlineStyle = horizontal == LineStyle.Solid ? LineStyle.Dot : LineStyle.None,
                MaximumPadding = 0,
                MinimumPadding = 0
            });
            return model;
        }
    }
}