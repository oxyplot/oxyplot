// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineAnnotation.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.LineAnnotation
    /// </summary>
    public class LineAnnotation : Annotation
    {
        #region Constants and Fields

        /// <summary>
        ///   The color property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(LineAnnotation), new PropertyMetadata(Colors.Blue));

        /// <summary>
        ///   The equation type property.
        /// </summary>
        public static readonly DependencyProperty EquationTypeProperty = DependencyProperty.Register(
            "Type", 
            typeof(LineAnnotationType), 
            typeof(LineAnnotation), 
            new PropertyMetadata(LineAnnotationType.LinearEquation));

        /// <summary>
        ///   The intercept property.
        /// </summary>
        public static readonly DependencyProperty InterceptProperty = DependencyProperty.Register(
            "Intercept", typeof(double), typeof(LineAnnotation), new PropertyMetadata(0.0));

        /// <summary>
        ///   The line style property.
        /// </summary>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(
            "LineStyle", typeof(LineStyle), typeof(LineAnnotation), new PropertyMetadata(LineStyle.Dash));

        /// <summary>
        ///   The maximum x property.
        /// </summary>
        public static readonly DependencyProperty MaximumXProperty = DependencyProperty.Register(
            "MaximumX", typeof(double), typeof(LineAnnotation), new PropertyMetadata(double.MaxValue));

        /// <summary>
        ///   The maximum y property.
        /// </summary>
        public static readonly DependencyProperty MaximumYProperty = DependencyProperty.Register(
            "MaximumY", typeof(double), typeof(LineAnnotation), new PropertyMetadata(double.MaxValue));

        /// <summary>
        ///   The minimum x property.
        /// </summary>
        public static readonly DependencyProperty MinimumXProperty = DependencyProperty.Register(
            "MinimumX", typeof(double), typeof(LineAnnotation), new PropertyMetadata(double.MinValue));

        /// <summary>
        ///   The minimum y property.
        /// </summary>
        public static readonly DependencyProperty MinimumYProperty = DependencyProperty.Register(
            "MinimumY", typeof(double), typeof(LineAnnotation), new PropertyMetadata(double.MinValue));

        /// <summary>
        ///   The slope property.
        /// </summary>
        public static readonly DependencyProperty SlopeProperty = DependencyProperty.Register(
            "Slope", typeof(double), typeof(LineAnnotation), new PropertyMetadata(0.0));

        /// <summary>
        ///   The stroke thickness property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", typeof(double), typeof(LineAnnotation), new PropertyMetadata(1.0));

        /// <summary>
        ///   The x property.
        /// </summary>
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", typeof(double), typeof(LineAnnotation), new PropertyMetadata(0.0));

        /// <summary>
        ///   The y property.
        /// </summary>
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", typeof(double), typeof(LineAnnotation), new PropertyMetadata(0.0));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LineAnnotation" /> class.
        /// </summary>
        public LineAnnotation()
        {
            this.internalAnnotation = new OxyPlot.LineAnnotation();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets Color.
        /// </summary>
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

        /// <summary>
        ///   Gets or sets Intercept.
        /// </summary>
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

        /// <summary>
        ///   Gets or sets LineStyle.
        /// </summary>
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

        /// <summary>
        ///   Gets or sets MaximumX.
        /// </summary>
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

        /// <summary>
        ///   Gets or sets MaximumY.
        /// </summary>
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

        /// <summary>
        ///   Gets or sets MinimumX.
        /// </summary>
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

        /// <summary>
        ///   Gets or sets MinimumY.
        /// </summary>
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

        /// <summary>
        ///   Gets or sets Slope.
        /// </summary>
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

        /// <summary>
        ///   Gets or sets StrokeThickness.
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
        ///   Gets or sets Type.
        /// </summary>
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

        /// <summary>
        ///   Gets or sets X.
        /// </summary>
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

        /// <summary>
        ///   Gets or sets Y.
        /// </summary>
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

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// </returns>
        public override OxyPlot.Annotation CreateModel()
        {
            this.SynchronizeProperties();
            return this.internalAnnotation;
        }

        /// <summary>
        /// The synchronize properties.
        /// </summary>
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