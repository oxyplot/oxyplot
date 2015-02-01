// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using Xwt.Drawing;

namespace OxyPlot.Xwt
{
    /// <summary>
    /// Represents settings for the tracker.
    /// </summary>
	public class TrackerSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether the tracker is enabled.
		/// </summary>
		/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
		public bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		/// <value>The background color.</value>
		public OxyColor Background { get; set; }

		/// <summary>
		/// Gets or sets the text padding.
		/// </summary>
		/// <value>The padding.</value>
		public double Padding { get; set; }

		/// <summary>
		/// Gets or sets the font.
		/// </summary>
		/// <value>The font.</value>
		public Font Font { get; set; }

		/// <summary>
		/// Gets or sets the color of the text.
		/// </summary>
		/// <value>The color of the text.</value>
		public OxyColor TextColor { get; set; }

		/// <summary>
		/// Gets or sets the color of the border.
		/// </summary>
		/// <value>The color of the border.</value>
		public OxyColor BorderColor { get; set; }

		/// <summary>
		/// Gets or sets the width of the border.
		/// </summary>
		/// <value>The width of the border.</value>
		public double BorderWidth { get; set; }

		/// <summary>
		/// Gets or sets the color of the horizontal line.
		/// </summary>
		/// <value>The color of the horizontal line.</value>
		public OxyColor HorizontalLineColor { get; set; }

		/// <summary>
		/// Gets or sets the width of the horizontal line.
		/// </summary>
		/// <value>The width of the horizontal line.</value>
		public double HorizontalLineWidth { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="OxyPlot.Xwt.TrackerSettings"/> horizontal line visible.
		/// </summary>
		/// <value><c>true</c> if horizontal line visible; otherwise, <c>false</c>.</value>
		public bool HorizontalLineVisible { get; set; }

		/// <summary>
		/// Gets or sets the horizontal line style (overridden by <see cref="HorizontalLineDashArray" />).
		/// </summary>
		/// <value>The horizontal line style.</value>
		public LineStyle HorizontalLineStyle { get; set; }

		/// <summary>
		/// Gets or sets the horizontal dash array (overrides <see cref="HorizontalLineStyle" />).
		/// </summary>
		/// <value>The horizontal dash array.</value>
		public double[] HorizontalLineDashArray { get; set; }

		/// <summary>
		/// Gets the actual horizontal dash array.
		/// </summary>
		/// <value>The actual horizontal dash array.</value>
		public double[] HorizontalLineActualDashArray
		{
			get
			{
				return HorizontalLineDashArray ?? HorizontalLineStyle.GetDashArray();
			}
		}

		/// <summary>
		/// Gets or sets the color of the vertical line.
		/// </summary>
		/// <value>The color of the vertical line.</value>
		public OxyColor VerticalLineColor { get; set; }

		/// <summary>
		/// Gets or sets the width of the vertical line.
		/// </summary>
		/// <value>The width of the vertical line.</value>
		public double VerticalLineWidth { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="OxyPlot.Xwt.TrackerSettings"/> vertical line visible.
		/// </summary>
		/// <value><c>true</c> if vertical line visible; otherwise, <c>false</c>.</value>
		public bool VerticalLineVisible { get; set; }

		/// <summary>
		/// Gets or sets the vertical line style (overridden by <see cref="HorizontalLineDashArray" />).
		/// </summary>
		/// <value>The vertical line style.</value>
		public LineStyle VerticalLineStyle { get; set; }

		/// <summary>
		/// Gets or sets the vertical dash array (overrides <see cref="HorizontalLineStyle" />).
		/// </summary>
		/// <value>The vertical dash array.</value>
		public double[] VerticalLineDashArray { get; set; }

		/// <summary>
		/// Gets the actual vertical dash array.
		/// </summary>
		/// <value>The actual vertical dash array.</value>
		public double[] VerticalLineActualDashArray
		{
			get
			{
				return VerticalLineDashArray ?? VerticalLineStyle.GetDashArray();
			}
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackerSettings"/> class.
        /// </summary>
		public TrackerSettings ()
		{
			Enabled = false;
			Background = OxyColor.FromArgb (0x80, 0xFF, 0xFF, 0x00);
			Font = Font.SystemFont;
			TextColor = OxyColors.Black;
			Padding = 6;
			BorderWidth = 1;
			BorderColor = OxyColor.FromArgb (0xFF, 0x3C, 0x3C, 0x3C);
			HorizontalLineColor = OxyColors.Black;
			HorizontalLineWidth = 1.0;
			HorizontalLineVisible = true;
			HorizontalLineStyle = LineStyle.Solid;
			HorizontalLineDashArray = null;
			VerticalLineColor = OxyColors.Black;
			VerticalLineWidth = 1.0;
			VerticalLineVisible = true;
			VerticalLineStyle = LineStyle.Solid;
			VerticalLineDashArray = null;
		}
	}
}

