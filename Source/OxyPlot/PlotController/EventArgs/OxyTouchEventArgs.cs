// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyTouchEventArgs.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
        /// Initializes a new instance of the <see cref="OxyTouchEventArgs"/> class.
        /// </summary>
        public OxyTouchEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyTouchEventArgs"/> class.
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
        /// <value>
        /// The position.
        /// </value>
        public ScreenPoint Position { get; set; }

        /// <summary>
        /// Gets or sets the relative change in scale.
        /// </summary>
        /// <value>
        /// The scale change.
        /// </value>
        public ScreenVector DeltaScale { get; set; }

        /// <summary>
        /// Gets or sets the change in x and y direction.
        /// </summary>
        /// <value>
        /// The translation.
        /// </value>
        public ScreenVector DeltaTranslation { get; set; }
    }
}