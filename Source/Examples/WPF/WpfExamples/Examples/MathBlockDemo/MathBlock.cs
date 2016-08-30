// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathBlock.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a control for displaying simple math.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Provides a control for displaying simple math.
    /// </summary>
    [TemplatePart(Name = PartCanvas, Type = typeof(Canvas))]
    public class MathBlock : ContentControl
    {
        /// <summary>
        /// The canvas template part.
        /// </summary>
        private const string PartCanvas = "PART_Canvas";

        /// <summary>
        /// The canvas
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// The render context
        /// </summary>
        private IRenderContext renderContext;

        /// <summary>
        /// Initializes static members of the <see cref="MathBlock" /> class.
        /// </summary>
        static MathBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(MathBlock),
                new FrameworkPropertyMetadata(typeof(MathBlock)));
            ContentProperty.OverrideMetadata(
                typeof(MathBlock),
                new FrameworkPropertyMetadata(typeof(MathBlock), (s, e) => ((MathBlock)s).ContentChanged()));
        }

        /// <summary>
        /// Handles changes in the Content property.
        /// </summary>
        private void ContentChanged()
        {
            this.UpdateContent();
        }

        /// <summary>
        /// Called to measure a control.
        /// </summary>
        /// <param name="constraint">The maximum size that the method can return.</param>
        /// <returns>The size of the control, up to the maximum specified by <paramref name="constraint" />.</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            if (this.Content == null)
            {
                return base.MeasureOverride(constraint);
            }

            var text = this.Content.ToString();

            double fontWeight = this.FontWeight.ToOpenTypeWeight();
            string fontFamily = null;
            if (this.FontFamily != null)
            {
                fontFamily = this.FontFamily.Source;
            }

            var size = this.renderContext.MeasureMathText(text, fontFamily, this.FontSize, fontWeight);
            return new Size(size.Width + this.Padding.Left + this.Padding.Right, size.Height + this.Padding.Top + this.Padding.Bottom);
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.canvas = this.GetTemplateChild(PartCanvas) as Canvas;
            this.renderContext = new CanvasRenderContext(this.canvas);
            this.SizeChanged += this.HandleSizeChanged;
        }

        /// <summary>
        /// Handles changes in control size.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs" /> instance containing the event data.</param>
        private void HandleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateContent();
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        private void UpdateContent()
        {
            if (this.canvas == null)
            {
                return;
            }

            this.canvas.Children.Clear();
            if (this.Content == null)
            {
                return;
            }

            var text = this.Content.ToString();
            var horizontalAlignment = this.HorizontalContentAlignment.ToHorizontalAlignment();
            var verticalAlignment = this.VerticalContentAlignment.ToVerticalAlignment();
            double fontWeight = this.FontWeight.ToOpenTypeWeight();
            double x = this.Padding.Left;
            switch (horizontalAlignment)
            {
                case OxyPlot.HorizontalAlignment.Right:
                    x = this.ActualWidth - this.Padding.Right;
                    break;
                case OxyPlot.HorizontalAlignment.Center:
                    x = this.Padding.Left + ((this.ActualWidth - this.Padding.Left - this.Padding.Right) * 0.5);
                    break;
            }

            double y = this.Padding.Top;
            switch (verticalAlignment)
            {
                case OxyPlot.VerticalAlignment.Bottom:
                    y = this.ActualHeight - this.Padding.Bottom;
                    break;
                case OxyPlot.VerticalAlignment.Middle:
                    y = this.Padding.Top + ((this.ActualWidth - this.Padding.Bottom - this.Padding.Top) * 0.5);
                    break;
            }

            var p = new ScreenPoint(x, y);
            string fontFamily = null;
            if (this.FontFamily != null)
            {
                fontFamily = this.FontFamily.Source;
            }

            this.renderContext.DrawMathText(
                p,
                text,
                this.Foreground.ToOxyColor(),
                fontFamily,
                this.FontSize,
                fontWeight,
                0,
                horizontalAlignment,
                verticalAlignment);
        }
    }
}