namespace OxyPlot
{
    public interface IPlot
    {
        void GetAxesFromPoint(ScreenPoint pt, out IAxis xaxis, out IAxis yaxis);
        
        ISeries GetSeriesFromPoint(ScreenPoint pt, double limit = 100);

        /// <summary>
        /// Refresh the plot right now
        /// </summary>
        void RefreshPlot();

        /// <summary>
        /// Invalidate the plot (not blocking the UI thread)
        /// </summary>
        void InvalidatePlot();

        /// <summary>
        /// Updates the transforms on the axes (used in pan/zoom).
        /// </summary>
        void UpdateAxisTransforms();

        void Reset(IAxis axis);
        
        void Pan(IAxis axis, double x0, double x1);
        
        void Zoom(IAxis axis, double p1, double p2);
        
        void ZoomAt(IAxis axis, double factor, double x);
        
        OxyRect GetPlotArea();
        
        void ShowTracker(ISeries s, DataPoint dp);
        
        void HideTracker();
        
        void ShowZoomRectangle(OxyRect r);
        
        void HideZoomRectangle();
    }
}