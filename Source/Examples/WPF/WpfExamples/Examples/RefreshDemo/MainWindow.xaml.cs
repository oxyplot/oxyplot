using System;
using System.Collections.Generic;
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

namespace RefreshDemo
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

        private void RefreshMethod1_Click(object sender, RoutedEventArgs e)
        {
            new Window1().Show();
        }
        private void RefreshMethod2_Click(object sender, RoutedEventArgs e)
        {
            new Window2().Show();
        }
        private void RefreshMethod3_Click(object sender, RoutedEventArgs e)
        {
            new Window3().Show();
        }

        private void RefreshMethod4_Click(object sender, RoutedEventArgs e)
        {
            new Window4().Show();
        }
    }
}
