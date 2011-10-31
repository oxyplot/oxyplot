// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxySize.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Globalization;

    /// <summary>
    /// Implements a structure that is used to describe the Size of an object.
    /// </summary>
    public struct OxySize
    {
        #region Constants and Fields

        /// <summary>
        ///   Empty Size.
        /// </summary>
        public static OxySize Empty = new OxySize(0, 0);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OxySize"/> struct.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public OxySize(double width, double height)
            : this()
        {
            this.Width = width;
            this.Height = height;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the height.
        /// </summary>
        /// <value>
        ///   The height.
        /// </value>
        public double Height { get; set; }

        /// <summary>
        ///   Gets or sets the width.
        /// </summary>
        /// <value>
        ///   The width.
        /// </value>
        public double Width { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1})", this.Width, this.Height);
        }

        #endregion
    }
}