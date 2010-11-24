using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CsvDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {

        }

        private void OpenCsv_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SaveSvg_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SavePdf_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SavePng_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SaveXaml_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SaveHtmlReport_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SavePdfReport_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            throw new NotImplementedException();
        }

        private void CopySvg_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CopyBitmap_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CopyXaml_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SavePlot_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SaveReport_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HelpAbout_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HelpWeb_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://oxyplot.codeplex.com");
        }
    }
}
