// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAxis.cs" company="OxyPlot">
//   See http://oxyplot.codeplex.com
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Axis positions
    /// </summary>
    public enum AxisPosition
    {
        /// <summary>
        /// Left of the plot area.
        /// </summary>
        Left, 

        /// <summary>
        /// Right of the plot area.
        /// </summary>
        Right, 

        /// <summary>
        /// Top of the plot area.
        /// </summary>
        Top, 

        /// <summary>
        /// Bottom of the plot area.
        /// </summary>
        Bottom
    }

    /// <summary>
    /// Tick styles.
    /// </summary>
    public enum TickStyle
    {
        /// <summary>
        /// Crossing the axis line.
        /// </summary>
        Crossing, 

        /// <summary>
        /// Inside of the plot area.
        /// </summary>
        Inside, 

        /// <summary>
        /// Outside the plot area.
        /// </summary>
        Outside, 

        /// <summary>
        /// No tick.
        /// </summary>
        None
    }

    /// <summary>
    /// Axis layer position.
    /// </summary>
    public enum AxisLayer
    {
        /// <summary>
        /// Below all series.
        /// </summary>
        BelowSeries, 

        /// <summary>
        /// Above all series.
        /// </summary>
        AboveSeries
    }

    /// <summary>
    /// Change types of the IAxis.AxisChanged event.
    /// </summary>
    public enum AxisChangeTypes
    {
        /// <summary>
        /// The zoom.
        /// </summary>
        Zoom,

        /// <summary>
        /// The pan.
        /// </summary>
        Pan,

        /// <summary>
        /// The reset.
        /// </summary>
        Reset
    }

    /// <summary>
    /// Axis interface.
    /// </summary>
    public interface IAxis
    {
        #region Public Events

        /// <summary>
        /// Occurs when the axis has been changed (by zooming, panning or resetting).
        /// </summary>
        event EventHandler<AxisChangedEventArgs> AxisChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the actual maximum value of the axis.
        /// If Maximum is not NaN, this value will be defined by Maximum.
        /// If ViewMaximum is not NaN, this value will be defined by ViewMaximum.
        /// Otherwise this value will be defined by the maximum (+padding) of the data.
        /// </summary>
        double ActualMaximum { get; }

        /// <summary>
        /// Gets the actual minimum value of the axis.
        /// If Minimum is not NaN, this value will be defined by Minimum.
        /// If ViewMinimum is not NaN, this value will be defined by ViewMinimum.
        /// Otherwise this value will be defined by the minimum (+padding) of the data.
        /// </summary>
        double ActualMinimum { get; }

        /// <summary>
        /// Gets the actual title.
        /// </summary>
        string ActualTitle { get; }

        /// <summary>
        /// Gets a value indicating whether this axis is visible.
        /// </summary>
        bool IsAxisVisible { get; }

        /// <summary>
        /// Gets the key of the axis.
        /// This can be used by series to select which axis to use.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets the position of the axis.
        /// </summary>
        AxisPosition Position { get; }

        /// <summary>
        /// Gets the screen coordinate of the maximum point on the axis.
        /// </summary>
        ScreenPoint ScreenMax { get; }

        /// <summary>
        /// Gets the screen coordinate of the minimum point on the axis.
        /// </summary>
        ScreenPoint ScreenMin { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the title format string.
        /// The formatting is used if a Unit is defined.
        /// </summary>
        string TitleFormatString { get; }

        /// <summary>
        /// Gets the unit.
        /// </summary>
        string Unit { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        /// <param name="x">
        /// The value.
        /// </param>
        /// <returns>
        /// The formatted value.
        /// </returns>
        string FormatValue(double x);

        /// <summary>
        /// Formats the value to be used by the tracker.
        /// </summary>
        /// <param name="x">
        /// The value.
        /// </param>
        /// <returns>
        /// The formatted value.
        /// </returns>
        string FormatValueForTracker(double x);

        /// <summary>
        /// Gets the value from an axis coordinate, converts from double to the correct data type if neccessary.
        /// e.g. DateTimeAxis returns the DateTime and CategoryAxis returns category strings.
        /// </summary>
        /// <param name="x">
        /// The coordinate.
        /// </param>
        /// <returns>
        /// The value.
        /// </returns>
        object GetValue(double x);

        /// <summary>
        /// Inverse transform the specified screen coordinate.
        /// This method can only be used with non-polar coordinate systems.
        /// </summary>
        /// <param name="sx">
        /// The screen coordinate.
        /// </param>
        /// <returns>
        /// The value.
        /// </returns>
        double InverseTransform(double sx);

        /// <summary>
        /// Inverse transform the specified screen point.
        /// </summary>
        /// <param name="sx">
        /// The x coordinate.
        /// </param>
        /// <param name="sy">
        /// The y coordinate.
        /// </param>
        /// <param name="yaxis">
        /// The y-axis.
        /// </param>
        /// <returns>
        /// The data point.
        /// </returns>
        DataPoint InverseTransform(double sx, double sy, IAxis yaxis);

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified value is valid; otherwise, <c>false</c>.
        /// </returns>
        bool IsValidValue(double value);

        /// <summary>
        /// Measures the size of the axis (maximum axis label width/height).
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <returns>
        /// The size of the axis.
        /// </returns>
        OxySize Measure(IRenderContext rc);

        /// <summary>
        /// Pans the axis.
        /// </summary>
        /// <param name="x0">
        /// The previous screen coordinate.
        /// </param>
        /// <param name="x1">
        /// The current screen coordinate.
        /// </param>
        void Pan(double x0, double x1);

        /// <summary>
        /// Renders the axis on the specified render context.
        /// </summary>
        /// <param name="rc">
        /// The render context.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="axisLayer">
        /// The rendering order.
        /// </param>
        void Render(IRenderContext rc, PlotModel model, AxisLayer axisLayer);

        /// <summary>
        /// Resets the user's modification (zooming/panning) to minmum and maximum of this axis.
        /// This method will not refresh the plot.
        /// </summary>
        void Reset();

        /// <summary>
        /// Sets the scaling factor.
        /// </summary>
        /// <param name="scale">
        /// The new scale.
        /// </param>
        void Zoom(double scale);

        /// <summary>
        /// Transforms the specified coordinate to screen coordinates.
        /// This method can only be used with non-polar coordinate systems.
        /// </summary>
        /// <param name="x">
        /// The value.
        /// </param>
        /// <returns>
        /// The transformed value (screen coordinate).
        /// </returns>
        double Transform(double x);

        /// <summary>
        /// Transforms the specified point to screen coordinates.
        /// </summary>
        /// <param name="x">
        /// The x value (for the current axis).
        /// </param>
        /// <param name="y">
        /// The y value.
        /// </param>
        /// <param name="yaxis">
        /// The y axis.
        /// </param>
        /// <returns>
        /// The transformed point.
        /// </returns>
        ScreenPoint Transform(double x, double y, IAxis yaxis);

        /// <summary>
        /// Updates the actual maximum and minimum values.
        /// If the user has zoomed/panned the axis, the internal ViewMaximum/ViewMinimum values will be used.
        /// If Maximum or Minimum have been set, these values will be used.
        /// Otherwise the maximum and minimum values of the series will be used, including the 'padding'.
        /// </summary>
        // void UpdateActualMaxMin();

        /// <summary>
        /// Updates the axis with information from the plot series.
        /// This is used by the category axis that need to know the number of series using the axis.
        /// </summary>
        /// <param name="series">
        /// The series collection.
        /// </param>
        //void UpdateFromSeries(IEnumerable<ISeries> series);

        /// <summary>
        /// Updates the actual minor and major step intervals.
        /// </summary>
        /// <param name="plotArea">
        /// The plot area rectangle.
        /// </param>
        //void UpdateIntervals(OxyRect plotArea);

        /// <summary>
        /// Updates the scale and offset properties of the transform
        /// from the specified boundary rectangle.
        /// </summary>
        /// <param name="plotArea">
        /// The plot area rectangle.
        /// </param>
       // void UpdateTransform(OxyRect plotArea);

        /// <summary>
        /// Zooms the axis to the range [x0,x1].
        /// </summary>
        /// <param name="x0">
        /// The new minimum.
        /// </param>
        /// <param name="x1">
        /// The new maximum.
        /// </param>
        void Zoom(double x0, double x1);

        /// <summary>
        /// Zooms the axis at the specified coordinate.
        /// </summary>
        /// <param name="factor">
        /// The zoom factor.
        /// </param>
        /// <param name="x">
        /// The coordinate to zoom at.
        /// </param>
        void ZoomAt(double factor, double x);

        #endregion
    }

    /// <summary>
    /// EventArgs for the IAxis.AxisChanged event.
    /// </summary>
    public class AxisChangedEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisChangedEventArgs"/> class.
        /// </summary>
        /// <param name="changeType">
        /// Type of the change.
        /// </param>
        public AxisChangedEventArgs(AxisChangeTypes changeType)
        {
            this.ChangeType = changeType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the type of the change.
        /// </summary>
        /// <value>The type of the change.</value>
        public AxisChangeTypes ChangeType { get; set; }

        #endregion
    }
}