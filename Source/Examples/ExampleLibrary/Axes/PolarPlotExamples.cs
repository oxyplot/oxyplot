// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolarPlotExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Shows how to orient 0 degrees at the bottom and add E/W to indicate directions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("Polar Plots"), Tags("Axes")]
    public static class PolarPlotExamples
    {
        [Example("Spiral")]
        public static PlotModel ArchimedeanSpiral()
        {
            var model = new PlotModel
            {
                Title = "Polar plot",
                Subtitle = "Archimedean spiral with equation r(θ) = θ for 0 < θ < 6π",
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
            var model = new PlotModel
            {
                Title = "Offset angle axis",
                PlotType = PlotType.Polar,
                PlotAreaBorderThickness = new OxyThickness(0),
            };

            var angleAxis = new AngleAxis
            {
                Minimum = 0,
                Maximum = Math.PI * 2,
                MajorStep = Math.PI / 4,
                MinorStep = Math.PI / 16,
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
                if (e.ChangedButton == OxyMouseButton.Left)
                {
                    increment = 15;
                }

                if (e.ChangedButton == OxyMouseButton.Right)
                {
                    increment = -15;
                }

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
            var model = new PlotModel
            {
                Title = "Semi-circle polar plot",
                PlotType = PlotType.Polar,
                PlotAreaBorderThickness = new OxyThickness(0),
            };
            model.Axes.Add(
                new AngleAxis
                {
                    Minimum = 0,
                    Maximum = 180,
                    MajorStep = 45,
                    MinorStep = 9,
                    StartAngle = 0,
                    EndAngle = 180,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid
                });
            model.Axes.Add(new MagnitudeAxis
            {
                Minimum = 0,
                Maximum = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });
            model.Series.Add(new FunctionSeries(x => Math.Sin(x / 180 * Math.PI), t => t, 0, 180, 0.01));
            return model;
        }

        [Example("Semi-circle offset angle axis range")]
        public static PlotModel SemiCircleOffsetAngleAxisRange()
        {
            var model = new PlotModel
            {
                Title = "Semi-circle polar plot",
                Subtitle = "Angle axis range offset to -180 - 180",
                PlotType = PlotType.Polar,
                PlotAreaBorderThickness = new OxyThickness(0),
            };
            model.Axes.Add(
                new AngleAxis
                {
                    Minimum = -180,
                    Maximum = 180,
                    MajorStep = 45,
                    MinorStep = 9,
                    StartAngle = 0,
                    EndAngle = 360,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid
                });
            model.Axes.Add(new MagnitudeAxis
            {
                Minimum = 0,
                Maximum = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });
            model.Series.Add(new FunctionSeries(x => Math.Sin(x / 180 * Math.PI), t => t, 0, 180, 0.01));
            return model;
        }

        /// <summary>
        /// Shows how to orient 0 degrees at the bottom and add E/W to indicate directions.
        /// </summary>
        /// <returns></returns>
        [Example("East/west directions")]
        public static PlotModel EastWestDirections()
        {
            var model = new PlotModel
            {
                Title = "East/west directions",
                PlotType = PlotType.Polar,
                PlotAreaBorderThickness = new OxyThickness(0),
            };
            model.Axes.Add(
                new AngleAxis
                {
                    Minimum = 0,
                    Maximum = 360,
                    MajorStep = 30,
                    MinorStep = 30,
                    StartAngle = -90,
                    EndAngle = 270,
                    LabelFormatter = angle =>
                    {
                        if (angle > 0 && angle < 180)
                        {
                            return angle + "E";
                        }

                        if (angle > 180)
                        {
                            return (360 - angle) + "W";
                        }

                        return angle.ToString();
                    },
                    MajorGridlineStyle = LineStyle.Dot,
                    MinorGridlineStyle = LineStyle.None
                });
            model.Axes.Add(new MagnitudeAxis
            {
                Minimum = 0,
                Maximum = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });
            model.Series.Add(new FunctionSeries(x => Math.Sin(x / 180 * Math.PI), t => t, 0, 180, 0.01));
            return model;
        }

        [Example("Semi-circle full plot area")]
        public static PlotModel SemiCircleFullPlotArea()
        {
            var model = new PlotModel
            {
                Title = "Semi-circle polar plot filling the plot area",
                Subtitle = "The center can be move using the right mouse button",
                PlotType = PlotType.Polar,
                PlotAreaBorderThickness = new OxyThickness(1),
            };
            model.Axes.Add(
                new AngleAxisFullPlotArea
                {
                    Minimum = 0,
                    Maximum = 180,
                    MajorStep = 45,
                    MinorStep = 9,
                    StartAngle = 0,
                    EndAngle = 180,
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Solid
                });
            model.Axes.Add(new MagnitudeAxisFullPlotArea
            {
                Minimum = 0,
                Maximum = 1,
                MidshiftH = 0,
                MidshiftV = 0.9d,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });
            model.Series.Add(new FunctionSeries(x => Math.Sin(x / 180 * Math.PI), t => t, 0, 180, 0.01));
            return model;
        }

        [Example("Spiral full plot area")]
        public static PlotModel ArchimedeanSpiralFullPlotArea()
        {
            var model = CreateFullPlotAreaPlotModel();
            model.Series.Add(new FunctionSeries(t => t, t => t, 0, Math.PI * 6, 0.01));
            return model;
        }

        [Example("Spiral full plot area with negative minimum")]
        public static PlotModel SpiralWithNegativeMinium()
        {
            var model = CreateFullPlotAreaPlotModel();
            model.Title += " with a negative minimum";
            model.Series.Add(new FunctionSeries(t => t, t => t, -Math.PI * 6, Math.PI * 6, 0.01));
            return model;
        }

        [Example("Spiral full plot area with positive minimum")]
        public static PlotModel SpiralWithPositiveMinium()
        {
            var model = CreateFullPlotAreaPlotModel();
            model.Title += " with a positive minimum";
            model.Series.Add(new FunctionSeries(t => t, t => t, Math.PI * 6, Math.PI * 12, 0.01));
            return model;
        }

        private static PlotModel CreateFullPlotAreaPlotModel()
        {
            var model = new PlotModel
            {
                Title = "Polar plot filling the plot area",
                Subtitle = "The center can be move using the right mouse button",
                PlotType = PlotType.Polar,
                PlotAreaBorderThickness = new OxyThickness(1),
            };

            model.Axes.Add(
                new AngleAxisFullPlotArea
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

            model.Axes.Add(new MagnitudeAxisFullPlotArea
            {
                MidshiftH = -0.1d,
                MidshiftV = -0.25d,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Solid
            });

            return model;
        }
    }
}
