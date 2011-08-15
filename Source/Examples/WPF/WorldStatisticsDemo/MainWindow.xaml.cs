using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace WorldStatisticsDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }

    public class Country
    {
        public Dictionary<int, Statistics> StatisticsByYear { get; set; }

        public List<Statistics> Statistics { get; set; }

        public Country()
        {
            this.StatisticsByYear = new Dictionary<int, Statistics>();
        }

        public void SortStatistics()
        {
            
        }

        public double FindValue(int year, PropertyInfo property)
        {
            if (!this.StatisticsByYear.ContainsKey(year))
                return double.NaN;
            var stats = this.StatisticsByYear[year];
            return (double)property.GetValue(stats, null);
        }
    }

    public class Statistics
    {
        public int Year { get; set; }
        public double GdpPerCapitaPpp { get; set; }
        public double LifeExpectancyAtBirth { get; set; }
        public double Population { get; set; }

        public Statistics()
        {
            GdpPerCapitaPpp = double.NaN;
            LifeExpectancyAtBirth = double.NaN;
            Population = double.NaN;
        }
    }
}
