// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerControl.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The tracker control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Controls;
    using global::Avalonia.Controls.Presenters;
    using global::Avalonia.Controls.Primitives;
    using global::Avalonia.Controls.Shapes;
    using global::Avalonia.Layout;
    using global::Avalonia.Markup.Xaml.Data;
    using global::Avalonia.Media;
    using global::Avalonia.VisualTree;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The tracker control.
    /// </summary>
    public class TrackerControl : ContentControl
    {
        /// <summary>
        /// Identifies the <see cref="HorizontalLineVisibility"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> HorizontalLineVisibilityProperty = AvaloniaProperty.Register<TrackerControl, bool>(nameof(HorizontalLineVisibility), true);

        /// <summary>
        /// Identifies the <see cref="VerticalLineVisibility"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> VerticalLineVisibilityProperty = AvaloniaProperty.Register<TrackerControl, bool>(nameof(VerticalLineVisibility), true);

        /// <summary>
        /// Identifies the <see cref="LineStroke"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<IBrush> LineStrokeProperty = AvaloniaProperty.Register<TrackerControl, IBrush>(nameof(LineStroke));

        /// <summary>
        /// Identifies the <see cref="LineExtents"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<OxyRect> LineExtentsProperty = AvaloniaProperty.Register<TrackerControl, OxyRect>(nameof(LineExtents), new OxyRect());

        /// <summary>
        /// Identifies the <see cref="LineDashArray"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<List<double>> LineDashArrayProperty = AvaloniaProperty.Register<TrackerControl, List<double>>(nameof(LineDashArray));
        
        /// <summary>
        /// Identifies the <see cref="ShowPointer"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> ShowPointerProperty = AvaloniaProperty.Register<TrackerControl, bool>(nameof(ShowPointer), true);

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> CornerRadiusProperty = AvaloniaProperty.Register<TrackerControl, double>(nameof(CornerRadius), 0.0);

        /// <summary>
        /// Identifies the <see cref="Distance"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> DistanceProperty = AvaloniaProperty.Register<TrackerControl, double>(nameof(Distance), 7.0);

        /// <summary>
        /// Identifies the <see cref="CanCenterHorizontally"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> CanCenterHorizontallyProperty = AvaloniaProperty.Register<TrackerControl, bool>(nameof(CanCenterHorizontally), true);

        /// <summary>
        /// Identifies the <see cref="CanCenterVertically"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> CanCenterVerticallyProperty = AvaloniaProperty.Register<TrackerControl, bool>(nameof(CanCenterVertically), true);

        /// <summary>
        /// Identifies the <see cref="Position"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<ScreenPoint> PositionProperty = AvaloniaProperty.Register<TrackerControl, ScreenPoint>(nameof(Position), new ScreenPoint());

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
        private const string PartContentContainer = "PART_ContentContainer";

        /// <summary>
        /// The horizontal line part string.
        /// </summary>
        private const string PartHorizontalLine = "PART_HorizontalLine";

        /// <summary>
        /// The vertical line part string.
        /// </summary>
        private const string PartVerticalLine = "PART_VerticalLine";

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
        private Panel contentContainer;

        /// <summary>
        /// The vertical line.
        /// </summary>
        private Line verticalLine;

        /// <summary>
        /// Initializes static members of the <see cref = "TrackerControl" /> class.
        /// </summary>
        static TrackerControl()
        {
            ClipToBoundsProperty.OverrideDefaultValue<TrackerControl>(false);
            PositionProperty.Changed.AddClassHandler<TrackerControl>(PositionChanged);
        }

        /// <summary>
        /// Gets or sets HorizontalLineVisibility.
        /// </summary>
        public bool HorizontalLineVisibility
        {
            get
            {
                return GetValue(HorizontalLineVisibilityProperty);
            }

            set
            {
                SetValue(HorizontalLineVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets VerticalLineVisibility.
        /// </summary>
        public bool VerticalLineVisibility
        {
            get
            {
                return GetValue(VerticalLineVisibilityProperty);
            }

            set
            {
                SetValue(VerticalLineVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineStroke.
        /// </summary>
        public IBrush LineStroke
        {
            get
            {
                return GetValue(LineStrokeProperty);
            }

            set
            {
                SetValue(LineStrokeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineExtents.
        /// </summary>
        public OxyRect LineExtents
        {
            get
            {
                return GetValue(LineExtentsProperty);
            }

            set
            {
                SetValue(LineExtentsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineDashArray.
        /// </summary>
        public List<double> LineDashArray
        {
            get
            {
                return GetValue(LineDashArrayProperty);
            }

            set
            {
                SetValue(LineDashArrayProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show a 'pointer' on the border.
        /// </summary>
        public bool ShowPointer
        {
            get
            {
                return GetValue(ShowPointerProperty);
            }

            set
            {
                SetValue(ShowPointerProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the corner radius (only used when ShowPoint=<c>false</c>).
        /// </summary>
        public double CornerRadius
        {
            get
            {
                return GetValue(CornerRadiusProperty);
            }

            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the distance of the content container from the trackers Position.
        /// </summary>
        public double Distance
        {
            get
            {
                return GetValue(DistanceProperty);
            }

            set
            {
                SetValue(DistanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tracker can center its content box horizontally.
        /// </summary>
        public bool CanCenterHorizontally
        {
            get
            {
                return GetValue(CanCenterHorizontallyProperty);
            }

            set
            {
                SetValue(CanCenterHorizontallyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tracker can center its content box vertically.
        /// </summary>
        public bool CanCenterVertically
        {
            get
            {
                return GetValue(CanCenterVerticallyProperty);
            }

            set
            {
                SetValue(CanCenterVerticallyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Position of the tracker.
        /// </summary>
        public ScreenPoint Position
        {
            get
            {
                return GetValue(PositionProperty);
            }

            set
            {
                SetValue(PositionProperty, value);
            }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);
            path = e.NameScope.Get<Path>(PartPath);
            content = e.NameScope.Get<ContentPresenter>(PartContent);
            contentContainer = e.NameScope.Get<Panel>(PartContentContainer);
            horizontalLine = e.NameScope.Find<Line>(PartHorizontalLine);
            verticalLine = e.NameScope.Find<Line>(PartVerticalLine);

            UpdatePositionAndBorder();

        }

        /// <summary>
        /// Called when the position is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void PositionChanged(AvaloniaObject sender, AvaloniaPropertyChangedEventArgs e)
        {
            ((TrackerControl)sender).OnPositionChanged(e);
        }

        /// <summary>
        /// Called when the position is changed.
        /// </summary>
        /// <param name="dependencyPropertyChangedEventArgs">The dependency property changed event args.</param>
        // ReSharper disable once UnusedParameter.Local
        private void OnPositionChanged(AvaloniaPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            UpdatePositionAndBorder();
        }

        /// <summary>
        /// Update the position and border of the tracker.
        /// </summary>
        private void UpdatePositionAndBorder()
        {
            if (contentContainer == null)
            {
                return;
            }

            Canvas.SetLeft(this, Position.X);
            Canvas.SetTop(this, Position.Y);
            Control parent = this;
            while (!(parent is Canvas) && parent != null)
            {
                parent = parent.GetVisualParent() as Control;
            }

            if (parent == null)
            {
                return;
            }

            // throw new InvalidOperationException("The TrackerControl must have a Canvas parent.");
            var canvasWidth = parent.Bounds.Width;
            var canvasHeight = parent.Bounds.Height;

            content.Measure(new Size(canvasWidth, canvasHeight));
            content.Arrange(new Rect(0, 0, content.DesiredSize.Width, content.DesiredSize.Height));

            var contentWidth = content.DesiredSize.Width;
            var contentHeight = content.DesiredSize.Height;

            // Minimum allowed margins around the tracker
            const double MarginLimit = 10;

            var ha = HorizontalAlignment.Center;
            if (CanCenterHorizontally)
            {
                if (Position.X - (contentWidth / 2) < MarginLimit)
                {
                    ha = HorizontalAlignment.Left;
                }

                if (Position.X + (contentWidth / 2) > canvasWidth - MarginLimit)
                {
                    ha = HorizontalAlignment.Right;
                }
            }
            else
            {
                ha = Position.X < canvasWidth / 2 ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            }

            var va = VerticalAlignment.Center;
            if (CanCenterVertically)
            {
                if (Position.Y - (contentHeight / 2) < MarginLimit)
                {
                    va = VerticalAlignment.Top;
                }

                if (ha == HorizontalAlignment.Center)
                {
                    va = VerticalAlignment.Bottom;
                    if (Position.Y - contentHeight < MarginLimit)
                    {
                        va = VerticalAlignment.Top;
                    }
                }

                if (va == VerticalAlignment.Center && Position.Y + (contentHeight / 2) > canvasHeight - MarginLimit)
                {
                    va = VerticalAlignment.Bottom;
                }

                if (va == VerticalAlignment.Top && Position.Y + contentHeight > canvasHeight - MarginLimit)
                {
                    va = VerticalAlignment.Bottom;
                }
            }
            else
            {
                va = Position.Y < canvasHeight / 2 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
            }

            var dx = ha == HorizontalAlignment.Center ? -0.5 : ha == HorizontalAlignment.Left ? 0 : -1;
            var dy = va == VerticalAlignment.Center ? -0.5 : va == VerticalAlignment.Top ? 0 : -1;

            Thickness margin;
            path.Data = ShowPointer
                                 ? CreatePointerBorderGeometry(ha, va, contentWidth, contentHeight, out margin)
                                 : CreateBorderGeometry(ha, va, contentWidth, contentHeight, out margin);

            content.Margin = margin;

            contentContainer.Measure(new Size(canvasWidth, canvasHeight));
            var contentSize = contentContainer.DesiredSize;

            contentContainer.RenderTransform = new TranslateTransform
                {
                    X = dx * contentSize.Width,
                    Y = dy * contentSize.Height
                };

            var pos = Position;

            if (horizontalLine != null)
            {
                if (LineExtents.Width > 0)
                {
                    horizontalLine.StartPoint = horizontalLine.StartPoint.WithX(LineExtents.Left);
                    horizontalLine.EndPoint = horizontalLine.StartPoint.WithX(LineExtents.Right);
                }
                else
                {
                    horizontalLine.StartPoint = horizontalLine.StartPoint.WithX(0);
                    horizontalLine.EndPoint = horizontalLine.StartPoint.WithX(canvasWidth);
                }

                horizontalLine.StartPoint = horizontalLine.StartPoint.WithY(pos.Y);
                horizontalLine.EndPoint = horizontalLine.EndPoint.WithY(pos.Y);
            }

            if (verticalLine != null)
            {
                if (LineExtents.Width > 0)
                {
                    horizontalLine.StartPoint = horizontalLine.StartPoint.WithY(LineExtents.Top);
                    horizontalLine.EndPoint = horizontalLine.StartPoint.WithY(LineExtents.Bottom);
                }
                else
                {
                    horizontalLine.StartPoint = horizontalLine.StartPoint.WithY(0);
                    horizontalLine.EndPoint = horizontalLine.StartPoint.WithY(canvasHeight);
                }

                horizontalLine.StartPoint = horizontalLine.StartPoint.WithY(pos.X);
                horizontalLine.EndPoint = horizontalLine.EndPoint.WithY(pos.X);
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
            var m = Distance;
            var rect = new Rect(
                ha == HorizontalAlignment.Left ? m : 0, va == VerticalAlignment.Top ? m : 0, width, height);
            margin = new Thickness(
                ha == HorizontalAlignment.Left ? m : 0,
                va == VerticalAlignment.Top ? m : 0,
                ha == HorizontalAlignment.Right ? m : 0,
                va == VerticalAlignment.Bottom ? m : 0);
            return new RectangleGeometry(rect)/* { RadiusX = this.CornerRadius, RadiusY = this.CornerRadius }*/;
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
            var m = Distance;
            margin = new Thickness();

            if (ha == HorizontalAlignment.Center && va == VerticalAlignment.Bottom)
            {
                double x0 = 0;
                var x1 = width;
                var x2 = (x0 + x1) / 2;
                double y0 = 0;
                var y1 = height;
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
                var x1 = width;
                var x2 = (x0 + x1) / 2;
                var y0 = m;
                var y1 = m + height;
                margin = new Thickness(0, m, 0, 0);
                points = new[]
                    {
                        new Point(x0, y0), new Point(x2 - (m / 2), y0), new Point(x2, 0), new Point(x2 + (m / 2), y0),
                        new Point(x1, y0), new Point(x1, y1), new Point(x0, y1)
                    };
            }

            if (ha == HorizontalAlignment.Left && va == VerticalAlignment.Center)
            {
                var x0 = m;
                var x1 = m + width;
                double y0 = 0;
                var y1 = height;
                var y2 = (y0 + y1) / 2;
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
                var x1 = width;
                double y0 = 0;
                var y1 = height;
                var y2 = (y0 + y1) / 2;
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
                var x0 = m;
                var x1 = m + width;
                var y0 = m;
                var y1 = m + height;
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
                var x1 = width;
                var y0 = m;
                var y1 = m + height;
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
                var x0 = m;
                var x1 = m + width;
                double y0 = 0;
                var y1 = height;
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
                var x1 = width;
                double y0 = 0;
                var y1 = height;
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

            var pc = new List<Point>(points.Length);
            foreach (var p in points)
            {
                pc.Add(p);
            }
            var segments = new PathSegments();
            segments.AddRange(pc.Select(p => new LineSegment { Point = p }));
            var pf = new PathFigure { StartPoint = points[0], Segments = segments, IsClosed = true };
            return new PathGeometry { Figures = new PathFigures { pf } };
        }
    }
}