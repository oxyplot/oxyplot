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


namespace OHLCViewer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			Gtk.Rc.Parse("/tmp/dark.gtkrc");

			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}
