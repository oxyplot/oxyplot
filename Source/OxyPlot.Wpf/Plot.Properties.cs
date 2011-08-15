namespace OxyPlot.Wpf
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Partial class for the WPF Plot control. 
    /// This file contains dependency properties used for defining the plot in XAML.
    /// These properties are only used when Model is null.
    /// In this case an internal PlotModel is created and the dependency properties are copied from the control to the 
    /// internal PlotModel.
    /// </summary>
    public partial class Plot
    {
        #region Constants and Fields

        public static readonly DependencyProperty AxisTickToLabelDistanceProperty =
            DependencyProperty.Register(
                "AxisTickToLabelDistance",
                typeof(double),
                typeof(Plot),
                new FrameworkPropertyMetadata(4.0, VisualChanged));

        public static readonly DependencyProperty AxisTitleDistanceProperty =
            DependencyProperty.Register(
                "AxisTitleDistance", typeof(double), typeof(Plot), new FrameworkPropertyMetadata(4.0, VisualChanged));

        /// <summary>
        /// The box color property.
        /// </summary>
        public static readonly DependencyProperty BoxColorProperty = DependencyProperty.Register(
            "BoxColor", typeof(Color), typeof(Plot), new FrameworkPropertyMetadata(Colors.Black, VisualChanged));

        /// <summary>
        /// The box thickness property.
        /// </summary>
        public static readonly DependencyProperty BoxThicknessProperty = DependencyProperty.Register(
            "BoxThickness", typeof(double), typeof(Plot), new FrameworkPropertyMetadata(1.0, VisualChanged));

        public static readonly DependencyProperty IsLegendVisibleProperty =
            DependencyProperty.Register(
                "IsLegendVisible", typeof(bool), typeof(Plot), new FrameworkPropertyMetadata(true, VisualChanged));

        public static readonly DependencyProperty LegendBackgroundProperty =
            DependencyProperty.Register(
                "LegendBackground",
                typeof(Color),
                typeof(Plot),
                new FrameworkPropertyMetadata(Color.FromArgb(220, 255, 255, 255), VisualChanged));

        public static readonly DependencyProperty LegendBorderProperty = DependencyProperty.Register(
            "LegendBorder", typeof(Color), typeof(Plot), new FrameworkPropertyMetadata(Colors.Black, VisualChanged));

        public static readonly DependencyProperty LegendBorderThicknessProperty =
            DependencyProperty.Register(
                "LegendBorderThickness", typeof(double), typeof(Plot), new FrameworkPropertyMetadata(1.0, VisualChanged));

        public static readonly DependencyProperty LegendFontProperty = DependencyProperty.Register(
            "LegendFont", typeof(string), typeof(Plot), new FrameworkPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty LegendFontSizeProperty = DependencyProperty.Register(
            "LegendFontSize", typeof(double), typeof(Plot), new FrameworkPropertyMetadata(12.0, VisualChanged));

        public static readonly DependencyProperty LegendFontWeightProperty =
            DependencyProperty.Register(
                "LegendFontWeight",
                typeof(FontWeight),
                typeof(Plot),
                new FrameworkPropertyMetadata(FontWeights.Normal, VisualChanged));

        public static readonly DependencyProperty LegendItemAlignmentProperty =
            DependencyProperty.Register(
                "LegendItemAlignment",
                typeof(HorizontalAlignment),
                typeof(Plot),
                new FrameworkPropertyMetadata(HorizontalAlignment.Left, VisualChanged));

        public static readonly DependencyProperty LegendItemOrderProperty =
            DependencyProperty.Register(
                "LegendItemOrder",
                typeof(LegendItemOrder),
                typeof(Plot),
                new FrameworkPropertyMetadata(LegendItemOrder.Normal, VisualChanged));

        public static readonly DependencyProperty LegendItemSpacingProperty =
            DependencyProperty.Register(
                "LegendItemSpacing", typeof(double), typeof(Plot), new FrameworkPropertyMetadata(24.0, VisualChanged));

        public static readonly DependencyProperty LegendMarginProperty = DependencyProperty.Register(
            "LegendMargin", typeof(double), typeof(Plot), new FrameworkPropertyMetadata(8.0, VisualChanged));

        public static readonly DependencyProperty LegendOrientationProperty =
            DependencyProperty.Register(
                "LegendOrientation",
                typeof(LegendOrientation),
                typeof(Plot),
                new FrameworkPropertyMetadata(LegendOrientation.Vertical, VisualChanged));

        public static readonly DependencyProperty LegendPaddingProperty = DependencyProperty.Register(
            "LegendPadding", typeof(double), typeof(Plot), new FrameworkPropertyMetadata(8.0, VisualChanged));

        /// <summary>
        /// The legend placement property.
        /// </summary>
        public static readonly DependencyProperty LegendPlacementProperty =
            DependencyProperty.Register(
                "LegendPlacement",
                typeof(LegendPlacement),
                typeof(Plot),
                new FrameworkPropertyMetadata(LegendPlacement.Inside, VisualChanged));

        /// <summary>
        /// The legend position property.
        /// </summary>
        public static readonly DependencyProperty LegendPositionProperty = DependencyProperty.Register(
            "LegendPosition",
            typeof(LegendPosition),
            typeof(Plot),
            new FrameworkPropertyMetadata(LegendPosition.RightTop, VisualChanged));

        public static readonly DependencyProperty LegendSymbolLengthProperty =
            DependencyProperty.Register(
                "LegendSymbolLength", typeof(double), typeof(Plot), new FrameworkPropertyMetadata(16.0, VisualChanged));

        public static readonly DependencyProperty LegendSymbolMarginProperty =
            DependencyProperty.Register(
                "LegendSymbolMargin", typeof(double), typeof(Plot), new FrameworkPropertyMetadata(4.0, VisualChanged));

        public static readonly DependencyProperty LegendSymbolPlacementProperty =
            DependencyProperty.Register(
                "LegendSymbolPlacement",
                typeof(LegendSymbolPlacement),
                typeof(Plot),
                new FrameworkPropertyMetadata(LegendSymbolPlacement.Left, VisualChanged));

        public static readonly DependencyProperty LegendTitleFontProperty =
            DependencyProperty.Register(
                "LegendTitleFont", typeof(string), typeof(Plot), new FrameworkPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty LegendTitleFontSizeProperty =
            DependencyProperty.Register(
                "LegendTitleFontSize", typeof(double), typeof(Plot), new FrameworkPropertyMetadata(12.0, VisualChanged));

        public static readonly DependencyProperty LegendTitleFontWeightProperty =
            DependencyProperty.Register(
                "LegendTitleFontWeight",
                typeof(FontWeight),
                typeof(Plot),
                new FrameworkPropertyMetadata(FontWeights.Bold, VisualChanged));

        /// <summary>
        /// The plot margins property.
        /// </summary>
        public static readonly DependencyProperty PlotMarginsProperty = DependencyProperty.Register(
            "PlotMargins",
            typeof(Thickness),
            typeof(Plot),
            new FrameworkPropertyMetadata(new Thickness(60, 4, 4, 40), VisualChanged));

        public static readonly DependencyProperty PlotTypeProperty = DependencyProperty.Register(
            "PlotType", typeof(PlotType), typeof(Plot), new FrameworkPropertyMetadata(PlotType.XY, VisualChanged));

        public static readonly DependencyProperty SubtitleFontSizeProperty =
            DependencyProperty.Register(
                "SubtitleFontSize", typeof(double), typeof(Plot), new FrameworkPropertyMetadata(14.0, VisualChanged));

        public static readonly DependencyProperty SubtitleFontWeightProperty =
            DependencyProperty.Register(
                "SubtitleFontWeight",
                typeof(FontWeight),
                typeof(Plot),
                new FrameworkPropertyMetadata(FontWeights.Normal, VisualChanged));

        /// <summary>
        /// The subtitle property.
        /// </summary>
        public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
            "Subtitle", typeof(string), typeof(Plot), new FrameworkPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            "TextColor", typeof(Color), typeof(Plot), new FrameworkPropertyMetadata(Colors.Black, VisualChanged));

        public static readonly DependencyProperty TitleFontProperty = DependencyProperty.Register(
            "TitleFont", typeof(string), typeof(Plot), new FrameworkPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(
            "TitleFontSize", typeof(double), typeof(Plot), new FrameworkPropertyMetadata(18.0, VisualChanged));

        public static readonly DependencyProperty TitleFontWeightProperty =
            DependencyProperty.Register(
                "TitleFontWeight", typeof(FontWeight), typeof(Plot), new FrameworkPropertyMetadata(FontWeights.Bold));

        public static readonly DependencyProperty TitlePaddingProperty = DependencyProperty.Register(
            "TitlePadding", typeof(double), typeof(Plot), new FrameworkPropertyMetadata(6.0, VisualChanged));

        /// <summary>
        /// The title property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(Plot), new FrameworkPropertyMetadata(null, VisualChanged));

        /// <summary>
        /// The annotations.
        /// </summary>
        private readonly ObservableCollection<IAnnotation> annotations;

        /// <summary>
        /// The axes.
        /// </summary>
        private readonly ObservableCollection<IAxis> axes;

        /// <summary>
        /// The series.
        /// </summary>
        private readonly ObservableCollection<ISeries> series;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the annotations.
        /// </summary>
        /// <value>The annotations.</value>
        public ObservableCollection<IAnnotation> Annotations
        {
            get
            {
                return this.annotations;
            }
        }

        /// <summary>
        /// Gets the axes.
        /// </summary>
        /// <value>The axes.</value>
        public ObservableCollection<IAxis> Axes
        {
            get
            {
                return this.axes;
            }
        }

        public double AxisTickToLabelDistance
        {
            get
            {
                return (double)this.GetValue(AxisTickToLabelDistanceProperty);
            }
            set
            {
                this.SetValue(AxisTickToLabelDistanceProperty, value);
            }
        }

        public double AxisTitleDistance
        {
            get
            {
                return (double)this.GetValue(AxisTitleDistanceProperty);
            }
            set
            {
                this.SetValue(AxisTitleDistanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the box.
        /// </summary>
        /// <value>The color of the box.</value>
        public Color BoxColor
        {
            get
            {
                return (Color)this.GetValue(BoxColorProperty);
            }

            set
            {
                this.SetValue(BoxColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the box thickness.
        /// </summary>
        /// <value>The box thickness.</value>
        public double BoxThickness
        {
            get
            {
                return (double)this.GetValue(BoxThicknessProperty);
            }

            set
            {
                this.SetValue(BoxThicknessProperty, value);
            }
        }

        public bool IsLegendVisible
        {
            get
            {
                return (bool)this.GetValue(IsLegendVisibleProperty);
            }
            set
            {
                this.SetValue(IsLegendVisibleProperty, value);
            }
        }

        public Color LegendBackground
        {
            get
            {
                return (Color)this.GetValue(LegendBackgroundProperty);
            }
            set
            {
                this.SetValue(LegendBackgroundProperty, value);
            }
        }

        public Color LegendBorder
        {
            get
            {
                return (Color)this.GetValue(LegendBorderProperty);
            }
            set
            {
                this.SetValue(LegendBorderProperty, value);
            }
        }

        public double LegendBorderThickness
        {
            get
            {
                return (double)this.GetValue(LegendBorderThicknessProperty);
            }
            set
            {
                this.SetValue(LegendBorderThicknessProperty, value);
            }
        }

        public string LegendFont
        {
            get
            {
                return (string)this.GetValue(LegendFontProperty);
            }
            set
            {
                this.SetValue(LegendFontProperty, value);
            }
        }

        public double LegendFontSize
        {
            get
            {
                return (double)this.GetValue(LegendFontSizeProperty);
            }
            set
            {
                this.SetValue(LegendFontSizeProperty, value);
            }
        }

        public FontWeight LegendFontWeight
        {
            get
            {
                return (FontWeight)this.GetValue(LegendFontWeightProperty);
            }
            set
            {
                this.SetValue(LegendFontWeightProperty, value);
            }
        }

        public HorizontalAlignment LegendItemAlignment
        {
            get
            {
                return (HorizontalAlignment)this.GetValue(LegendItemAlignmentProperty);
            }
            set
            {
                this.SetValue(LegendItemAlignmentProperty, value);
            }
        }

        public LegendItemOrder LegendItemOrder
        {
            get
            {
                return (LegendItemOrder)this.GetValue(LegendItemOrderProperty);
            }
            set
            {
                this.SetValue(LegendItemOrderProperty, value);
            }
        }

        public double LegendItemSpacing
        {
            get
            {
                return (double)this.GetValue(LegendItemSpacingProperty);
            }
            set
            {
                this.SetValue(LegendItemSpacingProperty, value);
            }
        }

        public double LegendMargin
        {
            get
            {
                return (double)this.GetValue(LegendMarginProperty);
            }
            set
            {
                this.SetValue(LegendMarginProperty, value);
            }
        }

        public LegendOrientation LegendOrientation
        {
            get
            {
                return (LegendOrientation)this.GetValue(LegendOrientationProperty);
            }
            set
            {
                this.SetValue(LegendOrientationProperty, value);
            }
        }

        public double LegendPadding
        {
            get
            {
                return (double)this.GetValue(LegendPaddingProperty);
            }
            set
            {
                this.SetValue(LegendPaddingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendPlacement.
        /// </summary>
        public LegendPlacement LegendPlacement
        {
            get
            {
                return (LegendPlacement)this.GetValue(LegendPlacementProperty);
            }

            set
            {
                this.SetValue(LegendPlacementProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the legend position.
        /// </summary>
        /// <value>The legend position.</value>
        public LegendPosition LegendPosition
        {
            get
            {
                return (LegendPosition)this.GetValue(LegendPositionProperty);
            }

            set
            {
                this.SetValue(LegendPositionProperty, value);
            }
        }

        public double LegendSymbolLength
        {
            get
            {
                return (double)this.GetValue(LegendSymbolLengthProperty);
            }
            set
            {
                this.SetValue(LegendSymbolLengthProperty, value);
            }
        }

        public double LegendSymbolMargin
        {
            get
            {
                return (double)this.GetValue(LegendSymbolMarginProperty);
            }
            set
            {
                this.SetValue(LegendSymbolMarginProperty, value);
            }
        }

        public LegendSymbolPlacement LegendSymbolPlacement
        {
            get
            {
                return (LegendSymbolPlacement)this.GetValue(LegendSymbolPlacementProperty);
            }
            set
            {
                this.SetValue(LegendSymbolPlacementProperty, value);
            }
        }

        public string LegendTitleFont
        {
            get
            {
                return (string)this.GetValue(LegendTitleFontProperty);
            }
            set
            {
                this.SetValue(LegendTitleFontProperty, value);
            }
        }

        public double LegendTitleFontSize
        {
            get
            {
                return (double)this.GetValue(LegendTitleFontSizeProperty);
            }
            set
            {
                this.SetValue(LegendTitleFontSizeProperty, value);
            }
        }

        public FontWeight LegendTitleFontWeight
        {
            get
            {
                return (FontWeight)this.GetValue(LegendTitleFontWeightProperty);
            }
            set
            {
                this.SetValue(LegendTitleFontWeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the plot margins.
        /// </summary>
        /// <value>The plot margins.</value>
        public Thickness PlotMargins
        {
            get
            {
                return (Thickness)this.GetValue(PlotMarginsProperty);
            }

            set
            {
                this.SetValue(PlotMarginsProperty, value);
            }
        }

        public PlotType PlotType
        {
            get
            {
                return (PlotType)this.GetValue(PlotTypeProperty);
            }
            set
            {
                this.SetValue(PlotTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets the series.
        /// </summary>
        /// <value>The series.</value>
        public ObservableCollection<ISeries> Series
        {
            get
            {
                return this.series;
            }
        }

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        /// <value>The subtitle.</value>
        public string Subtitle
        {
            get
            {
                return (string)this.GetValue(SubtitleProperty);
            }

            set
            {
                this.SetValue(SubtitleProperty, value);
            }
        }

        public double SubtitleFontSize
        {
            get
            {
                return (double)this.GetValue(SubtitleFontSizeProperty);
            }
            set
            {
                this.SetValue(SubtitleFontSizeProperty, value);
            }
        }

        public FontWeight SubtitleFontWeight
        {
            get
            {
                return (FontWeight)this.GetValue(SubtitleFontWeightProperty);
            }
            set
            {
                this.SetValue(SubtitleFontWeightProperty, value);
            }
        }

        public Color TextColor
        {
            get
            {
                return (Color)this.GetValue(TextColorProperty);
            }
            set
            {
                this.SetValue(TextColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return (string)this.GetValue(TitleProperty);
            }

            set
            {
                this.SetValue(TitleProperty, value);
            }
        }

        public string TitleFont
        {
            get
            {
                return (string)this.GetValue(TitleFontProperty);
            }
            set
            {
                this.SetValue(TitleFontProperty, value);
            }
        }

        public double TitleFontSize
        {
            get
            {
                return (double)this.GetValue(TitleFontSizeProperty);
            }
            set
            {
                this.SetValue(TitleFontSizeProperty, value);
            }
        }

        public FontWeight TitleFontWeight
        {
            get
            {
                return (FontWeight)this.GetValue(TitleFontWeightProperty);
            }
            set
            {
                this.SetValue(TitleFontWeightProperty, value);
            }
        }

        public double TitlePadding
        {
            get
            {
                return (double)this.GetValue(TitlePaddingProperty);
            }
            set
            {
                this.SetValue(TitlePaddingProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Synchronize properties between the WPF control and the internal PlotModel (only if Model is undefined).
        /// </summary>
        private void SynchronizeProperties()
        {
            if (this.internalModel != null)
            {
                PlotModel m = this.internalModel;
                m.Title = this.Title;
                m.Subtitle = this.Subtitle;
                m.PlotType = this.PlotType;
                m.AxisTitleDistance = this.AxisTitleDistance;
                m.AxisTickToLabelDistance = this.AxisTickToLabelDistance;
                m.PlotMargins = this.PlotMargins.ToOxyThickness();
                m.Padding = this.Padding.ToOxyThickness();
                m.TitleFont = this.TitleFont;
                m.TitleFontSize = this.TitleFontSize;
                m.SubtitleFontSize = this.SubtitleFontSize;
                m.TitleFontWeight = this.TitleFontWeight.ToOpenTypeWeight();
                m.SubtitleFontWeight = this.SubtitleFontWeight.ToOpenTypeWeight();
                m.TitlePadding = this.TitlePadding;
                m.TextColor = this.TextColor.ToOxyColor();

                m.IsLegendVisible = this.IsLegendVisible;
                m.LegendTitleFont = this.LegendTitleFont;
                m.LegendTitleFontSize = this.LegendTitleFontSize;
                m.LegendTitleFontWeight = this.LegendTitleFontWeight.ToOpenTypeWeight();
                m.LegendFont = this.LegendFont;
                m.LegendFontSize = this.LegendFontSize;
                m.LegendFontWeight = this.LegendFontWeight.ToOpenTypeWeight();
                m.LegendSymbolLength = this.LegendSymbolLength;
                m.LegendSymbolMargin = this.LegendSymbolMargin;
                m.LegendPadding = this.LegendPadding;
                m.LegendItemSpacing = this.LegendItemSpacing;
                m.LegendMargin = this.LegendMargin;

                m.LegendBackground = this.LegendBackground.ToOxyColor();
                m.LegendBorder = this.LegendBorder.ToOxyColor();
                m.LegendBorderThickness = this.LegendBorderThickness;

                m.LegendPlacement = this.LegendPlacement;
                m.LegendPosition = this.LegendPosition;
                m.LegendOrientation = this.LegendOrientation;
                m.LegendItemOrder = this.LegendItemOrder;
                m.LegendItemAlignment = this.LegendItemAlignment.ToHorizontalTextAlign();
                m.LegendSymbolPlacement = this.LegendSymbolPlacement;

                m.BoxColor = this.BoxColor.ToOxyColor();
                m.BoxThickness = this.BoxThickness;
            }
        }

        #endregion
    }
}