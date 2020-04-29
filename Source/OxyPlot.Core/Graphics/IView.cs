// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies common functionality for the views.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies common functionality for the views.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Gets the actual model in the view.
        /// </summary>
        /// <value>
        /// The actual <see cref="Model" />.
        /// </value>
        Model ActualModel { get; }

        /// <summary>
        /// Gets the actual controller.
        /// </summary>
        /// <value>
        /// The actual <see cref="IController" />.
        /// </value>
        IController ActualController { get; }

        /// <summary>
        /// Gets the coordinates of the client area of the view.
        /// </summary>
        /// <value>
        /// The client area rectangle.
        /// </value>
        OxyRect ClientArea { get; }

        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">The cursor type.</param>
        void SetCursorType(CursorType cursorType);

        /// <summary>
        /// Hides the zoom rectangle.
        /// </summary>
        void HideZoomRectangle();

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        void ShowZoomRectangle(OxyRect rectangle);
    }
}