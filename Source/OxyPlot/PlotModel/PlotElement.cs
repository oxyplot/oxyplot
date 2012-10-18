// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotElement.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Abstract base class for all plottable elements (Axes, Annotations, Series).
    /// </summary>
    [Serializable]
    public abstract class PlotElement
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="PlotElement"/> class.
        /// </summary>
        protected PlotElement()
        {
            this.Font = null;
            this.FontSize = double.NaN;
            this.FontWeight = FontWeights.Normal;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the font.
        /// </summary>
        /// <value> The font. </value>
        /// <remarks>
        ///   If the value is null, the parent PlotModel's DefaultFont will be used.
        /// </remarks>
        public string Font { get; set; }

        /// <summary>
        ///   Gets or sets the size of the font.
        /// </summary>
        /// <value> The size of the font. </value>
        /// <remarks>
        ///   If the value is NaN, the parent PlotModel's DefaultFontSize will be used.
        /// </remarks>
        public double FontSize { get; set; }

        /// <summary>
        ///   Gets or sets the font weight.
        /// </summary>
        /// <value> The font weight. </value>
        public double FontWeight { get; set; }

        /// <summary>
        ///   Gets the parent plot model.
        /// </summary>
        public PlotModel PlotModel { get; internal set; }

        /// <summary>
        ///   Gets or sets an arbitrary object value that can be used to store custom information about this plot element.
        /// </summary>
        /// <value> The intended value. This property has no default value. </value>
        /// <remarks>
        ///   This property is analogous to Tag properties in other Microsoft programming models. Tag is intended to provide a pre-existing property location where you can store some basic custom information about any PlotElement without requiring you to subclass an element.
        /// </remarks>
        public object Tag { get; set; }

        /// <summary>
        ///   Gets or sets the color of the text.
        /// </summary>
        /// <value> The color of the text. </value>
        /// <remarks>
        ///   If the value is null, the TextColor of the parent PlotModel will be used.
        /// </remarks>
        public OxyColor TextColor { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the actual font.
        /// </summary>
        protected internal string ActualFont
        {
            get
            {
                return this.Font ?? this.PlotModel.DefaultFont;
            }
        }

        /// <summary>
        ///   Gets the actual size of the font.
        /// </summary>
        /// <value> The actual size of the font. </value>
        protected internal double ActualFontSize
        {
            get
            {
                return !double.IsNaN(this.FontSize) ? this.FontSize : this.PlotModel.DefaultFontSize;
            }
        }

        /// <summary>
        ///   Gets the actual font weight.
        /// </summary>
        protected internal double ActualFontWeight
        {
            get
            {
                return this.FontWeight;
            }
        }

        /// <summary>
        ///   Gets the actual color of the text.
        /// </summary>
        /// <value> The actual color of the text. </value>
        protected internal OxyColor ActualTextColor
        {
            get
            {
                return this.TextColor ?? this.PlotModel.TextColor;
            }
        }

        /// <summary>
        /// Traces the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void Trace(string message)
        {
#if !SILVERLIGHT && !MONO && !PCL
            System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", this.GetType().Name, message));
#endif
        }
        #endregion
    }
}