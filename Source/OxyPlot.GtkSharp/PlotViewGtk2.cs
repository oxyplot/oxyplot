// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewGtk2.cs" company="OxyPlot">
//   Copyright (c) 2015 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using Gdk;


namespace OxyPlot.GtkSharp
{
    public partial class PlotView
    {
        protected override bool OnExposeEvent (EventExpose evnt)
        {
            using (var cr = Gdk.CairoHelper.Create (evnt.Window))
            {
                cr.Rectangle (evnt.Area.X, evnt.Area.Y, evnt.Area.Width, evnt.Area.Height);
                cr.Clip ();
                DrawPlot (cr);
            }
            return base.OnExposeEvent (evnt);
        }
    }
}

