using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;

namespace OxyPlotControls.Managers;

/// <summary>
/// Manages series operations for a PlotModel including adding, removing, and reordering.
/// This class replaces the series management logic from the monolithic VB toolbar (~ 600 lines).
/// </summary>
public class SeriesManager
{
    private readonly PlotModel _model;
    private readonly PlotView? _plotView;

    /// <summary>
    /// Initializes a new instance of the <see cref="SeriesManager"/> class.
    /// </summary>
    /// <param name="model">The plot model to manage.</param>
    /// <param name="plotView">Optional plot view for invalidation.</param>
    public SeriesManager(PlotModel model, PlotView? plotView = null)
    {
        ArgumentNullException.ThrowIfNull(model);
        _model = model;
        _plotView = plotView;
    }

    #region Add/Remove Series

    /// <summary>
    /// Adds a series to the plot model.
    /// </summary>
    /// <param name="series">The series to add.</param>
    public void AddSeries(Series series)
    {
        ArgumentNullException.ThrowIfNull(series);

        _model.Series.Add(series);
        InvalidatePlot(false);
    }

    /// <summary>
    /// Removes a series from the plot model.
    /// </summary>
    /// <param name="series">The series to remove.</param>
    /// <returns>True if the series was removed, false if it wasn't found.</returns>
    public bool RemoveSeries(Series series)
    {
        ArgumentNullException.ThrowIfNull(series);

        var result = _model.Series.Remove(series);
        if (result)
        {
            InvalidatePlot(false);
        }
        return result;
    }

    /// <summary>
    /// Removes a series at the specified index.
    /// </summary>
    /// <param name="index">The index of the series to remove.</param>
    public void RemoveSeriesAt(int index)
    {
        if (index < 0 || index >= _model.Series.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        _model.Series.RemoveAt(index);
        InvalidatePlot(false);
    }

    /// <summary>
    /// Removes all series from the plot model.
    /// </summary>
    public void ClearSeries()
    {
        _model.Series.Clear();
        InvalidatePlot(false);
    }

    #endregion

    #region Reorder Series

    /// <summary>
    /// Moves a series up in the drawing order (earlier in the collection).
    /// </summary>
    /// <param name="series">The series to move up.</param>
    /// <returns>True if the series was moved, false if it was already first or not found.</returns>
    /// <remarks>
    /// This uses the efficient Move() method instead of the VB code's inefficient
    /// clear-and-rebuild pattern that was O(n) for a simple swap.
    /// </remarks>
    public bool MoveSeriesUp(Series series)
    {
        ArgumentNullException.ThrowIfNull(series);

        var index = _model.Series.IndexOf(series);
        if (index <= 0) return false; // Already first or not found

        _model.Series.Move(index, index - 1);
        InvalidatePlot(false);
        return true;
    }

    /// <summary>
    /// Moves a series down in the drawing order (later in the collection).
    /// </summary>
    /// <param name="series">The series to move down.</param>
    /// <returns>True if the series was moved, false if it was already last or not found.</returns>
    public bool MoveSeriesDown(Series series)
    {
        ArgumentNullException.ThrowIfNull(series);

        var index = _model.Series.IndexOf(series);
        if (index < 0 || index >= _model.Series.Count - 1) return false; // Not found or already last

        _model.Series.Move(index, index + 1);
        InvalidatePlot(false);
        return true;
    }

    /// <summary>
    /// Moves a series to a specific index in the collection.
    /// </summary>
    /// <param name="series">The series to move.</param>
    /// <param name="newIndex">The target index.</param>
    /// <returns>True if the series was moved, false if not found.</returns>
    public bool MoveSeriesTo(Series series, int newIndex)
    {
        ArgumentNullException.ThrowIfNull(series);

        if (newIndex < 0 || newIndex >= _model.Series.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(newIndex));
        }

        var currentIndex = _model.Series.IndexOf(series);
        if (currentIndex < 0) return false; // Not found

        if (currentIndex == newIndex) return true; // Already at target

        _model.Series.Move(currentIndex, newIndex);
        InvalidatePlot(false);
        return true;
    }

    #endregion

    #region Query Series

    /// <summary>
    /// Gets all series in the plot model.
    /// </summary>
    /// <returns>Collection of all series.</returns>
    public IEnumerable<Series> GetAllSeries() => _model.Series;

    /// <summary>
    /// Gets series of a specific type.
    /// </summary>
    /// <typeparam name="TSeries">The series type to filter by.</typeparam>
    /// <returns>Collection of series of the specified type.</returns>
    public IEnumerable<TSeries> GetSeriesOfType<TSeries>() where TSeries : Series
        => _model.Series.OfType<TSeries>();

    /// <summary>
    /// Finds a series by title.
    /// </summary>
    /// <param name="title">The series title to search for.</param>
    /// <returns>The series with the matching title, or null if not found.</returns>
    public Series? FindSeriesByTitle(string title)
    {
        ArgumentNullException.ThrowIfNull(title);
        return _model.Series.FirstOrDefault(s => s.Title == title);
    }

    /// <summary>
    /// Gets the index of a series in the collection.
    /// </summary>
    /// <param name="series">The series to find.</param>
    /// <returns>The index of the series, or -1 if not found.</returns>
    public int GetSeriesIndex(Series series)
    {
        ArgumentNullException.ThrowIfNull(series);
        return _model.Series.IndexOf(series);
    }

    /// <summary>
    /// Gets the number of series in the plot model.
    /// </summary>
    public int SeriesCount => _model.Series.Count;

    #endregion

    #region Visibility

    /// <summary>
    /// Sets the visibility of a series.
    /// </summary>
    /// <param name="series">The series to modify.</param>
    /// <param name="isVisible">True to show the series, false to hide it.</param>
    public void SetSeriesVisibility(Series series, bool isVisible)
    {
        ArgumentNullException.ThrowIfNull(series);

        series.IsVisible = isVisible;
        InvalidatePlot(true);
    }

    /// <summary>
    /// Toggles the visibility of a series.
    /// </summary>
    /// <param name="series">The series to toggle.</param>
    public void ToggleSeriesVisibility(Series series)
    {
        ArgumentNullException.ThrowIfNull(series);

        series.IsVisible = !series.IsVisible;
        InvalidatePlot(true);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Invalidates the plot to trigger a re-render.
    /// </summary>
    /// <param name="updateData">If true, data will be updated before rendering.</param>
    private void InvalidatePlot(bool updateData)
    {
        _model.InvalidatePlot(updateData);
        _plotView?.InvalidatePlot(updateData);
    }

    #endregion
}
