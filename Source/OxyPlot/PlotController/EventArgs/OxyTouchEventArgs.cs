// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyTouchEventArgs.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides data for touch events.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides data for touch events.
    /// </summary>
    public class OxyTouchEventArgs : OxyInputEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OxyTouchEventArgs" /> class.
        /// </summary>
        public OxyTouchEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyTouchEventArgs" /> class.
        /// </summary>
        /// <param name="currentTouches">The current touches.</param>
        /// <param name="previousTouches">The previous touches.</param>
        public OxyTouchEventArgs(ScreenPoint[] currentTouches, ScreenPoint[] previousTouches)
        {
            this.Position = currentTouches[0];

            if (currentTouches.Length == previousTouches.Length)
            {
                this.DeltaTranslation = currentTouches[0] - previousTouches[0];
            }

            double scale = 1;
            if (currentTouches.Length > 1 && currentTouches.Length == previousTouches.Length)
            {
                var currentDistance = (currentTouches[1] - currentTouches[0]).Length;
                var previousDistance = (previousTouches[1] - previousTouches[0]).Length;
                scale = currentDistance / previousDistance;

                if (scale < 0.5)
                {
                    scale = 0.5;
                }

                if (scale > 2)
                {
                    scale = 2;
                }
            }

            this.DeltaScale = new ScreenVector(scale, scale);
        }

        /// <summary>
        /// Gets or sets the position of the touch.
        /// </summary>
        /// <value>The position.</value>
        public ScreenPoint Position { get; set; }

        /// <summary>
        /// Gets or sets the relative change in scale.
        /// </summary>
        /// <value>The scale change.</value>
        public ScreenVector DeltaScale { get; set; }

        /// <summary>
        /// Gets or sets the change in x and y direction.
        /// </summary>
        /// <value>The translation.</value>
        public ScreenVector DeltaTranslation { get; set; }
    }
}