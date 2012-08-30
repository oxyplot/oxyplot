// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManipulationEventArgs.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides data for the manipulation events.
    /// </summary>
    public class ManipulationEventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManipulationEventArgs"/> class.
        /// </summary>
        /// <param name="currentPosition">
        /// The current position.
        /// </param>
        public ManipulationEventArgs(ScreenPoint currentPosition)
        {
            this.CurrentPosition = currentPosition;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the current position.
        /// </summary>
        /// <value>The current position.</value>
        public ScreenPoint CurrentPosition { get; private set; }

        /// <summary>
        ///   Gets or sets the X scaling factor.
        /// </summary>
        /// <value>The scale value.</value>
        public double ScaleX { get; set; }

        /// <summary>
        ///   Gets or sets the Y scaling factor.
        /// </summary>
        /// <value>The scale value.</value>
        public double ScaleY { get; set; }

        #endregion
    }
}