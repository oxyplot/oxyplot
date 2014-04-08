// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscussionExamples.cs" company="OxyPlot">
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
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("Z0 Discussions")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    // ReSharper disable InconsistentNaming
    public class DiscussionExamples : ExamplesBase
    {
        [Example("#445576: Invisible contour series")]
        public static PlotModel InvisibleContourSeries()
        {
            var model = new PlotModel { Title = "Invisible contour series" };
            var cs = new ContourSeries
            {
                IsVisible = false,
                ColumnCoordinates = ArrayHelper.CreateVector(-1, 1, 0.05),
                RowCoordinates = ArrayHelper.CreateVector(-1, 1, 0.05)
            };
            cs.Data = ArrayHelper.Evaluate((x, y) => x + y, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Series.Add(cs);
            return model;
        }

        [Example("#461507: StairStepSeries NullReferenceException")]
        public static PlotModel StairStepSeries_NullReferenceException()
        {
            var plotModel1 = new PlotModel { Title = "StairStepSeries NullReferenceException" };
            plotModel1.Series.Add(new StairStepSeries());
            return plotModel1;
        }

        [Example("#501409: Heatmap interpolation color")]
        public static PlotModel HeatMapSeriesInterpolationColor()
        {
            var data = new double[2, 3];
            data[0, 0] = 10;
            data[0, 1] = 0;
            data[0, 2] = -10;

            var model = new PlotModel { Title = "HeatMapSeries" };
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = new OxyPalette(OxyColors.Red, OxyColors.Green, OxyColors.Blue) });

            var hms = new HeatMapSeries
            {
                CoordinateDefinition = HeatMapCoordinateDefinition.Center,
                X0 = 0,
                X1 = 3,
                Y0 = 0,
                Y1 = 2,
                Data = data,
                Interpolate = false,
                LabelFontSize = 0.2
            };
            model.Series.Add(hms);
            return model;
        }

        [Example("#522598: Peaks 400x400")]
        public static PlotModel Peaks400()
        {
            return HeatMapSeriesExamples.CreatePeaks(null, true, 400);
        }

        [Example("#474875: Updating HeatMapSeries 1")]
        public static PlotModel UpdatingHeatMapSeries1()
        {
            var model = HeatMapSeriesExamples.CreatePeaks();
            model.Title = "Updating HeatMapSeries";
            model.Subtitle = "Click the heat map to change the Maximum of the color axis.";
            var lca = (LinearColorAxis)model.Axes[0];
            var hms = (HeatMapSeries)model.Series[0];
            hms.MouseDown += (s, e) =>
            {
                lca.Maximum = double.IsNaN(lca.Maximum) ? 10 : double.NaN;
                model.InvalidatePlot(true);
            };
            return model;
        }

        [Example("#474875: Updating HeatMapSeries 2")]
        public static PlotModel UpdatingHeatMapSeries()
        {
            var model = HeatMapSeriesExamples.CreatePeaks();
            model.Title = "Updating HeatMapSeries";
            model.Subtitle = "Click the heat map to change the Maximum of the color axis and invoke the Invalidate method on the HeatMapSeries.";
            var lca = (LinearColorAxis)model.Axes[0];
            var hms = (HeatMapSeries)model.Series[0];
            hms.MouseDown += (s, e) =>
            {
                lca.Maximum = double.IsNaN(lca.Maximum) ? 10 : double.NaN;
                hms.Invalidate();
                model.InvalidatePlot(true);
            };
            return model;
        }

        [Example("#539104: Reduced color saturation")]
        public static PlotModel ReducedColorSaturation()
        {
            var model = new PlotModel
            {
                Title = "Reduced color saturation",
            };

            model.Axes.Add(new CategoryAxis { Position = AxisPosition.Bottom });

            // modify the saturation of the default colors
            model.DefaultColors = model.DefaultColors.Select(c => c.ChangeSaturation(0.5)).ToList();

            var r = new Random(37);
            for (var i = 0; i < model.DefaultColors.Count; i++)
            {
                var columnSeries = new ColumnSeries();
                columnSeries.Items.Add(new ColumnItem(50 + r.Next(50)));
                columnSeries.Items.Add(new ColumnItem(40 + r.Next(50)));
                model.Series.Add(columnSeries);
            }

            return model;
        }

        [Example("#539104: Medium intensity colors")]
        public static PlotModel MediumIntensityColors()
        {
            var model = new PlotModel
            {
                Title = "Medium intensity colors",
            };

            model.Axes.Add(new CategoryAxis { Position = AxisPosition.Bottom });

            // See http://www.perceptualedge.com/articles/visual_business_intelligence/rules_for_using_color.pdf
            model.DefaultColors = new[]
            {
                OxyColor.FromRgb(114, 114, 114),
                OxyColor.FromRgb(241, 89, 95),
                OxyColor.FromRgb(121, 195, 106),
                OxyColor.FromRgb(89, 154, 211),
                OxyColor.FromRgb(249, 166, 90),
                OxyColor.FromRgb(158, 102, 171),
                OxyColor.FromRgb(205, 112, 88),
                OxyColor.FromRgb(215, 127, 179)
            };

            var r = new Random(37);
            for (var i = 0; i < model.DefaultColors.Count; i++)
            {
                var columnSeries = new ColumnSeries();
                columnSeries.Items.Add(new ColumnItem(50 + r.Next(50)));
                columnSeries.Items.Add(new ColumnItem(40 + r.Next(50)));
                model.Series.Add(columnSeries);
            }

            return model;
        }

        [Example("#539104: Brewer colors (4)")]
        public static PlotModel BrewerColors4()
        {
            var model = new PlotModel
            {
                Title = "Brewer colors (Accent scheme)",
            };

            model.Axes.Add(new CategoryAxis { Position = AxisPosition.Bottom });

            // See http://colorbrewer2.org/?type=qualitative&scheme=Accent&n=4
            model.DefaultColors = new[]
            {
                OxyColor.FromRgb(127, 201, 127),
                OxyColor.FromRgb(190, 174, 212),
                OxyColor.FromRgb(253, 192, 134),
                OxyColor.FromRgb(255, 255, 153)
            };

            var r = new Random(37);
            for (var i = 0; i < model.DefaultColors.Count; i++)
            {
                var columnSeries = new ColumnSeries();
                columnSeries.Items.Add(new ColumnItem(50 + r.Next(50)));
                columnSeries.Items.Add(new ColumnItem(40 + r.Next(50)));
                model.Series.Add(columnSeries);
            }

            return model;
        }

        [Example("#539104: Brewer colors (6)")]
        public static PlotModel BrewerColors6()
        {
            var model = new PlotModel
            {
                Title = "Brewer colors (Paired scheme)",
            };

            model.Axes.Add(new CategoryAxis { Position = AxisPosition.Bottom });

            // See http://colorbrewer2.org/?type=qualitative&scheme=Paired&n=6
            model.DefaultColors = new[]
            {
                OxyColor.FromRgb(166, 206, 227),
                OxyColor.FromRgb(31, 120, 180),
                OxyColor.FromRgb(178, 223, 138),
                OxyColor.FromRgb(51, 160, 44),
                OxyColor.FromRgb(251, 154, 153),
                OxyColor.FromRgb(227, 26, 28)
            };

            var r = new Random(37);
            for (var i = 0; i < model.DefaultColors.Count; i++)
            {
                var columnSeries = new ColumnSeries();
                columnSeries.Items.Add(new ColumnItem(50 + r.Next(50)));
                columnSeries.Items.Add(new ColumnItem(40 + r.Next(50)));
                model.Series.Add(columnSeries);
            }

            return model;
        }
    }
}