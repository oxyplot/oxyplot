// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.LineSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.LineSeries
    /// </summary>
    public class LineSeries : DataPointSeries
    {
        #region Constants and Fields

        /// <summary>
        /// The dashes property.
        /// </summary>
        public static readonly DependencyProperty DashesProperty = DependencyProperty.Register(
            "Dashes", typeof(double[]), typeof(LineSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The label format string property.
        /// </summary>
        public static readonly DependencyProperty LabelFormatStringProperty =
            DependencyProperty.Register(
                "LabelFormatString", typeof(string), typeof(LineSeries), new UIPropertyMetadata(null));

        /// <summary>
        /// The label margin property.
        /// </summary>
        public static readonly DependencyProperty LabelMarginProperty = DependencyProperty.Register(
            "LabelMargin", typeof(double), typeof(LineSeries), new UIPropertyMetadata(6.0));

        /// <summary>
        /// The line join property.
        /// </summary>
        public static readonly DependencyProperty LineJoinProperty = DependencyProperty.Register(
            "LineJoin", 
            typeof(OxyPenLineJoin), 
            typeof(LineSeries), 
            new PropertyMetadata(OxyPenLineJoin.Miter, AppearanceChanged));

        /// <summary>
        /// The line legend position property.
        /// </summary>
        public static readonly DependencyProperty LineLegendPositionProperty =
            DependencyProperty.Register(
                "LineLegendPosition", 
                typeof(LineLegendPosition), 
                typeof(LineSeries), 
                new UIPropertyMetadata(LineLegendPosition.None));

        /// <summary>
        /// The line style property.
        /// </summary>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(
            "LineStyle", typeof(LineStyle), typeof(LineSeries), new PropertyMetadata(LineStyle.Solid, AppearanceChanged));

        /// <summary>
        /// The marker fill property.
        /// </summary>
        public static readonly DependencyProperty MarkerFillProperty = DependencyProperty.Register(
            "MarkerFill", typeof(Color?), typeof(LineSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The marker outline property.
        /// </summary>
        public static readonly DependencyProperty MarkerOutlineProperty = DependencyProperty.Register(
            "MarkerOutline", typeof(Point[]), typeof(LineSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The marker size property.
        /// </summary>
        public static readonly DependencyProperty MarkerSizeProperty = DependencyProperty.Register(
            "MarkerSize", typeof(double), typeof(LineSeries), new PropertyMetadata(3.0, AppearanceChanged));

        /// <summary>
        /// The marker stroke property.
        /// </summary>
        public static readonly DependencyProperty MarkerStrokeProperty = DependencyProperty.Register(
            "MarkerStroke", typeof(OxyColor), typeof(LineSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The marker stroke thickness property.
        /// </summary>
        public static readonly DependencyProperty MarkerStrokeThicknessProperty =
            DependencyProperty.Register(
                "MarkerStrokeThickness", 
                typeof(double), 
                typeof(LineSeries), 
                new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// The marker type property.
        /// </summary>
        public static readonly DependencyProperty MarkerTypeProperty = DependencyProperty.Register(
            "MarkerType", 
            typeof(MarkerType), 
            typeof(LineSeries), 
            new PropertyMetadata(MarkerType.None, AppearanceChanged));

        /// <summary>
        /// The minimum segment length property.
        /// </summary>
        public static readonly DependencyProperty MinimumSegmentLengthProperty =
            DependencyProperty.Register(
                "MinimumSegmentLength", typeof(double), typeof(LineSeries), new PropertyMetadata(2.0, AppearanceChanged));

        /// <summary>
        /// The smooth property.
        /// </summary>
        public static readonly DependencyProperty SmoothProperty = DependencyProperty.Register(
            "Smooth", typeof(bool), typeof(LineSeries), new PropertyMetadata(false, AppearanceChanged));

        /// <summary>
        /// The stroke thickness property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", typeof(double), typeof(LineSeries), new PropertyMetadata(2.0, AppearanceChanged));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="LineSeries"/> class.
        /// </summary>
        static LineSeries()
        {
            CanTrackerInterpolatePointsProperty.OverrideMetadata(
                typeof(LineSeries), new PropertyMetadata(true, AppearanceChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeries"/> class.
        /// </summary>
        public LineSeries()
        {
            this.InternalSeries = new OxyPlot.LineSeries();
        }

        #endregion

        #region Public Properties

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
        /// Gets or sets the label format string.
        /// </summary>
        /// <value>
        /// The label format string. 
        /// </value>
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
        /// <value>
        /// The label margin. 
        /// </value>
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
        public OxyPenLineJoin LineJoin
        {
            get
            {
                return (OxyPenLineJoin)this.GetValue(LineJoinProperty);
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
        public Color? MarkerFill
        {
            get
            {
                return (Color?)this.GetValue(MarkerFillProperty);
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
        /// Gets or sets MarkerSize.
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
        public OxyColor MarkerStroke
        {
            get
            {
                return (OxyColor)this.GetValue(MarkerStrokeProperty);
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
        /// Gets or sets a value indicating whether to smooth the data.
        /// </summary>
        public bool Smooth
        {
            get
            {
                return (bool)this.GetValue(SmoothProperty);
            }

            set
            {
                this.SetValue(SmoothProperty, value);
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

        #endregion

        #region Public Methods

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// The series. 
        /// </returns>
        public override OxyPlot.Series CreateModel()
        {
            this.SynchronizeProperties(this.InternalSeries);
            return this.InternalSeries;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        /// <param name="series">
        /// The series. 
        /// </param>
        protected override void SynchronizeProperties(OxyPlot.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.LineSeries)series;
            s.Color = this.Color.ToOxyColor();
            s.StrokeThickness = this.StrokeThickness;
            s.LineStyle = this.LineStyle;
            s.MarkerSize = this.MarkerSize;
            s.MarkerStroke = this.MarkerStroke;
            s.MarkerType = this.MarkerType;
            s.MarkerStrokeThickness = this.MarkerStrokeThickness;
            s.Smooth = this.Smooth;
            s.Dashes = this.Dashes;
            s.LineJoin = this.LineJoin;
            s.MarkerFill = this.MarkerFill.ToOxyColor();
            s.MarkerOutline = this.MarkerOutline.ToScreenPointArray();
            s.MinimumSegmentLength = this.MinimumSegmentLength;
            s.LabelFormatString = this.LabelFormatString;
            s.LabelMargin = this.LabelMargin;
            s.LineLegendPosition = this.LineLegendPosition;
        }

        #endregion
    }
}