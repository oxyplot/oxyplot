// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRenderUnit.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a IRenderUnit definition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.SharpDX
{    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using global::SharpDX;
    using global::SharpDX.Direct2D1;

    /// <summary>
    /// Represents a IRenderUnit definition.
    /// </summary>
    internal interface IRenderUnit : IDisposable
    {
        /// <summary>
        /// Renders image represented by current instance to render target.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        void Render(RenderTarget renderTarget);

        /// <summary>
        /// Checks if current instance bounds intersects with viewport or not.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <returns>Return <c>True</c> if bounds intersects with viewport, otherwise <c>False</c>.</returns>
        bool CheckBounds(RectangleF viewport);
    }
}
