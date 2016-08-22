// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewGtk2.cs" company="OxyPlot">
//   Copyright (c) 2015 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.GtkSharp
{
    public partial class PlotView
    {
        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">The cursor type.</param>
        public void SetCursorType(OxyPlot.CursorType cursorType)
        {
            switch (cursorType)
            {
                case OxyPlot.CursorType.Pan:
                    this.GdkWindow.Cursor = this.PanCursor;
                    break;
                case OxyPlot.CursorType.ZoomRectangle:
                    this.GdkWindow.Cursor = this.ZoomRectangleCursor;
                    break;
                case OxyPlot.CursorType.ZoomHorizontal:
                    this.GdkWindow.Cursor = this.ZoomHorizontalCursor;
                    break;
                case OxyPlot.CursorType.ZoomVertical:
                    this.GdkWindow.Cursor = this.ZoomVerticalCursor;
                    break;
                default:
                    this.GdkWindow.Cursor = new Gdk.Cursor(Gdk.CursorType.Arrow);
                    break;
            }
        }

        /// <summary>
        /// Called when the plot view is exposed.
        /// </summary>
        /// <param name="evnt">The event data.</param>
        /// <returns><c>true</c> if the event was handled, <c>false</c> otherwise.</returns>
        protected override bool OnExposeEvent(Gdk.EventExpose evnt)
        {
            using (var cr = Gdk.CairoHelper.Create(evnt.Window))
            {
                cr.Rectangle(evnt.Area.X, evnt.Area.Y, evnt.Area.Width, evnt.Area.Height);
                cr.Clip();
                this.DrawPlot(cr);
            }

            return base.OnExposeEvent(evnt);
        }
    }
}

