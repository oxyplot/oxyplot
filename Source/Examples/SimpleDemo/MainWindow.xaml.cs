using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace SimpleDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel vm = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SavePng_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = ".png files|*.png";
            dlg.DefaultExt = ".png";
            if (dlg.ShowDialog(this).Value)
                plot1.SaveBitmap(dlg.FileName);
        }

        private void SaveSvg_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = ".svg files|*.svg";
            dlg.DefaultExt = ".svg";
            if (dlg.ShowDialog(this).Value)
                vm.Model.SaveSvg(dlg.FileName, plot1.ActualWidth, plot1.ActualHeight);
        }

        private void ModelChange_Click(object sender, RoutedEventArgs e)
        {
            var mi = sender as MenuItem;
            int index = vm.ModelNames.IndexOf(mi.Header as string);
            vm.ChangeModel(index);
        }

        private void SaveHtml_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = ".html files|*.html";
            dlg.DefaultExt = ".html";
            if (dlg.ShowDialog(this).Value)
            {
                vm.SaveReport(dlg.FileName);
                Process.Start(dlg.FileName);
            }
        }

        private void SaveXaml_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = ".xaml files|*.xaml";
            dlg.DefaultExt = ".xaml";
            if (dlg.ShowDialog(this).Value)
            {
                plot1.SaveXaml(dlg.FileName);
            }
        }

        private void CopySvg_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(vm.Model.ToSvg(plot1.ActualWidth, plot1.ActualHeight, true));
        }

        private void CopyBitmap_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetImage(plot1.ToBitmap());
        }

        private void CopyXaml_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(plot1.ToXaml());
        }
    }
}