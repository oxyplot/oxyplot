namespace OxyPlot.Wpf
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;

    public class CategoryAxis : LinearAxis
    {
        #region Constants and Fields

        public static readonly DependencyProperty IsTickCenteredProperty = DependencyProperty.Register(
            "IsTickCentered", typeof(bool), typeof(CategoryAxis), new FrameworkPropertyMetadata(false, DataChanged));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(CategoryAxis), new FrameworkPropertyMetadata(null, DataChanged));

        public static readonly DependencyProperty LabelFieldProperty = DependencyProperty.Register(
            "LabelField", typeof(string), typeof(CategoryAxis), new FrameworkPropertyMetadata(null, DataChanged));

        public static readonly DependencyProperty LabelsProperty = DependencyProperty.Register(
            "Labels", typeof(IList<string>), typeof(CategoryAxis), new FrameworkPropertyMetadata(new List<string>(), DataChanged));

        #endregion

        #region Constructors and Destructors

        static CategoryAxis()
        {
            PositionProperty.OverrideMetadata(
                typeof(CategoryAxis), new FrameworkPropertyMetadata(AxisPosition.Bottom, DataChanged));
            MinimumPaddingProperty.OverrideMetadata(
                typeof(CategoryAxis), new FrameworkPropertyMetadata(0.0, DataChanged));
            TickStyleProperty.OverrideMetadata(
                typeof(CategoryAxis), new FrameworkPropertyMetadata(TickStyle.Outside, DataChanged));
        }

        public CategoryAxis()
        {
            this.Axis = new OxyPlot.CategoryAxis();
        }

        #endregion

        #region Public Properties

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

        public override OxyPlot.IAxis CreateModel()
        {
            this.SynchronizeProperties();
            return this.Axis;
        }

        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = this.Axis as OxyPlot.CategoryAxis;
            a.IsTickCentered = this.IsTickCentered;
            a.ItemsSource = this.ItemsSource;
            a.LabelField = this.LabelField;
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