// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples.Examples.ContourDemo
{
    using Avalonia.Interactivity;
    using Avalonia.Controls;

    using AvaloniaExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Plotting with contour series.")]
    public partial class MainWindow : Avalonia.Controls.Window
    {
        private MainViewModel vm = new MainViewModel();
        
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this.vm;
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
            this.FindNameScope().Get<MenuItem>("Exit").Click += FileExit;
            this.FindNameScope().Get<MenuItem>("ExamplesMenu").AddHandler(MenuItem.ClickEvent, ExampleClick);
        }

        private void FileExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExampleClick(object sender, RoutedEventArgs e)
        {
            var mi = (Avalonia.Controls.MenuItem)e.Source;
            
            this.vm.SelectedExample = mi.DataContext as MainViewModel.Example;
        }
    }
}