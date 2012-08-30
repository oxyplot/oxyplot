// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ExportDemo
{
    using OxyPlot;
    using OxyPlot.Pdf;

    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            var tmp = new PlotModel("Export demo");
            tmp.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 2, 100, "sin(x)"));
            PlotModel = tmp;

            DataContext = this;
        }

        public PlotModel PlotModel { get; set; }

        private void SavePdf_Click(object sender, RoutedEventArgs e)
        {
            var d = new SaveFileDialog { Filter = "Pdf files (*.pdf)|*.pdf", DefaultExt = ".pdf" };
            if (d.ShowDialog().Value)
            {
                using (var s = d.OpenFile())
                {
                    PdfExporter.Export(Plot1.ActualModel, s, Plot1.ActualWidth, Plot1.ActualHeight);
                }
            }
        }

        private void SavePng_Click(object sender, RoutedEventArgs e)
        {
            var d = new SaveFileDialog { Filter = "Png files (*.png)|*.png", DefaultExt = ".png" };
            if (d.ShowDialog().Value)
            {
                using (var s = d.OpenFile())
                {
                    // PngExporter.Export(Plot1.ActualModel, s, Plot1.ActualWidth, Plot1.ActualHeight);
                }
            }
        }
    }
}