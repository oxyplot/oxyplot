// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExportDemo
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Pdf;
    using OxyPlot.Series;
    using OxyPlot.Silverlight;

    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            this.InitializeComponent();
            var tmp = new PlotModel { Title = "Export demo" };
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Top });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Right });
            tmp.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 2, 100, "sin(x)"));

            //var assembly = Assembly.GetExecutingAssembly();
            //using (var stream = assembly.GetManifestResourceStream("ExportDemo.OxyPlot.png"))
            //{
            //    var image = new OxyImage(stream);
            //    tmp.Annotations.Add(new ImageAnnotation(image, 1, 0, HorizontalTextAlign.Right, VerticalTextAlign.Bottom));
            //}

            this.PlotModel = tmp;

            this.DataContext = this;
        }

        public PlotModel PlotModel { get; set; }

        private void SavePdf_Click(object sender, RoutedEventArgs e)
        {
            var d = new SaveFileDialog { Filter = "Pdf files (*.pdf)|*.pdf", DefaultExt = ".pdf" };
            if (true == d.ShowDialog())
            {
                using (var s = d.OpenFile())
                {
                    OxyPlot.PdfExporter.Export(this.plot1.ActualModel, s, this.plot1.ActualWidth, this.plot1.ActualHeight);
                }
            }
        }

        private void SaveSilverPdf_Click(object sender, RoutedEventArgs e)
        {
            var d = new SaveFileDialog { Filter = "Pdf files (*.pdf)|*.pdf", DefaultExt = ".pdf" };
            if (true == d.ShowDialog())
            {
                using (var s = d.OpenFile())
                {
                    OxyPlot.Pdf.PdfExporter.Export(this.plot1.ActualModel, s, this.plot1.ActualWidth, this.plot1.ActualHeight);
                }
            }
        }

        private void SaveSvg_Click(object sender, RoutedEventArgs e)
        {
            var d = new SaveFileDialog { Filter = "Svg files (*.svg)|*.svg", DefaultExt = ".svg" };
            if (true == d.ShowDialog())
            {
                using (var s = d.OpenFile())
                {
                    var rc = new CanvasRenderContext(new Canvas());
                    SvgExporter.Export(this.plot1.ActualModel, s, this.plot1.ActualWidth, this.plot1.ActualHeight, true, rc);
                }
            }
        }

        private void SavePng_Click(object sender, RoutedEventArgs e)
        {
            var d = new SaveFileDialog { Filter = "Png files (*.png)|*.png", DefaultExt = ".png" };
            if (true == d.ShowDialog())
            {
                using (var s = d.OpenFile())
                {
                    PngExporter.Export(this.plot1.ActualModel, s, this.plot1.ActualWidth, this.plot1.ActualHeight, OxyColors.Transparent);
                }
            }
        }
    }
}