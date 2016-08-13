// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples.Examples.ColorBindingDemo
{
    using System.Windows;

    using AvaloniaExamples;
    using OxyPlot.Avalonia.Converters;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Binds fore and background colors.")]
    public partial class MainWindow : Avalonia.Controls.Window
    {
        public static readonly OxyColorConverter OxyColorConverter = new OxyColorConverter();

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}