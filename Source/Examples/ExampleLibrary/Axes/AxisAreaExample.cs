using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExampleLibrary.Axes
{
    [Examples("AxisArea"), Tags("Axes")]
    public class AxisAreaExample
    {
        [Example("AxisArea LinearAxis")]
        public static PlotModel AxisAreaLinearAxis()
        {
            var model = GetLinearAxisPlotModel();
            model.Annotations.Add(new RenderingCapabilities.DelegateAnnotation(context =>
            {
                foreach (var axis in model.Axes)
                {
                    context.DrawRectangle((OxyRect)axis.AxisArea, OxyColors.Undefined, OxyColor.FromAColor(128, OxyColors.Red), 1, EdgeRenderingMode.Automatic);
                    context.DrawRectangle((OxyRect)axis.TitleArea, OxyColors.Undefined, OxyColor.FromAColor(128, OxyColors.Green), 1, EdgeRenderingMode.Automatic);
                    context.DrawRectangle((OxyRect)axis.AxisLineArea, OxyColors.Undefined, OxyColor.FromAColor(128, OxyColors.Blue), 1, EdgeRenderingMode.Automatic);
                }
            }));
            // Subscribe to the mouse down event on the line series
            model.MouseDown += (s, e) =>
            {
                ResetAxesFormat(model);
                if (e.ChangedButton == OxyMouseButton.Left)
                {
                    if (model.Axes.SingleOrDefault(axis => axis.TitleArea.Contains(e.Position.X, e.Position.Y)) is Axis clickedAxis1)
                    {
                        clickedAxis1.TitleFontWeight = 800;
                        clickedAxis1.TitleColor = OxyColor.FromAColor(255, OxyColors.Purple);
                    }
                    else if (model.Axes.SingleOrDefault(axis => axis.AxisLineArea.Contains(e.Position.X, e.Position.Y)) is Axis clickedAxis2)
                    {
                        clickedAxis2.FontWeight = 800;
                        clickedAxis2.TextColor = OxyColor.FromAColor(255, OxyColors.Purple);
                    }
                    else if (model.Axes.SingleOrDefault(axis => axis.AxisArea.Contains(e.Position.X, e.Position.Y)) is Axis clickedAxis3)
                    {
                        clickedAxis3.FontWeight = 800;
                        clickedAxis3.TitleFontWeight = 800;
                        clickedAxis3.TitleColor = OxyColor.FromAColor(255, OxyColors.Purple);
                        clickedAxis3.TextColor = OxyColor.FromAColor(255, OxyColors.Purple);
                    }
                }
                model.InvalidatePlot(false);
            };
            return model;
        }

        [Example("AxisArea AngleAxis")]
        public static PlotModel AxisAreaAngleAxis()
        {
            var model = GetAngleAxisPlotModel();
            model.Annotations.Add(new RenderingCapabilities.DelegateAnnotation(context =>
            {
                context.DrawRectangle((OxyRect)model.Axes[1].AxisArea, OxyColors.Undefined, OxyColor.FromAColor(255, OxyColors.Red), 1, EdgeRenderingMode.Automatic);
                var annulus = (OxyAnnulus)model.Axes[0].AxisArea;
                context.DrawCircle(annulus.Center, annulus.InnerRadius, OxyColors.Undefined, OxyColor.FromAColor(255, OxyColors.Blue), 1, EdgeRenderingMode.Automatic);
                context.DrawCircle(annulus.Center, annulus.OuterRadius, OxyColors.Undefined, OxyColor.FromAColor(255, OxyColors.Blue), 1, EdgeRenderingMode.Automatic);
            }));
                // Subscribe to the mouse down event on the line series
            model.MouseDown += (s, e) =>
            {
                ResetAxesFormat(model);
                if (e.ChangedButton == OxyMouseButton.Left)
                {
                    if (model.Axes.SingleOrDefault(axis => axis.TitleArea.Contains(e.Position.X, e.Position.Y)) is Axis clickedAxis1)
                    {
                        clickedAxis1.TitleFontWeight = 800;
                        clickedAxis1.TitleColor = OxyColor.FromAColor(255, OxyColors.Purple);
                    }
                    else if (model.Axes.SingleOrDefault(axis => axis.AxisLineArea.Contains(e.Position.X, e.Position.Y)) is Axis clickedAxis2)
                    {
                        clickedAxis2.FontWeight = 800;
                        clickedAxis2.TextColor = OxyColor.FromAColor(255, OxyColors.Purple);
                    }
                    else if (model.Axes.SingleOrDefault(axis => axis.AxisArea.Contains(e.Position.X, e.Position.Y)) is Axis clickedAxis3)
                    {
                        clickedAxis3.FontWeight = 800;
                        clickedAxis3.TitleFontWeight = 800;
                        clickedAxis3.TitleColor = OxyColor.FromAColor(255, OxyColors.Purple);
                        clickedAxis3.TextColor = OxyColor.FromAColor(255, OxyColors.Purple);
                    }
                }
                model.InvalidatePlot(false);
            };
            return model;
        }

        private static void ResetAxesFormat(PlotModel model)
        {
            foreach (var axis in model.Axes)
            {
                axis.TitleFontWeight = 400;
                axis.FontWeight = 400;
                axis.TitleColor = OxyColor.FromAColor(255, OxyColors.Black);
                axis.TextColor = OxyColor.FromAColor(255, OxyColors.Black);
            }
        }

        private static PlotModel GetLinearAxisPlotModel()
        {
            var model = new PlotModel();
            model.Axes.Add(new LinearAxis { Maximum = 36, Minimum = 0, Title = "km/h" });
            model.Axes.Add(new LinearAxis { Maximum = 10, Minimum = 0, Position = AxisPosition.Right, Title = "m/s" });
            model.Axes.Add(new LinearAxis
            {
                Maximum = 10,
                Minimum = 0,
                Position = AxisPosition.Bottom,
                Title = "meter"
            });
            model.Axes.Add(new LinearAxis
            {
                Maximum = 10000,
                Minimum = 0,
                Position = AxisPosition.Top,
                Title = "millimeter"
            });
            return model;
        }

        private static PlotModel GetAngleAxisPlotModel()
        {
            var model = new PlotModel
            {
                PlotType = PlotType.Polar,
                PlotAreaBorderThickness = new OxyThickness(0),
            };
            model.Axes.Add(
                new AngleAxis
                {
                    MajorStep = Math.PI / 4,
                    MinorStep = Math.PI / 16,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid,
                    FormatAsFractions = true,
                    FractionUnit = Math.PI,
                    FractionUnitSymbol = "π",
                    Minimum = 0,
                    Maximum = 2 * Math.PI
                });
            model.Axes.Add(new MagnitudeAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid,
            });
            model.Series.Add(new FunctionSeries(t => t, t => t, 0, Math.PI * 6, 0.01));
            return model;
        }
    }
}
