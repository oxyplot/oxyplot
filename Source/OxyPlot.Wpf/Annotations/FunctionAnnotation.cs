// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionAnnotation.cs" company="OxyPlot">
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
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Windows;

    using OxyPlot.Annotations;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.PathAnnotation
    /// </summary>
    public class FunctionAnnotation : OxyPlot.Wpf.PathAnnotation
    {
        /// <summary>
        /// The line type property.
        /// </summary>
        public static readonly DependencyProperty EquationTypeProperty = DependencyProperty.Register(
            "Type",
            typeof(FunctionAnnotationType),
            typeof(FunctionAnnotation),
            new PropertyMetadata(FunctionAnnotationType.EquationX, DataChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionAnnotation"/> class.
        /// </summary>
        public FunctionAnnotation()
        {
            this.InternalAnnotation = new OxyPlot.Annotations.FunctionAnnotation();
        }

        /// <summary>
        /// Gets or sets Type.
        /// </summary>
        public FunctionAnnotationType Type
        {
            get
            {
                return (FunctionAnnotationType)this.GetValue(EquationTypeProperty);
            }

            set
            {
                this.SetValue(EquationTypeProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal annotation object.
        /// </summary>
        /// <returns>
        /// The annotation.
        /// </returns>
        public override Annotations.Annotation CreateModel()
        {
            this.SynchronizeProperties();
            return this.InternalAnnotation;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();

            var a = (OxyPlot.Annotations.FunctionAnnotation)this.InternalAnnotation;
            a.Type = this.Type;
        }
    }
}
