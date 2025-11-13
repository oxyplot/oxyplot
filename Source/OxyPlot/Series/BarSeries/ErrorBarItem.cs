// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorBarItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an item used in the ErrorColumnSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represents an item used in the ErrorColumnSeries.
    /// </summary>
    public class ErrorBarItem : BarItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBarItem" /> class.
        /// </summary>
        public ErrorBarItem()
        {
            this.Color = OxyColors.Automatic;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBarItem" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="error">The error.</param>
        /// <param name="categoryIndex">Index of the category.</param>
        public ErrorBarItem(double value, double error, int categoryIndex = -1)
            : this()
        {
            this.Value = value;
            this.Error = error;
            this.CategoryIndex = categoryIndex;
        }

        /// <summary>
        /// Gets or sets the error of the item.
        /// </summary>
        public double Error { get; set; }

        /// <summary>
        /// Gets or sets the color of marker
        /// </summary>
        public OxyColor MarkerColor { get; set; }


        /// <summary>
        /// Gets or sets the marker Type
        /// </summary>
        public MarkerType MarkerType { get; set; }
        
        /// <summary>
        /// Gets or sets a custom polygon outline for the point marker. 
        /// </summary>
        public ScreenPoint[] CustomOutline { get; set; }

        /// <summary>
        /// Gets or sets the marker size
        /// </summary>
        public int MarkerSize { get; set; }

        /// <summary>
        /// Gets or sets the marker visible
        /// </summary>
        public bool IsMarkerVisible { get; set; }

        /// <summary>
        /// Gets or sets the marker offset
        /// </summary>
        public ScreenPoint MarkerOffset { get; set; }

        /// <summary>
        /// Gets or sets the marker stroke color
        /// </summary>
        public OxyColor MarkerStrokeColor { get; set; }

        /// <summary>
        /// Gets or sets the marker stroke thickness
        /// </summary>
        public double MarkerStrokeThickness { get; set; }

        /// <summary>
        /// Returns c# code that generates this instance.
        /// </summary>
        /// <returns>C# code.</returns>
        public override string ToCode()
        {
            if (!this.Color.IsUndefined())
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(), "{0},{1},{2},{3}", this.Value, this.Error, this.CategoryIndex, this.Color.ToCode());
            }

            if (this.CategoryIndex != -1)
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(), "{0},{1},{2}", this.Value, this.Error, this.CategoryIndex);
            }

            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.Value, this.Error);
        }
    }
}
