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
    using System.Linq;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public static class PlotModelUtilities
    {
        private const string XAXIS_KEY = "x";
        private const string YAXIS_KEY = "y";

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
        public static PlotModel ReverseAxes(this PlotModel model)
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

            var foundXAxis = false;
            var foundYAxis = false;

            foreach (var axis in model.Axes)
            {
                switch (axis.Position)
                {
                    case AxisPosition.Bottom:
                        axis.Position = AxisPosition.Left;
                        if (axis.Key == null)
                        {
                            axis.Key = XAXIS_KEY;
                        }

                        foundXAxis = true;
                        break;
                    case AxisPosition.Left:
                        axis.Position = AxisPosition.Bottom;
                        if (axis.Key == null)
                        {
                            axis.Key = YAXIS_KEY;
                        }

                        foundYAxis = true;
                        break;
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
                model.Axes.Add(new LinearAxis { Key = XAXIS_KEY, Position = AxisPosition.Left });
            }

            if (!foundYAxis)
            {
                model.Axes.Add(new LinearAxis { Key = YAXIS_KEY, Position = AxisPosition.Bottom });
            }

            foreach (var series in model.Series.OfType<XYAxisSeries>())
            {
                if (series.XAxisKey == null)
                {
                    series.XAxisKey = XAXIS_KEY;
                }

                if (series.YAxisKey == null)
                {
                    series.YAxisKey = YAXIS_KEY;
                }
            }

            return model;
        }
    }
}
