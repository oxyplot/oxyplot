// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Annotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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
        /// Identifies the <see cref="Layer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(
            "Layer",
            typeof(AnnotationLayer),
            typeof(Annotation),
            new PropertyMetadata(AnnotationLayer.AboveSeries, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="XAxisKey"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty XAxisKeyProperty = DependencyProperty.Register(
            "XAxisKey",
            typeof(string),
            typeof(Annotation),
            new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="YAxisKey"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YAxisKeyProperty = DependencyProperty.Register(
            "YAxisKey",
            typeof(string),
            typeof(Annotation),
            new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Gets or sets the rendering layer of the annotation. The default value is <see cref="AnnotationLayer.AboveSeries" />.
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
        /// Gets or sets the X axis key.
        /// </summary>
        public string XAxisKey
        {
            get
            {
                return (string)this.GetValue(XAxisKeyProperty);
            }

            set
            {
                this.SetValue(XAxisKeyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Y axis key.
        /// </summary>
        public string YAxisKey
        {
            get
            {
                return (string)this.GetValue(YAxisKeyProperty);
            }

            set
            {
                this.SetValue(YAxisKeyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the internal annotation object.
        /// </summary>
        public Annotations.Annotation InternalAnnotation { get; protected set; }

        /// <summary>
        /// Creates the internal annotation object.
        /// </summary>
        /// <returns>The annotation.</returns>
        public abstract Annotations.Annotation CreateModel();

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        public virtual void SynchronizeProperties()
        {
            var a = this.InternalAnnotation;
            a.Layer = this.Layer;
            a.XAxisKey = this.XAxisKey;
            a.YAxisKey = this.YAxisKey;
            a.ToolTip = this.ToolTip as string;
        }

        /// <summary>
        /// Handles changes in appearance.
        /// </summary>
        /// <param name="d">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pc = ((Annotation)d).Parent as IPlotView;
            if (pc != null)
            {
                pc.InvalidatePlot(false);
            }
        }

        /// <summary>
        /// Handles changes in data.
        /// </summary>
        /// <param name="d">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pc = ((Annotation)d).Parent as IPlotView;
            if (pc != null)
            {
                pc.InvalidatePlot();
            }
        }
    }
}
