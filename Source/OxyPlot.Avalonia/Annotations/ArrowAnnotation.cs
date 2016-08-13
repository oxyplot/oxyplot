// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrowAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.ArrowAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.ArrowAnnotation
    /// </summary>
    public class ArrowAnnotation : TextualAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="ArrowDirection"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<ScreenVector> ArrowDirectionProperty = AvaloniaProperty.Register<ArrowAnnotation, ScreenVector>(nameof(ArrowDirection));

        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> ColorProperty = AvaloniaProperty.Register<ArrowAnnotation, Color>(nameof(Color), Colors.Blue);

        /// <summary>
        /// Identifies the <see cref="EndPoint"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<DataPoint> EndPointProperty = AvaloniaProperty.Register<ArrowAnnotation, DataPoint>(nameof(EndPoint));

        /// <summary>
        /// Identifies the <see cref="HeadLength"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> HeadLengthProperty = AvaloniaProperty.Register<ArrowAnnotation, double>(nameof(HeadLength), 10.0);

        /// <summary>
        /// Identifies the <see cref="HeadWidth"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> HeadWidthProperty = AvaloniaProperty.Register<ArrowAnnotation, double>(nameof(HeadWidth), 3.0);

        /// <summary>
        /// Identifies the <see cref="LineJoin"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineJoin> LineJoinProperty = AvaloniaProperty.Register<ArrowAnnotation, LineJoin>(nameof(LineJoin), LineJoin.Miter);

        /// <summary>
        /// Identifies the <see cref="LineStyle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> LineStyleProperty = AvaloniaProperty.Register<ArrowAnnotation, LineStyle>(nameof(LineStyle), LineStyle.Solid);

        /// <summary>
        /// Identifies the <see cref="StartPoint"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<DataPoint> StartPointProperty = AvaloniaProperty.Register<ArrowAnnotation, DataPoint>(nameof(StartPoint));

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> StrokeThicknessProperty = AvaloniaProperty.Register<ArrowAnnotation, double>(nameof(StrokeThickness), 2.0);

        /// <summary>
        /// Identifies the <see cref="Veeness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> VeenessProperty = AvaloniaProperty.Register<ArrowAnnotation, double>(nameof(Veeness), 0.0);

        /// <summary>
        /// Initializes static members of the <see cref="ArrowAnnotation"/> class.
        /// </summary>
        static ArrowAnnotation()
        {
            TextColorProperty.OverrideDefaultValue<ArrowAnnotation>(MoreColors.Automatic);
            TextColorProperty.Changed.AddClassHandler<ArrowAnnotation>(AppearanceChanged);

            ArrowDirectionProperty.Changed.AddClassHandler<ArrowAnnotation>(DataChanged);
            ColorProperty.Changed.AddClassHandler<ArrowAnnotation>(AppearanceChanged);
            EndPointProperty.Changed.AddClassHandler<ArrowAnnotation>(DataChanged);
            HeadLengthProperty.Changed.AddClassHandler<ArrowAnnotation>(AppearanceChanged);
            HeadWidthProperty.Changed.AddClassHandler<ArrowAnnotation>(AppearanceChanged);
            LineJoinProperty.Changed.AddClassHandler<ArrowAnnotation>(AppearanceChanged);
            LineStyleProperty.Changed.AddClassHandler<ArrowAnnotation>(AppearanceChanged);
            StartPointProperty.Changed.AddClassHandler<ArrowAnnotation>(DataChanged);
            StrokeThicknessProperty.Changed.AddClassHandler<ArrowAnnotation>(AppearanceChanged);
            VeenessProperty.Changed.AddClassHandler<ArrowAnnotation>(AppearanceChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ArrowAnnotation" /> class.
        /// </summary>
        public ArrowAnnotation()
        {
            InternalAnnotation = new Annotations.ArrowAnnotation();
        }

        /// <summary>
        /// Gets or sets the arrow direction.
        /// </summary>
        public ScreenVector ArrowDirection
        {
            get
            {
                return GetValue(ArrowDirectionProperty);
            }

            set
            {
                SetValue(ArrowDirectionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public Color Color
        {
            get
            {
                return GetValue(ColorProperty);
            }

            set
            {
                SetValue(ColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        public DataPoint EndPoint
        {
            get
            {
                return GetValue(EndPointProperty);
            }

            set
            {
                SetValue(EndPointProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the length of the head (relative to the stroke thickness).
        /// </summary>
        /// <value>The length of the head.</value>
        public double HeadLength
        {
            get
            {
                return GetValue(HeadLengthProperty);
            }

            set
            {
                SetValue(HeadLengthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the head (relative to the stroke thickness).
        /// </summary>
        /// <value>The width of the head.</value>
        public double HeadWidth
        {
            get
            {
                return GetValue(HeadWidthProperty);
            }

            set
            {
                SetValue(HeadWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public LineJoin LineJoin
        {
            get
            {
                return GetValue(LineJoinProperty);
            }

            set
            {
                SetValue(LineJoinProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineStyle.
        /// </summary>
        public LineStyle LineStyle
        {
            get
            {
                return GetValue(LineStyleProperty);
            }

            set
            {
                SetValue(LineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        /// <remarks>This property is overridden by the ArrowDirection property, if set.</remarks>
        public DataPoint StartPoint
        {
            get
            {
                return GetValue(StartPointProperty);
            }

            set
            {
                SetValue(StartPointProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double StrokeThickness
        {
            get
            {
                return GetValue(StrokeThicknessProperty);
            }

            set
            {
                SetValue(StrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the 'veeness' of the arrow head (relative to thickness).
        /// </summary>
        public double Veeness
        {
            get
            {
                return GetValue(VeenessProperty);
            }

            set
            {
                SetValue(VeenessProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal annotation object.
        /// </summary>
        /// <returns>The annotation.</returns>
        public override Annotations.Annotation CreateModel()
        {
            SynchronizeProperties();
            return InternalAnnotation;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Annotations.ArrowAnnotation)InternalAnnotation;

            a.StartPoint = StartPoint;
            a.EndPoint = EndPoint;
            a.ArrowDirection = ArrowDirection;

            a.HeadLength = HeadLength;
            a.HeadWidth = HeadWidth;
            a.Veeness = Veeness;

            a.Color = Color.ToOxyColor();
            a.StrokeThickness = StrokeThickness;
            a.LineStyle = LineStyle;
            a.LineJoin = LineJoin;
        }
    }
}