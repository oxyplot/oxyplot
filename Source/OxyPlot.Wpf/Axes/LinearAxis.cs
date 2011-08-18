// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearAxis.cs" company="">
//   
// </copyright>
// <summary>
//   The linear axis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    /// <summary>
    /// The linear axis.
    /// </summary>
    public class LinearAxis : AxisBase
    {
        #region Constants and Fields

        /// <summary>
        /// The format as fractions property.
        /// </summary>
        public static readonly DependencyProperty FormatAsFractionsProperty =
            DependencyProperty.Register(
                "FormatAsFractions", typeof(bool), typeof(LinearAxis), new UIPropertyMetadata(false));

        /// <summary>
        /// The fraction unit property.
        /// </summary>
        public static readonly DependencyProperty FractionUnitProperty = DependencyProperty.Register(
            "FractionUnit", typeof(double), typeof(LinearAxis), new UIPropertyMetadata(1.0));

        /// <summary>
        /// The fraction unit symbol property.
        /// </summary>
        public static readonly DependencyProperty FractionUnitSymbolProperty =
            DependencyProperty.Register(
                "FractionUnitSymbol", typeof(string), typeof(LinearAxis), new UIPropertyMetadata(null));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearAxis"/> class.
        /// </summary>
        public LinearAxis()
        {
            this.Axis = new OxyPlot.LinearAxis();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether FormatAsFractions.
        /// </summary>
        public bool FormatAsFractions
        {
            get
            {
                return (bool)this.GetValue(FormatAsFractionsProperty);
            }

            set
            {
                this.SetValue(FormatAsFractionsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FractionUnit.
        /// </summary>
        public double FractionUnit
        {
            get
            {
                return (double)this.GetValue(FractionUnitProperty);
            }

            set
            {
                this.SetValue(FractionUnitProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FractionUnitSymbol.
        /// </summary>
        public string FractionUnitSymbol
        {
            get
            {
                return (string)this.GetValue(FractionUnitSymbolProperty);
            }

            set
            {
                this.SetValue(FractionUnitSymbolProperty, value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns></returns>
        public override OxyPlot.IAxis CreateModel()
        {
            this.SynchronizeProperties();
            return this.Axis;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = this.Axis as OxyPlot.LinearAxis;
            a.FormatAsFractions = this.FormatAsFractions;
            a.FractionUnit = this.FractionUnit;
            a.FractionUnitSymbol = this.FractionUnitSymbol;
        }

        #endregion
    }
}