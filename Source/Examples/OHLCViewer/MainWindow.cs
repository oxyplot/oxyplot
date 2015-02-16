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
			var bars = BarGenerator.MRProcess ();
			_ohlc = new OHLCVPlot (bars, 80);

			_plot = new PlotView();
			_plot.Model = _ohlc;
			_plot.Controller = _ohlc.Controller;
			_plot.Visible = true;
			_plot.SetSizeRequest (800, 550);

			this.Title = "OHLC Viewer";
			var vbox = new VBox (false, 10);
			var hbox = new HBox (false, 50);
			var fix = new Fixed ();
			hbox.Add (VolumeSelectorWidget ());
			hbox.Add (PeriodSelectorWidget ());
			hbox.Add (StatusTextWidget ());
			fix.Add (hbox);

			vbox.Add (fix);
			vbox.Add (_plot);
			this.Add (vbox);

			fix.Visible = true;
			hbox.Visible = true;
			vbox.Visible = true;
			this.Focus = vbox;

			this.DeleteEvent += OnDeleteEvent;
		}


		private ComboBox VolumeSelectorWidget ()
		{
			_volume = new ComboBox (new string[] { "None", "Combined", "Stacked", "+/-" });
			_volume.Changed += (object sender, EventArgs e) => OnVolumeChange ();
			_volume.Visible = true;

			return _volume;
		}


		private ComboBox PeriodSelectorWidget ()
		{
			_period = new ComboBox (new string[] { "5sec", "10sec", "30sec", "1min", "5min", "15min" });
			_period.Changed += (object sender, EventArgs e) => OnPeriodChange ();
			_period.Visible = true;
			Console.Error.WriteLine (_period.SizeRequest().Width + "x" + _period.SizeRequest().Height);
			return _period;
		}

		private Label StatusTextWidget ()
		{
			_info = new Label ("volume: " + _volume.ActiveText + "  period: " + _period.ActiveText);
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