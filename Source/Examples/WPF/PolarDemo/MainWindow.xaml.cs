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
            model.PlotMargins=new OxyThickness(20,20,4,40);
            model.BoxColor = null;
            model.Axes.Add(
                new AngleAxis(0, Math.PI*2, Math.PI/4, Math.PI/16)
                    {
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Solid,
                        FormatAsFractions = true,
                        FractionUnit = Math.PI,
                        FractionUnitSymbol = "π"
                    });
            model.Axes.Add(new MagnitudeAxis
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