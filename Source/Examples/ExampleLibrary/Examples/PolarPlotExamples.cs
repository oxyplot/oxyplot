// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolarPlotExamples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using OxyPlot;

namespace ExampleLibrary
{
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
                new AngleAxis(0, Math.PI * 2, Math.PI / 4, Math.PI / 16)
                    {
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Solid,
                        FormatAsFractions = true,
                        FractionUnit = Math.PI,
                        FractionUnitSymbol = "π"
                    });
            model.Axes.Add(new MagnitudeAxis()
                               {
                                   MajorGridlineStyle = LineStyle.Solid,
                                   MinorGridlineStyle = LineStyle.Solid
                               });
            model.Series.Add(new FunctionSeries(t => t, t => t, 0, Math.PI * 6, 0.01));
            return model;
        }
    }
}