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


namespace aux
{
	/// <summary>
	/// EMA on a timeseries, defined by impulse half-life window N
	/// <code>
	/// 	mu = 2 / (n+1)
	/// 	EMA[t] = EMA[t-1] + mu (V[t] - EMA[t-1])
	/// </code>
	/// </summary>
	public class EMA
	{
		/// <summary>
		/// Create an EWMA
		/// </summary>
		/// <param name='n'>
		/// impulse half-life window N
		/// </param>
		public EMA (double n)
		{
			N = n;
		}


		// Properties

		/// <summary>
		/// Smoothness parameter, an approximate measure of impulse half-life
		/// </summary>
		public double N
			{ get; set; }

		/// <summary>
		/// Window (before which is not fully primed)
		/// </summary>
		public double Window
			{ get { return N * 2.772588722239781; } }


		// Operations


		/// <summary>
		/// Evaluate EMA, given next tick
		/// </summary>
		/// <param name='x'>
		/// Value
		/// </param>
		public double Tick (double x)
		{
			var mu = 2.0 / (N + 1);

			_ema += mu * (x - _ema);
			return (++_t >= Window) ? _ema : double.NaN;
		}
			

		/// <summary>
		/// Reset EWMA state
		/// </summary>
		public void Reset ()
		{
			_ema = 0;
			_t = 0;
		}

		// Variables

		private int 		_t;
		private double		_ema;
	}
}

