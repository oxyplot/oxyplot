// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Annotation.cs" company="OxyPlot">
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
//   The annotation base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Windows;

    using OxyPlot.Annotations;

    /// <summary>
    /// The annotation base class.
    /// </summary>
    public abstract class Annotation : FrameworkElement
    {
        /// <summary>
        /// The layer property.
        /// </summary>
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(
            "Layer",
            typeof(AnnotationLayer),
            typeof(Annotation),
            new PropertyMetadata(AnnotationLayer.AboveSeries, AppearanceChanged));

        /// <summary>
        /// Gets or sets the layer.
        /// </summary>
        public AnnotationLayer Layer
        {
            get
            {
                return (AnnotationLayer)this.GetValue(LayerProperty);
            }

            set
            {
                this.SetValue(LayerProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the internal annotation object.
        /// </summary>
        public OxyPlot.Annotations.Annotation InternalAnnotation { get; protected set; }

        /// <summary>
        /// Creates the internal annotation object.
        /// </summary>
        /// <returns>
        /// The annotation.
        /// </returns>
        public abstract OxyPlot.Annotations.Annotation CreateModel();

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        public virtual void SynchronizeProperties()
        {
            var a = this.InternalAnnotation;
            a.Layer = this.Layer;
        }

        /// <summary>
        /// Handles changes in appearance.
        /// </summary>
        /// <param name="d">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        protected static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pc = ((Annotation)d).Parent as IPlotControl;
            if (pc != null)
            {
                pc.InvalidatePlot(false);
            }
        }

        /// <summary>
        /// Handles changes in data.
        /// </summary>
        /// <param name="d">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        protected static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pc = ((Annotation)d).Parent as IPlotControl;
            if (pc != null)
            {
                pc.InvalidatePlot();
            }
        }
    }
}