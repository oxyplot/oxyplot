// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorBarSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Legends;
    using OxyPlot.Series;
    using System;

    [Examples("ErrorBarSeries"), Tags("Series")]
    public class ErrorBarSeriesExamples
    {
        [Example("ErrorBarSeries")]
        [DocumentationExample("Series/ErrorBarSeries")]
        public static PlotModel GetErrorBarSeries()
        {
            var model = new PlotModel
            {
                Title = "ErrorBarSeries"
            };

            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            model.Legends.Add(l);

            var s1 = new ErrorBarSeries { Title = "Series 1", IsStacked = false, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Items.Add(new ErrorBarItem { Value = 25, Error = 2 });
            s1.Items.Add(new ErrorBarItem { Value = 137, Error = 25 });
            s1.Items.Add(new ErrorBarItem { Value = 18, Error = 4 });
            s1.Items.Add(new ErrorBarItem { Value = 40, Error = 29 });

            var s2 = new ErrorBarSeries { Title = "Series 2", IsStacked = false, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Items.Add(new ErrorBarItem { Value = 35, Error = 20 });
            s2.Items.Add(new ErrorBarItem { Value = 17, Error = 7 });
            s2.Items.Add(new ErrorBarItem { Value = 118, Error = 44 });
            s2.Items.Add(new ErrorBarItem { Value = 49, Error = 29 });

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");

            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);

            return model;
        }

        [Example("ErrorBarSeries (thick error lines)")]
        public static PlotModel GetErrorBarSeriesThickErrorLines()
        {
            var model = GetErrorBarSeries();
            foreach (ErrorBarSeries s in model.Series)
            {
                s.ErrorWidth = 0;
                s.ErrorStrokeThickness = 4;
            }

            return model;
        }
        [Example("MarkerErrorBarSeries")]
        [DocumentationExample("Series/MarkerErrorBarSeries")]
        public static PlotModel GetMarkerErrorBarSeries()
        {
            var model = new PlotModel
            {
                Title = "MarkerErrorBarSeries"
            };
            var l = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            model.Legends.Add(l);

            var s1 = new ErrorBarSeries { Title = "Series 1", IsStacked = false, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Items.Add(new ErrorBarItem
            {
                Value = 25,
                Error = 2,
                IsMarkerVisible = true,
                MarkerColor = OxyColors.Red,
                MarkerType = MarkerType.Star,
                MarkerSize = 5,
                MarkerOffset = new ScreenPoint(5, 0),
                MarkerStrokeColor = OxyColors.Blue,
                MarkerStrokeThickness = 1,
            });
            s1.Items.Add(new ErrorBarItem
            {
                Value = 137,
                Error = 25,
                IsMarkerVisible = true,
                MarkerColor = OxyColors.Yellow,
                MarkerType = MarkerType.Cross,
                MarkerSize = 5,
                MarkerOffset = new ScreenPoint(0, 0),
                MarkerStrokeColor = OxyColors.Red,
                MarkerStrokeThickness = 1,
            });
            s1.Items.Add(new ErrorBarItem
            {
                Value = 18,
                Error = 4,
                IsMarkerVisible = true,
                MarkerColor = OxyColors.Yellow,
                MarkerType = MarkerType.Diamond,
                MarkerSize = 8,
                MarkerOffset = new ScreenPoint(0, 0),
                MarkerStrokeColor = OxyColors.LightGreen,
                MarkerStrokeThickness = 1,
            });
            s1.Items.Add(new ErrorBarItem
            {
                Value = 40,
                Error = 29,
                IsMarkerVisible = true,
                MarkerColor = OxyColors.Yellow,
                MarkerType = MarkerType.Custom,
                CustomOutline = GetSpiralStar(),
                MarkerSize = 30,
                MarkerOffset = new ScreenPoint(50, 0),
                MarkerStrokeColor = OxyColors.LightGreen,
                MarkerStrokeThickness = 1,
            });

            var s2 = new ErrorBarSeries { Title = "Series 2", IsStacked = false, StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Items.Add(new ErrorBarItem
            {
                Value = 25,
                Error = 2,
                IsMarkerVisible = true,
                MarkerColor = OxyColors.Red,
                MarkerType = MarkerType.Square,
                MarkerSize = 5,
                MarkerOffset = new ScreenPoint(5, 0),
                MarkerStrokeColor = OxyColors.Blue,
                MarkerStrokeThickness = 1,
            });
            s2.Items.Add(new ErrorBarItem
            {
                Value = 137,
                Error = 25,
                IsMarkerVisible = true,
                MarkerColor = OxyColors.Yellow,
                MarkerType = MarkerType.Triangle,
                MarkerSize = 5,
                MarkerOffset = new ScreenPoint(0, 0),
                MarkerStrokeColor = OxyColors.Red,
                MarkerStrokeThickness = 1,
            });
            s2.Items.Add(new ErrorBarItem
            {
                Value = 18,
                Error = 4,
                IsMarkerVisible = true,
                MarkerColor = OxyColors.Yellow,
                MarkerType = MarkerType.Plus,
                MarkerSize = 8,
                MarkerOffset = new ScreenPoint(0, 0),
                MarkerStrokeColor = OxyColors.LightGreen,
                MarkerStrokeThickness = 1,
            });
            s2.Items.Add(new ErrorBarItem
            {
                Value = 40,
                Error = 29,
                IsMarkerVisible = true,
                MarkerColor = OxyColors.GreenYellow,
                MarkerType = MarkerType.Custom,
                CustomOutline = GetHeartbeat(),
                MarkerSize = 30,
                MarkerOffset = new ScreenPoint(50, 0),
                MarkerStrokeColor = OxyColors.BlueViolet,
                MarkerStrokeThickness = 1,
            });

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");

            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);

            return model;
        }
        public static ScreenPoint[] GetHeartbeat(int points = 20)
        {
            var outline = new ScreenPoint[points];
            for (int i = 0; i < points; i++)
            {
                double x = 2.0 * i / (points - 1) - 1; // -1 到 1
                double y = Math.Sin(x * Math.PI * 3) * Math.Exp(-Math.Abs(x)) * 0.8;

                // 添加一些随机脉冲效果
                y += Math.Sin(x * Math.PI * 8) * 0.1 * Math.Exp(-x * x * 4);

                outline[i] = new ScreenPoint(x, y);
            }
            return outline;
        }
        public static ScreenPoint[] GetSpiralStar(int points = 24, double tightness = 0.1)
        {
            var outline = new ScreenPoint[points];
            for (int i = 0; i < points; i++)
            {
                double progress = (double)i / points;
                double radius = 0.2 + 0.8 * progress; // 半径逐渐增大
                double angle = 2 * Math.PI * (5 * progress + tightness * i);

                outline[i] = new ScreenPoint(
                    Math.Cos(angle) * radius,
                    Math.Sin(angle) * radius
                );
            }
            return outline;
        }
    }
}
