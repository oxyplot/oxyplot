// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAxis.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.CategoryAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.CategoryAxis.
    /// </summary>
    public class CategoryAxis : LinearAxis
    {
        #region Constants and Fields

        /// <summary>
        /// The gap width property.
        /// </summary>
        public static readonly DependencyProperty GapWidthProperty = DependencyProperty.Register(
            "GapWidth", typeof(double), typeof(CategoryAxis), new UIPropertyMetadata(1.0));

        /// <summary>
        /// The is tick centered property.
        /// </summary>
        public static readonly DependencyProperty IsTickCenteredProperty = DependencyProperty.Register(
            "IsTickCentered", typeof(bool), typeof(CategoryAxis), new PropertyMetadata(false, DataChanged));

        /// <summary>
        /// The items source property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(CategoryAxis), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// The label field property.
        /// </summary>
        public static readonly DependencyProperty LabelFieldProperty = DependencyProperty.Register(
            "LabelField", typeof(string), typeof(CategoryAxis), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// The labels property.
        /// </summary>
        public static readonly DependencyProperty LabelsProperty = DependencyProperty.Register(
            "Labels", typeof(IList<string>), typeof(CategoryAxis), new PropertyMetadata(new List<string>(), DataChanged));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="CategoryAxis"/> class.
        /// </summary>
        static CategoryAxis()
        {
            PositionProperty.OverrideMetadata(
                typeof(CategoryAxis), new PropertyMetadata(AxisPosition.Bottom, DataChanged));
            MinimumPaddingProperty.OverrideMetadata(typeof(CategoryAxis), new PropertyMetadata(0.0, DataChanged));
            MaximumPaddingProperty.OverrideMetadata(typeof(CategoryAxis), new PropertyMetadata(0.0, DataChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAxis"/> class.
        /// </summary>
        public CategoryAxis()
        {
            this.internalAxis = new OxyPlot.CategoryAxis();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the gap width.
        /// </summary>
        /// <value>
        /// The width of the gap. 
        /// </value>
        public double GapWidth
        {
            get
            {
                return (double)this.GetValue(GapWidthProperty);
            }

            set
            {
                this.SetValue(GapWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsTickCentered.
        /// </summary>
        public bool IsTickCentered
        {
            get
            {
                return (bool)this.GetValue(IsTickCenteredProperty);
            }

            set
            {
                this.SetValue(IsTickCenteredProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ItemsSource.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)this.GetValue(ItemsSourceProperty);
            }

            set
            {
                this.SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LabelField.
        /// </summary>
        public string LabelField
        {
            get
            {
                return (string)this.GetValue(LabelFieldProperty);
            }

            set
            {
                this.SetValue(LabelFieldProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Labels.
        /// </summary>
        public IList<string> Labels
        {
            get
            {
                return (IList<string>)this.GetValue(LabelsProperty);
            }

            set
            {
                this.SetValue(LabelsProperty, value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// </returns>
        public override OxyPlot.Axis CreateModel()
        {
            this.SynchronizeProperties();
            return this.internalAxis;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = this.internalAxis as OxyPlot.CategoryAxis;
            a.IsTickCentered = this.IsTickCentered;
            a.ItemsSource = this.ItemsSource;
            a.LabelField = this.LabelField;
            a.GapWidth = this.GapWidth;
            if (this.Labels != null)
            {
                a.Labels.Clear();
                foreach (string label in this.Labels)
                {
                    a.Labels.Add(label);
                }
            }
        }

        #endregion
    }
}