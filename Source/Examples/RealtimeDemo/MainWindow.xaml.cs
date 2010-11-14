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
using System.Windows.Threading;
using OxyPlot;

namespace RealtimeDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel vm = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = vm;

            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            vm.Update();

            // todo: problems with datacontext
            plot1.Series[0].ItemsSource = vm.Results1;
            plot1.Series[1].ItemsSource = vm.Results2;

            plot1.Refresh();
        }

    }
}
