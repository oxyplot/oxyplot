// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotElement.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for elements of a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Provides an abstract base class for elements of a <see cref="PlotModel" />.
    /// </summary>
    public abstract class PlotElement : Element, IPlotElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotElement" /> class.
        /// </summary>
        protected PlotElement()
        {
            this.Font = null;
            this.FontSize = double.NaN;
            this.FontWeight = FontWeights.Normal;
            this.TextColor = OxyColors.Automatic;
            this.EdgeRenderingMode = EdgeRenderingMode.Automatic;
        }

        /// <summary>
        /// Gets or sets the font. The default is <c>null</c> (use <see cref="OxyPlot.PlotModel.DefaultFont" />.
        /// </summary>
        /// <value>The font.</value>
        /// <remarks>If the value is <c>null</c>, the DefaultFont of the parent PlotModel will be used.</remarks>
        public string Font { get; set; }

        /// <summary>
        /// Gets or sets the size of the font. The default is <c>double.NaN</c> (use <see cref="OxyPlot.PlotModel.DefaultFontSize" />).
        /// </summary>
        /// <value>The size of the font.</value>
        /// <remarks>If the value is <c>NaN</c>, the DefaultFontSize of the parent PlotModel will be used.</remarks>
        public double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the font weight. The default is <c>FontWeights.Normal</c>.
        /// </summary>
        /// <value>The font weight.</value>
        public double FontWeight { get; set; }

        /// <summary>
        /// Gets the parent <see cref="PlotModel" />.
        /// </summary>
        public PlotModel PlotModel
        {
            get
            {
                return (PlotModel)this.Parent;
            }
        }

        /// <summary>
        /// Gets or sets an arbitrary object value that can be used to store custom information about this plot element. The default is <c>null</c>.
        /// </summary>
        /// <value>The intended value.</value>
        /// <remarks>This property is analogous to Tag properties in other Microsoft programming models. Tag is intended to provide a pre-existing property location where you can store some basic custom information about any PlotElement without requiring you to subclass an element.</remarks>
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets the color of the text. The default is <c>OxyColors.Automatic</c> (use <see cref="OxyPlot.PlotModel.TextColor" />).
        /// </summary>
        /// <value>The color of the text.</value>
        /// <remarks>If the value is <c>OxyColors.Automatic</c>, the TextColor of the parent PlotModel will be used.</remarks>
        public OxyColor TextColor { get; set; }

        /// <summary>
        /// Gets or sets the edge rendering mode that is used for rendering the plot element.
        /// </summary>
        /// <value>The edge rendering mode. The default is <see cref="EdgeRenderingMode.Automatic"/>.</value>
        public EdgeRenderingMode EdgeRenderingMode { get; set; }

        /// <summary>
        /// Gets or sets the tool tip. The default is <c>null</c>.
        /// </summary>
        /// <value>
        /// The tool tip string.
        /// </value>
        public string ToolTip { get; set; }

        /// <summary>
        /// Gets the actual font.
        /// </summary>
        protected internal string ActualFont
        {
            get
            {
                return this.Font ?? this.PlotModel.DefaultFont;
            }
        }

        /// <summary>
        /// Gets the actual size of the font.
        /// </summary>
        /// <value>The actual size of the font.</value>
        protected internal double ActualFontSize
        {
            get
            {
                return !double.IsNaN(this.FontSize) ? this.FontSize : this.PlotModel.DefaultFontSize;
            }
        }

        /// <summary>
        /// Gets the actual font weight.
        /// </summary>
        protected internal double ActualFontWeight
        {
            get
            {
                return this.FontWeight;
            }
        }

        /// <summary>
        /// Gets the actual color of the text.
        /// </summary>
        /// <value>The actual color of the text.</value>
        protected internal OxyColor ActualTextColor
        {
            get
            {
                return this.TextColor.GetActualColor(this.PlotModel.TextColor);
            }
        }

        /// <summary>
        /// Gets the actual culture.
        /// </summary>
        /// <remarks>The culture is defined in the parent PlotModel.</remarks>
        protected CultureInfo ActualCulture
        {
            get
            {
                return this.PlotModel != null ? this.PlotModel.ActualCulture : CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        /// Returns a hash code for this element.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        /// <remarks>This method creates the hash code by reflecting the value of all public properties.</remarks>
        public virtual int GetElementHashCode()
        {
#if NET40
            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
#else
            // Get the values of all properties in the object (this is slow, any better ideas?)
            var properties = this.GetType().GetRuntimeProperties().Where(pi => pi.GetMethod.IsPublic && !pi.GetMethod.IsStatic);
#endif

            var propertyValues = properties.Select(pi => pi.GetValue(this, null));
            return HashCodeBuilder.GetHashCode(propertyValues);
        }
    }
}
