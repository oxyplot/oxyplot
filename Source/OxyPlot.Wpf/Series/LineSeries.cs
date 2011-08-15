namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.LineSeries
    /// Only a few properties are yet included.
    /// </summary>
    public class LineSeries : DataPointSeries
    {
        #region Constants and Fields

        public static readonly DependencyProperty DashesProperty = DependencyProperty.Register(
            "Dashes", typeof(double[]), typeof(LineSeries), new UIPropertyMetadata(null, VisualChanged));

        public static readonly DependencyProperty LineJoinProperty = DependencyProperty.Register(
            "LineJoin", typeof(OxyPenLineJoin), typeof(LineSeries), new UIPropertyMetadata(OxyPenLineJoin.Miter));

        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(
            "LineStyle", typeof(LineStyle), typeof(LineSeries), new UIPropertyMetadata(LineStyle.Solid));

        public static readonly DependencyProperty MarkerFillProperty = DependencyProperty.Register(
            "MarkerFill", typeof(Color), typeof(LineSeries), new UIPropertyMetadata(Colors.Transparent));

        public static readonly DependencyProperty MarkerOutlineProperty = DependencyProperty.Register(
            "MarkerOutline", typeof(Point[]), typeof(LineSeries), new UIPropertyMetadata(null));

        public static readonly DependencyProperty MarkerSizeProperty = DependencyProperty.Register(
            "MarkerSize", typeof(double), typeof(LineSeries), new UIPropertyMetadata(3.0));

        public static readonly DependencyProperty MarkerStrokeProperty = DependencyProperty.Register(
            "MarkerStroke", typeof(OxyColor), typeof(LineSeries), new UIPropertyMetadata(null));

        public static readonly DependencyProperty MarkerStrokeThicknessProperty =
            DependencyProperty.Register(
                "MarkerStrokeThickness", typeof(double), typeof(LineSeries), new UIPropertyMetadata(1.0));

        public static readonly DependencyProperty MarkerTypeProperty = DependencyProperty.Register(
            "MarkerType", typeof(MarkerType), typeof(LineSeries), new UIPropertyMetadata(MarkerType.None));

        public static readonly DependencyProperty MinimumSegmentLengthProperty =
            DependencyProperty.Register(
                "MinimumSegmentLength", typeof(double), typeof(LineSeries), new UIPropertyMetadata(2.0));

        public static readonly DependencyProperty SmoothProperty = DependencyProperty.Register(
            "Smooth", typeof(bool), typeof(LineSeries), new UIPropertyMetadata(false));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", typeof(double), typeof(LineSeries), new UIPropertyMetadata(2.0));

        #endregion

        #region Public Properties

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

        public override OxyPlot.ISeries CreateModel()
        {
            var s = new OxyPlot.LineSeries();
            this.SynchronizeProperties(s);
            return s;
        }

        public override void SynchronizeProperties(OxyPlot.ISeries series)
        {
            base.SynchronizeProperties(series);
            var s = series as OxyPlot.LineSeries;
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
        }

        #endregion
    }
}