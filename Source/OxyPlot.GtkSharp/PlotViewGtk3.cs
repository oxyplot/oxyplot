// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewGtk3.cs" company="OxyPlot">
//   Copyright (c) 2015 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using Gdk;


namespace OxyPlot.GtkSharp
{
    public partial class PlotView
    {
        protected override bool OnDrawn (Cairo.Context cr)
        {
            DrawPlot (cr);
            return base.OnDrawn (cr);
        }
    }
}

