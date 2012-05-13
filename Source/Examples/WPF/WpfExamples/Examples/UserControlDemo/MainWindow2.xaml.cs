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
using System.Windows.Shapes;

namespace UserControlDemo
{
    using OxyPlot;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            InitializeComponent();
            Models = new List<ViewModel>();
            Models.Add(new ViewModel { Title = "Plot1" });
            Models.Add(new ViewModel { Title = "Plot2" });
            DataContext = this;
        }

        public IList<ViewModel> Models { get; set; }
    }
}
