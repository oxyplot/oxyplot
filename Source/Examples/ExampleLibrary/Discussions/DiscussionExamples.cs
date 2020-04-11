// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscussionExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Globalization;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    [Examples("Z0 Discussions")]
    public class DiscussionExamples
    {
        [Example("#445576: Invisible contour series")]
        public static PlotModel InvisibleContourSeries()
        {
            var model = new PlotModel { Title = "Invisible contour series" };
            var cs = new ContourSeries
            {
                IsVisible = false,
                ColumnCoordinates = ArrayBuilder.CreateVector(-1, 1, 0.05),
                RowCoordinates = ArrayBuilder.CreateVector(-1, 1, 0.05)
            };
            cs.Data = ArrayBuilder.Evaluate((x, y) => x + y, cs.ColumnCoordinates, cs.RowCoordinates);
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
                lca.Maximum = Double.IsNaN(lca.Maximum) ? 10 : Double.NaN;
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
                lca.Maximum = Double.IsNaN(lca.Maximum) ? 10 : Double.NaN;
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
                var columnSeries = new BarSeries();
                columnSeries.Items.Add(new BarItem(50 + r.Next(50)));
                columnSeries.Items.Add(new BarItem(40 + r.Next(50)));
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
                var columnSeries = new BarSeries();
                columnSeries.Items.Add(new BarItem(50 + r.Next(50)));
                columnSeries.Items.Add(new BarItem(40 + r.Next(50)));
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
                var columnSeries = new BarSeries();
                columnSeries.Items.Add(new BarItem(50 + r.Next(50)));
                columnSeries.Items.Add(new BarItem(40 + r.Next(50)));
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
                var columnSeries = new BarSeries();
                columnSeries.Items.Add(new BarItem(50 + r.Next(50)));
                columnSeries.Items.Add(new BarItem(40 + r.Next(50)));
                model.Series.Add(columnSeries);
            }

            return model;
        }

        [Example("#542701: Same color of LineSeries and axis title & labels")]
        public static PlotModel SameColorOfLineSeriesAndAxisTitleAndLabels()
        {
            var model = new PlotModel { Title = "Same color of LineSeries and axis title & labels" };
            var color = OxyColors.IndianRed;
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Axis 1", TitleColor = color, TextColor = color });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Series.Add(new LineSeries { Title = "LineSeries 1", Color = color, ItemsSource = new[] { new DataPoint(0, 0), new DataPoint(10, 3), new DataPoint(20, 2) } });
            return model;
        }

        [Example("#549839: Polar plot with custom arrow series")]
        public static PlotModel PolarPlotWithArrows()
        {
            var model = new PlotModel { Title = "Custom arrow series", PlotType = PlotType.Polar, PlotAreaBorderColor = OxyColors.Undefined };
            model.Axes.Add(new AngleAxis { Minimum = 0, Maximum = 360, MajorStep = 30, MinorStep = 30, MajorGridlineStyle = LineStyle.Dash });
            model.Axes.Add(new MagnitudeAxis { Minimum = 0, Maximum = 5, MajorStep = 1, MinorStep = 1, Angle = 90, MajorGridlineStyle = LineStyle.Dash });
            model.Series.Add(new ArrowSeries549839 { EndPoint = new DataPoint(1, 40) });
            model.Series.Add(new ArrowSeries549839 { EndPoint = new DataPoint(2, 75) });
            model.Series.Add(new ArrowSeries549839 { EndPoint = new DataPoint(3, 110) });
            model.Series.Add(new ArrowSeries549839 { EndPoint = new DataPoint(4, 140) });
            model.Series.Add(new ArrowSeries549839 { EndPoint = new DataPoint(5, 180) });
            return model;
        }

        [Example("MarkerType = Circle problem")]
        public static PlotModel MarkerTypeCircleProblem()
        {
            var plotModel = new PlotModel { PlotType = PlotType.Cartesian, PlotAreaBorderThickness = new OxyThickness(0) };

            var l = new Legend
            {
                LegendSymbolLength = 30
            };

            var xaxis = new DateTimeAxis
                            {
                                Position = AxisPosition.Bottom,
                                TickStyle = TickStyle.None,
                                AxislineStyle = LineStyle.Solid,
                                AxislineColor = OxyColor.FromRgb(153, 153, 153),
                                StringFormat = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(1) + "d HH",
                                IntervalType = DateTimeIntervalType.Hours
                            };

            var yaxis = new LinearAxis
                            {
                                Position = AxisPosition.Left,
                                Minimum = 0.001f,
                                Maximum = 3,
                                MajorGridlineStyle = LineStyle.Solid,
                                TickStyle = TickStyle.None,
                                IntervalLength = 50
                            };

            plotModel.Axes.Add(xaxis);
            plotModel.Axes.Add(yaxis);

            var series1 = new LineSeries
                              {
                                  Color = OxyColor.FromRgb(44, 169, 173),
                                  StrokeThickness = 1,
                                  MarkerType = MarkerType.Circle,
                                  MarkerStroke = OxyColors.Blue,
                                  MarkerFill = OxyColors.SkyBlue,
                                  // MarkerStrokeThickness = 5,
                                  MarkerSize = 2,
                                  DataFieldX = "Date",
                                  DataFieldY = "Value",
                                  TrackerFormatString = "Date: {2:d HH}&#x0a;Value: {4}"
                              };

            series1.Points.Add(new DataPoint(0.1, 0.7));
            series1.Points.Add(new DataPoint(0.6, 0.9));
            series1.Points.Add(new DataPoint(1.0, 0.85));
            series1.Points.Add(new DataPoint(1.4, 0.95));
            series1.Points.Add(new DataPoint(1.8, 1.2));
            series1.Points.Add(new DataPoint(2.2, 1.7));
            series1.Points.Add(new DataPoint(2.6, 1.7));
            series1.Points.Add(new DataPoint(3.0, 0.7));

            plotModel.Series.Add(series1);

            return plotModel;
        }

        private class ArrowSeries549839 : XYAxisSeries
        {
            private OxyColor defaultColor;

            public ArrowSeries549839()
            {
                this.Color = OxyColors.Automatic;
                this.StrokeThickness = 2;
            }

            public DataPoint StartPoint { get; set; }

            public DataPoint EndPoint { get; set; }

            public OxyColor Color { get; set; }

            public double StrokeThickness { get; set; }

            protected override void SetDefaultValues()
            {
                if (this.Color.IsAutomatic())
                {
                    this.defaultColor = this.PlotModel.GetDefaultColor();
                }
            }

            public OxyColor ActualColor
            {
                get
                {
                    return this.Color.GetActualColor(this.defaultColor);
                }
            }

            public override void Render(IRenderContext rc)
            {
                // transform to screen coordinates
                var p0 = this.Transform(this.StartPoint);
                var p1 = this.Transform(this.EndPoint);

                var direction = p1 - p0;
                var normal = new ScreenVector(direction.Y, -direction.X);

                // the end points of the arrow head, scaled by length of arrow
                var p2 = p1 - (direction * 0.2) + (normal * 0.1);
                var p3 = p1 - (direction * 0.2) - (normal * 0.1);

                // draw the line segments
                rc.DrawLineSegments(new[] { p0, p1, p1, p2, p1, p3 }, this.ActualColor, this.StrokeThickness, this.EdgeRenderingMode);
            }
        }
    }
}
