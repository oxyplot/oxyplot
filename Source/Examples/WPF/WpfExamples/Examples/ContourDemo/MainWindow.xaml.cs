// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace ContourDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel vm = new MainViewModel();
        private MenuItem checkedItem = null;
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void FileExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ExampleClick(object sender, RoutedEventArgs e)
        {
            var mi = sender as MenuItem;
            if (checkedItem != null)
                checkedItem.IsChecked = false;
            mi.IsChecked = true;
            checkedItem = mi;
            vm.SelectedExample = mi.DataContext as MainViewModel.Example;
        }
    }
}