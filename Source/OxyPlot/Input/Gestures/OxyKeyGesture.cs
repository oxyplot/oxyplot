// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyKeyGesture.cs" company="OxyPlot">
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