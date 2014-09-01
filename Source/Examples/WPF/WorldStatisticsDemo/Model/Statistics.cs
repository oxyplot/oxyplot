// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Statistics.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WorldStatisticsDemo
{
    public class Statistics
    {
        public int Year { get; set; }
        public double GdpPerCapitaPpp { get; set; }
        public double LifeExpectancyAtBirth { get; set; }
        public double Population { get; set; }

        public Statistics(int year)
        {
            Year = year;
            GdpPerCapitaPpp = double.NaN;
            LifeExpectancyAtBirth = double.NaN;
            Population = double.NaN;
        }
    }
}