using System.Windows;

namespace OxyPlot.Wpf
{
    public abstract class Annotation : FrameworkElement
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (string), typeof (Annotation), new UIPropertyMetadata(null));

        public static readonly DependencyProperty LayerProperty =
            DependencyProperty.Register("Layer", typeof (AnnotationLayer), typeof (Annotation),
                                        new UIPropertyMetadata(AnnotationLayer.OverSeries));

        public OxyPlot.Annotation ModelAnnotation { get; protected set; }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public AnnotationLayer Layer
        {
            get { return (AnnotationLayer) GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }

        public virtual void UpdateModelProperties()
        {
            OxyPlot.Annotation a = ModelAnnotation;
            a.Text = Text;
            a.Layer = Layer;
        }
    }
}