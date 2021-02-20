﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    [Examples("Tracker")]
    public static class TrackerExamples
    {
        [Example("No interpolation")]
        public static PlotModel NoInterpolation()
        {
            var model = new PlotModel
            {
                Title = "No tracker interpolation",
                Subtitle = "Used for discrete values or scatter plots.",
            };
            var l = new Legend
            {
                LegendSymbolLength = 30
            };

            model.Legends.Add(l);

            var s1 = new LineSeries
            {
                Title = "Series 1",
                CanTrackerInterpolatePoints = false,
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 6,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5
            };
            for (int i = 0; i < 63; i++)
            {
                s1.Points.Add(new DataPoint((int)(Math.Sqrt(i) * Math.Cos(i * 0.1)), (int)(Math.Sqrt(i) * Math.Sin(i * 0.1))));
            }

            model.Series.Add(s1);

            return model;
        }

        [Example("TrackerChangedEvent")]
        public static PlotModel TrackerChangedEvent()
        {
            var model = new PlotModel
            {
                Title = "Handling the TrackerChanged event",
                Subtitle = "Press the left mouse button to test the tracker.",
            };
            model.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 100));
            model.TrackerChanged += (s, e) =>
            {
                model.Subtitle = e.HitResult != null ? "Tracker item index = " + e.HitResult.Index : "Not tracking";
                model.InvalidatePlot(false);
            };
            return model;
        }

        [Example("Specified distance of the tracker fires")]
        public static Example TrackerFiresDistance()
        {
            var model = new PlotModel
            {
                Title = "Specified distance of the tracker fires",
                Subtitle = "Press the left mouse button to test the tracker.",
            };
            model.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 100));
            
            // create a new plot controller with default bindings
            var plotController = new PlotController();

            // remove a tracker command to the mouse-left/touch down event by default
            plotController.Unbind(PlotCommands.SnapTrack);
            plotController.Unbind(PlotCommands.SnapTrackTouch);
            
            // add a tracker command to the mouse-left/touch down event with specified distance
            plotController.BindMouseDown(
                OxyMouseButton.Left,
                new DelegatePlotCommand<OxyMouseDownEventArgs>((view, controller, args) =>
                    controller.AddMouseManipulator(
                        view,
                        new TrackerManipulator(view)
                        {
                            Snap = true,
                            PointsOnly = false,
                            FiresDistance = 2.0,
                            CheckDistanceBetweenPoints = true,
                        },
                        args)));
            plotController.BindTouchDown(
                new DelegatePlotCommand<OxyTouchEventArgs>((view, controller, args) =>
                    controller.AddTouchManipulator(
                        view,
                        new TouchTrackerManipulator(view)
                        {
                            Snap = true,
                            PointsOnly = false,
                            FiresDistance = 2.0,
                        },
                        args)));

            return new Example(model, plotController);
        }
    }
}
