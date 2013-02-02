// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerDefinition.cs" company="OxyPlot">
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
//   The tracker definition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Silverlight
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The tracker definition.
    /// </summary>
    public class TrackerDefinition : DependencyObject
    {
        /// <summary>
        /// The tracker key property.
        /// </summary>
        public static readonly DependencyProperty TrackerKeyProperty = DependencyProperty.Register(
            "TrackerKey", typeof(string), typeof(TrackerDefinition), new PropertyMetadata(null));

        /// <summary>
        /// The tracker template property.
        /// </summary>
        public static readonly DependencyProperty TrackerTemplateProperty =
            DependencyProperty.Register(
                "TrackerTemplate", typeof(ControlTemplate), typeof(TrackerDefinition), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the tracker key.
        /// </summary>
        /// <remarks>
        /// The Plot will use this property to find the TrackerDefinition that matches the TrackerKey of the current series.
        /// </remarks>
        public string TrackerKey
        {
            get
            {
                return (string)this.GetValue(TrackerKeyProperty);
            }

            set
            {
                this.SetValue(TrackerKeyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the tracker template.
        /// </summary>
        /// <remarks>
        /// The tracker control will be added/removed from the Tracker overlay as neccessary.
        /// The DataContext of the tracker will be set to a TrackerHitResult with the current tracker data.
        /// </remarks>
        public ControlTemplate TrackerTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(TrackerTemplateProperty);
            }

            set
            {
                this.SetValue(TrackerTemplateProperty, value);
            }
        }
    }
}