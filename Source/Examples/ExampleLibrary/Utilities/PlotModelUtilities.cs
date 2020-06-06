// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModelUtilities.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides utility functions for PlotModel used in examples.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    /// <summary>
    /// Provides utility functions for PlotModel used in examples.
    /// </summary>
    public static class PlotModelUtilities
    {
        private const string XAXIS_KEY = "x";
        private const string YAXIS_KEY = "y";

        /// <summary>
        /// Lists all XYAxisSeries from the core library that are NOT reversible.
        /// </summary>
        private static readonly HashSet<Type> NonReversibleSeriesTypes = new HashSet<Type>
        {
        };

        /// <summary>
        /// Lists all Annotations that need axes and are NOT reversible.
        /// </summary>
        private static readonly HashSet<Type> NonReversibleDataSpaceAnnotationTypes = new HashSet<Type>
        {
            typeof(TileMapAnnotation),
        };

        /// <summary>
        /// Lists all XYAxisSeries from the core library that are NOT transposable.
        /// </summary>
        private static readonly HashSet<Type> NonTransposableSeriesTypes = new HashSet<Type>
        {
            typeof(CandleStickAndVolumeSeries),
            typeof(OldCandleStickSeries),
        };

        /// <summary>
        /// Lists all Annotations that need axes and are NOT transposable.
        /// </summary>
        private static readonly HashSet<Type> NonTransposableDataSpaceAnnotationTypes = new HashSet<Type>
        {
            typeof(TileMapAnnotation),
        };

        /// <summary>
        /// Returns a value indicating whether a plot model is reversible.
        /// </summary>
        /// <param name="model">The plot model.</param>
        /// <returns>True if the plot model in reversible; false otherwise.</returns>
        public static bool IsReversible(this PlotModel model)
        {
            return (model.Axes.Count > 0 || model.Series.Count > 0)
                && model.Axes.All(a => a.Position != AxisPosition.None)
                && model.Series.All(s =>
                {
                    var type = s.GetType();
                    return s is XYAxisSeries
                           && type.GetTypeInfo().Assembly == typeof(PlotModel).GetTypeInfo().Assembly
                           && !NonReversibleSeriesTypes.Contains(type);
                })
                && model.Annotations.All(a =>
                {
                    var type = a.GetType();
                    return !NonReversibleDataSpaceAnnotationTypes.Contains(type);
                });
        }

        /// <summary>
        /// Returns a value indicating whether a plot model is transposable.
        /// </summary>
        /// <param name="model">The plot model.</param>
        /// <returns>True if the plot model in transposable; false otherwise.</returns>
        public static bool IsTransposable(this PlotModel model)
        {
            return (model.Axes.Count > 0 || model.Series.Count > 0)
                && model.Axes.All(a => a.Position != AxisPosition.None)
                && model.Series.All(s =>
                {
                    var type = s.GetType();
                    return s is XYAxisSeries
                           && type.GetTypeInfo().Assembly == typeof(PlotModel).GetTypeInfo().Assembly
                           && !NonTransposableSeriesTypes.Contains(type);
                })
                && model.Annotations.All(a =>
                {
                    var type = a.GetType();
                    return !NonTransposableDataSpaceAnnotationTypes.Contains(type);
                });
        }

        /// <summary>
        /// Reverses the X Axis of a PlotModel. The given PlotModel is mutated and returned for convenience.
        /// </summary>
        /// <param name="model">The PlotModel.</param>
        /// <returns>The PlotModel with reversed X Axis.</returns>
        public static PlotModel ReverseXAxis(this PlotModel model)
        {
            if (!string.IsNullOrEmpty(model.Title))
            {
                model.Title += " (reversed X Axis)";
            }

            var foundXAxis = false;
            foreach (var axis in model.Axes)
            {
                switch (axis.Position)
                {
                    case AxisPosition.Bottom:
                        axis.StartPosition = 1 - axis.StartPosition;
                        axis.EndPosition = 1 - axis.EndPosition;
                        foundXAxis = true;
                        break;
                    case AxisPosition.Left:
                    case AxisPosition.Right:
                    case AxisPosition.Top:
                    case AxisPosition.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (!foundXAxis)
            {
                model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, StartPosition = 1, EndPosition = 0});
            }

            return model;
        }

        /// <summary>
        /// Reverses the Y Axis of a PlotModel. The given PlotModel is mutated and returned for convenience.
        /// </summary>
        /// <param name="model">The PlotModel.</param>
        /// <returns>The PlotModel with reversed Y Axis.</returns>
        public static PlotModel ReverseYAxis(this PlotModel model)
        {
            if (!string.IsNullOrEmpty(model.Title))
            {
                model.Title += " (reversed Y Axis)";
            }

            var foundYAxis = false;
            foreach (var axis in model.Axes)
            {
                switch (axis.Position)
                {
                    case AxisPosition.Left:
                        axis.StartPosition = 1 - axis.StartPosition;
                        axis.EndPosition = 1 - axis.EndPosition;
                        foundYAxis = true;
                        break;
                    case AxisPosition.Bottom:
                    case AxisPosition.Right:
                    case AxisPosition.Top:
                    case AxisPosition.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (!foundYAxis)
            {
                model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StartPosition = 1, EndPosition = 0});
            }

            return model;
        }

        /// <summary>
        /// Reverses X and Y Axes of a PlotModel. The given PlotModel is mutated and returned for convenience.
        /// </summary>
        /// <param name="model">The PlotModel.</param>
        /// <returns>The PlotModel with reversed X and Y Axis.</returns>
        public static PlotModel ReverseXandYAxes(this PlotModel model)
        {
            var title = model.Title;
            if (!string.IsNullOrEmpty(title))
            {
                title += " (reversed both Axes)";
            }

            model = model.ReverseXAxis().ReverseYAxis();
            model.Title = title;
            return model;
        }

        /// <summary>
        /// Reverses all axes of a PlotModel. The given PlotModel is mutated and returned for convenience.
        /// </summary>
        /// <param name="model">The PlotModel.</param>
        /// <returns>The PlotModel with reversed axes.</returns>
        public static PlotModel ReverseAllAxes(this PlotModel model)
        {
            if (!string.IsNullOrEmpty(model.Title))
            {
                model.Title += " (reversed all Axes)";
            }

            // Update plot to generate default axes etc.
            ((IPlotModel)model).Update(false);

            foreach (var axis in model.Axes)
            {
                switch (axis.Position)
                {
                    case AxisPosition.Left:
                    case AxisPosition.Bottom:
                    case AxisPosition.Right:
                    case AxisPosition.Top:
                        axis.StartPosition = 1 - axis.StartPosition;
                        axis.EndPosition = 1 - axis.EndPosition;
                        break;
                    case AxisPosition.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return model;
        }

        /// <summary>
        /// Transposes a PlotModel. The given PlotModel is mutated and returned for convenience.
        /// </summary>
        /// <param name="model">The PlotModel.</param>
        /// <returns>The transposed PlotModel.</returns>
        public static PlotModel Transpose(this PlotModel model)
        {
            if (!string.IsNullOrEmpty(model.Title))
            {
                model.Title += " (transposed)";
            }

            // Update plot to generate default axes etc.
            ((IPlotModel)model).Update(false);

            foreach (var axis in model.Axes)
            {
                switch (axis.Position)
                {
                    case AxisPosition.Bottom:
                        axis.Position = AxisPosition.Left;
                        break;
                    case AxisPosition.Left:
                        axis.Position = AxisPosition.Bottom;
                        break;
                    case AxisPosition.Right:
                        axis.Position = AxisPosition.Top;
                        break;
                    case AxisPosition.Top:
                        axis.Position = AxisPosition.Right;
                        break;
                    case AxisPosition.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (var annotation in model.Annotations)
            {
                if (annotation.XAxis != null && annotation.XAxisKey == null)
                {
                    if (annotation.XAxis.Key == null)
                    {
                        annotation.XAxis.Key = XAXIS_KEY;
                    }

                    annotation.XAxisKey = annotation.XAxis.Key;
                }

                if (annotation.YAxis != null && annotation.YAxisKey == null)
                {
                    if (annotation.YAxis.Key == null)
                    {
                        annotation.YAxis.Key = YAXIS_KEY;
                    }

                    annotation.YAxisKey = annotation.YAxis.Key;
                }
            }

            foreach (var series in model.Series.OfType<XYAxisSeries>())
            {
                if (series.XAxisKey == null)
                {
                    if (series.XAxis == null) // this can happen if the series is invisible initially
                    {
                        series.XAxisKey = XAXIS_KEY;
                    }
                    else
                    {
                        if (series.XAxis.Key == null)
                        {
                            series.XAxis.Key = XAXIS_KEY;
                        }

                        series.XAxisKey = series.XAxis.Key;
                    }
                }

                if (series.YAxisKey == null)
                {
                    if (series.YAxis == null)
                    {
                        series.YAxisKey = YAXIS_KEY;
                    }
                    else
                    {
                        if (series.YAxis.Key == null)
                        {
                            series.YAxis.Key = YAXIS_KEY;
                        }

                        series.YAxisKey = series.YAxis.Key;
                    }
                }
            }

            return model;
        }
    }
}
