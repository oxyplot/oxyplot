// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextualAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for annotations that contains text.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Annotations
{
    using System;

    /// <summary>
    /// Provides an abstract base class for annotations that contains text.
    /// </summary>
    public abstract class TextualAnnotation : Annotation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextualAnnotation"/> class.
        /// </summary>
        protected TextualAnnotation()
        {
            this.TextHorizontalAlignment = HorizontalAlignment.Center;
            this.TextVerticalAlignment = VerticalAlignment.Middle;
            this.TextPosition = DataPoint.Undefined;
            this.TextRotation = 0;
        }

        /// <summary>
        /// Gets or sets the annotation text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the position of the text.
        /// </summary>
        /// <remarks>If the value is <c>DataPoint.Undefined</c>, the default position of the text will be used.</remarks>
        public DataPoint TextPosition { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment of the text.
        /// </summary>
        public HorizontalAlignment TextHorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment of the text.
        /// </summary>
        public VerticalAlignment TextVerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the text.
        /// </summary>
        /// <value>The text rotation in degrees.</value>
        public double TextRotation { get; set; }

        /// <summary>
        /// Gets the actual position of the text.
        /// </summary>
        /// <param name="defaultPosition">A function that returns the default position. This is used if <see cref="TextPosition" /> is undefined.</param>
        /// <returns>The actual position of the text, in screen space.</returns>
        protected ScreenPoint GetActualTextPosition(Func<ScreenPoint> defaultPosition)
        {
            return this.TextPosition.IsDefined() ? this.Transform(this.TextPosition) : defaultPosition();
        }

        /// <summary>
        /// Gets the actual text alignment.
        /// </summary>
        /// <param name="ha">The horizontal alignment.</param>
        /// <param name="va">The vertical alignment.</param>
        protected void GetActualTextAlignment(out HorizontalAlignment ha, out VerticalAlignment va)
        {
            ha = this.TextHorizontalAlignment;
            va = this.TextVerticalAlignment;
            //this.Orientate(ref ha, ref va);
        }
    }
}
