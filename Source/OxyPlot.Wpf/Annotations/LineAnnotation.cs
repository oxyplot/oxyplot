namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    public class LineAnnotation : Annotation
    {
        #region Constants and Fields

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(LineAnnotation), new UIPropertyMetadata(Colors.Blue));

        public static readonly DependencyProperty EquationTypeProperty = DependencyProperty.Register(
            "Type",
            typeof(LineAnnotationType),
            typeof(LineAnnotation),
            new UIPropertyMetadata(LineAnnotationType.LinearEquation));

        public static readonly DependencyProperty InterceptProperty = DependencyProperty.Register(
            "Intercept", typeof(double), typeof(LineAnnotation), new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(
            "LineStyle", typeof(LineStyle), typeof(LineAnnotation), new UIPropertyMetadata(LineStyle.Dash));

        public static readonly DependencyProperty MaximumXProperty = DependencyProperty.Register(
            "MaximumX", typeof(double), typeof(LineAnnotation), new UIPropertyMetadata(double.MaxValue));

        public static readonly DependencyProperty MaximumYProperty = DependencyProperty.Register(
            "MaximumY", typeof(double), typeof(LineAnnotation), new UIPropertyMetadata(double.MaxValue));

        public static readonly DependencyProperty MinimumXProperty = DependencyProperty.Register(
            "MinimumX", typeof(double), typeof(LineAnnotation), new UIPropertyMetadata(double.MinValue));

        public static readonly DependencyProperty MinimumYProperty = DependencyProperty.Register(
            "MinimumY", typeof(double), typeof(LineAnnotation), new UIPropertyMetadata(double.MinValue));

        public static readonly DependencyProperty SlopeProperty = DependencyProperty.Register(
            "Slope", typeof(double), typeof(LineAnnotation), new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", typeof(double), typeof(LineAnnotation), new UIPropertyMetadata(1.0));

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", typeof(double), typeof(LineAnnotation), new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", typeof(double), typeof(LineAnnotation), new UIPropertyMetadata(0.0));

        #endregion

        #region Constructors and Destructors

        public LineAnnotation()
        {
            this.internalAnnotation = new OxyPlot.LineAnnotation();
        }

        #endregion

        #region Public Properties

        public Color Color
        {
            get
            {
                return (Color)this.GetValue(ColorProperty);
            }
            set
            {
                this.SetValue(ColorProperty, value);
            }
        }

        public double Intercept
        {
            get
            {
                return (double)this.GetValue(InterceptProperty);
            }
            set
            {
                this.SetValue(InterceptProperty, value);
            }
        }

        public LineStyle LineStyle
        {
            get
            {
                return (LineStyle)this.GetValue(LineStyleProperty);
            }
            set
            {
                this.SetValue(LineStyleProperty, value);
            }
        }

        public double MaximumX
        {
            get
            {
                return (double)this.GetValue(MaximumXProperty);
            }
            set
            {
                this.SetValue(MaximumXProperty, value);
            }
        }

        public double MaximumY
        {
            get
            {
                return (double)this.GetValue(MaximumYProperty);
            }
            set
            {
                this.SetValue(MaximumYProperty, value);
            }
        }

        public double MinimumX
        {
            get
            {
                return (double)this.GetValue(MinimumXProperty);
            }
            set
            {
                this.SetValue(MinimumXProperty, value);
            }
        }

        public double MinimumY
        {
            get
            {
                return (double)this.GetValue(MinimumYProperty);
            }
            set
            {
                this.SetValue(MinimumYProperty, value);
            }
        }

        public double Slope
        {
            get
            {
                return (double)this.GetValue(SlopeProperty);
            }
            set
            {
                this.SetValue(SlopeProperty, value);
            }
        }

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

        public LineAnnotationType Type
        {
            get
            {
                return (LineAnnotationType)this.GetValue(EquationTypeProperty);
            }
            set
            {
                this.SetValue(EquationTypeProperty, value);
            }
        }

        public double X
        {
            get
            {
                return (double)this.GetValue(XProperty);
            }
            set
            {
                this.SetValue(XProperty, value);
            }
        }

        public double Y
        {
            get
            {
                return (double)this.GetValue(YProperty);
            }
            set
            {
                this.SetValue(YProperty, value);
            }
        }

        #endregion

        #region Public Methods

        public override OxyPlot.IAnnotation CreateModel()
        {
            this.SynchronizeProperties();
            return this.internalAnnotation;
        }

        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = this.internalAnnotation as OxyPlot.LineAnnotation;
            a.Type = this.Type;
            a.Slope = this.Slope;
            a.Intercept = this.Intercept;
            a.X = this.X;
            a.Y = this.Y;

            a.MinimumX = this.MinimumX;
            a.MaximumX = this.MaximumX;
            a.MinimumY = this.MinimumY;
            a.MaximumY = this.MaximumY;

            a.Color = this.Color.ToOxyColor();
            a.StrokeThickness = this.StrokeThickness;
            a.LineStyle = this.LineStyle;
        }

        #endregion
    }
}