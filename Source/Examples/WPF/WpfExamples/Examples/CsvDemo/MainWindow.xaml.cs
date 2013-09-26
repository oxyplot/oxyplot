// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
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
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using OxyPlot.Pdf;

namespace CsvDemo
{
    using OxyPlot.Wpf;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel vm = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
            // vm.Open("Examples\CsvDemo\Data\GlobalTemperatureAnomaly.csv");
            // vm.Open("Examples\CsvDemo\Data\WorldPopulation.csv");
            vm.Open(@"Examples\CsvDemo\Data\RiverFlow.csv");
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            var data = e.Data as DataObject;
            if (data != null && data.ContainsFileDropList())
            {
                foreach (var file in data.GetFileDropList())
                {
                    vm.Open(file);
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenCsv_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = ".csv files|*.csv";
            dlg.DefaultExt = ".csv";
            if (dlg.ShowDialog(this).Value)
            {
                vm.Open(dlg.FileName);
            }
        }

        private void SavePlot_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = ".svg files|*.svg|.png files|*.png|.pdf files|*.pdf|.xaml files|*.xaml";
            dlg.DefaultExt = ".svg";
            if (dlg.ShowDialog(this).Value)
            {
                var ext = Path.GetExtension(dlg.FileName).ToLower();
                switch (ext)
                {
                    case ".png":
                        plot1.SaveBitmap(dlg.FileName);
                        break;
                    case ".svg":
                        var rc = new ShapesRenderContext(null);
                        var svg = vm.Model.ToSvg(plot1.ActualWidth, plot1.ActualHeight, false, rc);
                        File.WriteAllText(dlg.FileName, svg);
                        break;
                    case ".pdf":
                        PdfExporter.Export(vm.Model, dlg.FileName, plot1.ActualWidth, plot1.ActualHeight);
                        break;
                    case ".xaml":
                        plot1.SaveXaml(dlg.FileName);
                        break;
                }
                OpenContainingFolder(dlg.FileName);
            }
        }

        private void OpenContainingFolder(string fileName)
        {
            // var folder = Path.GetDirectoryName(fileName);
            var psi = new ProcessStartInfo("Explorer.exe", "/select," + fileName);
            Process.Start(psi);
        }

        private void SaveReport_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = ".html files|*.html";
            dlg.DefaultExt = ".html";
            if (dlg.ShowDialog(this).Value)
            {
                vm.SaveReport(dlg.FileName);
                OpenContainingFolder(dlg.FileName);
            }
        }

        private void CopySvg_Click(object sender, RoutedEventArgs e)
        {
            var rc = new ShapesRenderContext(null);
            Clipboard.SetText(vm.Model.ToSvg(plot1.ActualWidth, plot1.ActualHeight, true,rc));
        }

        private void CopyBitmap_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetImage(plot1.ToBitmap());
        }

        private void CopyXaml_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(plot1.ToXaml());
        }

        private void HelpAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("by OxyPlot");
        }

        private void HelpWeb_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://oxyplot.codeplex.com");
        }

    }
}