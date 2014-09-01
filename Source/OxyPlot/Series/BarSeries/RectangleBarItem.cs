// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleBarItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a rectangle item in a RectangleBarSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represents a rectangle item in a RectangleBarSeries.
    /// </summary>
    public class RectangleBarItem : ICodeGenerating
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleBarItem" /> class.
        /// </summary>
        public RectangleBarItem()
        {
            this.Color = OxyColors.Automatic;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleBarItem" /> class.
        /// </summary>
        /// <param name="x0">The x0.</param>
        /// <param name="y0">The y0.</param>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        public RectangleBarItem(double x0, double y0, double x1, double y1)
            : this()
        {
            this.X0 = x0;
            this.Y0 = y0;
            this.X1 = x1;
            this.Y1 = y1;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <remarks>If set to Automatic, the FillColor of the RectangleBarSeries will be used.</remarks>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the x0 coordinate.
        /// </summary>
        public double X0 { get; set; }

        /// <summary>
        /// Gets or sets the x1 coordinate.
        /// </summary>
        public double X1 { get; set; }

        /// <summary>
        /// Gets or sets the y0 coordinate.
        /// </summary>
        public double Y0 { get; set; }

        /// <summary>
        /// Gets or sets the y1 coordinate.
        /// </summary>
        public double Y1 { get; set; }

        /// <summary>
        /// Returns c# code that generates this instance.
        /// </summary>
        /// <returns>C# code.</returns>
        public string ToCode()
        {
            if (!this.Color.IsUndefined())
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(),
                    "{0},{1},{2},{3},{4},{5}",
                    this.X0,
                    this.Y0,
                    this.X1,
                    this.Y1,
                    this.Title,
                    this.Color.ToCode());
            }

            if (this.Title != null)
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(), "{0},{1},{2},{3},{4}", this.X0, this.Y0, this.X1, this.Y1, this.Title);
            }

            return CodeGenerator.FormatConstructor(
                this.GetType(), "{0},{1},{2},{3}", this.X0, this.Y0, this.X1, this.Y1);
        }
    }
}