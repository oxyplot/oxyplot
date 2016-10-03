// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.LineSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;
    using OxyPlot.Series;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.LineSeries
    /// </summary>
    public class LineSeries : DataPointSeries
    {
        /// <summary>
        /// Identifies the <see cref="BrokenLineColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> BrokenLineColorProperty = AvaloniaProperty.Register<LineSeries, Color>(nameof(BrokenLineColor), MoreColors.Undefined);

        /// <summary>
        /// Identifies the <see cref="BrokenLineStyle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> BrokenLineStyleProperty = AvaloniaProperty.Register<LineSeries, LineStyle>(nameof(BrokenLineStyle), LineStyle.Solid);

        /// <summary>
        /// Identifies the <see cref="BrokenLineThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> BrokenLineThicknessProperty = AvaloniaProperty.Register<LineSeries, double>(nameof(BrokenLineThickness), 0d);

        /// <summary>
        /// Identifies the <see cref="Dashes"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double[]> DashesProperty = AvaloniaProperty.Register<LineSeries, double[]>(nameof(Dashes), null);

        /// <summary>
        /// Identifies the <see cref="Decimator"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Action<List<ScreenPoint>, List<ScreenPoint>>> DecimatorProperty = AvaloniaProperty.Register<LineSeries, Action<List<ScreenPoint>, List<ScreenPoint>>>(nameof(Decimator), null);

        /// <summary>
        /// Identifies the <see cref="LabelFormatString"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> LabelFormatStringProperty = AvaloniaProperty.Register<LineSeries, string>(nameof(LabelFormatString));

        /// <summary>
        /// Identifies the <see cref="LabelMargin"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> LabelMarginProperty = AvaloniaProperty.Register<LineSeries, double>(nameof(LabelMargin), 6.0);

        /// <summary>
        /// Identifies the <see cref="LineJoin"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineJoin> LineJoinProperty = AvaloniaProperty.Register<LineSeries, LineJoin>(nameof(LineJoin), LineJoin.Bevel);

        /// <summary>
        /// Identifies the <see cref="LineLegendPosition"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineLegendPosition> LineLegendPositionProperty = AvaloniaProperty.Register<LineSeries, LineLegendPosition>(nameof(LineLegendPosition), LineLegendPosition.None);

        /// <summary>
        /// Identifies the <see cref="LineStyle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> LineStyleProperty = AvaloniaProperty.Register<LineSeries, LineStyle>(nameof(LineStyle), LineStyle.Automatic);

        /// <summary>
        /// Identifies the <see cref="MarkerFill"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> MarkerFillProperty = AvaloniaProperty.Register<LineSeries, Color>(nameof(MarkerFill), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="MarkerOutline"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Point[]> MarkerOutlineProperty = AvaloniaProperty.Register<LineSeries, Point[]>(nameof(MarkerOutline), null);

        /// <summary>
        /// Identifies the <see cref="MarkerResolution"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<int> MarkerResolutionProperty = AvaloniaProperty.Register<LineSeries, int>(nameof(MarkerResolution), 0);

        /// <summary>
        /// Identifies the <see cref="MarkerSize"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MarkerSizeProperty = AvaloniaProperty.Register<LineSeries, double>(nameof(MarkerSize), 3.0);

        /// <summary>
        /// Identifies the <see cref="MarkerStroke"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> MarkerStrokeProperty = AvaloniaProperty.Register<LineSeries, Color>(nameof(MarkerStroke), MoreColors.Automatic);

        /// <summary>
        /// Identifies the <see cref="MarkerStrokeThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MarkerStrokeThicknessProperty = AvaloniaProperty.Register<LineSeries, double>(nameof(MarkerStrokeThickness), 1.0);

        /// <summary>
        /// Identifies the <see cref="MarkerType"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<MarkerType> MarkerTypeProperty = AvaloniaProperty.Register<LineSeries, MarkerType>(nameof(MarkerType), MarkerType.None);

        /// <summary>
        /// Identifies the <see cref="MinimumSegmentLength"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MinimumSegmentLengthProperty = AvaloniaProperty.Register<LineSeries, double>(nameof(MinimumSegmentLength), 2.0);

        /// <summary>
        /// Identifies the <see cref="InterpolationAlgorithm"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<IInterpolationAlgorithm> InterpolationAlgorithmProperty = AvaloniaProperty.Register<LineSeries, IInterpolationAlgorithm>(nameof(InterpolationAlgorithm), null);

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> StrokeThicknessProperty = AvaloniaProperty.Register<LineSeries, double>(nameof(StrokeThickness), 2.0);

        /// <summary>
        /// Initializes static members of the <see cref="LineSeries" /> class.
        /// </summary>
        static LineSeries()
        {
            CanTrackerInterpolatePointsProperty.OverrideMetadata(typeof(LineSeries), new StyledPropertyMetadata<bool>(true));
            BrokenLineColorProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            BrokenLineStyleProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            BrokenLineThicknessProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            DashesProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            DecimatorProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            LineJoinProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            LineStyleProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            MarkerFillProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            MarkerOutlineProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            MarkerResolutionProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            MarkerSizeProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            MarkerStrokeProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            MarkerStrokeThicknessProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            MarkerTypeProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            MinimumSegmentLengthProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            StrokeThicknessProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
            CanTrackerInterpolatePointsProperty.Changed.AddClassHandler<LineSeries>(AppearanceChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries" /> class.
        /// </summary>
        public LineSeries()
        {
            this.InternalSeries = new OxyPlot.Series.LineSeries();
        }

        /// <summary>
        /// Gets or sets the broken line color.
        /// </summary>
        public Color BrokenLineColor
        {
            get
            {
                return GetValue(BrokenLineColorProperty);
            }

            set
            {
                SetValue(BrokenLineColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the broken line style.
        /// </summary>
        public LineStyle BrokenLineStyle
        {
            get
            {
                return GetValue(BrokenLineStyleProperty);
            }

            set
            {
                SetValue(BrokenLineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the broken line thickness.
        /// </summary>
        public double BrokenLineThickness
        {
            get
            {
                return GetValue(BrokenLineThicknessProperty);
            }

            set
            {
                SetValue(BrokenLineThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Dashes.
        /// </summary>
        public double[] Dashes
        {
            get
            {
                return GetValue(DashesProperty);
            }

            set
            {
                SetValue(DashesProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the decimator.
        /// </summary>
        /// <value>
        /// The decimator.
        /// </value>
        public Action<List<ScreenPoint>, List<ScreenPoint>> Decimator
        {
            get { return GetValue(DecimatorProperty); }
            set { SetValue(DecimatorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label format string.
        /// </summary>
        /// <value>The label format string.</value>
        public string LabelFormatString
        {
            get
            {
                return GetValue(LabelFormatStringProperty);
            }

            set
            {
                SetValue(LabelFormatStringProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label margin.
        /// </summary>
        /// <value>The label margin.</value>
        public double LabelMargin
        {
            get
            {
                return GetValue(LabelMarginProperty);
            }

            set
            {
                SetValue(LabelMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineJoin.
        /// </summary>
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
        /// Gets or sets LineLegendPosition.
        /// </summary>
        public LineLegendPosition LineLegendPosition
        {
            get
            {
                return GetValue(LineLegendPositionProperty);
            }

            set
            {
                SetValue(LineLegendPositionProperty, value);
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
        /// Gets or sets MarkerFill.
        /// </summary>
        public Color MarkerFill
        {
            get
            {
                return GetValue(MarkerFillProperty);
            }

            set
            {
                SetValue(MarkerFillProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerOutline.
        /// </summary>
        public Point[] MarkerOutline
        {
            get
            {
                return GetValue(MarkerOutlineProperty);
            }

            set
            {
                SetValue(MarkerOutlineProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the marker resolution.
        /// </summary>
        /// <value>The marker resolution.</value>
        public int MarkerResolution
        {
            get { return GetValue(MarkerResolutionProperty); }
            set { SetValue(MarkerResolutionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the marker size.
        /// </summary>
        public double MarkerSize
        {
            get
            {
                return GetValue(MarkerSizeProperty);
            }

            set
            {
                SetValue(MarkerSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerStroke.
        /// </summary>
        public Color MarkerStroke
        {
            get
            {
                return GetValue(MarkerStrokeProperty);
            }

            set
            {
                SetValue(MarkerStrokeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerStrokeThickness.
        /// </summary>
        public double MarkerStrokeThickness
        {
            get
            {
                return GetValue(MarkerStrokeThicknessProperty);
            }

            set
            {
                SetValue(MarkerStrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerType.
        /// </summary>
        public MarkerType MarkerType
        {
            get
            {
                return GetValue(MarkerTypeProperty);
            }

            set
            {
                SetValue(MarkerTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinimumSegmentLength.
        /// </summary>
        public double MinimumSegmentLength
        {
            get
            {
                return GetValue(MinimumSegmentLengthProperty);
            }

            set
            {
                SetValue(MinimumSegmentLengthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value the interpolation algorithm.
        /// </summary>
        /// <value>Interpolation algorithm.</value>
        public IInterpolationAlgorithm InterpolationAlgorithm
        {
            get
            {
                return this.GetValue(InterpolationAlgorithmProperty);
            }

            set
            {
                this.SetValue(InterpolationAlgorithmProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets StrokeThickness.
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
        /// Creates the internal series.
        /// </summary>
        /// <returns>The internal series.</returns>
        public override OxyPlot.Series.Series CreateModel()
        {
            SynchronizeProperties(InternalSeries);
            return InternalSeries;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.LineSeries)series;
            s.Color = Color.ToOxyColor();
            s.StrokeThickness = StrokeThickness;
            s.LineStyle = LineStyle;
            s.MarkerResolution = MarkerResolution;
            s.MarkerSize = MarkerSize;
            s.MarkerStroke = MarkerStroke.ToOxyColor();
            s.MarkerType = MarkerType;
            s.MarkerStrokeThickness = MarkerStrokeThickness;
            s.Dashes = Dashes;
            s.LineJoin = LineJoin;
            s.MarkerFill = MarkerFill.ToOxyColor();
            s.MarkerOutline = (MarkerOutline ?? Enumerable.Empty<Point>()).Select(point => point.ToScreenPoint()).ToArray();
            s.MinimumSegmentLength = MinimumSegmentLength;
            s.LabelFormatString = LabelFormatString;
            s.LabelMargin = LabelMargin;
            s.LineLegendPosition = LineLegendPosition;
            s.BrokenLineColor = BrokenLineColor.ToOxyColor();
            s.BrokenLineStyle = BrokenLineStyle;
            s.BrokenLineThickness = BrokenLineThickness;
            s.Decimator = Decimator;
            s.InterpolationAlgorithm = this.InterpolationAlgorithm;
        }
    }
}