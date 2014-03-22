// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolarPlotExamples.cs" company="OxyPlot">
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

using System;
using OxyPlot;

namespace ExampleLibrary
{
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("Polar Plots")]
    public static class PolarPlotExamples
    {
        [Example("Spiral")]
        public static PlotModel ArchimedeanSpiral()
        {
            var model = new PlotModel("Polar plot", "Archimedean spiral with equation r(θ) = θ for 0 < θ < 6π")
            {
                PlotType = PlotType.Polar,
                PlotAreaBorderThickness = 0,
                PlotMargins = new OxyThickness(60, 20, 4, 40)
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
                MinorGridlineStyle = LineStyle.Solid
            });
            model.Series.Add(new FunctionSeries(t => t, t => t, 0, Math.PI * 6, 0.01));
            return model;
        }

        [Example("Spiral2")]
        public static PlotModel ArchimedeanSpiral2()
        {
            var model = ArchimedeanSpiral();
            model.Title += "(reversed angle axis)";
            var angleAxis = (AngleAxis)model.Axes[0];
            angleAxis.StartAngle = 360;
            angleAxis.EndAngle = 0;
            return model;
        }

        [Example("Spiral with magnitude axis min and max")]
        public static PlotModel ArchimedeanSpiral3()
        {
            var model = ArchimedeanSpiral();
            model.Title += " (axis Minimum = 10 and Maximum = 20)";
            var magnitudeAxis = (MagnitudeAxis)model.Axes[1];
            magnitudeAxis.Minimum = 10;
            magnitudeAxis.Maximum = 20;
            return model;
        }

        [Example("Angle axis with offset angle")]
        public static PlotModel OffsetAngles()
        {
            var model = new PlotModel("Offset angle axis", "")
            {
                PlotType = PlotType.Polar,
                PlotAreaBorderThickness = 0,
                PlotMargins = new OxyThickness(60, 20, 4, 40)
            };

            var angleAxis = new AngleAxis(0, Math.PI * 2, Math.PI / 4, Math.PI / 16)
            {
                StringFormat = "0.00",
                StartAngle = 30,
                EndAngle = 390
            };
            model.Axes.Add(angleAxis);
            model.Axes.Add(new MagnitudeAxis());
            model.Series.Add(new FunctionSeries(t => t, t => t, 0, Math.PI * 6, 0.01));

            // Subscribe to the mouse down event on the line series.
            model.MouseDown += (s, e) =>
            {
                var increment = 0d;

                // Increment and decrement must be in degrees (corresponds to the StartAngle and EndAngle properties).
                if (e.ChangedButton == OxyMouseButton.Left) increment = 15;
                if (e.ChangedButton == OxyMouseButton.Right) increment = -15;

                if (Math.Abs(increment) > double.Epsilon)
                {
                    angleAxis.StartAngle += increment;
                    angleAxis.EndAngle += increment;
                    model.InvalidatePlot(false);
                    e.Handled = true;
                }
            };

            return model;
        }

        [Example("Semi-circle")]
        public static PlotModel SemiCircle()
        {
            var model = new PlotModel("Semi-circle polar plot", "")
            {
                PlotType = PlotType.Polar,
                PlotAreaBorderThickness = 0,
                PlotMargins = new OxyThickness(60, 20, 4, 40)
            };
            model.Axes.Add(
                new AngleAxis(0, 180, 45, 9)
                {
                    StartAngle = 0,
                    EndAngle = 180,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid
                });
            model.Axes.Add(new MagnitudeAxis(0, 1)
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });
            model.Series.Add(new FunctionSeries(x => Math.Sin(x / 180 * Math.PI), t => t, 0, 180, 0.01));
            return model;
        }

        [Example("Semi-circle offset angle axis range")]
        public static PlotModel SemiCircleOffsetAngleAxisRange()
        {
            var model = new PlotModel("Semi-circle polar plot", "Angle axis range offset to -180 - 180")
            {
                PlotType = PlotType.Polar,
                PlotAreaBorderThickness = 0,
                PlotMargins = new OxyThickness(60, 20, 4, 40)
            };
            model.Axes.Add(
                new AngleAxis(-180, 180, 45, 9)
                {
                    StartAngle = 0,
                    EndAngle = 360,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid
                });
            model.Axes.Add(new MagnitudeAxis(0, 1)
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });
            model.Series.Add(new FunctionSeries(x => Math.Sin(x / 180 * Math.PI), t => t, 0, 180, 0.01));
            return model;
        }
    }
}