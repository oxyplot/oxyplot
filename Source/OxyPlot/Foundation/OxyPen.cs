// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPen.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Describes a pen in terms of color, thickness, line style and line join type.
    /// </summary>
    public class OxyPen
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPen"/> class.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        /// <param name="lineStyle">
        /// The line style.
        /// </param>
        /// <param name="lineJoin">
        /// The line join.
        /// </param>
        public OxyPen(
            OxyColor color,
            double thickness = 1.0,
            LineStyle lineStyle = LineStyle.Solid,
            OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter)
        {
            this.Color = color;
            this.Thickness = thickness;
            this.DashArray = LineStyleHelper.GetDashArray(lineStyle);
            this.LineStyle = lineStyle;
            this.LineJoin = lineJoin;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color { get; set; }

        /// <summary>
        ///   Gets or sets the dash array.
        /// </summary>
        /// <value>The dash array.</value>
        public double[] DashArray { get; set; }

        /// <summary>
        ///   Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public OxyPenLineJoin LineJoin { get; set; }

        /// <summary>
        ///   Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        ///   Gets or sets the thickness.
        /// </summary>
        /// <value>The thickness.</value>
        public double Thickness { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the specified pen.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <returns>A pen.</returns>
        public static OxyPen Create(
            OxyColor color,
            double thickness,
            LineStyle lineStyle = LineStyle.Solid,
            OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter)
        {
            if (color == null || lineStyle == LineStyle.None || Math.Abs(thickness) < double.Epsilon)
            {
                return null;
            }

            return new OxyPen(color, thickness, lineStyle, lineJoin);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.Color.GetHashCode();
                result = (result * 397) ^ this.Thickness.GetHashCode();
                result = (result * 397) ^ this.LineStyle.GetHashCode();
                result = (result * 397) ^ this.LineJoin.GetHashCode();
                return result;
            }
        }

        #endregion
    }
}