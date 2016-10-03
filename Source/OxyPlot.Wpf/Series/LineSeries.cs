// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.LineSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    using OxyPlot.Series;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.LineSeries
    /// </summary>
    public class LineSeries : DataPointSeries
    {
        /// <summary>
        /// Identifies the <see cref="BrokenLineColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BrokenLineColorProperty = DependencyProperty.Register(
            "BrokenLineColor", typeof(Color), typeof(LineSeries), new PropertyMetadata(MoreColors.Undefined, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="BrokenLineStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BrokenLineStyleProperty = DependencyProperty.Register(
            "BrokenLineStyle", typeof(LineStyle), typeof(LineSeries), new PropertyMetadata(LineStyle.Solid, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="BrokenLineThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BrokenLineThicknessProperty = DependencyProperty.Register(
            "BrokenLineThickness", typeof(double), typeof(LineSeries), new PropertyMetadata(0d, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Dashes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DashesProperty = DependencyProperty.Register(
            "Dashes", typeof(double[]), typeof(LineSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Decimator"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DecimatorProperty =
            DependencyProperty.Register("Decimator", typeof(Action<List<ScreenPoint>, List<ScreenPoint>>), typeof(LineSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LabelFormatString"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFormatStringProperty =
            DependencyProperty.Register(
                "LabelFormatString", typeof(string), typeof(LineSeries), new UIPropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LabelMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelMarginProperty = DependencyProperty.Register(
            "LabelMargin", typeof(double), typeof(LineSeries), new UIPropertyMetadata(6.0));

        /// <summary>
        /// Identifies the <see cref="LineJoin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineJoinProperty = DependencyProperty.Register(
            "LineJoin",
            typeof(LineJoin),
            typeof(LineSeries),
            new PropertyMetadata(LineJoin.Bevel, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LineLegendPosition"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineLegendPositionProperty =
            DependencyProperty.Register(
                "LineLegendPosition",
                typeof(LineLegendPosition),
                typeof(LineSeries),
                new UIPropertyMetadata(LineLegendPosition.None));

        /// <summary>
        /// Identifies the <see cref="LineStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(
            "LineStyle", typeof(LineStyle), typeof(LineSeries), new PropertyMetadata(LineStyle.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerFill"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerFillProperty = DependencyProperty.Register(
            "MarkerFill", typeof(Color), typeof(LineSeries), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerOutline"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerOutlineProperty = DependencyProperty.Register(
            "MarkerOutline", typeof(Point[]), typeof(LineSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerResolution"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerResolutionProperty =
            DependencyProperty.Register("MarkerResolution", typeof(int), typeof(LineSeries), new PropertyMetadata(0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerSizeProperty = DependencyProperty.Register(
            "MarkerSize", typeof(double), typeof(LineSeries), new PropertyMetadata(3.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerStroke"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerStrokeProperty = DependencyProperty.Register(
            "MarkerStroke", typeof(Color), typeof(LineSeries), new PropertyMetadata(MoreColors.Automatic, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerStrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerStrokeThicknessProperty =
            DependencyProperty.Register(
                "MarkerStrokeThickness",
                typeof(double),
                typeof(LineSeries),
                new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MarkerType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MarkerTypeProperty = DependencyProperty.Register(
            "MarkerType",
            typeof(MarkerType),
            typeof(LineSeries),
            new PropertyMetadata(MarkerType.None, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="MinimumSegmentLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumSegmentLengthProperty =
            DependencyProperty.Register(
                "MinimumSegmentLength", typeof(double), typeof(LineSeries), new PropertyMetadata(2.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="InterpolationAlgorithm"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InterpolationAlgorithmProperty = DependencyProperty.Register(
            "InterpolationAlgorithm", typeof(IInterpolationAlgorithm), typeof(LineSeries), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", typeof(double), typeof(LineSeries), new PropertyMetadata(2.0, AppearanceChanged));

        /// <summary>
        /// Initializes static members of the <see cref="LineSeries" /> class.
        /// </summary>
        static LineSeries()
        {
            CanTrackerInterpolatePointsProperty.OverrideMetadata(typeof(LineSeries), new PropertyMetadata(true, AppearanceChanged));
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
                return (Color)this.GetValue(BrokenLineColorProperty);
            }

            set
            {
                this.SetValue(BrokenLineColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the broken line style.
        /// </summary>
        public LineStyle BrokenLineStyle
        {
            get
            {
                return (LineStyle)this.GetValue(BrokenLineStyleProperty);
            }

            set
            {
                this.SetValue(BrokenLineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the broken line thickness.
        /// </summary>
        public double BrokenLineThickness
        {
            get
            {
                return (double)this.GetValue(BrokenLineThicknessProperty);
            }

            set
            {
                this.SetValue(BrokenLineThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Dashes.
        /// </summary>
        public double[] Dashes
        {
            get
            {
                return (double[])this.GetValue(DashesProperty);
            }

            set
            {
                this.SetValue(DashesProperty, value);
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
            get { return (Action<List<ScreenPoint>, List<ScreenPoint>>)this.GetValue(DecimatorProperty); }
            set { this.SetValue(DecimatorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label format string.
        /// </summary>
        /// <value>The label format string.</value>
        public string LabelFormatString
        {
            get
            {
                return (string)this.GetValue(LabelFormatStringProperty);
            }

            set
            {
                this.SetValue(LabelFormatStringProperty, value);
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
                return (double)this.GetValue(LabelMarginProperty);
            }

            set
            {
                this.SetValue(LabelMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineJoin.
        /// </summary>
        public LineJoin LineJoin
        {
            get
            {
                return (LineJoin)this.GetValue(LineJoinProperty);
            }

            set
            {
                this.SetValue(LineJoinProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineLegendPosition.
        /// </summary>
        public LineLegendPosition LineLegendPosition
        {
            get
            {
                return (LineLegendPosition)this.GetValue(LineLegendPositionProperty);
            }

            set
            {
                this.SetValue(LineLegendPositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineStyle.
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
        /// Gets or sets MarkerFill.
        /// </summary>
        public Color MarkerFill
        {
            get
            {
                return (Color)this.GetValue(MarkerFillProperty);
            }

            set
            {
                this.SetValue(MarkerFillProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerOutline.
        /// </summary>
        public Point[] MarkerOutline
        {
            get
            {
                return (Point[])this.GetValue(MarkerOutlineProperty);
            }

            set
            {
                this.SetValue(MarkerOutlineProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the marker resolution.
        /// </summary>
        /// <value>The marker resolution.</value>
        public int MarkerResolution
        {
            get { return (int)this.GetValue(MarkerResolutionProperty); }
            set { this.SetValue(MarkerResolutionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the marker size.
        /// </summary>
        public double MarkerSize
        {
            get
            {
                return (double)this.GetValue(MarkerSizeProperty);
            }

            set
            {
                this.SetValue(MarkerSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerStroke.
        /// </summary>
        public Color MarkerStroke
        {
            get
            {
                return (Color)this.GetValue(MarkerStrokeProperty);
            }

            set
            {
                this.SetValue(MarkerStrokeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerStrokeThickness.
        /// </summary>
        public double MarkerStrokeThickness
        {
            get
            {
                return (double)this.GetValue(MarkerStrokeThicknessProperty);
            }

            set
            {
                this.SetValue(MarkerStrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MarkerType.
        /// </summary>
        public MarkerType MarkerType
        {
            get
            {
                return (MarkerType)this.GetValue(MarkerTypeProperty);
            }

            set
            {
                this.SetValue(MarkerTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MinimumSegmentLength.
        /// </summary>
        public double MinimumSegmentLength
        {
            get
            {
                return (double)this.GetValue(MinimumSegmentLengthProperty);
            }

            set
            {
                this.SetValue(MinimumSegmentLengthProperty, value);
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
                return (IInterpolationAlgorithm)this.GetValue(InterpolationAlgorithmProperty);
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
                return (double)this.GetValue(StrokeThicknessProperty);
            }

            set
            {
                this.SetValue(StrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal series.
        /// </summary>
        /// <returns>The internal series.</returns>
        public override OxyPlot.Series.Series CreateModel()
        {
            this.SynchronizeProperties(this.InternalSeries);
            return this.InternalSeries;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.LineSeries)series;
            s.Color = this.Color.ToOxyColor();
            s.StrokeThickness = this.StrokeThickness;
            s.LineStyle = this.LineStyle;
            s.MarkerResolution = this.MarkerResolution;
            s.MarkerSize = this.MarkerSize;
            s.MarkerStroke = this.MarkerStroke.ToOxyColor();
            s.MarkerType = this.MarkerType;
            s.MarkerStrokeThickness = this.MarkerStrokeThickness;
            s.Dashes = this.Dashes;
            s.LineJoin = this.LineJoin;
            s.MarkerFill = this.MarkerFill.ToOxyColor();
            s.MarkerOutline = this.MarkerOutline.ToScreenPointArray();
            s.MinimumSegmentLength = this.MinimumSegmentLength;
            s.LabelFormatString = this.LabelFormatString;
            s.LabelMargin = this.LabelMargin;
            s.LineLegendPosition = this.LineLegendPosition;
            s.BrokenLineColor = this.BrokenLineColor.ToOxyColor();
            s.BrokenLineStyle = this.BrokenLineStyle;
            s.BrokenLineThickness = this.BrokenLineThickness;
            s.Decimator = this.Decimator;
            s.InterpolationAlgorithm = this.InterpolationAlgorithm;
        }
    }
}