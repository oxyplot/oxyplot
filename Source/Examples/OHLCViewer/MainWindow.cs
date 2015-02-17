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
using Gtk;
using OxyPlot.GtkSharp;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace OHLCViewer
{
	public partial class MainWindow: Gtk.Window
	{
		public MainWindow () 
			: base ("OHLC Viewer")
		{
			Build ();

			this.Title = "OHLC Viewer";
			this.SetSizeRequest (800, 600);
		}


		#region Widget Creation


		private void Build ()
		{
			this.Title = "OHLC Viewer";
			this.Add (CreateLayout2 ());
			OnVolumeChange ();

			this.DeleteEvent += OnDeleteEvent;
		}


		private Widget CreateLayout2()
		{
			var table = new Table (2, 2, homogeneous: false);
			table.Attach (
				CreateSelectors (), 0, 1, 0, 1, 
				xoptions: AttachOptions.Shrink,
				yoptions: AttachOptions.Shrink,
				xpadding: 20,
				ypadding: 0);
			table.Attach (
				CreateStatusText (), 1, 2, 0, 1,
				xoptions: AttachOptions.Fill | AttachOptions.Expand,
				yoptions: AttachOptions.Shrink, 
				xpadding: 0,
				ypadding: 0);
			table.Attach (
				CreateView (), 0, 2, 1, 2, 
				xoptions: AttachOptions.Fill | AttachOptions.Expand, 
				yoptions: AttachOptions.Fill | AttachOptions.Expand,
				xpadding: 0,
				ypadding: 0);
			table.Visible = true;
			table.SetSizeRequest (800, 600);
			return table;
		}


		private PlotView CreateView ()
		{
			var bars = BarGenerator.MRProcess ();
			_ohlc = new OHLCVPlot (bars, 80);

			var plot = new PlotView();
			plot.Model = _ohlc;
			plot.Controller = _ohlc.Controller;
			plot.Visible = true;
			plot.SetSizeRequest (800, 550);

			return plot;
		}


		private Widget CreateSelectors ()
		{
			var hbox = new HBox (false, 50);
			hbox.Add (CreateVolumeSelector ());
			hbox.Add (CreatePeriodSelector ());
			hbox.Visible = true;
			return hbox;
		}



		private ComboBox CreateVolumeSelector ()
		{
			_volume = new ComboBox (new string[] { "None", "Combined", "Stacked", "+/-" }) { Active = 1 };
			_volume.Changed += (object sender, EventArgs e) => OnVolumeChange ();
			_volume.WidthRequest = 120;
			_volume.Visible = true;

			return _volume;
		}


		private ComboBox CreatePeriodSelector ()
		{
			_period = new ComboBox (new string[] { "5sec", "10sec", "30sec", "1min", "5min", "15min" }) { Active = 1 };
			_period.Changed += (object sender, EventArgs e) => OnPeriodChange ();
			_period.WidthRequest = 80;
			_period.Visible = true;
			return _period;
		}

		private Label CreateStatusText ()
		{
			_info = new Label ("volume: " + _volume.ActiveText + "  period: " + _period.ActiveText);
			_info.Justify = Justification.Right;
			_info.Visible = true;
			return _info;
		}


		#endregion

		#region Event handling


		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
			System.Environment.Exit (0);
		}


		protected void OnVolumeChange ()
		{
			switch (_volume.ActiveText)
			{
				case "None":
					_ohlc.VolumeRendering = OHLCVSeries.VolumeStyle.None;
					break;
				case "Combined":
					_ohlc.VolumeRendering = OHLCVSeries.VolumeStyle.Combined;
					break;
				case "Stacked":
					_ohlc.VolumeRendering = OHLCVSeries.VolumeStyle.Stacked;
					break;
				case "+/-":
					_ohlc.VolumeRendering = OHLCVSeries.VolumeStyle.PositiveNegative;
					break;
			}

			OnStatusChange ();
		}


		protected void OnPeriodChange ()
		{
			OnStatusChange ();
		}


		protected void OnStatusChange ()
		{
			_info.Text = "volume: " + _volume.ActiveText + "  period: " + _period.ActiveText;
		}


		#endregion

		// Variables

		private ComboBox		_volume;
		private ComboBox		_period;
		private Label			_info;
		private OHLCVPlot 		_ohlc;
		private PlotView		_plot;
	}
}