// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Model.cs" company="OxyPlot">
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
//   Provides an abstract base class for graphics models.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides an abstract base class for graphics models.
    /// </summary>
    public abstract partial class Model
    {
        /// <summary>
        /// The default selection color.
        /// </summary>
        internal static readonly OxyColor DefaultSelectionColor = OxyColors.Yellow;

        /// <summary>
        /// The synchronization root object.
        /// </summary>
        private readonly object syncRoot = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        protected Model()
        {
            this.SelectionColor = OxyColors.Yellow;
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="Model" />.
        /// </summary>
        /// <value>A synchronization object.</value>
        /// <remarks>This property can be used when modifying the <see cref="Model" /> on a separate thread (not the thread updating or rendering the model).</remarks>
        public object SyncRoot
        {
            get { return this.syncRoot; }
        }

        /// <summary>
        /// Gets or sets the color of the selection.
        /// </summary>
        /// <value>The color of the selection.</value>
        public OxyColor SelectionColor { get; set; }

        /// <summary>
        /// Returns the elements that are hit at the specified position.
        /// </summary>
        /// <param name="position">The position (in screen space).</param>
        /// <param name="tolerance">The tolerance (in screen space).</param>
        /// <returns>A sequence of hit results.</returns>
        public IEnumerable<HitTestResult> HitTest(ScreenPoint position, double tolerance)
        {
            // Revert the order to handle the top-level elements first
            foreach (var element in this.GetElements().Reverse())
            {
                var uiElement = element as UIElement;
                if (uiElement == null)
                {
                    continue;
                }

                var args = new HitTestArguments(position, MouseHitTolerance);
                var result = uiElement.HitTest(args);
                if (result != null)
                {
                    yield return result;
                }
            }
        }

        /// <summary>
        /// Gets all elements of the model, sorted by rendering priority.
        /// </summary>
        /// <returns>An enumerator of the elements.</returns>
        public abstract IEnumerable<PlotElement> GetElements();
    }
}