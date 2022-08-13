﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerControl.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   The tracker control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using DependencyPropertyGenerator;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// The tracker control.
    /// </summary>
    [DependencyProperty<Visibility>("HorizontalLineVisibility", DefaultValue = Visibility.Visible,
        Description = "Gets or sets HorizontalLineVisibility.")]
    [DependencyProperty<Visibility>("VerticalLineVisibility", DefaultValue = Visibility.Visible,
        Description = "Gets or sets VerticalLineVisibility.")]
    [DependencyProperty<double>("LineThickness", DefaultValue = 1.0,
        Description = "Gets or sets LineThickness.")]
    [DependencyProperty<Brush>("LineStroke",
        Description = "Gets or sets LineStroke.")]
    [DependencyProperty<OxyRect>("LineExtents",
        Description = "Gets or sets LineExtents.")]
    [DependencyProperty<DoubleCollection>("LineDashArray",
        Description = "Gets or sets LineDashArray.")]
    [DependencyProperty<bool>("ShowPointer", DefaultValue = true,
        Description = "Gets or sets a value indicating whether to show a 'pointer' on the border.")]
    [DependencyProperty<double>("Distance", DefaultValue = 7.0,
        Description = "Gets or sets the distance of the content container from the trackers Position.")]
    [DependencyProperty<bool>("CanCenterHorizontally", DefaultValue = true,
        Description = "Gets or sets a value indicating whether the tracker can center its content box horizontally.")]
    [DependencyProperty<bool>("CanCenterVertically", DefaultValue = true,
        Description = "Gets or sets a value indicating whether the tracker can center its content box vertically.")]
    [DependencyProperty<ScreenPoint>("Position",
        Description = "Gets or sets Position of the tracker.")]
    [DependencyProperty<EdgeMode>("BorderEdgeMode",
        Description = "Gets or sets BorderEdgeMode.")]
    [DependencyProperty<double>("CornerRadius",
        Description = "Gets or sets the corner radius (only used when ShowPoint=false).")]
    public partial class TrackerControl : ContentControl
    {
        /// <summary>
        /// The path part string.
        /// </summary>
        private const string PartPath = "PART_Path";

        /// <summary>
        /// The content part string.
        /// </summary>
        private const string PartContent = "PART_Content";

        /// <summary>
        /// The content container part string.
        /// </summary>
        private const string PartContentcontainer = "PART_ContentContainer";

        /// <summary>
        /// The horizontal line part string.
        /// </summary>
        private const string PartHorizontalline = "PART_HorizontalLine";

        /// <summary>
        /// The vertical line part string.
        /// </summary>
        private const string PartVerticalline = "PART_VerticalLine";

        /// <summary>
        /// The content.
        /// </summary>
        private ContentPresenter content;

        /// <summary>
        /// The horizontal line.
        /// </summary>
        private Line horizontalLine;

        /// <summary>
        /// The path.
        /// </summary>
        private Path path;

        /// <summary>
        /// The content container.
        /// </summary>
        private Grid contentContainer;

        /// <summary>
        /// The vertical line.
        /// </summary>
        private Line verticalLine;

        /// <summary>
        /// Initializes static members of the <see cref = "TrackerControl" /> class.
        /// </summary>
        static TrackerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TrackerControl), new FrameworkPropertyMetadata(typeof(TrackerControl)));
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.path = this.GetTemplateChild(PartPath) as Path;
            this.content = this.GetTemplateChild(PartContent) as ContentPresenter;
            this.contentContainer = this.GetTemplateChild(PartContentcontainer) as Grid;
            this.horizontalLine = this.GetTemplateChild(PartHorizontalline) as Line;
            this.verticalLine = this.GetTemplateChild(PartVerticalline) as Line;

            if (this.contentContainer == null)
            {
                throw new InvalidOperationException($"The TrackerControl template must contain a content container with name +'{PartContentcontainer}'");
            }

            if (this.path == null)
            {
                throw new InvalidOperationException($"The TrackerControl template must contain a Path with name +'{PartPath}'");
            }

            if (this.content == null)
            {
                throw new InvalidOperationException($"The TrackerControl template must contain a ContentPresenter with name +'{PartContent}'");
            }

            this.UpdatePositionAndBorder();
        }

        /// <summary>
        /// Called when the position is changed.
        /// </summary>
        // ReSharper disable once UnusedParameter.Local
        partial void OnPositionChanged(ScreenPoint oldValue, ScreenPoint newValue)
        {
            this.UpdatePositionAndBorder();
        }

        /// <summary>
        /// Update the position and border of the tracker.
        /// </summary>
        private void UpdatePositionAndBorder()
        {
            if (this.contentContainer == null)
            {
                return;
            }

            Canvas.SetLeft(this.contentContainer, this.Position.X);
            Canvas.SetTop(this.contentContainer, this.Position.Y);
            FrameworkElement parent = this;
            while (!(parent is Canvas) && parent != null)
            {
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }

            if (parent == null)
            {
                return;
            }

            // throw new InvalidOperationException("The TrackerControl must have a Canvas parent.");
            double canvasWidth = parent.ActualWidth;
            double canvasHeight = parent.ActualHeight;

            this.content.Measure(new Size(canvasWidth, canvasHeight));
            this.content.Arrange(new Rect(0, 0, this.content.DesiredSize.Width, this.content.DesiredSize.Height));

            double contentWidth = this.content.DesiredSize.Width;
            double contentHeight = this.content.DesiredSize.Height;

            // Minimum allowed margins around the tracker
            const double MarginLimit = 10;

            var ha = HorizontalAlignment.Center;
            if (this.CanCenterHorizontally)
            {
                if (this.Position.X - (contentWidth / 2) < MarginLimit)
                {
                    ha = HorizontalAlignment.Left;
                }

                if (this.Position.X + (contentWidth / 2) > canvasWidth - MarginLimit)
                {
                    ha = HorizontalAlignment.Right;
                }
            }
            else
            {
                ha = this.Position.X < canvasWidth / 2 ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            }

            var va = VerticalAlignment.Center;
            if (this.CanCenterVertically)
            {
                if (this.Position.Y - (contentHeight / 2) < MarginLimit)
                {
                    va = VerticalAlignment.Top;
                }

                if (ha == HorizontalAlignment.Center)
                {
                    va = VerticalAlignment.Bottom;
                    if (this.Position.Y - contentHeight < MarginLimit)
                    {
                        va = VerticalAlignment.Top;
                    }
                }

                if (va == VerticalAlignment.Center && this.Position.Y + (contentHeight / 2) > canvasHeight - MarginLimit)
                {
                    va = VerticalAlignment.Bottom;
                }

                if (va == VerticalAlignment.Top && this.Position.Y + contentHeight > canvasHeight - MarginLimit)
                {
                    va = VerticalAlignment.Bottom;
                }
            }
            else
            {
                va = this.Position.Y < canvasHeight / 2 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
            }

            double dx = ha == HorizontalAlignment.Center ? -0.5 : ha == HorizontalAlignment.Left ? 0 : -1;
            double dy = va == VerticalAlignment.Center ? -0.5 : va == VerticalAlignment.Top ? 0 : -1;

            this.path.Data = this.ShowPointer
                                 ? this.CreatePointerBorderGeometry(ha, va, contentWidth, contentHeight, out Thickness margin)
                                 : this.CreateBorderGeometry(ha, va, contentWidth, contentHeight, out margin);

            this.content.Margin = margin;

            this.contentContainer.Measure(new Size(canvasWidth, canvasHeight));
            var contentSize = this.contentContainer.DesiredSize;

            this.contentContainer.RenderTransform = new TranslateTransform
                {
                    X = dx * contentSize.Width,
                    Y = dy * contentSize.Height
                };

            var pos = this.Position;

            if (this.horizontalLine != null)
            {
                if (this.LineExtents.Width > 0)
                {
                    this.horizontalLine.X1 = this.LineExtents.Left;
                    this.horizontalLine.X2 = this.LineExtents.Right;
                }
                else
                {
                    this.horizontalLine.X1 = 0;
                    this.horizontalLine.X2 = canvasWidth;
                }

                this.horizontalLine.Y1 = pos.Y;
                this.horizontalLine.Y2 = pos.Y;
            }

            if (this.verticalLine != null)
            {
                if (this.LineExtents.Width > 0)
                {
                    this.verticalLine.Y1 = this.LineExtents.Top;
                    this.verticalLine.Y2 = this.LineExtents.Bottom;
                }
                else
                {
                    this.verticalLine.Y1 = 0;
                    this.verticalLine.Y2 = canvasHeight;
                }

                this.verticalLine.X1 = pos.X;
                this.verticalLine.X2 = pos.X;
            }
        }

        /// <summary>
        /// Create the border geometry.
        /// </summary>
        /// <param name="ha">The horizontal alignment.</param>
        /// <param name="va">The vertical alignment.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="margin">The margin.</param>
        /// <returns>The border geometry.</returns>
        private Geometry CreateBorderGeometry(
            HorizontalAlignment ha, VerticalAlignment va, double width, double height, out Thickness margin)
        {
            double m = this.Distance;
            var rect = new Rect(
                ha == HorizontalAlignment.Left ? m : 0, va == VerticalAlignment.Top ? m : 0, width, height);
            margin = new Thickness(
                ha == HorizontalAlignment.Left ? m : 0,
                va == VerticalAlignment.Top ? m : 0,
                ha == HorizontalAlignment.Right ? m : 0,
                va == VerticalAlignment.Bottom ? m : 0);
            return new RectangleGeometry { Rect = rect, RadiusX = this.CornerRadius, RadiusY = this.CornerRadius };
        }

        /// <summary>
        /// Create a border geometry with a 'pointer'.
        /// </summary>
        /// <param name="ha">The horizontal alignment.</param>
        /// <param name="va">The vertical alignment.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="margin">The margin.</param>
        /// <returns>The border geometry.</returns>
        private Geometry CreatePointerBorderGeometry(
            HorizontalAlignment ha, VerticalAlignment va, double width, double height, out Thickness margin)
        {
            Point[] points = null;
            double m = this.Distance;
            margin = new Thickness();

            if (ha == HorizontalAlignment.Center && va == VerticalAlignment.Bottom)
            {
                double x0 = 0;
                double x1 = width;
                double x2 = (x0 + x1) / 2;
                double y0 = 0;
                double y1 = height;
                margin = new Thickness(0, 0, 0, m);
                points = new[]
                    {
                        new Point(x0, y0), new Point(x1, y0), new Point(x1, y1), new Point(x2 + (m / 2), y1),
                        new Point(x2, y1 + m), new Point(x2 - (m / 2), y1), new Point(x0, y1)
                    };
            }

            if (ha == HorizontalAlignment.Center && va == VerticalAlignment.Top)
            {
                double x0 = 0;
                double x1 = width;
                double x2 = (x0 + x1) / 2;
                double y0 = m;
                double y1 = m + height;
                margin = new Thickness(0, m, 0, 0);
                points = new[]
                    {
                        new Point(x0, y0), new Point(x2 - (m / 2), y0), new Point(x2, 0), new Point(x2 + (m / 2), y0),
                        new Point(x1, y0), new Point(x1, y1), new Point(x0, y1)
                    };
            }

            if (ha == HorizontalAlignment.Left && va == VerticalAlignment.Center)
            {
                double x0 = m;
                double x1 = m + width;
                double y0 = 0;
                double y1 = height;
                double y2 = (y0 + y1) / 2;
                margin = new Thickness(m, 0, 0, 0);
                points = new[]
                    {
                        new Point(0, y2), new Point(x0, y2 - (m / 2)), new Point(x0, y0), new Point(x1, y0),
                        new Point(x1, y1), new Point(x0, y1), new Point(x0, y2 + (m / 2))
                    };
            }

            if (ha == HorizontalAlignment.Right && va == VerticalAlignment.Center)
            {
                double x0 = 0;
                double x1 = width;
                double y0 = 0;
                double y1 = height;
                double y2 = (y0 + y1) / 2;
                margin = new Thickness(0, 0, m, 0);
                points = new[]
                    {
                        new Point(x1 + m, y2), new Point(x1, y2 + (m / 2)), new Point(x1, y1), new Point(x0, y1),
                        new Point(x0, y0), new Point(x1, y0), new Point(x1, y2 - (m / 2))
                    };
            }

            if (ha == HorizontalAlignment.Left && va == VerticalAlignment.Top)
            {
                m *= 0.67;
                double x0 = m;
                double x1 = m + width;
                double y0 = m;
                double y1 = m + height;
                margin = new Thickness(m, m, 0, 0);
                points = new[]
                    {
                        new Point(0, 0), new Point(m * 2, y0), new Point(x1, y0), new Point(x1, y1), new Point(x0, y1),
                        new Point(x0, m * 2)
                    };
            }

            if (ha == HorizontalAlignment.Right && va == VerticalAlignment.Top)
            {
                m *= 0.67;
                double x0 = 0;
                double x1 = width;
                double y0 = m;
                double y1 = m + height;
                margin = new Thickness(0, m, m, 0);
                points = new[]
                    {
                        new Point(x1 + m, 0), new Point(x1, y0 + m), new Point(x1, y1), new Point(x0, y1),
                        new Point(x0, y0), new Point(x1 - m, y0)
                    };
            }

            if (ha == HorizontalAlignment.Left && va == VerticalAlignment.Bottom)
            {
                m *= 0.67;
                double x0 = m;
                double x1 = m + width;
                double y0 = 0;
                double y1 = height;
                margin = new Thickness(m, 0, 0, m);
                points = new[]
                    {
                        new Point(0, y1 + m), new Point(x0, y1 - m), new Point(x0, y0), new Point(x1, y0),
                        new Point(x1, y1), new Point(x0 + m, y1)
                    };
            }

            if (ha == HorizontalAlignment.Right && va == VerticalAlignment.Bottom)
            {
                m *= 0.67;
                double x0 = 0;
                double x1 = width;
                double y0 = 0;
                double y1 = height;
                margin = new Thickness(0, 0, m, m);
                points = new[]
                    {
                        new Point(x1 + m, y1 + m), new Point(x1 - m, y1), new Point(x0, y1), new Point(x0, y0),
                        new Point(x1, y0), new Point(x1, y1 - m)
                    };
            }

            if (points == null)
            {
                return null;
            }

            var pc = new PointCollection(points.Length);
            foreach (var p in points)
            {
                pc.Add(p);
            }

            var segments = new PathSegmentCollection { new PolyLineSegment { Points = pc } };
            var pf = new PathFigure { StartPoint = points[0], Segments = segments, IsClosed = true };
            return new PathGeometry { Figures = new PathFigureCollection { pf } };
        }
    }
}
