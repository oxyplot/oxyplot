// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Annotation.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
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