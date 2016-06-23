﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerControl.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SharpDX.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// Provides a tracker control.
    /// </summary>
    public class TrackerControl : ContentControl
    {
        /// <summary>
        /// Identifies the <see cref="HorizontalLineVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalLineVisibilityProperty =
            DependencyProperty.Register(
                "HorizontalLineVisibility",
                typeof(Visibility),
                typeof(TrackerControl),
                new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Identifies the <see cref="VerticalLineVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalLineVisibilityProperty =
            DependencyProperty.Register(
                "VerticalLineVisibility",
                typeof(Visibility),
                typeof(TrackerControl),
                new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Identifies the <see cref="LineStroke"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineStrokeProperty = DependencyProperty.Register(
            "LineStroke", typeof(Brush), typeof(TrackerControl), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LineExtents"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineExtentsProperty = DependencyProperty.Register(
            "LineExtents", typeof(OxyRect), typeof(TrackerControl), new PropertyMetadata(new OxyRect()));

        /// <summary>
        /// Identifies the <see cref="LineDashArray"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineDashArrayProperty = DependencyProperty.Register(
            "LineDashArray", typeof(DoubleCollection), typeof(TrackerControl), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="BorderEdgeMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BorderEdgeModeProperty = DependencyProperty.Register(
            "BorderEdgeMode", typeof(EdgeMode), typeof(TrackerControl));

        /// <summary>
        /// Identifies the <see cref="ShowPointer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowPointerProperty = DependencyProperty.Register(
            "ShowPointer", typeof(bool), typeof(TrackerControl), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(double), typeof(TrackerControl), new PropertyMetadata(0.0));

        /// <summary>
        /// Identifies the <see cref="Distance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DistanceProperty = DependencyProperty.Register(
            "Distance", typeof(double), typeof(TrackerControl), new PropertyMetadata(7.0));

        /// <summary>
        /// Identifies the <see cref="CanCenterHorizontally"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanCenterHorizontallyProperty =
            DependencyProperty.Register(
                "CanCenterHorizontally", typeof(bool), typeof(TrackerControl), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="CanCenterVertically"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanCenterVerticallyProperty =
            DependencyProperty.Register(
                "CanCenterVertically", typeof(bool), typeof(TrackerControl), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="Position"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position",
            typeof(ScreenPoint),
            typeof(TrackerControl),
            new PropertyMetadata(new ScreenPoint(), PositionChanged));

        /// <summary>
        /// The path part.
        /// </summary>
        private const string PartPath = "PART_Path";

        /// <summary>
        /// The content part.
        /// </summary>
        private const string PartContent = "PART_Content";

        /// <summary>
        /// The content container part.
        /// </summary>
        private const string PartContentcontainer = "PART_ContentContainer";

        /// <summary>
        /// The horizontal line part.
        /// </summary>
        private const string PartHorizontalline = "PART_HorizontalLine";

        /// <summary>
        /// The vertical line part.
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
        /// Initializes static members of the <see cref="TrackerControl" /> class.
        /// </summary>
        static TrackerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TrackerControl), new FrameworkPropertyMetadata(typeof(TrackerControl)));
        }
        
        /// <summary>
        /// Gets or sets BorderEdgeMode.
        /// </summary>
        public EdgeMode BorderEdgeMode
        {
            get
            {
                return (EdgeMode)this.GetValue(BorderEdgeModeProperty);
            }

            set
            {
                this.SetValue(BorderEdgeModeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets HorizontalLineVisibility.
        /// </summary>
        public Visibility HorizontalLineVisibility
        {
            get
            {
                return (Visibility)this.GetValue(HorizontalLineVisibilityProperty);
            }

            set
            {
                this.SetValue(HorizontalLineVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets VerticalLineVisibility.
        /// </summary>
        public Visibility VerticalLineVisibility
        {
            get
            {
                return (Visibility)this.GetValue(VerticalLineVisibilityProperty);
            }

            set
            {
                this.SetValue(VerticalLineVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineStroke.
        /// </summary>
        public Brush LineStroke
        {
            get
            {
                return (Brush)this.GetValue(LineStrokeProperty);
            }

            set
            {
                this.SetValue(LineStrokeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineExtents.
        /// </summary>
        public OxyRect LineExtents
        {
            get
            {
                return (OxyRect)this.GetValue(LineExtentsProperty);
            }

            set
            {
                this.SetValue(LineExtentsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineDashArray.
        /// </summary>
        public DoubleCollection LineDashArray
        {
            get
            {
                return (DoubleCollection)this.GetValue(LineDashArrayProperty);
            }

            set
            {
                this.SetValue(LineDashArrayProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show a 'pointer' on the border.
        /// </summary>
        public bool ShowPointer
        {
            get
            {
                return (bool)this.GetValue(ShowPointerProperty);
            }

            set
            {
                this.SetValue(ShowPointerProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the corner radius (only used when ShowPoint=<c>false</c>).
        /// </summary>
        public double CornerRadius
        {
            get
            {
                return (double)this.GetValue(CornerRadiusProperty);
            }

            set
            {
                this.SetValue(CornerRadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the distance of the content container from the trackers Position.
        /// </summary>
        public double Distance
        {
            get
            {
                return (double)this.GetValue(DistanceProperty);
            }

            set
            {
                this.SetValue(DistanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tracker can center its content box horizontally.
        /// </summary>
        public bool CanCenterHorizontally
        {
            get
            {
                return (bool)this.GetValue(CanCenterHorizontallyProperty);
            }

            set
            {
                this.SetValue(CanCenterHorizontallyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tracker can center its content box vertically.
        /// </summary>
        public bool CanCenterVertically
        {
            get
            {
                return (bool)this.GetValue(CanCenterVerticallyProperty);
            }

            set
            {
                this.SetValue(CanCenterVerticallyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Position of the tracker.
        /// </summary>
        public ScreenPoint Position
        {
            get
            {
                return (ScreenPoint)this.GetValue(PositionProperty);
            }

            set
            {
                this.SetValue(PositionProperty, value);
            }
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
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
                throw new InvalidOperationException(string.Format("The TrackerControl template must contain a content container with name +'{0}'", PartContentcontainer));
            }

            if (this.path == null)
            {
                throw new InvalidOperationException(string.Format("The TrackerControl template must contain a Path with name +'{0}'", PartPath));
            }

            if (this.content == null)
            {
                throw new InvalidOperationException(string.Format("The TrackerControl template must contain a ContentPresenter with name +'{0}'", PartContent));
            }

            this.UpdatePositionAndBorder();
        }

        /// <summary>
        /// Called when the position is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void PositionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((TrackerControl)sender).OnPositionChanged(e);
        }

        /// <summary>
        /// Called when the position is changed.
        /// </summary>
        /// <param name="e">The dependency property changed event args.</param>
        // ReSharper disable once UnusedParameter.Local
        private void OnPositionChanged(DependencyPropertyChangedEventArgs e)
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

            var ha = System.Windows.HorizontalAlignment.Center;
            if (this.CanCenterHorizontally)
            {
                if (this.Position.X - (contentWidth / 2) < MarginLimit)
                {
                    ha = System.Windows.HorizontalAlignment.Left;
                }

                if (this.Position.X + (contentWidth / 2) > canvasWidth - MarginLimit)
                {
                    ha = System.Windows.HorizontalAlignment.Right;
                }
            }
            else
            {
                ha = this.Position.X < canvasWidth / 2 ? System.Windows.HorizontalAlignment.Left : System.Windows.HorizontalAlignment.Right;
            }

            var va = System.Windows.VerticalAlignment.Center;
            if (this.CanCenterVertically)
            {
                if (this.Position.Y - (contentHeight / 2) < MarginLimit)
                {
                    va = System.Windows.VerticalAlignment.Top;
                }

                if (ha == System.Windows.HorizontalAlignment.Center)
                {
                    va = System.Windows.VerticalAlignment.Bottom;
                    if (this.Position.Y - contentHeight < MarginLimit)
                    {
                        va = System.Windows.VerticalAlignment.Top;
                    }
                }

                if (va == System.Windows.VerticalAlignment.Center && this.Position.Y + (contentHeight / 2) > canvasHeight - MarginLimit)
                {
                    va = System.Windows.VerticalAlignment.Bottom;
                }

                if (va == System.Windows.VerticalAlignment.Top && this.Position.Y + contentHeight > canvasHeight - MarginLimit)
                {
                    va = System.Windows.VerticalAlignment.Bottom;
                }
            }
            else
            {
                va = this.Position.Y < canvasHeight / 2 ? System.Windows.VerticalAlignment.Top : System.Windows.VerticalAlignment.Bottom;
            }

            double dx = ha == System.Windows.HorizontalAlignment.Center ? -0.5 : ha == System.Windows.HorizontalAlignment.Left ? 0 : -1;
            double dy = va == System.Windows.VerticalAlignment.Center ? -0.5 : va == System.Windows.VerticalAlignment.Top ? 0 : -1;

            Thickness margin;
            this.path.Data = this.ShowPointer
                                 ? this.CreatePointerBorderGeometry(ha, va, contentWidth, contentHeight, out margin)
                                 : this.CreateBorderGeometry(ha, va, contentWidth, contentHeight, out margin);

            this.content.Margin = margin;

            this.contentContainer.Measure(new Size(canvasWidth, canvasHeight));
            Size contentSize = this.contentContainer.DesiredSize;

            this.contentContainer.RenderTransform = new TranslateTransform
            {
                X = dx * contentSize.Width,
                Y = dy * contentSize.Height
            };
            
            ScreenPoint pos = this.Position;
            
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
            System.Windows.HorizontalAlignment ha, System.Windows.VerticalAlignment va, double width, double height, out Thickness margin)
        {
            double m = this.Distance;
            var rect = new Rect(
                ha == System.Windows.HorizontalAlignment.Left ? m : 0, va == System.Windows.VerticalAlignment.Top ? m : 0, width, height);
            margin = new Thickness
            {
                Left = ha == System.Windows.HorizontalAlignment.Left ? m : 0,
                Top = va == System.Windows.VerticalAlignment.Top ? m : 0,
                Right = ha == System.Windows.HorizontalAlignment.Right ? m : 0,
                Bottom = va == System.Windows.VerticalAlignment.Bottom ? m : 0
            };
            return new RectangleGeometry { Rect = rect /*, RadiusX = this.CornerRadius, RadiusY = this.CornerRadius*/ };
        }

        /// <summary>
        /// Creates a border geometry with a 'pointer'.
        /// </summary>
        /// <param name="ha">The horizontal alignment.</param>
        /// <param name="va">The vertical alignment.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="margin">The margin.</param>
        /// <returns>The border geometry.</returns>
        private Geometry CreatePointerBorderGeometry(
            System.Windows.HorizontalAlignment ha, System.Windows.VerticalAlignment va, double width, double height, out Thickness margin)
        {
            Point[] points = null;
            double m = this.Distance;
            margin = new Thickness();

            if (ha == System.Windows.HorizontalAlignment.Center && va == System.Windows.VerticalAlignment.Bottom)
            {
                double x0 = 0;
                double x1 = width;
                double x2 = (x0 + x1) / 2;
                double y0 = 0;
                double y1 = height;
                margin = new Thickness { Bottom = m };
                points = new[]
                    {
                        new Point(x0, y0), new Point(x1, y0), new Point(x1, y1), new Point(x2 + (m / 2), y1),
                        new Point(x2, y1 + m), new Point(x2 - (m / 2), y1), new Point(x0, y1)
                    };
            }

            if (ha == System.Windows.HorizontalAlignment.Center && va == System.Windows.VerticalAlignment.Top)
            {
                double x0 = 0;
                double x1 = width;
                double x2 = (x0 + x1) / 2;
                double y0 = m;
                double y1 = m + height;
                margin = new Thickness { Top = m };
                points = new[]
                    {
                        new Point(x0, y0), new Point(x2 - (m / 2), y0), new Point(x2, 0), new Point(x2 + (m / 2), y0),
                        new Point(x1, y0), new Point(x1, y1), new Point(x0, y1)
                    };
            }

            if (ha == System.Windows.HorizontalAlignment.Left && va == System.Windows.VerticalAlignment.Center)
            {
                double x0 = m;
                double x1 = m + width;
                double y0 = 0;
                double y1 = height;
                double y2 = (y0 + y1) / 2;
                margin = new Thickness { Left = m };
                points = new[]
                    {
                        new Point(0, y2), new Point(x0, y2 - (m / 2)), new Point(x0, y0), new Point(x1, y0),
                        new Point(x1, y1), new Point(x0, y1), new Point(x0, y2 + (m / 2))
                    };
            }

            if (ha == System.Windows.HorizontalAlignment.Right && va == System.Windows.VerticalAlignment.Center)
            {
                double x0 = 0;
                double x1 = width;
                double y0 = 0;
                double y1 = height;
                double y2 = (y0 + y1) / 2;
                margin = new Thickness { Right = m };
                points = new[]
                    {
                        new Point(x1 + m, y2), new Point(x1, y2 + (m / 2)), new Point(x1, y1), new Point(x0, y1),
                        new Point(x0, y0), new Point(x1, y0), new Point(x1, y2 - (m / 2))
                    };
            }

            if (ha == System.Windows.HorizontalAlignment.Left && va == System.Windows.VerticalAlignment.Top)
            {
                m *= 0.67;
                double x0 = m;
                double x1 = m + width;
                double y0 = m;
                double y1 = m + height;
                margin = new Thickness { Left = m, Top = m };
                points = new[]
                    {
                        new Point(0, 0), new Point(m * 2, y0), new Point(x1, y0), new Point(x1, y1), new Point(x0, y1),
                        new Point(x0, m * 2)
                    };
            }

            if (ha == System.Windows.HorizontalAlignment.Right && va == System.Windows.VerticalAlignment.Top)
            {
                m *= 0.67;
                double x0 = 0;
                double x1 = width;
                double y0 = m;
                double y1 = m + height;
                margin = new Thickness { Top = m, Right = m };
                points = new[]
                    {
                        new Point(x1 + m, 0), new Point(x1, y0 + m), new Point(x1, y1), new Point(x0, y1),
                        new Point(x0, y0), new Point(x1 - m, y0)
                    };
            }

            if (ha == System.Windows.HorizontalAlignment.Left && va == System.Windows.VerticalAlignment.Bottom)
            {
                m *= 0.67;
                double x0 = m;
                double x1 = m + width;
                double y0 = 0;
                double y1 = height;
                margin = new Thickness { Left = m, Bottom = m };
                points = new[]
                    {
                        new Point(0, y1 + m), new Point(x0, y1 - m), new Point(x0, y0), new Point(x1, y0),
                        new Point(x1, y1), new Point(x0 + m, y1)
                    };
            }

            if (ha == System.Windows.HorizontalAlignment.Right && va == System.Windows.VerticalAlignment.Bottom)
            {
                m *= 0.67;
                double x0 = 0;
                double x1 = width;
                double y0 = 0;
                double y1 = height;
                margin = new Thickness { Right = m, Bottom = m };
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

            var pointCollection = new PointCollection();
            foreach (var point in points)
            {
                pointCollection.Add(point);
            }

            var segments = new PathSegmentCollection { new PolyLineSegment { Points = pointCollection } };
            var pf = new PathFigure { StartPoint = points[0], Segments = segments, IsClosed = true };
            return new PathGeometry { Figures = new PathFigureCollection { pf } };
        }
    }
}
