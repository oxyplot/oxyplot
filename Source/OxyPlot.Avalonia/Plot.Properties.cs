// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Plot.Properties.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;
using Avalonia.Media;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Metadata;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    /// <remarks>This file contains dependency properties used for defining the Plot in XAML. These properties are only used when Model is <c>null</c>. In this case an internal PlotModel is created and the dependency properties are copied from the control to the internal PlotModel.</remarks>
    public partial class Plot
    {
        /// <summary>
        /// Identifies the <see cref="Culture"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<CultureInfo> CultureProperty = AvaloniaProperty.Register<Plot, CultureInfo>(nameof(Culture), null);

        /// <summary>
        /// Identifies the <see cref="IsLegendVisible"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> IsLegendVisibleProperty = AvaloniaProperty.Register<Plot, bool>(nameof(IsLegendVisible), true);

        /// <summary>
        /// Identifies the <see cref="LegendBackground"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> LegendBackgroundProperty = AvaloniaProperty.Register<Plot, Color>(nameof(LegendBackground), MoreColors.Undefined);

        /// <summary>
        /// Identifies the <see cref="LegendBorder"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> LegendBorderProperty = AvaloniaProperty.Register<Plot, Color>(nameof(LegendBorder), MoreColors.Undefined);

        /// <summary>
        /// Identifies the <see cref="LegendBorderThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LegendBorderThicknessProperty = AvaloniaProperty.Register<Plot, double>(nameof(LegendBorderThickness), 1.0);

        /// <summary>
        /// Identifies the <see cref="LegendFont"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> LegendFontProperty = AvaloniaProperty.Register<Plot, string>(nameof(LegendFont), null);

        /// <summary>
        /// Identifies the <see cref="LegendFontSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LegendFontSizeProperty = AvaloniaProperty.Register<Plot, double>(nameof(LegendFontSize), 12.0);

        /// <summary>
        /// Identifies the <see cref="LegendFontWeight"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<FontWeight> LegendFontWeightProperty = AvaloniaProperty.Register<Plot, FontWeight>(nameof(LegendFontWeight), FontWeight.Normal);

        /// <summary>
        /// Identifies the <see cref="LegendItemAlignment"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<global::Avalonia.Layout.HorizontalAlignment> LegendItemAlignmentProperty = AvaloniaProperty.Register<Plot, global::Avalonia.Layout.HorizontalAlignment>(nameof(LegendItemAlignment), global::Avalonia.Layout.HorizontalAlignment.Left);

        /// <summary>
        /// Identifies the <see cref="LegendItemOrder"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LegendItemOrder> LegendItemOrderProperty = AvaloniaProperty.Register<Plot, LegendItemOrder>(nameof(LegendItemOrder), LegendItemOrder.Normal);

        /// <summary>
        /// Identifies the <see cref="LegendItemSpacing"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LegendItemSpacingProperty = AvaloniaProperty.Register<Plot, double>(nameof(LegendItemSpacing), 24.0);

        /// <summary>
        /// Identifies the <see cref="LegendLineSpacing"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LegendLineSpacingProperty = AvaloniaProperty.Register<Plot, double>(nameof(LegendLineSpacing), 0d);

        /// <summary>
        /// Identifies the <see cref="LegendMargin"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LegendMarginProperty = AvaloniaProperty.Register<Plot, double>(nameof(LegendMargin), 8.0);

        /// <summary>
        /// Identifies the <see cref="LegendMaxHeight"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LegendMaxHeightProperty = AvaloniaProperty.Register<Plot, double>(nameof(LegendMaxHeight), double.NaN);

        /// <summary>
        /// Identifies the <see cref="LegendMaxWidth"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LegendMaxWidthProperty = AvaloniaProperty.Register<Plot, double>(nameof(LegendMaxWidth), double.NaN);

        /// <summary>
        /// Identifies the <see cref="LegendOrientation"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LegendOrientation> LegendOrientationProperty = AvaloniaProperty.Register<Plot, LegendOrientation>(nameof(LegendOrientation), LegendOrientation.Vertical);

        /// <summary>
        /// Identifies the <see cref="LegendColumnSpacing"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LegendColumnSpacingProperty = AvaloniaProperty.Register<Plot, double>(nameof(LegendColumnSpacing), 8.0);

        /// <summary>
        /// Identifies the <see cref="LegendPadding"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LegendPaddingProperty = AvaloniaProperty.Register<Plot, double>(nameof(LegendPadding), 8.0);

        /// <summary>
        /// Identifies the <see cref="LegendPlacement"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LegendPlacement> LegendPlacementProperty = AvaloniaProperty.Register<Plot, LegendPlacement>(nameof(LegendPlacement), LegendPlacement.Inside);

        /// <summary>
        /// Identifies the <see cref="LegendPosition"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LegendPosition> LegendPositionProperty = AvaloniaProperty.Register<Plot, LegendPosition>(nameof(LegendPosition), LegendPosition.RightTop);

        /// <summary>
        /// Identifies the <see cref="LegendSymbolLength"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LegendSymbolLengthProperty = AvaloniaProperty.Register<Plot, double>(nameof(LegendSymbolLength), 16.0);

        /// <summary>
        /// Identifies the <see cref="LegendSymbolMargin"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LegendSymbolMarginProperty = AvaloniaProperty.Register<Plot, double>(nameof(LegendSymbolMargin), 4.0);

        /// <summary>
        /// Identifies the <see cref="LegendSymbolPlacement"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LegendSymbolPlacement> LegendSymbolPlacementProperty = AvaloniaProperty.Register<Plot, LegendSymbolPlacement>(nameof(LegendSymbolPlacement), LegendSymbolPlacement.Left);

        /// <summary>
        /// Identifies the <see cref="SelectionColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> SelectionColorProperty = AvaloniaProperty.Register<Plot, Color>(nameof(SelectionColor), Colors.Yellow);

        /// <summary>
        /// Identifies the <see cref="RenderingDecorator"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Func<IRenderContext, IRenderContext>> RenderingDecoratorProperty = AvaloniaProperty.Register<Plot, Func<IRenderContext, IRenderContext>>(nameof(RenderingDecorator), null);

        /// <summary>
        /// Identifies the <see cref="SubtitleFont"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> SubtitleFontProperty = AvaloniaProperty.Register<Plot, string>(nameof(SubtitleFont), null);

        /// <summary>
        /// Identifies the <see cref="TitleColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> TitleColorProperty = AvaloniaProperty.Register<Plot, Color>(nameof(TitleColor), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="SubtitleColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> SubtitleColorProperty = AvaloniaProperty.Register<Plot, Color>(nameof(SubtitleColor), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="DefaultFont"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> DefaultFontProperty = AvaloniaProperty.Register<Plot, string>(nameof(DefaultFont), "Segoe UI");

        /// <summary>
        /// Identifies the <see cref="DefaultFontSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> DefaultFontSizeProperty = AvaloniaProperty.Register<Plot, double>(nameof(DefaultFontSize), 12d);

        /// <summary>
        /// Identifies the <see cref="DefaultColors"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<IList<Color>> DefaultColorsProperty = AvaloniaProperty.Register<Plot, IList<Color>>(nameof(DefaultColors), new[]
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
            });

        /// <summary>
        /// Identifies the <see cref="AxisTierDistance"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> AxisTierDistanceProperty = AvaloniaProperty.Register<Plot, double>(nameof(AxisTierDistance), 4d);

        /// <summary>
        /// Identifies the <see cref="LegendTextColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> LegendTextColorProperty = AvaloniaProperty.Register<Plot, Color>(nameof(LegendTextColor), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="LegendTitle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> LegendTitleProperty = AvaloniaProperty.Register<Plot, string>(nameof(LegendTitle), null);

        /// <summary>
        /// Identifies the <see cref="LegendTextColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> LegendTitleColorProperty = AvaloniaProperty.Register<Plot, Color>(nameof(LegendTitleColor), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="LegendTitleFont"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> LegendTitleFontProperty = AvaloniaProperty.Register<Plot, string>(nameof(LegendTitleFont), null);

        /// <summary>
        /// Identifies the <see cref="LegendTitleFontSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LegendTitleFontSizeProperty = AvaloniaProperty.Register<Plot, double>(nameof(LegendTitleFontSize), 12.0);

        /// <summary>
        /// Identifies the <see cref="LegendTitleFontWeight"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<FontWeight> LegendTitleFontWeightProperty = AvaloniaProperty.Register<Plot, FontWeight>(nameof(LegendTitleFontWeight), FontWeight.Bold);

        /// <summary>
        /// Identifies the <see cref="PlotAreaBackground"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Brush> PlotAreaBackgroundProperty = AvaloniaProperty.Register<Plot, Brush>(nameof(PlotAreaBackground), null);

        /// <summary>
        /// Identifies the <see cref="PlotAreaBorderColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> PlotAreaBorderColorProperty = AvaloniaProperty.Register<Plot, Color>(nameof(PlotAreaBorderColor), Colors.Black);

        /// <summary>
        /// Identifies the <see cref="PlotAreaBorderThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Thickness> PlotAreaBorderThicknessProperty = AvaloniaProperty.Register<Plot, Thickness>(nameof(PlotAreaBorderThickness), new Thickness(1.0));

        /// <summary>
        /// Identifies the <see cref="PlotMargins"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Thickness> PlotMarginsProperty = AvaloniaProperty.Register<Plot, Thickness>(nameof(PlotMargins), new Thickness(double.NaN));

        /// <summary>
        /// Identifies the <see cref="PlotType"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<PlotType> PlotTypeProperty = AvaloniaProperty.Register<Plot, PlotType>(nameof(PlotType), PlotType.XY);

        /// <summary>
        /// Identifies the <see cref="SubtitleFontSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> SubtitleFontSizeProperty = AvaloniaProperty.Register<Plot, double>(nameof(SubtitleFontSize), 14.0);

        /// <summary>
        /// Identifies the <see cref="SubtitleFontWeight"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<FontWeight> SubtitleFontWeightProperty = AvaloniaProperty.Register<Plot, FontWeight>(nameof(SubtitleFontWeight), FontWeight.Normal);

        /// <summary>
        /// Identifies the <see cref="Subtitle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> SubtitleProperty = AvaloniaProperty.Register<Plot, string>(nameof(Subtitle), null);

        /// <summary>
        /// Identifies the <see cref="TextColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> TextColorProperty = AvaloniaProperty.Register<Plot, Color>(nameof(TextColor), Colors.Black);

        /// <summary>
        /// Identifies the <see cref="TitleHorizontalAlignment"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<TitleHorizontalAlignment> TitleAlignmentProperty = AvaloniaProperty.Register<Plot, TitleHorizontalAlignment>(nameof(TitleHorizontalAlignment), TitleHorizontalAlignment.CenteredWithinPlotArea);

        /// <summary>
        /// Identifies the <see cref="TitleFont"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> TitleFontProperty = AvaloniaProperty.Register<Plot, string>(nameof(TitleFont), null);

        /// <summary>
        /// Identifies the <see cref="TitleFontSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> TitleFontSizeProperty = AvaloniaProperty.Register<Plot, double>(nameof(TitleFontSize), 18.0);

        /// <summary>
        /// Identifies the <see cref="TitleFontWeight"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<FontWeight> TitleFontWeightProperty = AvaloniaProperty.Register<Plot, FontWeight>(nameof(TitleFontWeight), FontWeight.Bold);

        /// <summary>
        /// Identifies the <see cref="TitlePadding"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> TitlePaddingProperty = AvaloniaProperty.Register<Plot, double>(nameof(TitlePadding), 6.0);

        /// <summary>
        /// Identifies the <see cref="Title"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<Plot, string>(nameof(Title), null);

        /// <summary>
        /// Identifies the <see cref="TitleToolTip"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> TitleToolTipProperty = AvaloniaProperty.Register<Plot, string>(nameof(TitleToolTip), null);

        /// <summary>
        /// Identifies the <see cref="InvalidateFlag"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<int> InvalidateFlagProperty = AvaloniaProperty.Register<Plot, int>(nameof(InvalidateFlag), 0);

        /// <summary>
        /// Initializes static members of the <see cref="Plot" /> class.
        /// </summary>
        static Plot()
        {
            PaddingProperty.OverrideDefaultValue<Plot>(new Thickness(8));
            PaddingProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            CultureProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            IsLegendVisibleProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendBackgroundProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendBorderProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendBorderThicknessProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendFontProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendFontSizeProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendFontWeightProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendItemAlignmentProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendItemOrderProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendItemSpacingProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendLineSpacingProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendMarginProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendMaxHeightProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendMaxWidthProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendOrientationProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendColumnSpacingProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendPaddingProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendPlacementProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendPositionProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendSymbolLengthProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendSymbolMarginProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendSymbolPlacementProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            SelectionColorProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            RenderingDecoratorProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            SubtitleFontProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            TitleColorProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            SubtitleColorProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            DefaultFontProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            DefaultFontSizeProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            DefaultColorsProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            AxisTierDistanceProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendTextColorProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendTitleProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendTitleColorProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendTitleFontProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendTitleFontSizeProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            LegendTitleFontWeightProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            PlotAreaBackgroundProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            PlotAreaBorderColorProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            PlotAreaBorderThicknessProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            PlotMarginsProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            PlotTypeProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            SubtitleFontSizeProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            SubtitleFontWeightProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            SubtitleProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            TextColorProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            TitleAlignmentProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            TitleFontProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            TitleFontSizeProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            TitleFontWeightProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            TitlePaddingProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            TitleProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            TitleToolTipProperty.Changed.AddClassHandler<Plot>(AppearanceChanged);
            InvalidateFlagProperty.Changed.AddClassHandler<Plot>((s, e) => s.InvalidateFlagChanged());
        }

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
                return axes;
            }
        }

        /// <summary>
        /// Gets or sets Culture.
        /// </summary>
        public CultureInfo Culture
        {
            get
            {
                return GetValue(CultureProperty);
            }

            set
            {
                SetValue(CultureProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsLegendVisible.
        /// </summary>
        public bool IsLegendVisible
        {
            get
            {
                return GetValue(IsLegendVisibleProperty);
            }

            set
            {
                SetValue(IsLegendVisibleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendBackground.
        /// </summary>
        public Color LegendBackground
        {
            get
            {
                return GetValue(LegendBackgroundProperty);
            }

            set
            {
                SetValue(LegendBackgroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendBorder.
        /// </summary>
        public Color LegendBorder
        {
            get
            {
                return GetValue(LegendBorderProperty);
            }

            set
            {
                SetValue(LegendBorderProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendBorderThickness.
        /// </summary>
        public double LegendBorderThickness
        {
            get
            {
                return GetValue(LegendBorderThicknessProperty);
            }

            set
            {
                SetValue(LegendBorderThicknessProperty, value);
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
                return GetValue(LegendColumnSpacingProperty);
            }

            set
            {
                SetValue(LegendColumnSpacingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendFont.
        /// </summary>
        public string LegendFont
        {
            get
            {
                return GetValue(LegendFontProperty);
            }

            set
            {
                SetValue(LegendFontProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendFontSize.
        /// </summary>
        public double LegendFontSize
        {
            get
            {
                return GetValue(LegendFontSizeProperty);
            }

            set
            {
                SetValue(LegendFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendFontWeight.
        /// </summary>
        public FontWeight LegendFontWeight
        {
            get
            {
                return GetValue(LegendFontWeightProperty);
            }

            set
            {
                SetValue(LegendFontWeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendItemAlignment.
        /// </summary>
        public global::Avalonia.Layout.HorizontalAlignment LegendItemAlignment
        {
            get
            {
                return GetValue(LegendItemAlignmentProperty);
            }

            set
            {
                SetValue(LegendItemAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendItemOrder.
        /// </summary>
        public LegendItemOrder LegendItemOrder
        {
            get
            {
                return GetValue(LegendItemOrderProperty);
            }

            set
            {
                SetValue(LegendItemOrderProperty, value);
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
                return GetValue(LegendItemSpacingProperty);
            }

            set
            {
                SetValue(LegendItemSpacingProperty, value);
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
                return GetValue(LegendLineSpacingProperty);
            }

            set
            {
                SetValue(LegendLineSpacingProperty, value);
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
                return GetValue(LegendMaxHeightProperty);
            }

            set
            {
                SetValue(LegendMaxHeightProperty, value);
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
                return GetValue(LegendMaxWidthProperty);
            }

            set
            {
                SetValue(LegendMaxWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendMargin.
        /// </summary>
        public double LegendMargin
        {
            get
            {
                return GetValue(LegendMarginProperty);
            }

            set
            {
                SetValue(LegendMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendOrientation.
        /// </summary>
        public LegendOrientation LegendOrientation
        {
            get
            {
                return GetValue(LegendOrientationProperty);
            }

            set
            {
                SetValue(LegendOrientationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the legend padding.
        /// </summary>
        public double LegendPadding
        {
            get
            {
                return GetValue(LegendPaddingProperty);
            }

            set
            {
                SetValue(LegendPaddingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendPlacement.
        /// </summary>
        public LegendPlacement LegendPlacement
        {
            get
            {
                return GetValue(LegendPlacementProperty);
            }

            set
            {
                SetValue(LegendPlacementProperty, value);
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
                return GetValue(LegendPositionProperty);
            }

            set
            {
                SetValue(LegendPositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendSymbolLength.
        /// </summary>
        public double LegendSymbolLength
        {
            get
            {
                return GetValue(LegendSymbolLengthProperty);
            }

            set
            {
                SetValue(LegendSymbolLengthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendSymbolMargin.
        /// </summary>
        public double LegendSymbolMargin
        {
            get
            {
                return GetValue(LegendSymbolMarginProperty);
            }

            set
            {
                SetValue(LegendSymbolMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendSymbolPlacement.
        /// </summary>
        public LegendSymbolPlacement LegendSymbolPlacement
        {
            get
            {
                return GetValue(LegendSymbolPlacementProperty);
            }

            set
            {
                SetValue(LegendSymbolPlacementProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LegendTitleFont.
        /// </summary>
        public string LegendTitleFont
        {
            get
            {
                return GetValue(LegendTitleFontProperty);
            }

            set
            {
                SetValue(LegendTitleFontProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the default font.
        /// </summary>
        public string DefaultFont
        {
            get
            {
                return GetValue(DefaultFontProperty);
            }

            set
            {
                SetValue(DefaultFontProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the default font size.
        /// </summary>
        public double DefaultFontSize
        {
            get
            {
                return GetValue(DefaultFontSizeProperty);
            }

            set
            {
                SetValue(DefaultFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the default colors.
        /// </summary>
        public IList<Color> DefaultColors
        {
            get
            {
                return GetValue(DefaultColorsProperty);
            }

            set
            {
                SetValue(DefaultColorsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text color of the legends.
        /// </summary>
        public Color LegendTextColor
        {
            get
            {
                return GetValue(LegendTextColorProperty);
            }

            set
            {
                SetValue(LegendTextColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the legend title.
        /// </summary>
        public string LegendTitle
        {
            get
            {
                return GetValue(LegendTitleProperty);
            }

            set
            {
                SetValue(LegendTitleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets color of the legend title.
        /// </summary>
        public Color LegendTitleColor
        {
            get
            {
                return GetValue(LegendTitleColorProperty);
            }

            set
            {
                SetValue(LegendTitleColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the axis tier distance.
        /// </summary>
        public double AxisTierDistance
        {
            get
            {
                return GetValue(AxisTierDistanceProperty);
            }

            set
            {
                SetValue(LegendTitleFontProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of selected elements.
        /// </summary>
        public Color SelectionColor
        {
            get
            {
                return GetValue(SelectionColorProperty);
            }

            set
            {
                SetValue(LegendTitleFontProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a rendering decorator.
        /// </summary>
        public Func<IRenderContext, IRenderContext> RenderingDecorator
        {
            get
            {
                return GetValue(RenderingDecoratorProperty);
            }

            set
            {
                SetValue(RenderingDecoratorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the font of the subtitles.
        /// </summary>
        public string SubtitleFont
        {
            get
            {
                return GetValue(SubtitleFontProperty);
            }

            set
            {
                SetValue(SubtitleFontProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the titles.
        /// </summary>
        public Color TitleColor
        {
            get
            {
                return GetValue(TitleColorProperty);
            }

            set
            {
                SetValue(TitleColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the font size of the legend titles.
        /// </summary>
        public double LegendTitleFontSize
        {
            get
            {
                return GetValue(LegendTitleFontSizeProperty);
            }

            set
            {
                SetValue(LegendTitleFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the subtitles.
        /// </summary>
        public Color SubtitleColor
        {
            get
            {
                return GetValue(SubtitleColorProperty);
            }

            set
            {
                SetValue(SubtitleColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the font weight of the legend titles.
        /// </summary>
        public FontWeight LegendTitleFontWeight
        {
            get
            {
                return GetValue(LegendTitleFontWeightProperty);
            }

            set
            {
                SetValue(LegendTitleFontWeightProperty, value);
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
                return GetValue(PlotAreaBackgroundProperty);
            }

            set
            {
                SetValue(PlotAreaBackgroundProperty, value);
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
                return GetValue(PlotAreaBorderColorProperty);
            }

            set
            {
                SetValue(PlotAreaBorderColorProperty, value);
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
                return GetValue(PlotAreaBorderThicknessProperty);
            }

            set
            {
                SetValue(PlotAreaBorderThicknessProperty, value);
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
                return GetValue(PlotMarginsProperty);
            }

            set
            {
                SetValue(PlotMarginsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets PlotType.
        /// </summary>
        public PlotType PlotType
        {
            get
            {
                return GetValue(PlotTypeProperty);
            }

            set
            {
                SetValue(PlotTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets the series.
        /// </summary>
        /// <value>The series.</value>
        [Content]
        public Collection<Series> Series
        {
            get
            {
                return series;
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
                return GetValue(SubtitleProperty);
            }

            set
            {
                SetValue(SubtitleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the font size of the subtitle.
        /// </summary>
        public double SubtitleFontSize
        {
            get
            {
                return GetValue(SubtitleFontSizeProperty);
            }

            set
            {
                SetValue(SubtitleFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the font weight of the subtitle.
        /// </summary>
        public FontWeight SubtitleFontWeight
        {
            get
            {
                return GetValue(SubtitleFontWeightProperty);
            }

            set
            {
                SetValue(SubtitleFontWeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets text color.
        /// </summary>
        public Color TextColor
        {
            get
            {
                return GetValue(TextColorProperty);
            }

            set
            {
                SetValue(TextColorProperty, value);
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
                return GetValue(TitleProperty);
            }

            set
            {
                SetValue(TitleProperty, value);
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
                return GetValue(TitleToolTipProperty);
            }

            set
            {
                SetValue(TitleToolTipProperty, value);
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
                return GetValue(TitleAlignmentProperty);
            }

            set
            {
                SetValue(TitleAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets font of the title.
        /// </summary>
        public string TitleFont
        {
            get
            {
                return GetValue(TitleFontProperty);
            }

            set
            {
                SetValue(TitleFontProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets font size of the title.
        /// </summary>
        public double TitleFontSize
        {
            get
            {
                return GetValue(TitleFontSizeProperty);
            }

            set
            {
                SetValue(TitleFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets font weight of the title.
        /// </summary>
        public FontWeight TitleFontWeight
        {
            get
            {
                return GetValue(TitleFontWeightProperty);
            }

            set
            {
                SetValue(TitleFontWeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets padding around the title.
        /// </summary>
        public double TitlePadding
        {
            get
            {
                return GetValue(TitlePaddingProperty);
            }

            set
            {
                SetValue(TitlePaddingProperty, value);
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
                return GetValue(InvalidateFlagProperty);
            }

            set
            {
                SetValue(InvalidateFlagProperty, value);
            }
        }

        /// <summary>
        /// Invalidates the Plot control/view when the <see cref="InvalidateFlag" /> property is changed.
        /// </summary>
        private void InvalidateFlagChanged()
        {
            InvalidatePlot();
        }
    }
}