using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace LiveDemo
{
    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Visibility example")]
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        Random rnd = new Random();
        double time = 0d;


        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Data.Add(new DataPoint(time++, rnd.NextDouble() * 10));
        }

        public ObservableCollection<DataPoint> Data
        {
            get { return (ObservableCollection<DataPoint>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ObservableCollection<DataPoint>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<DataPoint>()));


        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            timer.IsEnabled = (sender as CheckBox)?.IsChecked ?? false;
            if (timer.IsEnabled)
            {
                // Reset
                time = 0d;
                Data = new ObservableCollection<DataPoint>();
            }
        }
    }
}
