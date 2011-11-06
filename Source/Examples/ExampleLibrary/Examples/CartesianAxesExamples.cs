namespace ExampleLibrary
{
    using System;

    using OxyPlot;

    [Examples("Cartesian axes")]
    public class CartesianAxesExamples : ExamplesBase
    {
        [Example("Trigonometric functions")]
        public static PlotModel FunctionSeries()
        {
            var pm = new PlotModel("Trigonometric functions") { PlotType = PlotType.Cartesian };
            pm.Series.Add(new FunctionSeries(Math.Sin, -10, 10, 0.1, "sin(x)"));
            pm.Series.Add(new FunctionSeries(Math.Cos, -10, 10, 0.1, "cos(x)"));
            pm.Series.Add(new FunctionSeries(t => 5 * Math.Cos(t), t => 5 * Math.Sin(t), 0, 2 * Math.PI, 1000, "cos(t),sin(t)"));
            return pm;
        }

        [Example("Clover")]
        public static PlotModel Clover()
        {
            var plot = new PlotModel { Title = "Parametric function", PlotType = PlotType.Cartesian };
            plot.Series.Add(new FunctionSeries(t => 2 * Math.Cos(2 * t) * Math.Cos(t), t => 2 * Math.Cos(2 * t) * Math.Sin(t),
                0, Math.PI * 2, 1000, "2cos(2t)cos(t) , 2cos(2t)sin(t)"));
            return plot;

        }
    }
}