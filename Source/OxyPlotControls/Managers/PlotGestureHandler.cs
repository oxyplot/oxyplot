using System;
using OxyPlot;
using OxyPlot.Wpf;

namespace OxyPlotControls.Managers;

/// <summary>
/// Handles plot gestures and interactions including pan, zoom, and reset.
/// This class replaces the gesture handling logic from the monolithic VB toolbar (~800 lines).
/// </summary>
public class PlotGestureHandler
{
    private readonly PlotModel _model;
    private readonly PlotView? _plotView;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlotGestureHandler"/> class.
    /// </summary>
    /// <param name="model">The plot model.</param>
    /// <param name="plotView">Optional plot view for gesture handling.</param>
    public PlotGestureHandler(PlotModel model, PlotView? plotView = null)
    {
        ArgumentNullException.ThrowIfNull(model);
        _model = model;
        _plotView = plotView;
    }

    #region Zoom Operations

    /// <summary>
    /// Zooms all axes in by the specified factor.
    /// </summary>
    /// <param name="factor">The zoom factor (e.g., 0.5 for 50% zoom in).</param>
    public void ZoomIn(double factor = 0.5)
    {
        if (factor <= 0 || factor >= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(factor), "Factor must be between 0 and 1");
        }

        foreach (var axis in _model.Axes)
        {
            axis.Zoom(factor);
        }

        InvalidatePlot(false);
    }

    /// <summary>
    /// Zooms all axes out by the specified factor.
    /// </summary>
    /// <param name="factor">The zoom factor (e.g., 2.0 for 200% zoom out).</param>
    public void ZoomOut(double factor = 2.0)
    {
        if (factor <= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(factor), "Factor must be greater than 1");
        }

        foreach (var axis in _model.Axes)
        {
            axis.Zoom(factor);
        }

        InvalidatePlot(false);
    }

    /// <summary>
    /// Zooms to fit all data within the visible area.
    /// </summary>
    public void ZoomToFit()
    {
        _plotView?.ResetAllAxes();
        InvalidatePlot(false);
    }

    #endregion

    #region Pan Operations

    /// <summary>
    /// Pans all axes by the specified delta.
    /// </summary>
    /// <param name="deltaX">The horizontal pan delta in data coordinates.</param>
    /// <param name="deltaY">The vertical pan delta in data coordinates.</param>
    public void Pan(double deltaX, double deltaY)
    {
        foreach (var axis in _model.Axes)
        {
            if (axis.IsHorizontal())
            {
                axis.Pan(deltaX);
            }
            else if (axis.IsVertical())
            {
                axis.Pan(deltaY);
            }
        }

        InvalidatePlot(false);
    }

    #endregion

    #region Reset Operations

    /// <summary>
    /// Resets all axes to their default ranges.
    /// </summary>
    public void ResetAllAxes()
    {
        _plotView?.ResetAllAxes();
        InvalidatePlot(false);
    }

    /// <summary>
    /// Resets the plot view to its default state.
    /// </summary>
    public void ResetView()
    {
        ResetAllAxes();
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
