// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyMouseDownGesture.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a mouse down input gesture.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents a mouse down input gesture.
    /// </summary>
    /// <remarks>The input gesture can be bound to a command in a <see cref="PlotController" />.</remarks>
    public class OxyMouseDownGesture : OxyInputGesture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OxyMouseDownGesture" /> class.
        /// </summary>
        /// <param name="mouseButton">The mouse button.</param>
        /// <param name="modifiers">The modifiers.</param>
        /// <param name="clickCount">The click count.</param>
        public OxyMouseDownGesture(OxyMouseButton mouseButton, OxyModifierKeys modifiers = OxyModifierKeys.None, int clickCount = 1)
        {
            this.MouseButton = mouseButton;
            this.Modifiers = modifiers;
            this.ClickCount = clickCount;
        }

        /// <summary>
        /// Gets the modifier keys.
        /// </summary>
        public OxyModifierKeys Modifiers { get; private set; }

        /// <summary>
        /// Gets the mouse button.
        /// </summary>
        public OxyMouseButton MouseButton { get; private set; }

        /// <summary>
        /// Gets the click count.
        /// </summary>
        public int ClickCount { get; private set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.</returns>
        public override bool Equals(OxyInputGesture other)
        {
            var mg = other as OxyMouseDownGesture;
            return mg != null && mg.Modifiers == this.Modifiers && mg.MouseButton == this.MouseButton && mg.ClickCount == this.ClickCount;
        }
    }
}