// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntervalBarItem.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Represents an interval in an IntervalBarSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents an item in an IntervalBarSeries.
    /// </summary>
    public class IntervalBarItem : CategorizedItem, ICodeGenerating
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalBarItem"/> class.
        /// </summary>
        public IntervalBarItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalBarItem"/> class.
        /// </summary>
        /// <param name="start">
        /// The start. 
        /// </param>
        /// <param name="end">
        /// The end. 
        /// </param>
        /// <param name="title">
        /// The title. 
        /// </param>
        /// <param name="color">
        /// The color. 
        /// </param>
        public IntervalBarItem(double start, double end, string title = null, OxyColor color = null)
        {
            this.Start = start;
            this.End = end;
            this.Title = title;
            this.Color = color;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets or sets the end value.
        /// </summary>
        public double End { get; set; }

        /// <summary>
        /// Gets or sets the start value.
        /// </summary>
        public double Start { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns c# code that generates this instance.
        /// </summary>
        /// <returns>
        /// C# code. 
        /// </returns>
        public string ToCode()
        {
            if (this.Color != null)
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(), "{0},{1},{2},{3}", this.Start, this.End, this.Title, this.Color.ToCode());
            }

            if (this.Title != null)
            {
                return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1},{2}", this.Start, this.End, this.Title);
            }

            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.Start, this.End);
        }

        #endregion
    }
}