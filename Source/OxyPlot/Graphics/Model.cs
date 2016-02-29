// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Model.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for graphics models.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;

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
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// A sequence of hit results.
        /// </returns>
        public IEnumerable<HitTestResult> HitTest(HitTestArguments args)
        {
            var hitTestElements = this.GetHitTestElements();

            foreach (var element in hitTestElements)
            {
                var result = element.HitTest(args);
                if (result != null)
                {
                    yield return result;
                }
            }
        }

        /// <summary>
        /// Gets all elements of the model, top-level elements first.
        /// </summary>
        /// <returns>An enumerator of the elements.</returns>
        protected abstract IEnumerable<PlotElement> GetHitTestElements();
    }
}