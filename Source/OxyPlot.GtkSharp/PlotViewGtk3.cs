// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewGtk3.cs" company="OxyPlot">
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
                    this.Window.Cursor = this.PanCursor;
                    break;
                case OxyPlot.CursorType.ZoomRectangle:
                    this.Window.Cursor = this.ZoomRectangleCursor;
                    break;
                case OxyPlot.CursorType.ZoomHorizontal:
                    this.Window.Cursor = this.ZoomHorizontalCursor;
                    break;
                case OxyPlot.CursorType.ZoomVertical:
                    this.Window.Cursor = this.ZoomVerticalCursor;
                    break;
                default:
                    this.Window.Cursor = new Gdk.Cursor(Gdk.CursorType.Arrow);
                    break;
            }
        }
        
        /// <summary>
                 /// Called when the view is drawn.
                 /// </summary>
                 /// <param name="cr">The drawing context.</param>
                 /// <returns><c>true</c> if handled, <c>false</c> otherwise.</returns>
        protected override bool OnDrawn (Cairo.Context cr)
        {
            this.DrawPlot (cr);
            return base.OnDrawn (cr);
        }
    }
}

