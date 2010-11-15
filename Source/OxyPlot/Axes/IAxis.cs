using System.Collections.Generic;

namespace OxyPlot
{
    public enum AxisPosition
    {
        Left,
        Right,
        Top,
        Bottom
    } ;

    public enum TickStyle
    {
        Crossing,
        Inside,
        Outside,
        None
    } ;

    public interface IAxis
    {
        string Key { get; }
        bool IsHorizontal { get; }
        double ActualMaximum { get; set; }
        double ActualMinimum { get; set; }
        AxisPosition Position { get; }
        Color MinorGridlineColor { get; }
        Color TicklineColor { get; }
        Color MajorGridlineColor { get; }
        Color ExtraGridlineColor { get; }
        double MinorGridlineThickness { get; }
        double MajorGridlineThickness { get; }
        double ExtraGridlineThickness { get; }
        LineStyle MinorGridlineStyle { get; }
        LineStyle MajorGridlineStyle { get; }
        LineStyle ExtraGridlineStyle { get; }
        bool PositionAtZeroCrossing { get; }
        bool ShowMinorTicks { get; }
        TickStyle TickStyle { get; }
        double MinorTickSize { get; }
        double MajorTickSize { get; }
        string FontFamily { get; }
        double FontSize { get; }
        double FontWeight { get; }
        double Angle { get; }
        double[] ExtraGridlines { get; }
        string Title { get; }
        double Maximum { get; }
        double MaximumPadding { get; }
        double Minimum { get; }
        double MinimumPadding { get; }
        bool IsVisible { get; }
        string FormatValue(double x);
        void GetTickValues(out ICollection<double> majorValues, out ICollection<double> minorValues);
        double UpdateTransform(double x0, double x1, double y0, double y1);
        void UpdateIntervals(double length, double labelSize);
        void Include(double p);
        double Transform(double x);
        double InverseTransform(double x);
        void Pan(double dx);
        void Reset();
        void SetScale(double scale);
        void ScaleAt(double factor, double x);
        void Zoom(double x0, double x1);
    }
}