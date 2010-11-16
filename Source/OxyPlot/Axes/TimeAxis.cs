using System;
using System.Linq;

namespace OxyPlot
{
    public class TimeAxis : LinearAxis
    {
        public TimeAxis()
        {
            StringFormat = "m:ss";
        }

        public override string FormatValue(double x)
        {
            TimeSpan span = TimeSpan.FromSeconds(x);
            string s = ActualStringFormat;
            s = s.Replace("mm", span.Minutes.ToString("00"));
            s = s.Replace("ss", span.Seconds.ToString("00"));
            s = s.Replace("hh", span.Hours.ToString("00"));
            s = s.Replace("msec", span.Milliseconds.ToString("000"));
            s = s.Replace("m", ((int)(span.TotalMinutes)).ToString("0"));
            s = s.Replace("s", ((int)(span.TotalSeconds)).ToString("0"));
            s = s.Replace("h", ((int)(span.TotalHours)).ToString("0"));
            return s;
        }

        protected override double CalculateActualInterval(double availableSize, double maxIntervalSize)
        {
            double range = Math.Abs(ActualMinimum - ActualMaximum);
            double interval = 1;
            var goodIntervals = new[] { 1.0, 5, 10, 30, 60, 120, 300, 600, 900, 1200, 1800, 3600 };

            int maxSteps = 20;

            while (true)
            {
                if (range / interval < maxSteps)
                    return interval;
                double nextInterval = goodIntervals.FirstOrDefault(i => i > interval);
                if (nextInterval == 0)
                    nextInterval = interval * 2;
                interval = nextInterval;
            }
        }
    }
}