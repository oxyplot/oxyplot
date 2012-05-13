using System.Windows;

namespace UserControlDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Model1 = new ViewModel { Title = "Plot1" };
            Model2 = new ViewModel { Title = "Plot2" };
            DataContext = this;
        }

        public ViewModel Model1 { get; set; }
        public ViewModel Model2 { get; set; }
    }
}
