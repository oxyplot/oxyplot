using System.Windows;
using System.Windows.Controls;
using OxyPlot.Annotations;

namespace OxyPlotControls.Controls.Base;

/// <summary>
/// Base class for annotation editor controls.
/// </summary>
public class AnnotationControlBase : UserControl
{
    public static readonly DependencyProperty AnnotationProperty =
        DependencyProperty.Register(
            nameof(Annotation),
            typeof(Annotation),
            typeof(AnnotationControlBase),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public Annotation? Annotation
    {
        get => (Annotation?)GetValue(AnnotationProperty);
        set => SetValue(AnnotationProperty, value);
    }
}
