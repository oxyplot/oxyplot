// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CsvDemo
{
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

    using Microsoft.Win32;

    using OxyPlot;
    using OxyPlot.Wpf;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Plotting data from CSV files.")]
    public partial class MainWindow
    {
        private readonly MainViewModel vm = new MainViewModel();

        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this.vm;
            // vm.Open("Examples\CsvDemo\Data\GlobalTemperatureAnomaly.csv");
            // vm.Open("Examples\CsvDemo\Data\WorldPopulation.csv");
            this.vm.Open(@"Examples\CsvDemo\Data\RiverFlow.csv");
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            var data = e.Data as DataObject;
            if (data != null && data.ContainsFileDropList())
            {
                foreach (var file in data.GetFileDropList())
                {
                    this.vm.Open(file);
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OpenCsv_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = ".csv files|*.csv";
            dlg.DefaultExt = ".csv";
            if (dlg.ShowDialog(this).Value)
            {
                this.vm.Open(dlg.FileName);
            }
        }

        private void SavePlot_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = ".svg files|*.svg|.png files|*.png|.pdf files|*.pdf|.xaml files|*.xaml",
                DefaultExt = ".svg"
            };
            if (dlg.ShowDialog(this).Value)
            {
                var ext = Path.GetExtension(dlg.FileName).ToLower();
                switch (ext)
                {
                    case ".png":
                        plot1.SaveBitmap(dlg.FileName, 0, 0);
                        break;
                    case ".svg":
                        var rc = new CanvasRenderContext(new Canvas());
                        var svg = OxyPlot.SvgExporter.ExportToString(this.vm.Model, plot1.ActualWidth, plot1.ActualHeight, false, rc);
                        File.WriteAllText(dlg.FileName, svg);
                        break;
                    case ".pdf":
                        using (var s = File.Create(dlg.FileName))
                        {
                            PdfExporter.Export(vm.Model, s, plot1.ActualWidth, plot1.ActualHeight);
                        }

                        break;
                    case ".xaml":
                        plot1.SaveXaml(dlg.FileName);
                        break;
                }

                this.OpenContainingFolder(dlg.FileName);
            }
        }

        private void OpenContainingFolder(string fileName)
        {
            // var folder = Path.GetDirectoryName(fileName);
            var psi = new ProcessStartInfo("Explorer.exe", "/select," + fileName);
            Process.Start(psi);
        }

        //private void SaveReport_Click(object sender, RoutedEventArgs e)
        //{
        //    var dlg = new SaveFileDialog { Filter = ".html files|*.html", DefaultExt = ".html" };
        //    if (dlg.ShowDialog(this).Value)
        //    {
        //        this.vm.SaveReport(dlg.FileName);
        //        this.OpenContainingFolder(dlg.FileName);
        //    }
        //}

        private void CopySvg_Click(object sender, RoutedEventArgs e)
        {
            var rc = new CanvasRenderContext(null);
            var svg = OxyPlot.SvgExporter.ExportToString(this.vm.Model, plot1.ActualWidth, plot1.ActualHeight, true, rc);
            Clipboard.SetText(svg);
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
            Process.Start("http://oxyplot.org");
        }
    }
}
