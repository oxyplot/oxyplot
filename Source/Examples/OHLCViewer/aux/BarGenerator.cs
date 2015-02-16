//
// 	Authors
// 		Jonathan Shore
//
// 	Copyright:
// 		2015 Systematic Trading LLC
//
// 		This software is only to be used for the purpose for which
// 		it has been provided.  No part of it is to be reproduced,
// 		disassembled, transmitted, stored in a retrieval system nor
// 		translated in any human or computer language in any way or
// 		for any other purposes whatsoever without the prior written
// 		consent of Systematic Trading LLC
//
//
using System;
using System.Collections.Generic;
using OxyPlot.Series;


namespace OHLCViewer
{
	/// <summary>
	/// Generates plausible OHLC + volume bars
	/// </summary>
	public class BarGenerator
	{
		/// <summary>
		/// Create bars governed by a MR process
		/// </summary>
		/// <returns>The process.</returns>
		/// <param name="Tstart">Tstart.</param>
		/// <param name="Tend">Tend.</param>
		/// <param name="Tdt">Tdt.</param>
		/// <param name="x0">X0.</param>
		/// <param name="csigma">Csigma.</param>
		/// <param name="esigma">Esigma.</param>
		/// <param name="kappa">Kappa.</param>
		public static IEnumerable<OHLCVItem> MRProcess (
			long Tstart = 1423872000000,
			long Tend = 1423872000000 + 24*3600*1000L,
			long Tdt = 60000,
			double x0 = 100.0,
			double v0 = 500,
			double csigma = 0.50, 
			double esigma = 0.75, 
			double kappa = 0.01)
		{
			double x = x0;
			for (var t = Tstart ; t <= Tend ; t += Tdt)
			{
				var dx_c = -kappa * (x - x0) + RandomNormal (0, csigma);
				var dx_1 = -kappa * (x - x0) + RandomNormal (0, esigma);
				var dx_2 = -kappa * (x - x0) + RandomNormal (0, esigma);

				var open = x;
				var close = x = x + dx_c;
				var low = Min (open, close, open + dx_1, open + dx_2);
				var high = Max (open, close, open + dx_1, open + dx_2);

				var dp = (close - open);
				var v = v0 * Math.Exp (Math.Abs (dp) / csigma);
				var dir = (dp < 0) ?
					-Math.Min (-dp / esigma, 1.0) :
					Math.Min (dp / esigma, 1.0);

				var skew = (dir + 1) / 2.0;
				var buyvol = skew * v;
				var sellvol = (1 - skew) * v;

				yield return new OHLCVItem (t, open, high, low, close, buyvol, sellvol);
			}
		}



		private static double Min (double a, double b, double c, double d)
		{
			return Math.Min (a, Math.Min (b, Math.Min (c, d)));
		}

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
		/// inverse cum normal
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

