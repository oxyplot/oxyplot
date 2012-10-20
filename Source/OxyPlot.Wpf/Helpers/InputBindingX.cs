// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputBindingX.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
//   Represents an input binding that supports binding to the InputGesture.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Represents an input binding that supports binding to the InputGesture.
    /// </summary>
    public class InputBindingX : InputBinding
    {
        /// <summary>
        /// The gesture property.
        /// </summary>
        public static readonly DependencyProperty GeztureProperty = DependencyProperty.Register(
            "Gezture", typeof(InputGesture), typeof(InputBindingX), new UIPropertyMetadata(null, GeztureChanged));

        /// <summary>
        /// Gets or sets the gesture.
        /// </summary>
        /// <value>The gezture.</value>
        public InputGesture Gezture
        {
            get
            {
                return (InputGesture)this.GetValue(GeztureProperty);
            }

            set
            {
                this.SetValue(GeztureProperty, value);
            }
        }

        /// <summary>
        /// Called when the gesture property changed.
        /// </summary>
        protected virtual void OnGeztureChanged()
        {
            this.Gesture = this.Gezture;
        }

        /// <summary>
        /// Called when the gesture property changed.
        /// </summary>
        /// <param name="d">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private static void GeztureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((InputBindingX)d).OnGeztureChanged();
        }

    }
}