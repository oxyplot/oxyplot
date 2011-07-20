using System;

namespace OxyPlot
{
    public enum AxisPosition
    {
        // cartesian axes:
        Left,
        Right,
        Top,
        Bottom,

        // polar axes:
        Angle,
        Magnitude
    }

    public enum TickStyle
    {
        Crossing,
        Inside,
        Outside,
        None
    }

    public interface IAxis
    {
        string Title { get; }
        string ActualTitle { get; }

        string Key { get;  }
        bool IsVisible { get; set; }

		string TitleFormatString { get; set; }

        AxisPosition Position { get; set; }
        
        double ActualMinimum { get; set; }
        double ActualMaximum { get; set; }

		double AbsoluteMinimum { get; set; }
		double AbsoluteMaximum { get; set; }
		
		double Scale { get; set; }
        double Offset { get; set; }
        
        ScreenPoint MidPoint { get; set; }
        ScreenPoint ScreenMin { get; set; }
        ScreenPoint ScreenMax { get; set; }

        bool IsValidValue(double value);

        bool IsHorizontal();
        bool IsVertical();
        bool IsPolar();
        
        void UpdateActualMaxMin();
        void Include(double value);
        void SetScale(double scale);
        void UpdateIntervals(OxyRect plotArea);
        string FormatValue(double x);
        void Render(IRenderContext rc, PlotModel model);
        
        void UpdateTransform(OxyRect plotArea);
        double Transform(double x);
        ScreenPoint Transform(DataPoint dp, IAxis yAxis);
        double InverseTransform(double sx);

        void Reset();
        void Pan(double x0, double x1);
        void Zoom(double x1, double x2);
        void ZoomAt(double factor, double x);

        event EventHandler<AxisChangedEventArgs> AxisChanged;
    }

    public enum AxisChangeTypes { Zoom, Pan, Reset }

    public class AxisChangedEventArgs : EventArgs
    {
        public AxisChangeTypes ChangeType { get; set; }

        public AxisChangedEventArgs(AxisChangeTypes changeType)
        {
            this.ChangeType = changeType;
        }
    }
}