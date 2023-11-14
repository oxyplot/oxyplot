// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorAxisRenderer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes.Rendering
{
    using System;

    /// <summary>
    /// Provides functionality to render color axes.
    /// </summary>
    public abstract class ColorAxisRenderer<T> : HorizontalAndVerticalAxisRenderer<T> where T: Axis, IColorAxis
    {
        /// <summary>
        /// Position of the left edge of the color block.
        /// </summary>
        protected double left;

        /// <summary>
        /// Position of the top edge of the color block.
        /// </summary>
        protected double top;

        /// <summary>
        /// 'Size' of the color block; this is the width for vertical axes, or the height for horizontal axes.
        /// </summary>
        protected double size;

        /// <summary>
        /// Screen position of the minimum value of the axis.
        /// </summary>
        protected double minScreenPosition;

        /// <summary>
        /// Screen position of the maximum value of the axis.
        /// </summary>
        protected double maxScreenPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorAxisRenderer{T}" /> class.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="plot">The plot.</param>
        public ColorAxisRenderer(IRenderContext rc, PlotModel plot) : base(rc, plot)
        {
        }

        /// <summary>
        /// Initializes fields containing information about position and size of the color axis on screen.
        /// </summary>
        /// <param name="axis">The color axis.</param>
        protected virtual void InitPosition(T axis)
        {
            this.size = axis.MajorTickSize - 2;
            var distance = axis.AxisDistance;
            this.minScreenPosition = axis.Transform(axis.ClipMinimum);
            this.maxScreenPosition = axis.Transform(axis.ClipMaximum);

            switch (axis.Position)
            {
                case AxisPosition.Left:
                    this.left = axis.PlotModel.PlotArea.Left - axis.PositionTierMinShift - this.size - distance;
                    this.top = axis.PlotModel.PlotArea.Top;
                    break;
                case AxisPosition.Right:
                    this.left = axis.PlotModel.PlotArea.Right + axis.PositionTierMinShift + distance;
                    this.top = axis.PlotModel.PlotArea.Top;
                    break;
                case AxisPosition.Top:
                    this.left = axis.PlotModel.PlotArea.Left;
                    this.top = axis.PlotModel.PlotArea.Top - axis.PositionTierMinShift - this.size - distance;
                    break;
                case AxisPosition.Bottom:
                    this.left = axis.PlotModel.PlotArea.Left;
                    this.top = axis.PlotModel.PlotArea.Bottom + axis.PositionTierMinShift + distance;
                    break;
            }
        }

        /// <inheritdoc />
        public override void Render(T axis, int pass)
        {
            if (axis.Position is not (AxisPosition.Left or AxisPosition.Right or AxisPosition.Top or AxisPosition.Bottom))
            {
                return;
            }

            if (pass == 0)
            {
                this.InitPosition(axis);
                this.RenderColorBlock(axis);
            }

            base.Render(axis, pass);
        }

        /// <summary>
        /// Renders the color block.
        /// </summary>
        /// <param name="axis">The color axis.</param>
        protected abstract void RenderColorBlock(T axis);

        /// <summary>
        /// Draws a single colored rectangle at the provided screen position.
        /// </summary>
        /// <param name="axis">The color axis.</param>
        /// <param name="ylow">The screen position of the lower end of the rectangle.</param>
        /// <param name="yhigh">The screen position of the higher end of the rectangle.</param>
        /// <param name="color">The color.</param>
        protected void DrawColorRect(T axis, double ylow, double yhigh, OxyColor color) 
        {
            var ymin = Math.Min(ylow, yhigh);
            var ymax = Math.Max(ylow, yhigh) + 0.5;
            var rect = axis.IsHorizontal()
                    ? new OxyRect(ymin, this.top, ymax - ymin, this.size)
                    : new OxyRect(this.left, ymin, this.size, ymax - ymin);

            this.RenderContext.DrawRectangle(
                rect,
                color,
                OxyColors.Undefined,
                0,
                axis.EdgeRenderingMode);
        }
    }
}
