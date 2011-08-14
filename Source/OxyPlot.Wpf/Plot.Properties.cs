// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Plot.Properties.cs" company="">
//   
// </copyright>
// <summary>
//   Partial class for the WPF Plot control.
//   This file contains dependency properties used for defining the plot in XAML.
//   These properties are only used when Model is null.
//   In this case an internal PlotModel is created and the dependency properties are copied from the control to the PlotModel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Partial class for the WPF Plot control. 
    /// This file contains dependency properties used for defining the plot in XAML.
    /// These properties are only used when Model is null.
    /// In this case an internal PlotModel is created and the dependency properties are copied from the control to the PlotModel.
    /// </summary>
    public partial class Plot
    {
        #region Constants and Fields

        /// <summary>
        /// The box color property.
        /// </summary>
        public static readonly DependencyProperty BoxColorProperty = DependencyProperty.Register(
            "BoxColor", typeof(Color), typeof(Plot), new UIPropertyMetadata(Colors.Black));

        /// <summary>
        /// The box thickness property.
        /// </summary>
        public static readonly DependencyProperty BoxThicknessProperty = DependencyProperty.Register(
            "BoxThickness", typeof(double), typeof(Plot), new UIPropertyMetadata(1.0));

        /// <summary>
        /// The legend placement property.
        /// </summary>
        public static readonly DependencyProperty LegendPlacementProperty =
            DependencyProperty.Register(
                "LegendPlacement", typeof(LegendPlacement), typeof(Plot), new UIPropertyMetadata(LegendPlacement.Inside));

        /// <summary>
        /// The legend position property.
        /// </summary>
        public static readonly DependencyProperty LegendPositionProperty = DependencyProperty.Register(
            "LegendPosition", typeof(LegendPosition), typeof(Plot), new UIPropertyMetadata(LegendPosition.RightTop));

        /// <summary>
        /// The plot margins property.
        /// </summary>
        public static readonly DependencyProperty PlotMarginsProperty = DependencyProperty.Register(
            "PlotMargins", typeof(Thickness?), typeof(Plot), new UIPropertyMetadata(null));

        /// <summary>
        /// The subtitle property.
        /// </summary>
        public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
            "Subtitle", typeof(string), typeof(Plot), new UIPropertyMetadata(null));

        /// <summary>
        /// The title property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(Plot), new UIPropertyMetadata(null, VisualChanged));

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
        private readonly ObservableCollection<DataSeries> series;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the annotations.
        /// </summary>
        /// <value>The annotations.</value>
        public ObservableCollection<Annotation> Annotations
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
        public ObservableCollection<Axis> Axes
        {
            get
            {
                return this.axes;
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
        /// Gets or sets the plot margins.
        /// </summary>
        /// <value>The plot margins.</value>
        public Thickness? PlotMargins
        {
            get
            {
                return (Thickness?)this.GetValue(PlotMarginsProperty);
            }

            set
            {
                this.SetValue(PlotMarginsProperty, value);
            }
        }

        /// <summary>
        /// Gets the series.
        /// </summary>
        /// <value>The series.</value>
        public ObservableCollection<DataSeries> Series
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

        #endregion

        #region Methods

        /// <summary>
        /// Synchronize properties between the WPF control and the internal PlotModel (only if Model is undefined).
        /// </summary>
        private void SynchronizeProperties()
        {
            if (this.internalModel != null)
            {
                this.internalModel.Title = this.Title;
                this.internalModel.Subtitle = this.Subtitle;

                this.internalModel.BoxColor = this.BoxColor.ToOxyColor();
                this.internalModel.BoxThickness = this.BoxThickness;

                this.internalModel.LegendPosition = this.LegendPosition;
                this.internalModel.LegendPlacement = this.LegendPlacement;

                if (this.PlotMargins.HasValue)
                {
                    this.internalModel.PlotMargins = new OxyThickness(
                        this.PlotMargins.Value.Left, 
                        this.PlotMargins.Value.Top, 
                        this.PlotMargins.Value.Right, 
                        this.PlotMargins.Value.Bottom);
                }
            }
        }

        #endregion
    }
}