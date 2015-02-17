// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeRenderingStyle.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents rendering style for volume in either <see cref="CandleStickAndVolumeSeries" /> or 
//	 <see cref="VolumeSeries" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;


namespace OxyPlot.Series
{
	/// <summary>
	/// Volume rendering style.
	/// </summary>
	public enum VolumeStyle
	{
		None, 
		Combined, 
		Stacked, 
		PositiveNegative
	}
}

