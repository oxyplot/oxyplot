using System.Windows;

namespace OxyPlot.Wpf
{
    public abstract class Axis : FrameworkElement
    {
        public static readonly DependencyProperty StartPositionProperty =
            DependencyProperty.Register("StartPosition", typeof (double), typeof (Axis),
                                        new FrameworkPropertyMetadata(0.0,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty EndPositionProperty =
            DependencyProperty.Register("EndPosition", typeof (double), typeof (Axis),
                                        new FrameworkPropertyMetadata(1.0,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty PositionAtZeroCrossingProperty =
            DependencyProperty.Register("PositionAtZeroCrossing", typeof (bool), typeof (Axis),
                                        new FrameworkPropertyMetadata(false,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof (double), typeof (Axis),
                                        new FrameworkPropertyMetadata(0.0,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty TickStyleProperty =
            DependencyProperty.Register("TickStyle", typeof (TickStyle), typeof (Axis),
                                        new FrameworkPropertyMetadata(TickStyle.Inside,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty MajorGridlineStyleProperty =
            DependencyProperty.Register("MajorGridlineStyle", typeof (LineStyle), typeof (Axis),
                                        new FrameworkPropertyMetadata(LineStyle.None,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty MajorGridlineThicknessProperty =
            DependencyProperty.Register("MajorGridlineThickness", typeof (double), typeof (Axis),
                                        new FrameworkPropertyMetadata(1.0,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty MinorGridlineStyleProperty =
            DependencyProperty.Register("MinorGridlineStyle", typeof (LineStyle), typeof (Axis),
                                        new FrameworkPropertyMetadata(LineStyle.None,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty MinorGridlineThicknessProperty =
            DependencyProperty.Register("MinorGridlineThickness", typeof (double), typeof (Axis),
                                        new FrameworkPropertyMetadata(1.0,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof (AxisPosition), typeof (Axis),
                                        new FrameworkPropertyMetadata(AxisPosition.Left,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof (double), typeof (Axis),
                                        new FrameworkPropertyMetadata(double.NaN,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof (double), typeof (Axis),
                                        new FrameworkPropertyMetadata(double.NaN,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty MinimumPaddingProperty =
            DependencyProperty.Register("MinimumPadding", typeof (double), typeof (Axis), new UIPropertyMetadata(0.01));

        public static readonly DependencyProperty MaximumPaddingProperty =
            DependencyProperty.Register("MaximumPadding", typeof (double), typeof (Axis), new UIPropertyMetadata(0.01));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof (string), typeof (Axis),
                                        new FrameworkPropertyMetadata(null,
                                                                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty UseSuperExponentialFormatProperty =
            DependencyProperty.Register("UseSuperExponentialFormat", typeof (bool), typeof (Axis),
                                        new UIPropertyMetadata(false));

        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register("StringFormat", typeof (string), typeof (Axis), new UIPropertyMetadata(null));

        public AxisBase ModelAxis { get; protected set; }

        public double StartPosition
        {
            get { return (double) GetValue(StartPositionProperty); }
            set { SetValue(StartPositionProperty, value); }
        }

        public double EndPosition
        {
            get { return (double) GetValue(EndPositionProperty); }
            set { SetValue(EndPositionProperty, value); }
        }

        public bool PositionAtZeroCrossing
        {
            get { return (bool) GetValue(PositionAtZeroCrossingProperty); }
            set { SetValue(PositionAtZeroCrossingProperty, value); }
        }


        public double Angle
        {
            get { return (double) GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }


        public TickStyle TickStyle
        {
            get { return (TickStyle) GetValue(TickStyleProperty); }
            set { SetValue(TickStyleProperty, value); }
        }

        public LineStyle MajorGridlineStyle
        {
            get { return (LineStyle) GetValue(MajorGridlineStyleProperty); }
            set { SetValue(MajorGridlineStyleProperty, value); }
        }

        public double MajorGridlineThickness
        {
            get { return (double) GetValue(MajorGridlineThicknessProperty); }
            set { SetValue(MajorGridlineThicknessProperty, value); }
        }


        public LineStyle MinorGridlineStyle
        {
            get { return (LineStyle) GetValue(MinorGridlineStyleProperty); }
            set { SetValue(MinorGridlineStyleProperty, value); }
        }

        public double MinorGridlineThickness
        {
            get { return (double) GetValue(MinorGridlineThicknessProperty); }
            set { SetValue(MinorGridlineThicknessProperty, value); }
        }

        public AxisPosition Position
        {
            get { return (AxisPosition) GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }


        public double Minimum
        {
            get { return (double) GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Maximum
        {
            get { return (double) GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public double MinimumPadding
        {
            get { return (double) GetValue(MinimumPaddingProperty); }
            set { SetValue(MinimumPaddingProperty, value); }
        }


        public double MaximumPadding
        {
            get { return (double) GetValue(MaximumPaddingProperty); }
            set { SetValue(MaximumPaddingProperty, value); }
        }


        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public bool UseSuperExponentialFormat
        {
            get { return (bool) GetValue(UseSuperExponentialFormatProperty); }
            set { SetValue(UseSuperExponentialFormatProperty, value); }
        }

        public string StringFormat
        {
            get { return (string) GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.OwnerType == GetType())
            {
                var fpm = e.Property.GetMetadata(e.Property.OwnerType) as FrameworkPropertyMetadata;
                if (fpm != null && fpm.AffectsRender)
                {
                    var plot = Parent as Plot;
                    plot.InvalidatePlot();
                }
            }
        }

        public virtual void SynchronizeProperties()
        {
            AxisBase a = ModelAxis;
            a.Minimum = Minimum;
            a.Maximum = Maximum;
            a.Title = Title;
            a.Position = Position;
            a.PositionAtZeroCrossing = PositionAtZeroCrossing;
            a.TickStyle = TickStyle;
            a.MajorGridlineStyle = MajorGridlineStyle;
            a.MinorGridlineStyle = MinorGridlineStyle;
            //TicklineColor = TicklineColor;
            //MajorGridlineColor = MajorGridlineColor;
            //TicklineColor = TicklineColor;
            //MinorGridlineColor = MinorGridlineColor;
            a.MajorGridlineThickness = MajorGridlineThickness;
            a.MinorGridlineThickness = MinorGridlineThickness;

            a.MinimumPadding = MinimumPadding;
            a.MaximumPadding = MaximumPadding;

            //a.ExtraGridlineStyle = ExtraGridlineStyle;
            //a.ExtraGridlineColor = ExtraGridlineColor;
            //a.ExtraGridlineThickness = ExtraGridlineThickness;

            //a.ShowMinorTicks = ShowMinorTicks;

            //a.Font = Font;
            //a.FontSize = FontSize;

            //a.MinorTickSize = MinorTickSize;
            //a.MajorTickSize = MajorTickSize;

            a.StartPosition = StartPosition;
            a.EndPosition = EndPosition;
            a.StringFormat = StringFormat;
            a.UseSuperExponentialFormat = UseSuperExponentialFormat;

            a.Angle = Angle;
        }     
    }
}