namespace ExampleLibrary
{
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Axes;

    [Examples("Custom axes")]
    public static class CustomAxisExamples
    {
        public class ArrowAxis : LinearAxis
        {
            public override void Render(IRenderContext rc, PlotModel model, AxisLayer axisLayer, int pass)
            {
                base.Render(rc, model, axisLayer, pass);
                var points = new List<ScreenPoint>();
                if (this.IsHorizontal())
                {
                    var xmax = this.Transform(this.ActualMaximum);
                    points.Add(new ScreenPoint(xmax + 4, model.PlotArea.Bottom - 4));
                    points.Add(new ScreenPoint(xmax + 18, model.PlotArea.Bottom));
                    points.Add(new ScreenPoint(xmax + 4, model.PlotArea.Bottom + 4));
                    //// etc.
                }
                else
                {
                    var ymax = this.Transform(this.ActualMaximum);
                    points.Add(new ScreenPoint(model.PlotArea.Left - 4, ymax - 4));
                    points.Add(new ScreenPoint(model.PlotArea.Left, ymax - 18));
                    points.Add(new ScreenPoint(model.PlotArea.Left + 4, ymax - 4));
                    //// etc.
                }

                rc.DrawPolygon(points, OxyColors.Black, OxyColors.Undefined);
            }
        }

        [Example("ArrowAxis")]
        public static PlotModel CustomArrowAxis()
        {
            // https://oxyplot.codeplex.com/discussions/467255
            var model = new PlotModel { PlotAreaBorderThickness = 0, PlotMargins = new OxyThickness(60, 60, 60, 60) };
            model.Axes.Add(new ArrowAxis { Position = AxisPosition.Bottom, AxislineStyle = LineStyle.Solid });
            model.Axes.Add(new ArrowAxis { Position = AxisPosition.Left, AxislineStyle = LineStyle.Solid });
            return model;
        }
    }
}