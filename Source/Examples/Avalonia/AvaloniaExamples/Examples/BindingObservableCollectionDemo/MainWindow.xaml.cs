// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples.Examples.BindingObservableCollectionDemo
{
    using System.Windows;

    using AvaloniaExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Demonstrates data binding with an observable collection.")]
    public partial class MainWindow : Avalonia.Controls.Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            var vm = new BindingObservableCollectionDemo.MainViewModel();
            this.DataContext = vm;
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}