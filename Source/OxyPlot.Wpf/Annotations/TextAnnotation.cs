// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextAnnotation.cs" company="OxyPlot">
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
//   This is a WPF wrapper of OxyPlot.TextAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.TextAnnotation
    /// </summary>
    public class TextAnnotation : TextualAnnotation
    {
        /// <summary>
        /// The Background property.
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background", typeof(Color?), typeof(TextAnnotation), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The TextColor property.
        /// </summary>
        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            "TextColor", typeof(Color), typeof(TextAnnotation), new PropertyMetadata(Colors.Blue, AppearanceChanged));

        /// <summary>
        /// The Offset property.
        /// </summary>
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
            "Offset", 
            typeof(Vector), 
            typeof(TextAnnotation), 
            new PropertyMetadata(default(Vector), AppearanceChanged));

        /// <summary>
        /// The Padding property.
        /// </summary>
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            "Padding", 
            typeof(Thickness), 
            typeof(TextAnnotation), 
            new PropertyMetadata(new Thickness(4), AppearanceChanged));

        /// <summary>
        /// The Position property.
        /// </summary>
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position", typeof(DataPoint), typeof(TextAnnotation), new PropertyMetadata(AppearanceChanged));

        /// <summary>
        /// The Rotation property.
        /// </summary>
        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(
            "Rotation", typeof(double), typeof(TextAnnotation), new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// The Stroke property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof(Color), typeof(TextAnnotation), new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        /// The StrokeThickness property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", typeof(double), typeof(TextAnnotation), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "TextAnnotation" /> class.
        /// </summary>
        public TextAnnotation()
        {
            this.InternalAnnotation = new OxyPlot.Annotations.TextAnnotation();
        }

        /// <summary>
        /// Gets or sets the fill color of the background rectangle.
        /// </summary>
        public Color? Background
        {
            get
            {
                return (Color?)this.GetValue(BackgroundProperty);
            }

            set
            {
                this.SetValue(BackgroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public Color TextColor
        {
            get
            {
                return (Color)this.GetValue(TextColorProperty);
            }

            set
            {
                this.SetValue(TextColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the position offset (screen coordinates).
        /// </summary>
        public Vector Offset
        {
            get
            {
                return (Vector)this.GetValue(OffsetProperty);
            }

            set
            {
                this.SetValue(OffsetProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the padding of the background rectangle.
        /// </summary>
        public Thickness Padding
        {
            get
            {
                return (Thickness)this.GetValue(PaddingProperty);
            }

            set
            {
                this.SetValue(PaddingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the position of the text.
        /// </summary>
        public DataPoint Position
        {
            get
            {
                return (DataPoint)this.GetValue(PositionProperty);
            }

            set
            {
                this.SetValue(PositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the rotation angle (degrees).
        /// </summary>
        public double Rotation
        {
            get
            {
                return (double)this.GetValue(RotationProperty);
            }

            set
            {
                this.SetValue(RotationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke color of the background rectangle.
        /// </summary>
        public Color Stroke
        {
            get
            {
                return (Color)this.GetValue(StrokeProperty);
            }

            set
            {
                this.SetValue(StrokeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness of the background rectangle.
        /// </summary>
        public double StrokeThickness
        {
            get
            {
                return (double)this.GetValue(StrokeThicknessProperty);
            }

            set
            {
                this.SetValue(StrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal annotation object.
        /// </summary>
        /// <returns>
        /// The annotation.
        /// </returns>
        public override OxyPlot.Annotations.Annotation CreateModel()
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
            var a = (OxyPlot.Annotations.TextAnnotation)this.InternalAnnotation;
            a.HorizontalAlignment = this.HorizontalAlignment.ToHorizontalTextAlign();
            a.Background = this.Background.ToOxyColor();

            a.Position = this.Position;
            a.Offset = this.Offset.ToScreenVector();
            a.VerticalAlignment = this.VerticalAlignment.ToVerticalTextAlign();
            a.Padding = this.Padding.ToOxyThickness();

            a.TextColor = this.TextColor.ToOxyColor();
            a.Stroke = this.Stroke.ToOxyColor();
            a.StrokeThickness = this.StrokeThickness;

            a.Rotation = this.Rotation;
        }
    }
}