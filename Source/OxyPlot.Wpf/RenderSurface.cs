// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderSurface.cs" company="OxyPlot">
//   Copyright (c) 2026 OxyPlot contributors
// </copyright>
// <summary>
//   A subclass of <see cref="System.Windows.Controls.Panel" /> that is used to render the plot.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Represents a custom rendering surface that provides methods for drawing shapes, images, and text.
    /// </summary>
    /// <remarks>The <see cref="RenderSurface"/> class extends the <see cref="Panel"/> class and provides a
    /// drawing context for rendering graphical elements. It replaces the use of Canvas when rendering OxyPlot for WPF
    /// Instead of creating Path and TextBlock element, it renders the graphical elements into a Drawing group,
    /// bypassing the need for measure and arrange. This also solves a problem with the default rendering in
    /// that it creates new WPF elements in OnRender.</remarks>
    public class RenderSurface : Panel
    {
        private static readonly Pen HitTestPen = CreateHitTestPen();

        private readonly DrawingGroup objectsToRender = new();
        private DrawingContext currentDc;
        private readonly List<Tuple<Geometry, string>> tooltipRegions = new();
        // Keeps track of whether the tooltip has been opened while the mouse is within this control.
        private bool toolTipIsOpen;

        private static Pen CreateHitTestPen()
        {
            var pen = new Pen(Brushes.Black, 3.0);
            pen.Freeze();
            return pen;
        }

        internal void RegisterTooltipRegion(Geometry geometry, string tooltip)
        {
            this.tooltipRegions.Add(new Tuple<Geometry, string>(geometry, tooltip));
        }

        private string GetToolTip(Point pos)
        {
            // iterate in reverse so the last-drawn (topmost) element wins
            for (int i = this.tooltipRegions.Count - 1; i >= 0; i--)
            {
                var toolTip = this.tooltipRegions[i];
                if (toolTip.Item1.FillContains(pos) || toolTip.Item1.StrokeContains(HitTestPen, pos))
                {
                    return toolTip.Item2;
                }
            }
            return null;
        }

        /// <inheritdoc/>
        protected override void OnToolTipOpening(ToolTipEventArgs e)
        {
            this.toolTipIsOpen = true;

            if (this.ToolTip is ToolTip tt)
            {
                var toolTipText = tt.Content as string;
                if (string.IsNullOrEmpty(toolTipText))
                {
                    e.Handled = true;
                    return;
                }
            }

            base.OnToolTipOpening(e);
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.ToolTip is ToolTip tt)
            {
                var toolTipText = this.GetToolTip(e.GetPosition(this));
                var previousToolTipText = tt.Content as string;
                tt.Content = toolTipText;
                if (string.IsNullOrEmpty(toolTipText))
                {
                    tt.IsOpen = false;
                }
                else if (this.toolTipIsOpen && previousToolTipText != toolTipText)
                {
                    // tooltip text has changed, reopen since the tooltip is no longer in the right place
                    tt.IsOpen = false;
                    tt.IsOpen = true;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            this.toolTipIsOpen = false;
            base.OnMouseLeave(e);
        }

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            dc.DrawDrawing(this.objectsToRender);
        }

        /// <summary>
        /// Draws an ellipse with the specified fill, outline, center, radii, and edge rendering mode.
        /// </summary>
        /// <remarks>When <paramref name="edgeMode"/> is set to <see cref="EdgeMode.Aliased"/>, the method
        /// applies snapping guidelines to ensure aliased rendering. If both <paramref name="fillBrush"/> and <paramref
        /// name="pen"/> are <see langword="null"/>, the method does not render anything.</remarks>
        /// <param name="fillBrush">The <see cref="Brush"/> used to fill the interior of the ellipse. Can be <see langword="null"/> to draw only
        /// the outline.</param>
        /// <param name="pen">The <see cref="Pen"/> used to draw the outline of the ellipse. Can be <see langword="null"/> to draw only
        /// the fill.</param>
        /// <param name="center">The <see cref="Point"/> representing the center of the ellipse.</param>
        /// <param name="radiusX">The horizontal radius of the ellipse. Must be a non-negative value.</param>
        /// <param name="radiusY">The vertical radius of the ellipse. Must be a non-negative value.</param>
        /// <param name="edgeMode">The <see cref="EdgeMode"/> that determines how the edges of the ellipse are rendered. Use <see
        /// cref="EdgeMode.Aliased"/> for aliased edges or <see cref="EdgeMode.Unspecified"/> for default rendering.</param>
        internal void DrawEllipse(Brush fillBrush, Pen pen, Point center, double radiusX, double radiusY, EdgeMode edgeMode)
        {
            if (edgeMode == EdgeMode.Aliased)
            {
                var wrappedGroup = new DrawingGroup();
                wrappedGroup.SetValue(RenderOptions.EdgeModeProperty, edgeMode);
                using (var dc = wrappedGroup.Open())
                {
                    dc.DrawEllipse(fillBrush, pen, center, radiusX, radiusY);
                }
                this.currentDc?.DrawDrawing(wrappedGroup);
            }
            else
            {
                this.currentDc?.DrawEllipse(fillBrush, pen, center, radiusX, radiusY);
            }
        }

        /// <summary>
        /// Draws the specified geometry using the provided brush and pen, with an optional edge mode for rendering.
        /// </summary>
        /// <remarks>When <paramref name="edgeMode"/> is set to <see cref="EdgeMode.Aliased"/>, the
        /// geometry is wrapped in a <see cref="DrawingGroup"/> with snapping guidelines applied to ensure aliased
        /// rendering. For other edge modes, the geometry is drawn directly.</remarks>
        /// <param name="fillBrush">The <see cref="Brush"/> used to fill the interior of the geometry. Can be <see langword="null"/> if no fill
        /// is desired.</param>
        /// <param name="pen">The <see cref="Pen"/> used to outline the geometry. Can be <see langword="null"/> if no outline is desired.</param>
        /// <param name="streamGeometry">The <see cref="StreamGeometry"/> that defines the shape to be drawn. Cannot be <see langword="null"/>.</param>
        /// <param name="edgeMode">Specifies the <see cref="EdgeMode"/> to use for rendering the geometry. If set to <see
        /// cref="EdgeMode.Aliased"/>, the geometry is drawn with aliased edges.</param>
        internal void DrawGeometry(Brush fillBrush, Pen pen, StreamGeometry streamGeometry, EdgeMode edgeMode)
        {
            if (edgeMode == EdgeMode.Aliased)
            {
                var wrappedGroup = new DrawingGroup();
                wrappedGroup.SetValue(RenderOptions.EdgeModeProperty, edgeMode);
                using (var dc = wrappedGroup.Open())
                {
                    dc.DrawGeometry(fillBrush, pen, streamGeometry);
                }
                this.currentDc?.DrawDrawing(wrappedGroup);
            }
            else
            {
                this.currentDc?.DrawGeometry(fillBrush, pen, streamGeometry);
            }
        }

        /// <summary>
        /// Draws a rectangle with the specified brush, pen, and dimensions, optionally applying edge mode settings.
        /// </summary>
        /// <remarks>When <paramref name="edgeMode"/> is set to <see cref="EdgeMode.Aliased"/>, the
        /// rectangle is drawn with aliased edges, and snapping guidelines are applied to ensure alignment. For other
        /// edge modes, the rectangle is drawn using the default rendering behavior.</remarks>
        /// <param name="fillBrush">The <see cref="Brush"/> used to fill the interior of the rectangle. Can be <see langword="null"/> to draw
        /// only the outline.</param>
        /// <param name="pen">The <see cref="Pen"/> used to draw the outline of the rectangle. Can be <see langword="null"/> to draw only
        /// the fill.</param>
        /// <param name="rect">The <see cref="Rect"/> structure that defines the dimensions and position of the rectangle.</param>
        /// <param name="edgeMode">The <see cref="EdgeMode"/> that determines how the edges of the rectangle are rendered. Use <see
        /// cref="EdgeMode.Aliased"/> for aliased edges or <see cref="EdgeMode.Unspecified"/> for default rendering.</param>
        internal void DrawRectangle(Brush fillBrush, Pen pen, Rect rect, EdgeMode edgeMode)
        {
            if (edgeMode == EdgeMode.Aliased)
            {
                var wrappedGroup = new DrawingGroup();
                wrappedGroup.SetValue(RenderOptions.EdgeModeProperty, edgeMode);
                using (var dc = wrappedGroup.Open())
                {
                    dc.DrawRectangle(fillBrush, pen, rect);
                }
                this.currentDc?.DrawDrawing(wrappedGroup);
            }
            else
            {
                this.currentDc?.DrawRectangle(fillBrush, pen, rect);
            }
        }

        /// <summary>
        /// Draws the specified image onto the rendering surface at the specified location, size, and opacity.
        /// </summary>
        /// <remarks>If the specified <paramref name="scalingMode"/> differs from the current scaling mode
        /// of the rendering context, the image is drawn within a temporary <see cref="DrawingGroup"/> to apply the
        /// desired scaling mode.</remarks>
        /// <param name="sourceImage">The <see cref="ImageSource"/> to be drawn.</param>
        /// <param name="destX">The x-coordinate of the top-left corner of the destination rectangle.</param>
        /// <param name="destY">The y-coordinate of the top-left corner of the destination rectangle.</param>
        /// <param name="destWidth">The width of the destination rectangle.</param>
        /// <param name="destHeight">The height of the destination rectangle.</param>
        /// <param name="opacity">The opacity of the image, where 1.0 is fully opaque and 0.0 is fully transparent.</param>
        /// <param name="scalingMode">The <see cref="BitmapScalingMode"/> to use when scaling the image.</param>
        internal void DrawImage(ImageSource sourceImage, double destX, double destY, double destWidth, double destHeight, double opacity, BitmapScalingMode scalingMode)
        {
            if (this.currentDc == null)
                return;

            try
            {
                this.currentDc.PushOpacity(opacity);
                if (RenderOptions.GetBitmapScalingMode(this.objectsToRender) != scalingMode)
                {
                    var wrappedGroup = new DrawingGroup();
                    RenderOptions.SetBitmapScalingMode(wrappedGroup, scalingMode);
                    using (var dc = wrappedGroup.Open())
                    {
                        dc.DrawImage(sourceImage, new Rect(new Point(destX, destY), new Size(destWidth, destHeight)));
                    }

                    this.currentDc.DrawDrawing(wrappedGroup);
                }
                else
                {
                    this.currentDc.DrawImage(
                        sourceImage,
                        new Rect(new Point(destX, destY), new Size(destWidth, destHeight)));
                }
            }
            finally
            {
                this.currentDc.Pop();
            }
        }

        /// <summary>
        /// Draws the specified text at the given location with the specified formatting options.
        /// </summary>
        /// <remarks>This method uses the current drawing context to render the text. If the drawing
        /// context is null, the method does nothing. The text is formatted using the specified font properties and
        /// rendered with the specified brush.</remarks>
        /// <param name="location">The top-left point where the text will be drawn.</param>
        /// <param name="text">The text to be drawn. If the text is null or empty, no drawing occurs.</param>
        /// <param name="brush">The <see cref="Brush"/> used to paint the text. Cannot be null.</param>
        /// <param name="fontFamily">The <see cref="FontFamily"/> to use for the text. If null, a default font family is used.</param>
        /// <param name="fontSize">The size of the font in device-independent units (1/96th inch per unit). Must be greater than 0; otherwise,
        /// a default size is used.</param>
        /// <param name="fontWeight">The weight of the font, such as <see cref="FontWeights.Bold"/>.</param>
        /// <param name="textFormattingMode">Specifies the text rendering mode, such as <see cref="TextFormattingMode.Display"/> or <see
        /// cref="TextFormattingMode.Ideal"/>.</param>
        /// <param name="transform">The <see cref="Transform"/> to apply to the text before drawing. Cannot be null.</param>
        internal void DrawText(Point location, string text, Brush brush, FontFamily fontFamily, double fontSize,
          FontWeight fontWeight, TextFormattingMode textFormattingMode, Transform transform)
        {
            if (this.currentDc == null || brush == null || string.IsNullOrEmpty(text))
                return; // no text or invisible text, do not draw.

            if (fontSize <= 0)
            {
                fontSize = this.GetValue(TextBlock.FontSizeProperty) is double fs ? fs : 12.0;
            }

            try
            {
                this.currentDc.PushTransform(transform);

                Typeface typeface = new Typeface(
                    (fontFamily ?? this.GetValue(TextBlock.FontFamilyProperty) as FontFamily)!,
                    FontStyles.Normal,
                    fontWeight,
                    FontStretches.Normal);

                var dpiScale = VisualTreeHelper.GetDpi(this).PixelsPerDip;

                FormattedText formattedText = new FormattedText(
                    text,
                    CultureInfo.CurrentCulture,
                    this.FlowDirection,
                    typeface,
                    fontSize,
                    brush,
                    null,
                    textFormattingMode,
                    dpiScale);

                this.currentDc.DrawText(formattedText, location);
            }
            finally
            {
                this.currentDc.Pop(); // transform
            }
        }

        /// <summary>
        /// Prepares the rendering process by enabling ClearType rendering and opening the drawing context.
        /// </summary>
        /// <remarks>This method sets the ClearType hint for the rendering objects to improve text clarity
        /// and initializes the drawing context for subsequent rendering operations. Ensure that the rendering process
        /// is properly finalized after calling this method.</remarks>
        internal void BeginRender()
        {
            this.tooltipRegions.Clear();
            RenderOptions.SetClearTypeHint(this.objectsToRender, ClearTypeHint.Enabled);
            this.currentDc = this.objectsToRender.Open();
        }

        /// <summary>
        /// Ends the current rendering operation and releases associated resources.
        /// </summary>
        /// <remarks>This method finalizes the rendering process by closing the current drawing context,
        /// if one is active.  After calling this method, the rendering context is reset and no further drawing
        /// operations can be performed  until a new rendering context is initialized.</remarks>
        internal void EndRender()
        {
            if (this.ToolTip is null && this.tooltipRegions.Count > 0)
            {
                this.ToolTip = new ToolTip() { Content = string.Empty };
            }
            this.currentDc?.Close();
            this.currentDc = null;
        }

        /// <summary>
        /// Sets the clipping region for the current drawing context.
        /// </summary>
        /// <remarks>The specified <paramref name="clipRect"/> defines the boundaries beyond which drawing
        /// operations  will be clipped. This method requires a valid drawing context to be active.</remarks>
        /// <param name="clipRect">The rectangular region to apply as the clipping area.</param>
        internal void SetClip(Rect clipRect)
        {
            this.currentDc?.PushClip(new RectangleGeometry(clipRect));
        }

        /// <summary>
        /// Resets the current clipping region to its previous state.
        /// </summary>
        /// <remarks>This method removes the most recently applied clipping region, restoring the clipping
        /// state  to what it was before the last clipping operation. If no clipping region is currently applied,  this
        /// method has no effect.</remarks>
        internal void ResetClip()
        {
            this.currentDc?.Pop();
        }

    }
}

