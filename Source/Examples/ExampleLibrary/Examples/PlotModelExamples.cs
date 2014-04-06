// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModelExamples.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;

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

        [Example("PlotMargins = (100,20,100,50)")]
        public static PlotModel PlotMargins()
        {
            var model = new PlotModel { Title = "PlotMargins = (100,20,100,50)", AutoAdjustPlotMargins = false, PlotMargins = new OxyThickness(100, 20, 100, 50) };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            return model;
        }
    }
}