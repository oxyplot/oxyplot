// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ContourDemo
{
    using System.Windows;
    using System.Windows.Controls;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Plotting with contour series.")]
    public partial class MainWindow : Window
    {
        private MainViewModel vm = new MainViewModel();

        private MenuItem checkedItem;

        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this.vm;
        }

        private void FileExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExampleClick(object sender, RoutedEventArgs e)
        {
            var mi = (MenuItem)sender;
            if (this.checkedItem != null)
            {
                this.checkedItem.IsChecked = false;
            }

            mi.IsChecked = true;
            this.checkedItem = mi;
            this.vm.SelectedExample = mi.DataContext as MainViewModel.Example;
        }
    }
}