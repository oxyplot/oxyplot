// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionAnnotationExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;

    [Examples("FunctionAnnotation"), Tags("Annotations")]
    public static class FunctionAnnotationExamples
    {
        [Example("FunctionAnnotation")]
        public static PlotModel FunctionAnnotation()
        {
            var model = new PlotModel { Title = "FunctionAnnotation" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -20, Maximum = 80 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            model.Annotations.Add(new FunctionAnnotation { Equation = Math.Sin, StrokeThickness = 2, Color = OxyColor.FromAColor(120, OxyColors.Blue), Text = "f(x)=sin(x)" });
            model.Annotations.Add(new FunctionAnnotation { Equation = y => y * y, StrokeThickness = 2, Color = OxyColor.FromAColor(120, OxyColors.Red), Type = FunctionAnnotationType.EquationY, Text = "f(y)=y^2" });
            return model;
        }
    }
}