using System;
using System.Windows;
using OxyPlot;

namespace PolarDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var model = new PlotModel("Polar plot", "Archimedean spiral with equation r(θ) = θ for 0 < θ < 6π");
            model.PlotType = PlotType.Polar;
            model.BorderThickness = 0;
            model.Axes.Add(
                new LinearAxis(AxisPosition.Angle, 0, Math.PI*2, Math.PI/4, Math.PI/16)
                    {
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Solid
                    });
            model.Axes.Add(new LinearAxis(AxisPosition.Magnitude)
                               {
                                   MajorGridlineStyle = LineStyle.Solid,
                                   MinorGridlineStyle = LineStyle.Solid
                               });
            model.Series.Add(new FunctionSeries(t => t, t => t,
                                                0, Math.PI*6, 0.01));
            plot1.Model = model;
        }
    }
}