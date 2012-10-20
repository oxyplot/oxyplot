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

    /// <summary>
    /// The annotation base class.
    /// </summary>
    public abstract class Annotation : FrameworkElement
    {
        #region Constants and Fields

        /// <summary>
        ///   The layer property.
        /// </summary>
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(
            "Layer", typeof(AnnotationLayer), typeof(Annotation), new PropertyMetadata(AnnotationLayer.AboveSeries));

        /// <summary>
        ///   The text property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(Annotation), new PropertyMetadata(null));

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets Layer.
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
        ///   Gets or sets Text.
        /// </summary>
        public string Text
        {
            get
            {
                return (string)this.GetValue(TextProperty);
            }

            set
            {
                this.SetValue(TextProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets internalAnnotation.
        /// </summary>
        public OxyPlot.Annotation internalAnnotation { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// </returns>
        public abstract OxyPlot.Annotation CreateModel();

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        public virtual void SynchronizeProperties()
        {
            OxyPlot.Annotation a = this.internalAnnotation;
            a.Text = this.Text;
            a.Layer = this.Layer;
        }

        #endregion
    }
}