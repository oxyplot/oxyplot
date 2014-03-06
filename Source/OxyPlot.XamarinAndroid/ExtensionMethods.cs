// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="OxyPlot">
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

namespace OxyPlot.XamarinAndroid
{
	using System;
	using Android.Graphics;
	using Android.Views;

	/// <summary>
	/// Provides extension methods that converts between Android types and OxyPlot types.
	/// </summary>
	public static class ExtensionMethods
	{
		/// <summary>
		/// Converts an <see cref="OxyColor"/> to a <see cref="Color"/>.
		/// </summary>
		/// <param name="color">The color to convert.</param>
		/// <returns>The converted color.</returns>
		public static Color ToColor (this OxyColor color)
		{
			return color.IsInvisible () ? Color.Transparent : new Color (color.R, color.G, color.B, color.A);
		}

		/// <summary>
		/// Converts an <see cref="OxyRect"/> to a <see cref="RectF"/>.
		/// </summary>
		/// <param name="rect">The rectangle to convert.</param>
		/// <returns>The converted rectangle.</returns>
		public static RectF ToRectF (this OxyRect rect)
		{
			return new RectF ((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom);
		}

		/// <summary>
		/// Converts an <see cref="OxyPenLineJoin"/> to a <see cref="Paint.Join"/>.
		/// </summary>
		/// <param name="join">The join value to convert.</param>
		/// <returns>The converted join value.</returns>
		public static Paint.Join Convert (this OxyPenLineJoin join)
		{
			switch (join) {
			case OxyPenLineJoin.Bevel:
				return Paint.Join.Bevel;
			case OxyPenLineJoin.Miter:
				return Paint.Join.Miter;
			case OxyPenLineJoin.Round:
				return Paint.Join.Round;
			default:
				throw new InvalidOperationException ("Invalid join type.");
			}
		}
	}
}