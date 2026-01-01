using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Wpf;

namespace OxyPlotControls.Managers;

/// <summary>
/// Manages annotation operations for a PlotModel including adding, removing, and modifying annotations.
/// This class replaces the annotation management logic from the monolithic VB toolbar (~800 lines).
/// </summary>
public class AnnotationManager
{
    private readonly PlotModel _model;
    private readonly PlotView? _plotView;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnnotationManager"/> class.
    /// </summary>
    /// <param name="model">The plot model to manage.</param>
    /// <param name="plotView">Optional plot view for invalidation.</param>
    public AnnotationManager(PlotModel model, PlotView? plotView = null)
    {
        ArgumentNullException.ThrowIfNull(model);
        _model = model;
        _plotView = plotView;
    }

    #region Add/Remove Annotations

    /// <summary>
    /// Adds an annotation to the plot model.
    /// </summary>
    /// <param name="annotation">The annotation to add.</param>
    public void AddAnnotation(Annotation annotation)
    {
        ArgumentNullException.ThrowIfNull(annotation);

        _model.Annotations.Add(annotation);
        InvalidatePlot(false);
    }

    /// <summary>
    /// Removes an annotation from the plot model.
    /// </summary>
    /// <param name="annotation">The annotation to remove.</param>
    /// <returns>True if the annotation was removed, false if it wasn't found.</returns>
    public bool RemoveAnnotation(Annotation annotation)
    {
        ArgumentNullException.ThrowIfNull(annotation);

        var result = _model.Annotations.Remove(annotation);
        if (result)
        {
            InvalidatePlot(false);
        }
        return result;
    }

    /// <summary>
    /// Removes an annotation at the specified index.
    /// </summary>
    /// <param name="index">The index of the annotation to remove.</param>
    public void RemoveAnnotationAt(int index)
    {
        if (index < 0 || index >= _model.Annotations.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        _model.Annotations.RemoveAt(index);
        InvalidatePlot(false);
    }

    /// <summary>
    /// Removes all annotations from the plot model.
    /// </summary>
    public void ClearAnnotations()
    {
        _model.Annotations.Clear();
        InvalidatePlot(false);
    }

    #endregion

    #region Create Specific Annotations

    /// <summary>
    /// Creates a new line annotation with default properties.
    /// </summary>
    /// <param name="x1">Start X coordinate.</param>
    /// <param name="y1">Start Y coordinate.</param>
    /// <param name="x2">End X coordinate.</param>
    /// <param name="y2">End Y coordinate.</param>
    /// <returns>The created line annotation.</returns>
    public LineAnnotation CreateLineAnnotation(double x1, double y1, double x2, double y2)
    {
        return new LineAnnotation
        {
            X = x1,
            Y = y1,
            X2 = x2,
            Y2 = y2,
            Color = OxyColors.Black,
            StrokeThickness = 1,
            LineStyle = LineStyle.Solid
        };
    }

    /// <summary>
    /// Creates a new arrow annotation with default properties.
    /// </summary>
    /// <param name="x1">Start X coordinate.</param>
    /// <param name="y1">Start Y coordinate.</param>
    /// <param name="x2">End X coordinate (arrow head).</param>
    /// <param name="y2">End Y coordinate (arrow head).</param>
    /// <returns>The created arrow annotation.</returns>
    public ArrowAnnotation CreateArrowAnnotation(double x1, double y1, double x2, double y2)
    {
        return new ArrowAnnotation
        {
            StartPoint = new DataPoint(x1, y1),
            EndPoint = new DataPoint(x2, y2),
            Color = OxyColors.Black,
            StrokeThickness = 1,
            HeadLength = 10,
            HeadWidth = 6
        };
    }

    /// <summary>
    /// Creates a new text annotation with default properties.
    /// </summary>
    /// <param name="text">The text to display.</param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <returns>The created text annotation.</returns>
    public TextAnnotation CreateTextAnnotation(string text, double x, double y)
    {
        return new TextAnnotation
        {
            Text = text ?? string.Empty,
            TextPosition = new DataPoint(x, y),
            TextColor = OxyColors.Black,
            FontSize = 12,
            FontWeight = FontWeights.Normal
        };
    }

    /// <summary>
    /// Creates a new polygon annotation with default properties.
    /// </summary>
    /// <param name="points">The polygon vertices.</param>
    /// <returns>The created polygon annotation.</returns>
    public PolygonAnnotation CreatePolygonAnnotation(IEnumerable<DataPoint> points)
    {
        ArgumentNullException.ThrowIfNull(points);

        var polygon = new PolygonAnnotation
        {
            Fill = OxyColor.FromAColor(128, OxyColors.LightBlue),
            Stroke = OxyColors.Blue,
            StrokeThickness = 1
        };

        foreach (var point in points)
        {
            polygon.Points.Add(point);
        }

        return polygon;
    }

    #endregion

    #region Query Annotations

    /// <summary>
    /// Gets all annotations in the plot model.
    /// </summary>
    /// <returns>Collection of all annotations.</returns>
    public IEnumerable<Annotation> GetAllAnnotations() => _model.Annotations;

    /// <summary>
    /// Gets annotations of a specific type.
    /// </summary>
    /// <typeparam name="TAnnotation">The annotation type to filter by.</typeparam>
    /// <returns>Collection of annotations of the specified type.</returns>
    public IEnumerable<TAnnotation> GetAnnotationsOfType<TAnnotation>() where TAnnotation : Annotation
        => _model.Annotations.OfType<TAnnotation>();

    /// <summary>
    /// Gets the index of an annotation in the collection.
    /// </summary>
    /// <param name="annotation">The annotation to find.</param>
    /// <returns>The index of the annotation, or -1 if not found.</returns>
    public int GetAnnotationIndex(Annotation annotation)
    {
        ArgumentNullException.ThrowIfNull(annotation);
        return _model.Annotations.IndexOf(annotation);
    }

    /// <summary>
    /// Gets the number of annotations in the plot model.
    /// </summary>
    public int AnnotationCount => _model.Annotations.Count;

    #endregion

    #region Reorder Annotations

    /// <summary>
    /// Moves an annotation up in the drawing order (earlier in the collection).
    /// </summary>
    /// <param name="annotation">The annotation to move up.</param>
    /// <returns>True if the annotation was moved, false if it was already first or not found.</returns>
    public bool MoveAnnotationUp(Annotation annotation)
    {
        ArgumentNullException.ThrowIfNull(annotation);

        var index = _model.Annotations.IndexOf(annotation);
        if (index <= 0) return false; // Already first or not found

        _model.Annotations.Move(index, index - 1);
        InvalidatePlot(false);
        return true;
    }

    /// <summary>
    /// Moves an annotation down in the drawing order (later in the collection).
    /// </summary>
    /// <param name="annotation">The annotation to move down.</param>
    /// <returns>True if the annotation was moved, false if it was already last or not found.</returns>
    public bool MoveAnnotationDown(Annotation annotation)
    {
        ArgumentNullException.ThrowIfNull(annotation);

        var index = _model.Annotations.IndexOf(annotation);
        if (index < 0 || index >= _model.Annotations.Count - 1) return false; // Not found or already last

        _model.Annotations.Move(index, index + 1);
        InvalidatePlot(false);
        return true;
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
