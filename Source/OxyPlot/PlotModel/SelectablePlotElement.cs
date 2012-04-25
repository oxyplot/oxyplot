// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectablePlotElement.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Represents a plot element that supports selection.
    /// </summary>
    [Serializable]
    public abstract class SelectablePlotElement : PlotElement
    {
        #region Constants and Fields

        /// <summary>
        ///   The is selected.
        /// </summary>
        private bool isSelected;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectablePlotElement"/> class. 
        /// </summary>
        protected SelectablePlotElement()
        {
            this.Selectable = true;
            this.IsSelected = false;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Occurs when the IsSelected property is changed.
        /// </summary>
        public event EventHandler Selected;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the index of the selected item (or -1 if all items are selected).
        /// </summary>
        /// <value>
        /// The index of the selected.
        /// </value>
        public int SelectedIndex { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this plot element is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                if (value == this.isSelected)
                {
                    return;
                }

                this.isSelected = value;
                this.OnIsSelectedChanged();
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this plot element can be selected.
        /// </summary>
        public bool Selectable { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the actual selection color.
        /// </summary>
        /// <value> The actual selection color. </value>
        protected OxyColor ActualSelectedColor
        {
            get
            {
                if (this.PlotModel != null)
                {
                    return this.PlotModel.SelectionColor ?? PlotModel.DefaultSelectionColor;
                }

                return PlotModel.DefaultSelectionColor;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the selection color it the element is selected, or the specified color if it is not.
        /// </summary>
        /// <param name="originalColor">The unselected color of the element.</param>
        /// <param name="index">The index of the item to check (use -1 for all items).</param>
        /// <returns>
        /// A color.
        /// </returns>
        protected OxyColor GetSelectableColor(OxyColor originalColor, int index = -1)
        {
            if (originalColor == null)
            {
                return null;
            }

            if (this.IsSelected && (index == -1 || index == this.SelectedIndex))
            {
                return this.ActualSelectedColor;
            }

            return originalColor;
        }

        /// <summary>
        /// Gets the selection fill color it the element is selected, or the specified fill color if it is not.
        /// </summary>
        /// <param name="originalColor">The unselected fill color of the element.</param>
        /// <param name="index">The index of the item to check (use -1 for all items).</param>
        /// <returns>
        /// A fill color.
        /// </returns>
        protected OxyColor GetSelectableFillColor(OxyColor originalColor, int index = -1)
        {
            return this.GetSelectableColor(originalColor, index);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the Selected event.
        /// </summary>
        protected void OnIsSelectedChanged()
        {
            var eh = this.Selected;
            if (eh != null)
            {
                eh(this, new EventArgs());
            }
        }

        #endregion
    }
}