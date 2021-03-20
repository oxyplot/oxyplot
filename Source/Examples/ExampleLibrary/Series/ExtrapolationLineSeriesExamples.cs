// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtrapolationLineSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides examples for the ExtrapolationLineSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary.Series
{
    using ExampleLibrary.Utilities;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Legends;
    using OxyPlot.Series;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides examples for the <see cref="ExtrapolatedLineSeries" />.
    /// </summary>
    [Examples("ExtrapolationLineSeries")]
    [Tags("Series")]
    public static class ExtrapolationLineSeriesExamples
    {
        /// <summary>
        /// Creates an example showing a line fit which is extrapolated
        /// beyond the range given by the data points.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Line Fit (Ignore Extrapolation For Scaling)")]
        public static PlotModel ExtrapolatedLineSeries()
        {
            var model = new PlotModel { Title = "Line Fit" };

            var scatterSeries = new ScatterSeries()
            {
                Title = "Data",
            };

            scatterSeries.Points.Add(new ScatterPoint(3, 1.4));
            scatterSeries.Points.Add(new ScatterPoint(4, 1.3));
            scatterSeries.Points.Add(new ScatterPoint(5, 1.6));
            scatterSeries.Points.Add(new ScatterPoint(6, 2.3));
            scatterSeries.Points.Add(new ScatterPoint(7, 2.2));
            scatterSeries.Points.Add(new ScatterPoint(8, 2.5));
            scatterSeries.Points.Add(new ScatterPoint(9, 2.9));
            scatterSeries.Points.Add(new ScatterPoint(10, 3.1));
            scatterSeries.Points.Add(new ScatterPoint(11, 3.1));
            scatterSeries.Points.Add(new ScatterPoint(12, 3.8));

            model.Series.Add(scatterSeries);

            CalculateLinearRegressionParameters(scatterSeries.Points, out double slope, out double intercept);

            var lineSeries = new ExtrapolationLineSeries
            {
                Title = "Fit",
                Color = OxyColors.Black,
                LineStyle = LineStyle.Solid,
                ExtrapolationColor = OxyColors.DarkGray,
                ExtrapolationLineStyle = LineStyle.Dash,
                StrokeThickness = 3,
                IgnoreExtraplotationForScaling = true,
            };

            lineSeries.Intervals.Add(new DataRange(double.NegativeInfinity, scatterSeries.Points.Select(p => p.X).Min()));
            lineSeries.Intervals.Add(new DataRange(scatterSeries.Points.Select(p => p.X).Max(), double.PositiveInfinity));

            var fitPoints = Enumerable
                .Range(-100, 200)
                .Select(x => new DataPoint(x, (slope * x) + intercept));

            lineSeries.Points.AddRange(fitPoints);
            model.Series.Add(lineSeries);

            var legend = new Legend
            {
                LegendPosition = LegendPosition.BottomRight,
            };

            model.Legends.Add(legend);

            return model;
        }

        /// <summary>
        /// Creates an example showing a third-order polynomial with extra- and interpolation style.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Interpolation")]
        public static PlotModel InterpolationStyleLineSeries()
        {
            var model = new PlotModel { Title = "Interpolation" };

            var lineSeries = new ExtrapolationLineSeries
            {
                Title = "Third Order Polynomial",
                Color = OxyColors.Black,
                LineStyle = LineStyle.Dash,
                ExtrapolationColor = OxyColors.Gray,
                ExtrapolationLineStyle = LineStyle.Dot,
                StrokeThickness = 3,
            };

            lineSeries.Intervals.Add(new DataRange(double.NegativeInfinity, -15));
            lineSeries.Intervals.Add(new DataRange(10, 30));
            lineSeries.Intervals.Add(new DataRange(55, double.PositiveInfinity));

            var coefficients = new double[] { 0.1, -6.0, -12, 0 };

            double PolynomialValue(double x, IEnumerable<double> coeff)
            {
                // Horner's schema
                return coeff.Aggregate((acc, coefficient) => (acc * x) + coefficient);
            }

            var points = Enumerable
                .Range(-30, 100)
                .Select(x => new DataPoint(x, PolynomialValue(x, coefficients)));

            lineSeries.Points.AddRange(points);

            model.Series.Add(lineSeries);

            var legend = new Legend
            {
                LegendPosition = LegendPosition.TopCenter,
            };

            model.Legends.Add(legend);

            return model;
        }

        /// <summary>
        /// Creates an example showing a third-order polynomial with extra- and
        /// interpolation style and an inverted y-axis.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Interpolation (Y Axis reversed)")]
        public static PlotModel TwoColorLineSeriesReversed()
        {
            return InterpolationStyleLineSeries().ReverseYAxis();
        }

        /// <summary>
        /// Creates an example where the provided extrapolation
        /// intervals overlap with each other.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Intersecting Intervals")]
        public static PlotModel IntersectingIntervals()
        {
            var model = new PlotModel { Title = "Intersecting Intervals" };

            var i1 = new DataRange(-20, 20);
            var i2 = new DataRange(0, 30);
            var i3 = new DataRange(10, 40);

            var lineSeries = new ExtrapolationLineSeries
            {
                Title = $"Overlapping intervals {i1}, {i2}, {i3}",
                Color = OxyColors.Black,
                LineStyle = LineStyle.Solid,
                ExtrapolationColor = OxyColors.Gray,
                ExtrapolationLineStyle = LineStyle.Dot,
                StrokeThickness = 3,
            };

            lineSeries.Intervals.Add(i1);
            lineSeries.Intervals.Add(i2);
            lineSeries.Intervals.Add(i3);

            var coefficients = new double[] { 0.1, -6.0, -12, 0 };

            double PolynomialValue(double x, IEnumerable<double> coeff)
            {
                // Horner's schema
                return coeff.Aggregate((acc, coefficient) => (acc * x) + coefficient);
            }

            var points = Enumerable
                .Range(-30, 100)
                .Select(x => new DataPoint(x, PolynomialValue(x, coefficients)));

            lineSeries.Points.AddRange(points);

            model.Series.Add(lineSeries);

            var legend = new Legend
            {
                LegendPosition = LegendPosition.TopCenter,
            };

            model.Legends.Add(legend);

            return model;
        }

        /// <summary>
        /// Creates an example showing a line using custom dash arrays for the
        /// normal and extrapolated parts of the curve.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Custom Dashes")]
        public static PlotModel CustomDashes()
        {
            var model = new PlotModel { Title = "Custom Dashes" };

            var lineSeries = new ExtrapolationLineSeries
            {
                Title = "y = 5",
                Color = OxyColors.Black,
                LineStyle = LineStyle.Dash,
                Dashes = new double[] { 5, 1 },
                ExtrapolationColor = OxyColors.Gray,
                ExtrapolationLineStyle = LineStyle.Dot,
                ExtrapolationDashes = new double[] { 1, 5 },
                StrokeThickness = 3,
            };

            lineSeries.Intervals.Add(new DataRange(double.NegativeInfinity, 0));

            var points = Enumerable
                .Range(-100, 200)
                .Select(x => new DataPoint(x, 5));

            lineSeries.Points.AddRange(points);

            model.Series.Add(lineSeries);

            var legend = new Legend
            {
                LegendPosition = LegendPosition.TopCenter,
            };

            model.Legends.Add(legend);

            return model;
        }

        /// <summary>
        /// Creates an example to test the performance with 100000 points and 100 intervals.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Many Intervals")]
        public static PlotModel ManyIntervals()
        {
            var model = new PlotModel { Title = "ManyIntervals" };

            var lineSeries = new ExtrapolationLineSeries
            {
                Title = "y = x",
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid,
                ExtrapolationLineStyle = LineStyle.Solid,
                StrokeThickness = 3,
                IgnoreExtraplotationForScaling = true,
            };

            lineSeries.Intervals.Add(new DataRange(-1000, 10_000));

            var intervals = Enumerable
                .Range(0, 98)
                .Select(x => new DataRange(1000 * x, (1000 * x) + 500));

            foreach(var interval in intervals)
                lineSeries.Intervals.Add(interval);

            lineSeries.Intervals.Add(new DataRange(200_000, double.PositiveInfinity));

            var points = Enumerable
                .Range(0, 100_000)
                .Select(x => new DataPoint(x, x));

            lineSeries.Points.AddRange(points);

            model.Series.Add(lineSeries);

            var legend = new Legend
            {
                LegendPosition = LegendPosition.TopCenter,
            };

            model.Legends.Add(legend);

            return model;
        }

        /// <summary>
        /// Creates an example where Moore's law is fitted and
        /// extrapolated to the future.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Moore's Law")]
        public static PlotModel MooresLaw()
        {
            var model = new PlotModel { Title = "Moore's Law" };

            var scatterSeries = new ScatterSeries()
            {
                Title = "Data",
            };

            scatterSeries.Points.AddRange(GetPointForMooresLaw());

            model.Series.Add(scatterSeries);

            model.Axes.Add(new LinearAxis { Title = "Year", Position = AxisPosition.Bottom });
            model.Axes.Add(new LogarithmicAxis { Title = "Transistors (in thousands)", Position = AxisPosition.Left });

            CalculateLinearRegressionParameters(scatterSeries.Points.Select(p => new ScatterPoint(p.X, Math.Log10(p.Y))), out double slope, out double intercept);

            var lineSeries = new ExtrapolationLineSeries
            {
                Title = "Fit and Extrapolation",
                Color = OxyColors.Black,
                LineStyle = LineStyle.Solid,
                ExtrapolationColor = OxyColors.Blue,
                ExtrapolationLineStyle = LineStyle.Dot,
                StrokeThickness = 3,
            };

            lineSeries.Intervals.Add(new DataRange(2015, double.PositiveInfinity));

            var fitPoints = Enumerable
                .Range(1970, 55)
                .Select(x => new DataPoint(x, Math.Pow(10, (slope * x) + intercept)));

            lineSeries.Points.AddRange(fitPoints);

            model.Series.Add(lineSeries);

            var legend = new Legend
            {
                LegendPosition = LegendPosition.TopCenter,
            };

            model.Legends.Add(legend);

            return model;
        }

        /// <summary>
        /// Sets the slope and intercept of a linear regression line through the provided points.
        /// </summary>
        private static void CalculateLinearRegressionParameters(IEnumerable<ScatterPoint> points, out double slope, out double intercept)
        {
            if (points == null)
            {
                throw new ArgumentNullException(nameof(points));
            }

            if (points.Count() < 2)
            {
                throw new ArgumentException("at least two points required", nameof(points));
            }

            var meanX = points.Select(p => p.X).Average();
            var meanY = points.Select(p => p.Y).Average();

            var cov = Covariance(points, meanX, meanY);
            var var2_x = Variance2(points.Select(p => p.X));

            slope = cov / var2_x;
            intercept = meanY - (slope * meanX);
        }

        /// <summary>
        /// Returns the covariance between the points x and y values.
        /// </summary>
        private static double Covariance(IEnumerable<ScatterPoint> points, double meanX, double meanY)
        {
            var res = points.Sum(p => p.X * p.Y);

            res -= points.Count() * meanX * meanY;
            res /= points.Count() - 1;

            return res;
        }

        /// <summary>
        /// Returns the squared variance of a quantity.
        /// </summary>
        private static double Variance2(IEnumerable<double> values)
        {
            var mean = values.Average();

            var res = values.Sum(x => x * x);

            res -= values.Count() * mean * mean;
            res /= values.Count() - 1;

            return res;
        }

        /// <summary>
        /// Returns data points to demonstrate Moore's law.
        /// Source: https://www.karlrupp.net/2015/06/40-years-of-microprocessor-trend-data/.
        /// </summary>
        private static IEnumerable<ScatterPoint> GetPointForMooresLaw()
        {
            yield return new ScatterPoint(1971.875, 2.30824152676);
            yield return new ScatterPoint(1972.30769231, 3.55452235561);
            yield return new ScatterPoint(1974.32692308, 6.09756235221);
            yield return new ScatterPoint(1979.56730769, 29.1637757405);
            yield return new ScatterPoint(1982.30769231, 135.772714211);
            yield return new ScatterPoint(1985.91346154, 273.841963426);
            yield return new ScatterPoint(1986.25, 109.411381058);
            yield return new ScatterPoint(1988.65384615, 121.881418484);
            yield return new ScatterPoint(1989.47115385, 1207.90074743);
            yield return new ScatterPoint(1990.57692308, 1207.90074743);
            yield return new ScatterPoint(1992.40384615, 1207.90074743);
            yield return new ScatterPoint(1992.69230769, 3105.90022362);
            yield return new ScatterPoint(1992.69230769, 1113.97385999);
            yield return new ScatterPoint(1993.02884615, 1715.43789634);
            yield return new ScatterPoint(1993.41346154, 3105.90022362);
            yield return new ScatterPoint(1993.41346154, 922.239565104);
            yield return new ScatterPoint(1994.71153846, 1910.95297497);
            yield return new ScatterPoint(1994.71153846, 2788.12666541);
            yield return new ScatterPoint(1995.43269231, 9646.61619911);
            yield return new ScatterPoint(1995.72115385, 3105.90022362);
            yield return new ScatterPoint(1996.15384615, 5473.70326288);
            yield return new ScatterPoint(1996.34615385, 6792.52507006);
            yield return new ScatterPoint(1996.34615385, 3651.74127255);
            yield return new ScatterPoint(1996.44230769, 4293.51021008);
            yield return new ScatterPoint(1996.82692308, 9646.61619911);
            yield return new ScatterPoint(1997.35576923, 5473.70326288);
            yield return new ScatterPoint(1997.45192308, 3554.52235561);
            yield return new ScatterPoint(1997.54807692, 8896.4911282);
            yield return new ScatterPoint(1997.64423077, 7566.6953714);
            yield return new ScatterPoint(1998.89423077, 15261.3780258);
            yield return new ScatterPoint(1999.13461538, 9389.79801048);
            yield return new ScatterPoint(1999.18269231, 6978.3058486);
            yield return new ScatterPoint(1999.47115385, 9389.79801048);
            yield return new ScatterPoint(1999.47115385, 21673.9216957);
            yield return new ScatterPoint(2000.19230769, 22266.7201035);
            yield return new ScatterPoint(2000.67307692, 28387.3596476);
            yield return new ScatterPoint(2000.67307692, 37180.2666391);
            yield return new ScatterPoint(2001.10576923, 29163.7757405);
            yield return new ScatterPoint(2001.20192308, 42550.6550247);
            yield return new ScatterPoint(2001.68269231, 25482.9674798);
            yield return new ScatterPoint(2001.82692308, 37180.2666391);
            yield return new ScatterPoint(2002.40384615, 55730.6040127);
            yield return new ScatterPoint(2002.78846154, 38197.1754928);
            yield return new ScatterPoint(2002.88461538, 220673.406908);
            yield return new ScatterPoint(2003.41346154, 151247.254531);
            yield return new ScatterPoint(2003.50961538, 54246.9093701);
            yield return new ScatterPoint(2003.65384615, 106498.563535);
            yield return new ScatterPoint(2004.80769231, 125214.968907);
            yield return new ScatterPoint(2004.80769231, 106498.563535);
            yield return new ScatterPoint(2004.80769231, 273841.963426);
            yield return new ScatterPoint(2005.72115385, 232909.659246);
            yield return new ScatterPoint(2005.81730769, 112403.866377);
            yield return new ScatterPoint(2005.96153846, 305052.789027);
            yield return new ScatterPoint(2006.05769231, 115478.198469);
            yield return new ScatterPoint(2006.875, 378551.524926);
            yield return new ScatterPoint(2006.875, 155383.983127);
            yield return new ScatterPoint(2006.92307692, 245824.406892);
            yield return new ScatterPoint(2006.97115385, 296931.48482);
            yield return new ScatterPoint(2006.97115385, 582941.534714);
            yield return new ScatterPoint(2007.06730769, 151247.254531);
            yield return new ScatterPoint(2007.64423077, 582941.534714);
            yield return new ScatterPoint(2007.74038462, 232909.659246);
            yield return new ScatterPoint(2007.78846154, 805842.187761);
            yield return new ScatterPoint(2007.83653846, 115478.198469);
            yield return new ScatterPoint(2007.98076923, 509367.521678);
            yield return new ScatterPoint(2008.22115385, 445079.406236);
            yield return new ScatterPoint(2008.46153846, 410469.838044);
            yield return new ScatterPoint(2009.18269231, 457252.669897);
            yield return new ScatterPoint(2009.27884615, 784388.558145);
            yield return new ScatterPoint(2009.66346154, 2308241.52676);
            yield return new ScatterPoint(2009.71153846, 1910952.97497);
            yield return new ScatterPoint(2010.19230769, 410469.838044);
            yield return new ScatterPoint(2010.38461538, 1309747.2643);
            yield return new ScatterPoint(2011.16, 1170000);
            yield return new ScatterPoint(2011.32, 2600000);
            yield return new ScatterPoint(2011.9, 1200000);
            yield return new ScatterPoint(2012.40, 2400000);
            yield return new ScatterPoint(2012.41, 2300000);
            yield return new ScatterPoint(2012.7, 2100000);
            yield return new ScatterPoint(2012.9, 1200000);
            yield return new ScatterPoint(2013.4, 5000000);
            yield return new ScatterPoint(2013.8, 4300000);
            yield return new ScatterPoint(2014.16, 4300000);
            yield return new ScatterPoint(2014.5, 4200000);
            yield return new ScatterPoint(2014.8, 2600000);
            yield return new ScatterPoint(2014.81, 3800000);
            yield return new ScatterPoint(2014.82, 5700000);
        }
    }
}
