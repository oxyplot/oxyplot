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
        public Dictionary<int, Statistics> YearlyStatistics { get; set; }

        public Country()
        {
            YearlyStatistics = new Dictionary<int, Statistics>();
        }

        public double FindValue(int year, PropertyInfo property)
        {
            if (!YearlyStatistics.ContainsKey(year))
                return double.NaN;
            var stats = YearlyStatistics[year];
            return (double)property.GetValue(stats, null);
        }
    }

    public class Statistics
    {
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
