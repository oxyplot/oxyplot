// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectablePlotElement.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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
//   Represents a series for scatter plots.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides an abstract base class for plot elements that support selection.
    /// </summary>
    public abstract class SelectablePlotElement : PlotElement
    {
        /// <summary>
        /// The selection
        /// </summary>
        private Selection selection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectablePlotElement"/> class.
        /// </summary>
        protected SelectablePlotElement()
        {
            this.Selectable = true;
            this.SelectionMode = SelectionMode.All;
        }

        /// <summary>
        /// Occurs when the selected items is changed.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Gets or sets a value indicating whether this plot element can be selected.
        /// </summary>
        public bool Selectable { get; set; }

        /// <summary>
        /// Gets or sets the selection mode of items in this element.
        /// </summary>
        /// <value>The selection mode.</value>
        /// <remarks>
        /// This is only used by the select/unselect functionality, not by the rendering.
        /// </remarks>
        public SelectionMode SelectionMode { get; set; }

#if X
        // Not yet implemented - must make sure that Selection.Everything is not modified...

        /// <summary>
        /// Determines whether everything is selected.
        /// </summary>
        /// <returns><c>true</c> if everything is selected; otherwise, <c>false</c>.</returns>
        public bool IsEverythingSelected()
        {
            return this.selection.IsEverythingSelected();
        }
#endif

        /// <summary>
        /// Gets the actual selection color.
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

        /// <summary>
        /// Determines whether any part of this element is selected.
        /// </summary>
        /// <returns><c>true</c> if this element is selected; otherwise, <c>false</c>.</returns>
        public bool IsSelected()
        {
            return this.selection != null;
        }

        /// <summary>
        /// Gets the indices of the selected items in this element.
        /// </summary>
        /// <returns>Enumerator of item indices.</returns>
        public IEnumerable<int> GetSelectedItems()
        {
            this.EnsureSelection();
            return this.selection.GetSelectedItems();
        }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        public void ClearSelection()
        {
            this.selection = null;
            this.OnSelectionChanged();
        }

        /// <summary>
        /// Unselects all items in this element.
        /// </summary>
        public void Unselect()
        {
            this.selection = null;
            this.OnSelectionChanged();
        }

        /// <summary>
        /// Determines whether the specified item is selected.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns><c>true</c> if the item is selected; otherwise, <c>false</c>.</returns>
        public bool IsItemSelected(int index)
        {
            if (this.selection == null)
            {
                return false;
            }

            if (index == -1)
            {
                return this.selection.IsEverythingSelected();
            }

            return this.selection.IsItemSelected(index);
        }

        /// <summary>
        /// Selects all items in this element.
        /// </summary>
        public void Select()
        {
            this.selection = Selection.Everything;
            this.OnSelectionChanged();
        }

        /// <summary>
        /// Selects the specified item.
        /// </summary>
        /// <param name="index">The index.</param>
        public void SelectItem(int index)
        {
            if (this.SelectionMode == SelectionMode.All)
            {
                throw new InvalidOperationException("Use the Select() method when using SelectionMode.All");
            }

            this.EnsureSelection();
            if (this.SelectionMode == SelectionMode.Single)
            {
                this.selection.Clear();
            }
            
            this.selection.Select(index);
            this.OnSelectionChanged();
        }

        /// <summary>
        /// Unselects the specified item.
        /// </summary>
        /// <param name="index">The index.</param>
        public void UnselectItem(int index)
        {
            if (this.SelectionMode == SelectionMode.All)
            {
                throw new InvalidOperationException("Use the Unselect() method when using SelectionMode.All");
            }

            this.EnsureSelection();
            this.selection.Unselect(index);
            this.OnSelectionChanged();
        }

        /// <summary>
        /// Gets the selection color if the item is selected, or the specified color if it is not.
        /// </summary>
        /// <param name="originalColor">The unselected color of the element.</param>
        /// <param name="index">The index of the item to check (use -1 for all items).</param>
        /// <returns>
        /// A color.
        /// </returns>
        protected OxyColor GetSelectableColor(OxyColor originalColor, int index = -1)
        {
            // TODO: rename to GetActualColor? (33 usages)
            if (originalColor == null)
            {
                return null;
            }

            if (this.IsItemSelected(index))
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
            // TODO: rename to GetActualFillColor (13 usages)
            return this.GetSelectableColor(originalColor, index);
        }

        /// <summary>
        /// Ensures that the selection field is not null.
        /// </summary>
        private void EnsureSelection()
        {
            if (this.selection == null)
            {
                this.selection = new Selection();
            }
        }

        /// <summary>
        /// Raises the <see cref="SelectionChanged" /> event.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSelectionChanged(EventArgs args = null)
        {
            var e = this.SelectionChanged;
            if (e != null)
            {
                e(this, args);
            }
        }
    }
}