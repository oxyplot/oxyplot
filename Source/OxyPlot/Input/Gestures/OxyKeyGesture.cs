// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyKeyGesture.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a keyboard input gesture.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents a keyboard input gesture.
    /// </summary>
    /// <remarks>The input gesture can be bound to a command in a <see cref="PlotController" />.</remarks>
    public class OxyKeyGesture : OxyInputGesture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OxyKeyGesture" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifiers">The modifier keys.</param>
        public OxyKeyGesture(OxyKey key, OxyModifierKeys modifiers = OxyModifierKeys.None)
        {
            this.Key = key;
            this.Modifiers = modifiers;
        }

        /// <summary>
        /// Gets or sets the modifier keys.
        /// </summary>
        public OxyModifierKeys Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public OxyKey Key { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.</returns>
        public override bool Equals(OxyInputGesture other)
        {
            var kg = other as OxyKeyGesture;
            return kg != null && kg.Modifiers == this.Modifiers && kg.Key == this.Key;
        }
    }
}