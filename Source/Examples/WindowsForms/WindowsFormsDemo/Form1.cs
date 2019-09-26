// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Form1.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WindowsFormsDemo
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using OxyPlot;
    using OxyPlot.Series;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            var pm = new PlotModel
            {
                Title = "Trigonometric functions",
                Subtitle = "Example using the FunctionSeries",
                PlotType = PlotType.Cartesian,
                Background = OxyColors.White,
                TitleToolTip = "My little tooltip! :-)"
            };
            pm.Series.Add(new FunctionSeries(Math.Sin, -10, 10, 0.1, "sin(x)"));
            pm.Series.Add(new FunctionSeries(Math.Cos, -10, 10, 0.1, "cos(x)"));
            pm.Series.Add(new FunctionSeries(t => 5 * Math.Cos(t), t => 5 * Math.Sin(t), 0, 2 * Math.PI, 0.1, "cos(t),sin(t)")
            {
                ToolTip = "My Series tooltip! :-D"
            });


            pm.Annotations.Add(new OxyPlot.Annotations.RectangleAnnotation()
            {
                ToolTip = "My rectangle.",
                Fill = OxyColors.Blue,
                MinimumX = 100,
                MinimumY = 100,
                MaximumX = 200,
                MaximumY = 200
            });

            pm.Axes.Clear();
            pm.Axes.Add(new OxyPlot.Axes.LinearAxis()
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Minimum = -20,
                Maximum = 80,
                ToolTip = "Bottom axis"
            });
            pm.Axes.Add(new OxyPlot.Axes.LinearAxis()
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Minimum = -10,
                Maximum = 10,
                ToolTip = "Left axis"
            });

            plot1.Model = pm;
        }
    }
}
