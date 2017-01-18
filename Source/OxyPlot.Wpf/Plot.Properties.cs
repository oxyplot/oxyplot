// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Plot.Properties.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    /// <remarks>This file contains dependency properties used for defining the Plot in XAML. These properties are only used when Model is <c>null</c>. In this case an internal PlotModel is created and the dependency properties are copied from the control to the internal PlotModel.</remarks>
    public partial class Plot
    {
        /// <summary>
        /// Identifies the <see cref="Culture"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(
            "Culture", typeof(CultureInfo), typeof(Plot), new UIPropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="IsLegendVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLegendVisibleProperty =
            DependencyProperty.Register(
                "IsLegendVisible", typeof(bool), typeof(Plot), new PropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendBackgroundProperty =
            DependencyProperty.Register(
                "LegendBackground", typeof(Color), typeof(Plot), new PropertyMetadata(MoreColors.Undefined, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendBorder"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendBorderProperty = DependencyProperty.Register(
            "LegendBorder", typeof(Color), typeof(Plot), new PropertyMetadata(MoreColors.Undefined, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendBorderThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendBorderThicknessProperty =
            DependencyProperty.Register(
                "LegendBorderThickness", typeof(double), typeof(Plot), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendFont"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendFontProperty = DependencyProperty.Register(
            "LegendFont", typeof(string), typeof(Plot), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendFontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendFontSizeProperty = DependencyProperty.Register(
            "LegendFontSize", typeof(double), typeof(Plot), new PropertyMetadata(12.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendFontWeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendFontWeightProperty =
            DependencyProperty.Register(
                "LegendFontWeight",
                typeof(FontWeight),
                typeof(Plot),
                new PropertyMetadata(FontWeights.Normal, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendItemAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendItemAlignmentProperty =
            DependencyProperty.Register(
                "LegendItemAlignment",
                typeof(HorizontalAlignment),
                typeof(Plot),
                new PropertyMetadata(HorizontalAlignment.Left, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendItemOrder"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendItemOrderProperty =
            DependencyProperty.Register(
                "LegendItemOrder",
                typeof(LegendItemOrder),
                typeof(Plot),
                new PropertyMetadata(LegendItemOrder.Normal, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendItemSpacing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendItemSpacingProperty =
            DependencyProperty.Register(
                "LegendItemSpacing", typeof(double), typeof(Plot), new PropertyMetadata(24.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendLineSpacing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendLineSpacingProperty =
            DependencyProperty.Register(
                "LegendLineSpacing", typeof(double), typeof(Plot), new PropertyMetadata(0d, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendMarginProperty = DependencyProperty.Register(
            "LegendMargin", typeof(double), typeof(Plot), new PropertyMetadata(8.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendMaxHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendMaxHeightProperty =
            DependencyProperty.Register("LegendMaxHeight", typeof(double), typeof(Plot), new UIPropertyMetadata(double.NaN, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendMaxWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendMaxWidthProperty =
            DependencyProperty.Register("LegendMaxWidth", typeof(double), typeof(Plot), new UIPropertyMetadata(double.NaN, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendOrientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendOrientationProperty =
            DependencyProperty.Register(
                "LegendOrientation",
                typeof(LegendOrientation),
                typeof(Plot),
                new PropertyMetadata(LegendOrientation.Vertical, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendColumnSpacing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendColumnSpacingProperty =
            DependencyProperty.Register("LegendColumnSpacing", typeof(double), typeof(Plot), new UIPropertyMetadata(8.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendPaddingProperty = DependencyProperty.Register(
            "LegendPadding", typeof(double), typeof(Plot), new PropertyMetadata(8.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendPlacement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendPlacementProperty =
            DependencyProperty.Register(
                "LegendPlacement",
                typeof(LegendPlacement),
                typeof(Plot),
                new PropertyMetadata(LegendPlacement.Inside, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendPosition"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendPositionProperty = DependencyProperty.Register(
            "LegendPosition",
            typeof(LegendPosition),
            typeof(Plot),
            new PropertyMetadata(LegendPosition.RightTop, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendSymbolLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendSymbolLengthProperty =
            DependencyProperty.Register(
                "LegendSymbolLength", typeof(double), typeof(Plot), new PropertyMetadata(16.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendSymbolMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendSymbolMarginProperty =
            DependencyProperty.Register(
                "LegendSymbolMargin", typeof(double), typeof(Plot), new PropertyMetadata(4.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendSymbolPlacement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendSymbolPlacementProperty =
            DependencyProperty.Register(
                "LegendSymbolPlacement",
                typeof(LegendSymbolPlacement),
                typeof(Plot),
                new PropertyMetadata(LegendSymbolPlacement.Left, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="SelectionColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectionColorProperty = DependencyProperty.Register(
            "SelectionColor", typeof(Color), typeof(Plot), new PropertyMetadata(Colors.Yellow, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="RenderingDecorator"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RenderingDecoratorProperty = DependencyProperty.Register(
            "RenderingDecorator", typeof(Func<IRenderContext, IRenderContext>), typeof(Plot), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="SubtitleFont"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubtitleFontProperty = DependencyProperty.Register(
            "SubtitleFont", typeof(string), typeof(Plot), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitleColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleColorProperty = DependencyProperty.Register(
            "TitleColor", typeof(Color), typeof(Plot), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="SubtitleColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubtitleColorProperty = DependencyProperty.Register(
            "SubtitleColor", typeof(Color), typeof(Plot), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="DefaultFont"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultFontProperty = DependencyProperty.Register(
            "DefaultFont", typeof(string), typeof(Plot), new PropertyMetadata("Segoe UI", AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="DefaultFontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultFontSizeProperty = DependencyProperty.Register(
            "DefaultFontSize", typeof(double), typeof(Plot), new PropertyMetadata(12d, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="DefaultColors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultColorsProperty = DependencyProperty.Register(
            "DefaultColors",
            typeof(IList<Color>),
            typeof(Plot),
            new PropertyMetadata(
                new[]
            {
                Color.FromRgb(0x4E, 0x9A, 0x06),
                    Color.FromRgb(0xC8, 0x8D, 0x00),
                    Color.FromRgb(0xCC, 0x00, 0x00),
                    Color.FromRgb(0x20, 0x4A, 0x87),
                    Colors.Red,
                    Colors.Orange,
                    Colors.Yellow,
                    Colors.Green,
                    Colors.Blue,
                    Colors.Indigo,
                    Colors.Violet
            },
                    AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="AxisTierDistance"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AxisTierDistanceProperty = DependencyProperty.Register(
            "AxisTierDistance", typeof(double), typeof(Plot), new PropertyMetadata(4d, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendTextColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendTextColorProperty = DependencyProperty.Register(
            "LegendTextColor", typeof(Color), typeof(Plot), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendTitleProperty = DependencyProperty.Register(
            "LegendTitle", typeof(string), typeof(Plot), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendTextColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendTitleColorProperty = DependencyProperty.Register(
            "LegendTitleColor", typeof(Color), typeof(Plot), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendTitleFont"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendTitleFontProperty =
            DependencyProperty.Register(
                "LegendTitleFont", typeof(string), typeof(Plot), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendTitleFontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendTitleFontSizeProperty =
            DependencyProperty.Register(
                "LegendTitleFontSize", typeof(double), typeof(Plot), new PropertyMetadata(12.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LegendTitleFontWeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendTitleFontWeightProperty =
            DependencyProperty.Register(
                "LegendTitleFontWeight",
                typeof(FontWeight),
                typeof(Plot),
                new PropertyMetadata(FontWeights.Bold, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="PlotAreaBackground"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlotAreaBackgroundProperty =
            DependencyProperty.Register(
                "PlotAreaBackground",
                typeof(Brush),
                typeof(Plot),
                new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="PlotAreaBorderColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlotAreaBorderColorProperty =
            DependencyProperty.Register(
                "PlotAreaBorderColor",
                typeof(Color),
                typeof(Plot),
                new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="PlotAreaBorderThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlotAreaBorderThicknessProperty =
            DependencyProperty.Register(
                "PlotAreaBorderThickness", typeof(Thickness), typeof(Plot), new PropertyMetadata(new Thickness(1.0), AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="PlotMargins"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlotMarginsProperty = DependencyProperty.Register(
            "PlotMargins",
            typeof(Thickness),
            typeof(Plot),
            new PropertyMetadata(new Thickness(double.NaN), AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="PlotType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlotTypeProperty = DependencyProperty.Register(
            "PlotType", typeof(PlotType), typeof(Plot), new PropertyMetadata(PlotType.XY, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="SubtitleFontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubtitleFontSizeProperty =
            DependencyProperty.Register(
                "SubtitleFontSize", typeof(double), typeof(Plot), new PropertyMetadata(14.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="SubtitleFontWeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubtitleFontWeightProperty =
            DependencyProperty.Register(
                "SubtitleFontWeight",
                typeof(FontWeight),
                typeof(Plot),
                new PropertyMetadata(FontWeights.Normal, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Subtitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
            "Subtitle", typeof(string), typeof(Plot), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TextColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            "TextColor", typeof(Color), typeof(Plot), new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitleHorizontalAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleAlignmentProperty =
            DependencyProperty.Register("TitleHorizontalAlignment", typeof(TitleHorizontalAlignment), typeof(Plot), new PropertyMetadata(TitleHorizontalAlignment.CenteredWithinPlotArea, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitleFont"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleFontProperty = DependencyProperty.Register(
            "TitleFont", typeof(string), typeof(Plot), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitleFontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(
            "TitleFontSize", typeof(double), typeof(Plot), new PropertyMetadata(18.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitleFontWeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleFontWeightProperty =
            DependencyProperty.Register(
                "TitleFontWeight",
                typeof(FontWeight),
                typeof(Plot),
                new PropertyMetadata(FontWeights.Bold, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitlePadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitlePaddingProperty = DependencyProperty.Register(
            "TitlePadding", typeof(double), typeof(Plot), new PropertyMetadata(6.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Title"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(Plot), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TitleToolTip"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleToolTipProperty = DependencyProperty.Register(
            "TitleToolTip", typeof(string), typeof(Plot), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="InvalidateFlag"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InvalidateFlagProperty = DependencyProperty.Register(
            "InvalidateFlag",
            typeof(int),
            typeof(Plot),
            new FrameworkPropertyMetadata(0, (s, e) => ((Plot)s).InvalidateFlagChanged()));

        /// <summary>
        /// The annotations.
        /// </summary>
        private readonly ObservableCollection<Annotation> annotations;

        /// <summary>
        /// The axes.
        /// </summary>
        private readonly ObservableCollection<Axis> axes;

        /// <summary>
        /// The series.
        /// </summary>
        private readonly ObservableCollection<Series> series;

        /// <summary>
        /// Gets the axes.
        /// </summary>
        /// <value>The axes.</value>
        public Collection<Axis> Axes
        {
            get
            {
                return this.axes;
            }
        }

        /// <summary>
        /// Gets or sets Culture.
        /// </summary>
        public CultureInfo Culture
        {
            get
            {
                return (CultureInfo)this.GetValue(CultureProperty);
            }

            set
            {
                this.SetValue(CultureProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsLegendVisible.
        /// </summary>
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

        /// <summary>
        /// Gets or sets LegendBackground.
        /// </summary>
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

        /// <summary>
        /// Gets or sets LegendBorder.
        /// </summary>
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

        /// <summary>
        /// Gets or sets LegendBorderThickness.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the spacing between columns of legend items (only for vertical orientation).
        /// </summary>
        /// <value>The spacing in device independent units.</value>
        public double LegendColumnSpacing
        {
            get
            {
                return (double)this.GetValue(LegendColumnSpacingProperty);
            }

            set
            {
                this.SetValue(LegendColumnSpacingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendFont.
        /// </summary>
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

        /// <summary>
        /// Gets or sets LegendFontSize.
        /// </summary>
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

        /// <summary>
        /// Gets or sets LegendFontWeight.
        /// </summary>
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

        /// <summary>
        /// Gets or sets LegendItemAlignment.
        /// </summary>
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

        /// <summary>
        /// Gets or sets LegendItemOrder.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the horizontal spacing between legend items when the orientation is horizontal.
        /// </summary>
        /// <value>The horizontal distance between items in device independent units.</value>
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

        /// <summary>
        /// Gets or sets the vertical spacing between legend items.
        /// </summary>
        /// <value>The spacing in device independent units.</value>
        public double LegendLineSpacing
        {
            get
            {
                return (double)this.GetValue(LegendLineSpacingProperty);
            }

            set
            {
                this.SetValue(LegendLineSpacingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the max height of the legend.
        /// </summary>
        /// <value>The max width of the legends.</value>
        public double LegendMaxHeight
        {
            get
            {
                return (double)this.GetValue(LegendMaxHeightProperty);
            }

            set
            {
                this.SetValue(LegendMaxHeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the max width of the legend.
        /// </summary>
        /// <value>The max width of the legends.</value>
        public double LegendMaxWidth
        {
            get
            {
                return (double)this.GetValue(LegendMaxWidthProperty);
            }

            set
            {
                this.SetValue(LegendMaxWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendMargin.
        /// </summary>
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

        /// <summary>
        /// Gets or sets LegendOrientation.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the legend padding.
        /// </summary>
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

        /// <summary>
        /// Gets or sets LegendSymbolLength.
        /// </summary>
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

        /// <summary>
        /// Gets or sets LegendSymbolMargin.
        /// </summary>
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

        /// <summary>
        /// Gets or sets LegendSymbolPlacement.
        /// </summary>
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

        /// <summary>
        /// Gets or sets LegendTitleFont.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the default font.
        /// </summary>
        public string DefaultFont
        {
            get
            {
                return (string)this.GetValue(DefaultFontProperty);
            }

            set
            {
                this.SetValue(DefaultFontProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the default font size.
        /// </summary>
        public double DefaultFontSize
        {
            get
            {
                return (double)this.GetValue(DefaultFontSizeProperty);
            }

            set
            {
                this.SetValue(DefaultFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the default colors.
        /// </summary>
        public IList<Color> DefaultColors
        {
            get
            {
                return (IList<Color>)this.GetValue(DefaultColorsProperty);
            }

            set
            {
                this.SetValue(DefaultColorsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text color of the legends.
        /// </summary>
        public Color LegendTextColor
        {
            get
            {
                return (Color)this.GetValue(LegendTextColorProperty);
            }

            set
            {
                this.SetValue(LegendTextColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the legend title.
        /// </summary>
        public string LegendTitle
        {
            get
            {
                return (string)this.GetValue(LegendTitleProperty);
            }

            set
            {
                this.SetValue(LegendTitleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets color of the legend title.
        /// </summary>
        public Color LegendTitleColor
        {
            get
            {
                return (Color)this.GetValue(LegendTitleColorProperty);
            }

            set
            {
                this.SetValue(LegendTitleColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the axis tier distance.
        /// </summary>
        public double AxisTierDistance
        {
            get
            {
                return (double)this.GetValue(AxisTierDistanceProperty);
            }

            set
            {
                this.SetValue(AxisTierDistanceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of selected elements.
        /// </summary>
        public Color SelectionColor
        {
            get
            {
                return (Color)this.GetValue(SelectionColorProperty);
            }

            set
            {
                this.SetValue(SelectionColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a rendering decorator.
        /// </summary>
        public Func<IRenderContext, IRenderContext> RenderingDecorator
        {
            get
            {
                return (Func<IRenderContext, IRenderContext>)this.GetValue(RenderingDecoratorProperty);
            }

            set
            {
                this.SetValue(RenderingDecoratorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the font of the subtitles.
        /// </summary>
        public string SubtitleFont
        {
            get
            {
                return (string)this.GetValue(SubtitleFontProperty);
            }

            set
            {
                this.SetValue(SubtitleFontProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the titles.
        /// </summary>
        public Color TitleColor
        {
            get
            {
                return (Color)this.GetValue(TitleColorProperty);
            }

            set
            {
                this.SetValue(TitleColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the font size of the legend titles.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the color of the subtitles.
        /// </summary>
        public Color SubtitleColor
        {
            get
            {
                return (Color)this.GetValue(SubtitleColorProperty);
            }

            set
            {
                this.SetValue(SubtitleColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the font weight of the legend titles.
        /// </summary>
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
        /// Gets or sets the background brush of the Plot area.
        /// </summary>
        /// <value>The brush.</value>
        public Brush PlotAreaBackground
        {
            get
            {
                return (Brush)this.GetValue(PlotAreaBackgroundProperty);
            }

            set
            {
                this.SetValue(PlotAreaBackgroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the Plot area border.
        /// </summary>
        /// <value>The color of the Plot area border.</value>
        public Color PlotAreaBorderColor
        {
            get
            {
                return (Color)this.GetValue(PlotAreaBorderColorProperty);
            }

            set
            {
                this.SetValue(PlotAreaBorderColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the thickness of the Plot area border.
        /// </summary>
        /// <value>The thickness of the Plot area border.</value>
        public Thickness PlotAreaBorderThickness
        {
            get
            {
                return (Thickness)this.GetValue(PlotAreaBorderThicknessProperty);
            }

            set
            {
                this.SetValue(PlotAreaBorderThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Plot margins.
        /// </summary>
        /// <value>The Plot margins.</value>
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

        /// <summary>
        /// Gets or sets PlotType.
        /// </summary>
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
        public Collection<Series> Series
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

        /// <summary>
        /// Gets or sets the font size of the subtitle.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the font weight of the subtitle.
        /// </summary>
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

        /// <summary>
        /// Gets or sets text color.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the title tool tip.
        /// </summary>
        /// <value>The title tool tip.</value>
        public string TitleToolTip
        {
            get
            {
                return (string)this.GetValue(TitleToolTipProperty);
            }

            set
            {
                this.SetValue(TitleToolTipProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the title and subtitle.
        /// </summary>
        /// <value>
        /// The alignment.
        /// </value>
        public TitleHorizontalAlignment TitleHorizontalAlignment
        {
            get
            {
                return (TitleHorizontalAlignment)this.GetValue(TitleAlignmentProperty);
            }

            set
            {
                this.SetValue(TitleAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets font of the title.
        /// </summary>
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

        /// <summary>
        /// Gets or sets font size of the title.
        /// </summary>
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

        /// <summary>
        /// Gets or sets font weight of the title.
        /// </summary>
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

        /// <summary>
        /// Gets or sets padding around the title.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the refresh flag (an integer value). When the flag is changed, the Plot will be refreshed.
        /// </summary>
        /// <value>The refresh value.</value>
        public int InvalidateFlag
        {
            get
            {
                return (int)this.GetValue(InvalidateFlagProperty);
            }

            set
            {
                this.SetValue(InvalidateFlagProperty, value);
            }
        }

        /// <summary>
        /// Invalidates the Plot control/view when the <see cref="InvalidateFlag" /> property is changed.
        /// </summary>
        private void InvalidateFlagChanged()
        {
            this.InvalidatePlot();
        }
    }
}