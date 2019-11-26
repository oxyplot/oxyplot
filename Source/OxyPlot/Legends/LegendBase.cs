// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies the Legend common properties and methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Legends
{
    /// <summary>
    /// Specifies the placement of the legend box.
    /// </summary>
    public enum LegendPlacement
    {
        /// <summary>
        /// Place the legends inside the plot area.
        /// </summary>
        Inside,

        /// <summary>
        /// Place the legends outside the plot area.
        /// </summary>
        Outside
    }

    /// <summary>
    /// Specifies the position of the legend box.
    /// </summary>
    public enum LegendPosition
    {
        /// <summary>
        /// Place the legend box in the top-left corner.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Place the legend box centered at the top.
        /// </summary>
        TopCenter,

        /// <summary>
        /// Place the legend box in the top-right corner.
        /// </summary>
        TopRight,

        /// <summary>
        /// Place the legend box in the bottom-left corner.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Place the legend box centered at the bottom.
        /// </summary>
        BottomCenter,

        /// <summary>
        /// Place the legend box in the bottom-right corner.
        /// </summary>
        BottomRight,

        /// <summary>
        /// Place the legend box in the left-top corner.
        /// </summary>
        LeftTop,

        /// <summary>
        /// Place the legend box centered at the left.
        /// </summary>
        LeftMiddle,

        /// <summary>
        /// Place the legend box in the left-bottom corner.
        /// </summary>
        LeftBottom,

        /// <summary>
        /// Place the legend box in the right-top corner.
        /// </summary>
        RightTop,

        /// <summary>
        /// Place the legend box centered at the right.
        /// </summary>
        RightMiddle,

        /// <summary>
        /// Place the legend box in the right-bottom corner.
        /// </summary>
        RightBottom
    }

    /// <summary>
    /// Specifies the orientation of the items in the legend box.
    /// </summary>
    public enum LegendOrientation
    {
        /// <summary>
        /// Orient the items horizontally.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Orient the items vertically.
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Specifies the item order of the legends.
    /// </summary>
    public enum LegendItemOrder
    {
        /// <summary>
        /// Render the items in the normal order.
        /// </summary>
        Normal,

        /// <summary>
        /// Render the items in the reverse order.
        /// </summary>
        Reverse
    }

    /// <summary>
    /// Specifies the placement of the legend symbols.
    /// </summary>
    public enum LegendSymbolPlacement
    {
        /// <summary>
        /// Render symbols to the left of the labels.
        /// </summary>
        Left,

        /// <summary>
        /// Render symbols to the right of the labels.
        /// </summary>
        Right
    }

    /// <summary>
    /// The abstract Legend class.
    /// </summary>
    public abstract class LegendBase : PlotElement
    {
        /// <summary>
        /// Override for legend hit test.
        /// </summary>
        /// <param name="args">Arguments passe to the hit test</param>
        /// <returns>The hit test results.</returns>
        protected override HitTestResult HitTestOverride(HitTestArguments args)
        {
            return this.LegendHitTest(args);
        }

        /// <summary>
        /// Defines the legend hit test behaviour.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>The hit test result.</returns>
        protected abstract HitTestResult LegendHitTest(HitTestArguments args);

        /// <summary>
        /// Gets or sets a key to identify this legend. The default is <c>null</c>.
        /// </summary>
        /// The key is used to identify which series to show in the legends by comparing with the Series.LegendKey property.
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the legend is visible. The titles of the series must be set to use the legend.
        /// </summary>
        public bool IsLegendVisible { get; set; }

        /// <summary>
        /// Gets or sets the legend orientation.
        /// </summary>
        /// <value>The legend orientation.</value>
        /// <remarks>Horizontal orientation is reverted to Vertical if Legend is positioned Left or Right of the plot.</remarks>
        public LegendOrientation LegendOrientation { get; set; }

        /// <summary>
        /// Gets or sets the legend padding.
        /// </summary>
        /// <value>The legend padding.</value>
        public double LegendPadding { get; set; }

        /// <summary>
        /// Gets or sets the length of the legend symbols (the default value is 16).
        /// </summary>
        public double LegendSymbolLength { get; set; }

        /// <summary>
        /// Gets or sets the legend symbol margins (distance between the symbol and the text).
        /// </summary>
        /// <value>The legend symbol margin.</value>
        public double LegendSymbolMargin { get; set; }

        /// <summary>
        /// Gets or sets the legend symbol placement.
        /// </summary>
        /// <value>The legend symbol placement.</value>
        public LegendSymbolPlacement LegendSymbolPlacement { get; set; }

        /// <summary>
        /// Gets or sets the legend title.
        /// </summary>
        /// <value>The legend title.</value>
        public string LegendTitle { get; set; }

        /// <summary>
        /// Gets or sets the color of the legend title.
        /// </summary>
        /// <value>The color of the legend title.</value>
        /// <remarks>If this value is <c>null</c>, the TextColor will be used.</remarks>
        public OxyColor LegendTitleColor { get; set; }

        /// <summary>
        /// Gets or sets the legend title font.
        /// </summary>
        /// <value>The legend title font.</value>
        public string LegendTitleFont { get; set; }

        /// <summary>
        /// Gets or sets the size of the legend title font.
        /// </summary>
        /// <value>The size of the legend title font.</value>
        public double LegendTitleFontSize { get; set; }

        /// <summary>
        /// Gets or sets the legend title font weight.
        /// </summary>
        /// <value>The legend title font weight.</value>
        public double LegendTitleFontWeight { get; set; }

        /// <summary>
        /// Gets the legend area.
        /// </summary>
        /// <value>The legend area.</value>
        public OxyRect LegendArea { get; set; }

        /// <summary>
        /// Gets or sets the size of the legend.
        /// </summary>
        public OxySize LegendSize { get; set; }

        /// <summary>
        /// Gets or sets the background color of the legend. Use <c>null</c> for no background.
        /// </summary>
        /// <value>The legend background.</value>
        public OxyColor LegendBackground { get; set; }

        /// <summary>
        /// Gets or sets the border color of the legend.
        /// </summary>
        /// <value>The legend border.</value>
        public OxyColor LegendBorder { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the legend border. Use 0 for no border.
        /// </summary>
        /// <value>The legend border thickness.</value>
        public double LegendBorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the spacing between columns of legend items (only for vertical orientation).
        /// </summary>
        /// <value>The spacing in device independent units.</value>
        public double LegendColumnSpacing { get; set; }

        /// <summary>
        /// Gets or sets the legend font.
        /// </summary>
        /// <value>The legend font.</value>
        public string LegendFont { get; set; }

        /// <summary>
        /// Gets or sets the size of the legend font.
        /// </summary>
        /// <value>The size of the legend font.</value>
        public double LegendFontSize { get; set; }

        /// <summary>
        /// Gets or sets the color of the legend text.
        /// </summary>
        /// <value>The color of the legend text.</value>
        /// <remarks>If this value is <c>null</c>, the TextColor will be used.</remarks>
        public OxyColor LegendTextColor { get; set; }

        /// <summary>
        /// Gets or sets the legend font weight.
        /// </summary>
        /// <value>The legend font weight.</value>
        public double LegendFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the legend item alignment.
        /// </summary>
        /// <value>The legend item alignment.</value>
        public HorizontalAlignment LegendItemAlignment { get; set; }

        /// <summary>
        /// Gets or sets the legend item order.
        /// </summary>
        /// <value>The legend item order.</value>
        public LegendItemOrder LegendItemOrder { get; set; }

        /// <summary>
        /// Gets or sets the horizontal spacing between legend items when the orientation is horizontal.
        /// </summary>
        /// <value>The horizontal distance between items in device independent units.</value>
        public double LegendItemSpacing { get; set; }

        /// <summary>
        /// Gets or sets the vertical spacing between legend items.
        /// </summary>
        /// <value>The spacing in device independent units.</value>
        public double LegendLineSpacing { get; set; }

        /// <summary>
        /// Gets or sets the legend margin.
        /// </summary>
        /// <value>The legend margin.</value>
        public double LegendMargin { get; set; }

        /// <summary>
        /// Gets or sets the max width of the legend.
        /// </summary>
        /// <value>The max width of the legend.</value>
        public double LegendMaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the max height of the legend.
        /// </summary>
        /// <value>The max height of the legend.</value>
        public double LegendMaxHeight { get; set; }

        /// <summary>
        /// Gets or sets the legend placement.
        /// </summary>
        /// <value>The legend placement.</value>
        public LegendPlacement LegendPlacement { get; set; }

        /// <summary>
        /// Gets or sets the legend position.
        /// </summary>
        /// <value>The legend position.</value>
        public LegendPosition LegendPosition { get; set; }

        /// <summary>
        /// Makes the LegendOrientation property safe.
        /// </summary>
        /// <remarks>If Legend is positioned left or right, force it to vertical orientation</remarks>
        public abstract void EnsureLegendProperties();

        /// <summary>
        /// Measures the legend area and gets the legend size.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="availableLegendArea">The area available to legend.</param>
        public abstract OxySize GetLegendSize(IRenderContext rc, OxySize availableLegendArea);

        /// <summary>
        /// Gets the rectangle of the legend box.
        /// </summary>
        /// <param name="legendSize">Size of the legend box.</param>
        /// <returns>The legend area rectangle.</returns>
        public abstract OxyRect GetLegendRectangle(OxySize legendSize);

        /// <summary>
        /// Renders or measures the legends.
        /// </summary>
        /// <param name="rc">The render context.</param>
        public abstract void RenderLegends(IRenderContext rc);
    }
}
