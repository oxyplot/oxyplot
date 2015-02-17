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
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using System.Collections.Generic;
using OxyPlot.Series;
using aux;


namespace OHLCViewer
{
	public class OHLCVPlot : PlotModel
	{
		public OHLCVPlot (
			IEnumerable<OHLCVItem> bars, 
			int barwindow, 
			double maxvol = 5000,
			int mawindow = 10,
			double volumepane = 0.20)
		{
			Window = barwindow;
			MAWindow = mawindow;

			SetupOHLCV (bars);
			SetupMA ();

			VolumePane = 0.20;
			VolumeMax = maxvol;
			VolumeRendering = OHLCVSeries.VolumeStyle.PositiveNegative;

			this.Background = OxyColor.Parse("#101010");
			this.PlotAreaBorderColor = OxyColor.Parse ("#676b80");
		}


		// Properties

		public List<OHLCVItem> Data
			{ get { return _Sohlcv.Items; } }

		public int Window
			{ get; private set; }

		public int MAWindow
			{ get; private set; }

		public PlotController Controller
			{ get; private set; }

		public int VisibleStart
			{ get { return _coffset; } }

		public int VisibleEnd
			{ get { return _coffset + Window - 1; } }

		public double VolumePane
		{ 
			get { return _volpane; } 
			set { AdjustVolumeSettings (VolumeRendering, VolumeMax, value); }
		}

		public double VolumeMax
		{ 
			get { return _maxvol; } 
			set { AdjustVolumeSettings(VolumeRendering, value, VolumePane); } 
		}

		public OHLCVSeries.VolumeStyle VolumeRendering
		{ 
			get { return _Sohlcv.VolumeRendering; } 
			set { AdjustVolumeSettings(value, VolumeMax, VolumePane); } 
		}


		// Events


		/// <summary>
		/// onPan: event called when a pan has 
		/// </summary>
		public event EventHandler<DateTimeAxis> OnPan;


		// Functions


		#region Implementation


		private void SetupOHLCV (IEnumerable<OHLCVItem> bars)
		{
			var orange = OxyColor.Parse ("#c26109");
			var grey = OxyColor.Parse ("#676b80");
			var green = OxyColor.Parse ("#48c143");
			var red = OxyColor.Parse ("#9a100f");

			// create series
			_Sohlcv = new OHLCVSeries
			{
				PositiveColor = green,
				NegativeColor = red,
				SeparatorColor = grey,
				VolumeStacked = true
			};

			foreach (var bar in bars)
				_Sohlcv.Append (bar);

			this.Series.Add (_Sohlcv);

			// axes
			_xaxis = new DateTimeAxis { 
				Position = AxisPosition.Bottom, IntervalType = DateTimeIntervalType.Auto,
				TextColor = orange, TitleColor = orange,
				TicklineColor = grey, AxislineColor = grey, MajorGridlineColor = grey, MinorGridlineColor = grey,
				Minimum = DateTimeAxis.ToDouble (Data[0].X),
				Maximum = DateTimeAxis.ToDouble (Data[Window-1].X) };
			this.Axes.Add(_xaxis);
			_xaxis.AxisChanged += (sender, e) =>  AdjustYExtent (e);

			_yaxis_bars = new LinearAxis { 
				Position = AxisPosition.Left,
				StartPosition = VolumePane+ 0.03, EndPosition = 1.0,
				Key = "Bars",
				TextColor = orange, TitleColor = orange,
				TicklineColor = grey, AxislineColor = grey, MajorGridlineColor = grey, MinorGridlineColor = grey };
			this.Axes.Add(_yaxis_bars);

			_yaxis_volume = new LinearAxis { 
				Position = AxisPosition.Left,
				StartPosition = 0.0, EndPosition = 0.20,
				Minimum = 0.0,
				Key = "Volume",
				TextColor = orange, TitleColor = orange,
				TicklineColor = grey, AxislineColor = grey, MajorGridlineColor = grey, MinorGridlineColor = grey };
			this.Axes.Add(_yaxis_volume);

			AdjustYExtent (null);

			// controller
			Controller = new PlotController();
            Controller.InputCommandBindings.Clear();
            Controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
		}


		private void SetupMA ()
		{
			var data = Data;
			var ma = new EMA (MAWindow);

			// create series
			_Sma = new LineSeries
			{
				StrokeThickness = 2,
				Color = OxyColors.Red,
				YAxisKey = "Bars"
			};

			// add MA
			foreach (var bar in data)
			{
				var v = ma.Tick (bar.Close);
				_Sma.Points.Add (new DataPoint (bar.X, v));
			}

			this.Series.Add (_Sma);
		}


		private void AdjustVolumeSettings (OHLCVSeries.VolumeStyle style, double max, double panesize)
		{
			_yaxis_bars.StartPosition = panesize + 0.02;
			_yaxis_bars.EndPosition = 1.0;
			_yaxis_volume.StartPosition = 0;
			_yaxis_volume.EndPosition = panesize;
			_yaxis_volume.IsAxisVisible = true;

			_volpane = panesize;
			_maxvol = max;

			switch (style)
			{
				case OHLCVSeries.VolumeStyle.None:
					_yaxis_bars.StartPosition = 0;
					_yaxis_bars.EndPosition = 1.0;
					_yaxis_volume.StartPosition = 0;
					_yaxis_volume.EndPosition = 0;
					_yaxis_volume.IsAxisVisible = false;
					break;
				case OHLCVSeries.VolumeStyle.Combined:
					_yaxis_volume.Maximum = max;
					_yaxis_volume.Minimum = 0;
					_yaxis_volume.Reset ();
					_Sohlcv.VolumeRendering = style;
					_yaxis_volume.IsAxisVisible = true;
					break;
				case OHLCVSeries.VolumeStyle.PositiveNegative:
					_yaxis_volume.Maximum = max;
					_yaxis_volume.Minimum = -max;
					_yaxis_volume.Reset ();
					_Sohlcv.VolumeRendering = style;
					break;
				case OHLCVSeries.VolumeStyle.Stacked:
					_yaxis_volume.Maximum = max;
					_yaxis_volume.Minimum = 0;
					_yaxis_volume.Reset ();
					_Sohlcv.VolumeRendering = style;
					break;
			}

			this.InvalidatePlot (false);
		}


		private void AdjustYExtent (AxisChangedEventArgs e)
		{
			var xmin = _xaxis.ActualMinimum;
			var xmax = _xaxis.ActualMaximum;

			var istart = Math.Min (_Sohlcv.FindByX (xmin), Data.Count - Window);
			var iend = _coffset + Window;

			var items = Data;

			var ymin = double.MaxValue;
			var ymax = double.MinValue;
			for (int i = istart; i <= iend; i++)
			{
				var bar = items [i];
				ymin = Math.Min (ymin, bar.Low);
				ymax = Math.Max (ymax, bar.High);
			}

			var extent = (ymax - ymin);
			var margin = extent * 0.10;

			_yaxis_bars.Zoom (ymin - margin, ymax + margin);

			// call panning event
			_coffset = istart;
			if (OnPan != null)
				OnPan (this, _xaxis);
		}


		#endregion


		private OHLCVSeries		_Sohlcv;
		private LineSeries		_Sma;

		private DateTimeAxis	_xaxis;
		private LinearAxis		_yaxis_bars;
		private LinearAxis		_yaxis_volume;

		private double			_maxvol;
		private double			_volpane;
		private int				_coffset = 0;
	}
}

