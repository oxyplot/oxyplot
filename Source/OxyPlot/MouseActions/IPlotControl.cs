namespace OxyPlot
{
    public interface IPlot
    {
        void GetAxesFromPoint(ScreenPoint pt, out IAxis xaxis, out IAxis yaxis);
        ISeries GetSeriesFromPoint(ScreenPoint pt, double limit = 100);
        
        void Refresh(bool refreshData = true);

        void Pan(IAxis axis, double dx);
        void Reset(IAxis axis);
        void Zoom(IAxis axis, double p1, double p2);
        void ZoomAt(IAxis axis, double factor, double x);
        OxyRect GetPlotArea();
        
        void ShowTracker(ISeries s, DataPoint dp);
        void HideTracker();
        void ShowZoomRectangle(OxyRect r);
        void HideZoomRectangle();
    }
}