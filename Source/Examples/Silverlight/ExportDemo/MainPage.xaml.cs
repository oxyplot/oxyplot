// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
    using System.Reflection;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Pdf;
    using OxyPlot.Series;
    using OxyPlot.Silverlight;

    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            var tmp = new PlotModel("Export demo");
            tmp.Axes.Add(new LinearAxis(AxisPosition.Bottom));
            tmp.Axes.Add(new LinearAxis(AxisPosition.Top));
            tmp.Axes.Add(new LinearAxis(AxisPosition.Left));
            tmp.Axes.Add(new LinearAxis(AxisPosition.Right));
            tmp.Series.Add(new FunctionSeries(Math.Sin, 0, Math.PI * 2, 100, "sin(x)"));

            //var assembly = Assembly.GetExecutingAssembly();
            //using (var stream = assembly.GetManifestResourceStream("ExportDemo.OxyPlot.png"))
            //{
            //    var image = new OxyImage(stream);
            //    tmp.Annotations.Add(new ImageAnnotation(image, 1, 0, HorizontalTextAlign.Right, VerticalTextAlign.Bottom));
            //}

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

        private void SaveSvg_Click(object sender, RoutedEventArgs e)
        {
            var d = new SaveFileDialog { Filter = "Svg files (*.svg)|*.svg", DefaultExt = ".svg" };
            if (d.ShowDialog().Value)
            {
                using (var s = d.OpenFile())
                {
                    var rc = new SilverlightRenderContext(new Canvas());
                    SvgExporter.Export(Plot1.ActualModel, s, Plot1.ActualWidth, Plot1.ActualHeight, true, rc);
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
                    PngExporter.Export(Plot1.ActualModel, s, Plot1.ActualWidth, Plot1.ActualHeight);
                }
            }
        }
    }
}