// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CandleStickSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
	using System.Linq;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("FastCandleStickSeries"), Tags("Series")]
    public static class FastCandleStickSeriesExamples
    {
		[Example("Large Data Set (wide window)")]
		public static Example LargeDataSet_Wide ()
		{
			var pm = new PlotModel { Title = "Large Data Set (wide window)" };

			var timeSpanAxis1 = new DateTimeAxis { Position = AxisPosition.Bottom };
			pm.Axes.Add(timeSpanAxis1);
			var linearAxis1 = new LinearAxis { Position = AxisPosition.Left };
			pm.Axes.Add(linearAxis1);
			var series = new FastCandleStickSeries
			{
				Color = OxyColors.Black,
				IncreasingColor = OxyColors.DarkGreen,
				DecreasingColor = OxyColors.Red,
				DataFieldX = "Time",
				DataFieldHigh = "H",
				DataFieldLow = "L",
				DataFieldOpen = "O",
				DataFieldClose = "C",
				TrackerFormatString = "High: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}",
			};

			var n = 1000000;
			foreach (var bar in BarGenerator.MRProcess (n))
			{
				series.Append (bar);
			}

			timeSpanAxis1.Minimum = series.Items [n - 200].X;
			timeSpanAxis1.Maximum = series.Items [n-130].X;

			linearAxis1.Minimum = series.Items.Skip (n - 200).Take(70).Select (x => x.Low).Min();
			linearAxis1.Maximum = series.Items.Skip (n - 200).Take(70).Select (x => x.High).Max();

			pm.Series.Add(series);

			timeSpanAxis1.AxisChanged += (sender, e) =>  AdjustYExtent (series, timeSpanAxis1, linearAxis1);

			var controller = new PlotController();
			controller.InputCommandBindings.Clear();
			controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
			return new Example(pm, controller);
		}


		[Example("Large Data Set (narrow window)")]
		public static Example LargeDataSet_Narrow ()
		{
			var pm = new PlotModel { Title = "Large Data Set (narrow window)" };

			var timeSpanAxis1 = new DateTimeAxis { Position = AxisPosition.Bottom };
			pm.Axes.Add(timeSpanAxis1);
			var linearAxis1 = new LinearAxis { Position = AxisPosition.Left };
			pm.Axes.Add(linearAxis1);
			var series = new FastCandleStickSeries
			{
				Color = OxyColors.Black,
				IncreasingColor = OxyColors.DarkGreen,
				DecreasingColor = OxyColors.Red,
				DataFieldX = "Time",
				DataFieldHigh = "H",
				DataFieldLow = "L",
				DataFieldOpen = "O",
				DataFieldClose = "C",
				TrackerFormatString = "High: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}",
			};

			var n = 1000000;
			foreach (var bar in BarGenerator.MRProcess (n))
			{
				series.Append (bar);
			}

			timeSpanAxis1.Minimum = series.Items [0].X;
			timeSpanAxis1.Maximum = series.Items [29].X;

			linearAxis1.Minimum = series.Items.Take(30).Select (x => x.Low).Min();
			linearAxis1.Maximum = series.Items.Take(30).Select (x => x.High).Max();

			pm.Series.Add(series);

			timeSpanAxis1.AxisChanged += (sender, e) =>  AdjustYExtent (series, timeSpanAxis1, linearAxis1);

			var controller = new PlotController();
			controller.InputCommandBindings.Clear();
			controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
			return new Example(pm, controller);
		}


		[Example("Small Set")]
		public static Example SmallDataSet ()
		{
			var pm = new PlotModel { Title = "Small Data Set" };

			var timeSpanAxis1 = new DateTimeAxis { Position = AxisPosition.Bottom };
			pm.Axes.Add(timeSpanAxis1);
			var linearAxis1 = new LinearAxis { Position = AxisPosition.Left };
			pm.Axes.Add(linearAxis1);
			var series = new FastCandleStickSeries
			{
				Color = OxyColors.Black,
				IncreasingColor = OxyColors.DarkGreen,
				DecreasingColor = OxyColors.Red,
				DataFieldX = "Time",
				DataFieldHigh = "H",
				DataFieldLow = "L",
				DataFieldOpen = "O",
				DataFieldClose = "C",
				TrackerFormatString = "High: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}",
			};

			var n = 100;
			foreach (var bar in BarGenerator.MRProcess (n))
			{
				series.Append (bar);
			}

			pm.Series.Add(series);

			timeSpanAxis1.AxisChanged += (sender, e) =>  AdjustYExtent (series, timeSpanAxis1, linearAxis1);			

			var controller = new PlotController();
			controller.InputCommandBindings.Clear();
			controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
			return new Example(pm, controller);
		}


		/// <summary>
		/// Adjusts the Y extent.
		/// </summary>
		/// <param name="series">Series.</param>
		/// <param name="xaxis">Xaxis.</param>
		/// <param name="yaxis">Yaxis.</param>
		private static void AdjustYExtent (FastCandleStickSeries series, DateTimeAxis xaxis, LinearAxis yaxis)
		{
			var xmin = xaxis.ActualMinimum;
			var xmax = xaxis.ActualMaximum;

			var istart = series.FindByX (xmin);
			var iend = series.FindByX (xmax, istart);

			var ymin = double.MaxValue;
			var ymax = double.MinValue;
			for (int i = istart; i <= iend; i++)
			{
				var bar = series.Items [i];
				ymin = Math.Min (ymin, bar.Low);
				ymax = Math.Max (ymax, bar.High);
			}

			var extent = (ymax - ymin);
			var margin = extent * 0.10;

			yaxis.Zoom (ymin - margin, ymax + margin);
		}
    }


	/// <summary>
	/// Creates realistic bars
	/// </summary>
	public static class BarGenerator
	{
		/// <summary>
		/// Create bars governed by a MR process
		/// </summary>
		/// <returns>The process.</returns>
		/// <param name="x0">X0.</param>
		/// <param name="csigma">Csigma.</param>
		/// <param name="esigma">Esigma.</param>
		/// <param name="kappa">Kappa.</param>
		public static IEnumerable<HighLowItem> MRProcess (
			int n,
			double x0 = 100.0,
			double csigma = 0.50, 
			double esigma = 0.70, 
			double kappa = 0.01)
		{
			double x = x0;

			var Tbase = DateTime.UtcNow;
			for (int ti = 0 ; ti < n ; ti++)
			{
				var dx_c = -kappa * (x - x0) + RandomNormal (0, csigma);
				var dx_1 = -kappa * (x - x0) + RandomNormal (0, esigma);
				var dx_2 = -kappa * (x - x0) + RandomNormal (0, esigma);

				var open = x;
				var close = x = x + dx_c;
				var low = Min (open, close, open + dx_1, open + dx_2);
				var high = Max (open, close, open + dx_1, open + dx_2);

				var Tnow = Tbase.AddSeconds(ti);
				var t = DateTimeAxis.ToDouble (Tnow);
				yield return new HighLowItem (t, high, low, open, close);
			}
		}



		/// <summary>
		/// Minimum of the specified a, b, c and d.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B.</param>
		/// <param name="c">C.</param>
		/// <param name="d">D.</param>
		private static double Min (double a, double b, double c, double d)
		{
			return Math.Min (a, Math.Min (b, Math.Min (c, d)));
		}


		/// <summary>
		/// Maximum of the specified a, b, c and d.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">B.</param>
		/// <param name="c">C.</param>
		/// <param name="d">D.</param>
		private static double Max (double a, double b, double c, double d)
		{
			return Math.Max (a, Math.Max (b, Math.Max (c, d)));
		}


		/// <summary>
		/// Get random normal
		/// </summary>
		/// <param name="mu">Mu.</param>
		/// <param name="sigma">Sigma.</param>
		private static double RandomNormal (double mu, double sigma)
		{
			return InverseCumNormal (_rand.NextDouble (), mu, sigma);
		}


		/// <summary>
		/// fast approximation for inverse cum normal
		/// </summary>
		/// <param name="u">probability</param>
		/// <param name="mu">Mean</param>
		/// <param name="sigma">std dev</param>
		private static double InverseCumNormal (double p, double mu, double sigma)
		{
			const double a1_ = -3.969683028665376e+01;
			const double a2_ =  2.209460984245205e+02;
			const double a3_ = -2.759285104469687e+02;
			const double a4_ =  1.383577518672690e+02;
			const double a5_ = -3.066479806614716e+01;
			const double a6_ =  2.506628277459239e+00;

			const double b1_ = -5.447609879822406e+01;
			const double b2_ =  1.615858368580409e+02;
			const double b3_ = -1.556989798598866e+02;
			const double b4_ =  6.680131188771972e+01;
			const double b5_ = -1.328068155288572e+01;

			const double c1_ = -7.784894002430293e-03;
			const double c2_ = -3.223964580411365e-01;
			const double c3_ = -2.400758277161838e+00;
			const double c4_ = -2.549732539343734e+00;
			const double c5_ =  4.374664141464968e+00;
			const double c6_ =  2.938163982698783e+00;

			const double d1_ =  7.784695709041462e-03;
			const double d2_ =  3.224671290700398e-01;
			const double d3_ =  2.445134137142996e+00;
			const double d4_ =  3.754408661907416e+00;

			const double x_low_ = 0.02425;
			const double x_high_ = 1.0 - x_low_;

			double z,r;

			if (p < x_low_) 
			{
				// Rational approximation for the lower region 0<x<u_low
				z = Math.Sqrt(-2.0*Math.Log(p));
				z = (((((c1_*z+c2_)*z+c3_)*z+c4_)*z+c5_)*z+c6_) /
					((((d1_*z+d2_)*z+d3_)*z+d4_)*z+1.0);
			} 
			else if (p <= x_high_) 
			{
				// Rational approximation for the central region u_low<=x<=u_high
				z = p - 0.5;
				r = z*z;
				z = (((((a1_*r+a2_)*r+a3_)*r+a4_)*r+a5_)*r+a6_)*z /
					(((((b1_*r+b2_)*r+b3_)*r+b4_)*r+b5_)*r+1.0);
			} 
			else 
			{
				// Rational approximation for the upper region u_high<x<1
				z = Math.Sqrt(-2.0*Math.Log(1.0-p));
				z = -(((((c1_*z+c2_)*z+c3_)*z+c4_)*z+c5_)*z+c6_) /
					((((d1_*z+d2_)*z+d3_)*z+d4_)*z+1.0);
			}

			// error (f_(z) - x) divided by the cumulative's derivative
			r = (CumN0 (z) - p) * Math.Sqrt (2.0) * Math.Sqrt(Math.PI) * Math.Exp(0.5 * z*z);

			//  Halley's method
			z -= r/(1+0.5*z*r);

			return mu + z * sigma;

		}



		/// <summary>
		/// Cumulative for a N(0,1) distribution
		/// </summary>
		/// <returns>The n0.</returns>
		/// <param name="x">The x coordinate.</param>
		private static double CumN0 (double x)
		{
			const double b1 =  0.319381530;
			const double b2 = -0.356563782;
			const double b3 =  1.781477937;
			const double b4 = -1.821255978;
			const double b5 =  1.330274429;
			const double p  =  0.2316419;
			const double c  =  0.39894228;

			if (x >= 0.0) 
			{
				double t = 1.0 / ( 1.0 + p * x );
				return (1.0 - c * Math.Exp( -x * x / 2.0 ) * t *
					( t *( t * ( t * ( t * b5 + b4 ) + b3 ) + b2 ) + b1 ));
			}
			else 
			{
				double t = 1.0 / ( 1.0 - p * x );
				return ( c * Math.Exp( -x * x / 2.0 ) * t *
					( t *( t * ( t * ( t * b5 + b4 ) + b3 ) + b2 ) + b1 ));
			}
		}


		// Variables

		static Random		_rand = new Random ();
	}

}