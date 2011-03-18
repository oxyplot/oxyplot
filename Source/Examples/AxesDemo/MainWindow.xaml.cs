using System.Windows;

namespace AxesDemo
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
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            /*foreach (var a in plot1.Axes)
                a.GridlineType = vm.GridlineType;

            plot1.Invalidate();*/
        }

        private void Angle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (AnglePlot1 != null)
            {
                // TODO: binding should handle this
                AnglePlot1.Axes[0].Angle = Angle.Value;
                AnglePlot1.Axes[1].Angle = Angle.Value;
                AnglePlot1.InvalidatePlot();
            }
            if (AnglePlot2 != null)
            {
                // TODO: binding should handle this
                AnglePlot2.Axes[0].Angle = Angle.Value;
                AnglePlot2.Axes[1].Angle = Angle.Value;
                AnglePlot2.InvalidatePlot();
            }
        }
    }
}
