// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellView.xaml.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using OxyPlot.Pdf;

namespace ExportDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        private readonly ShellViewModel vm = new ShellViewModel();

        public ShellView()
        {
            InitializeComponent();           
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var svm=e.NewValue as ShellViewModel;
            svm.Attach(this,plot1);
        }
       
    }
}