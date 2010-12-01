namespace OxyPlot
{
    public interface IPlotControl
    {
        void GetAxesFromPoint(ScreenPoint pt, out AxisBase xaxis, out AxisBase yaxis);
        DataSeries GetSeriesFromPoint(ScreenPoint pt, double limit = 100);
        
        void Refresh(bool refreshData = true);

        void Pan(AxisBase axis, double dx);
        void Reset(AxisBase axis);
        void Zoom(AxisBase axis, double p1, double p2);
        void ZoomAt(AxisBase axis, double factor, double x);
        OxyRect GetPlotArea();
        
        void ShowSlider(DataSeries s, DataPoint dp);
        void HideSlider();
        void ShowZoomRectangle(OxyRect r);
        void HideZoomRectangle();
    }
}