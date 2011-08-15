namespace OxyPlot.Wpf
{
    using System.Windows;

    public abstract class Annotation : FrameworkElement, IAnnotation
    {
        #region Constants and Fields

        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(
            "Layer", typeof(AnnotationLayer), typeof(Annotation), new UIPropertyMetadata(AnnotationLayer.OverSeries));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(Annotation), new UIPropertyMetadata(null));

        #endregion

        #region Public Properties

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

        public OxyPlot.IAnnotation internalAnnotation { get; set; }

        #endregion

        #region Public Methods

        public abstract OxyPlot.IAnnotation CreateModel();

        public virtual void SynchronizeProperties()
        {
            var a = this.internalAnnotation as OxyPlot.Annotation;
            a.Text = this.Text;
            a.Layer = this.Layer;
        }

        #endregion
    }
}