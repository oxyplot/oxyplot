// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Country.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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