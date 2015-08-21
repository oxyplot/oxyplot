using System;
using System.Collections.Generic;
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
using System.Threading;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FixIssue256
{
    public partial class MainWindow : Window
    {
        Issue256ViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.viewModel = new Issue256ViewModel();
            this.DataContext = this.viewModel;
        }

        private void NextTestButtonClick(object sender, RoutedEventArgs e)
        {
            this.viewModel.Next();
        }
    }

    internal class Issue256ViewModel : WpfNotifier
    {
        private int testIndex = 0;
        private const int testCount = 4;
        private PlotModel _heatMapPlotModel;
        public PlotModel heatMapPlotModel // WPF binds here
        {
            get { return _heatMapPlotModel; }
            set 
            {
                if (_heatMapPlotModel != value)
                {
                    _heatMapPlotModel = value;
                    NotifyPropertyChanged();
                }
            }
        }

        internal Issue256ViewModel()
        {
        }

        internal void Next()
        {
            if (testIndex == 0)
                this.heatMapPlotModel = ExampleLibrary.HeatMapSeriesExamples.InterpolatedWithNanValue();
            if (testIndex == 1)
                this.heatMapPlotModel = ExampleLibrary.HeatMapSeriesExamples.InterpolatedWithNanValue2();
            if (testIndex == 2)
                this.heatMapPlotModel = ExampleLibrary.HeatMapSeriesExamples.NotInterpolatedWithNanValue();
            if (testIndex == 3)
                this.heatMapPlotModel = ExampleLibrary.HeatMapSeriesExamples.NotInterpolatedWithNanValue2();
            testIndex++;
            if (testIndex == testCount)
                testIndex = 0;
        }
    }

    public class WpfNotifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; // public for interface

        internal void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
