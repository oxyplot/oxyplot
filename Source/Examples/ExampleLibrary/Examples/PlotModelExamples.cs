// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModelExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("PlotModel examples")]
    public static class PlotModelExamples
    {
        [Example("Title")]
        public static PlotModel Title()
        {
            var model = new PlotModel { Title = "Title" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("Title and Subtitle")]
        public static PlotModel TitleAndSubtitle()
        {
            var model = new PlotModel { Title = "Title", Subtitle = "Subtitle" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("Sub- and superscript in titles")]
        public static PlotModel TitleAndSubtitleWithSubSuperscript()
        {
            var model = new PlotModel { Title = "Title with^{super}_{sub}script", Subtitle = "Subtitle with^{super}_{sub}script" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("TitlePadding = 0")]
        public static PlotModel TitlePadding0()
        {
            var model = new PlotModel { Title = "TitlePadding = 0", Subtitle = "This controls the distance between the titles and the plot area. The default value is 6", TitlePadding = 0 };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("TitlePadding = 100")]
        public static PlotModel TitlePadding100()
        {
            var model = new PlotModel { Title = "TitlePadding = 100", TitlePadding = 100 };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("TitleHorizontalAlignment = CenteredWithinView")]
        public static PlotModel TitlesCenteredWithinView()
        {
            var model = new PlotModel { Title = "Title", Subtitle = "Subtitle", TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("TitleClippingOff")]
        public static PlotModel TitleClippingOff()
        {
            var model = new PlotModel { Title = "This is a very long title to illustrate that title clipping is necessary, because currently it's not clipped.", ClipTitle = false };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("TitleClipping60")]
        public static PlotModel TitleClipping60()
        {
            var model = new PlotModel { Title = "This is a very long title, that shows that title clippling is working with crrently 60% of title area", TitleClippingLength = 0.6};
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("No model")]
        public static PlotModel NoModel()
        {
            return null;
        }

        [Example("Background = Undefined (default)")]
        public static PlotModel BackgroundUndefined()
        {
            var model = new PlotModel { Title = "Background = Undefined", Background = OxyColors.Undefined };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("Background = 50% White")]
        public static PlotModel BackgroundWhite50()
        {
            var model = new PlotModel { Title = "Background = 50% White", Background = OxyColor.FromAColor(128, OxyColors.White) };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("Background = Transparent")]
        public static PlotModel BackgroundTransparent()
        {
            var model = new PlotModel { Title = "Background = Transparent", Background = OxyColors.Transparent };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("Background = LightSkyBlue")]
        public static PlotModel BackgroundLightGray()
        {
            var model = new PlotModel { Title = "Background = LightSkyBlue", Background = OxyColors.LightSkyBlue };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("Background = White")]
        public static PlotModel BackgroundWhite()
        {
            var model = new PlotModel { Title = "Background = White", Background = OxyColors.White };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("Background = Black")]
        public static PlotModel BackgroundBlack()
        {
            var model = new PlotModel { Title = "Background = Black", Background = OxyColors.Black, TextColor = OxyColors.White, TitleColor = OxyColors.White, PlotAreaBorderColor = OxyColors.White };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, TicklineColor = OxyColors.White });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, TicklineColor = OxyColors.White });
            return model;
        }

        [Example("PlotAreaBorderThickness = 2")]
        public static PlotModel PlotAreaBorderThickness2()
        {
            var model = new PlotModel { Title = "PlotAreaBorderThickness = 2", PlotAreaBorderThickness = new OxyThickness(2) };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("PlotAreaBorderThickness = (1,0,0,1)")]
        public static PlotModel PlotAreaBorderThickness1001()
        {
            var model = new PlotModel { Title = "PlotAreaBorderThickness = (1,0,0,1)", PlotAreaBorderThickness = new OxyThickness(1, 0, 0, 1) };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("PlotAreaBorderThickness = (4,1,1,4)")]
        public static PlotModel PlotAreaBorderThickness4114()
        {
            var model = new PlotModel { Title = "PlotAreaBorderThickness = (4,1,1,4)", PlotAreaBorderThickness = new OxyThickness(4, 1, 1, 4) };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("PlotAreaBorderThickness = 0")]
        public static PlotModel PlotAreaBorderThickness0()
        {
            var model = new PlotModel { Title = "PlotAreaBorderThickness = 0", PlotAreaBorderThickness = new OxyThickness(0) };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }

        [Example("PlotAreaBorderThickness / AxisLine")]
        public static PlotModel PlotAreaBorderThickness0AxisLineThickness1()
        {
            var model = new PlotModel { Title = "PlotAreaBorderThickness = 0", Subtitle = "AxislineThickness = 1, AxislineColor = OxyColors.Blue, AxislineStyle = LineStyle.Solid", PlotAreaBorderThickness = new OxyThickness(0) };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, AxislineThickness = 1, AxislineColor = OxyColors.Blue, AxislineStyle = LineStyle.Solid });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, AxislineThickness = 1, AxislineColor = OxyColors.Blue, AxislineStyle = LineStyle.Solid });
            return model;
        }

        [Example("Exception handling (invalid XAxisKey)")]
        public static PlotModel InvalidAxisKey()
        {
            var model = new PlotModel();
            model.Axes.Add(new LinearAxis());
            model.Series.Add(new LineSeries { XAxisKey = "invalidKey" });
            return model;
        }
    }
}
