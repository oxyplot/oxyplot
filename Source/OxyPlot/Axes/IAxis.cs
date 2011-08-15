using System;
using System.Collections.Generic;

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
        /// <summary>
        /// Gets the title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the unit.
        /// </summary>
        string Unit { get; }

        /// <summary>
        /// Gets or sets the title format string.
        /// The formatting is used if a Unit is defined.
        /// </summary>
        string TitleFormatString { get; }

        /// <summary>
        /// Gets the actual title.
        /// </summary>
        string ActualTitle { get; }

        /// <summary>
        /// Gets the key of the axis.
        /// This can be used by series to select which axis to use.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets a value indicating whether this axis is visible.
        /// </summary>
        bool IsAxisVisible { get; }

        /// <summary>
        /// Gets the position of the axis.
        /// </summary>
        AxisPosition Position { get; }

        double ActualMinimum { get; }
        double ActualMaximum { get; }

        double Scale { get; }
        double Offset { get; }

        ScreenPoint MidPoint { get; }
        ScreenPoint ScreenMin { get; }
        ScreenPoint ScreenMax { get; }

        bool IsValidValue(double value);

        bool IsHorizontal();
        bool IsVertical();
        bool IsPolar();

        void UpdateData(IEnumerable<ISeries> series);
        void ResetActualMaxMin();
        void UpdateActualMaxMin();
        void Include(double value);
        void SetScale(double scale);
        void UpdateIntervals(OxyRect plotArea);

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        string FormatValue(double x);

        /// <summary>
        /// Formats the value to be used by the tracker.
        /// </summary>
        string FormatValueForTracker(double x);

        /// <summary>
        /// Gets the value, converts from double to the correct data type if neccessary
        /// e.g. DateTimeAxis returns the DateTime and CategoryAxis returns category strings.
        /// </summary>
        object GetValue(double x);

        /// <summary>
        /// Renders the axis on the specified render context.
        /// </summary>
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