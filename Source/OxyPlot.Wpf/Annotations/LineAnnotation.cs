using System.Windows;
using System.Windows.Media;

namespace OxyPlot.Wpf
{
    public class LineAnnotation : Annotation
    {
        public static readonly DependencyProperty EquationTypeProperty =
            DependencyProperty.Register("Type", typeof (LineAnnotationType), typeof (LineAnnotation),
                                        new UIPropertyMetadata(LineAnnotationType.LinearEquation));

        public static readonly DependencyProperty SlopeProperty =
            DependencyProperty.Register("Slope", typeof (double), typeof (LineAnnotation), new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty InterceptProperty =
            DependencyProperty.Register("Intercept", typeof (double), typeof (LineAnnotation),
                                        new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof (double), typeof (LineAnnotation), new UIPropertyMetadata(0.0));


        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof (double), typeof (LineAnnotation), new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof (Color), typeof (LineAnnotation),
                                        new UIPropertyMetadata(Colors.Blue));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof (double), typeof (LineAnnotation),
                                        new UIPropertyMetadata(1.0));

        public static readonly DependencyProperty LineStyleProperty =
            DependencyProperty.Register("LineStyle", typeof (LineStyle), typeof (LineAnnotation),
                                        new UIPropertyMetadata(LineStyle.Dash));

        public static readonly DependencyProperty MinimumXProperty =
            DependencyProperty.Register("MinimumX", typeof (double), typeof (LineAnnotation),
                                        new UIPropertyMetadata(double.MinValue));

        public static readonly DependencyProperty MaximumXProperty =
            DependencyProperty.Register("MaximumX", typeof (double), typeof (LineAnnotation),
                                        new UIPropertyMetadata(double.MaxValue));

        public static readonly DependencyProperty MinimumYProperty =
            DependencyProperty.Register("MinimumY", typeof (double), typeof (LineAnnotation),
                                        new UIPropertyMetadata(double.MinValue));


        public static readonly DependencyProperty MaximumYProperty =
            DependencyProperty.Register("MaximumY", typeof (double), typeof (LineAnnotation),
                                        new UIPropertyMetadata(double.MaxValue));

        public LineAnnotation()
        {
            ModelAnnotation = new OxyPlot.LineAnnotation();
        }

        public LineAnnotationType Type
        {
            get { return (LineAnnotationType) GetValue(EquationTypeProperty); }
            set { SetValue(EquationTypeProperty, value); }
        }

        public double Slope
        {
            get { return (double) GetValue(SlopeProperty); }
            set { SetValue(SlopeProperty, value); }
        }

        public double Intercept
        {
            get { return (double) GetValue(InterceptProperty); }
            set { SetValue(InterceptProperty, value); }
        }

        public double X
        {
            get { return (double) GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public double Y
        {
            get { return (double) GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public Color Color
        {
            get { return (Color) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double) GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public LineStyle LineStyle
        {
            get { return (LineStyle) GetValue(LineStyleProperty); }
            set { SetValue(LineStyleProperty, value); }
        }

        public double MinimumX
        {
            get { return (double) GetValue(MinimumXProperty); }
            set { SetValue(MinimumXProperty, value); }
        }

        public double MaximumX
        {
            get { return (double) GetValue(MaximumXProperty); }
            set { SetValue(MaximumXProperty, value); }
        }

        public double MinimumY
        {
            get { return (double) GetValue(MinimumYProperty); }
            set { SetValue(MinimumYProperty, value); }
        }

        public double MaximumY
        {
            get { return (double) GetValue(MaximumYProperty); }
            set { SetValue(MaximumYProperty, value); }
        }

        public override void UpdateModelProperties()
        {
            base.UpdateModelProperties();
            var a = ModelAnnotation as OxyPlot.LineAnnotation;
            a.Type = Type;
            a.Slope = Slope;
            a.Intercept = Intercept;
            a.X = X;
            a.Y = Y;

            a.MinimumX = MinimumX;
            a.MaximumX = MaximumX;
            a.MinimumY = MinimumY;
            a.MaximumY = MaximumY;

            a.Color = Color.ToOxyColor();
            a.StrokeThickness = StrokeThickness;
            a.LineStyle = LineStyle;
        }
    }
}