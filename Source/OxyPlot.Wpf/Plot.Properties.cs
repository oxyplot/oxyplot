// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Plot.Properties.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents the WPF Plot control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Represents the WPF Plot control.
    /// </summary>
    /// <remarks>This file contains dependency properties used for defining the plot in XAML. These properties are only used when Model is <c>null</c>. In this case an internal PlotModel is created and the dependency properties are copied from the control to the internal PlotModel.</remarks>
    public partial class Plot
    {
        /// <summary>
        /// Identifies the <see cref="Controller"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register("Controller", typeof(IPlotController), typeof(Plot), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="AutoAdjustPlotMargins"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AutoAdjustPlotMarginsProperty =
            DependencyProperty.Register("AutoAdjustPlotMargins", typeof(bool), typeof(Plot), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="Culture"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(
            "Culture", typeof(CultureInfo), typeof(Plot), new UIPropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="DefaultTrackerTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DefaultTrackerTemplateProperty =
            DependencyProperty.Register("DefaultTrackerTemplate", typeof(ControlTemplate), typeof(Plot));

        /// <summary>
        /// Identifies the <see cref="IsLegendVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLegendVisibleProperty =
            DependencyProperty.Register(
                "IsLegendVisible", typeof(bool), typeof(Plot), new PropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="IsMouseWheelEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsMouseWheelEnabledProperty =
            DependencyProperty.Register("IsMouseWheelEnabled", typeof(bool), typeof(Plot), new UIPropertyMetadata(true));

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
        /// Identifies the <see cref="LegendMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LegendMarginProperty = DependencyProperty.Register(
            "LegendMargin", typeof(double), typeof(Plot), new PropertyMetadata(8.0, AppearanceChanged));

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
        /// Identifies the <see cref="Model"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
            "Model", typeof(PlotModel), typeof(Plot), new PropertyMetadata(null, ModelChanged));

        /// <summary>
        /// Identifies the <see cref="PanCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PanCursorProperty = DependencyProperty.Register(
            "PanCursor", typeof(Cursor), typeof(Plot), new PropertyMetadata(Cursors.Hand));

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
                "PlotAreaBorderThickness", typeof(double), typeof(Plot), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="PlotMargins"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlotMarginsProperty = DependencyProperty.Register(
            "PlotMargins",
            typeof(Thickness),
            typeof(Plot),
            new PropertyMetadata(new Thickness(60, 4, 4, 40), AppearanceChanged));

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
        /// Identifies the <see cref="InvalidateFlag"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InvalidateFlagProperty = DependencyProperty.Register(
            "InvalidateFlag",
            typeof(int),
            typeof(Plot),
            new FrameworkPropertyMetadata(0, (s, e) => ((Plot)s).InvalidateFlagChanged()));

        /// <summary>
        /// Identifies the <see cref="ZoomHorizontalCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomHorizontalCursorProperty =
            DependencyProperty.Register(
                "ZoomHorizontalCursor", typeof(Cursor), typeof(Plot), new PropertyMetadata(Cursors.SizeWE));

        /// <summary>
        /// Identifies the <see cref="ZoomRectangleCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomRectangleCursorProperty =
            DependencyProperty.Register(
                "ZoomRectangleCursor", typeof(Cursor), typeof(Plot), new PropertyMetadata(Cursors.SizeNWSE));

        /// <summary>
        /// Identifies the <see cref="ZoomRectangleTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomRectangleTemplateProperty =
            DependencyProperty.Register(
                "ZoomRectangleTemplate", typeof(ControlTemplate), typeof(Plot), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ZoomVerticalCursor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ZoomVerticalCursorProperty =
            DependencyProperty.Register(
                "ZoomVerticalCursor", typeof(Cursor), typeof(Plot), new PropertyMetadata(Cursors.SizeNS));

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
        /// Gets or sets a value indicating whether AutoAdjustPlotMargins.
        /// </summary>
        public bool AutoAdjustPlotMargins
        {
            get
            {
                return (bool)this.GetValue(AutoAdjustPlotMarginsProperty);
            }

            set
            {
                this.SetValue(AutoAdjustPlotMarginsProperty, value);
            }
        }

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
        /// Gets or sets the tracker template.
        /// </summary>
        /// <value>The tracker template.</value>
        public ControlTemplate DefaultTrackerTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(DefaultTrackerTemplateProperty);
            }

            set
            {
                this.SetValue(DefaultTrackerTemplateProperty, value);
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
        /// Gets or sets a value indicating whether the mouse wheel is enabled.
        /// </summary>
        public bool IsMouseWheelEnabled
        {
            get
            {
                return (bool)this.GetValue(IsMouseWheelEnabledProperty);
            }

            set
            {
                this.SetValue(IsMouseWheelEnabledProperty, value);
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
        /// Gets or sets LegendItemSpacing.
        /// </summary>
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
        /// Gets or sets the max width of the legends.
        /// </summary>
        /// <value>The max width of the legends.</value>
        public double LegendMaxWidth
        {
            get
            {
                return (double)GetValue(LegendMaxWidthProperty);
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
        /// Gets or sets the legend column spacing.
        /// </summary>
        /// <value>The legend column spacing.</value>
        public double LegendColumnSpacing
        {
            get { return (double)GetValue(LegendColumnSpacingProperty); }
            set { SetValue(LegendColumnSpacingProperty, value); }
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
        /// Gets or sets LegendTitleFontSize.
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
        /// Gets or sets LegendTitleFontWeight.
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
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public PlotModel Model
        {
            get
            {
                return (PlotModel)this.GetValue(ModelProperty);
            }

            set
            {
                this.SetValue(ModelProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the pan cursor.
        /// </summary>
        /// <value>The pan cursor.</value>
        public Cursor PanCursor
        {
            get
            {
                return (Cursor)this.GetValue(PanCursorProperty);
            }

            set
            {
                this.SetValue(PanCursorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the background brush of the plot area.
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
        /// Gets or sets the color of the plot area border.
        /// </summary>
        /// <value>The color of the plot area border.</value>
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
        /// Gets or sets the thickness of the plot area border.
        /// </summary>
        /// <value>The thickness of the plot area border.</value>
        public double PlotAreaBorderThickness
        {
            get
            {
                return (double)this.GetValue(PlotAreaBorderThicknessProperty);
            }

            set
            {
                this.SetValue(PlotAreaBorderThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the plot controller.
        /// </summary>
        /// <value>The plot controller.</value>
        public IPlotController Controller
        {
            get { return (IPlotController)GetValue(ControllerProperty); }
            set { SetValue(ControllerProperty, value); }
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
        /// Gets or sets the horizontal alignment of the title and subtitle.
        /// </summary>
        /// <value>
        /// The alignment.
        /// </value>
        public TitleHorizontalAlignment TitleHorizontalAlignment
        {
            get { return (TitleHorizontalAlignment)GetValue(TitleAlignmentProperty); }
            set { SetValue(TitleAlignmentProperty, value); }
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
        /// Gets or sets the refresh flag (an integer value). When the flag is changed, the plot will be refreshed.
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
        /// Gets or sets the horizontal zoom cursor.
        /// </summary>
        /// <value>The zoom horizontal cursor.</value>
        public Cursor ZoomHorizontalCursor
        {
            get
            {
                return (Cursor)this.GetValue(ZoomHorizontalCursorProperty);
            }

            set
            {
                this.SetValue(ZoomHorizontalCursorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the rectangle zoom cursor.
        /// </summary>
        /// <value>The zoom rectangle cursor.</value>
        public Cursor ZoomRectangleCursor
        {
            get
            {
                return (Cursor)this.GetValue(ZoomRectangleCursorProperty);
            }

            set
            {
                this.SetValue(ZoomRectangleCursorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the zoom rectangle template.
        /// </summary>
        /// <value>The zoom rectangle template.</value>
        public ControlTemplate ZoomRectangleTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(ZoomRectangleTemplateProperty);
            }

            set
            {
                this.SetValue(ZoomRectangleTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the vertical zoom cursor.
        /// </summary>
        /// <value>The zoom vertical cursor.</value>
        public Cursor ZoomVerticalCursor
        {
            get
            {
                return (Cursor)this.GetValue(ZoomVerticalCursorProperty);
            }

            set
            {
                this.SetValue(ZoomVerticalCursorProperty, value);
            }
        }

        /// <summary>
        /// Invalidates the plot control/view when the <see cref="InvalidateFlag" /> property is changed.
        /// </summary>
        private void InvalidateFlagChanged()
        {
            this.InvalidatePlot();
        }
    }
}