// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Country.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WorldStatisticsDemo
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class Country
    {
        public string Name { get; set; }

        public Dictionary<int, Statistics> StatisticsByYear { get; set; }

        public List<Statistics> Statistics { get; set; }

        public Country(string name)
        {
            this.Name = name;
            this.StatisticsByYear = new Dictionary<int, Statistics>();
        }

        public override string ToString()
        {
            return Name;
        }

        public void SortStatistics()
        {
            var years = StatisticsByYear.Keys.ToList();
            years.Sort();
            Statistics=new List<Statistics>();
            foreach (var year in years)
            {
                Statistics.Add(StatisticsByYear[year]);
            }

            int i0GDP = int.MaxValue-1;
            int i0LEB = int.MaxValue-1;
            int i0POP = int.MaxValue-1;
            for (int i = 0; i < Statistics.Count; i++)
            {
                if (!double.IsNaN(Statistics[i].GdpPerCapitaPpp))
                {
                    if (i > i0GDP + 1)
                    {
                        var Si = Statistics[i];
                        var Si0 = Statistics[i0GDP];
                        for (int j=i0GDP+1;j<i;j++)
                        {
                            var Sj = Statistics[j];
                            Sj.GdpPerCapitaPpp = Lerp(Sj.Year, Si0.Year, Si.Year, Si0.GdpPerCapitaPpp, Si.GdpPerCapitaPpp);
                        }
                    }
                    i0GDP = i;
                }

                if (!double.IsNaN(Statistics[i].LifeExpectancyAtBirth))
                {
                    if (i > i0LEB + 1)
                    {
                        var Si = Statistics[i];
                        var Si0 = Statistics[i0LEB];
                        for (int j = i0LEB + 1; j < i; j++)
                        {
                            var Sj = Statistics[j];
                            Sj.LifeExpectancyAtBirth = Lerp(Sj.Year, Si0.Year, Si.Year,Si0.LifeExpectancyAtBirth,Si.LifeExpectancyAtBirth);
                        }
                    }
                    i0LEB = i;
                }

                if (!double.IsNaN(Statistics[i].Population))
                {
                    if (i > i0POP + 1)
                    {
                        var Si = Statistics[i];
                        var Si0 = Statistics[i0POP];
                        for (int j = i0POP + 1; j < i; j++)
                        {
                            var Sj = Statistics[j];
                            Sj.Population = Lerp(Sj.Year, Si0.Year, Si.Year, Si0.Population, Si.Population);
                        }
                    }
                    i0POP = i;
                }
            }
        }

        private double Lerp(double xj, double x0, double x1, double v0, double v1)
        {
            return v0 + (xj - x0) / (x1 - x0) * (v1 - v0);
        }

        public double FindValue(int year, PropertyInfo property)
        {
            if (this.StatisticsByYear.ContainsKey(year))
            {
                var stats = this.StatisticsByYear[year];
                double value=(double)property.GetValue(stats, null);
                if (!double.IsNaN(value)) return value;
            }
            return double.NaN;
            /*
            int previousYear = -10000;
            int followingYear = 10000;
            foreach (var y in StatisticsByYear.Keys)
            {
                var stats = this.StatisticsByYear[y];
                double va = (double)property.GetValue(stats, null);
                if (double.IsNaN(va)) continue;

                if (y < year && y > previousYear)
                    previousYear = y;
                if (y > year && y < followingYear)
                    followingYear = y;
            }
            if (previousYear==-10000 || followingYear==10000) return double.NaN;

            var stats0 = StatisticsByYear[previousYear];
            var stats1 = StatisticsByYear[followingYear];
            double v0 = (double)property.GetValue(stats0, null);
            double v1 = (double)property.GetValue(stats1, null);
            double f = (double)(year - previousYear) / (followingYear - previousYear);
            double v = v0 + f * (v1 - v0);
            return v;*/
        }
    }
}