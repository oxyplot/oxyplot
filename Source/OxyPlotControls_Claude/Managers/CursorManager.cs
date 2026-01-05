using System;
using System.Windows.Input;
using OxyPlot.Wpf;

namespace OxyPlotControls.Managers;

/// <summary>
/// Manages cursor appearance for different plot interaction modes.
/// This class replaces the cursor management logic from the monolithic VB toolbar (~300 lines).
/// </summary>
public class CursorManager
{
    private readonly PlotView? _plotView;
    private Cursor _defaultCursor = Cursors.Arrow;

    /// <summary>
    /// Initializes a new instance of the <see cref="CursorManager"/> class.
    /// </summary>
    /// <param name="plotView">The plot view to manage cursors for.</param>
    public CursorManager(PlotView? plotView = null)
    {
        _plotView = plotView;
        if (_plotView != null)
        {
            _defaultCursor = _plotView.Cursor ?? Cursors.Arrow;
        }
    }

    #region Cursor Types

    /// <summary>
    /// Sets the cursor to the default arrow.
    /// </summary>
    public void SetDefaultCursor()
    {
        SetCursor(_defaultCursor);
    }

    /// <summary>
    /// Sets the cursor to a crosshair for precision selection.
    /// </summary>
    public void SetCrosshairCursor()
    {
        SetCursor(Cursors.Cross);
    }

    /// <summary>
    /// Sets the cursor to a hand for panning.
    /// </summary>
    public void SetPanCursor()
    {
        SetCursor(Cursors.Hand);
    }

    /// <summary>
    /// Sets the cursor to indicate zoom mode.
    /// </summary>
    public void SetZoomCursor()
    {
        // Use SizeAll for zoom, or custom cursor if available
        SetCursor(Cursors.SizeAll);
    }

    /// <summary>
    /// Sets the cursor to indicate drawing/annotation mode.
    /// </summary>
    public void SetDrawCursor()
    {
        SetCursor(Cursors.Pen);
    }

    /// <summary>
    /// Sets the cursor to indicate waiting/busy state.
    /// </summary>
    public void SetWaitCursor()
    {
        SetCursor(Cursors.Wait);
    }

    /// <summary>
    /// Sets a custom cursor.
    /// </summary>
    /// <param name="cursor">The cursor to set.</param>
    public void SetCursor(Cursor cursor)
    {
        ArgumentNullException.ThrowIfNull(cursor);

        if (_plotView != null)
        {
            _plotView.Cursor = cursor;
        }
    }

    #endregion

    #region Cursor State

    /// <summary>
    /// Gets the current cursor.
    /// </summary>
    public Cursor? CurrentCursor => _plotView?.Cursor;

    /// <summary>
    /// Temporarily changes the cursor and returns an IDisposable that restores it.
    /// </summary>
    /// <param name="cursor">The temporary cursor.</param>
    /// <returns>IDisposable that restores the previous cursor when disposed.</returns>
    /// <example>
    /// using (cursorManager.TemporaryCursor(Cursors.Wait))
    /// {
    ///     // Perform long operation
    /// }
    /// // Cursor automatically restored
    /// </example>
    public IDisposable TemporaryCursor(Cursor cursor)
    {
        ArgumentNullException.ThrowIfNull(cursor);
        return new CursorScope(this, cursor);
    }

    #endregion

    #region Helper Classes

    private sealed class CursorScope : IDisposable
    {
        private readonly CursorManager _manager;
        private readonly Cursor _previousCursor;

        public CursorScope(CursorManager manager, Cursor temporaryCursor)
        {
            _manager = manager;
            _previousCursor = _manager.CurrentCursor ?? Cursors.Arrow;
            _manager.SetCursor(temporaryCursor);
        }

        public void Dispose()
        {
            _manager.SetCursor(_previousCursor);
        }
    }

    #endregion
}
