// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Annotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The annotation base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Controls;
    using OxyPlot.Annotations;

    /// <summary>
    /// The annotation base class.
    /// </summary>
    public abstract class Annotation : Control
    {
        /// <summary>
        /// Identifies the <see cref="Layer"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<AnnotationLayer> LayerProperty = AvaloniaProperty.Register<Annotation, AnnotationLayer>(nameof(Layer), AnnotationLayer.AboveSeries);

        /// <summary>
        /// Identifies the <see cref="XAxisKey"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> XAxisKeyProperty = AvaloniaProperty.Register<Annotation, string>(nameof(XAxisKey), null);

        /// <summary>
        /// Identifies the <see cref="YAxisKey"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> YAxisKeyProperty = AvaloniaProperty.Register<Annotation, string>(nameof(YAxisKey), null);

        /// <summary>
        /// Gets or sets the rendering layer of the annotation. The default value is <see cref="AnnotationLayer.AboveSeries" />.
        /// </summary>
        public AnnotationLayer Layer
        {
            get
            {
                return GetValue(LayerProperty);
            }

            set
            {
                SetValue(LayerProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the X axis key.
        /// </summary>
        public string XAxisKey
        {
            get
            {
                return GetValue(XAxisKeyProperty);
            }

            set
            {
                SetValue(XAxisKeyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Y axis key.
        /// </summary>
        public string YAxisKey
        {
            get
            {
                return GetValue(YAxisKeyProperty);
            }

            set
            {
                SetValue(YAxisKeyProperty, value);
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
            var a = InternalAnnotation;
            a.Layer = Layer;
            a.XAxisKey = XAxisKey;
            a.YAxisKey = YAxisKey;
        }

        /// <summary>
        /// Handles changes in appearance.
        /// </summary>
        /// <param name="d">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        protected static void AppearanceChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
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
        protected static void DataChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            var pc = ((Annotation)d).Parent as IPlotView;
            if (pc != null)
            {
                pc.InvalidatePlot();
            }
        }

        static Annotation()
        {
            LayerProperty.Changed.AddClassHandler<Annotation>(AppearanceChanged);
            XAxisKeyProperty.Changed.AddClassHandler<Annotation>(AppearanceChanged);
            YAxisKeyProperty.Changed.AddClassHandler<Annotation>(AppearanceChanged);
        }
    }
}